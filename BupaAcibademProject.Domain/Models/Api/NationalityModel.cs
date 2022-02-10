using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class NationalityModel : ApiResult
    {
        public List<Nationality> Nationalities { get; set; }

        public NationalityModel()
        {
            Nationalities = new List<Nationality>();
        }
    }
}
