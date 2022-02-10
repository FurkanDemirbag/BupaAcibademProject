using BupaAcibademProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    public class EntityLog : Entity
    {
        [Display(Name = "Tablo")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        [MaxLength(400, ErrorMessage = "{0} en fazla {1} karakter uzunluğunda olabilir")]
        public string TableName { get; set; }

        [Display(Name = "Tablo Id")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public int TableId { get; set; }

        [Display(Name = "Log Tipi")]
        [Required(ErrorMessage = "{0} boş olamaz")]
        public LogType LogType { get; set; }

        [Display(Name = "Kullanıcı")]
        public int? InsurerId { get; set; }

        [Display(Name = "Değişiklik")]
        public string Changes { get; set; }

        [Display(Name = "Ip Adresi")]
        [MaxLength(400, ErrorMessage = "{0} en fazla {1} karakter uzunluğunda olabilir")]
        public string ClientIP { get; set; }

        public Insurer Insurer { get; set; }

        public EntityLog() : base()
        {
        }
    }
}
