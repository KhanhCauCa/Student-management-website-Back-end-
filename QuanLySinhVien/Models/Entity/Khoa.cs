using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class Khoa
    {
        public Khoa()
        {
            Nghanhs = new HashSet<Nghanh>();
        }

        public string MaKhoa { get; set; } = null!;
        public string TenKhoa { get; set; } = null!;
        public string NamThanhLap { get; set; } = null!;

        public virtual ICollection<Nghanh> Nghanhs { get; set; }
    }
}
