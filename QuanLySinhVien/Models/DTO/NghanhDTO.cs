using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class NghanhDTO
    {
        [Required(ErrorMessage = "Mã nghành không được để trống ")]
        [StringLength(50)]
        public string MaNghanh { get; set; }

        [Required(ErrorMessage = "Tên nghành không được để trống ")]
        [StringLength(50)]
        public string TenNghanh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn khoa  ")]
        [StringLength(50)]
        public string MaKhoa { get; set; }
    }
}
