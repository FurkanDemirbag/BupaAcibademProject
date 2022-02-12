using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.FrontEnd
{
    public class OfferModel
    {
        [Display(Name = "Teklif No")]
        [StringLength(11, MinimumLength = 6)]
        public string OfferNumber { get; set; }

        [Display(Name = "Teminatlar")]
        public List<ProductModel> ProductModels { get; set; }
    }
}
