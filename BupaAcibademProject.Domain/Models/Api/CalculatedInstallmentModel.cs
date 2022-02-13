using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class CalculatedInstallmentModel : ApiResult
    {
        public decimal TotalPrice { get; set; }
        public List<CalculatedModel> Installments { get; set; }

        public CalculatedInstallmentModel()
        {
            Installments = new List<CalculatedModel>();
        }
    }

    public class CalculatedModel
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
