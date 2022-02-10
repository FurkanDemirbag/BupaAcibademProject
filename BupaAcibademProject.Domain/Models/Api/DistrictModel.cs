using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class DistrictModel : ApiResult
    {
        public List<District> Districts { get; set; }

        public DistrictModel()
        {
            Districts = new List<District>();
        }
    }
}
