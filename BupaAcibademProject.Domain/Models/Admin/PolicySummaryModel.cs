using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Admin
{
    public class PolicySummaryModel
    {
        [Display(Name = "PolicyId")]
        public int PolicyId { get; set; }

        [Display(Name = "Sigorta Ettiren Ad")]
        public string InsurerName { get; set; }

        [Display(Name = "Sigorta Ettiren Soyad")]
        public string InsurerSurname { get; set; }

        [Display(Name = "Yakınlık Derecesi")]
        public ProximityType ProximityType { get; set; }

        [Display(Name = "TC")]
        public string TC { get; set; }

        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Doğum Tarihi")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Teklif No")]
        public string OfferNumber { get; set; }

        [Display(Name = "Poliçe No")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Seçilen Paket")]
        public string ProductName { get; set; }

        [Display(Name = "Taksit Sayısı")]
        public int Installment { get; set; }

        [Display(Name = "Prim")]
        public decimal Price { get; set; }

        [Display(Name = "Poliçe Başlangıç")]
        public DateTime PolicyStartDate { get; set; }

        [Display(Name = "Poliçe Bitiş")]
        public DateTime PolicyEndDate { get; set; }

        [Display(Name = "Tamamlandı mı?")]
        public bool PolicyIsDone { get; set; }
    }
}
