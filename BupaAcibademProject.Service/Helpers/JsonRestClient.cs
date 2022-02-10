using BupaAcibademProject.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BupaAcibademProject.Service.Helpers
{
    public class JsonRestClient
    {
        public async Task<Result<T>> GetBasic<T>(string url, string userName, string password)
        {
            return await Get<T>(url, Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))), "Basic");
        }
        public async Task<Result<T>> PostBasic<T>(string url, object postData, string userName, string password)
        {
            return await Post<T>(url, postData, Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))), "Basic");
        }
        public async Task<Result<T>> PutBasic<T>(string url, object putData, string userName, string password)
        {
            return await Put<T>(url, putData, Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))), "Basic");
        }
        public async Task<Result<T>> DeleteBasic<T>(string url, string userName, string password)
        {
            return await Delete<T>(url, Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password))), "Basic");
        }

        public async Task<Result<T>> Get<T>(string url, string authHeader = null, string authScheme = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Url boş olamaz");
            }

            if (authScheme == "Basic")
            {
                return await Get<T>(url, new AuthenticationHeaderValue("Basic", authHeader));
            }
            if (authScheme == "Bearer")
            {
                return await Get<T>(url, new AuthenticationHeaderValue("Bearer", authHeader));
            }

            return await Get<T>(url, null);
        }
        public async Task<Result<T>> Post<T>(string url, object postData, string authHeader = null, string authScheme = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Url boş olamaz");
            }

            var dataStr = default(string);
            if (postData is string str)
            {
                dataStr = str;
            }
            else if (postData != null)
            {
                await using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, postData);
                stream.Position = 0;

                using var reader = new StreamReader(stream);
                dataStr = await reader.ReadToEndAsync();
            }

            if (authScheme == "Basic")
            {
                return await Post<T>(url, new AuthenticationHeaderValue("Basic", authHeader), dataStr);
            }
            if (authScheme == "Bearer")
            {
                return await Post<T>(url, new AuthenticationHeaderValue("Bearer", authHeader), dataStr);
            }

            return await Post<T>(url, null, dataStr);
        }
        public async Task<Result<T>> Put<T>(string url, object putData, string authHeader = null, string authScheme = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Url boş olamaz");
            }

            var dataStr = default(string);
            if (putData is string str)
            {
                dataStr = str;
            }
            else if (putData != null)
            {
                await using var stream = new MemoryStream();
                await JsonSerializer.SerializeAsync(stream, putData);
                stream.Position = 0;

                using var reader = new StreamReader(stream);
                dataStr = await reader.ReadToEndAsync();
            }

            if (authScheme == "Basic")
            {
                return await Put<T>(url, new AuthenticationHeaderValue("Basic", authHeader), dataStr);
            }
            if (authScheme == "Bearer")
            {
                return await Put<T>(url, new AuthenticationHeaderValue("Bearer", authHeader), dataStr);
            }

            return await Put<T>(url, null, dataStr);
        }
        public async Task<Result<T>> Delete<T>(string url, string authHeader = null, string authScheme = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Url boş olamaz");
            }

            if (authScheme == "Basic")
            {
                return await Delete<T>(url, new AuthenticationHeaderValue("Basic", authHeader));
            }
            if (authScheme == "Bearer")
            {
                return await Delete<T>(url, new AuthenticationHeaderValue("Bearer", authHeader));
            }

            return await Delete<T>(url, null);
        }

        private async Task<Result<T>> Get<T>(string url, AuthenticationHeaderValue authenticationHeader)
        {
            try
            {
                using var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls11 |
                                   System.Security.Authentication.SslProtocols.Tls
                };

                using var client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (authenticationHeader != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }

                using var response = await client.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                try
                {
                    var data = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (data == null && !response.IsSuccessStatusCode)
                    {
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), response.ReasonPhrase);
                    }

                    return new Result<T>() { Data = data };
                }
                catch (Exception ex)
                {
                    if (ex is JsonException)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message)
                        {
                            Extra = new System.Collections.Generic.Dictionary<string, object>() { { "JsonRawString", data } }
                        };
                    }
                    return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<Result<T>> Post<T>(string url, AuthenticationHeaderValue authenticationHeader, string postData = null)
        {
            try
            {
                postData ??= "";
                using var httpContent = new StringContent(postData, Encoding.UTF8, "application/json");

                using var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls11 |
                                   System.Security.Authentication.SslProtocols.Tls
                };

                using var client = new HttpClient(clientHandler);

                if (authenticationHeader != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using var response = await client.PostAsync(url, httpContent);
                var stream = await response.Content.ReadAsStreamAsync();
                try
                {
                    var data = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (data == null && !response.IsSuccessStatusCode)
                    {
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), response.ReasonPhrase);
                    }

                    return new Result<T>() { Data = data };
                }
                catch (Exception ex)
                {
                    if (ex is JsonException)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message)
                        {
                            Extra = new System.Collections.Generic.Dictionary<string, object>() { { "JsonRawString", data } }
                        };
                    }
                    return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<Result<T>> Put<T>(string url, AuthenticationHeaderValue authenticationHeader, string putData = null)
        {
            try
            {
                putData ??= "";
                using var httpContent = new StringContent(putData, Encoding.UTF8, "application/json");

                using var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls11 |
                                   System.Security.Authentication.SslProtocols.Tls
                };

                using var client = new HttpClient(clientHandler);

                if (authenticationHeader != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using var response = await client.PutAsync(url, httpContent);
                var stream = await response.Content.ReadAsStreamAsync();
                try
                {
                    var data = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (data == null && !response.IsSuccessStatusCode)
                    {
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), response.ReasonPhrase);
                    }

                    return new Result<T>() { Data = data };
                }
                catch (Exception ex)
                {
                    if (ex is JsonException)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message)
                        {
                            Extra = new System.Collections.Generic.Dictionary<string, object>() { { "JsonRawString", data } }
                        };
                    }
                    return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<Result<T>> Delete<T>(string url, AuthenticationHeaderValue authenticationHeader)
        {
            try
            {
                using var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls11 |
                                   System.Security.Authentication.SslProtocols.Tls
                };

                using var client = new HttpClient(clientHandler);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (authenticationHeader != null)
                {
                    client.DefaultRequestHeaders.Authorization = authenticationHeader;
                }

                using var response = await client.DeleteAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                try
                {
                    var data = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                    if (data == null && !response.IsSuccessStatusCode)
                    {
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), response.ReasonPhrase);
                    }

                    return new Result<T>() { Data = data };
                }
                catch (Exception ex)
                {
                    if (ex is JsonException)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message)
                        {
                            Extra = new System.Collections.Generic.Dictionary<string, object>() { { "JsonRawString", data } }
                        };
                    }
                    return new Result<T>(StatusCodes.Status500InternalServerError.ToString(), "Json okuma hatası. Ex: " + ex.Message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
