using Ninject.Modules;
using OsosOracle.Business.Abstract;
using OsosOracle.Business.Concrete;
using OsosOracle.DataLayer.Abstract;
using OsosOracle.DataLayer.Concrete.EntityFramework.Dal;

namespace OsosOracle.Business.DependencyResolvers.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {

            #region Service Bindings
            Bind<ICSTSAYACMODELService>().To<CSTSAYACMODELManager>().InSingletonScope();
            Bind<ICONKURUMService>().To<CONKURUMManager>().InSingletonScope();
            Bind<ICSTHUMARKAService>().To<CSTHUMARKAManager>().InSingletonScope();
            Bind<ICSTHUMODELService>().To<CSTHUMODELManager>().InSingletonScope();
            Bind<IENTABONEService>().To<ENTABONEManager>().InSingletonScope();
            Bind<IENTABONEBILGIService>().To<ENTABONEBILGIManager>().InSingletonScope();
            Bind<IENTABONESAYACService>().To<ENTABONESAYACManager>().InSingletonScope();
            Bind<IENTSAYACService>().To<ENTSAYACManager>().InSingletonScope();
            Bind<IENTSAYACDURUMSUService>().To<ENTSAYACDURUMSUManager>().InSingletonScope();
            Bind<IENTTUKETIMSUService>().To<ENTTUKETIMSUManager>().InSingletonScope();
            Bind<IENTSATISService>().To<ENTSATISManager>().InSingletonScope();
            Bind<IENTHABERLESMEUNITESIService>().To<ENTHABERLESMEUNITESIManager>().InSingletonScope();
            Bind<IENTHUSAYACService>().To<ENTHUSAYACManager>().InSingletonScope();
            Bind<IENTKOMUTLARBEKLEYENService>().To<ENTKOMUTLARBEKLEYENManager>().InSingletonScope();
            Bind<IENTKOMUTLARSONUCLANANService>().To<ENTKOMUTLARSONUCLANANManager>().InSingletonScope();
            Bind<IENTKREDIKOMUTTAKIPService>().To<ENTKREDIKOMUTTAKIPManager>().InSingletonScope();
            Bind<IENTSAYACSONDURUMSUService>().To<ENTSAYACSONDURUMSUManager>().InSingletonScope();
            Bind<IPRMTARIFEELKService>().To<PRMTARIFEELKManager>().InSingletonScope();
            Bind<IPRMTARIFEGAZService>().To<PRMTARIFEGAZManager>().InSingletonScope();
            Bind<IPRMTARIFEORTAKAVMService>().To<PRMTARIFEORTAKAVMManager>().InSingletonScope();
            Bind<IPRMTARIFESUService>().To<PRMTARIFESUManager>().InSingletonScope();
            Bind<IRPTDASHBOARDService>().To<RPTDASHBOARDManager>().InSingletonScope();
            Bind<ISYSCSTOPERASYONService>().To<SYSCSTOPERASYONManager>().InSingletonScope();
            Bind<ISYSGOREVService>().To<SYSGOREVManager>().InSingletonScope();
            Bind<ISYSGOREVROLService>().To<SYSGOREVROLManager>().InSingletonScope();
            Bind<ISYSKULLANICIService>().To<SYSKULLANICIManager>().InSingletonScope();
            Bind<ISYSKULLANICIDETAYService>().To<SYSKULLANICIDETAYManager>().InSingletonScope();
            Bind<ISYSMENUService>().To<SYSMENUManager>().InSingletonScope();
            Bind<ISYSOPERASYONGOREVService>().To<SYSOPERASYONGOREVManager>().InSingletonScope();
            Bind<ISYSROLService>().To<SYSROLManager>().InSingletonScope();
            Bind<ISYSROLKULLANICIService>().To<SYSROLKULLANICIManager>().InSingletonScope();

            #endregion

            #region Data Access Layer bindings
            Bind<ICSTSAYACMODELDal>().To<EfCSTSAYACMODELDal>().InSingletonScope();
            Bind<ICONKURUMDal>().To<EfCONKURUMDal>().InSingletonScope();
            Bind<ICSTHUMARKADal>().To<EfCSTHUMARKADal>().InSingletonScope();
            Bind<ICSTHUMODELDal>().To<EfCSTHUMODELDal>().InSingletonScope();
            Bind<IENTABONEDal>().To<EfENTABONEDal>().InSingletonScope();
            Bind<IENTABONEBILGIDal>().To<EfENTABONEBILGIDal>().InSingletonScope();
            Bind<IENTABONESAYACDal>().To<EfENTABONESAYACDal>().InSingletonScope();
            Bind<IENTSAYACDal>().To<EfENTSAYACDal>().InSingletonScope();
            Bind<IENTSAYACDURUMSUDal>().To<EfENTSAYACDURUMSUDal>().InSingletonScope();
            Bind<IENTTUKETIMSUDal>().To<EfENTTUKETIMSUDal>().InSingletonScope();
            Bind<IENTSATISDal>().To<EfENTSATISDal>().InSingletonScope();
            Bind<IENTHABERLESMEUNITESIDal>().To<EfENTHABERLESMEUNITESIDal>().InSingletonScope();
            Bind<IENTHUSAYACDal>().To<EfENTHUSAYACDal>().InSingletonScope();
            Bind<IENTKOMUTLARBEKLEYENDal>().To<EfENTKOMUTLARBEKLEYENDal>().InSingletonScope();
            Bind<IENTKOMUTLARSONUCLANANDal>().To<EfENTKOMUTLARSONUCLANANDal>().InSingletonScope();
            Bind<IENTKREDIKOMUTTAKIPDal>().To<EfENTKREDIKOMUTTAKIPDal>().InSingletonScope();
            Bind<IENTSAYACSONDURUMSUDal>().To<EfENTSAYACSONDURUMSUDal>().InSingletonScope();
            Bind<IPRMTARIFEELKDal>().To<EfPRMTARIFEELKDal>().InSingletonScope();
            Bind<IPRMTARIFEGAZDal>().To<EfPRMTARIFEGAZDal>().InSingletonScope();
            Bind<IPRMTARIFEORTAKAVMDal>().To<EfPRMTARIFEORTAKAVMDal>().InSingletonScope();
            Bind<IPRMTARIFESUDal>().To<EfPRMTARIFESUDal>().InSingletonScope();
            Bind<IRPTDASHBOARDDal>().To<EfRPTDASHBOARDDal>().InSingletonScope();
            Bind<ISYSGOREVDal>().To<EfSYSGOREVDal>().InSingletonScope();
            Bind<ISYSGOREVROLDal>().To<EfSYSGOREVROLDal>().InSingletonScope();
            Bind<ISYSCSTOPERASYONDal>().To<EfSYSCSTOPERASYONDal>().InSingletonScope();
            Bind<ISYSKULLANICIDal>().To<EfSYSKULLANICIDal>().InSingletonScope();
            Bind<ISYSKULLANICIDETAYDal>().To<EfSYSKULLANICIDETAYDal>().InSingletonScope();
            Bind<ISYSMENUDal>().To<EfSYSMENUDal>().InSingletonScope();
            Bind<ISYSOPERASYONGOREVDal>().To<EfSYSOPERASYONGOREVDal>().InSingletonScope();
            Bind<ISYSROLDal>().To<EfSYSROLDal>().InSingletonScope();
            Bind<ISYSROLKULLANICIDal>().To<EfSYSROLKULLANICIDal>().InSingletonScope();
            #endregion
        }
    }
}
