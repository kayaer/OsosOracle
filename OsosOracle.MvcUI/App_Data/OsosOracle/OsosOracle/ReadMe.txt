
OsosOracle Projesi Üretilmiş Kod



//AutoComplete functions
 [Description("/ENTSAYACSONDURUMSU/AjaxAra")]
 ENTSAYACSONDURUMSUGetir = 1000,

 [Description("/ENTSAYACSONDURUMSU/AjaxTekDeger")]
 ENTSAYACSONDURUMSUGetir = 1000,

 [Description("/ENTSAYACSONDURUMSU/AjaxCokDeger")]
 ENTSAYACSONDURUMSUGetir = 1000,

//Service Modül Bindings
Bind<IENTSAYACSONDURUMSUService>().ToConstant(WcfProxy<IENTSAYACSONDURUMSUService>.CreateChannel()).InSingletonScope();


//Appcontext Properties
public DbSet<ENTSAYACSONDURUMSUEf> ENTSAYACSONDURUMSUEf { get; set; }


//Appcontext Mappings
modelBuilder.Configurations.Add(new ENTSAYACSONDURUMSUMap());


//BussinesModule Service Bindindgs
Bind<IENTSAYACSONDURUMSUService>().To<ENTSAYACSONDURUMSUManager>().InSingletonScope();


//BussinesModule Data Access Layer bindings
Bind<IENTSAYACSONDURUMSUDal>().To<EfENTSAYACSONDURUMSUDal>().InSingletonScope();


//validator bindings
Bind<IValidator<ENTSAYACSONDURUMSU>>().To<ENTSAYACSONDURUMSUValidator>().InSingletonScope();
