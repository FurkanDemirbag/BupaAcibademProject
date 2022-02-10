using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models.Api
{
    public class CountryModel : ApiResult
    {
        public List<Country> Countries { get; set; }

        public CountryModel()
        {
            Countries = new List<Country>();
        }
    }
}
