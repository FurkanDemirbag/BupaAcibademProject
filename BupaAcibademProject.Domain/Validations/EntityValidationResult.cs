using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Validations
{
    public class EntityValidationResult
    {
        public IList<ValidationResult> ValidationErrors { get; private set; }

        public bool HasError
        {
            get { return ValidationErrors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> violations = null)
        {
            ValidationErrors = violations;
            if (violations == null)
            {
                ValidationErrors = new List<ValidationResult>();
            }
        }

        public override string ToString()
        {
            return string.Join("\r\n", ValidationErrors.Where(a => !string.IsNullOrEmpty(a.ErrorMessage)).Select(a => a.ErrorMessage));
        }
    }
}
