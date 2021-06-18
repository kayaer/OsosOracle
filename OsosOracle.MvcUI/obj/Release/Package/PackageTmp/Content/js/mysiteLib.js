
var baseUrl = "http://yonca.elektromed.com.tr/Home/GetSession";
if (location.hostname == "localhost") {
     baseUrl = "http://localhost:12735/Home/GetSession";
}

var sessionInformation = {};

function GetSession(url) {

    var data = dataTableLanguage;
    $.ajax({
        url: url,
        dataType: "json",
        method: "GET",
        async: false,
        success: function (jqXHR) {

            if (jqXHR.Dil == "1") {
                data = englishLanguage;
              
            } else if (jqXHR.Dil == "2") {
                data = dataTableLanguage;
               
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {

            ajaxMesajGoster2(xhr.responseJSON.Mesaj, "Hata oluştu", xhr.responseJSON.RedirectUrl);
        }
    });
    return data;
}

//var getsessionInformation=function GetSessionInformation(callback) {
    
//    $.ajax({
//        url: baseUrl,
//        dataType: "json",
//        method: "GET",
//        async: false,
//        success: callback,
//        error: function (xhr, ajaxOptions, thrownError) {

           
//        }
//    });
   
//}

//Callback senkron kullanım örnegi
//getsessionInformation(function (data) {

//    console.log(data);
//});

//internet explorer desteklemiyor
//async function GetSessionInformation() {

//    var response = await fetch(baseUrl);
//    return response.json();

//    //$.ajax({
//    //    url: baseUrl,
//    //    dataType: "json",
//    //    method: "GET",
//    //    async: false,
//    //    success: callback,
//    //    error: function (xhr, ajaxOptions, thrownError) {


//    //    }
//    //});

//}


function GetSessionInformation() {

    const xhr = new XMLHttpRequest();

    xhr.open("GET", baseUrl);

    xhr.send();

    xhr.onload = function () {

        if (this.status == 200) {
            const response = JSON.parse(this.responseText);
           
            sessionInformation = response;

            if (sessionInformation.Dil == "1") {
                datepickerLang = 'en';
            } else {
                datepickerLang = 'tr';
            }
          
        }
    }

}

GetSessionInformation();

