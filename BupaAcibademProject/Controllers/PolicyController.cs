using BupaAcibademProject.Domain.Models.Api;
using BupaAcibademProject.Domain.Models.FrontEnd;
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
        public PolicyController()
        {
            url = "https://localhost:44339/api/";
        }
        public IActionResult Insurer()
        {
            var currentUrl = url + "Catalog/GetCountries";

            var countries = currentUrl.GetRequest();
            if (countries != null)
            {
                var countryResponse = JsonConvert.DeserializeObject<CountryModel>(countries);
                if (countryResponse != null && countryResponse.Success && countryResponse.Countries != null)
                {
                    ViewBag.Countries = countryResponse.Countries;
                }
            }

            currentUrl = url + "Catalog/GetNationalities";

            var nationalities = currentUrl.GetRequest();
            if (nationalities != null)
            {
                var nationalityResponse = JsonConvert.DeserializeObject<NationalityModel>(nationalities);
                if (nationalityResponse != null && nationalityResponse.Success && nationalityResponse.Nationalities != null)
                {
                    ViewBag.Nationalities = nationalityResponse.Nationalities;
                }
            }

            var model = new InsurerModel();

            return View(model);
        }
        public async Task<IActionResult> Insurer_Save(InsurerModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.ErrorJson(ModelState);
            }

            var currentUrl = url + "Policy/SaveInsurer";

            var result = await currentUrl.PostRequest<InsurerModel>(model);


            return this.SuccesJson();
        }

        public IActionResult Customer()
        {
            var model = new CustomerModel();

            return View(model);
        }

        public IActionResult Offer()
        {
            var model = new OfferModel();

            return View(model);
        }

        public IActionResult Installment()
        {
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

    }
}
