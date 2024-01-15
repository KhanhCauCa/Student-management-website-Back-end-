using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLySinhVien.Models.DTO;
using QuanLySinhVien.Models.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using X.PagedList;

namespace QuanLySinhVien.Controllers
{
    public class SinhVienController : Controller
    {
        private readonly QuanLySinhVienContext _context;
        private readonly IWebHostEnvironment _env;
        public SinhVienController(QuanLySinhVienContext quanLySinhVienContext, IWebHostEnvironment env)
        {
            _context = quanLySinhVienContext;
            _env = env;
        }

        public async Task<IActionResult> Index(string? searchQuery, int? page)
        {
            var data = await _context.SinhViens.Include(c => c.MaLopNavigation).ToListAsync();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                data = data.Where(c => c.TenSv.Contains(searchQuery) || c.MaSv.Contains(searchQuery)).ToList();
            }
            int pageSize = 4; // Số bản ghi trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại
            var pagedList = data.OrderByDescending(p => p.MaSv).ToPagedList(pageNumber, pageSize); // Tạo trang dữ liệu

            ViewBag.SearchQuery = searchQuery; // Lưu lại searchQuery để truyền cho view     
            ViewBag.PageNumber = pageNumber;

            return View(pagedList);
        }
        public IActionResult Create()
        {
            ViewData["Lop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
            return View();


        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Lop"] = new SelectList(_context.Lops, "MaLop", "TenLop");

            var sinhvien = await _context.SinhViens.Where(c => c.MaSv == id).FirstOrDefaultAsync();


            if (sinhvien == null) return RedirectToAction("Error", "Home");

            SinhVienDTO sinhvienDto = new SinhVienDTO();
            sinhvienDto.MaSv = sinhvien.MaSv;
            sinhvienDto.TenSv = sinhvien.TenSv;
            sinhvienDto.Email = sinhvien.Email;
            sinhvienDto.DiaChi = sinhvien.DiaChi;
            sinhvienDto.NgaySinh = sinhvien.NgaySinh;
            sinhvienDto.Sdt = sinhvien.Sdt;
            sinhvienDto.MaLop = sinhvien.MaLop;
            sinhvienDto.AnhSv = sinhvien.AnhSv;
            sinhvienDto.Cccd = sinhvien.Cccd;

            return View(sinhvienDto);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SinhVienDTO sinhVienDTO, IFormFile image)
        {

            var existingSinhVien = await _context.SinhViens.FindAsync(sinhVienDTO.MaSv);

            if (existingSinhVien == null)
            {
                ViewData["Lop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
                // Xử lý lỗi nếu ModelState không hợp lệ
                return View(sinhVienDTO);
            }

            if (image != null && image.Length > 0)
            {
                // Xử lý tệp ảnh và cập nhật thông tin sinh viên
                // Code tương tự như phần Create
                string extension = Path.GetExtension(image.FileName);

                var uploads = Path.Combine(_env.WebRootPath, "img/anhsv");
                var fileName = existingSinhVien.MaSv + extension;
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                existingSinhVien.AnhSv = fileName;
            }

            // Cập nhật thông tin sinh viên (trừ phần ảnh)
            existingSinhVien.TenSv = sinhVienDTO.TenSv;
            existingSinhVien.Cccd = sinhVienDTO.Cccd;
            existingSinhVien.DiaChi = sinhVienDTO.DiaChi;
            existingSinhVien.Email = sinhVienDTO.Email;
            existingSinhVien.NgaySinh = sinhVienDTO.NgaySinh;
            existingSinhVien.Sdt = sinhVienDTO.Sdt;
            existingSinhVien.MaLop = sinhVienDTO.MaLop;

            _context.Update(existingSinhVien);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");


            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SinhVienDTO sinhVienDTO, IFormFile image)
        {

            if (image != null && image.Length > 0)
            {


                SinhVien sinhVien = new SinhVien();
                var maxid = Convert.ToInt32(await GetMaxMaSVAsync()) + 1;
                sinhVien.MaSv = "PH" + maxid;



                // Thực hiện xử lý tệp ảnh ở đây, ví dụ: lưu vào thư mục
                var uploads = Path.Combine(_env.WebRootPath, "img/anhsv");

                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }
                string extension = Path.GetExtension(image.FileName);
                var fileName = sinhVien.MaSv + extension;
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                sinhVien.TenSv = sinhVienDTO.TenSv;
                sinhVien.Cccd = sinhVienDTO.Cccd;
                sinhVien.DiaChi = sinhVienDTO.DiaChi;
                sinhVien.Email = sinhVienDTO.Email;
                sinhVien.NgaySinh = sinhVienDTO.NgaySinh;
                sinhVien.Sdt = sinhVienDTO.Sdt;
                sinhVien.AnhSv = fileName;
                sinhVien.MaLop = sinhVienDTO.MaLop;

                _context.SinhViens.Add(sinhVien);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý lỗi nếu không có ảnh được chọn
                ViewData["Lop"] = new SelectList(_context.Lops, "MaLop", "TenLop");
                ModelState.AddModelError("", "Vui lòng chọn ảnh.");
            }


            return View();
        }
        public async Task<string> GetMaxMaSVAsync()
        {
            var sinhViens = await _context.SinhViens
                .Where(sv => sv.MaSv != null && sv.MaSv.StartsWith("PH"))
                .ToListAsync();

            var maxMaSv = sinhViens
                .Select(sv => sv.MaSv.Substring(2))
                .Where(substring => int.TryParse(substring, out int parsedValue))
                .Select(parsedValue => parsedValue);

            var result = maxMaSv.Any() ? maxMaSv.Max().ToString() : "0";

            return result;
        }
        [HttpPost]
        public JsonResult Delete(string masv)
        {
            if (!string.IsNullOrEmpty(masv))
            {
                var sinhVien = _context.SinhViens.Where(c => c.MaSv == masv).FirstOrDefault();
                if (sinhVien == null)
                {
                    return Json(0);
                }
                _context.SinhViens.Remove(sinhVien);
                _context.SaveChanges();
                return Json(1);
            }
            return Json(0); // thêm thành công
        }

    }
}
