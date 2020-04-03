function KartBosalt() {

    try {

        var su = new ActiveXObject("SmartCard.Mercan");
        var kartTip = su.KartTipi();

        if (kartTip == "Abone Karti" || kartTip == "Abone Kartı" || kartTip == "CBA ") {
            var result = su.AboneBosalt();
            if (result === "1") {
                ajaxMesajGoster('İşlem Başarılı');
            } else {
                ajaxMesajGoster('İşlem Başarısız', 'Hata');
            }
            return;
        } else if (kartTip == "Bos Kart") {
            ajaxMesajGoster('Kart Boş');
        }

    }
    catch (ex) {
        ajaxMesajGoster(ex, 'Hata');
    }

}

function KartTipi() {

    try {

        var su = new ActiveXObject("SmartCard.Mercan");
        var kartTip = su.KartTipi();
        ajaxMesajGoster(kartTip);
    }
    catch (ex) {       
        ajaxMesajGoster(ex, 'Hata');
    }

}

function AboneOku() {

    try {
        var aboneIslem = new ActiveXObject("SmartCard.Mcm");
        var result = false;
        try {

            var kartTip = aboneIslem.KartTipi();
            if (kartTip === "Abone Kartı" || kartTip === "Abone Karti" || kartTip === "Subscriber Card") {
                okunanData = aboneIslem.AboneOku();
                return true;
            } else if (kartTip === "Bos Kart") {
                ajaxMesajGoster('Kart Boş');
                return false;
            } else {
                ajaxMesajGoster('Kart Bilgisi Alınamıyor');
                return false;
            }
            return result;
        } catch (e) {
            ajaxMesajGoster(e);
            return false;
        }

    } catch (e) {
        return false;
    }

}

function KrediYukle(satisKaydetModel) {

        var mcm = new ActiveXObject("SmartCard.Mcm");
        result = mcm.AboneYaz(satisKaydetModel.SogukSuOkunan.SayacSeriNo,
                              parseInt($("#txtTutar").val()),
                              satisKaydetModel.PrmTarifeSuDetay.SAYACCAP,
                              satisKaydetModel.PrmTarifeSuDetay.DONEMGUN,
                              satisKaydetModel.PrmTarifeSuDetay.FIYAT1,
                              satisKaydetModel.PrmTarifeSuDetay.FIYAT2,
                              satisKaydetModel.PrmTarifeSuDetay.FIYAT3,
                              satisKaydetModel.PrmTarifeSuDetay.FIYAT4,
                              satisKaydetModel.PrmTarifeSuDetay.FIYAT5,
                              satisKaydetModel.PrmTarifeSuDetay.LIMIT1,
                              satisKaydetModel.PrmTarifeSuDetay.LIMIT2,
                              satisKaydetModel.PrmTarifeSuDetay.LIMIT3,
                              satisKaydetModel.PrmTarifeSuDetay.LIMIT4,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM1GUN,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM1AY,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM1SURE,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM2GUN,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM2AY,
                              satisKaydetModel.PrmTarifeSuDetay.BAYRAM2SURE,
                              satisKaydetModel.PrmTarifeSuDetay.AVANSONAY,
                              0,
                              0,
                              satisKaydetModel.PrmTarifeSuDetay.MAXDEBI,
                              satisKaydetModel.PrmTarifeSuDetay.KRITIKKREDI);
                             if (result == "1") {
                                 ajaxMesajGoster('İşlem Başarılı');
                             } else {
                                 ajaxMesajGoster('İşlem Başarısız');
                             }

}