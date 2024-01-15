using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class SinhVienDTO
    {


        [Required(ErrorMessage = "CCCD không được để trống")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "CCCD phải có 12 số")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "CCCD phải chứa 12 chữ số.")]
        public string Cccd { get; set; }

        [Required(ErrorMessage = "Tên sinh viên không được để trống")]
        [StringLength(20, ErrorMessage = "Tên sinh viên không được quá 20 ký tự")]
        public string TenSv { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ")]
        [StringLength(255, ErrorMessage = "Độ dài tối đa cho trường Email là 255 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [StringLength(200, ErrorMessage = "Độ dài tối đa cho trường DiaChi là 200 ký tự")]
        public string DiaChi { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime NgaySinh { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(10, ErrorMessage = "Số điện thoại không được quá 10 ký tự")]
        public string Sdt { get; set; }

        [Required(ErrorMessage = "Lớp không được để trống")]
        [StringLength(50, ErrorMessage = "Lớp không được quá 50 ký")]

        public string MaLop { get; set; }
        public string? AnhSv { get; set; }
        public string? MaSv { get; set; }
    }
}
