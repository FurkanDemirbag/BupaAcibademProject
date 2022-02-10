﻿using BupaAcibademProject.Domain.Entities;
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
        Task<Result<Insurer>> SaveInsurer(InsurerModel model);
    }
}
