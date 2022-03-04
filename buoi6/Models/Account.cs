using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace buoi6.Models
{
    public class Account
    {
        public int Id { get; set; }
        [DisplayName("Tên đăng nhập")]
        public string Username { get; set; }
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("SDT")]
        public string Phone { get; set; }
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }
        [DisplayName("Họ và tên")]
        public string FullName { get; set; }
        [DisplayName("Là Admin")]
        public bool IsAdmin { get; set; }
        [DisplayName("Ảnh")]

        public string Avatar { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }


        [DisplayName("Trang thái")]
        public bool TrangThai { get; set; }
        [ForeignKey("AccountId")]
        public List<Invoice> Invoices { get; set; }
        [ForeignKey("AccountId")]
        public List<Cart> Carts { get; set; }
    }
}
