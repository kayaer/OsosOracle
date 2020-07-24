using OsosOracle.DataLayer.Concrete.EntityFramework.Entity;
using OsosOracle.DataLayer.Concrete.EntityFramework.Mappings;
using OsosOracle.Framework.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsosOracle.DataLayer.Concrete.EntityFramework
{
    public class AppContext : DbContextBase
    {
        public AppContext()
               : base("name=AppContext")
        {
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

#if DEBUG
            ////debug modda çalışrken tabloları kilitlemez 
            //Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            //        Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppContext, Configuration>("AppContext"));
            //Database.SetInitializer<AppContext>(new CreateDatabaseIfNotExists<AppContext>());
#endif
        }

        //Appcontext Properties
        public DbSet<CSTSAYACMODELEf> CSTSAYACMODELEf { get; set; }
        public DbSet<CONDILEf> CONDILEf { get; set; }
        public DbSet<CONKURUMEf> CONKURUMEf { get; set; }
        public DbSet<CSTHUMARKAEf> CstHuMarkaEf { get; set; }

        public DbSet<CSTHUMODELEf> CstHuModelEf { get; set; }
        public DbSet<ENTABONEEf> ENTABONEEf { get; set; }
        public DbSet<ENTABONESAYACEf> ENTABONESAYACEf { get; set; }

   
        public DbSet<ENTHABERLESMEUNITESIEf> ENTHABERLESMEUNITESIEf { get; set; }
       
        public DbSet<ENTHUSAYACEf> ENTHUSAYACEf { get; set; }

        public DbSet<EntIsEmriEf> EntIsEmriEf { get; set; }
        public DbSet<ENTKOMUTLARSONUCLANANEf> ENTKOMUTLARSONUCLANANEf { get; set; }
        public DbSet<ENTKREDIKOMUTTAKIPEf> ENTKREDIKOMUTTAKIPEf { get; set; }
        public DbSet<ENTSAYACEf> ENTSAYACEf { get; set; }

        public DbSet<EntSayacOkumaVeriEf> EntSayacOkumaVeriEf { get; set; }

        public DbSet<ENTTUKETIMSUEf> EntTuketimSuEf { get; set; }
        public DbSet<ENTSATISEf> ENTSATISEf { get; set; }
        public DbSet<ENTSAYACSONDURUMSUEf> ENTSAYACSONDURUMSUEf { get; set; }
        public DbSet<PRMTARIFEELKEf> PRMTARIFEELKEf { get; set; }
        public DbSet<PRMTARIFEGAZEf> PRMTARIFEGAZEf { get; set; }
        public DbSet<PRMTARIFEKALORIMETREEf> PRMTARIFEKALORIMETREEf { get; set; }
        public DbSet<PRMTARIFESUEf> PRMTARIFESUEf { get; set; }
        public DbSet<RPTDASHBOARDEf> RPTDASHBOARDEf { get; set; }
        public DbSet<SYSCSTOPERASYONEf> SYSCSTOPERASYONEf { get; set; }
        public DbSet<SYSGOREVEf> SYSGOREVEf { get; set; }
        public DbSet<SYSGOREVROLEf> SYSGOREVROLEf { get; set; }
        public DbSet<SYSKULLANICIEf> SYSKULLANICIEf { get; set; }
    
        public DbSet<SYSMENUEf> SYSMENUEf { get; set; }
        public DbSet<SYSOPERASYONGOREVEf> SYSOPERASYONGOREVEf { get; set; }
        public DbSet<SYSROLEf> SYSROLEf { get; set; }
        public DbSet<SYSROLKULLANICIEf> SYSROLKULLANICIEf { get; set; }

        public DbSet<NESNEDEGEREf> NESNEDEGEREf { get; set; }
        public DbSet<NESNETIPEf> NESNETIPEf { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.HasDefaultSchema("GPRSSAYAC");
            modelBuilder.HasDefaultSchema("YONCAPROD");
            modelBuilder.Configurations.Add(new CSTSAYACMODELMap());
            modelBuilder.Configurations.Add(new CONDILMap());
            modelBuilder.Configurations.Add(new CONKURUMMap());
            modelBuilder.Configurations.Add(new CSTHUMARKAMap());
            modelBuilder.Configurations.Add(new CSTHUMODELMap());
            modelBuilder.Configurations.Add(new ENTABONEMap());
            modelBuilder.Configurations.Add(new ENTABONESAYACMap());
            modelBuilder.Configurations.Add(new ENTHABERLESMEUNITESIMap());
            modelBuilder.Configurations.Add(new ENTHUSAYACMap());
            modelBuilder.Configurations.Add(new EntIsEmriMap());
            modelBuilder.Configurations.Add(new ENTKOMUTLARSONUCLANANMap());
            modelBuilder.Configurations.Add(new ENTKREDIKOMUTTAKIPMap());
            modelBuilder.Configurations.Add(new ENTSAYACMap());
            modelBuilder.Configurations.Add(new EntSayacOkumaVeriMap());
            modelBuilder.Configurations.Add(new ENTTUKETIMSUMap());
            modelBuilder.Configurations.Add(new ENTSATISMap());
            modelBuilder.Configurations.Add(new ENTSAYACSONDURUMSUMap());
            modelBuilder.Configurations.Add(new PRMTARIFEELKMap());
            modelBuilder.Configurations.Add(new PRMTARIFEGAZMap());
            modelBuilder.Configurations.Add(new PRMTARIFEKALORIMETREMap());
            modelBuilder.Configurations.Add(new PRMTARIFESUMap());
            modelBuilder.Configurations.Add(new RPTDASHBOARDMap());
            modelBuilder.Configurations.Add(new SYSCSTOPERASYONMap());
            modelBuilder.Configurations.Add(new SYSGOREVMap());
            modelBuilder.Configurations.Add(new SYSGOREVROLMap());
            modelBuilder.Configurations.Add(new SYSKULLANICIMap());
            modelBuilder.Configurations.Add(new SYSMENUMap());
            modelBuilder.Configurations.Add(new SYSOPERASYONGOREVMap());
            modelBuilder.Configurations.Add(new SYSROLMap());
            modelBuilder.Configurations.Add(new SYSROLKULLANICIMap());
            modelBuilder.Configurations.Add(new NESNEDEGERMap());
            modelBuilder.Configurations.Add(new NESNETIPMap());
            //çoğul s takısının kapatılması
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }


    }
}
