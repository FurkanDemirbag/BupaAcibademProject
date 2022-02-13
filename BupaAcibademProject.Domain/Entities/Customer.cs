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
    public class Customer : Entity
    {
        [Display(Name = "Insurer Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int InsurerId { get; set; }

        [Display(Name = "Yakınlık Derecesi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public ProximityType ProximityType { get; set; }

        [Display(Name = "TCKNo")]
        [StringLength(11, MinimumLength = 11)]
        public string TCKNo { get; set; }

        [Display(Name = "ForeignTCKNo")]
        [StringLength(20, MinimumLength = 20)]
        public string ForeignTCKNo { get; set; }

        [Display(Name = "PassportNo")]
        [StringLength(7, MinimumLength = 7)]
        public string PassportNo { get; set; }

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

        [Display(Name = "Boy")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal Height { get; set; }

        [Display(Name = "Kilo")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal Weight { get; set; }

        [Display(Name = "Telefon Numarası")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Display(Name = "EMail")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [StringLength(400)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Job Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int JobId { get; set; }

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


        public Insurer Insurer { get; set; }
        public Job Job { get; set; }
        public Country Country { get; set; }
        public Nationality Nationality { get; set; }
        public City City { get; set; }
        public District District { get; set; }

        public List<Offer> Offers { get; set; }

        public Customer() : base()
        {
            Offers = new List<Offer>();
        }
    }
}
