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
    public class NghanhController : Controller
    {
        private readonly QuanLySinhVienContext _context;

        public NghanhController(QuanLySinhVienContext context)
        {
            _context = context;
        }

        // GET: Nghanh
    
        public async Task<IActionResult> Index(string? searchQuery, int? page)
        {
            var data = await _context.Nghanhs.AsNoTracking().Include(n => n.MaKhoaNavigation).ToListAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(c => c.TenNghanh.Contains(searchQuery) || c.MaNghanh.Contains(searchQuery) || c.MaKhoaNavigation.TenKhoa.Contains(searchQuery) || c.MaKhoaNavigation.MaKhoa.Contains(searchQuery)).ToList();
            }
            int pageSize = 15; // Số bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedList = data.OrderByDescending(p => p.MaKhoa).ToPagedList(pageNumber, pageSize); // Tạo trang dữ liệu

            ViewBag.SearchQuery = searchQuery; // Lưu lại searchQuery để truyền cho view     
            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        // GET: Nghanh/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Nghanhs == null)
            {
                return NotFound();
            }

            var nghanh = await _context.Nghanhs
                .Include(n => n.MaKhoaNavigation)
                .FirstOrDefaultAsync(m => m.MaNghanh == id);
            if (nghanh == null)
            {
                return NotFound();
            }

            return View(nghanh);
        }

        // GET: Nghanh/Create
        public IActionResult Create()
        {
            ViewData["Khoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa");
            return View();
        }

        // POST: Nghanh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNghanh,TenNghanh,MaKhoa")] NghanhDTO nghanhdto)
        {
            ViewData["Khoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", nghanhdto.MaKhoa);
            if (ModelState.IsValid)
            {
                if (NghanhExists(nghanhdto.MaNghanh))
                {

                    ModelState.AddModelError("", "Mã khoa đã tồn tại");
                    return View(nghanhdto);
                }
                Nghanh nghanh = new Nghanh();
                nghanh.MaNghanh = nghanhdto.MaNghanh;
                nghanh.TenNghanh = nghanhdto.TenNghanh;
                nghanh.MaKhoa = nghanhdto.MaKhoa;
                _context.Add(nghanh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(nghanhdto);
        }

        // GET: Nghanh/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Nghanhs == null)
            {
                return NotFound();
            }

            var nghanh = await _context.Nghanhs.FindAsync(id);
            if (nghanh == null)
            {
                return NotFound();
            }
            ViewData["Khoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", nghanh.MaKhoa);
            NghanhDTO nghanhDTO = new NghanhDTO();
            nghanhDTO.MaNghanh = nghanh.MaNghanh;
            nghanhDTO.TenNghanh = nghanh.TenNghanh;
    
            return View(nghanhDTO);
        }

        // POST: Nghanh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaNghanh,TenNghanh,MaKhoa")] NghanhDTO nghanhDto)
        {
            if (id != nghanhDto.MaNghanh)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Nghanh nghanh = new Nghanh();
                    nghanh.MaNghanh = nghanhDto.MaNghanh;
                    nghanh.TenNghanh = nghanhDto.TenNghanh;
                    nghanh.MaKhoa = nghanhDto.MaKhoa;

                    _context.Update(nghanh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NghanhExists(nghanhDto.MaNghanh))
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
            ViewData["Khoa"] = new SelectList(_context.Khoas, "MaKhoa", "TenKhoa", nghanhDto.MaKhoa);
            return View(nghanhDto);
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var checkcount = await _context.Lops.Where(c => c.MaNghanh == id).ToListAsync();
            if (checkcount.Count() > 0)
            {
                var deletemessA = new MessDTO
                {
                    Status = 3,
                    Mess = "Không thể xóa nghành này vì nó đang được sử dụng"
                };
                return Json(deletemessA, new System.Text.Json.JsonSerializerOptions());
            }

            var nghanh = await _context.Nghanhs.FindAsync(id);
            if (nghanh != null)
            {
                _context.Nghanhs.Remove(nghanh);
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

        private bool NghanhExists(string id)
        {
          return (_context.Nghanhs?.Any(e => e.MaNghanh == id)).GetValueOrDefault();
        }
    }
}
