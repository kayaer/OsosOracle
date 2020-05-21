function OrtakAvm() {
    this.DllNamespace = "SmartCard.OrtakAvm";
}

OrtakAvm.prototype.KartTipi = function () {
    try {
        var su = new ActiveXObject(this.DllNamespace);
        var kartTip = su.KartTipi();
        if (kartTip == "Subscriber Card") {
            kartTip = "Abone Kartı";
        } else if (kartTip == "Empty Card") {
            kartTip = "Boş kart";
        }
        ajaxMesajGoster(kartTip);

    }
    catch (ex) {
        ajaxMesajGoster(ex);
    }

}

OrtakAvm.prototype.KartBosalt = function () {
    try {

        var su = new ActiveXObject(this.DllNamespace);
        var kartTip = su.KartTipi();

        if (kartTip == "Abone Karti" || kartTip == "Abone Kartı" || kartTip == "CBA " || kartTip == "Subscriber Card") {
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


OrtakAvm.prototype.AboneYap = function (jqXHR, tip) {
    try {
        var su = new ActiveXObject(this.DllNamespace);
        if (tip === "su") {

            var result = su.AboneYap(jqXHR.EntSayacDetay.SERINO,
                1,
                jqXHR.PRMTARIFESU.LIMIT1,
                jqXHR.PRMTARIFESU.LIMIT2,
                jqXHR.PRMTARIFESU.FIYAT1,
                jqXHR.PRMTARIFESU.FIYAT2,
                jqXHR.PRMTARIFESU.FIYAT3,
                jqXHR.PRMTARIFESU.FIYAT4,
                jqXHR.PRMTARIFESU.FIYAT4,
                jqXHR.PRMTARIFESU.FIYAT4,
                jqXHR.PRMTARIFESU.FIYAT4


            );
            if (result === "1") {
                ajaxMesajGoster('İşlem Başarılı');
            } else {
                ajaxMesajGoster('İşlem Başarısız', 'Hata');
            }
        } else if (tip === "kalorimetre") {

            var result = su.AboneYap(jqXHR.EntSayacDetay.SERINO,
                1,
                jqXHR.PrmTarifeKALORIMETRE.LIMIT1,
                jqXHR.PrmTarifeKALORIMETRE.LIMIT2,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT1,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT2,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT3,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT4,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT4,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT4,
                jqXHR.PrmTarifeKALORIMETRE.FIYAT4


            );
            if (result === "1") {
                ajaxMesajGoster('İşlem Başarılı');
            } else {
                ajaxMesajGoster('İşlem Başarısız', 'Hata');
            }
        } else {
            ajaxMesajGoster('Tip Bulunamadı', 'Hata');
        }


    }
    catch (ex) {
        ajaxMesajGoster(ex);
    }
}

OrtakAvm.prototype.AboneOku = function () {

    getsessionInformation(function (data) {

        try {
            var aboneIslem = new ActiveXObject(this.DllNamespace);
            return aboneIslem.AboneOku();
        } catch (ex) {
            if (data.Dil == "1") { ajaxMesajGoster(ex, 'Error'); }
            else { ajaxMesajGoster(ex, 'Hata'); }
           
        }


    });

}

OrtakAvm.prototype.SuKrediYukle = function (SuSatisModel) {
    try {
        var sogukSu = new ActiveXObject(this.DllNamespace);
        result = sogukSu.AboneYazSu(SuSatisModel.SogukSuOkunan.SayacSeriNo,
            SuSatisModel.Satis.ToplamKredi,
            SuSatisModel.PrmTarifeSuDetay.YEDEKKREDI,  //SuSatisModel.Satis.YEDEKKREDI,
            SuSatisModel.PrmTarifeSuDetay.LIMIT1,
            SuSatisModel.PrmTarifeSuDetay.LIMIT2,
            SuSatisModel.PrmTarifeSuDetay.FIYAT1,
            SuSatisModel.PrmTarifeSuDetay.FIYAT2,
            SuSatisModel.PrmTarifeSuDetay.FIYAT3
        );
        return result;
    } catch (ex) {
        ajaxMesajGoster(ex, 'Hata');
    }


}

OrtakAvm.prototype.KalorimetreKrediYukle = function (KalorimetreSatisModel) {
    try {
        var kalorimetre = new ActiveXObject(this.DllNamespace);

        result = kalorimetre.KalorimetreYaz(KalorimetreSatisModel.KalorimetreOkunan.CihazNo,//SAyacserino getirilecek
            KalorimetreSatisModel.Satis.ToplamKredi,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.RezervKredi,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM1GUN,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM1AY,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM1SURE,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM2GUN,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM2AY,
            KalorimetreSatisModel.PrmTarifeKALORIMETREDetay.BAYRAM2SURE
        );
        return result;
    } catch (ex) {
        ajaxMesajGoster(ex, 'Hata');
    }




}
