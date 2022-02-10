using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.FrontEnd
{
    public class PaymentModel
    {
        [Display(Name = "InsurerId")]
        [Required(ErrorMessage = "{0} boş olamaz.")]
        public int InsurerId { get; set; }

        [Display(Name = "Cvc")]
        [Required(ErrorMessage = "{0} boş olamaz.")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "{0} {1} ile {2} karakter uzunluğunda bir değer olmabilir")]
        public string CVC { get; set; }

        [Display(Name = "Kart sahibi")]
        [Required(ErrorMessage = "{0} boş olamaz.")]
        [StringLength(400, ErrorMessage = "{0} en fazla {1} karakter uzunluğunda bir değer olmabilir")]
        public string CardHolderName { get; set; }

        [Display(Name = "Kart numarası")]
        [CreditCard(ErrorMessage = "Kredi kartı numarası geçerli değil")]
        [Required(ErrorMessage = "{0} boş olamaz.")]
        public string CardNumber { get; set; }

        [Display(Name = "Son kullanim Tarihi")]
        [Required(ErrorMessage = "{0} boş olamaz.")]
        public DateTime ExpirationDate { get; set; }
    }
}
