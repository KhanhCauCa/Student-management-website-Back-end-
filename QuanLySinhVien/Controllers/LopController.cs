using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models.DTO;
using QuanLySinhVien.Models.Entity;
using X.PagedList;

namespace QuanLySinhVien.Controllers
{
    public class LopController : Controller
    {
        private readonly QuanLySinhVienContext _context;

        public LopController(QuanLySinhVienContext context)
        {
            _context = context;
        }

        // GET: Lop
        public async Task<IActionResult> Index(string? searchQuery, int? page)
        {
            var data = await _context.Lops.AsNoTracking().Include(n => n.MaNghanhNavigation).ToListAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(c => c.TenLop.Contains(searchQuery) || c.MaLop.Contains(searchQuery) || c.MaNghanhNavigation.TenNghanh.Contains(searchQuery)  ).ToList();
            }
            int pageSize = 15; // Số bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedList = data.OrderByDescending(p => p.MaLop).ToPagedList(pageNumber, pageSize); // Tạo trang dữ liệu

            ViewBag.SearchQuery = searchQuery; // Lưu lại searchQuery để truyền cho view     
            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

       

        // GET: Lop/Create
        public IActionResult Create()
        {
            ViewData["Nghanh"] = new SelectList(_context.Nghanhs, "MaNghanh", "TenNghanh");
            return View();
        }

        // POST: Lop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLop,TenLop,MaNghanh")] LopDTO lopdto)
        {
            ViewData["Nghanh"] = new SelectList(_context.Nghanhs, "MaNghanh", "TenNghanh", lopdto.MaNghanh);
            if (ModelState.IsValid)
            {
                if (LopExists(lopdto.MaLop))
                {

                    ModelState.AddModelError("", "Mã lớp đã tồn tại");
                    return View(lopdto);
                }
                Lop  lop = new Lop();
                lop.MaLop  = lopdto.MaLop;
                lop.TenLop = lopdto.TenLop;
                lop.MaNghanh = lopdto.MaNghanh;
                _context.Add(lop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(lopdto);
        }

        // GET: Lop/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Lops == null)
            {
                return NotFound();
            }

            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return NotFound();
            }
            LopDTO lopdto = new LopDTO();
            lopdto.MaLop = lop.MaLop;
            lopdto.TenLop = lop.TenLop;
            lopdto.MaNghanh = lop.MaNghanh;
            ViewData["Nghanh"] = new SelectList(_context.Nghanhs, "MaNghanh", "TenNghanh", lop.MaNghanh);
            return View(lopdto);
        }

        // POST: Lop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaLop,TenLop,MaNghanh")] LopDTO lopdto)
        {
            if (id != lopdto.MaLop)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Lop lop = new Lop();
                    lop.MaLop = lopdto.MaLop;
                    lop.TenLop = lopdto.TenLop;
                    lop.MaNghanh = lopdto.MaNghanh;
                    _context.Update(lop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LopExists(lopdto.MaLop))
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
            ViewData["Nghanh"] = new SelectList(_context.Nghanhs, "MaNghanh", "TenNghanh", lopdto.MaNghanh);
            return View(lopdto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var checkcount = await _context.SinhViens.Where(c => c.MaLop == id).ToListAsync();
            if (checkcount.Count() > 0)
            {
                var deletemessA = new MessDTO
                {
                    Status = 3,
                    Mess = "Không thể xóa lớp này vì đang có sinh viên trong lớp đó !"
                };
                return Json(deletemessA, new System.Text.Json.JsonSerializerOptions());
            }

            var lop = await _context.Lops.FindAsync(id);
            if (lop != null)
            {
                _context.Lops.Remove(lop);
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

        private bool LopExists(string id)
        {
          return (_context.Lops?.Any(e => e.MaLop == id)).GetValueOrDefault();
        }
    }
}
