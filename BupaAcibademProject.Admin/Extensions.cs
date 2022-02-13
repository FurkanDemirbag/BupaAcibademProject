
using BupaAcibademProject.Domain.Models;
using BupaAcibademProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace BupaAcibademProject.Admin
{
    public static class Extensions
    {
        public static string GetRequest(this string url)
        {
            try
            {
                using var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true;
                clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls;

                using var client = new HttpClient(clientHandler);


                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = client.GetAsync(url).Result)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static async Task<Result<T>> PostRequest<T>(this string url, object postData)
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

            try
            {
                dataStr ??= "";
                using var httpContent = new StringContent(dataStr, Encoding.UTF8, "application/json");

                using var clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
                                   System.Security.Authentication.SslProtocols.Tls11 |
                                   System.Security.Authentication.SslProtocols.Tls
                };

                using var client = new HttpClient(clientHandler);

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
        public static IActionResult ErrorJson(this Controller controller, string errorMsg)
        {
            return new Result(StatusCodes.Status500InternalServerError.ToString(), errorMsg).ToJson();
        }
        public static IActionResult ErrorJson(this Controller controller, ModelStateDictionary modelState)
        {
            var errors = new List<ResultError>();
            foreach (var error in modelState.Values.Where(a => a.Errors.Count > 0))
            {
                foreach (var item in error.Errors)
                {
                    errors.Add(new ResultError()
                    {
                        Code = item.ErrorMessage,
                        Message = item.ErrorMessage
                    });
                }
            }
            return new Result() { Errors = errors }.ToJson();
        }
        public static ActionResult SuccesJson(this Controller controller, object data = null)
        {
            return new JsonResult(new { HasError = false, Data = data });
        }
        public static IActionResult ToJson(this Result result)
        {
            return new JsonResult(result);
        }
        public static IActionResult ToView<T>(this Result<T> result, Controller controller, string viewPath = null)
        {
            return ToView(result, controller, result.Data, viewPath);
        }
        public static IActionResult ToView(this Result result, Controller controller, object data = null, string viewPath = null)
        {
            if (data != null)
            {
                controller.ViewData.Model = data;
            }
            if (result.Errors != null && result.Errors.Any())
            {
                controller.ModelState.Clear();
                result.Errors.ForEach(a => controller.ModelState.AddModelError("", a.ToString()));
            }
            return new ViewResult()
            {
                ViewName = viewPath,
                ViewData = controller.ViewData,
                TempData = controller.TempData,
            };
        }
        public static IActionResult ToPartialView<T>(this Result<T> result, Controller controller, string viewPath = null)
        {
            return ToPartialView(result, controller, result.Data, viewPath);
        }
        public static IActionResult ToPartialView(this Result result, Controller controller, object data = null, string viewPath = null)
        {
            if (data != null)
            {
                controller.ViewData.Model = data;
            }
            if (result.Errors != null && result.Errors.Any())
            {
                controller.ModelState.Clear();
                result.Errors.ForEach(a => controller.ModelState.AddModelError("", a.ToString()));
            }
            return new PartialViewResult()
            {
                ViewName = viewPath,
                ViewData = controller.ViewData,
                TempData = controller.TempData
            };
        }
        public static string ToPartialViewString<T>(this Result<T> result, Controller controller, string viewName)
        {
            if (result.Data != null)
            {
                var enumerableData = result.Data as IEnumerable<T>;
                if (enumerableData != null)
                {
                    return controller.RenderPartialViewToString(viewName, enumerableData);
                }
                else
                {
                    return controller.RenderPartialViewToString(viewName, result.Data);
                }
            }

            return controller.RenderPartialViewToString(viewName, result.Data);
        }
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model, Dictionary<string, object> extraData = null)
        {
            using (var sw = new StringWriter())
            {
                var viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                var viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);
                if (viewResult.View == null)
                {
                    viewResult = viewEngine.GetView(null, viewName, false);
                    if (viewResult.View == null)
                    {
                        throw new ArgumentNullException($"{viewName} does not match any available view");
                    }
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                };

                foreach (var item in controller.ViewData)
                {
                    viewDictionary[item.Key] = item.Value;
                }
                //controller.ViewData.Model = model;

                var viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    //controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewContext.ViewData.TemplateInfo.HtmlFieldPrefix = controller.ViewData.TemplateInfo.HtmlFieldPrefix;

                if (extraData != null)
                {
                    foreach (var item in extraData)
                    {
                        viewContext.ViewData[item.Key] = item.Value;
                    }
                }

                viewResult.View.RenderAsync(viewContext).Wait();
                return sw.ToString();
            }
        }
    }
}
