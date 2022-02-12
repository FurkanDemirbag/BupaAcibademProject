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
    }
}
