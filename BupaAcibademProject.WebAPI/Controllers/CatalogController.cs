using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BupaAcibademProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController: ControllerBase
    {
        private ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<ActionResult<CountryModel>> GetCountries()
        {
            var countryResult = await _catalogService.GetCountries();
            if (countryResult.HasError)
            {
                return new CountryModel()
                {
                    ErrorCode = countryResult.Errors.First().Code,
                    ErrorMessage = countryResult.Errors.First().Message
                };
            }

            return new CountryModel()
            {
                Success = true,
                Countries = countryResult.Data
            };
        }

        [HttpGet]
        [Route("GetNationalities")]
        public async Task<ActionResult<NationalityModel>> GetNationalities()
        {
            var nationalityResult = await _catalogService.GetNationalities();
            if (nationalityResult.HasError)
            {
                return new NationalityModel()
                {
                    ErrorCode = nationalityResult.Errors.First().Code,
                    ErrorMessage = nationalityResult.Errors.First().Message
                };
            }

            return new NationalityModel()
            {
                Success = true,
                Nationalities = nationalityResult.Data
            };
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<ActionResult<CityModel>> GetCities(int countryId)
        {
            var cityResult = await _catalogService.GetCities(countryId);
            if (cityResult.HasError)
            {
                return new CityModel()
                {
                    ErrorCode = cityResult.Errors.First().Code,
                    ErrorMessage = cityResult.Errors.First().Message
                };
            }

            return new CityModel()
            {
                Success = true,
                Cities = cityResult.Data
            };
        }

        [HttpGet]
        [Route("GetDistricts")]
        public async Task<ActionResult<DistrictModel>> GetDistricts(int cityId)
        {
            var districtResult = await _catalogService.GetDistricts(cityId);
            if (districtResult.HasError)
            {
                return new DistrictModel()
                {
                    ErrorCode = districtResult.Errors.First().Code,
                    ErrorMessage = districtResult.Errors.First().Message
                };
            }

            return new DistrictModel()
            {
                Success = true,
                Districts = districtResult.Data
            };
        }

        [HttpGet]
        [Route("GetJobs")]
        public async Task<ActionResult<JobModel>> GetJobs()
        {
            var jobResult = await _catalogService.GetJobs();
            if (jobResult.HasError)
            {
                return new JobModel()
                {
                    ErrorCode = jobResult.Errors.First().Code,
                    ErrorMessage = jobResult.Errors.First().Message
                };
            }

            return new JobModel()
            {
                Success = true,
                Jobs = jobResult.Data
            };
        }
    }
}
