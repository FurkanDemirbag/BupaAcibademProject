using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class PolicyDetail : Entity
    {

        [Display(Name = "Policy Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int PolicyId { get; set; }

        [Display(Name = "Taksit Sayısı")]
        [StringLength(40, MinimumLength = 40)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Installment { get; set; }

        [Display(Name = "Toplam Fiyat")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Ödeme Tarihi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Poliçe Başlangıç Tarihi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public DateTime PolicyStartDate { get; set; }

        [Display(Name = "Poliçe Bitiş Tarihi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public DateTime PolicyEndDate { get; set; }

        public Policy Policy { get; set; }

        public PolicyDetail() : base()
        {
        }
    }
}
