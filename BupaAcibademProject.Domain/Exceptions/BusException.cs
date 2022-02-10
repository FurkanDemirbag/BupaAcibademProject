using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BupaAcibademProject.Domain.Models;

namespace BupaAcibademProject.Domain.Exceptions
{
    public class BusException : Exception
    {
        public BusException() : base()
        {

        }

        public BusException(string message) : base(message)
        {

        }
        public BusException(Result result) : base(string.Join(Environment.NewLine, result.Errors.Select(a => a.Message)))
        {

        }
    }
}
