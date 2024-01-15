using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models.Entity
{
    public partial class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Role { get; set; }
    }
}
