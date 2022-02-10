using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models;
using BupaAcibademProject.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface ICatalogService : IScopedService, IServiceBase
    {
        Task<Result<List<Country>>> GetCountries();
        Task<Result<List<Nationality>>> GetNationalities();
        Task<Result<List<City>>> GetCities(int countryId = 0);
        Task<Result<List<District>>> GetDistricts(int cityId = 0);
    }
}
