using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class KhoaDto
    {
        [Required(ErrorMessage = "Mã khoa không được để trống ")]
        [StringLength(50)]
        public string MaKhoa { get; set; }
        [Required(ErrorMessage = "Tên không được để trống ")]
        [StringLength(50)]
        public string TenKhoa { get; set; }
        [Required(ErrorMessage = "Năm thành lập được để trống ")]
        public string NamThanhLap { get; set; }
    }
}
