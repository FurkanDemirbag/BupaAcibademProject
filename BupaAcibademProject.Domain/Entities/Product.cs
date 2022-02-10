using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class Product : Entity
    {
        [Display(Name = "Tedavi Adı")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal Price { get; set; }

        public List<Offer> Offers { get; set; }

        public Product(): base()
        {
        }
    }
}
