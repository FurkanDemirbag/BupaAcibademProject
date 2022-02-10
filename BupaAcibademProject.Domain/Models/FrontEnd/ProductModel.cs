using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.FrontEnd
{
    public class ProductModel
    {
        [Display(Name = "Offer Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int OfferId { get; set; }

        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Display(Name = "Teminat")]
        [StringLength(11, MinimumLength = 11)]
        public string ProductName { get; set; }

        [Display(Name = "Toplam Fiyat")]
        public decimal TotalPrice { get; set; }
    }
}
