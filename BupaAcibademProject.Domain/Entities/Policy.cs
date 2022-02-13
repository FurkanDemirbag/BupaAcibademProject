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
    public class Policy : Entity
    {

        [Display(Name = "Insurer Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int InsurerId { get; set; }

        [Display(Name = "Offer Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string OfferIds { get; set; }

        [Display(Name = "Installment Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int InstallmentId { get; set; }

        [Display(Name = "Toplam Fiyat")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Poliçe tamamlandı mı?")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public bool PolicyIsDone { get; set; }


        public Insurer Insurer { get; set; }
        public Installment Installment { get; set; }

        public List<PolicyDetail> PolicyDetails { get; set; }
        public List<PaymentLog> PaymentLogs { get; set; }

        public Policy() : base()
        {
        }
    }
}
