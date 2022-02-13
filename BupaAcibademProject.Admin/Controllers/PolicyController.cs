using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Enums;
using BupaAcibademProject.Domain.Models.Admin;
using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BupaAcibademProject.Admin.Controllers
{
    public class PolicyController : Controller
    {
        //PROJEDE TÜM METODLARIN REST APIDEN ÇAĞRILMASI İSTENDİĞİ İÇİN; BURADA DİREK SERVİS KATMANINDAKİ METODA İSTEK ATMAK YERİNE APIYE İSTEK ATTIM, APIDEN DE SERVİS METODUNA İSTEK ATILIYOR
        //WEB API PROJESINDEKİ İSTEKLER GELMİYORSA WEB API PROJESINDEKİ PROPERTIES DOSYASINDAKİ LAUNCHSETTINGSTEKİ LOCALHOST ADRESİNİN BURADAKİ LOCALHOST'A SET EDİLMESİ GEREKİR 
        private string url;
        private readonly IUserAccessor _userAccessor;

        public PolicyController(IUserAccessor userAccessor)
        {
            url = "https://localhost:44339/api/";
            _userAccessor = userAccessor;
        }

        public IActionResult Index()
        {
            var model = new List<PolicySummaryModel>();

            var policySummaries = GetPolicySummaryModels();
            if (policySummaries != null && policySummaries.Any())
            {
                model = policySummaries;

                _userAccessor.Store("CurrentPolicies", policySummaries);
            }

            return View(model);
        }

        public IActionResult UpdatePolicyState(int policyId, bool confirmRequest, bool deleteRequest)
        {
            if (policyId == 0)
            {
                return this.ErrorJson("Poliçe bulunamadı.");
            }

            var currentUrl = url + "Policy/UpdatePolicyState?policyId=" + policyId + "&confirmRequest=" + confirmRequest + "&deleteRequest=" + deleteRequest;
            var result = currentUrl.GetRequest();
            if (result != null)
            {
                var response = JsonConvert.DeserializeObject<UpdatePolicyStateResultModel>(result);
                if (response != null && response.Success && response.IsSuccess)
                {
                    return this.SuccesJson();
                }
            }

            return this.ErrorJson("Poliçe güncellenirken hata oluştu.");
        }

        public IActionResult FilterPolicies(FilterForm form)
        {
            var list = _userAccessor.PolicySummaryModels;

            if (form.ProximityType != ProximityType.ALL)
            {
                list = list.Where(a => a.ProximityType == form.ProximityType).ToList();
            }

            if (form.PolicyStartDate.Year > 1 && form.PolicyEndDate.Year > 1)
            {
                list = list.Where(a => a.PolicyEndDate < form.PolicyEndDate && form.PolicyStartDate > a.PolicyStartDate).ToList();
            }
            else if (form.PolicyStartDate.Year > 1)
            {
                list = list.Where(a => form.PolicyStartDate > a.PolicyStartDate).ToList();
            }
            else if (form.PolicyEndDate.Year > 1)
            {
                list = list.Where(a => a.PolicyEndDate < form.PolicyEndDate).ToList();
            }

            list = list.Where(a => !string.IsNullOrEmpty(form.Name) ? a.Name == form.Name : a.Name != null && !string.IsNullOrEmpty(form.Surname) ? a.Surname == form.Surname : a.Surname != null && !string.IsNullOrEmpty(form.TC) ? a.TC == form.TC : a.TC != null).ToList();
            list = list.Where(a => !string.IsNullOrEmpty(form.InsurerName) ? a.InsurerName == form.InsurerName : a.InsurerName != null && !string.IsNullOrEmpty(form.InsurerSurname) ? a.InsurerSurname == form.InsurerSurname : a.InsurerSurname != null).ToList();
            list = list.Where(a => !string.IsNullOrEmpty(form.OfferNumber) ? a.OfferNumber == form.OfferNumber : a.OfferNumber != null && !string.IsNullOrEmpty(form.PolicyNumber) ? a.PolicyNumber == form.PolicyNumber : a.PolicyNumber != null).ToList();


            return this.SuccesJson(new { list = list });
        }

        private List<PolicySummaryModel> GetPolicySummaryModels()
        {
            var currentUrl = url + "Policy/GetPolicySummaryModels";

            var models = currentUrl.GetRequest();
            if (models != null)
            {
                var modelResponse = JsonConvert.DeserializeObject<PolicySummaryResultModel>(models);
                if (modelResponse != null && modelResponse.Success && modelResponse.PolicySummaryModels != null)
                {
                    return modelResponse.PolicySummaryModels;
                }
            }

            return new List<PolicySummaryModel>();
        }
    }
}
