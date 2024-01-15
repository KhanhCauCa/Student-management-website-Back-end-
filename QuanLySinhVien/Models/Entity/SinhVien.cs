using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class SinhVien
    {
        public SinhVien()
        {
            Diems = new HashSet<Diem>();
        }

        public string MaSv { get; set; } = null!;
        public string Cccd { get; set; } = null!;
        public string AnhSv { get; set; } = null!;
        public string TenSv { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public DateTime NgaySinh { get; set; }
        public string Sdt { get; set; } = null!;
        public string MaLop { get; set; } = null!;

        public virtual Lop MaLopNavigation { get; set; } = null!;
        public virtual ICollection<Diem> Diems { get; set; }
    }
}
