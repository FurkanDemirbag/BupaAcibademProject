using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class City : Entity
    {
        [Display(Name = "Country Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int CountryId { get; set; }

        [Display(Name = "Şehir")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        public Country Country { get; set; }
        public List<District> Districts { get; set; }
        public List<Insurer> Insurers { get; set; }
        public List<Customer> Customers { get; set; }

        public City() : base()
        {
        }
    }
}
