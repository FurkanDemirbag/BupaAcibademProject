$().ready(function () {
    bupa.prepareSubmit("#filterForm", function () {
        if ($("#PolicyIsDone").is(":checked")) {
            $("#PolicyIsDone").attr("value", "true");
        }
        bupa.ajaxPost("/Policy/FilterPolicies", $("#filterForm"), $("#filterForm").serialize(), function (result) {
            if (result.hasError) {
                bupa.alert("danger", bupa.resultError(result));
            }
            else {
                var targetElement = $("table tbody");
                $(targetElement).html(null);

                $.each(result.data.list, function (i, e) {
                    var tr = $("<tr>");
                    $(tr).append("<th scope='row'>" + (i+1) + "</th>");
                    $(tr).append("<td>" + e.insurerName + "</td>");
                    $(tr).append("<td>" + e.insurerSurname + "</td>");
                    $(tr).append("<td>" + GetProximityType(e.proximityType) + "</td>");
                    $(tr).append("<td>" + e.tc + "</td>");
                    $(tr).append("<td>" + e.name + "</td>");
                    $(tr).append("<td>" + e.surname + "</td>");
                    $(tr).append("<td>" + getDateTime(e.dateOfBirth) + "</td>");
                    $(tr).append("<td>" + e.offerNumber + "</td>");
                    $(tr).append("<td>" + e.policyNumber + "</td>");
                    $(tr).append("<td>" + e.productName + "</td>");
                    $(tr).append("<td>" + e.installment + "</td>");
                    $(tr).append("<td>" + parseFloat(e.price).toFixed(2) + "</td>");
                    $(tr).append("<td>" + getDateTime(e.policyStartDate) + "</td>");
                    $(tr).append("<td>" + getDateTime(e.policyEndDate) + "</td>");
                    $(tr).append("<td>" + e.policyIsDone + "</td>");

                    var td = $("<td>");
                    if (!e.policyIsDone) {
                        $(td).append('<button type="button" class="btn btn-sm btn-success" onclick="javascript:updatePolicyState(@(item.PolicyId),true,false)"><i class="fa fa-check"></i></button>');
                    }
                    $(td).append('<button type="button" class="btn btn-sm btn-danger" onclick="javascript:updatePolicyState(@(item.PolicyId),false,true)"><i class="fa fa-trash"></i></button>');
                    $(tr).append(td);
                    $(targetElement).append(tr);
                });
            }
        }, function () {
            bupa.alert("danger", "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
        });
    });
});

function updatePolicyState(policyId, confirmRequest, deleteRequest) {
    if (!policyId) {
        return;
    }

    bupa.ajaxGet('/Policy/UpdatePolicyState', 'body', { policyId: policyId, confirmRequest: confirmRequest, deleteRequest: deleteRequest }, function (res) {
        if (res.hasError) {
            bupa.alert('danger', 'Poliçe güncellenirken hata oluştu.');
            return false;
        }

        location.reload();
    });
}

function getDateTime(date) {
    var day = date.split("-")[2].split("T")[0];
    var month = date.split("-")[1];
    var year = date.split("-")[0];
    return (day + "/" + month + "/" + year);
}

function GetProximityType(type) {
    if (type == 0) {
        return "Kendisi";
    }
    else if (type == 1) {
        return "Eşi";
    }
    else if (type == 2){
        return "Çoçuğu";
    }
    else{
        return "Diğer";
    }
}
