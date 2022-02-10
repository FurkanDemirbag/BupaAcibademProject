using BupaAcibademProject.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Domain.Models
{
    public class Result
    {
        public List<ResultError> Errors { get; set; }
        public List<string> Warnings { get; set; }
        public bool HasError => Errors.Any();
        public Dictionary<string, object> Extra { get; set; }

        public Result()
        {
            Errors = new List<ResultError>();
            Warnings = new List<string>();
            Extra = new Dictionary<string, object>();
        }

        public Result(string errorCode, string errorMessage)
        {
            Errors = new List<ResultError>()
            {
                new ResultError()
                {
                    Code = errorCode,
                    Message = errorMessage
                }
            };
            Warnings = new List<string>();
            Extra = new Dictionary<string, object>();
        }

        public Result(EntityValidationResult validationResult)
        {
            Errors = new List<ResultError>();
            Errors.AddRange(validationResult.ValidationErrors.Select(a => new ResultError()
            {
                Message = a.ErrorMessage
            }).Distinct());
            Warnings = new List<string>();
            Extra = new Dictionary<string, object>();
        }

        public Result(Result other)
        {
            Errors = other.Errors;
            Warnings = other.Warnings;
            Extra = other.Extra;
        }
    }

    public class ResultError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public Result() : base()
        {
        }

        public Result(string errorCode, string errorMessage) : base(errorCode, errorMessage)
        {
        }

        public Result(EntityValidationResult validationResult) : base(validationResult)
        {
        }

        public Result(Result other) : base(other)
        {
        }
    }
}
