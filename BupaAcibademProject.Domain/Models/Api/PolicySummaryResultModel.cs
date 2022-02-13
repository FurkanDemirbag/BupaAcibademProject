using BupaAcibademProject.Domain.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class PolicySummaryResultModel : ApiResult
    {
        public List<PolicySummaryModel> PolicySummaryModels { get; set; }

        public PolicySummaryResultModel()
        {
            PolicySummaryModels = new List<PolicySummaryModel>();
        }
    }
}
