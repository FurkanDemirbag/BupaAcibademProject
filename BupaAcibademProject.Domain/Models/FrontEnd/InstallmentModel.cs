using BupaAcibademProject.Domain.Entities;
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

        [Display(Name = "Ödemeler")]
        public List<Installment> Installments { get; set; }
    }
}
