using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using QuanLySinhVien.Models.DTO;
using QuanLySinhVien.Models.Entity;

namespace QuanLySinhVien.Controllers
{
    public class DiemController : Controller
    {
        private readonly QuanLySinhVienContext _context;
        public DiemController(QuanLySinhVienContext quanLySinhVienContext)
        {
            _context = quanLySinhVienContext;
        }
        public IActionResult Index()
        {
            var sinhvienItems = _context.SinhViens
            .Select(sv => new SelectListItem
            {
                Value = sv.MaSv,
                Text = $"Mã sinh viên: {sv.MaSv} - Tên sinh viên : {sv.TenSv}"
            })
            .ToList();

            var monHocItems = _context.MonHocs
            .Select(sv => new SelectListItem
            {
                Value = sv.MaMh,
                Text = $"Mã môn học: {sv.MaMh} - Tên môn học : {sv.TenMh}"
            })
            .ToList();
            ViewData["SinhVien"] = new SelectList(sinhvienItems, "Value", "Text");
            ViewData["MonHoc"] = new SelectList(monHocItems, "Value", "Text");
            return View();
        }
        [HttpGet]
        public JsonResult GetListDiem()
        {
            var lstDiem = _context.Diems
            .Join(_context.SinhViens,
                diem => diem.MaSv,
                sinhVien => sinhVien.MaSv,
                (diem, sinhVien) => new { Diem = diem, SinhVien = sinhVien })
            .Join(_context.MonHocs,
                diemSinhVien => diemSinhVien.Diem.MaMonHoc,
                monHoc => monHoc.MaMh,
                (diemSinhVien, monHoc) => new
                {
                    MaDiem  = diemSinhVien.Diem.MaDiem,
                    MaSv = diemSinhVien.Diem.MaSv,
                    TenSv = diemSinhVien.SinhVien.TenSv,
                    TenMonHoc = monHoc.TenMh,
                    Diem = diemSinhVien.Diem.DiemThi
                })
            .OrderByDescending(c => c.MaSv)
            .ToList();

            return Json(lstDiem, new System.Text.Json.JsonSerializerOptions());
        }
        [HttpGet]
        public JsonResult GetDiem(string madiem)
        {
           
            var lstDiem = _context.Diems
            .Join(_context.SinhViens,
                diem => diem.MaSv,
                sinhVien => sinhVien.MaSv,
                (diem, sinhVien) => new { Diem = diem, SinhVien = sinhVien })
            .Join(_context.MonHocs,
                diemSinhVien => diemSinhVien.Diem.MaMonHoc,
                monHoc => monHoc.MaMh,
                (diemSinhVien, monHoc) => new
                {
                    MaDiem = diemSinhVien.Diem.MaDiem,
                    MaSv = diemSinhVien.Diem.MaSv,
                    TenSv = diemSinhVien.SinhVien.TenSv,
                    TenMonHoc = monHoc.TenMh,
                    Diem = diemSinhVien.Diem.DiemThi
                })
            .OrderByDescending(c => c.MaSv)
            .ToList();

            var diem = lstDiem.Where(c => c.MaDiem == madiem).FirstOrDefault();
            return Json(diem);
        }
        [HttpPost]
        public JsonResult AddDiem(DiemDTO diemDTO)
        {
            if (ModelState.IsValid)
            {
                if (DiemExists(diemDTO.MaSv + diemDTO.MaMonHoc))
                {
                    return Json(3); //Đã tồn tại điểm môn học này
                }
                Diem diem = new Diem();
                diem.MaDiem = diemDTO.MaSv + diemDTO.MaMonHoc;
                diem.MaMonHoc = diemDTO.MaMonHoc;
                diem.DiemThi = diemDTO.DiemThi; 
                diem.MaSv = diemDTO.MaSv; 
                _context.Diems.Add(diem);
                _context.SaveChanges(); 
            }
            else
            {
                return Json(0); // thêm thất bại
            } 

            return Json(1); // thêm thành công
        }
        [HttpPost]
        public JsonResult UpdateDiem(string madiem, int diemthi)
        {
            if (!string.IsNullOrEmpty(madiem))
            {
                var diem = _context.Diems.Where(c => c.MaDiem == madiem).FirstOrDefault();
                if (diem == null)
                {
                    return Json(0);
                }
                else
                {
                    diem.DiemThi = diemthi;
                    _context.Diems.Update(diem);
                    _context.SaveChanges();
                    return Json(1); // thêm thành công

                }
                
            }
            return Json(0);
        }
        [HttpPost]
        public JsonResult DeleteDiem(string madiem)
        {
            if (!string.IsNullOrEmpty(madiem))
            {
                var diem = _context.Diems.Where(c => c.MaDiem == madiem).FirstOrDefault();      
                if (diem == null)
                {
                    return Json(0);
                }
                _context.Diems.Remove(diem);
                _context.SaveChanges();
                return Json(1);
            }
            return Json(0); // thêm thành công
        }

        private bool DiemExists(string id)
        {
            return (_context.Diems?.Any(e => e.MaDiem == id)).GetValueOrDefault();
        }
    }
}
