using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models.DTO;
using QuanLySinhVien.Models.Entity;
using X.PagedList;

namespace QuanLySinhVien.Controllers
{
    public class MonHocController : Controller
    {
        private readonly QuanLySinhVienContext _context;

        public MonHocController(QuanLySinhVienContext context)
        {
            _context = context;
        }

        // GET: MonHocs
        public async Task<IActionResult> Index(string? searchQuery, int? page)
        {
            var data = await _context.MonHocs.ToListAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(c => c.TenMh.Contains(searchQuery) || c.MaMh.Contains(searchQuery)).ToList();
            }
            int pageSize = 15; // Số bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedList = data.OrderByDescending(p => p.MaMh).ToPagedList(pageNumber, pageSize); // Tạo trang dữ liệu

            ViewBag.SearchQuery = searchQuery; // Lưu lại searchQuery để truyền cho view     
            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        // GET: MonHocs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.MonHocs == null)
            {
                return NotFound();
            }

            var monHoc = await _context.MonHocs
                .FirstOrDefaultAsync(m => m.MaMh == id);
            if (monHoc == null)
            {
                return NotFound();
            }

            return View(monHoc);
        }

        // GET: MonHocs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MonHocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMh,TenMh,Ki,SoTinChi")] MonHocDTO monHocdto)
        {
            if (ModelState.IsValid)
            {
                if (MonHocExists(monHocdto.MaMh))
                {
                    
                    ModelState.AddModelError("", "Mã Môn học đã tồn tại");
                    return View(monHocdto);
                }
                MonHoc monHoc = new MonHoc();
                monHoc.MaMh = monHocdto.MaMh;
                monHoc.TenMh = monHocdto.TenMh;
                monHoc.SoTinChi = monHocdto.SoTinChi;
                monHoc.Ki = monHocdto.Ki;
                _context.Add(monHoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monHocdto);
        }

        // GET: MonHocs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.MonHocs == null)
            {
                return NotFound();
            }

            var monHoc = await _context.MonHocs.FindAsync(id);
            if (monHoc == null)
            {
                return NotFound();
            }

            MonHocDTO monHocDTO = new MonHocDTO();
            monHocDTO.MaMh = monHoc.MaMh;
            monHocDTO.TenMh = monHoc.TenMh;
            monHocDTO.SoTinChi = monHoc.SoTinChi;
            monHocDTO.Ki = monHoc.Ki;

            return View(monHocDTO);
        }

        // POST: MonHocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaMh,TenMh,Ki,SoTinChi")] MonHocDTO monHocDTO)
        {
            if (id != monHocDTO.MaMh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    MonHoc monHoc = new MonHoc();
                    monHoc.MaMh = monHocDTO.MaMh;
                    monHoc.TenMh = monHocDTO.TenMh;
                    monHoc.SoTinChi = monHocDTO.SoTinChi;
                    monHoc.Ki = monHocDTO.Ki;
                    _context.Update(monHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonHocExists(monHocDTO.MaMh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(monHocDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var checkcount = await _context.Diems.Where(c => c.MaMonHoc == id).ToListAsync();
            if (checkcount.Count() > 0)
            {
                var deletemessA = new MessDTO
                {
                    Status = 3,
                    Mess = "Không thể xóa môn này vì nó đang được sử dụng"
                };
                return Json(deletemessA, new System.Text.Json.JsonSerializerOptions());
            }

            var monhoc = await _context.MonHocs.FindAsync(id);
            if (monhoc != null)
            {
                _context.MonHocs.Remove(monhoc);
                await _context.SaveChangesAsync();
                var deletemessB = new MessDTO
                {
                    Status = 1,
                    Mess = "Xóa thành công"
                };
                return Json(deletemessB, new System.Text.Json.JsonSerializerOptions());
            }
            var deletemessC = new MessDTO
            {
                Status = 0,
                Mess = "Xóa thất Bại"
            };
            return Json(deletemessC, new System.Text.Json.JsonSerializerOptions());

        }

        private bool MonHocExists(string id)
        {
          return (_context.MonHocs?.Any(e => e.MaMh == id)).GetValueOrDefault();
        }
    }
}
