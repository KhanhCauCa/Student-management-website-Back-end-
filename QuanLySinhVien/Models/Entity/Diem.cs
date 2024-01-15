using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class Diem
    {
        public string MaDiem { get; set; } = null!;
        public string MaMonHoc { get; set; } = null!;
        public string MaSv { get; set; } = null!;
        public int? DiemThi { get; set; }

        public virtual MonHoc MaMonHocNavigation { get; set; } = null!;
        public virtual SinhVien MaSvNavigation { get; set; } = null!;
    }
}
