using System.ComponentModel.DataAnnotations;

namespace QuanLySinhVien.Models.DTO
{
    public class UserDto
    {
        [Required(ErrorMessage = "Username không được để trống ")]
        [StringLength(50)]

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password không được để trống")]
        [StringLength(50)]
        public string? Password { get; set; }

    }
}
