using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Entities
{
    [Serializable]
    public class ErrorLog : Entity
    {
        [MaxLength(400)]
        public string ClientIP { get; set; }

        [MaxLength(4000)]
        public string RequestLink { get; set; }

        public string ErrorMessage { get; set; }

        public int? UserId { get; set; }

        public Insurer Insurer { get; set; }

        public ErrorLog() : base()
        {
        }
    }
}
