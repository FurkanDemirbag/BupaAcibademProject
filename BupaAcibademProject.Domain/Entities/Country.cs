using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class Country : Entity
    {
        [Display(Name = "Ülke")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        public List<City> Cities { get; set; }
        public List<Insurer> Insurers { get; set; }
        public List<Customer> Customers { get; set; }

        public Country(): base()
        {
        }
    }
}
