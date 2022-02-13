using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class InstallmentResultModel : ApiResult
    {
        public List<Installment> Installments { get; set; }

        public InstallmentResultModel()
        {
            Installments = new List<Installment>();
        }
    }
}
