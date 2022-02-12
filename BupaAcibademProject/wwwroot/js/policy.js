﻿$().ready(function () {
    $.validator.addMethod("notOnlyZero", function (value, element, param) {
        return this.optional(element) || parseInt(value) > 0;
    });

    $("#insurerForm").validate({
        rules: {
            TCKNo: { required: true, maxlength: 11, minlength: 11 },
            PhoneNumber: { required: true, maxlength: 10, minlength: 10 },
            ForeignTCKNo: { maxlength: 20, minlength: 5 },
            PassportNo: { maxlength: 7, minlength: 7 },
            Email: { required: true, email: true },
            Name: { required: true, maxlength: 400, minlength: 2 },
            Surname: { required: true, maxlength: 400, minlength: 2 },
            CustomerType: { required: true },
            Gender: { required: true },
            DateOfBirth: { required: true, date: true },
            CountryId: { required: true, notOnlyZero: '0' },
            NationalityId: { required: true, notOnlyZero: '0' },
            CityId: { required: true, notOnlyZero: '0' },
            DistrictId: { required: true, notOnlyZero: '0' },
            Address: { required: true, maxlength: 400, minlength: 10 },
            CompanyName: { required: true, maxlength: 400, minlength: 5 },
            VatOffice: { required: true, maxlength: 400, minlength: 5 },
            VatNumber: { required: true, number: true }
        },
        messages: {
            TCKNo: {
                required: "Tc Kimlik No boş olamaz",
                minlength: "Tc Kimlik No 11 karakter uzunluğunda olmalıdır",
                maxlength: "Tc Kimlik No 11 karakter uzunluğunda olmalıdır"
            },
            PhoneNumber: {
                required: "Cep telefonu boş olamaz",
                minlength: "Cep telefonu 10 karakter uzunluğunda olmalıdır",
                maxlength: "Cep telefonu 10 karakter uzunluğunda olmalıdır"
            },
            ForeignTCKNo: {
                minlength: "Yabancı Kimlik No en az 5 karakter uzunluğunda olmalıdır",
                maxlength: "Yabancı Kimlik No en fazla 20 karakter uzunluğunda olmalıdır"
            },
            PassportNo: {
                minlength: "Pasaport No 7 karakter uzunluğunda olmalıdır",
                maxlength: "Pasaport No 7 karakter uzunluğunda olmalıdır"
            },
            Email: {
                required: "E-Posta boş olamaz",
                email: "Geçerli bir e-posta adresi giriniz"
            },
            CompanyName: {
                required: "Şirket Adı boş olamaz",
                minlength: "Şirket Adı 5 karakter uzunluğunda olmalıdır",
                maxlength: "Şirket Adı 400 karakter uzunluğunda olmalıdır"
            },
            VatOffice: {
                required: "Vergi Dairesi boş olamaz",
                minlength: "Vergi Dairesi 5 karakter uzunluğunda olmalıdır",
                maxlength: "Vergi Dairesi 400 karakter uzunluğunda olmalıdır"
            },
            VatNumber: {
                required: "Vergi Numarası boş olamaz",
                number: "Geçerli bir vergi numarası giriniz"
            },
            Name: {
                required: "Ad boş olamaz",
                minlength: "Ad minumum 2 karakter uzunluğunda olmalıdır",
                maxlength: "Ad maximum 400 karakter uzunluğunda olmalıdır"
            },
            Surname: {
                required: "Soyad boş olamaz",
                minlength: "Soyad minumum 2 karakter uzunluğunda olmalıdır",
                maxlength: "Soyad maximum 400 karakter uzunluğunda olmalıdır"
            },
            CustomerType: {
                required: "Lütfen sigorta ettiren tipini seçiniz"
            },
            Gender: {
                required: "Lütfen cinsiyet seçiniz"
            },
            DateOfBirth: {
                required: "Doğum Tarihi boş olamaz",
                date: "Geçerli bir tarih giriniz"
            },
            CountryId: {
                required: "Lütfen ülke seçiniz",
                notOnlyZero: "Lütfen ülke seçiniz"
            },
            NationalityId: {
                required: "Lütfen uyruk seçiniz",
                notOnlyZero: "Lütfen uyruk seçiniz"
            },
            CityId: {
                required: "Lütfen il seçiniz",
                notOnlyZero: "Lütfen il seçiniz"
            },
            DistrictId: {
                required: "Lütfen ilçe seçiniz",
                notOnlyZero: "Lütfen ilçe seçiniz"
            },
            Address: {
                required: "Adres boş olamaz",
                minlength: "Adres minumum 10 karakter uzunluğunda olmalıdır",
                maxlength: "Adres maximum 400 karakter uzunluğunda olmalıdır"
            }
        }
    });

    bupa.prepareSubmit("#insurerForm", function () {
        if ($("#InsurerIsInsured").is(":checked")) {
            $("#InsurerIsInsured").attr("value", "true");
        }
        bupa.ajaxPost("/Policy/Insurer_Save", $("#insurerForm"), $("#insurerForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                bupa.goto('/Policy/Customer');
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });

    $("#customerForm").validate({
        rules: {
            'customers[0].InsurerId': { required: true },
            'customers[0].TCKNo': { required: true, maxlength: 11, minlength: 11 },
            'customers[0].ForeignTCKNo': { maxlength: 20, minlength: 5 },
            'customers[0].PassportNo': { maxlength: 7, minlength: 7 },
            'customers[0].PhoneNumber': { required: true, maxlength: 10, minlength: 10 },
            'customers[0].Email': { required: true, email: true },
            'customers[0].Name': { required: true, maxlength: 400, minlength: 2 },
            'customers[0].Surname': { required: true, maxlength: 400, minlength: 2 },
            'customers[0].ProximityType': { required: true },
            'customers[0].Gender': { required: true },
            'customers[0].DateOfBirth': { required: true, date: true },
            'customers[0].JobId': { required: true, notOnlyZero: '0' },
            'customers[0].CountryId': { required: true, notOnlyZero: '0' },
            'customers[0].NationalityId': { required: true, notOnlyZero: '0' },
            'customers[0].CityId': { required: true, notOnlyZero: '0' },
            'customers[0].DistrictId': { required: true, notOnlyZero: '0' },
            'customers[0].Height': { required: true, number: true, notOnlyZero: '0' },
            'customers[0].Weight': { required: true, number: true, notOnlyZero: '0' },
            'customers[0].Address': { required: true, maxlength: 400, minlength: 10 }

        },
        messages: {
            'customers[0].InsurerId': {
                required: "Sigorta ettiren boş olamaz"
            },
            'customers[0].TCKNo': {
                required: "Tc Kimlik No boş olamaz",
                minlength: "Tc Kimlik No 11 karakter uzunluğunda olmalıdır",
                maxlength: "Tc Kimlik No 11 karakter uzunluğunda olmalıdır"
            },
            'customers[0].ForeignTCKNo': {
                minlength: "Yabancı Kimlik No en az 5 karakter uzunluğunda olmalıdır",
                maxlength: "Yabancı Kimlik No en fazla 20 karakter uzunluğunda olmalıdır"
            },
            'customers[0].PassportNo': {
                minlength: "Pasaport No 7 karakter uzunluğunda olmalıdır",
                maxlength: "Pasaport No 7 karakter uzunluğunda olmalıdır"
            },
            'customers[0].PhoneNumber': {
                required: "Cep telefonu boş olamaz",
                minlength: "Cep telefonu 10 karakter uzunluğunda olmalıdır",
                maxlength: "Cep telefonu 10 karakter uzunluğunda olmalıdır"
            },
            'customers[0].Email': {
                required: "E-Posta boş olamaz",
                email: "Geçerli bir e-posta adresi giriniz"
            },
            'customers[0].Name': {
                required: "Ad boş olamaz",
                minlength: "Ad minumum 2 karakter uzunluğunda olmalıdır",
                maxlength: "Ad maximum 400 karakter uzunluğunda olmalıdır"
            },
            'customers[0].Surname': {
                required: "Soyad boş olamaz",
                minlength: "Soyad minumum 2 karakter uzunluğunda olmalıdır",
                maxlength: "Soyad maximum 400 karakter uzunluğunda olmalıdır"
            },
            'customers[0].ProximityType': {
                required: "Yakınlık derecesini seçiniz"
            },
            'customers[0].Gender': {
                required: "Lütfen cinsiyet seçiniz"
            },
            'customers[0].DateOfBirth': {
                required: "Doğum Tarihi boş olamaz",
                date: "Geçerli bir tarih giriniz"
            },
            'customers[0].JobId': {
                required: "Lütfen meslek seçiniz",
                notOnlyZero: "Lütfen meslek seçiniz"
            },
            'customers[0].CountryId': {
                required: "Lütfen ülke seçiniz",
                notOnlyZero: "Lütfen ülke seçiniz"
            },
            'customers[0].NationalityId': {
                required: "Lütfen uyruk seçiniz",
                notOnlyZero: "Lütfen uyruk seçiniz"
            },
            'customers[0].CityId': {
                required: "Lütfen il seçiniz",
                notOnlyZero: "Lütfen il seçiniz"
            },
            'customers[0].DistrictId': {
                required: "Lütfen ilçe seçiniz",
                notOnlyZero: "Lütfen ilçe seçiniz"
            },
            'customers[0].Height': {
                required: "Boy boş olamaz",
                number: "Geçerli bir boy giriniz",
                notOnlyZero: "Boy sıfır olamaz"
            },
            'customers[0].Weight': {
                required: "Kilo boş olamaz",
                number: "Geçerli bir kilo giriniz",
                notOnlyZero: "Kilo sıfır olamaz"
            },
            'customers[0].Address': {
                required: "Adres boş olamaz",
                minlength: "Adres minumum 10 karakter uzunluğunda olmalıdır",
                maxlength: "Adres maximum 400 karakter uzunluğunda olmalıdır"
            }
        }
    });

    bupa.prepareSubmit("#customerForm", function () {
        bupa.ajaxPost("/Policy/Customer_Save", $("#customerForm"), $("#customerForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                bupa.goto('/Policy/Offer');
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });

    bupa.prepareSubmit("#offerForm", function () {
        bupa.ajaxPost("/Policy/CalculatePhonePolicy", $("#imeiForm"), $("#imeiForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                var data = result.data;

                if (history && history.pushState) {
                    history.pushState({}, document.title, "/teklif-al?id=" + data.refId + "&refToken=" + encodeURIComponent(data.refToken) + "#s2");
                }

                bupa.goto('/Policy/Installment');
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });

    bupa.prepareSubmit("#installmentForm", function () {
        bupa.ajaxPost("/Policy/CalculatePhonePolicy", $("#imeiForm"), $("#imeiForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                var data = result.data;

                if (history && history.pushState) {
                    history.pushState({}, document.title, "/teklif-al?id=" + data.refId + "&refToken=" + encodeURIComponent(data.refToken) + "#s2");
                }

                bupa.goto('/Policy/Payment');
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });

    $("#paymentForm").validate({
        rules: {
            CardHolderName: { required: true },
            CardNumber: { required: true },
            Expiration: { required: true },
            CVC: { required: true }
        },
        messages: {
            CardHolderName: {
                required: "Kart sahibi boş olamaz"
            },
            CardNumber: {
                required: "Kart numarası boş olamaz"
            },
            Expiration: {
                required: "Son kullanım tarihi boş olamaz"
            },
            CVC: {
                required: "Cvc boş olamaz"
            }
        }
    });

    if ($("#paymentForm")) {
        $("#paymentForm").find("#CardNumber").mask("9999-9999-9999-9999", { placeholder: " " });
        $("#paymentForm").find("#Expiration").mask("99/99", { placeholder: " " });
        $("#paymentForm").find("#CVC").mask("999", { placeholder: " " });
    }

    bupa.prepareSubmit("#paymentForm", function () {
        bupa.ajaxPost("/Policy/CreatePhonePolicy", $("body"), $("#paymentForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                var data = result.data;
                bupa.showMask("body", "Ödeme işlemi başarılı. <br /> Yönlendiriliyorsunuz. Lütfen bekleyiniz...");
                bupa.goto('/Policy/PaymentDone');
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });

    $("#Expiration").change(function () {
        var value = $(this).val();
        if (value && value != "") {
            var parts = value.split("/");
            if (parts.length >= 2) {
                $("#ExpirationMonth").val(parts[0].trim());
                $("#ExpirationYear").val("20" + parts[1].trim());
            }
        }
        else {
            $("#ExpirationMonth").val("");
            $("#ExpirationYear").val("");
        }
    });
});

function changeCustomerType(item) {
    if ($(item).val() == 0) {
        $(".companyArea").addClass("d-none");
        $(".customerArea").removeClass("d-none");
    }
    else {
        $(".companyArea").removeClass("d-none");
        $(".customerArea").addClass("d-none");
    }
}

function fillCities(e, id) {
    if (!id && !e) {
        return;
    }

    var targetElement = "";
    var countryId = 0;
    if (!e) {
        countryId = id;
        targetElement = e;
    }
    else {
        countryId = $(e).val();
        if ($(e).attr("name").split(".").length > 1) {
            targetElement = "select[name='" + $(e).attr("name").split(".")[0] + ".CityId" + "']";
        }
        else {
            targetElement = 'select[name="CityId"]';
        }
    }
    bupa.ajaxGet('/Policy/GetCities', targetElement, { countryId: countryId }, function (res) {
        if (res.hasError) {
            bupa.alert('danger', 'Şehirler çekilirken hata oluştu.');
            return false;
        }

        $(targetElement).html(null);
        $(targetElement).append($("<option>", { value: "0", text: "Seçiniz" }));

        $.each(res.data, function (i, e) {
            $(targetElement).append($("<option>", { value: e.id, text: e.name }));
        });
    });
}

function fillDistricts(e, id) {
    if (!id && !e) {
        return;
    }

    var targetElement = "";
    var cityId = 0;
    if (id) {
        cityId = id;
        targetElement = e;
    }
    else {
        cityId = $(e).val();
        if ($(e).attr("name").split(".").length > 1) {
            targetElement = "select[name='" + $(e).attr("name").split(".")[0] + ".DistrictId" + "']";
        }
        else {
            targetElement = 'select[name="DistrictId"]';
        }
    }
    bupa.ajaxGet('/Policy/GetDistricts', targetElement, { cityId: cityId }, function (res) {
        if (res.hasError) {
            bupa.alert('danger', 'İlçeler çekilirken hata oluştu.');
            return false;
        }

        $(targetElement).html(null);
        $(targetElement).append($("<option>", { value: "0", text: "Seçiniz" }));

        $.each(res.data, function (i, e) {
            $(targetElement).append($("<option>", { value: e.id, text: e.name }));
        });
    });
}
