using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Domain.Models.FrontEnd;
using BupaAcibademProject.Service;
using BupaAcibademProject.Service.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BupaAcibademProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly IUserAccessor _userAccessor;

        public PolicyController(IPolicyService policyService, IUserAccessor userAccessor)
        {
            _policyService = policyService;
            _userAccessor = userAccessor;
        }

        [HttpGet]
        [Route("GetInsurer")]
        public async Task<ActionResult<InsurerResultModel>> SaveInsurer(int id)
        {
            if (id == 0)
            {
                return new InsurerResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Sigorta ettiren bulunamadı."
                };
            }

            var result = await _policyService.GetInsurer(id);
            if (result.HasError)
            {
                return new InsurerResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new InsurerResultModel()
            {
                Success = true,
                Insurer = result.Data
            };
        }

        [HttpPost]
        [Route("SaveInsurer")]
        public async Task<ActionResult<InsurerResultModel>> SaveInsurer([FromBody] InsurerModel model)
        {
            if (model == null)
            {
                return new InsurerResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Sigorta ettiren bulunamadı."
                };
            }

            var validationResult = DataAnnotation.ValidateEntity(model);
            if (validationResult.HasError)
            {
                return new InsurerResultModel()
                {
                    ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                    ErrorMessage = validationResult.ValidationErrors.First().ErrorMessage
                };
            }

            var result = await _policyService.SaveInsurer(model);
            if (result.HasError)
            {
                return new InsurerResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new InsurerResultModel()
            {
                Success = true,
                Insurer = result.Data
            };
        }

        [HttpGet]
        [Route("GetCustomer")]
        public async Task<ActionResult<CustomerResultModel>> GetCustomer(int id)
        {
            if (id == 0)
            {
                return new CustomerResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Sigortalı bulunamadı."
                };
            }

            var result = await _policyService.GetCustomer(id);
            if (result.HasError)
            {
                return new CustomerResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new CustomerResultModel()
            {
                Success = true,
                Customers = new List<Customer>()
                {
                    result.Data
                }
            };
        }

        [HttpPost]
        [Route("SaveCustomers")]
        public async Task<ActionResult<CustomerResultModel>> SaveCustomers([FromBody] List<CustomerModel> model)
        {
            if (model == null)
            {
                return new CustomerResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Sigortalı bulunamadı."
                };
            }

            foreach (var item in model)
            {
                var validationResult = DataAnnotation.ValidateEntity(item);
                if (validationResult.HasError)
                {
                    return new CustomerResultModel()
                    {
                        ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                        ErrorMessage = validationResult.ValidationErrors.First().ErrorMessage
                    };
                }
            }

            var result = await _policyService.SaveCustomers(model);
            if (result.HasError)
            {
                return new CustomerResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new CustomerResultModel()
            {
                Success = true,
                Customers = result.Data
            };
        }

        [HttpGet]
        [Route("GetOffers")]
        public async Task<ActionResult<OfferResultModel>> GetOffers(string offerNumber)
        {
            if (string.IsNullOrEmpty(offerNumber))
            {
                return new OfferResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Teklif bulunamadı."
                };
            }

            var result = await _policyService.GetOffers(offerNumber);
            if (result.HasError)
            {
                return new OfferResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new OfferResultModel()
            {
                Success = true,
                OfferModel = new OfferModel()
                {
                    OfferNumber = offerNumber,
                    ProductModels = new List<ProductModel>()
                    {
                        result.Data
                    }
                }
            };
        }

        [HttpPost]
        [Route("CreatePolicy")]
        public async Task<ActionResult<PolicyResultModel>> CreatePolicy([FromBody] PolicyModel model)
        {
            if (model == null)
            {
                return new PolicyResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Poliçe bulunamadı."
                };
            }

            var validationResult = DataAnnotation.ValidateEntity(model);
            if (validationResult.HasError)
            {
                return new PolicyResultModel()
                {
                    ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                    ErrorMessage = validationResult.ValidationErrors.First().ErrorMessage
                };
            }

            var result = await _policyService.CreatePolicy(model);
            if (result.HasError)
            {
                return new PolicyResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new PolicyResultModel()
            {
                Success = true,
                Policy = result.Data
            };
        }

        [HttpGet]
        [Route("GetInstallments")]
        public async Task<ActionResult<InstallmentResultModel>> GetInstallments()
        {
            var installmentResult = await _policyService.GetInstallments();
            if (installmentResult.HasError)
            {
                return new InstallmentResultModel()
                {
                    ErrorCode = installmentResult.Errors.First().Code,
                    ErrorMessage = installmentResult.Errors.First().Message
                };
            }

            return new InstallmentResultModel()
            {
                Success = true,
                Installments = installmentResult.Data
            };
        }

        [HttpGet]
        [Route("SelectInstallment")]
        public async Task<ActionResult<CalculatedInstallmentModel>> SelectInstallment(int installmentId, int policyId)
        {
            if (installmentId == 0 || policyId == 0)
            {
                return new CalculatedInstallmentModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Taksit veya poliçe bulunamadı."
                };
            }

            var result = await _policyService.SelectInstallment(installmentId, policyId);
            if (result.HasError)
            {
                return new CalculatedInstallmentModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new CalculatedInstallmentModel()
            {
                Success = true,
                Installments = result.Data.Installments,
                TotalPrice = result.Data.TotalPrice
            };
        }

        [HttpGet]
        [Route("ContinuePolicy")]
        public async Task<ActionResult<ContinuePolicyModel>> ContinuePolicy(int installmentId, int policyId)
        {
            if (installmentId == 0 || policyId == 0)
            {
                return new ContinuePolicyModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Taksit veya poliçe bulunamadı."
                };
            }

            var result = await _policyService.ContinuePolicy(installmentId, policyId);
            if (result.HasError)
            {
                return new ContinuePolicyModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new ContinuePolicyModel()
            {
                Success = true,
                InstallmentId = result.Data.InstallmentId,
                PolicyId = result.Data.PolicyId
            };
        }

        [HttpPost]
        [Route("PayByCreditCard")]
        public async Task<ActionResult<PolicyNumberModel>> PayByCreditCard([FromBody] PaymentModel model)
        {
            if (model == null)
            {
                return new PolicyNumberModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Sigorta ettiren bulunamadı."
                };
            }

            var validationResult = DataAnnotation.ValidateEntity(model);
            if (validationResult.HasError)
            {
                return new PolicyNumberModel()
                {
                    ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                    ErrorMessage = validationResult.ValidationErrors.First().ErrorMessage
                };
            }

            var result = await _policyService.PayByCreditCard(model);
            if (result.HasError)
            {
                return new PolicyNumberModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new PolicyNumberModel()
            {
                Success = true,
                PolicyNumber = result.Data.PolicyNumber
            };
        }


        [HttpGet]
        [Route("GetPolicySummaryModels")]
        public async Task<ActionResult<PolicySummaryResultModel>> GetPolicySummaryModels()
        {
            var policySummaryResult = await _policyService.GetPolicySummaryModels();
            if (policySummaryResult.HasError)
            {
                return new PolicySummaryResultModel()
                {
                    ErrorCode = policySummaryResult.Errors.First().Code,
                    ErrorMessage = policySummaryResult.Errors.First().Message
                };
            }

            return new PolicySummaryResultModel()
            {
                Success = true,
                PolicySummaryModels = policySummaryResult.Data
            };
        }

        [HttpGet]
        [Route("UpdatePolicyState")]
        public async Task<ActionResult<UpdatePolicyStateResultModel>> UpdatePolicyState(int policyId, bool confirmRequest, bool deleteRequest)
        {
            if (policyId == 0)
            {
                return new UpdatePolicyStateResultModel()
                {
                    ErrorCode = StatusCodes.Status404NotFound.ToString(),
                    ErrorMessage = "Poliçe bulunamadı."
                };
            }

            var result = await _policyService.UpdatePolicyState(policyId,confirmRequest, deleteRequest);
            if (result.HasError)
            {
                return new UpdatePolicyStateResultModel()
                {
                    ErrorCode = result.Errors.First().Code,
                    ErrorMessage = result.Errors.First().Message
                };
            }

            return new UpdatePolicyStateResultModel()
            {
                Success = true,
                IsSuccess = true
            };
        }
    }
}
