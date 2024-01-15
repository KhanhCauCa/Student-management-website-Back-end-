using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class LopDTO
    {
        [Required(ErrorMessage = "Mã lớp không được để trống ")]
        [StringLength(50)]
        public string MaLop { get; set; }

        [Required(ErrorMessage = "Tên lớp không được để trống ")]
        public string TenLop { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn nghành ")]
        public string MaNghanh { get; set; } 
    }
}
