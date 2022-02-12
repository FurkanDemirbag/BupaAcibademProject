using BupaAcibademProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service
{
    public interface IUserAccessor
    {
        bool IsLogined { get; }
        Insurer Insurer { get; set; }
        string ClientIP { get; }
        string RequestLink { get; }
        void Store<T>(string key, T data);
        T Get<T>(string key);
        void Clear(string key = null);
    }
}
