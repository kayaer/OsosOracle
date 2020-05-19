class Mcm {

    constructor() {
        this.DllNamespace = "SmartCard.Mcm"; 
    }

    KartTipi() {
        try {
            var su = new ActiveXObject(this.DllNamespace);
            var kartTip = su.KartTipi();
            ajaxMesajGoster(kartTip);
        }
        catch (ex) {
            ajaxMesajGoster(ex);
        }

    }

    KartBosalt() {
        try {

            var su = new ActiveXObject(this.DllNamespace);
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

            ajaxMesajGoster(ex);
            
        }
    }

    AboneOku() {
        try {
            var aboneIslem = new ActiveXObject(this.DllNamespace);
            return aboneIslem.AboneOku();
        } catch (ex) {
            ajaxMesajGoster(ex, 'Hata');
        }

    }

    AboneYap(jqXHR) {

        try {
            var su = new ActiveXObject(this.DllNamespace);
            var result = su.AboneYap(jqXHR.EntSayacDetay.SERINO,
                jqXHR.AboneNo,
                1,
                jqXHR.PRMTARIFESU.SAYACCAP,
                1,//tip
                jqXHR.PRMTARIFESU.DONEMGUN,
                jqXHR.PRMTARIFESU.FIYAT1,
                jqXHR.PRMTARIFESU.FIYAT2,
                jqXHR.PRMTARIFESU.FIYAT3,
                jqXHR.PRMTARIFESU.FIYAT4,
                jqXHR.PRMTARIFESU.FIYAT5,
                jqXHR.PRMTARIFESU.LIMIT1,
                jqXHR.PRMTARIFESU.LIMIT2,
                jqXHR.PRMTARIFESU.LIMIT3,
                jqXHR.PRMTARIFESU.LIMIT4,
                jqXHR.PRMTARIFESU.BAYRAM1GUN,
                jqXHR.PRMTARIFESU.BAYRAM1AY,
                jqXHR.PRMTARIFESU.BAYRAM1SURE,
                jqXHR.PRMTARIFESU.BAYRAM2GUN,
                jqXHR.PRMTARIFESU.BAYRAM2AY,
                jqXHR.PRMTARIFESU.BAYRAM2SURE,
                jqXHR.PRMTARIFESU.AVANSONAY,
                1,
                0,
                jqXHR.PRMTARIFESU.MAXDEBI,
                jqXHR.PRMTARIFESU.KRITIKKREDI,
                1//BaglantiPeriyot


            );
            if (result === "1") {
                ajaxMesajGoster('İşlem Başarılı');
            } else {
                ajaxMesajGoster('İşlem Başarısız', 'Hata');
            }
        }
        catch (ex) {
            ajaxMesajGoster(ex);
        }
    }

    AboneYaz(satisKaydetModel) {

        try {
            var mcm = new ActiveXObject(this.DllNamespace);
            result = mcm.AboneYaz(satisKaydetModel.SogukSuOkunan.SayacSeriNo,
                satisKaydetModel.Satis.ToplamKredi,
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
                satisKaydetModel.Satis.YEDEKKREDI);
            return result;
        } catch (ex) {
            ajaxMesajGoster(ex,'HATA');
        }

    }
    
}