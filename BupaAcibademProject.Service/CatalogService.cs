using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BupaAcibademProject.DataAccessLayer;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace BupaAcibademProject.Service
{
    public class CatalogService : ServiceBase, ICatalogService
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IDAL _dal;

        public CatalogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userAccessor = _serviceProvider.GetService<IUserAccessor>();
            _dal = _serviceProvider.GetService<IDAL>();
        }

        public async Task<Result<List<Country>>> GetCountries()
        {
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllCountries", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var countryList = new List<Country>();

                    while (dr.Read())
                    {
                        var country = new Country
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        countryList.Add(country);
                    }

                    return new Result<List<Country>>()
                    {
                        Data = countryList.ToList()
                    };
                }

                return new Result<List<Country>>();
            }
            catch (Exception ex)
            {
                return new Result<List<Country>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<List<Nationality>>> GetNationalities()
        {
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllNationalities", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var nationalityList = new List<Nationality>();

                    while (dr.Read())
                    {
                        var nationality = new Nationality
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        nationalityList.Add(nationality);
                    }

                    return new Result<List<Nationality>>()
                    {
                        Data = nationalityList.ToList()
                    };
                }

                return new Result<List<Nationality>>();
            }
            catch (Exception ex)
            {
                return new Result<List<Nationality>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<List<City>>> GetCities(int countryId = 0)
        {
            try
            {
                _dal.AddInputParameter(new SqlParameter("@CountryId", countryId));

                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllCities", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var cityList = new List<City>();

                    while (dr.Read())
                    {
                        var city = new City
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            CountryId = Convert.ToInt32(dr["CountryId"]),
                            Name = dr["Name"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        cityList.Add(city);
                    }

                    return new Result<List<City>>()
                    {
                        Data = cityList.ToList()
                    };
                }

                return new Result<List<City>>();
            }
            catch (Exception ex)
            {
                return new Result<List<City>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<List<District>>> GetDistricts(int cityId = 0)
        {
            try
            {
                _dal.AddInputParameter(new SqlParameter("@CityId", cityId));

                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllDistricts", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var districtList = new List<District>();

                    while (dr.Read())
                    {
                        var district = new District
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            CityId = Convert.ToInt32(dr["CityId"]),
                            Name = dr["Name"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        districtList.Add(district);
                    }

                    return new Result<List<District>>()
                    {
                        Data = districtList.ToList()
                    };
                }

                return new Result<List<District>>();
            }
            catch (Exception ex)
            {
                return new Result<List<District>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<List<Job>>> GetJobs()
        {
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllJobs", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var jobList = new List<Job>();

                    while (dr.Read())
                    {
                        var job = new Job
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        jobList.Add(job);
                    }

                    return new Result<List<Job>>()
                    {
                        Data = jobList.ToList()
                    };
                }

                return new Result<List<Job>>();
            }
            catch (Exception ex)
            {
                return new Result<List<Job>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
    }
}
