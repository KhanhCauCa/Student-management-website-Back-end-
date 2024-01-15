using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class Nghanh
    {
        public Nghanh()
        {
            Lops = new HashSet<Lop>();
        }

        public string MaNghanh { get; set; } = null!;
        public string TenNghanh { get; set; } = null!;
        public string MaKhoa { get; set; } = null!;

        public virtual Khoa MaKhoaNavigation { get; set; } = null!;
        public virtual ICollection<Lop> Lops { get; set; }
    }
}
