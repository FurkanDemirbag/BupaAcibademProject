using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.FrontEnd
{
    public class InsurerModel
    {
        [Display(Name = "Müşteri Tipi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public CustomerType CustomerType { get; set; }

        [Display(Name = "TCKNo")]
        [StringLength(11, MinimumLength = 11)]
        public string TCKNo { get; set; }

        [Display(Name = "ForeignTCKNo")]
        [StringLength(20, MinimumLength = 20)]
        public string ForeignTCKNo { get; set; }

        [Display(Name = "PassportNo")]
        [StringLength(7, MinimumLength = 7)]
        public string PassportNo { get; set; }

        [Display(Name = "Şirket Adı")]
        [StringLength(400)]
        public string CompanyName { get; set; }

        [Display(Name = "Vergi Dairesi")]
        [StringLength(400)]
        public string VatOffice { get; set; }

        [Display(Name = "Vergi Numarası")]
        [StringLength(10, MinimumLength = 10)]
        public string VatNumber { get; set; }

        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        public string Surname { get; set; }

        [Display(Name = "Cinsiyet")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public Gender Gender { get; set; }

        [Display(Name = "Doğum Tarihi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Display(Name = "EMail")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Country Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int CountryId { get; set; }

        [Display(Name = "Nationality Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int NationalityId { get; set; }

        [Display(Name = "City Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int CityId { get; set; }

        [Display(Name = "District Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int DistrictId { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        public string Address { get; set; }

        [Display(Name = "Sigorta ettiren aynı zamanda sigortalı mı?")]
        public bool InsurerIsInsured { get; set; }
    }
}
