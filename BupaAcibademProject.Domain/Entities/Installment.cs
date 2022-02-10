using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class Installment : Entity
    {
        [Display(Name = "Taksit Adı")]
        [MaxLength(400)]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public string Name { get; set; }

        [Display(Name = "Taksit Sayısı")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int InstallmentCount { get; set; }

        [Display(Name = "Oran")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public decimal Rate { get; set; }


        public List<Policy> Policies { get; set; }

        public Installment(): base()
        {
        }
    }
}
