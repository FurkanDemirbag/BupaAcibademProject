using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class CityModel : ApiResult
    {
        public List<City> Cities { get; set; }

        public CityModel()
        {
            Cities = new List<City>();
        }
    }
}
