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
using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Domain.Models.Admin;

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
                var customerList = new List<Customer>()
                {

                };

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
                                    Id = Convert.ToInt32(dr["Id"]),
                                    PhoneNumber = dr["PhoneNumber"].ToString(),
                                    TCKNo = dr["TCKNo"].ToString(),
                                    DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"])
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

                var productList = GetProducts();
                if (!productList.Result.HasError && productList.Result.Data != null)
                {
                    foreach (var product in productList.Result.Data)
                    {
                        var offerNumber = new Random().Next(100000, 1000000).ToString();

                        foreach (var item in customerList)
                        {
                            var currentCustomer = customers.FirstOrDefault(a => a.DateOfBirth == item.DateOfBirth && a.TCKNo == item.TCKNo && a.PhoneNumber == item.PhoneNumber);
                            if (currentCustomer != null)
                            {
                                item.DateOfBirth = currentCustomer.DateOfBirth;
                                item.Weight = currentCustomer.Weight;
                                item.Height = currentCustomer.Height;

                                try
                                {
                                    var offerResult = CalculateAndSaveOffer(product, item, offerNumber);
                                    if (!offerResult.Result.HasError && offerResult.Result.Data != null)
                                    {
                                        item.Offers.Add(offerResult.Result.Data);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return new Result<List<Customer>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                                }
                            }
                        }
                    }
                }

                return new Result<List<Customer>>() { Data = customerList };
            }

            return new Result<List<Customer>>();
        }

        public async Task<Result<ProductModel>> GetOffers(string offerNo)
        {
            if (!string.IsNullOrEmpty(offerNo))
            {
                try
                {
                    var resultModel = new ProductModel();

                    var productModelList = new List<ProductModel>();

                    _dal.AddInputParameter(new SqlParameter("@OfferNumber", offerNo));

                    var dr = _dal.ExecuteDrSelectQuery("sp_GetProductModelsByOfferNumber", CommandType.StoredProcedure);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var model = new ProductModel
                            {
                                OfferIds = dr["Id"].ToString(),
                                ProductId = Convert.ToInt32(dr["ProductId"]),
                                ProductName = dr["ProductName"].ToString(),
                                TotalPrice = Convert.ToDecimal(dr["TotalPrice"])
                            };

                            productModelList.Add(model);
                        }
                    }

                    if (productModelList != null)
                    {
                        foreach (var item in productModelList)
                        {
                            resultModel.TotalPrice += item.TotalPrice;
                            resultModel.OfferIds += productModelList.IndexOf(item) + 1 == productModelList.Count ? item.OfferIds : item.OfferIds + ",";
                            resultModel.ProductName = item.ProductName;
                            resultModel.ProductId = item.ProductId;
                        }
                    }

                    return new Result<ProductModel>() { Data = resultModel };
                }
                catch (Exception ex)
                {
                    return new Result<ProductModel>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<ProductModel>();
        }

        public async Task<Result<Policy>> CreatePolicy(PolicyModel model)
        {
            if (model != null)
            {
                try
                {
                    var validationResult = _validation.Validate(model);
                    if (validationResult.HasError)
                    {
                        return new Result<Policy>(validationResult);
                    }

                    _dal.AddInputParameter(
                        new SqlParameter("@InsurerId", model.InsurerId),
                        new SqlParameter("@OfferIds", model.OfferIds),
                        new SqlParameter("@TotalPrice", model.TotalPrice),
                        new SqlParameter("@PolicyIsDone", model.PolicyIsDone)
                        );

                    var insertResult = _dal.ExecuteQuery("sp_CreatePolicy", CommandType.StoredProcedure);

                    var policy = default(Policy);

                    _dal.AddInputParameter(
                        new SqlParameter("@InsurerId", model.InsurerId),
                        new SqlParameter("@OfferIds", model.OfferIds),
                        new SqlParameter("@TotalPrice", model.TotalPrice),
                        new SqlParameter("@PolicyIsDone", model.PolicyIsDone)
                        );

                    var dr = _dal.ExecuteDrSelectQuery("sp_GetPolicyByParameters", CommandType.StoredProcedure);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            var policyResult = new Policy
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                OfferIds = dr["OfferIds"].ToString(),
                                TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                                PolicyIsDone = Convert.ToBoolean(dr["PolicyIsDone"])
                            };

                            policy = policyResult;
                        }
                    }

                    return new Result<Policy>() { Data = policy };
                }
                catch (Exception ex)
                {
                    return new Result<Policy>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Policy>();
        }

        public async Task<Result<List<Installment>>> GetInstallments()
        {
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllInstallments", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var installmentList = new List<Installment>();

                    while (dr.Read())
                    {
                        var installment = new Installment
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            InstallmentCount = Convert.ToInt32(dr["InstallmentCount"]),
                            Rate = Convert.ToDecimal(dr["Rate"]),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        installmentList.Add(installment);
                    }

                    return new Result<List<Installment>>()
                    {
                        Data = installmentList.ToList()
                    };
                }

                return new Result<List<Installment>>();
            }
            catch (Exception ex)
            {
                return new Result<List<Installment>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<CalculatedInstallmentModel>> SelectInstallment(int installmentId, int policyId)
        {
            if (installmentId == 0 || policyId == 0)
            {
                return new Result<CalculatedInstallmentModel>(StatusCodes.Status404NotFound.ToString(), "Taksit veya poliçe bulunamadı.");
            }

            try
            {
                var calculatedModel = new CalculatedInstallmentModel()
                {
                    Installments = new List<CalculatedModel>()
                };

                var policyResult = GetPolicy(policyId);
                var installmentResult = GetInstallment(installmentId);

                if (policyResult.Result.HasError || installmentResult.Result.HasError)
                {
                    return new Result<CalculatedInstallmentModel>(StatusCodes.Status500InternalServerError.ToString(), "Poliçe veya taksit çekilirken hata oluştu.");
                }

                var policy = policyResult.Result.Data;
                var installment = installmentResult.Result.Data;

                if (installment.InstallmentCount == 1)
                {
                    calculatedModel.TotalPrice = policy.TotalPrice;
                    calculatedModel.Installments.Add(new CalculatedModel()
                    {
                        Name = "Peşin",
                        Price = policy.TotalPrice
                    });
                }
                else
                {
                    var price = policy.TotalPrice + ((policy.TotalPrice * installment.Rate) / 100);

                    calculatedModel.TotalPrice = price;
                    for (int i = 1; i <= installment.InstallmentCount; i++)
                    {
                        calculatedModel.Installments.Add(new CalculatedModel()
                        {
                            Name = i + ". Taksit Ödemesi",
                            Price = price / installment.InstallmentCount
                        });
                    }
                }

                return new Result<CalculatedInstallmentModel>() { Data = calculatedModel };
            }
            catch (Exception ex)
            {
                return new Result<CalculatedInstallmentModel>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<ContinuePolicyModel>> ContinuePolicy(int installmentId, int policyId)
        {
            if (installmentId == 0 || policyId == 0)
            {
                return new Result<ContinuePolicyModel>(StatusCodes.Status404NotFound.ToString(), "Taksit veya poliçe bulunamadı.");
            }

            try
            {
                var calculatedModel = await SelectInstallment(installmentId, policyId);
                if (calculatedModel.HasError || calculatedModel.Data == null)
                {
                    return new Result<ContinuePolicyModel>(StatusCodes.Status500InternalServerError.ToString(), "Poliçe oluşturulurken hata oluştu.");
                }

                var updatePolicyResult = UpdatePolicy(policyId, installmentId, calculatedModel.Data.TotalPrice, false);
                if (updatePolicyResult.Result.HasError || updatePolicyResult.Result.Data == null)
                {
                    return new Result<ContinuePolicyModel>(StatusCodes.Status500InternalServerError.ToString(), "Poliçe oluşturulurken hata oluştu.");
                }

                return new Result<ContinuePolicyModel>() { Data = new ContinuePolicyModel() { InstallmentId = installmentId, PolicyId = policyId } };
            }
            catch (Exception ex)
            {
                return new Result<ContinuePolicyModel>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<PolicyNumberModel>> PayByCreditCard(PaymentModel model)
        {
            if (model == null)
            {
                return new Result<PolicyNumberModel>(StatusCodes.Status404NotFound.ToString(), "Ödeme bilgileri bulunamadı.");
            }

            try
            {
                var validationResult = _validation.Validate(model);
                if (validationResult.HasError)
                {
                    return new Result<PolicyNumberModel>(validationResult);
                }

                var paymentLog = new PaymentLog()
                {
                    InsurerId = model.InsurerId,
                    PolicyId = model.PolicyId,
                    CardHolderName = model.CardHolderName,
                    CardNumber = model.CardNumber,
                    CVC = model.CVC,
                    Expiration = model.Expiration
                };

                var paymentLogResult = SavePaymentLog(paymentLog);
                if (!paymentLogResult.Result.Data)
                {
                    return new Result<PolicyNumberModel>(StatusCodes.Status500InternalServerError.ToString(), "Ödeme alınırken hata oluştu.");
                }

                var policyDetail = new PolicyDetail()
                {
                    PolicyId = model.PolicyId,
                    PaymentDate = DateTime.Now,
                    PolicyEndDate = DateTime.Now.AddYears(1),
                    PolicyStartDate = DateTime.Now
                };

                var paymentDetailResult = SavePolicyDetail(policyDetail);
                if (!paymentDetailResult.Result.Data)
                {
                    return new Result<PolicyNumberModel>(StatusCodes.Status500InternalServerError.ToString(), "Poliçe detayı oluşturulurken hata oluştu.");
                }

                var policyNumber = new Random().Next(100000, 1000000).ToString();

                var updatePolicyResult = UpdatePolicy(model.PolicyId, 0, 0, true, policyNumber);

                return new Result<PolicyNumberModel>() { Data = new PolicyNumberModel() { PolicyNumber = policyNumber } };
            }
            catch (Exception ex)
            {
                return new Result<PolicyNumberModel>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<List<PolicySummaryModel>>> GetPolicySummaryModels()
        {
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllPolicySummaries", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    var summaryList = new List<PolicySummaryModel>();

                    while (dr.Read())
                    {
                        var summary = new PolicySummaryModel
                        {
                            PolicyId = Convert.ToInt32(dr["PolicyId"]),
                            InsurerName = dr["InsurerName"].ToString(),
                            InsurerSurname = dr["InsurerSurname"].ToString(),
                            ProximityType = (ProximityType)Convert.ToInt32(dr["ProximityType"]),
                            TC = dr["TC"].ToString(),
                            Name = dr["Name"].ToString(),
                            Surname = dr["Surname"].ToString(),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                            OfferNumber = dr["OfferNumber"].ToString(),
                            PolicyNumber = dr["PolicyNumber"].ToString(),
                            ProductName = dr["ProductName"].ToString(),
                            Installment = Convert.ToInt32(dr["Installment"]),
                            Price = Convert.ToDecimal(dr["Price"]),
                            PolicyIsDone = Convert.ToBoolean(dr["PolicyIsDone"]),
                            PolicyStartDate = Convert.ToBoolean(dr["PolicyIsDone"]) ? Convert.ToDateTime(dr["PolicyStartDate"]) : DateTime.Now,
                            PolicyEndDate = Convert.ToBoolean(dr["PolicyIsDone"]) ? Convert.ToDateTime(dr["PolicyEndDate"]) : DateTime.Now
                        };

                        summaryList.Add(summary);
                    }

                    var list = summaryList.GroupBy(a => a.TC).Select(b => b.First()).ToList();

                    return new Result<List<PolicySummaryModel>>()
                    {
                        Data = list
                    };
                }

                return new Result<List<PolicySummaryModel>>();
            }
            catch (Exception ex)
            {
                return new Result<List<PolicySummaryModel>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        public async Task<Result<UpdatePolicyStateResultModel>> UpdatePolicyState(int policyId, bool confirmRequest, bool deleteRequest)
        {
            if (policyId == 0)
            {
                return new Result<UpdatePolicyStateResultModel>(StatusCodes.Status404NotFound.ToString(), "Poliçe bulunamadı.");
            }

            try
            {
                _dal.AddInputParameter(
                     new SqlParameter("@Id", policyId),
                     new SqlParameter("@PolicyIsDone", confirmRequest),
                     new SqlParameter("@Deleted", deleteRequest)
                     );

                var offerResult = _dal.ExecuteQuery("sp_UpdatePolicyState", CommandType.StoredProcedure);

                return new Result<UpdatePolicyStateResultModel>();
            }
            catch (Exception ex)
            {
                return new Result<UpdatePolicyStateResultModel>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }

        private async Task<Result<Offer>> CalculateAndSaveOffer(Product product, Customer customer, string offerNumber)
        {
            if (customer != null && product != null)
            {
                try
                {
                    var age = DateTime.Now.Year - customer.DateOfBirth.Year;
                    decimal price = product.Price;

                    var bodyMassIndex = CalculateBodyMassIndex(customer.Weight, customer.Height);
                    if (bodyMassIndex == BodyMassIndex.LOW || bodyMassIndex == BodyMassIndex.OWERWEIGHT)
                    {
                        price = product.Price + (product.Price / 5);
                    }
                    else if (bodyMassIndex == BodyMassIndex.OBESE)
                    {
                        price = product.Price + (product.Price / 10);
                    }

                    if (age > 50)
                    {
                        price = product.Price + (product.Price / 5);
                    }
                    else if (age > 65)
                    {
                        price = product.Price + (product.Price / 10);
                    }

                    var offer = new Offer()
                    {
                        CustomerId = customer.Id,
                        ProductId = product.Id,
                        TotalPrice = price,
                        CompanyName = "Bupa Acıbadem Sigorta",
                        OfferNumber = offerNumber
                    };

                    var saveOfferResult = SaveOffer(offer);
                    if (!saveOfferResult.Result.HasError && saveOfferResult.Result.Data != null)
                    {
                        return new Result<Offer>()
                        {
                            Data = offer
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new Result<Offer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Offer>();
        }
        private async Task<Result<Offer>> SaveOffer(Offer offer)
        {
            if (offer != null)
            {
                try
                {
                    var validationResult = _validation.Validate(offer);
                    if (validationResult.HasError)
                    {
                        return new Result<Offer>(validationResult);
                    }

                    _dal.AddInputParameter(
                        new SqlParameter("@ProductId", offer.ProductId),
                        new SqlParameter("@CustomerId", offer.CustomerId),
                        new SqlParameter("@CompanyName", offer.CompanyName),
                        new SqlParameter("@OfferNumber", offer.OfferNumber),
                        new SqlParameter("@TotalPrice", offer.TotalPrice)
                        );

                    var offerResult = _dal.ExecuteQuery("sp_OfferSave", CommandType.StoredProcedure);

                    return new Result<Offer>() { Data = offer };
                }
                catch (Exception ex)
                {
                    return new Result<Offer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<Offer>();
        }
        private async Task<Result<Policy>> UpdatePolicy(int policyId, int installmentId, decimal price, bool policyIsDone, string policyNumber = null)
        {
            if (policyId == 0)
            {
                return new Result<Policy>(StatusCodes.Status404NotFound.ToString(), "Poliçe bulunamadı.");
            }

            try
            {
                _dal.AddInputParameter(
                    new SqlParameter("@Id", policyId),
                    new SqlParameter("@InstallmentId", installmentId),
                    new SqlParameter("@TotalPrice", price),
                    new SqlParameter("@PolicyIsDone", policyIsDone),
                    new SqlParameter("@PolicyNumber", policyNumber)
                    );

                var offerResult = _dal.ExecuteQuery("sp_UpdatePolicy", CommandType.StoredProcedure);

                return new Result<Policy>() { Data = new Policy() { Id = policyId, InstallmentId = installmentId } };
            }
            catch (Exception ex)
            {
                return new Result<Policy>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
        private async Task<Result<bool>> SavePaymentLog(PaymentLog paymentLog)
        {
            if (paymentLog != null)
            {
                try
                {
                    var validationResult = _validation.Validate(paymentLog);
                    if (validationResult.HasError)
                    {
                        return new Result<bool>(validationResult);
                    }

                    _dal.AddInputParameter(
                        new SqlParameter("@InsurerId", paymentLog.InsurerId),
                        new SqlParameter("@PolicyId", paymentLog.PolicyId),
                        new SqlParameter("@CVC", paymentLog.CVC),
                        new SqlParameter("@CardHolderName", paymentLog.CardHolderName),
                        new SqlParameter("@CardNumber", paymentLog.CardNumber),
                        new SqlParameter("@Expiration", paymentLog.Expiration)
                        );

                    var offerResult = _dal.ExecuteQuery("sp_PaymentLogSave", CommandType.StoredProcedure);

                    return new Result<bool>() { Data = true };
                }
                catch (Exception ex)
                {
                    return new Result<bool>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<bool>();
        }
        private async Task<Result<bool>> SavePolicyDetail(PolicyDetail policyDetail)
        {
            if (policyDetail != null)
            {
                try
                {
                    var validationResult = _validation.Validate(policyDetail);
                    if (validationResult.HasError)
                    {
                        return new Result<bool>(validationResult);
                    }

                    _dal.AddInputParameter(
                        new SqlParameter("@PolicyId", policyDetail.PolicyId),
                        new SqlParameter("@PaymentDate", policyDetail.PaymentDate),
                        new SqlParameter("@PolicyStartDate", policyDetail.PolicyStartDate),
                        new SqlParameter("@PolicyEndDate", policyDetail.PolicyEndDate)
                        );

                    var offerResult = _dal.ExecuteQuery("sp_PolicyDetailSave", CommandType.StoredProcedure);

                    return new Result<bool>() { Data = true };
                }
                catch (Exception ex)
                {
                    return new Result<bool>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
                }
            }

            return new Result<bool>();
        }
        private async Task<Result<List<Product>>> GetProducts()
        {
            var productList = new List<Product>();
            try
            {
                var dr = _dal.ExecuteDrSelectQuery("sp_GetAllProducts", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var product = new Product
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            Price = Convert.ToDecimal(dr["Price"]),
                            CreateDate = Convert.ToDateTime(dr["CreateDate"]),
                            UpdateDate = Convert.ToDateTime(dr["UpdateDate"])
                        };

                        productList.Add(product);
                    }
                }

                return new Result<List<Product>>()
                {
                    Data = productList
                };
            }
            catch (Exception ex)
            {
                return new Result<List<Product>>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
        private async Task<Result<Policy>> GetPolicy(int policyId)
        {
            try
            {
                var policyResult = default(Policy);

                _dal.AddInputParameter(new SqlParameter("@Id", policyId));

                var dr = _dal.ExecuteDrSelectQuery("sp_GetPolicyById", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var policy = new Policy
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            InsurerId = Convert.ToInt32(dr["InsurerId"]),
                            OfferIds = dr["OfferIds"].ToString(),
                            InstallmentId = !string.IsNullOrEmpty(dr["InsurerId"].ToString()) ? Convert.ToInt32(dr["InsurerId"]) : 0,
                            TotalPrice = Convert.ToDecimal(dr["TotalPrice"]),
                            PolicyIsDone = Convert.ToBoolean(dr["PolicyIsDone"]),
                            PolicyNumber = dr["PolicyNumber"].ToString(),
                        };

                        policyResult = policy;
                    }
                }

                return new Result<Policy>() { Data = policyResult };
            }
            catch (Exception ex)
            {
                return new Result<Policy>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
        private async Task<Result<Installment>> GetInstallment(int installmentId)
        {
            try
            {
                var installmentResult = default(Installment);

                _dal.AddInputParameter(new SqlParameter("@Id", installmentId));

                var dr = _dal.ExecuteDrSelectQuery("sp_GetInstallmentById", CommandType.StoredProcedure);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var installment = new Installment
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["Name"].ToString(),
                            InstallmentCount = Convert.ToInt32(dr["InstallmentCount"]),
                            Rate = Convert.ToDecimal(dr["Rate"])
                        };

                        installmentResult = installment;
                    }
                }

                return new Result<Installment>() { Data = installmentResult };
            }
            catch (Exception ex)
            {
                return new Result<Installment>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
        private BodyMassIndex CalculateBodyMassIndex(decimal weight, decimal height)
        {
            height = height / 100;
            var res = Convert.ToDouble(weight / (height * height));

            if (res < 18.5)
            {
                return BodyMassIndex.LOW;
            }
            else if (res > 18.5 && res < 24.9)
            {
                return BodyMassIndex.NORMAL;
            }
            else if (res > 25 && res < 29.9)
            {
                return BodyMassIndex.OWERWEIGHT;
            }

            return BodyMassIndex.OBESE;
        }

    }
}
