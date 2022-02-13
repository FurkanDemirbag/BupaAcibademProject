using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class PolicyResultModel : ApiResult
    {
        public Policy Policy { get; set; }

        public PolicyResultModel() : base()
        {
        }
    }
}
