using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class MonHocDTO
    {
        [Required(ErrorMessage = "Mã môn học không được để trống ")]
        [StringLength(50)]
        public string MaMh { get; set; }

        [Required(ErrorMessage = "Tên môn học không được để trống ")]
        [StringLength(50)]
        public string TenMh { get; set; }

        [Required(ErrorMessage = "Kì không được để trống ")]
        public string Ki { get; set; }

        [Required(ErrorMessage = "Số tín chỉ không được để trống ")]
        public int SoTinChi { get; set; }
    }
}
