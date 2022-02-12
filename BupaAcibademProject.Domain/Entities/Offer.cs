using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class Offer : Entity
    {
        [Display(Name = "Şirket Adı")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string CompanyName { get; set; }

        [Display(Name = "Teklif Numarası")]
        [StringLength(11, MinimumLength = 6)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string OfferNumber { get; set; }

        [Display(Name = "Product Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int ProductId { get; set; }

        [Display(Name = "Customer Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int CustomerId { get; set; }


        [Display(Name = "Toplam Fiyat")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal TotalPrice { get; set; }


        public Product Product { get; set; }
        public Customer Customer { get; set; }
        public List<Policy> Policies { get; set; }

        public Offer(): base()
        {
        }
    }
}
