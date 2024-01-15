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
    public class KhoaController : Controller
    {
        private readonly QuanLySinhVienContext _context;

        public KhoaController(QuanLySinhVienContext context)
        {
            _context = context;
        }

        // GET: Khoa
        public async Task<IActionResult> Index(string? searchQuery, int? page)
        {
            var data = await _context.Khoas.ToListAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(c => c.TenKhoa.Contains(searchQuery) ||  c.MaKhoa.Contains(searchQuery)).ToList();
            }
            int pageSize = 15; // Số bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedList = data.OrderByDescending(p => p.MaKhoa).ToPagedList(pageNumber, pageSize); // Tạo trang dữ liệu

            ViewBag.SearchQuery = searchQuery; // Lưu lại searchQuery để truyền cho view     
            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }

        // GET: Khoa/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Khoa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaKhoa,TenKhoa,NamThanhLap")] KhoaDto khoadto)
        {
           
            if (ModelState.IsValid)
            {
                if (KhoaExists(khoadto.MaKhoa))
                {

                    ModelState.AddModelError("", "Mã khoa đã tồn tại");
                    return View(khoadto);
                }                
                Khoa khoa = new Khoa();
                khoa.MaKhoa = khoadto.MaKhoa;
                khoa.TenKhoa = khoadto.TenKhoa;
                khoa.NamThanhLap = khoadto.NamThanhLap;
                _context.Add(khoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khoadto);
        }

        // GET: Khoa/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Khoas == null)
            {
                return NotFound();
            }

            var khoa = await _context.Khoas.FindAsync(id);
            if (khoa == null)
            {
                return NotFound();
            }
            KhoaDto khoadto = new KhoaDto();
            khoadto.MaKhoa = khoa.MaKhoa;
            khoadto.TenKhoa = khoa.TenKhoa;
            khoadto.NamThanhLap = khoa.NamThanhLap;
            return View(khoadto);
        }

        // POST: Khoa/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaKhoa,TenKhoa,NamThanhLap")]KhoaDto khoadto)
        {
            if (id != khoadto.MaKhoa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Khoa khoa = new Khoa();
                    khoa.MaKhoa = khoadto.MaKhoa;
                    khoa.TenKhoa = khoadto.TenKhoa;
                    khoa.NamThanhLap = khoadto.NamThanhLap;
                    _context.Update(khoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KhoaExists(khoadto.MaKhoa))
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
            return View(khoadto);
        }


        // POST: Khoa/Delete/5
        [HttpPost]  
        public async Task<IActionResult> Delete(string id)
        {
             var checkcount  =  await _context.Nghanhs.Where(c=>c.MaKhoa == id).ToListAsync();
            if (checkcount.Count() > 0)
            {
                var deletemessA = new MessDTO
                {
                    Status = 3,
                    Mess = "Không thể xóa khoa này vì nó đang được sử dụng"
                };
                return Json(deletemessA, new System.Text.Json.JsonSerializerOptions());
            }

            var khoa = await _context.Khoas.FindAsync(id);
            if (khoa != null)
            {
                _context.Khoas.Remove(khoa);
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

        private bool KhoaExists(string id)
        {
          return (_context.Khoas?.Any(e => e.MaKhoa == id)).GetValueOrDefault();
        }
    }
}
