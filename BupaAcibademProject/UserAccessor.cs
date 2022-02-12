using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BupaAcibademProject
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLogined => Insurer != null;

        public Insurer Insurer
        {
            get
            {
                return Get<Insurer>("CurrentInsurer");
            }
            set
            {
                Store("CurrentInsurer", value);
            }
        }

        public List<int> CustomerIds 
        {
            get
            {
                return Get<List<int>>("CurrentCustomers");
            }
            set
            {
                Store("CurrentCustomers", value);
            }
        }

        public string ClientIP
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.Connection != null)
                {
                    return httpContext.Connection.RemoteIpAddress?.ToString();
                }
                return null;
            }
        }

        public string RequestLink
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.Request != null)
                {
                    return httpContext.Request.Path.Value + httpContext.Request.QueryString.Value;
                }
                return null;
            }
        }

        public void Clear(string key = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                if (string.IsNullOrEmpty(key))
                {
                    httpContext.Session.Clear();
                }
                else if (httpContext.Session.Keys.Contains(key))
                {
                    httpContext.Session.Remove(key);
                }
            }
        }
        public T Get<T>(string key)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                var value = httpContext.Session.GetString(key);
                return value == null ? default : JsonSerializer.Deserialize<T>(value);
            }
            else
            {
                throw new ArgumentNullException("HttpContext");
            }
        }
        public void Store<T>(string key, T data)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                httpContext.Session.SetString(key, JsonSerializer.Serialize(data));
            }
            else
            {
                throw new ArgumentNullException("HttpContext");
            }
        }
    }
}
