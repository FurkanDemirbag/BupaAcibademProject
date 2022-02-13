using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Models;
using BupaAcibademProject.Domain.Models.FrontEnd;
using BupaAcibademProject.Service.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface IPolicyService : IScopedService, IServiceBase
    {
        Task<Result<Insurer>> GetInsurer(int id);
        Task<Result<Insurer>> SaveInsurer(InsurerModel model);

        Task<Result<Customer>> GetCustomer(int id);
        Task<Result<List<Customer>>> SaveCustomers(List<CustomerModel> customers);

        Task<Result<ProductModel>> GetOffers(string offerNo);

        Task<Result<Policy>> CreatePolicy(PolicyModel model);
    }
}
