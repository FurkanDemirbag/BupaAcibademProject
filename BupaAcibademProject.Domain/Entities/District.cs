using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class District : Entity
    {
        [Display(Name = "City Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int CityId { get; set; }

        [Display(Name = "İlçe")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        public City City { get; set; }
        public List<Insurer> Insurers { get; set; }
        public List<Customer> Customers { get; set; }

        public District() : base()
        {
        }
    }
}
