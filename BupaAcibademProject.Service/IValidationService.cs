using BupaAcibademProject.Domain.Validations;
using BupaAcibademProject.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface IValidationService : ISingletonService
    {
        EntityValidationResult Validate<T>(T entity);
    }
}
