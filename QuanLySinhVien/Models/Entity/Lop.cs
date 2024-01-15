using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class Lop
    {
        public Lop()
        {
            SinhViens = new HashSet<SinhVien>();
        }

        public string MaLop { get; set; } = null!;
        public string TenLop { get; set; } = null!;
        public string MaNghanh { get; set; } = null!;

        public virtual Nghanh MaNghanhNavigation { get; set; } = null!;
        public virtual ICollection<SinhVien> SinhViens { get; set; }
    }
}
