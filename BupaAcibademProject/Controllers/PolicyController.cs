using BupaAcibademProject.Domain.Entities;
using BupaAcibademProject.Domain.Enums;
using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Domain.Models.FrontEnd;
using BupaAcibademProject.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BupaAcibademProject.Controllers
{
    public class PolicyController : Controller
    {
        //PROJEDE TÜM METODLARIN REST APIDEN ÇAĞRILMASI İSTENDİĞİ İÇİN; BURADA DİREK SERVİS KATMANINDAKİ METODA İSTEK ATMAK YERİNE APIYE İSTEK ATTIM, APIDEN DE SERVİS METODUNA İSTEK ATILIYOR
        private string url;
        private readonly IUserAccessor _userAccessor;

        public PolicyController(IUserAccessor userAccessor)
        {
            url = "https://localhost:44339/api/";
            _userAccessor = userAccessor;
        }
        public IActionResult Insurer()
        {
            ViewBag.Countries = GetCountries();
            ViewBag.Nationalities = GetNationalities();

            var model = new InsurerModel();

            return View(model);
        }
        public async Task<IActionResult> Insurer_Save(InsurerModel model)
        {
            if (model.CustomerType == CustomerType.PERSONAL)
            {
                ModelState.Remove("CompanyName");
                ModelState.Remove("VatOffice");
                ModelState.Remove("VatNumber");
            }
            else
            {
                ModelState.Remove("Name");
                ModelState.Remove("Surname");
                ModelState.Remove("DateOfBirth");
                ModelState.Remove("NationalityId");

                model.DateOfBirth = DateTime.Now;
            }
            if (!ModelState.IsValid)
            {
                return this.ErrorJson(ModelState);
            }

            var currentUrl = url + "Policy/SaveInsurer";

            var result = await currentUrl.PostRequest<InsurerResultModel>(model);
            if (result.HasError)
            {
                return this.ErrorJson("Sigorta ettiren kaydedilirken hata oluştu.");
            }

            var insurer = new Insurer();
            insurer.Id = result.Data.Insurer.Id;
            insurer.InsurerIsInsured = result.Data.Insurer.InsurerIsInsured;

            _userAccessor.Store("CurrentInsurer", insurer);

            return this.SuccesJson();
        }

        public IActionResult Customer()
        {
            ViewBag.Countries = GetCountries();
            ViewBag.Nationalities = GetNationalities();
            ViewBag.Jobs = GetJobs();

            var model = new CustomerModel();

            if (_userAccessor.Insurer != null)
            {
                model.InsurerId = _userAccessor.Insurer.Id;

                if (_userAccessor.Insurer.InsurerIsInsured)
                {
                    var currentUrl = url + "Policy/GetInsurer?id=" + model.InsurerId;

                    var insurerResponse = currentUrl.GetRequest();
                    if (insurerResponse != null)
                    {
                        var insurer = JsonConvert.DeserializeObject<InsurerResultModel>(insurerResponse);
                        if (insurer != null && insurer.Success && insurer.Insurer != null)
                        {
                            var ins = insurer.Insurer;
                            model.Address = ins.Address;
                            model.CityId = ins.CityId;
                            model.CountryId = ins.CountryId;
                            model.DateOfBirth = ins.DateOfBirth;
                            model.DistrictId = ins.DistrictId;
                            model.Email = ins.Email;
                            model.ForeignTCKNo = ins.ForeignTCKNo;
                            model.Gender = ins.Gender;
                            model.Name = ins.Name;
                            model.NationalityId = ins.NationalityId;
                            model.PassportNo = ins.PassportNo;
                            model.PhoneNumber = ins.PhoneNumber;
                            model.Surname = ins.Surname;
                            model.TCKNo = ins.TCKNo;
                            model.ProximityType = ProximityType.HIMSELF;
                        }
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Customer_Save(List<CustomerModel> customers)
        {
            if (customers == null || !customers.Any())
            {
                return this.ErrorJson("Sigortalı bulunamadı.");
            }

            if (!ModelState.IsValid)
            {
                return this.ErrorJson(ModelState);
            }

            var currentUrl = url + "Policy/SaveCustomers";

            var result = await currentUrl.PostRequest<CustomerResultModel>(customers);
            if (result.HasError)
            {
                return this.ErrorJson("Sigortalı kaydedilirken hata oluştu.");
            }

            var customerIds = new List<int>();
            customerIds = result.Data.Customers.Select(a => a.Id).ToList();

            _userAccessor.Store("CurrentCustomers", customerIds);

            if (result.Data.Success && result.Data.Customers != null)
            {
                var offerModel = new OfferModel();
                foreach (var customer in result.Data.Customers)
                {
                    if (customer.Offers != null && customer.Offers.Any())
                    {
                        var offerNumbers = customer.Offers.Select(a => a.OfferNumber);
                        if (offerNumbers != null)
                        {
                            foreach (var offerNumber in offerNumbers)
                            {
                                var offerModelResult = GetOffers(offerNumber);
                                if (offerModelResult != null && offerModelResult.ProductModels != null)
                                {
                                    foreach (var item in offerModelResult.ProductModels)
                                    {
                                        if (offerModel.ProductModels != null && !offerModel.ProductModels.Any(a => a.OfferIds == item.OfferIds && a.ProductId == item.ProductId && a.TotalPrice == item.TotalPrice))
                                        {
                                            offerModel.ProductModels.Add(item);
                                        }
                                    }
                                }

                                offerModel.OfferNumber = offerNumber;
                            }
                        }
                    }
                }

                if (offerModel != null)
                {
                    _userAccessor.Store("CurrentOffer", offerModel);
                }
            }

            return this.SuccesJson();
        }

        public IActionResult Offer()
        {
            var model = new OfferModel();

            if (_userAccessor.Offer != null)
            {
                model = _userAccessor.Offer;
            }

            return View(model);
        }
        public async Task<IActionResult> CreatePolicy(string offerIds)
        {
            if (string.IsNullOrEmpty(offerIds))
            {
                return this.ErrorJson("Seçili teklif bulunamadı.");
            }

            var selectedOffer = _userAccessor.Offer.ProductModels.FirstOrDefault(a => a.OfferIds == offerIds);
            if (selectedOffer != null)
            {
                var policyModel = new PolicyModel()
                {
                    OfferIds = selectedOffer.OfferIds,
                    InsurerId = _userAccessor.Insurer.Id,
                    PolicyIsDone = false,
                    TotalPrice = selectedOffer.TotalPrice
                };

                var currentUrl = url + "Policy/CreatePolicy";

                var result = await currentUrl.PostRequest<PolicyResultModel>(policyModel);
                if (result.HasError)
                {
                    return this.ErrorJson("Poliçe kaydedilirken hata oluştu.");
                }

                return this.SuccesJson();
            }

            return this.ErrorJson("Poliçe kaydedilirken hata oluştu.");
        }

        public IActionResult Installment()
        {
            if (_userAccessor.Offer != null)
            {
                ViewBag.OfferNumber = _userAccessor.Offer.OfferNumber;
            }
            var model = new InstallmentModel();

            return View(model);
        }

        public IActionResult Payment()
        {
            var model = new PaymentModel();

            return View(model);
        }

        public IActionResult PaymentDone()
        {
            return View();
        }

        public OfferModel GetOffers(string offerNumber)
        {
            var currentUrl = url + "Policy/GetOffers?offerNumber=" + offerNumber;

            var offers = currentUrl.GetRequest();
            if (offers != null)
            {
                var offerResponse = JsonConvert.DeserializeObject<OfferResultModel>(offers);
                if (offerResponse != null && offerResponse.Success && offerResponse.OfferModel != null)
                {
                    return offerResponse.OfferModel;
                }
            }

            return null;
        }

        public IActionResult GetCities(int countryId)
        {
            var currentUrl = url + "Catalog/GetCities?countryId=" + countryId;

            var cities = currentUrl.GetRequest();
            if (cities != null)
            {
                var cityResponse = JsonConvert.DeserializeObject<CityModel>(cities);
                if (cityResponse != null && cityResponse.Success && cityResponse.Cities != null)
                {
                    return this.SuccesJson(cityResponse.Cities);
                }
            }

            return this.ErrorJson("Şehirler servisten çekilemedi.");
        }

        public IActionResult GetDistricts(int cityId)
        {
            var currentUrl = url + "Catalog/GetDistricts?cityId=" + cityId;

            var districts = currentUrl.GetRequest();
            if (districts != null)
            {
                var districtResponse = JsonConvert.DeserializeObject<DistrictModel>(districts);
                if (districtResponse != null && districtResponse.Success && districtResponse.Districts != null)
                {
                    return this.SuccesJson(districtResponse.Districts);
                }
            }

            return this.ErrorJson("İlçeler servisten çekilemedi.");
        }

        private List<Country> GetCountries()
        {
            var currentUrl = url + "Catalog/GetCountries";

            var countries = currentUrl.GetRequest();
            if (countries != null)
            {
                var countryResponse = JsonConvert.DeserializeObject<CountryModel>(countries);
                if (countryResponse != null && countryResponse.Success && countryResponse.Countries != null)
                {
                    return countryResponse.Countries;
                }
            }

            return new List<Country>();
        }

        private List<Nationality> GetNationalities()
        {
            var currentUrl = url + "Catalog/GetNationalities";

            var nationalities = currentUrl.GetRequest();
            if (nationalities != null)
            {
                var nationalityResponse = JsonConvert.DeserializeObject<NationalityModel>(nationalities);
                if (nationalityResponse != null && nationalityResponse.Success && nationalityResponse.Nationalities != null)
                {
                    return nationalityResponse.Nationalities;
                }
            }

            return new List<Nationality>();
        }

        private List<Job> GetJobs()
        {
            var currentUrl = url + "Catalog/GetJobs";

            var jobs = currentUrl.GetRequest();
            if (jobs != null)
            {
                var jobResponse = JsonConvert.DeserializeObject<JobModel>(jobs);
                if (jobResponse != null && jobResponse.Success && jobResponse.Jobs != null)
                {
                    return jobResponse.Jobs;
                }
            }

            return new List<Job>();
        }
    }
}
