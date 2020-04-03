using System.Collections.Generic;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.Entities.ComplexType.ENTSATISComplexTypes;
using OsosOracle.Entities.Concrete;
using OsosOracle.Business.Concrete.Infrastructure;
using OsosOracle.Framework.Aspects.ValidationAspects;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Entities.ComplexType.ENTABONESAYACComplexTypes;
using System.Linq;
using OsosOracle.Framework.CrossCuttingConcern.ExceptionHandling;
using System;
using OsosOracle.Framework.Aspects.TransactionAspects;

namespace OsosOracle.Business.Concrete
{
    public class ENTSATISManager : BaseManager, IENTSATISService
    {
        private readonly IENTSATISDal _eNTSATISDal;
        public ENTSATISManager(IENTSATISDal eNTSATISDal)
        {
            _eNTSATISDal = eNTSATISDal;
        }

        public ENTSATIS GetirById(int id)
        {
            return _eNTSATISDal.Getir(id);
        }

        public ENTSATISDetay DetayGetirById(int id)
        {
            return _eNTSATISDal.DetayGetir(id);
        }

        public List<ENTSATIS> Getir(ENTSATISAra filtre)
        {
            return _eNTSATISDal.Getir(filtre);
        }

        public int KayitSayisiGetir(ENTSATISAra filtre)
        {
            return _eNTSATISDal.KayitSayisiGetir(filtre);
        }

        public List<ENTSATISDetay> DetayGetir(ENTSATISAra filtre)
        {
            return _eNTSATISDal.DetayGetir(filtre);
        }

        public ENTSATISDataTable Ara(ENTSATISAra filtre = null)
        {
            return _eNTSATISDal.Ara(filtre);
        }


        [FluentValidationAspect(typeof(ENTSATISValidator))]
        private void Validate(ENTSATIS entity)
        {

        }
        [TransactionScopeAspect]
        public ENTSATIS Ekle(ENTSATIS entity)
        {

            var _entAboneSayacService = ServisGetir<IENTABONESAYACService>();
            ENTABONESAYAC aboneSayac = _entAboneSayacService.Getir(new ENTABONESAYACAra { SAYACKAYITNO = entity.SAYACKAYITNO }).FirstOrDefault();

            if (aboneSayac == null)
            {
                throw new NotificationException("Abone Sayaç bulunamadı" + entity.SAYACKAYITNO);
            }
            aboneSayac.SONSATISTARIH = DateTime.Now;
            _entAboneSayacService.Guncelle(aboneSayac.List());
            entity.ABONEKAYITNO = aboneSayac.ABONEKAYITNO;
            entity = _eNTSATISDal.Ekle(entity.ConvertEfList<ENTSATIS, ENTSATISEf>()).FirstOrDefault();
            return entity;
        }

        public void Guncelle(List<ENTSATIS> entityler)
        {
            foreach (var entity in entityler)
            {
                Validate(entity);
            }
            _eNTSATISDal.Guncelle(entityler.ConvertEfList<ENTSATIS, ENTSATISEf>());
        }

        public void Sil(List<int> idler)
        {
            _eNTSATISDal.Sil(idler);
        }

        public ENTSATISDataTable SonSatisGetir(ENTSATISAra filtre = null)
        {
            return _eNTSATISDal.SonSatisGetir(filtre);
        }

        public List<ENTSATIS> SatisGetir(int kurumKayitNo)
        {
            return _eNTSATISDal.SatisGetir(kurumKayitNo);
        }
    }
}