using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class Nationality : Entity
    {
        [Display(Name = "Uyruk")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        public List<Insurer> Insurers { get; set; }
        public List<Customer> Customers { get; set; }

        public Nationality() : base()
        {
        }
    }
}
