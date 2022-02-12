using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models.FrontEnd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class OfferResultModel : ApiResult
    {
        public OfferModel OfferModel { get; set; }

        public OfferResultModel() : base()
        {
        }
    }
}
