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

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
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
                Insurer = result.Data
            };
        }
    }
}
