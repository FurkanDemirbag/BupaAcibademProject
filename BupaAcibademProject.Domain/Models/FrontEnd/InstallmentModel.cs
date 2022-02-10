using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.FrontEnd
{
    public class InstallmentModel
    {
        [Display(Name = "Teklif No")]
        [StringLength(11, MinimumLength = 11)]
        public string OfferNumber { get; set; }

        [Display(Name = "Ödeme Planı")]
        public int InstallmentId { get; set; }

        [Display(Name = "Ödemeler")]
        public Dictionary<string, decimal> Installments { get; set; }

        [Display(Name = "Toplam Prim")]
        public decimal TotalPrice { get; set; }

    }
}
