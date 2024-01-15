using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class MonHoc
    {
        public MonHoc()
        {
            Diems = new HashSet<Diem>();
        }

        public string MaMh { get; set; } = null!;
        public string TenMh { get; set; } = null!;
        public string Ki { get; set; } = null!;
        public int SoTinChi { get; set; }

        public virtual ICollection<Diem> Diems { get; set; }
    }
}
