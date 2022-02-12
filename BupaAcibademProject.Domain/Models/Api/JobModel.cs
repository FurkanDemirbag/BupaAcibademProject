using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class JobModel : ApiResult
    {
        public List<Job> Jobs { get; set; }

        public JobModel()
        {
            Jobs = new List<Job>();
        }
    }
}
