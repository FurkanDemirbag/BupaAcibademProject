using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class ContinuePolicyModel : ApiResult
    {
        public int PolicyId { get; set; }
        public int InstallmentId { get; set; }
    }
}
