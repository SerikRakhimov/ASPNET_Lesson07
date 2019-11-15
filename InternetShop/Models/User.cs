using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [MaxLength(10, ErrorMessage = "Максимум 10 символов")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [MaxLength(8, ErrorMessage = "Максимум 8 символов")]
        public string Password { get; set; }
    }

}