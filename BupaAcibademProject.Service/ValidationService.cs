using BupaAcibademProject.Domain.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public class ValidationService : IValidationService
    {
        public ValidationService()
        {
        }

        public EntityValidationResult Validate<T>(T entity)
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            Validator.TryValidateObject(entity, vc, validationResults, true);
            return new EntityValidationResult(validationResults);
        }
    }
}
