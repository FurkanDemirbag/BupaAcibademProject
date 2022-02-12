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
using BupaAcibademProject.Domain.Models.FrontEnd;
using BupaAcibademProject.Domain.Enums;

namespace BupaAcibademProject.Service
{
    public class PolicyService : ServiceBase, IPolicyService
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IDAL _dal;
        private readonly IValidationService _validation;

        public PolicyService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userAccessor = _serviceProvider.GetService<IUserAccessor>();
            _dal = _serviceProvider.GetService<IDAL>();
            _validation = _serviceProvider.GetService<IValidationService>();
        }

        public async Task<Result<Insurer>> GetInsurer(int id)
        {
            if (id > 0)
            {
                try
                {
                    var insurerResult = default(Insurer);

                    _dal.AddInputParameter(new SqlParameter("@Id", id));

                    var dr = _dal.ExecuteDrSelectQuery("sp_GetInsurerById", CommandType.StoredProcedure);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var insurer = new Insurer
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                CountryId = Convert.ToInt32(dr["CountryId"]),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                DistrictId = Convert.ToInt32(dr["DistrictId"]),
                                NationalityId = !string.IsNullOrEmpty(dr["NationalityId"].ToString()) ? Convert.ToInt32(dr["NationalityId"]) : 0,
                                Name = dr["Name"].ToString(),
                                Surname = dr["Surname"].ToString(),
                                Email = dr["Email"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                Address = dr["Address"].ToString(),
                                DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                                InsurerIsInsured = Convert.ToBoolean(dr["InsurerIsInsured"]),
                                Gender = Convert.ToInt32(dr["Gender"]) == 1 ? Gender.MALE : Gender.FEMALE,
                                CustomerType = Convert.ToInt32(dr["CustomerType"]) == 1 ? CustomerType.PERSONAL : CustomerType.CORPORATE,
                                TCKNo = dr["TCKNo"].ToString(),
                                ForeignTCKNo = dr["ForeignTCKNo"].ToString(),
                                PassportNo = dr["PassportNo"].ToString(),
                                CompanyName = dr["CompanyName"].ToString(),
                                VatOffice = dr["VatOffice"].ToString(),
                                VatNumber = dr["VatNumber"].ToString()
                            };

                            insurerResult = insurer;
                        }
                    }

                    return new Result<Insurer>() { Data = insurerResult };
                }
                catch (Exception ex)
                {
                    return new Result<Insurer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Insurer>();
        }
        public async Task<Result<Insurer>> SaveInsurer(InsurerModel model)
        {
            if (model != null)
            {
                try
                {
                    var validationResult = _validation.Validate(model);
                    if (validationResult.HasError)
                    {
                        return new Result<Insurer>(validationResult);
                    }

                    _dal.AddInputParameter(
                        new SqlParameter("@CountryId", model.CountryId),
                        new SqlParameter("@NationalityId", model.NationalityId > 0 ? model.NationalityId : null),
                        new SqlParameter("@CityId", model.CityId),
                        new SqlParameter("@DistrictId", model.DistrictId),
                        new SqlParameter("@Name", model.Name),
                        new SqlParameter("@Surname", model.Surname),
                        new SqlParameter("@Address", model.Address),
                        new SqlParameter("@CustomerType", (int)model.CustomerType),
                        new SqlParameter("@CompanyName", model.CompanyName),
                        new SqlParameter("@DateOfBirth", model.DateOfBirth),
                        new SqlParameter("@Email", model.Email),
                        new SqlParameter("@ForeignTCKNo", model.ForeignTCKNo),
                        new SqlParameter("@Gender", (int)model.Gender),
                        new SqlParameter("@InsurerIsInsured", model.InsurerIsInsured),
                        new SqlParameter("@PassportNo", model.PassportNo),
                        new SqlParameter("@PhoneNumber", model.PhoneNumber),
                        new SqlParameter("@TCKNo", model.TCKNo),
                        new SqlParameter("@VatNumber", model.VatNumber),
                        new SqlParameter("@VatOffice", model.VatOffice)
                        );

                    var insertResult = _dal.ExecuteQuery("sp_InsurerSave", CommandType.StoredProcedure);

                    var insurer = default(Insurer);

                    _dal.AddInputParameter(
                        new SqlParameter("@CountryId", model.CountryId),
                        new SqlParameter("@CityId", model.CityId),
                        new SqlParameter("@DistrictId", model.DistrictId),
                        new SqlParameter("@Address", model.Address),
                        new SqlParameter("@CustomerType", (int)model.CustomerType),
                        new SqlParameter("@Email", model.Email),
                        new SqlParameter("@InsurerIsInsured", model.InsurerIsInsured),
                        new SqlParameter("@PhoneNumber", model.PhoneNumber)
                        );

                    var dr = _dal.ExecuteDrSelectQuery("sp_GetInsurerByParameters", CommandType.StoredProcedure);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var insurerResult = new Insurer
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                InsurerIsInsured = Convert.ToBoolean(dr["InsurerIsInsured"])
                            };

                            insurer = insurerResult;
                        }
                    }

                    return new Result<Insurer>() { Data = insurer };
                }
                catch (Exception ex)
                {
                    return new Result<Insurer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Insurer>();
        }

        public async Task<Result<Customer>> GetCustomer(int id)
        {
            if (id > 0)
            {
                try
                {
                    var customerResult = default(Customer);

                    _dal.AddInputParameter(new SqlParameter("@Id", id));

                    var dr = _dal.ExecuteDrSelectQuery("sp_GetCustomerById", CommandType.StoredProcedure);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var customer = new Customer
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                InsurerId = Convert.ToInt32(dr["InsurerId"]),
                                CountryId = Convert.ToInt32(dr["CountryId"]),
                                CityId = Convert.ToInt32(dr["CityId"]),
                                JobId = Convert.ToInt32(dr["JobId"]),
                                DistrictId = Convert.ToInt32(dr["DistrictId"]),
                                NationalityId = !string.IsNullOrEmpty(dr["NationalityId"].ToString()) ? Convert.ToInt32(dr["NationalityId"]) : 0,
                                Name = dr["Name"].ToString(),
                                Surname = dr["Surname"].ToString(),
                                Email = dr["Email"].ToString(),
                                PhoneNumber = dr["PhoneNumber"].ToString(),
                                Address = dr["Address"].ToString(),
                                DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                                Gender = Convert.ToInt32(dr["Gender"]) == 1 ? Gender.MALE : Gender.FEMALE,
                                ProximityType = (ProximityType)Convert.ToInt32(dr["ProximityType"]),
                                TCKNo = dr["TCKNo"].ToString(),
                                ForeignTCKNo = dr["ForeignTCKNo"].ToString(),
                                PassportNo = dr["PassportNo"].ToString(),
                                Height = Convert.ToDecimal(dr["Height"]),
                                Weight = Convert.ToDecimal(dr["Weight"])
                            };

                            customerResult = customer;
                        }
                    }

                    return new Result<Customer>() { Data = customerResult };
                }
                catch (Exception ex)
                {
                    return new Result<Customer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Customer>();
        }
        public async Task<Result<List<Customer>>> SaveCustomers(List<CustomerModel> customers)
        {
            if (customers != null)
            {
                var customerList = new List<Customer>();

                foreach (var model in customers)
                {
                    try
                    {
                        var validationResult = _validation.Validate(model);
                        if (validationResult.HasError)
                        {
                            return new Result<List<Customer>>(validationResult);
                        }

                        _dal.AddInputParameter(
                            new SqlParameter("@InsurerId", model.InsurerId),
                            new SqlParameter("@CountryId", model.CountryId),
                            new SqlParameter("@NationalityId", model.NationalityId > 0 ? model.NationalityId : null),
                            new SqlParameter("@CityId", model.CityId),
                            new SqlParameter("@DistrictId", model.DistrictId),
                            new SqlParameter("@JobId", model.JobId),
                            new SqlParameter("@Name", model.Name),
                            new SqlParameter("@Surname", model.Surname),
                            new SqlParameter("@Address", model.Address),
                            new SqlParameter("@ProximityType", (int)model.ProximityType),
                            new SqlParameter("@DateOfBirth", model.DateOfBirth),
                            new SqlParameter("@Email", model.Email),
                            new SqlParameter("@ForeignTCKNo", model.ForeignTCKNo),
                            new SqlParameter("@Gender", (int)model.Gender),
                            new SqlParameter("@Height", model.Height),
                            new SqlParameter("@Weight", model.Weight),
                            new SqlParameter("@PassportNo", model.PassportNo),
                            new SqlParameter("@PhoneNumber", model.PhoneNumber),
                            new SqlParameter("@TCKNo", model.TCKNo)
                            );

                        var insertResult = _dal.ExecuteQuery("sp_CustomerSave", CommandType.StoredProcedure);

                        _dal.AddInputParameter(
                            new SqlParameter("@InsurerId", model.InsurerId),
                            new SqlParameter("@CountryId", model.CountryId),
                            new SqlParameter("@CityId", model.CityId),
                            new SqlParameter("@DistrictId", model.DistrictId),
                            new SqlParameter("@Address", model.Address),
                            new SqlParameter("@ProximityType", (int)model.ProximityType),
                            new SqlParameter("@Email", model.Email),
                            new SqlParameter("@TCKNo", model.TCKNo),
                            new SqlParameter("@PhoneNumber", model.PhoneNumber)
                            );

                        var dr = _dal.ExecuteDrSelectQuery("sp_GetCustomerByParameters", CommandType.StoredProcedure);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var customer = new Customer
                                {
                                    Id = Convert.ToInt32(dr["Id"])
                                };

                                customerList.Add(customer);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return new Result<List<Customer>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                    }
                }

                return new Result<List<Customer>>() { Data = customerList };
            }

            return new Result<List<Customer>>();
        }
    }
}
