
var baseUrl = "http://localhost:12735/Home/GetSession";

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

