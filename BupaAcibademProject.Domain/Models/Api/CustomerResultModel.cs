using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class CustomerResultModel : ApiResult
    {
        public List<Customer> Customers { get; set; }

        public CustomerResultModel() : base()
        {
        }
    }
}
