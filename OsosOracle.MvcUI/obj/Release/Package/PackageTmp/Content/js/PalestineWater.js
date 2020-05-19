class PalestineWater {

    constructor() {
        this.DllNamespace = "SmartCard.Filistin.PalestineWater"; 
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
                jqXHR.PRMTARIFESU.MAXDEBI,
                1,//ZoneIndex
                jqXHR.PRMTARIFESU.SABITUCRET
            );
            if (result === "1") {
                ajaxMesajGoster('İşlem Başarılı');
            } else {
                ajaxMesajGoster('İşlem Başarısız', 'Hata');
            }
        } catch (ex) {
            ajaxMesajGoster(ex);
        }

    }

    AboneYaz(satisKaydetModel) {
        try {
            var mercanYd = new ActiveXObject("SmartCard.Filistin.PalestineWater");
            result = mercanYd.AboneYaz(satisKaydetModel.SogukSuOkunan.SayacSeriNo,
                satisKaydetModel.Satis.ToplamKredi,
                satisKaydetModel.Satis.YEDEKKREDI,
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
                satisKaydetModel.PrmTarifeSuDetay.MAXDEBI,
                1,
                satisKaydetModel.PrmTarifeSuDetay.SABITUCRET);
            return result;
        } catch (ex) {
            ajaxMesajGoster(ex,'HATA');
        }

       
    }
}