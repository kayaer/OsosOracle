using FluentValidation;
using Ninject.Modules;
using OsosOracle.Business.ValidationRules.FluentValidation;
using OsosOracle.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.Business.DependencyResolvers.Ninject
{
    public class ValidationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IValidator<ENTSAYAC>>().To<ENTSAYACValidator>().InSingletonScope();
            Bind<IValidator<SYSCSTOPERASYON>>().To<SYSCSTOPERASYONValidator>().InSingletonScope();
            Bind<IValidator<PRMTARIFEELK>>().To<PRMTARIFEELKValidator>().InSingletonScope();
            Bind<IValidator<PRMTARIFEGAZ>>().To<PRMTARIFEGAZValidator>().InSingletonScope();
            Bind<IValidator<PRMTARIFEORTAKAVM>>().To<PRMTARIFEORTAKAVMValidator>().InSingletonScope();
            Bind<IValidator<PRMTARIFESU>>().To<PRMTARIFESUValidator>().InSingletonScope();
            Bind<IValidator<ENTKOMUTLARBEKLEYEN>>().To<ENTKOMUTLARBEKLEYENValidator>().InSingletonScope();
            Bind<IValidator<ENTKOMUTLARSONUCLANAN>>().To<ENTKOMUTLARSONUCLANANValidator>().InSingletonScope();
            Bind<IValidator<ENTKREDIKOMUTTAKIP>>().To<ENTKREDIKOMUTTAKIPValidator>().InSingletonScope();

        }
    }
}
