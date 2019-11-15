using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InternetShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        public int CategoryId { get; set; }
    }
}