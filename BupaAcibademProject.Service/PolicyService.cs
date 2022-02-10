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

        public async Task<Result<Insurer>> SaveInsurer(InsurerModel model)
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
                    new SqlParameter("@NationalityId", model.NationalityId),
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

                var result = _dal.ExecuteQuery("sp_InsurerSave", CommandType.StoredProcedure);

                //await _logService.LogEntityHistory().LogEntityHistory(entity, oldEntity, isTransactional);

                return new Result<Insurer>() { Data = null };
            }
            catch (Exception ex)
            {
                return new Result<Insurer>(StatusCodes.Status500InternalServerError.ToString(), await _logService.LogException(ex));
            }
        }
    }
}
