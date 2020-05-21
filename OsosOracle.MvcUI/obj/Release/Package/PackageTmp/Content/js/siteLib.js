//map init
function initMap(mapid, startLng, startLat, startZoom) {


    mapid = typeof mapid !== 'undefined' ? mapid : "map";
    startLng = typeof startLng !== 'undefined' ? startLng : 32.85411;
    startLat = typeof startLat !== 'undefined' ? startLat : 39.92077;
    startZoom = typeof startZoom !== 'undefined' ? startZoom : 11;

    if (document.getElementById(mapid) == null) {
        alert("div " + mapid + " bulunamadı");
    }

    startLng = parseFloat(startLng);
    startLat = parseFloat(startLat);


    //kullanici lokasyonu
    var home = new google.maps.LatLng(startLat, startLng);

    var mapOptions = {
        zoom: startZoom,
        center: home,
        mapTypeId: google.maps.MapTypeId.SATELLITE_MAP
    };

    map = new google.maps.Map(document.getElementById(mapid), mapOptions);


}

function setAc(id, value) {

    var self = $("#_" + id);

    var targetId = self.data("target");
    var connectedWith = self.data("connectedwith");
    var connectedWith2 = self.data("connectedwith2");
    var connectedwithlist = self.data("connectedwithlist");

    var singleurl = self.data("ajaxdeger") + "?id=";

    //içi dolu ise ajax ile dolu set et
    var selectedId = value;
    $("#" + targetId).val(value);
    if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {


        if (self.val() === "") {

            var acRequest = $.ajax({
                url: singleurl + selectedId,
                method: "POST",
                async: true
            });


            acRequest.done(function (data) {

                self.val(data.text).trigger("change");


                self.prop('readonly', true);
            });

            acRequest.error(function (msg) {
                self.val("Yüklenemedi!!");
                self.prop('disabled', true).addClass("text-danger");

            });


        } else {
            self.prop('readonly', true);
        }
    }


    if (connectedWith !== undefined) {
        if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
            $("#_" + connectedWith).data("rootid", selectedId).addClass("connected");

        } else {
            $("#_" + connectedWith).prop('disabled', true);
        }
    }
    if (targetId !== undefined) {
        $("#_" + connectedWith).data("rootid", selectedId).addClass("connected").prop('disabled', false).focus();

    }

    if (connectedWith2 !== undefined) {
        if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
            $("#_" + connectedWith2).data("rootid2", selectedId);
        } else {
            $("#_" + connectedWith2).prop('disabled', true);
        }
    }



    if (connectedwithlist !== undefined) {

        var arr = connectedwithlist.split(',');

        $.each(arr, function (index, value) {

            if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                $("#_" + value).data("rootid", selectedId).addClass("connected");

            } else {
                $("#_" + value).prop('disabled', true);
            }

        });
    }
    //if (targetId !== undefined) {

    //    var arr = connectedwithlist.split(',');

    //    $("#_" + arr[0]).data("rootid", selectedId).addClass("connected").prop('disabled', false).focus();

    //}

}


function clearAc(id) {

    // e.preventDefault();

    var target = id;
    var self = $("#_" + target);
    var connectedId = self.data("connectedwith");
    var connectedId2 = self.data("connectedwith2");
    var connectedwithlist = self.data("connectedwithlist");

    self.val('').prop("readonly", false);//.removeClass("autocomplete_loading").focus();
    $("#" + target).val("");


    if (self.data("clearcallback") !== undefined) {
        window[self.data("clearcallback")](e);
    }

    if (connectedId !== undefined) {
        //bağlantılı ise bağlı olduğu ototamamları da sıfırla
        $("#ac_clear_" + connectedId + " ").trigger("click");
        $("#_" + connectedId).prop('disabled', true);

    }
    if (connectedId2 !== undefined) {
        //bağlantılı ise bağlı olduğu ototamamları da sıfırla
        $("#ac_clear_" + connectedId2 + " ").trigger("click");
        $("#_" + connectedId2).prop('disabled', true);

    }

    if (connectedwithlist !== undefined) {

        var arr = connectedwithlist.split(',');

        $.each(arr, function (index, value) {

            //bağlantılı ise bağlı olduğu ototamamları da sıfırla
            $("#ac_clear_" + value + " ").trigger("click");
            $("#_" + value).prop('disabled', true);

        });

    }

    $("#ac_selection_" + target).html("");

}


var mainModal;

//nebi deneme amaclı kapattım
//$(window).bind('beforeunload', function () {
//    blockUI();
//    // App.startPageLoading();
//});


$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        //xhr.overrideMimeType("text/plain; charset=x-user-defined");
        //console.log(xhr);
        //console.log(settings);
        //      console.log(settings.data);
        //        console.log(btoa(unescape(encodeURIComponent(settings.data))));
        //        settings.data ="d="+ btoa(unescape(encodeURIComponent(settings.data)));
    }
});





$(document).ready(function () {


    if (typeof (Storage) !== "undefined") {//tıklanan menüyü sayfa geçişinde seçili getirme

        var activeMenu = sessionStorage.getItem('activeMenu');
        $("#" + activeMenu).parents("li.nav-item").addClass("active open");

        $("#mainMenu a.nav-link:not(.nav-toggle)").click(function (e) {
            sessionStorage.setItem('activeMenu', this.id);
        });
    }


    //genel ajax hataları 
    //hatalarda mutlaka ajaxsonuc objesi dönmeli
    $(document).ajaxError(function (event, jqxhr, settings, thrownError) {
        unblockUI();
        var responseJson = jQuery.parseJSON(jqxhr.responseText);
        bootbox.alert({
            //title: responseJson.Durum === "1" ? "Bilgi" : "Uyarı",//Dil sorunu yüzünden değiştirdim
            title: responseJson.Baslik,
            message: responseJson.Mesaj,
            locale: 'tr',
            callback: function (result) {
                if (responseJson.RedirectUrl != "") {
                    window.location = responseJson.RedirectUrl;
                }

            }
        });
    });






    ///////////////////////////
    /*
    GENEL işlemler
  */
    ///////////////////////
    /*Onaylama*/
    //Onay classı verilen objelerin click eventinde objenin title özelliğini soru olarak gösterir.


    $("body").on("click", '.onay,.confirm', function (e) {

        var a = $(this);
        var soru = a.attr("title");
        if (soru !== undefined) {
            if (soru.length !== 0) {
                soru = soru + " işlemi gerçekleştirilecektir.";
            }
            else if (a.text().length > 0)
                soru = a.text() + " işlemi gerçekleştirilecektir.";
            else
                soru = a.data("original-title") + " işlemi gerçekleştirilecektir.";
        }
        else {
            if (a.text().length > 0)
                soru = a.text() + " işlemi gerçekleştirilecektir.";
            else
                soru = a.data("original-title") + " işlemi gerçekleştirilecektir.";
        }

        if (confirm(soru + " Devam etmek istiyor musunuz?")) { return true; }
        else { e.preventDefault(); return false; }
    });



    $(document).on("click", '.ajax', function (e) {
        e.preventDefault();
        blockUI();
        var a = $(this);
        var url = $(a).attr("href");
        var method = a.hasClass("post") ? "POST" : "GET";
        var request = $.ajax({
            url: url,
            method: method,
            async: true, dataType: "json"
        });

        request.done(function (jqXHR, textStatus) {
            if (jqXHR.Durum === 1) {
                if (jqXHR.RedirectUrl !== "") {
                    window.location = jqXHR.RedirectUrl;
                } else {
                    unblockUI();
                    ajaxMesajGoster(jqXHR.Mesaj);
                }
            } else {
                unblockUI();
                ajaxMesajGoster(jqXHR.Mesaj);
            }
        });
        /*
        request.fail(function (jqXHR, textStatus) {
            unblockUI();
            var msg = jQuery.parseJSON(jqXHR.responseText);
            ajaxMesajGoster(msg.Mesaj);
        });*/


    });

    //iframe modal
    $(document).on("click", '.frameModalizer', function (e) {
        e.preventDefault();
        var a = $(this);
        var modalHeight = a.data("height") ? a.data("height") : '400';

        var url = $(a).attr("href");
        if (url.contains("?")) {
            url = url + "&layout=modal";
        } else {
            url = url + "?layout=modal";
        }

        var mymodal = bootbox.dialog({
            title: a.data("original-title"),
            backdrop: true,
            closeButton: true,
            animate: true,
            className: "my-modal",
            size: "large",
            message: '<iframe style="width:100%;height:99%;min-height:' + modalHeight + 'px;border:0;"  src="' + url + '"></iframe>'
        }).resizable();

        $(mymodal).draggable({
            handle: ".modal-header"
        });
    });

    //modal pencere görüntüleme
    $(document).on("click", '.modalizer', function (e) {
        e.preventDefault();

        blockUI();

        var a = $(this);
        var modalTitle = a.text();

        if (a.data("original-title") !== "" && a.data("original-title") !== undefined) {
            modalTitle = a.data("original-title");
        }
        var url = $(a).attr("href");
        if (url.contains("?")) {
            url = url + "&layout=modal";
        } else {
            url = url + "?layout=modal";
        }

        var request = $.ajax({
            url: url,
            method: "GET",
            async: true,
            dataType: "html"
        });

        request.done(function (msg) {
            unblockUI();
            mainModal = bootbox.dialog({
                title: modalTitle,
                backdrop: true,
                closeButton: true,
                animate: true,
                className: "mainModal",
                size: "large",
                message: msg
            });



            $('.mainModal .modal-content')
                .draggable({
                    cancel: ".bootbox-body",
                    handle: ".modal-header"
                })
                .resizable({
                    minWidth: 350,
                    minHeight: 450,
                    alsoResize: '.mainModal .bootbox-body'
                });

        });

        //request.error(function (xhr) {
        //    unblockUI();



        //});


    });

    $(document).on("click", ".sendToPrinter", function (e) {
        e.preventDefault();
        window.print();
    });


    $(document).on("click", "a.report", function () {
        blockUI();
        $.fileDownload($(this).attr('href'), {
            successCallback: function (url) {
                unblockUI();
            },
            failCallback: function (responseHtml, url) {
                unblockUI();
                ajaxMesajGoster(responseHtml, "Bişeyler ters gitti!");
            }
        });
        return false; //this is critical to stop the click event which will trigger a normal file download!
    });


    $(document).on("submit", "form.report", function (e) {
        blockUI();
        $.fileDownload($(this).prop('action'), {
            successCallback: function (url) {
                unblockUI();
            },
            failCallback: function (responseHtml, url) {
                unblockUI();
                ajaxMesajGoster(responseHtml, "Bişeyler ters gitti! ");
            },
            httpMethod: "POST",
            data: $(this).serialize()
        });
        e.preventDefault(); //otherwise a normal form submit would occur
    });


    $(document).on('click', '#ModalPopUp a', function () {

        var url = $(this).attr("href");
        if (url.contains("?")) {
            url = url + "&layout=modal";
        } else {
            url = url + "?layout=modal";
        }
        $(this).attr("href", url);


    });


    $("#ModalPopUp form").each(function (index) {

        var url = $(this).attr("action");
        if (!url.contains("layout")) {
            if (url.contains("?")) {
                url = url + "&layout=modal";
            } else {
                url = url + "?layout=modal";
            }
            $(this).attr("action", url);
        }
    });


});//end of document.ready

function split(val) {
    return val.split(/,\s*/);
}
function extractLast(term) {
    return split(term).pop();
}

function pageInit(scope) {


    Gostergeler = new Array(17);
    for (i = 0; i < Gostergeler.length; ++i)
        Gostergeler[i] = new Array(10);

    Gostergeler[1][1] = "1320";
    Gostergeler[1][2] = "1380";
    Gostergeler[1][3] = "1440";
    Gostergeler[1][4] = "1500";
    Gostergeler[2][1] = "1155";
    Gostergeler[2][2] = "1210";
    Gostergeler[2][3] = "1265";
    Gostergeler[2][4] = "1320";
    Gostergeler[2][5] = "1380";
    Gostergeler[2][6] = "1440";
    Gostergeler[3][1] = "1020";
    Gostergeler[3][2] = "1065";
    Gostergeler[3][3] = "1110";
    Gostergeler[3][4] = "1155";
    Gostergeler[3][5] = "1210";
    Gostergeler[3][6] = "1265";
    Gostergeler[3][7] = "1320";
    Gostergeler[3][8] = "1380";
    Gostergeler[4][1] = "915";
    Gostergeler[4][2] = "950";
    Gostergeler[4][3] = "985";
    Gostergeler[4][4] = "1020";
    Gostergeler[4][5] = "1065";
    Gostergeler[4][6] = "1110";
    Gostergeler[4][7] = "1155";
    Gostergeler[4][8] = "1210";
    Gostergeler[4][9] = "1265";
    Gostergeler[5][1] = "835";
    Gostergeler[5][2] = "865";
    Gostergeler[5][3] = "895";
    Gostergeler[5][4] = "915";
    Gostergeler[5][5] = "950";
    Gostergeler[5][6] = "985";
    Gostergeler[5][7] = "1020";
    Gostergeler[5][8] = "1065";
    Gostergeler[5][9] = "1110";
    Gostergeler[6][1] = "760";
    Gostergeler[6][2] = "785";
    Gostergeler[6][3] = "810";
    Gostergeler[6][4] = "835";
    Gostergeler[6][5] = "865";
    Gostergeler[6][6] = "895";
    Gostergeler[6][7] = "915";
    Gostergeler[6][8] = "950";
    Gostergeler[6][9] = "958";
    Gostergeler[7][1] = "705";
    Gostergeler[7][2] = "720";
    Gostergeler[7][3] = "740";
    Gostergeler[7][4] = "760";
    Gostergeler[7][5] = "785";
    Gostergeler[7][6] = "810";
    Gostergeler[7][7] = "835";
    Gostergeler[7][8] = "865";
    Gostergeler[7][9] = "895";
    Gostergeler[8][1] = "660";
    Gostergeler[8][2] = "675";
    Gostergeler[8][3] = "690";
    Gostergeler[8][4] = "705";
    Gostergeler[8][5] = "720";
    Gostergeler[8][6] = "740";
    Gostergeler[8][7] = "760";
    Gostergeler[8][8] = "785";
    Gostergeler[8][9] = "810";
    Gostergeler[9][1] = "620";
    Gostergeler[9][2] = "630";
    Gostergeler[9][3] = "645";
    Gostergeler[9][4] = "660";
    Gostergeler[9][5] = "675";
    Gostergeler[9][6] = "690";
    Gostergeler[9][7] = "705";
    Gostergeler[9][8] = "720";
    Gostergeler[9][9] = "740";
    Gostergeler[10][1] = "590";
    Gostergeler[10][2] = "600";
    Gostergeler[10][3] = "610";
    Gostergeler[10][4] = "620";
    Gostergeler[10][5] = "630";
    Gostergeler[10][6] = "645";
    Gostergeler[10][7] = "660";
    Gostergeler[10][8] = "675";
    Gostergeler[10][9] = "690";
    Gostergeler[11][1] = "560";
    Gostergeler[11][2] = "570";
    Gostergeler[11][3] = "580";
    Gostergeler[11][4] = "590";
    Gostergeler[11][5] = "600";
    Gostergeler[11][6] = "610";
    Gostergeler[11][7] = "620";
    Gostergeler[11][8] = "630";
    Gostergeler[11][9] = "645";
    Gostergeler[12][1] = "545";
    Gostergeler[12][2] = "550";
    Gostergeler[12][3] = "555";
    Gostergeler[12][4] = "560";
    Gostergeler[12][5] = "570";
    Gostergeler[12][6] = "580";
    Gostergeler[12][7] = "590";
    Gostergeler[12][8] = "600";
    Gostergeler[12][9] = "610";
    Gostergeler[13][1] = "530";
    Gostergeler[13][2] = "535";
    Gostergeler[13][3] = "540";
    Gostergeler[13][4] = "545";
    Gostergeler[13][5] = "550";
    Gostergeler[13][6] = "555";
    Gostergeler[13][7] = "560";
    Gostergeler[13][8] = "570";
    Gostergeler[13][9] = "580";
    Gostergeler[14][1] = "515";
    Gostergeler[14][2] = "520";
    Gostergeler[14][3] = "525";
    Gostergeler[14][4] = "530";
    Gostergeler[14][5] = "535";
    Gostergeler[14][6] = "540";
    Gostergeler[14][7] = "545";
    Gostergeler[14][8] = "550";
    Gostergeler[14][9] = "555";
    Gostergeler[15][1] = "500";
    Gostergeler[15][2] = "505";
    Gostergeler[15][3] = "510";
    Gostergeler[15][4] = "515";
    Gostergeler[15][5] = "520";
    Gostergeler[15][6] = "525";
    Gostergeler[15][7] = "530";
    Gostergeler[15][8] = "535";
    Gostergeler[15][9] = "540";
    Gostergeler[16][1] = "70";
    Gostergeler[16][2] = "75";
    Gostergeler[16][3] = "80";
    Gostergeler[16][4] = "85";
    Gostergeler[16][5] = "90";
    Gostergeler[16][6] = "95";
    Gostergeler[16][7] = "100";
    Gostergeler[16][8] = "105";
    Gostergeler[16][9] = "110";


    $("input.Derece").keypress(function (e) {
        if ((e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) || (e.which == 17 || e.which == 78)) {
            return false;
        }
        else {
            if ((e.which == 48) && $(this).val().length == 0) //ilk hanede sıfır yazılmayacak
                return false;
        }

        if ($(this).val() == "1" && (e.which < 48 || e.which > 54) && e.which != 8 && e.which != 0)
            return false;

        //if ($(this).val().length != 0 && $(this).val() != "1" && (e.which >= 48 && e.which <= 57))
        //    return false;

        //GostergeYaz($(this).attr('id'));
    });

    $("input.Kademe").keypress(function (e) {
        if ((e.which != 8 && e.which != 0 && (e.which < 49 || e.which > 57)) || (e.which == 17 || e.which == 78)) {
            return false;
        }

        //GostergeYaz($(this).attr("id").replace('Kademe', 'Derece'));
    });

    $("input.Derece").blur(function (e) {
        GostergeYaz($(this).attr('id'));
    });

    $("input.Kademe").blur(function (e) {
        GostergeYaz($(this).attr("id").replace('Kademe', 'Derece'));
    });

    function GostergeYaz(obj) {

        if ($('#' + obj).val() == "" || $('#' + obj.replace('Derece', 'Kademe')).val() == "")
            $('#' + obj.replace('Derece', 'Gosterge')).val("");
        else
            $('#' + obj.replace('Derece', 'Gosterge')).val(Gostergeler[$('#' + obj).val()][$('#' + obj.replace('Derece', 'Kademe')).val()]);

    }










    $.validator.unobtrusive.parse($("form", scope));
    $.validator.setDefaults({ ignore: null });
    $("input:hidden[value=0]", scope).val("");



    //$(".html-edit").summernote({
    //    airMode: true,
    //    popover: {
    //        air: [

    //          ['font', ['bold', 'italic', 'superscript', 'subscript']],
    //        ]
    //    },
    //    callbacks: {
    //        onChange: function (contents) {


    //            // (conte)

    //            //l var html = $('<div/>').text(contents).html();

    //            $("." + $(this).data("target")).val(contents);
    //        }
    //    }
    //});
    $(".html-edit").summernote({
        height: 250,   //set editable area's height
        codemirror: { // codemirror options
            theme: 'monokai'
        },
        //callbacks: {
        //    onChange: function (contents) {
        //        $("." + $(this).data("target")).val(contents);
        //    }
        //}
    });

    $(".html-edit .Light").summernote({
        height: 250,   //set editable area's height
        codemirror: { // codemirror options
            theme: 'monokai'
        },
        //callbacks: {
        //    onChange: function (contents) {
        //        $("." + $(this).data("target")).val(contents);
        //    }
        //}
    });

    var keyboard1 = {
        'layout': [
            // alphanumeric keyboard type
            // text displayed on keyboard button, keyboard value, keycode, column span, new row
            [
                [
                    ['`', '`', 192, 0, true], ['1', '1', 49, 0, false], ['2', '2', 50, 0, false], ['3', '3', 51, 0, false], ['4', '4', 52, 0, false], ['5', '5', 53, 0, false], ['6', '6', 54, 0, false],
                    ['7', '7', 55, 0, false], ['8', '8', 56, 0, false], ['9', '9', 57, 0, false], ['0', '0', 48, 0, false], ['-', '-', 189, 0, false], ['=', '=', 187, 0, false],
                    ['q', 'q', 81, 0, true], ['w', 'w', 87, 0, false], ['e', 'e', 69, 0, false], ['r', 'r', 82, 0, false], ['t', 't', 84, 0, false], ['y', 'y', 89, 0, false], ['u', 'u', 85, 0, false],
                    ['i', 'i', 73, 0, false], ['o', 'o', 79, 0, false], ['p', 'p', 80, 0, false], ['[', '[', 219, 0, false], [']', ']', 221, 0, false], ['&#92;', '\\', 220, 0, false],
                    ['a', 'a', 65, 0, true], ['s', 's', 83, 0, false], ['d', 'd', 68, 0, false], ['f', 'f', 70, 0, false], ['g', 'g', 71, 0, false], ['h', 'h', 72, 0, false], ['j', 'j', 74, 0, false],
                    ['k', 'k', 75, 0, false], ['l', 'l', 76, 0, false], [';', ';', 186, 0, false], ['&#39;', '\'', 222, 0, false], ['Enter', '13', 13, 3, false],
                    ['Shift', '16', 16, 2, true], ['z', 'z', 90, 0, false], ['x', 'x', 88, 0, false], ['c', 'c', 67, 0, false], ['v', 'v', 86, 0, false], ['b', 'b', 66, 0, false], ['n', 'n', 78, 0, false],
                    ['m', 'm', 77, 0, false], [',', ',', 188, 0, false], ['.', '.', 190, 0, false], ['/', '/', 191, 0, false], ['Shift', '16', 16, 2, false],
                    ['Bksp', '8', 8, 3, true], ['Space', '32', 32, 12, false], ['asd', '46', 46, 3, false], ['Vazgeç', '27', 27, 3, false]
                ]
            ]
        ]
    }
    $('input.keyboard').initKeypad({ 'keyboardLayout': keyboard1 });

    $("div.ajaxForm form, form.ajaxForm", scope).on("submit", function (event) {
        event.preventDefault();
        var frm = $(this);
        if (!frm.valid()) {
            return false;
        }

        blockUI();

        var url = frm.attr('action');
        if (url.contains("?")) {
            url = url + "&layout=modal";
        } else {
            url = url + "?layout=modal";
        }


        var data = frm.serializeObject();
        var request = $.ajax({
            url: url,
            method: frm.attr('method'),
            data: data,
            dataType: "json"
        });

        request.done(function (jqXHR, textStatus) {
            if (jqXHR.Durum === 1) {
                if (jqXHR.RedirectUrl !== "") {
                    window.location = jqXHR.RedirectUrl;
                } else {
                    unblockUI();
                    ajaxMesajGoster(jqXHR.Mesaj);
                }
            } else {
                unblockUI();
                ajaxMesajGoster(jqXHR.Mesaj);
            }
        });


        return false;
    });


    $('form .cancel', scope).click(function (e) {
        e.preventDefault();
        if (mainModal !== undefined) {
            mainModal.modal('hide');

            return false;
        } else {
            return true;

        }
    });

    $('div.ajaxForm .cancel', scope).click(function (e) {
        e.preventDefault();
        if (mainModal !== undefined) {
            mainModal.modal('hide');

            return false;
        } else {
            return true;

        }
    });



    //zorunlu alanlara kırmızı border ekleme
    $("[data-val='true']", scope).addClass("input-validation-error");


    $(scope).tooltip({
        selector: '[title]',
        placement: "auto"
    });


    $(".disabled", scope).attr("disabled", "disabled");
    $(".readonly", scope).attr("readonly", "readonly");

    //sayfa içi help işlemleri
    if ($('span.help-block', scope).length) {
        $('span.help-block', scope).each(function () {
            var b = $(this);
            var a = $(' <i class="fa fa-question-circle text-success" title="Yardım"></i> ');
            $(a).popover({
                placement: 'right',
                offset: 15,
                trigger: 'click',
                delay: { show: 100, hide: 300 },
                html: true,
                content: b.html()
            }).css("cursor", "pointer");

            b.html(a).removeClass("help-block");

        });
    }
    if ($('span.commentcount', scope).length) {
        $('span.commentcount', scope).each(function () {
            var b = $(this);
            $.ajax({
                method: "POST",
                url: "/Yorum/GetYorumCount",
                data: { kaynakTip: b.data("kaynaktip"), kaynakId: b.data("kaynakid") }
            })
                .done(function (msg) {
                    b.html(msg.count);
                });


        });
    }









    //integer veri girişi
    $("input.sayi, input.int", scope).keypress(function (e) {

        var self = $(this);
        //eksi
        if (self.hasClass("eksi")) {
            if (e.which === 45 && self.val().contains("-")) {
                return false;
            }
            if (e.which !== 45 && e.keyCode !== 37 && e.keyCode !== 39 && e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }
        else {
            if (e.keyCode !== 37 && e.keyCode !== 39 && e.which !== 8 && e.which !== 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }


    });


    //integer veri girişi
    $("input.sayi, input.int", scope).keyup(function (e) {

        var self = $(this);
        var val = self.val() !== "" ? self.val() : "0";

        if (val.contains("-")) {
            if (val.charAt(0) !== "-") {
                val = val.replace("-", "");
                self.val(val);
            }
        }


    });


    ////decimal veri girişi


    //decimal kabul etme
    $("input.decimal", scope).keypress(function (e) {

        var self = $(this);
        //Tek Virgül
        if (e.which === 44 && self.val().contains(",")) {
            return false;
        }

        if (self.hasClass("eksi")) {
            //eksi
            if (e.which === 45 && self.val().contains("-")) {
                return false;
            }
            //45 eksi
            if (e.which !== 44 && e.which !== 45 && e.which !== 8 && e.which !== 0 && e.keyCode !== 37 && e.keyCode !== 39 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        } else {

            //& e.which != 46 nokta // 36 39 arrow keys
            if (e.which !== 44 && e.which !== 8 && e.which !== 0 && e.keyCode !== 37 && e.keyCode !== 39 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        }


    });

    //decimal gösterme
    $("input.decimal", scope).keyup(function (e) {
        var self = $(this);
        $("span", self.prev()).remove();
        Decimal.format = {
            decimalSeparator: ',',
            groupSeparator: '.',
            groupSize: 3,
            secondaryGroupSize: 3
        };
        var val = self.val() !== "" ? self.val() : "0";

        if (val.contains("-")) {
            if (val.charAt(0) !== "-") {
                val = val.replace("-", "");
                self.val(val);
            }
        }


        var disp = new Decimal(val.replace(",", "."));
        self.prev().append("<span> : " + disp.toFormat() + "</span>");
    });


    if ($("input.decimal", scope).length > 0) {

        $('input.decimal', scope).each(function (index) {
            var self = $(this);

            $("span", self.prev()).remove();
            Decimal.format = {
                decimalSeparator: ',',
                groupSeparator: '.',
                groupSize: 3,
                secondaryGroupSize: 3
            };


            var val = self.val() !== "" ? self.val() : "0";
            var disp = new Decimal(val.replace(",", "."));
            self.prev().append("<span> : " + disp.toFormat() + "</span>");
        });
    }



    if (jQuery().datepicker) {
        $('.datePicker', scope).datepicker({

            orientation: "left",
            autoclose: true,
            format: "dd.mm.yyyy",
            language: 'tr',
            todayHighlight: true,
            todayBtn: true,
            useCurrent: false

        });
    }
    if (jQuery().datetimepicker) {
        $('.dateTimePicker', scope).datetimepicker({


            isRTL: App.isRTL(),
            format: "dd.mm.yyyy hh:ii",
            language: 'tr',
            autoclose: true,
            pickerPosition: (App.isRTL() ? "bottom-right" : "bottom-left"),
            minuteStep: 5,
            todayHighlight: true,
            todayBtn: true,
            useCurrent: false
        });

    }


    /*datepicker
    $(".datePicker", scope).datetimepicker({
        format: "DD.MM.YYYY",
        locale: "tr",
        useCurrent: false
    });
    //dateTime Picker
    $(".dateTimePicker", scope).datetimepicker({
        format: "DD.MM.YYYY HH:mm",
        locale: "tr",
        useCurrent: false
    });
    //Time picker
    $(".timePicker", scope).datetimepicker({ format: "HH.mm" });
    */





    /////////////////////////////
    /*autocomplete*/
    /////////////////////////////

    $('input[type!=hidden],textarea,select', scope).floatlabel();






    if ($('.ac2', scope).length) {



        $('.ac2', scope)
            .each(function (index) {

                var self = $(this);
                var showDescription = self.data("showdesc");
                var isMultiple = self.data("multiple");
                var targetId = self.data("target");
                var connectedWith = self.data("connectedwith");
                var connectedWith2 = self.data("connectedwith2");
                var connectedwithlist = self.data("connectedwithlist");

                var singleurl = self.data("ajaxdeger") + "?id=";
                if (isMultiple === "True") {
                    singleurl = self.data("ajaxcokdeger") + "?idler=";
                }
                //içi dolu ise ajax ile dolu set et
                var selectedId = $("#" + targetId).val();
                if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {


                    if (self.val() === "") {

                        var acRequest = $.ajax({
                            url: singleurl + selectedId,
                            method: "GET",
                            async: true
                        });


                        acRequest.done(function (data) {
                            if (isMultiple === "True") {
                                $.each(data,
                                    function (i, item) {
                                        $("<span></span>")
                                            .addClass("badge badge-info")
                                            .data("vv", item.id)
                                            .text(item.text + " ")
                                            .append($("<i></i>")
                                                .addClass("fa fa-times")
                                                .click(function () {
                                                    var v = $("#" + targetId).val();
                                                    var vtr = $(this).parent().data("vv");
                                                    v = _.without(v, vtr);
                                                    $("#" + targetId).val(v.toString());
                                                    $(this).parent().remove();
                                                })
                                            )
                                            .appendTo($("#ac_selection_" + targetId));
                                        $("#ac_selection_" + targetId).append(" ");
                                    });
                            } else {
                                self.val(data.text).trigger("change");
                            }

                            self.prop('readonly', true);
                        });

                        acRequest.error(function (msg) {
                            self.val("Yüklenemedi!!");
                            self.prop('disabled', true).addClass("text-danger");

                        });


                    } else {
                        self.prop('readonly', true);
                    }
                }


                if (isMultiple === "True") {

                    self.on("keydown",
                        function (event) {
                            if (event.keyCode === $.ui.keyCode.TAB &&
                                $(this).autocomplete("instance").menu.active) {
                                event.preventDefault();
                            }
                        });

                }

                if (connectedWith !== undefined) {
                    if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                        $("#_" + connectedWith).data("rootid", selectedId).addClass("connected");

                    } else {
                        $("#_" + connectedWith).prop('disabled', true);
                    }
                }

                if (connectedWith2 !== undefined) {
                    if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                        $("#_" + connectedWith2).data("rootid2", selectedId);

                    } else {
                        $("#_" + connectedWith2).prop('disabled', true);
                    }
                }

                if (connectedwithlist !== undefined) {


                    var arr = connectedwithlist.split(',');

                    $.each(arr, function (index, value) {

                        if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                            $("#_" + value).data("rootid", selectedId).addClass("connected");

                        } else {
                            $("#_" + value).prop('disabled', true);
                        }

                    });
                }

                //     debugger;

                self.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: self.data("url"),
                            dataType: "json",
                            data: {
                                key: extractLast(request.term),//request.term
                                rootId: self.data("rootid"),
                                rootId2: self.data("rootid2")
                            },
                            success: function (data) {

                                if (data.acData !== undefined) {
                                    response($.map(data.acData,
                                        function (item) {
                                            return {
                                                label: item.description,
                                                value: item.text,
                                                id: item.id
                                            };
                                        }));
                                } else {
                                    response($.map(data,
                                        function (item) {
                                            return {
                                                label: item.description,
                                                value: item.text,
                                                id: item.id
                                            };
                                        }));
                                }
                            },
                            error: function (xhr, ajaxOptions, thrownError) {

                                ajaxMesajGoster2(xhr.responseJSON.Mesaj, "Hata oluştu", xhr.responseJSON.RedirectUrl);
                            }
                        });
                    },

                    minLength: 0,
                    //   multiselect: isMultiple === "True"?true:false,

                    select: function (event, ui) {


                        if (isMultiple === "True") {

                            var v = [];
                            if ($("#" + targetId).val() !== "")
                                v = split($("#" + targetId).val());

                            if (!_.contains(v, ui.item.id)) {

                                $("<span></span>")
                                    .addClass("badge badge-info")
                                    .data("vv", ui.item.id)
                                    .text(ui.item.value + " ")
                                    .append($("<i></i>")
                                        .addClass("fa fa-times")
                                        .click(function () {
                                            var v = split($("#" + targetId).val());
                                            var vtr = $(this).parent().data("vv");
                                            v = _.without(v, vtr);
                                            $("#" + targetId).val(v.toString());
                                            $(this).parent().remove();
                                        })
                                    )
                                    .appendTo($("#ac_selection_" + targetId));
                                $("#ac_selection_" + targetId).append(" ");
                                v.push(ui.item.id);


                                $("#" + targetId).val(v.toString());
                                self.val("");



                            }
                            self.val("");

                        }
                        else {
                            $("#" + targetId).val(ui.item.id);
                            self.val(ui.item.value).addClass("text-danger").prop("readonly", true);//.trigger("change");
                        }

                        if (self.data("callback") !== undefined) {
                            window[self.data("callback")](ui.item);
                        }

                        if (targetId !== undefined) {
                            $("#_" + connectedWith).data("rootid", ui.item.id).addClass("connected").prop('disabled', false).focus();
                            $("#_" + connectedWith).val("");
                            $("#" + connectedWith).val("");

                            $("#_" + connectedWith2).data("rootid2", ui.item.id);
                            $("#_" + connectedWith2).val("");
                            $("#" + connectedWith2).val("");

                            if (connectedwithlist !== undefined) {
                                var arr = connectedwithlist.split(',');

                                $.each(arr,
                                    function(index, value) {
                                        $("#_" + value)
                                            .data("rootid", ui.item.id)
                                            .addClass("connected")
                                            .prop('disabled', false)
                                            .focus();
                                        $("#_" + value).val("");
                                        $("#" + value).val("");
                                    });
                            }
                        }

                        self.blur();
                        return false;


                    }

                });

                self.focus(function () {
                    $(this).autocomplete("search", "");
                }).autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>").append(item.value).appendTo(ul);
                };

            });





        $(".autocomplete_clear", scope).click(function (e) {
            e.preventDefault();

            var target = $(this).data("target");
            var self = $("#_" + target);
            var connectedId = self.data("connectedwith");
            var connectedwithlist = self.data("connectedwithlist");

            self.val('').prop("readonly", false).removeClass("autocomplete_loading").focus();
            $("#" + target).val("");


            if (self.data("clearcallback") !== undefined) {
                window[self.data("clearcallback")](e);
            }

            if (connectedId !== undefined) {
                //bağlantılı ise bağlı olduğu ototamamları da sıfırla
                $("#ac_clear_" + connectedId + " ").trigger("click");
                $("#_" + connectedId).prop('disabled', true);

            }

            if (connectedwithlist !== undefined) {
                //bağlantılı ise bağlı olduğu ototamamları da sıfırla

                var arr = connectedwithlist.split(',');

                $.each(arr, function (index, value) {
                    $("#ac_clear_" + value + " ").trigger("click");
                    $("#_" + value).prop('disabled', true);
                });

            }

            $("#ac_selection_" + target).html("");
        });

        $(".listopener", scope).click(function (e) {
            e.preventDefault();

            var type = $(this).data("type");
            var target = $(this).data("target");
            var self = $("#_" + target);
            var url = self.data("url");
            var viewUrl = self.data("viewurl");
            var mymodal;

            if (type === "Tree") { //tree
                url = url.replace("AjaxAra", "AjaxAraTree");
                mymodal = bootbox.dialog({
                    title: "<i class='fa fa-sitemap'></i> Ağaçtan Seçiniz",
                    message: '<div id="autocompTree"></div>'
                });
                $('#autocompTree')
                .on('changed.jstree', function (e, data) {
                    var i, j, r = [], v = [];
                    for (i = 0, j = data.selected.length; i < j; i++) {
                        r.push(data.instance.get_node(data.selected[i]).text);

                        v.push(data.instance.get_node(data.selected[i]).id);
                    }
                    self.val(r.join(', ')).trigger("change");
                    $("#" + target).val(v.join(', '));

                    self.prop('readonly', true).addClass("text-danger");

                    if (self.data("callback") !== undefined) {
                        window[self.data("callback")](data.instance.get_node(data.selected[0]));
                    }
                    mymodal.modal("hide");

                })
                .jstree({
                    'core': {
                        'themes': {
                            'name': 'proton',
                            'responsive': true
                        },
                        'data': {
                            "url": url,
                            "data": function (node) {
                                return { "treenodeid": node.id };
                            }
                        }
                    }
                });
            }
            else if (type === "CustomView") {
                if (viewUrl === "") {
                    alert("viewUrl parametresi ayarlanmamış!");
                } else {
                    if (viewUrl.contains("?")) {
                        viewUrl = viewUrl + "&layout=modal";
                    } else {
                        viewUrl = viewUrl + "?layout=modal";
                    }

                    var request = $.ajax({
                        url: viewUrl,
                        method: "GET",
                        async: true,
                        dataType: "html"
                    });

                    request.done(function (msg) {
                        unblockUI();
                        mymodal = bootbox.dialog({
                            title: "<i class=\"fa fa-table\"></i> Arayıp Seçiniz",
                            closeButton: true,
                            className: "mymodal",
                            size: "large",
                            message: msg
                        });

                        mymodal.on("shown.bs.modal", function () {
                            AraFiltered();

                            $("table tbody").on("click", "tr td a", function (e) {
                                var ahref = $(this);
                                self.val(ahref.data("label")).trigger("change").prop("readonly", true).addClass("text-danger");
                                self.data("ui-autocomplete")._trigger("select", "autocompleteselect", { item: { value: self.val(), id: ahref.data("value") } });
                                $("#" + target).val(ahref.data("value"));

                                mymodal.modal("hide");
                            });
                        });
                    });
                }
            }
            else if (type === "List") // List veya none
            {
                var baslangic = 0;

                mymodal = bootbox.dialog({
                    title: "<i class='fa fa-list-ul'></i> Listeden Seçiniz",
                    message: '<div id="autocompList"><input type="text" class="form-control" placeholder="aramak için yazmaya başlayınız" id="autocompListKey"/></div>',
                    buttons: {
                        nextbtn: {
                            label: '<i class="fa fa-chevron-right"></i>',
                            className: "btn btn-primary",
                            callback: function () {
                                baslangic = baslangic + 10;
                                getData();
                                return false;
                            }
                        },
                        prvsbtn: {
                            label: '<i class="fa fa-chevron-left"></i>',
                            className: "btn btn-primary pull-left",
                            title: "Önceki Sayfa",
                            callback: function () {
                                if (baslangic >= 10) {
                                    baslangic = baslangic - 10;
                                } else {
                                    return false;
                                }
                                getData();
                                return false;
                            }
                        }
                    }
                });

                getData();

                var delay = (function () {
                    var timer = 0;
                    return function (callback, ms) {
                        clearTimeout(timer);
                        timer = setTimeout(callback, ms);
                    };
                })();

                $("#autocompListKey").keyup(function () {
                    delay(function () {
                        baslangic = 0;
                        getData();
                    }, 500);
                });

                function getData() {
                    var urlPrm = url + "&key=" + $("#autocompListKey").val() + "&baslangic=" + baslangic;
                    $.getJSON(urlPrm, function (data) {
                        var items = [];
                        $.each(data, function (key, val) {
                            items.push('<tr class="pointer" id="tr_' + key + '" data-value="' + val.id + '" data-label="' + val.text + '" data-desc="' + val.description + '"><td>' + val.text + '</td><td>' + val.description + '</td></tr>');
                        });

                        $('table.autocompListTable').remove();

                        $('<table/>', {
                            'data-start': 0,
                            'class': 'table table-striped table-hover autocompListTable',
                            html: items.join('')
                        }).appendTo($("#autocompList"));
                        $('table.autocompListTable tr')
                            .on('click', function (e, data) {
                                var tr = $(this);
                                self.val(tr.data("label")).trigger("change").prop('readonly', true).addClass("text-danger");
                                self.data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: self.val(), id: tr.data("value") } });
                                $("#" + target).val(tr.data("value"));

                                mymodal.modal("hide");
                            });

                    });
                }
            }
        });

    }

    if ($('.typeahead', scope).length) {

        /*
        $('.typeahead', scope)
            .each(function(index) {

                var self = $(this);
                var isMultiple = self.data("multiple");
                var url = self.data("ajaxdeger") + "?id=";
                if (isMultiple === "True") {
                    url = self.data("ajaxcokdeger") + "?idler=";
                }
                //içi dolu ise ajax ile dolu set et
                var selecteddata = $("input[type=hidden]", $("#" + self.data("target"))).val();

                if (selecteddata !== "" && selecteddata !== "0" && selecteddata !== undefined) {

                    var acRequest = $.ajax({
                        url: url + selecteddata,
                        method: "GET",
                        async: true
                    });


                    acRequest.done(function (data) {
                        if (isMultiple === "True") {
                            $.each(data,
                                function(i, item) {
                                    $("<span></span>")
                                        .addClass("badge badge-info")
                                        .data("vv", item.id)
                                        .text(item.text + " ")
                                        .append($("<i></i>")
                                            .addClass("fa fa-times")
                                            .click(function() {
                                                var v =
                                                    split($("input[type=hidden]", $("#" + self.data("target"))).val());
                                                var vtr = $(this).parent().data("vv");
                                                v = _.without(v, vtr);
                                                $("input[type=hidden]", $("#" + self.data("target"))).val(v.toString());
                                                $(this).parent().remove();
                                            })
                                        )
                                        .appendTo($("#ac_selection" + self.data("target")));
                                    $("#ac_selection" + self.data("target")).append(" ");
                                });
                        } else {
                            self.val(data.text).trigger("change");
                        }
                     
                        if (self.data("connectedwith") !== undefined) {
                            $("#_" + self.data("connectedwith")).prop('disabled', true);
                        }


                        self.prop('readonly', true);
                    });

                    acRequest.error(function(msg) {
                        self.val("Yüklenemedi!!");
                        self.prop('disabled', true).addClass("text-danger");

                    });


                }
            });*/






        $('.typeahead', scope)
            .each(function (index) {

                var self = $(this);
                var showDescription = self.data("showdesc");
                var isMultiple = self.data("multiple");
                var targetId = self.data("target");
                var connectedWith = self.data("connectedwith");
                var connectedWith2 = self.data("connectedwith2");
                var connectedwithlist = self.data("connectedwithlist");

                var singleurl = self.data("ajaxdeger") + "?id=";
                if (isMultiple === "True") {
                    singleurl = self.data("ajaxcokdeger") + "?idler=";
                }
                //içi dolu ise ajax ile dolu set et
                var selectedId = $("#" + targetId).val();
                if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {


                    if (self.val() === "") {

                        var acRequest = $.ajax({
                            url: singleurl + selectedId,
                            method: "GET",
                            async: true
                        });


                        acRequest.done(function (data) {
                            if (isMultiple === "True") {
                                $.each(data,
                                    function (i, item) {
                                        $("<span></span>")
                                            .addClass("badge badge-info")
                                            .data("vv", item.id)
                                            .text(item.text + " ")
                                            .append($("<i></i>")
                                                .addClass("fa fa-times")
                                                .click(function () {
                                                    var v = $("#" + targetId).val();
                                                    var vtr = $(this).parent().data("vv");
                                                    v = _.without(v, vtr);
                                                    $("#" + targetId).val(v.toString());
                                                    $(this).parent().remove();
                                                })
                                            )
                                            .appendTo($("#ac_selection_" + targetId));
                                        $("#ac_selection_" + targetId).append(" ");
                                    });
                            } else {
                                self.val(data.text).trigger("change");
                            }

                            self.prop('readonly', true);
                        });

                        acRequest.error(function (msg) {
                            self.val("Yüklenemedi!!");
                            self.prop('disabled', true).addClass("text-danger");

                        });


                    } else {
                        self.prop('readonly', true);
                    }
                }


                if (isMultiple === "True") {

                    self.on("keydown",
                        function (event) {
                            if (event.keyCode === $.ui.keyCode.TAB &&
                                $(this).autocomplete("instance").menu.active) {
                                event.preventDefault();
                            }
                        });

                }

                if (connectedWith !== undefined) {
                    if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                        $("#_" + connectedWith).data("rootid", selectedId).addClass("connected");

                    } else {
                        $("#_" + connectedWith).prop('disabled', true);
                    }
                }
                if (connectedWith2 !== undefined) {
                    if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                        $("#_" + connectedWith2).data("rootid2", selectedId);

                    } else {
                        $("#_" + connectedWith2).prop('disabled', true);
                    }
                }

                if (connectedwithlist !== undefined) {

                    var arr = connectedwithlist.split(',');

                    $.each(arr, function (index, value) {

                        if (selectedId !== "" && selectedId !== "0" && selectedId !== undefined) {
                            $("#_" + value).data("rootid", selectedId).addClass("connected");

                        } else {
                            $("#_" + value).prop('disabled', true);
                        }
                    });
                }

                //     debugger;

                self.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            url: self.data("url"),
                            dataType: "json",
                            data: {
                                key: extractLast(request.term),//request.term
                                rootId: self.data("rootid"),
                                rootId2: self.data("rootid2")
                            },
                            success: function (data) {

                                response($.map(data,
                                    function (item) {
                                        return {
                                            label: item.description,
                                            value: item.text,
                                            id: item.id
                                        };
                                    }));
                            },
                            error: function (xhr, ajaxOptions, thrownError) {
                                debugger;
                                ajaxMesajGoster2(xhr.responseJSON.Mesaj, "Hata oluştu", xhr.responseJSON.RedirectUrl);
                            }
                        });
                    },

                    minLength: 0,
                    //   multiselect: isMultiple === "True"?true:false,

                    select: function (event, ui) {


                        if (isMultiple === "True") {

                            var v = [];
                            if ($("#" + targetId).val() !== "")
                                v = split($("#" + targetId).val());

                            if (!_.contains(v, ui.item.id)) {

                                $("<span></span>")
                                    .addClass("badge badge-info")
                                    .data("vv", ui.item.id)
                                    .text(ui.item.value + " ")
                                    .append($("<i></i>")
                                        .addClass("fa fa-times")
                                        .click(function () {
                                            var v = split($("#" + targetId).val());
                                            var vtr = $(this).parent().data("vv");
                                            v = _.without(v, vtr);
                                            $("#" + targetId).val(v.toString());
                                            $(this).parent().remove();
                                        })
                                    )
                                    .appendTo($("#ac_selection_" + targetId));
                                $("#ac_selection_" + targetId).append(" ");
                                v.push(ui.item.id);


                                $("#" + targetId).val(v.toString());
                                self.val("");



                            }
                            self.val("");

                        }
                        else {
                            $("#" + targetId).val(ui.item.id);
                            self.val(ui.item.value).addClass("text-danger").prop("readonly", true);//.trigger("change");
                        }

                        if (self.data("callback") !== undefined) {
                            window[self.data("callback")](ui.item);
                        }

                        if (targetId !== undefined) {
                            $("#_" + connectedWith).data("rootid", ui.item.id).addClass("connected").prop('disabled', false).focus();

                            if (connectedwithlist !== undefined) {
                                var arr = connectedwithlist.split(',');

                                $.each(arr,
                                    function(index, value) {
                                        $("#_" + value)
                                            .data("rootid", ui.item.id)
                                            .addClass("connected")
                                            .prop('disabled', false)
                                            .focus();
                                    });
                            }
                        }

                        self.blur();
                        return false;


                    }

                });

                self.focus(function () {
                    $(this).autocomplete("search", "");
                }).autocomplete("instance")._renderItem = function (ul, item) {
                    return $("<li>").append(item.value).appendTo(ul);
                };

            });





        //$(".autocomplete_clear", scope).click(function (e) {
        //    e.preventDefault();

        //    var target = $(this).data("target");
        //    var self = $("#_" + target);
        //    var connectedId = self.data("connectedwith");
        //    var connectedId2 = self.data("connectedwith2");
        //    self.val('').prop("readonly", false).removeClass("autocomplete_loading").focus();
        //    $("#" + target).val("");


        //    if (self.data("clearcallback") !== undefined) {
        //        window[self.data("clearcallback")](e);
        //    }

        //    if (connectedId !== undefined) {
        //        //bağlantılı ise bağlı olduğu ototamamları da sıfırla
        //        $("#ac_clear_" + connectedId + " ").trigger("click");
        //        $("#_" + connectedId).prop('disabled', true);
        //    }
        //    if (connectedId2 !== undefined) {
        //        //bağlantılı ise bağlı olduğu ototamamları da sıfırla
        //        $("#ac_clear_" + connectedId2 + " ").trigger("click");
        //        $("#_" + connectedId2).prop('disabled', true);
        //    }

        //    $("#ac_selection_" + target).html("");
        //});

        //$(".listopener", scope).click(function (e) {
        //    e.preventDefault();

        //    var type = $(this).data("type");
        //    var target = $(this).data("target");
        //    var self = $("#_" + target);
        //    var url = self.data("url");
        //    var viewUrl = self.data("viewurl");
        //    var mymodal;




        //    if (type === "Tree") { //tree
        //        url = url.replace("AjaxAra", "AjaxAraTree");
        //        mymodal = bootbox.dialog({
        //            title: "<i class='fa fa-sitemap'></i> Ağaçtan Seçiniz",
        //            message: '<div id="autocompTree"></div>'
        //        });
        //        $('#autocompTree')
        //        .on('changed.jstree', function (e, data) {
        //            var i, j, r = [], v = [];
        //            for (i = 0, j = data.selected.length; i < j; i++) {
        //                r.push(data.instance.get_node(data.selected[i]).text);

        //                v.push(data.instance.get_node(data.selected[i]).id);
        //            }
        //            self.val(r.join(', ')).trigger("change");
        //            $("#" + target).val(v.join(', '));

        //            self.prop('readonly', true).addClass("text-danger");

        //            if (self.data("callback") !== undefined) {
        //                window[self.data("callback")](data.instance.get_node(data.selected[0]));
        //            }
        //            mymodal.modal("hide");

        //        })
        //        .jstree({
        //            'core': {
        //                'themes': {
        //                    'name': 'proton',
        //                    'responsive': true
        //                },
        //                'data': {
        //                    "url": url,
        //                    "data": function (node) {
        //                        return { "treenodeid": node.id };
        //                    }
        //                }
        //            }
        //        });
        //    }
        //    else if (type === "CustomView") {
        //        if (viewUrl === "") {
        //            alert("viewUrl parametresi ayarlanmamış!");
        //        } else {


        //            var modal_width = '800';
        //            var modal_height = '400';

        //            if (viewUrl.contains("?")) {
        //                viewUrl = viewUrl + "&layout=modal";
        //            } else {
        //                viewUrl = viewUrl + "?layout=modal";
        //            }

        //            mymodal = bootbox.dialog({
        //                title: $(this).data("original-title"),
        //                backdrop: true,
        //                closeButton: true,
        //                animate: true,
        //                className: "my-modal",
        //                size: "large",
        //                message: '<iframe style="width:100%;height:99%;min-height:' + modal_height + 'px;border:0;"  src="' + viewUrl + '"></iframe>'
        //            }).resizable();

        //        }


        //    }


        //    else if (type === "List") // List veya none
        //    {
        //        var baslangic = 0;

        //        mymodal = bootbox.dialog({
        //            title: "<i class='fa fa-list-ul'></i> Listeden Seçiniz",
        //            message: '<div id="autocompList"><input type="text" class="form-control" placeholder="aramak için yazmaya başlayınız" id="autocompListKey"/></div>',
        //            buttons: {
        //                nextbtn: {
        //                    label: '<i class="fa fa-chevron-right"></i>',
        //                    className: "btn btn-primary",
        //                    callback: function () {
        //                        baslangic = baslangic + 10;
        //                        getData();
        //                        return false;
        //                    }
        //                },
        //                prvsbtn: {
        //                    label: '<i class="fa fa-chevron-left"></i>',
        //                    className: "btn btn-primary pull-left",
        //                    title: "Önceki Sayfa",
        //                    callback: function () {
        //                        if (baslangic >= 10) {
        //                            baslangic = baslangic - 10;
        //                        } else {
        //                            return false;
        //                        }
        //                        getData();
        //                        return false;
        //                    }
        //                }
        //            }

        //        });

        //        getData();


        //        var delay = (function () {
        //            var timer = 0;
        //            return function (callback, ms) {
        //                clearTimeout(timer);
        //                timer = setTimeout(callback, ms);
        //            };
        //        })();

        //        $("#autocompListKey").keyup(function () {
        //            delay(function () {
        //                baslangic = 0;
        //                getData();
        //            }, 500);
        //        });

        //        function getData() {
        //            var urlPrm = url + "&key=" + $("#autocompListKey").val() + "&baslangic=" + baslangic;
        //            $.getJSON(urlPrm, function (data) {
        //                var items = [];
        //                $.each(data, function (key, val) {
        //                    items.push('<tr class="pointer" id="tr_' + key + '" data-value="' + val.id + '" data-label="' + val.text + '" data-desc="' + val.description + '"><td>' + val.text + '</td><td>' + val.description + '</td></tr>');
        //                });

        //                $('table.autocompListTable').remove();

        //                $('<table/>', {
        //                    'data-start': 0,
        //                    'class': 'table table-striped table-hover autocompListTable',
        //                    html: items.join('')
        //                }).appendTo($("#autocompList"));
        //                $('table.autocompListTable tr')
        //                    .on('click', function (e, data) {
        //                        var tr = $(this);
        //                        self.val(tr.data("label")).trigger("change").prop('readonly', true).addClass("text-danger");
        //                        self.data('ui-autocomplete')._trigger('select', 'autocompleteselect', { item: { value: self.val(), id: tr.data("value") } });
        //                        $("#" + target).val(tr.data("value"));

        //                        mymodal.modal("hide");
        //                    });

        //            });
        //        }

        //    }
        //    /*    $(mymodal).draggable({
        //            handle: ".modal-header"
        //        }); */
        //});

    }


    /////////////////////////////
    /*checkbox(switch)*/
    /////////////////////////////
    $("input.switch", scope).bootstrapSwitch({
        size: 'mini',
        animate: "true",
        onColor: 'success',
        onText: 'Evet',
        offColor: 'danger',
        offText: 'Hayır'
    });
    $('input.switch', scope).on('switchChange.bootstrapSwitch', function (event, state) {
        this.value = state;
    });

    $.mask.definitions['&'] = '[+-]';
    $("input.geographicalPosition", scope).mask("&99.999999?999999999999");




    $('table.data-table', scope).dataTable({
        //dom: 'Blfrtip',
        //buttons: [
        //      { extend: 'excel', className: 'btn btn-info' },
        //    {
        //        extend: 'pdf',
        //        className: 'btn btn-primary',
        //        title: "Deneme",
        //        exportOptions: {
        //            modifier: {

        //                order:  'applied',  // 'current', 'applied', 'index',  'original'
        //                page:   'all',      // 'all',     'current'
        //                search: 'applied',     // 'none',    'applied', 'removed'

        //            }
        //        }
        //    },

        //],

        "language": dataTableLanguage,
        "pageLength": 20,
        "columnDefs": [{
            "targets": 'no-sort',

            "orderable": false,
        }]
    });



}

//alert yerine kullanılabilir
function ajaxMesajGoster(msg, title) {
    bootbox.alert({
        title: title,
        message: msg,
        locale: 'tr'
    });
}

function ajaxMesajGoster2(msg, title, redirecturl, btnlabel) {
    bootbox.alert({
        title: title,
        message: msg,
        locale: 'tr',
        callback: function (result) {
            window.location = redirecturl;

        }
    });
}


//function ajaxMesajGoster(jsonAjaxSonuc) {


//    bootbox.alert({
//        title: jsonAjaxSonuc.Durum=="1"?"Bilgi":"Uyarı",
//        message: jsonAjaxSonuc.Mesaj,
//        locale: 'tr',
//        callback: function (result) {
//            if (jsonAjaxSonuc.RedirectUrl != "") {
//                window.location = jsonAjaxSonuc.RedirectUrl;
//            }

//        }
//    });
//}




function blockUI() {
    $.blockUI({
        //        message: "<div id='lab'><div id='beaker'><span class='bubble'> <span class='glow'> </span> </span><span class='bubble1'> <span class='glow1'> </span> </span> <span class='bubble2'> <span class='glow2'></span> </span> <span class='bubble3'> <span class='glow3'> </span> </span> <span class='bubble4'> <span class='glow4'> </span> </span> </div></div>",

        message: "<i class='fa fa-circle-o-notch fa-spin fa-3x fa-fw'></i>",
        css: {
            border: 'none',
            padding: '40px',
            backgroundColor: 'transparent',
            'border-radius': '10px',
            opacity: 0.5,
            color: '#fff'
        }
    });
}

function unblockUI() {
    $.unblockUI();
}

function Ajax(method, url, data) {
    blockUI();
    var request = $.ajax({
        url: url,
        data: data,
        method: method,
        async: true,
        dataType: "json"
    });

    request.done(function (jqXHR, textStatus) {
        if (jqXHR.Durum === 1) {
            if (jqXHR.RedirectUrl !== "") {
                window.location = jqXHR.RedirectUrl;
            } else {
                unblockUI();
                ajaxMesajGoster(jqXHR.Mesaj);
            }
        } else {
            unblockUI();
            ajaxMesajGoster(jqXHR.Mesaj);
        }
    });
    request.fail(function (xhr, textStatus) {
        unblockUI();
        var msg = jQuery.parseJSON(xhr.responseText);
        ajaxMesajGoster(msg.Mesaj, "Birşeyler ters gitti!");
    });
}

var newExportAction = function (e, dt, button, config) {
    var self = this;
    var oldStart = dt.settings()[0]._iDisplayStart;

    dt.one('preXhr', function (e, s, data) {
        // Just this once, load all data from the server...
        data.start = 0;
        data.length = 100000;

        dt.one('preDraw', function (e, settings) {
            // Call the original action function
            oldExportAction(self, e, dt, button, config);

            dt.one('preXhr', function (e, s, data) {
                // DataTables thinks the first item displayed is index 0, but we're not drawing that.
                // Set the property to what it was before exporting.
                settings._iDisplayStart = oldStart;
                data.start = oldStart;
            });

            // Reload the grid with the original page. Otherwise, API functions like table.cell(this) don't work properly.
            setTimeout(dt.ajax.reload, 0);

            // Prevent rendering of the full data to the DOM
            return false;
        });
    });

    // Requery the server with the new one-time export settings
    dt.ajax.reload();
};

var oldExportAction = function (self, e, dt, button, config) {
    if (button[0].className.indexOf('buttons-excel') >= 0) {
        if ($.fn.dataTable.ext.buttons.excelHtml5.available(dt, config)) {
            $.fn.dataTable.ext.buttons.excelHtml5.action.call(self, e, dt, button, config);
        }
        else {
            $.fn.dataTable.ext.buttons.excelFlash.action.call(self, e, dt, button, config);
        }
    } else if (button[0].className.indexOf('buttons-print') >= 0) {
        $.fn.dataTable.ext.buttons.print.action(e, dt, button, config);
    }
};
////////////////////////////////////////////
///EXTENSIONS/////////////////////////
////////////////////////////////////////////

String.prototype.toMoney = function () {


    Decimal.format = {
        decimalSeparator: ',',
        groupSeparator: '.',
        groupSize: 3,
        secondaryGroupSize: 3
    };

    var disp = new Decimal(this.replace(",", "."));
    return disp.toFormat();

};

//string contains metodu
if (!('contains' in String.prototype)) {
    String.prototype.contains = function (str, startIndex) {
        return -1 !== String.prototype.indexOf.call(this, str, startIndex);
    };
}



//object serializer
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


//Türkçe upper case
$.fn.trUpperCase = function () {
    this.each(function () {
        var inlineText = $(this).text();
        inlineText = inlineText.replace(new RegExp('ğ', 'g'), 'Ğ');
        inlineText = inlineText.replace(new RegExp('ü', 'g'), 'Ü');
        inlineText = inlineText.replace(new RegExp('ş', 'g'), 'Ş');
        inlineText = inlineText.replace(new RegExp('i', 'g'), 'İ');
        inlineText = inlineText.replace(new RegExp('ö', 'g'), 'Ö');
        inlineText = inlineText.replace(new RegExp('ç', 'g'), 'Ç');
        inlineText = inlineText.replace(new RegExp('ı', 'g'), 'I');
        inlineText = inlineText.toUpperCase();
        $(this).text(inlineText);
    });
};



//datatable varsayılanları
var dataTableLanguage = {
    "sDecimal": ",",
    "sEmptyTable": "Kayıt bulunmamaktadır",
    "sInfo": "Toplam: <b>_TOTAL_</b> kayıt | _START_ - _END_ arası gösteriliyor",
    "sInfoEmpty": "Kayıt bulunmamaktadır",
    "sInfoFiltered": "(_MAX_ kayit içerisinden bulunan)",
    "sInfoPostFix": "",
    "sInfoThousands": ".",
    "sLengthMenu": "Sayfada _MENU_ kayıt göster",
    "sLoadingRecords": "Yükleniyor...",
    "sProcessing": "<i class='fa fa-circle-o-notch fa-spin fa-fw'></i> Veri Yükleniyor...",
    "sSearch": "",
    "sZeroRecords": "Eşleşen kayıt bulunamadı",
    searchPlaceholder: "Hızlı Ara",
    "oPaginate": {
        "sFirst": "İlk",
        "sLast": "Son",
        "sNext": "Sonraki",
        "sPrevious": "Önceki"
    },
    "oAria": {
        "sSortAscending": ": artan sütun sıralamasını aktifleştir",
        "sSortDescending": ": azalan sütun soralamasını aktifleştir"
    },
    select: {
        rows: {
            _: "Bu sayfada <b>%d</b> kayıt seçildi",
            0: " ",
            1: "Bu sayfada 1 kayıt seçildi"
        }
    }
};

var englishLanguage = {
    "sDecimal": ",",
    "sEmptyTable": "No data available in table",
    "sInfo": "Showing _START_ to _END_ of _TOTAL_ entries",
    "sInfoEmpty": "Showing 0 to 0 of 0 entries",
    "sInfoFiltered": "(_MAX_ kayit içerisinden bulunan)",
    "sInfoPostFix": "",
    "sInfoThousands": ".",
    "sLengthMenu": "Show _MENU_ entries",
    "sLoadingRecords": "Loading...",
    "sProcessing": "<i class='fa fa-circle-o-notch fa-spin fa-fw'></i> Processing...",
    "sSearch": "",
    "sZeroRecords": "No matching records found",
    searchPlaceholder: "Quick Search",
    "oPaginate": {
        "sFirst": "First",
        "sLast":  "Last",
        "sNext":  "Next",
        "sPrevious": "Previous"
    },
    "oAria": {
        "sSortAscending":  ":activate to sort column ascending",
        "sSortDescending": ":activate to sort column descending"
    },
    select: {
        rows: {
            _: "Bu sayfada <b>%d</b> kayıt seçildi",
            0: " ",
            1: "Bu sayfada 1 kayıt seçildi"
        }
    }
};

 

$.extend(true, $.fn.dataTable.defaults, {
    processing: true,
    //stateSave: true,
    fixedHeader: true,
    language: dataTableLanguage,

    serverSide: true,
    destroy: true,
});



///Seçilen nesneyi ve onun içindeki nesneleri gizler ve değerlerini temizler
$.fn.HideAndClear = function () {
    this.hide();
    this.Temizle();
};
$.fn.Temizle = function() {
    this.find("input").val("").blur();
    this.find("input[type=text], textarea").val("").blur();

    this.find("input[type=checkbox]").prop("checked", false);

    this.find(".autocomplete_clear").click();
}