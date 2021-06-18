using SCLibWin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartCard.Filistin
{
    #region yetki

    public struct YetkiKarti
    {
        public string[] DegiskenDegerleri;
        public byte BilgiTipi, KartKodu, Cap, Tip;

        public UInt32 CihazNo, AboneNo;
        public Int32 KalanKredi;
        public UInt32 HarcananKredi, KritikKredi, Limit1, Limit2, Limit3, Limit4, Katsayi1, Katsayi2, Katsayi3,
               Katsayi4, Katsayi5, DonemTuketimi, NegatifTuketim, GerCekTuketim, FixCharge, ToplamFixCharge;
        public Int32 SonYuklenenKredi;
        public UInt32 Kademe1Tuketimi, Kademe2Tuketimi, Kademe3Tuketimi, Kademe4Tuketimi, Kademe5Tuketimi,
               DonemTuketim1, DonemTuketim2, DonemTuketim3, DonemTuketim4, DonemTuketim5, DonemTuketim6;

        //4 Byte    
        public string SayacTarihi;
        //2 Byte
        public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, Ceza3Tarihi, Ceza4Tarihi,
        BorcTarihi, VanaAcilmaTarihi, VanaKapanmaTarihi;

        public string MotorPil;
        public byte HaftaninGunu;
        public string ArizaTipi, ETransDurum;
        public byte VanaPulseSure, VanaCntSure,
             KartNo, IslemNo, Vop, TestSaat;
        public string TestReel;
        public byte SayacDurumu, ResetSayisi;

        public string AnaPil, Format;

        public byte Versiyon, Kademe, DonemGun, DonemGunNo;

        //2 Byte
        public string MaxdebiTarihi;

        public UInt16 MaxdebiSeviye, MaxdebiSinir;

        public string VanaDurumu;

        public UInt32 DonemTuketim7, DonemTuketim8, DonemTuketim9, DonemTuketim10,
               DonemTuketim11, DonemTuketim12, MekanikTuketim;

        public byte HaftaSonuOnay;

        public string Degiskenler;

        byte Index;

        public YetkiKarti(byte[] OkunanDegerler)
        {
#if DEBUG
            Degiskenler = "BilgiTipi;KartKodu;Cap;Tip;CihazNo;AboneNo;KalanKredi;HarcananKredi;KritikKredi;Limit1;Limit2;Limit3;Limit4;Katsayi1;Katsayi2;Katsayi3;Katsayi4;Katsayi5;DonemTuketimi;NegatifTuketim;GerCekTuketim;SonYuklenenKredi;Kademe1Tuketimi;Kademe2Tuketimi;Kademe3Tuketimi;Kademe4Tuketimi;Kademe5Tuketimi;DonemTuketim1;DonemTuketim2;DonemTuketim3;DonemTuketim4;DonemTuketim5;DonemTuketim6;SayacTarihi;SonKrediTarihi;SonPulseTarihi;SonCezaTarihi;SonArizaTarihi;Ceza3Tarihi;Ceza4Tarihi;BorcTarihi;VanaAcilmaTarihi;VanaKapanmaTarihi;MotorPil;HaftaninGunu;ArizaTipi;ETransDurum;VanaPulseSure;VanaCntSure;KartNo;IslemNo;Vop;TestSaat;TestReel;SayacDurumu;AnaPil;Format;Versiyon;Kademe;DonemGun;DonemGunNo;MaxdebiTarihi;MaxdebiSeviye;MaxdebiSinir;VanaDurumu;DonemTuketim7;DonemTuketim8;DonemTuketim9;DonemTuketim10;DonemTuketim11;DonemTuketim12;Mektuk;HaftaSonuOnay;ResetSayısı;FixCharge;ToplamFixCharge";
#else
            Degiskenler = "Info Type;Card Code;Diameter;Type;Device No;Subscriber No;Remaining Crdt;Spent Crdt;Critic Crdt;Limit1;Limit2;Limit3;Limit4;Coefficient1;Coefficient2;Coefficient3;Coefficient4;Coefficient5;Period Consumption;Negative Consumption;Actual Consumption;Last Credit;Stage1 Cons;Stage1 Cons;Stage3 Cons;Stage4 Cons;Stage5 Cons;Per1 Cons;Per2 Cons;Per3 Cons;Per4 Cons;Per5 Cons;Per6 Cons;Meter Date;Last Crdt Date;Last Pulse Date;Last Penalty Date;Last Fault Date;Penalty3 Date;Penalty4 Date;Debt Date;Valve Open Date;Valve Close Date;Motor Battery;Day Of Week;Fault Type;ETrans State;Valve Pulse Duration;Valve Cnt Duration;Card No;Process No;Vop;Test Time;Test Reel;Meter State;Main Battery;Format;Version;Stage;Period/Day;Period Day No;Maxdebi date;Maxdebi Level;Maxdebi Limit;Valve State;Per7 Cons;Per8 Cons;Per9 Cons;Per10 Cons;Per11 Cons;Per12 Cons;Mechanical Consumption;Weekend Approval;Number Of Reset;FixCharge;TotalFixCharge";
#endif

            string[] Degerler = Degiskenler.Split(';');
            DegiskenDegerleri = new string[Degerler.Length];

            BilgiTipi = KartKodu = Cap = Tip = ResetSayisi = HaftaSonuOnay = 0;

            CihazNo = AboneNo = 0;
            KalanKredi = 0;
            HarcananKredi = KritikKredi = Limit1 = Limit2 = Limit3 = Limit4 = Katsayi1 = Katsayi2 =
            Katsayi3 = Katsayi4 = Katsayi5 = DonemTuketimi = NegatifTuketim = GerCekTuketim =
            FixCharge = ToplamFixCharge = 0;
            SonYuklenenKredi = 0;
            Kademe1Tuketimi = Kademe2Tuketimi = Kademe3Tuketimi = Kademe4Tuketimi = Kademe5Tuketimi =
            DonemTuketim1 = DonemTuketim2 = DonemTuketim3 = DonemTuketim4 = DonemTuketim5 = DonemTuketim6 = 0;
            //4 Byte    
            SayacTarihi = "";
            //2 Byte
            SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = Ceza3Tarihi = Ceza4Tarihi =
            BorcTarihi = VanaAcilmaTarihi = VanaKapanmaTarihi = "";

            MotorPil = "";

            HaftaninGunu = 0;
            ArizaTipi = ETransDurum = "";
            VanaPulseSure = VanaCntSure =
            KartNo = IslemNo = Vop = TestSaat = 0;
            TestReel = "";
            SayacDurumu = 0;

            AnaPil = Format = "";

            Versiyon = Kademe = DonemGun = DonemGunNo = 0;
            //2 Byte
            MaxdebiTarihi = "";

            MaxdebiSeviye = MaxdebiSinir = 0;

            VanaDurumu = "";

            DonemTuketim7 = DonemTuketim8 = DonemTuketim9 = DonemTuketim10 =
            DonemTuketim11 = DonemTuketim12 = MekanikTuketim = 0;



            HaftaSonuOnay = 0;

            Index = 0;

            //-----------------------------------------

            BilgiTipi = OkunanDegerler[Index];
            KartKodu = OkunanDegerler[Index + 1]; Index++;
            Cap = OkunanDegerler[Index + 1]; Index++;
            Tip = OkunanDegerler[Index + 1]; Index++;

            CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            AboneNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            KalanKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HarcananKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            KritikKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            NegatifTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            GerCekTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonYuklenenKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            Kademe1Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe2Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe3Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim5 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim6 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SayacTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            SonKrediTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonPulseTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonCezaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            SonArizaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza3Tarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            Ceza4Tarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            BorcTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaAcilmaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            VanaKapanmaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            MotorPil = String.Format("{0,8:0.000}", OkunanDegerler[Index + 1] * 6 / 255.0); Index++;

            HaftaninGunu = OkunanDegerler[Index + 1]; Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x10:
#if DEBUG
                    ArizaTipi = "ServisA";
#else
                    ArizaTipi = "ServiceA";
#endif
                    break;
                case 0x20:
#if DEBUG
                    ArizaTipi = "ServisK";
#else
                    ArizaTipi = "ServiceK";
#endif
                    break;
                case 0x30:
#if DEBUG
                    ArizaTipi = "ServisS";
#else
                    ArizaTipi = "ServiceS";
#endif
                    break;
                case 0x40:
#if DEBUG
                    ArizaTipi = "ServisP";
#else
                    ArizaTipi = "ServiceP";
#endif
                    break;
            }
            Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 11:
#if DEBUG
                    ETransDurum = "Olumlu";
#else
                    ETransDurum = "Positive";
#endif
                    break;
                default:
#if DEBUG
                    ETransDurum = "Olumsuz";
#else
                    ETransDurum = "Negative";
#endif
                    break;
            }
            Index++;

            VanaPulseSure = OkunanDegerler[Index + 1]; Index++;
            VanaCntSure = OkunanDegerler[Index + 1]; Index++;
            KartNo = OkunanDegerler[Index + 1]; Index++;
            IslemNo = OkunanDegerler[Index + 1]; Index++;
            Vop = OkunanDegerler[Index + 1]; Index++;
            TestSaat = OkunanDegerler[Index + 1]; Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x00:
                    TestReel = "Test Mode";
                    break;
                case 0xFF:
                    TestReel = "Reel Mode";
                    break;
            }
            Index++;

            SayacDurumu = OkunanDegerler[Index + 1]; Index++;

            AnaPil = String.Format("{0,8:0.000}", OkunanDegerler[Index + 1] * 6 / 255.0); Index++;

            switch (OkunanDegerler[Index + 1])
            {
                case 0x00:
#if DEBUG
                    Format = "Sayaç Formatlı";
#else
                    Format = "Meter formatted";
#endif
                    break;
                default:
#if DEBUG
                    Format = "Sayaç Formatsız";
#else
                    Format = "Meter not formatted";
#endif
                    break;
            }
            Index++;

            Versiyon = OkunanDegerler[Index + 1]; Index++;
            Kademe = OkunanDegerler[Index + 1]; Index++;
            DonemGun = OkunanDegerler[Index + 1]; Index++;
            DonemGunNo = OkunanDegerler[Index + 1]; Index++;

            MaxdebiTarihi = Hexcon.TarihDuzenle(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            MaxdebiSeviye = Hexcon.Byte2toUInt16(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;
            MaxdebiSinir = Hexcon.Byte2toUInt16(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2]); Index += 2;

            switch (OkunanDegerler[Index + 1])
            {
                case 0:
#if DEBUG
                    VanaDurumu = "Kapalı";
#else
                    VanaDurumu = "Closed";
#endif                    
                    break;
                case 1:
#if DEBUG
                    VanaDurumu = "Açık";
#else
                    VanaDurumu = "Open";
#endif
                    break;
            }
            Index++;

            DonemTuketim7 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim8 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim9 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim10 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim11 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            DonemTuketim12 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            MekanikTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            HaftaSonuOnay = OkunanDegerler[Index + 1]; Index++;
            ResetSayisi = OkunanDegerler[Index + 1]; Index++;
            Katsayi4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Katsayi5 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Limit4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe4Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            Kademe5Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            FixCharge = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
            ToplamFixCharge = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            #region Değişken Değerleri
            DegiskenDegerleri[0] = BilgiTipi.ToString();
            DegiskenDegerleri[1] = KartKodu.ToString();
            DegiskenDegerleri[2] = Cap.ToString();
            DegiskenDegerleri[3] = Tip.ToString();
            DegiskenDegerleri[4] = CihazNo.ToString();
            DegiskenDegerleri[5] = AboneNo.ToString();
            DegiskenDegerleri[6] = KalanKredi.ToString();
            DegiskenDegerleri[7] = HarcananKredi.ToString();
            DegiskenDegerleri[8] = KritikKredi.ToString();
            DegiskenDegerleri[9] = Limit1.ToString();
            DegiskenDegerleri[10] = Limit2.ToString();
            DegiskenDegerleri[11] = Limit3.ToString();
            DegiskenDegerleri[12] = Limit4.ToString();
            DegiskenDegerleri[13] = Katsayi1.ToString();
            DegiskenDegerleri[14] = Katsayi2.ToString();
            DegiskenDegerleri[15] = Katsayi3.ToString();
            DegiskenDegerleri[16] = Katsayi4.ToString();
            DegiskenDegerleri[17] = Katsayi5.ToString();
            DegiskenDegerleri[18] = DonemTuketimi.ToString();
            DegiskenDegerleri[19] = NegatifTuketim.ToString();
            DegiskenDegerleri[20] = GerCekTuketim.ToString();
            DegiskenDegerleri[21] = SonYuklenenKredi.ToString();
            DegiskenDegerleri[22] = Kademe1Tuketimi.ToString();
            DegiskenDegerleri[23] = Kademe2Tuketimi.ToString();
            DegiskenDegerleri[24] = Kademe3Tuketimi.ToString();
            DegiskenDegerleri[25] = Kademe4Tuketimi.ToString();
            DegiskenDegerleri[26] = Kademe5Tuketimi.ToString();
            DegiskenDegerleri[27] = DonemTuketim1.ToString();
            DegiskenDegerleri[28] = DonemTuketim2.ToString();
            DegiskenDegerleri[29] = DonemTuketim3.ToString();
            DegiskenDegerleri[30] = DonemTuketim4.ToString();
            DegiskenDegerleri[31] = DonemTuketim5.ToString();
            DegiskenDegerleri[32] = DonemTuketim6.ToString();
            DegiskenDegerleri[33] = SayacTarihi.ToString();
            DegiskenDegerleri[34] = SonKrediTarihi.ToString();
            DegiskenDegerleri[35] = SonPulseTarihi.ToString();
            DegiskenDegerleri[36] = SonCezaTarihi.ToString();
            DegiskenDegerleri[37] = SonArizaTarihi.ToString();
            DegiskenDegerleri[38] = Ceza3Tarihi.ToString();
            DegiskenDegerleri[39] = Ceza4Tarihi.ToString();
            DegiskenDegerleri[40] = BorcTarihi.ToString();
            DegiskenDegerleri[41] = VanaAcilmaTarihi.ToString();
            DegiskenDegerleri[42] = VanaKapanmaTarihi.ToString();
            DegiskenDegerleri[43] = MotorPil.ToString();
            DegiskenDegerleri[44] = HaftaninGunu.ToString();
            DegiskenDegerleri[45] = ArizaTipi.ToString();
            DegiskenDegerleri[46] = ETransDurum.ToString();
            DegiskenDegerleri[47] = VanaPulseSure.ToString();
            DegiskenDegerleri[48] = VanaCntSure.ToString();
            DegiskenDegerleri[49] = KartNo.ToString();
            DegiskenDegerleri[50] = IslemNo.ToString();
            DegiskenDegerleri[51] = Vop.ToString();
            DegiskenDegerleri[52] = TestSaat.ToString();
            DegiskenDegerleri[53] = TestReel.ToString();
            DegiskenDegerleri[54] = SayacDurumu.ToString();
            DegiskenDegerleri[55] = AnaPil.ToString();
            DegiskenDegerleri[56] = Format.ToString();
            DegiskenDegerleri[57] = Versiyon.ToString();
            DegiskenDegerleri[58] = Kademe.ToString();
            DegiskenDegerleri[59] = DonemGun.ToString();
            DegiskenDegerleri[60] = DonemGunNo.ToString();
            DegiskenDegerleri[61] = MaxdebiTarihi.ToString();
            DegiskenDegerleri[62] = MaxdebiSeviye.ToString();
            DegiskenDegerleri[63] = MaxdebiSinir.ToString();
            DegiskenDegerleri[64] = VanaDurumu.ToString();
            DegiskenDegerleri[65] = DonemTuketim7.ToString();
            DegiskenDegerleri[66] = DonemTuketim8.ToString();
            DegiskenDegerleri[67] = DonemTuketim9.ToString();
            DegiskenDegerleri[68] = DonemTuketim10.ToString();
            DegiskenDegerleri[69] = DonemTuketim11.ToString();
            DegiskenDegerleri[70] = DonemTuketim12.ToString();
            DegiskenDegerleri[71] = MekanikTuketim.ToString();
            DegiskenDegerleri[72] = HaftaSonuOnay.ToString();
            DegiskenDegerleri[73] = ResetSayisi.ToString();
            DegiskenDegerleri[74] = FixCharge.ToString();
            DegiskenDegerleri[75] = ToplamFixCharge.ToString();

            #endregion
        }
    }

    #endregion
    [ClassInterface(ClassInterfaceType.None)]
    public class PalestineWater : IAddInterface
    {

        Util util = new Util();
        public Int32 zone;

        #region Bufferlar
        byte[] inBuf = new byte[128];
        //    byte[] outBuf = new byte[128];
        byte[] buffer = new byte[100];
        #endregion

        string sp = "#";
        string _hata;

        SCResMgr mng;
        SCReader rd;
        string rdName;

        #region değişkenler

        byte[] Yazilacak = new byte[24];
        private static byte[] Okunacak = new byte[64];

        byte[] MasterKey = { 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23, 0x23 };
        byte[] DKAbone = { 0x12, 0x53, 0x83, 0x26, 0x26, 0x61, 0x15, 0x98, 0x42, 0x11, 0x45, 0x76, 0x13, 0x62, 0x77, 0x19 };
        byte[] PinYetki = { 0x34, 0x61, 0x29, 0x56, 0x25, 0x07, 0x13, 0x68 };
        byte[] PinUretim = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        byte[] PinAbone = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        string DFName = "NN";
        string EFIssuerName = "EI";
        string EFDataName1 = "E1";
        string[] issue_area = new string[2];

        byte[] YetkiYazilacak = new byte[12];
        byte[] UretimYazilacak = new byte[15];
        byte[] UretimYazilacak5Byte = new byte[5];
        byte[] YetkiAvansDizi = new byte[24];
        byte[] Yetki = new byte[256];

        public byte KontrolDegeri;

        #endregion

        #region Structlar

        public struct Abone000023
        {
            public UInt32 CihazNo, Katsayi1, Katsayi2, Katsayi3, Limit1, Limit2;

            public Abone000023(byte[] OkunanDegerler)
            {
                CihazNo = Katsayi1 = Katsayi2 = Katsayi3 = Limit1 = Limit2 = 0;

                CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                Katsayi1 = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                Katsayi2 = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
                Katsayi3 = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
                Limit1 = Hexcon.Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
                Limit2 = Hexcon.Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
            }
        }

        public struct Abone024051
        {
            public Int32 AnaKredi, YedekKredi, AboneNo;
            public byte AkoYko, IslemNo, KartNo, Cap, Tip, DonemGun, VanaPulseSure, VanaCntSure, Iade,
            MaxdebiSiniri, HaftaSonuOnay, bos;
            public UInt16 Bayram1, Bayram2;

            public Abone024051(byte[] OkunanDegerler)
            {
                AnaKredi = YedekKredi = AboneNo = 0;
                AkoYko = IslemNo = KartNo = Cap = Tip = DonemGun = VanaPulseSure = VanaCntSure = Iade =
                MaxdebiSiniri = HaftaSonuOnay = bos = 0;
                Bayram1 = Bayram2 = 0;

                AnaKredi = Hexcon.Byte4toInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                YedekKredi = Hexcon.Byte4toInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                AkoYko = OkunanDegerler[8];
                IslemNo = OkunanDegerler[9];
                KartNo = OkunanDegerler[10];
                Cap = OkunanDegerler[11];
                Tip = OkunanDegerler[12];
                DonemGun = OkunanDegerler[13];
                VanaPulseSure = OkunanDegerler[14];
                VanaCntSure = OkunanDegerler[15];
                Iade = OkunanDegerler[16];
                MaxdebiSiniri = OkunanDegerler[17];
                HaftaSonuOnay = OkunanDegerler[19];
                Bayram1 = Hexcon.Byte2toUInt16(OkunanDegerler[24], OkunanDegerler[25]);
                Bayram2 = Hexcon.Byte2toUInt16(OkunanDegerler[26], OkunanDegerler[27]);
            }
        }

        public struct Abone051070
        {
            public UInt32 Katsayi4, Katsayi5, Limit3, Limit4, FixCharge;

            public Abone051070(byte[] OkunanDegerler)
            {
                Katsayi4 = Katsayi5 = Limit3 = Limit4 = FixCharge = 0;

                Katsayi4 = Hexcon.Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                Katsayi5 = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                Limit3 = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
                Limit4 = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
                FixCharge = Hexcon.Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
                FixCharge /= 10000;
            }
        }

        public struct Abone072095
        {
            public Int32 KalanKredi;

            public UInt32 HarcananKredi, GercekTuketim, KademeTuketim1, KademeTuketim2, KademeTuketim3;

            public Abone072095(byte[] OkunanDegerler)
            {
                KalanKredi = 0;
                HarcananKredi = GercekTuketim = KademeTuketim1 = KademeTuketim2 = KademeTuketim3 = 0;

                KalanKredi = Hexcon.Byte4toInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                HarcananKredi = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                GercekTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
                KademeTuketim1 = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
                KademeTuketim2 = Hexcon.Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
                KademeTuketim3 = Hexcon.Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
            }
        }

        public struct Abone096119
        {
            public string SayacTarihi;
            public UInt32 NegatifTuketim, DonemTuketimi, DonemTuketimi1, DonemTuketimi2, DonemTuketimi3;

            public Abone096119(byte[] OkunanDegerler)
            {
                SayacTarihi = "";
                NegatifTuketim = DonemTuketimi = DonemTuketimi1 = DonemTuketimi2 = DonemTuketimi3 = 0;

                SayacTarihi = Hexcon.TarihDuzenle(OkunanDegerler[0], OkunanDegerler[1]);

                NegatifTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                DonemTuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
                DonemTuketimi1 = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
                DonemTuketimi2 = Hexcon.Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
                DonemTuketimi3 = Hexcon.Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
            }
        }

        public struct Abone120143
        {
            public UInt32 DonemTuketimi4, DonemTuketimi5, DonemTuketimi6;
            public string SonKrediTarihi, SonPulseTarihi, SonCezaTarihi, SonArizaTarihi, BorcTarihi, VanaAcmaTarihi;

            public Abone120143(byte[] OkunanDegerler)
            {
                DonemTuketimi4 = DonemTuketimi5 = DonemTuketimi6 = 0;
                SonKrediTarihi = SonPulseTarihi = SonCezaTarihi = SonArizaTarihi = BorcTarihi = VanaAcmaTarihi = "";

                DonemTuketimi4 = Hexcon.Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                DonemTuketimi5 = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                DonemTuketimi6 = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);

                SonKrediTarihi = Hexcon.TarihDuzenle(OkunanDegerler[12], OkunanDegerler[13]);
                SonPulseTarihi = Hexcon.TarihDuzenle(OkunanDegerler[14], OkunanDegerler[15]);
                SonCezaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[16], OkunanDegerler[17]);
                SonArizaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[18], OkunanDegerler[19]);
                BorcTarihi = Hexcon.TarihDuzenle(OkunanDegerler[20], OkunanDegerler[21]);
                VanaAcmaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[22], OkunanDegerler[23]);
            }
        }

        public struct Abone144171
        {
            public string VanaKapamaTarihi;
            public byte Versiyon, VanaOperasyonSayisi, SayacDurumu, AnaPil, DonemGun, KartHata, HaftaninGunu, VanaDurumu, ArizaTipi, MaxdebiSeviyesi;
            public string SonTakilanYetkiKartiOzellik1, SonTakilanYetkiKartiOzellik2, SonTakilanYetkiKartiOzellik3, MaxDebiTarihi;

            public Abone144171(byte[] OkunanDegerler)
            {
                VanaKapamaTarihi = MaxDebiTarihi = "";
                Versiyon = VanaOperasyonSayisi = SayacDurumu = AnaPil = DonemGun = KartHata = HaftaninGunu = VanaDurumu = ArizaTipi = MaxdebiSeviyesi = 0;
                SonTakilanYetkiKartiOzellik1 = SonTakilanYetkiKartiOzellik2 = SonTakilanYetkiKartiOzellik3 = "";

                VanaKapamaTarihi = Hexcon.TarihDuzenle(OkunanDegerler[0], OkunanDegerler[1]);
                Versiyon = OkunanDegerler[2];
                VanaOperasyonSayisi = OkunanDegerler[3];

                SayacDurumu = OkunanDegerler[4];
                AnaPil = OkunanDegerler[5];
                DonemGun = OkunanDegerler[6];
                KartHata = OkunanDegerler[7];
                HaftaninGunu = OkunanDegerler[8];
                VanaDurumu = OkunanDegerler[9];
                ArizaTipi = OkunanDegerler[10];
                MaxdebiSeviyesi = OkunanDegerler[11];

                SonTakilanYetkiKartiOzellik1 = Hexcon.TarihDuzenle(OkunanDegerler[12], OkunanDegerler[13]) + " - " +
                                               OkunanDegerler[14].ToString() + " - " + OkunanDegerler[15].ToString();
                SonTakilanYetkiKartiOzellik2 = Hexcon.TarihDuzenle(OkunanDegerler[16], OkunanDegerler[17]) + " - " +
                                               OkunanDegerler[18].ToString() + " - " + OkunanDegerler[19].ToString();
                SonTakilanYetkiKartiOzellik3 = Hexcon.TarihDuzenle(OkunanDegerler[20], OkunanDegerler[21]) + " - " +
                                               OkunanDegerler[22].ToString() + " - " + OkunanDegerler[23].ToString();

                MaxDebiTarihi = Hexcon.TarihDuzenle(OkunanDegerler[24], OkunanDegerler[25]);
            }
        }

        public struct Abone170197
        {

            public UInt32 DonemTuketimi7, DonemTuketimi8, DonemTuketimi9, DonemTuketimi10, DonemTuketimi11, DonemTuketimi12;
            public UInt16 DonemTuketimi13, DonemTuketimi14;


            public Abone170197(byte[] OkunanDegerler)
            {
                DonemTuketimi7 = DonemTuketimi8 = DonemTuketimi9 = DonemTuketimi10 = DonemTuketimi11 = DonemTuketimi12 = 0;
                DonemTuketimi13 = DonemTuketimi14 = 0;

                DonemTuketimi7 = Hexcon.Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                DonemTuketimi8 = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                DonemTuketimi9 = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
                DonemTuketimi10 = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);
                DonemTuketimi11 = Hexcon.Byte4toUInt32(OkunanDegerler[16], OkunanDegerler[17], OkunanDegerler[18], OkunanDegerler[19]);
                DonemTuketimi12 = Hexcon.Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
                DonemTuketimi13 = Hexcon.Byte2toUInt16(OkunanDegerler[24], OkunanDegerler[25]);
                DonemTuketimi14 = Hexcon.Byte2toUInt16(OkunanDegerler[26], OkunanDegerler[27]);
            }
        }

        public struct Abone198226
        {
            public UInt16 DonemTuketimi15, DonemTuketimi16, DonemTuketimi17, DonemTuketimi18;
            public UInt16 DonemTuketimi19, DonemTuketimi20, DonemTuketimi21, DonemTuketimi22, DonemTuketimi23, DonemTuketimi24;
            public UInt32 Mektuk;
            public Int32 IadeKalan;
            public byte ResetSayisi;

            public Abone198226(byte[] OkunanDegerler)
            {
                DonemTuketimi15 = DonemTuketimi16 = DonemTuketimi17 = DonemTuketimi18 = 0;
                DonemTuketimi19 = DonemTuketimi20 = DonemTuketimi21 = DonemTuketimi22 = DonemTuketimi23 = DonemTuketimi24 = 0;
                Mektuk = 0;
                IadeKalan = 0;
                ResetSayisi = 0;

                DonemTuketimi15 = Hexcon.Byte2toUInt16(OkunanDegerler[0], OkunanDegerler[1]);
                DonemTuketimi16 = Hexcon.Byte2toUInt16(OkunanDegerler[2], OkunanDegerler[3]);
                DonemTuketimi17 = Hexcon.Byte2toUInt16(OkunanDegerler[4], OkunanDegerler[5]);
                DonemTuketimi18 = Hexcon.Byte2toUInt16(OkunanDegerler[6], OkunanDegerler[7]);
                DonemTuketimi19 = Hexcon.Byte2toUInt16(OkunanDegerler[8], OkunanDegerler[9]);
                DonemTuketimi20 = Hexcon.Byte2toUInt16(OkunanDegerler[10], OkunanDegerler[11]);
                DonemTuketimi21 = Hexcon.Byte2toUInt16(OkunanDegerler[12], OkunanDegerler[13]);
                DonemTuketimi22 = Hexcon.Byte2toUInt16(OkunanDegerler[14], OkunanDegerler[15]);
                DonemTuketimi23 = Hexcon.Byte2toUInt16(OkunanDegerler[16], OkunanDegerler[17]);
                DonemTuketimi24 = Hexcon.Byte2toUInt16(OkunanDegerler[18], OkunanDegerler[19]);
                Mektuk = Hexcon.Byte4toUInt32(OkunanDegerler[20], OkunanDegerler[21], OkunanDegerler[22], OkunanDegerler[23]);
                IadeKalan = Hexcon.Byte4toInt32(OkunanDegerler[24], OkunanDegerler[25], OkunanDegerler[26], OkunanDegerler[27]);
                ResetSayisi = OkunanDegerler[28];
            }
        }

        public struct Abone228239
        {
            public UInt32 KademeTuketim4, KademeTuketim5, TotalFixCharge;

            public Abone228239(byte[] OkunanDegerler)
            {
                KademeTuketim4 = KademeTuketim5 = TotalFixCharge = 0;

                KademeTuketim4 = Hexcon.Byte4toUInt32(OkunanDegerler[0], OkunanDegerler[1], OkunanDegerler[2], OkunanDegerler[3]);
                KademeTuketim5 = Hexcon.Byte4toUInt32(OkunanDegerler[4], OkunanDegerler[5], OkunanDegerler[6], OkunanDegerler[7]);
                TotalFixCharge = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            }
        }

        public struct AkoYkoIslemNoKartNoCapTip
        {
            public byte AkoYko, IslemNo, KartNo, Cap, Tip, MaxDebiSiniri, Iade;
            public UInt32 AboneNo;

            public AkoYkoIslemNoKartNoCapTip(byte[] OkunanDegerler)
            {
                AkoYko = IslemNo = KartNo = Cap = Tip = MaxDebiSiniri = 0;
                AboneNo = 0;

                AkoYko = OkunanDegerler[0];
                IslemNo = OkunanDegerler[1];
                KartNo = OkunanDegerler[2];
                Cap = OkunanDegerler[3];
                Tip = OkunanDegerler[4];
                Iade = OkunanDegerler[8];
                MaxDebiSiniri = OkunanDegerler[9];

                AboneNo = Hexcon.Byte4toUInt32(OkunanDegerler[12], OkunanDegerler[13], OkunanDegerler[14], OkunanDegerler[15]);

            }
        }

        public struct IssuerAreaa
        {
            public string IssuerArea;
            public UInt32 CihazNo;
            public IssuerAreaa(string[] IssuerDegerleri)
            {
                if (IssuerDegerleri[0] == "1")
                {
                    IssuerArea = IssuerDegerleri[1];
                    CihazNo = Convert.ToUInt32(IssuerDegerleri[2]);
                }
                else
                {
                    IssuerArea = "";
                    CihazNo = 0;
                }
            }
        }

        public struct YetkiKartiTuketim
        {
            public byte Durum, KartKodu, DonemGun, DonemGunNo;

            public UInt32 CihazNo, AboneNo;
            public Int32 KalanKredi;
            public UInt32 HarcananKredi, DonemTuketimi, NegatifTuketim, GercekTuketim;
            public Int32 SonYuklenenKredi;
            public UInt32 Kademe1Tuketimi, Kademe2Tuketimi, Kademe3Tuketimi,
                          DonemTuketim1, DonemTuketim2, DonemTuketim3, DonemTuketim4, DonemTuketim5, DonemTuketim6,
                          DonemTuketim7, DonemTuketim8, DonemTuketim9, DonemTuketim10, DonemTuketim11, DonemTuketim12,
                          DonemTuketim13, DonemTuketim14, DonemTuketim15, DonemTuketim16, DonemTuketim17, DonemTuketim18,
                          DonemTuketim19, DonemTuketim20, DonemTuketim21, DonemTuketim22, DonemTuketim23, DonemTuketim24;

            byte Index;

            public YetkiKartiTuketim(byte[] OkunanDegerler)
            {
                Durum = KartKodu = DonemGun = DonemGunNo = 0;

                CihazNo = AboneNo = 0;
                KalanKredi = 0;
                HarcananKredi = DonemTuketimi = NegatifTuketim = GercekTuketim = 0;
                SonYuklenenKredi = 0;
                Kademe1Tuketimi = Kademe2Tuketimi = Kademe3Tuketimi =
                DonemTuketim1 = DonemTuketim2 = DonemTuketim3 = DonemTuketim4 = DonemTuketim5 = DonemTuketim6 =
                DonemTuketim7 = DonemTuketim8 = DonemTuketim9 = DonemTuketim10 = DonemTuketim11 = DonemTuketim12 =
                DonemTuketim13 = DonemTuketim14 = DonemTuketim15 = DonemTuketim16 = DonemTuketim17 = DonemTuketim18 =
                DonemTuketim19 = DonemTuketim20 = DonemTuketim21 = DonemTuketim22 = DonemTuketim23 = DonemTuketim24 = 0;

                Index = 0;

                //-----------------------------------------

                Durum = OkunanDegerler[Index];
                KartKodu = OkunanDegerler[Index + 1]; Index++;
                DonemGun = OkunanDegerler[Index + 1]; Index++;
                DonemGunNo = OkunanDegerler[Index + 1]; Index++;

                CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                AboneNo = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                KalanKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                HarcananKredi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                NegatifTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                GercekTuketim = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                SonYuklenenKredi = Hexcon.Byte4toInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                Kademe1Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                Kademe2Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                Kademe3Tuketimi = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                DonemTuketim1 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim2 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim3 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim4 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim5 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim6 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim7 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim8 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim9 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim10 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim11 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim12 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

                DonemTuketim13 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim14 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim15 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim16 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim17 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim18 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim19 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim20 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim21 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim22 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim23 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;
                DonemTuketim24 = Hexcon.Byte4toUInt32(OkunanDegerler[Index + 1], OkunanDegerler[Index + 2], OkunanDegerler[Index + 3], OkunanDegerler[Index + 4]); Index += 4;

            }
        }

        #endregion

        public PalestineWater()
        {
            //zone = Zone;
            //GetIssuer(zone);
        }

        private void GetIssuer(Int32 zone)
        {

            ////deneme Kodu
            //issue_area[0] = "E";
            //issue_area[1] = "M";
            ////deneme Kodu

            byte ZoneIndex = Convert.ToByte(zone.ToString());
            char issuechar;

            if ((ZoneIndex > 0) && (ZoneIndex < 37))
            {
                issue_area[0] = "P";

                if (ZoneIndex < 11)
                {
                    issue_area[1] = Convert.ToString(ZoneIndex - 1);
                }
                else
                {
                    issuechar = Convert.ToChar(Convert.ToByte('A') + ZoneIndex - 11);
                    issue_area[1] = "";
                    issue_area[1] += issuechar;
                }
            }
            else
            {
                if ((ZoneIndex >= 37) && (ZoneIndex < 73))
                {
                    issue_area[0] = "Q";

                    if (ZoneIndex < 47)
                    {
                        issue_area[1] = Convert.ToString(ZoneIndex - 37);
                    }
                    else
                    {
                        issuechar = Convert.ToChar(Convert.ToByte('A') + ZoneIndex - 47);
                        issue_area[1] = "";
                        issue_area[1] += issuechar;
                    }
                }
                else
                {
                    if ((ZoneIndex >= 73) && (ZoneIndex < 109))
                    {
                        issue_area[0] = "R";

                        if (ZoneIndex < 83)
                        {
                            issue_area[1] = Convert.ToString(ZoneIndex - 73);
                        }
                        else
                        {
                            issuechar = Convert.ToChar(Convert.ToByte('A') + ZoneIndex - 83);
                            issue_area[1] = "";
                            issue_area[1] += issuechar;
                        }
                    }
                }
            }
        }

        public string HataMesaji
        {
            get
            {
                return _hata;
            }
            set
            {
                _hata = value;
            }
        }

        private void HataSet(int Kod)
        {
            _hata = Kod + ":";

#if DEBUG
            switch (Kod)
            {
                case 0:
                    _hata += "Başarılı";
                    break;
                case 1:
                    _hata += "Kart bağlantı hata";
                    break;
                case 2:
                    _hata += "Kart DF hata";
                    break;
                case 3:
                    _hata += "Kart Ext. Auth. hata";
                    break;
                case 4:
                    _hata += "Kart EFIssuerName hata";
                    break;
                case 5:
                    _hata += "Kart Issuer okuma hata";
                    break;
                case 6:
                    _hata += "Kart EFDataName1 hata";
                    break;
                case 7:
                    _hata += "Abone Kartı değil";
                    break;
                case 8:
                    _hata += "Kart pini doğrulanamadı";
                    break;
                case 9:
                    _hata += "Kart pini yazılamadı";
                    break;
                case 10:
                    _hata += "Issuer ve CihazNo yazılamadı";
                    break;
                case 11:
                    _hata += "Kart boş değil";
                    break;
                case 12:
                    _hata += "Karttaki CihazNo ile Gönderilen CihazNo bilgisi uyuşmuyor";
                    break;
                case 13:
                    _hata += "Hatalı çap bilgisi : Karttaki çapla gönderilen çap bilgisi uyuşmuyor";
                    break;
                case 14:
                    _hata += "Parametre yazma hatası";
                    break;
                case 15:
                    _hata += "İade yazma hatası";
                    break;
                case 16:
                    _hata += "Kart boşaltılamadı";
                    break;
                case 17:
                    _hata += "Issuer silinemedi";
                    break;
                case 18:
                    _hata += "Yetki kartı değil";
                    break;
                case 19:
                    _hata += "Yetki bilgi kartı değil";
                    break;
                case 20:
                    _hata += "Yetki kartı pini doğrulanamadı";
                    break;
                case 21:
                    _hata += "Yetki bilgi kartı sayaca takılmamış";
                    break;
                case 22:
                    _hata += "Yetki tüketim kartı değil";
                    break;
                case 23:
                    _hata += "Üretim kartı değil";
                    break;
                case 24:
                    _hata += "Üretim kartı pini doğrulanamadı";
                    break;
                case 25:
                    _hata += "Hatalı Parametre(0) : Fiyatları kontrol ediniz";
                    break;
                case 26:
                    _hata += "Hatalı Parametre(0) : Limitleri kontrol ediniz";
                    break;
                case 27:
                    _hata += "Hatalı Parametre : Çapı kontrol ediniz";
                    break;
                case 28:
                    _hata += "Hatalı Parametre : Çapı değeri;15,20,25,40,50,65,80,100,150,200 olabilir";
                    break;
                case 29:
                    _hata += "Boş Kart";
                    break;
                case 30:
                    _hata += "Farklı Bölge Kartı";
                    break;
                case 31:
                    _hata += "Yetki olay kartı değil";
                    break;
                case 32:
                    _hata += "Yetki olay kartı sayaca takılmamış";
                    break;

                default:
                    _hata = "0:";
                    _hata += "Başarılı";
                    break;
            }
#else
            switch (Kod)
            {
                case 0:
                    _hata += "Successful";
                    break;
                case 1:
                    _hata += "Card connection error";
                    break;
                case 2:
                    _hata += "Card DF error";
                    break;
                case 3:
                    _hata += "Card Ext. Auth. error";
                    break;
                case 4:
                    _hata += "Card EFIssuerName error";
                    break;
                case 5:
                    _hata += "Card Issuer read error";
                    break;
                case 6:
                    _hata += "Card EFDataName1 error";
                    break;
                case 7:
                    _hata += "Not Subscriber Card";
                    break;
                case 8:
                    _hata += "Card pin not verified";
                    break;
                case 9:
                    _hata += "Card pin not write";
                    break;
                case 10:
                    _hata += "Issuer and Device Number not write";
                    break;
                case 11:
                    _hata += "Card is not empty";
                    break;
                case 12:
                    _hata += "Card Device Number and Parameter Device Number is mismatch";
                    break;
                case 13:
                    _hata += "Diameter error : Card diameter and Parameter Diameter is mismatch";
                    break;
                case 14:
                    _hata += "Parameter write error";
                    break;
                case 15:
                    _hata += "Payback Card write error";
                    break;
                case 16:
                    _hata += "Card couldn't be erased";
                    break;
                case 17:
                    _hata += "Issuer couldn't be erased";
                    break;
                case 18:
                    _hata += "Not Authorization Card";
                    break;
                case 19:
                    _hata += "Not Authorization Info Card";
                    break;
                case 20:
                    _hata += "Authorization Card Pin not verified";
                    break;
                case 21:
                    _hata += "Authorization Card not plugged to the meter";
                    break;
                case 22:
                    _hata += "Not Authorization Consumption Card";
                    break;
                case 23:
                    _hata += "Not Production Card";
                    break;
                case 24:
                    _hata += "Production Card Pin not verified";
                    break;
                case 25:
                    _hata += "Parameter Error (0) : Check prices";
                    break;
                case 26:
                    _hata += "Parameter Error (0) : Check Limits";
                    break;
                case 27:
                    _hata += "Parameter Error : Check diameter";
                    break;
                case 28:
                    _hata += "Parameter Error  : Diameter can be : 15,20,25,40,50,65,80,100,150,200";
                    break;
                case 29:
                    _hata += "Empty Card";
                    break;
                case 30:
                    _hata += "Different Zone Card";
                    break;
                case 31:
                    _hata += "Not Authorization Log Card";
                    break;
                case 32:
                    _hata += "Authorization Log Card not plugged to the meter";
                    break;

                default:
                    _hata = "0:";
                    _hata += "Successful";
                    break;
            }
#endif

        }

        private string Return(int Kod)
        {
            HataSet(Kod);
            return FinishKart();
        }

        #region Acr128 Kart Reader fonksyonları

        private int InitKart()
        {
            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.System);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);

                foreach (string st in al)
                {
                    rdName = st;
                    if (!rd.IsConnected) rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.T1);
                    else break;
                }



                if (rd.IsConnected) rd.BeginTransaction();

                return Convert.ToInt32(rd.IsConnected);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(rd.IsConnected);
            }
        }

        private string FinishKart()
        {
            try
            {
                if (rd.IsConnected)
                {
                    rd.EndTransaction(SCardDisposition.UnpowerCard);
                    rd.Disconnect(SCardDisposition.UnpowerCard);
                    mng.ReleaseContext();

                }

            }
            catch (Exception ex)
            {
                return "0";
            }

            return "0";
        }

        private bool SelectFile(string File)
        {
            byte[] command = new byte[7];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0XA4; //INS
            command[2] = 0X00; //P1 SC : Selection Kontrol
            command[3] = 0X0C; //P2 SO : Selection Options
            command[4] = 0X02; // Lc : Command Length
            // DATA   

            char[] ID = File.ToCharArray();
            byte[] Veriler = { Convert.ToByte(ID[0]), Convert.ToByte(ID[1]) };

            Array.Copy(Veriler, 0, command, 5, 2);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private bool ExternalAuthenticate(byte KeyId, byte[] PinAbone)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0X82; //INS
            command[2] = 0X00; //P1 Symmetric Algorithm
            command[3] = KeyId; //P2 KeyId
            command[4] = 0X08; // Lc : Command Length
            // PİN   

            Array.Copy(PinAbone, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private byte[] GetChallenge(byte[] PinAbone)
        {

            byte[] command = new byte[5];
            byte[] response = new byte[8];

            byte[] RandomSayi = new byte[8];
            byte[] kriptoPin;

            command[0] = 0X00; //CLA
            command[1] = 0X84; //INS
            command[2] = 0X00; //P1 
            command[3] = 0X00; //P2
            command[4] = 0X08; // Le : Command Length
            // PİN   

            try
            {
                rd.Transmit(command, out response);
                if (response[8] == 0X90 && response[9] == 0X00)
                {
                    Array.Copy(response, 0, RandomSayi, 0, 8);
                    kriptoPin = new byte[8];

                    return classDes.TripleDes(DKAbone, RandomSayi, kriptoPin);
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }


            return kriptoPin;
        }

        private byte[] ReadBinary(UInt16 Ofset, byte Adet)
        {
            byte[] command = new byte[5];
            byte[] response = new byte[2];

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XB0; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Adet; // Le : Number of digits to be read  


            try
            {
                rd.Transmit(command, out response);
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return response;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            return null;
        }

        private bool VerifyPin(byte KeyID, byte[] Sifre)
        {
            byte[] command = new byte[13];
            byte[] response = new byte[2];

            command[0] = 0X00; //CLA
            command[1] = 0X20; //INS
            command[2] = 0X00; //P1 
            command[3] = KeyID; //P2 KeyId
            command[4] = 0X08; // Lc : Command Length
            // PİN   

            Array.Copy(Sifre, 0, command, 5, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        private bool UpdateBinary(UInt16 Ofset, params byte[] data)
        {
            byte[] command = new byte[5 + data.Length];
            byte[] response = new byte[2];

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XD6; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Convert.ToByte(data.Length); // Le : Number of digits to be write  

            Array.Copy(data, 0, command, 5, data.Length);

            try
            {
                rd.Transmit(command, out response);
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
        private bool UpdateBinaryAdet(UInt16 Ofset, byte Adet, params byte[] data)
        {
            byte[] Veriler = new byte[Adet];
            byte[] command = new byte[5 + Veriler.Length];
            byte[] response = new byte[2];


            Array.Copy(data, Ofset, Veriler, 0, Adet);

            Integer2Byte Index = new Integer2Byte(Ofset);

            command[0] = 0X00; //CLA
            command[1] = 0XD6; //INS
            command[2] = Index.iki; //P1, OH = Offset High Byte for implicit selection 
            command[3] = Index.bir; //P2, OL = Offset Low Byte
            command[4] = Convert.ToByte(Veriler.Length); // Le : Number of digits to be write  

            Array.Copy(Veriler, 0, command, 5, Veriler.Length);

            try
            {
                rd.Transmit(command, out response);
                if (response[response.Length - 2] == 0X90 && response[response.Length - 1] == 0X00)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        private bool EFDataName1Eris(UInt32 CihazNo)
        {
            byte[] PinAbone = new byte[8];

            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);

            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            bool Status = VerifyPin(0x83, PinAbone);
            if (!Status) { return false; }

            Status = SelectFile(EFDataName1);
            if (!Status) { return false; }

            return true;
        }

        private bool EFDataName1Eris(string KartTipi)
        {
            bool Status = true;

            switch (KartTipi)
            {
                case "Uretim":
                    Status = VerifyPin(0x81, PinUretim);
                    if (!Status) { return false; }
                    break;
                case "Yetki":
                    Status = VerifyPin(0x82, PinYetki);
                    if (!Status) { return false; }
                    break;
            }

            Status = SelectFile(EFDataName1);
            if (!Status) { return false; }

            return true;
        }

        private string[] EFDataName1DosyasinaErisimAyarlari()
        {
            string[] DonusDegeri = { "0", "0", "0" };

            bool Status = SelectFile(DFName);
            if (!Status) { DonusDegeri[0] = "0"; return DonusDegeri; }

            Status = ExternalAuthenticate(0x84, GetChallenge(DKAbone));
            if (!Status) { DonusDegeri[0] = "0"; return DonusDegeri; }

            Status = SelectFile(EFIssuerName);
            if (!Status) { DonusDegeri[0] = "0"; return DonusDegeri; }

            /*byte[] a = { 66, 84, 76, 68, 0, 0, 0, 0, 0, 0, 0, 0 };//BootLoader
            Status = classPosDuoFonksiyonlari.UpdateBinary(0, a);
            if (!Status) { DonusDegeri[0] = "0"; return DonusDegeri; }*/

            /*byte[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };//IssuerAreaBoşalt
            Status = classPosDuoFonksiyonlari.UpdateBinary(0, a);
            if (!Status) { DonusDegeri[0] = "0"; return DonusDegeri; }*/

            byte[] OkunanDegerler = ReadBinary(0, 12);

            string IssuerArea = Hexcon.BytetoString(0, 4, OkunanDegerler);
            DonusDegeri[1] = IssuerArea;

            UInt32 CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            DonusDegeri[2] = CihazNo.ToString();

            DonusDegeri[0] = "1";

            return DonusDegeri;
        }

        private bool UpdateIssuer(string IssuerName, UInt32 CihazNo)
        {
            char[] Issuer = IssuerName.ToCharArray();

            byte[] YazilacakData = new byte[12];

            for (int i = 0; i < Issuer.Length; i++)
            {
                YazilacakData[i] = Convert.ToByte(Issuer[i]);
            }

            Integer4Byte Cihaz = new Integer4Byte(CihazNo);

            YazilacakData[8] = Cihaz.bir;
            YazilacakData[9] = Cihaz.iki;
            YazilacakData[10] = Cihaz.uc;
            YazilacakData[11] = Cihaz.dort;

            bool Sonuc = UpdateBinary(0, YazilacakData);

            /*byte[] OkunanDegerler = classPosDuoFonksiyonlari.ReadBinary(0, 12);

            string IssuerArea = Yorumla.BytetoString(1, 4, OkunanDegerler);*/


            return Sonuc;
        }

        private bool WriteKey(byte KeyID, byte[] KeyData)
        {
            byte[] command = new byte[8 + KeyData.Length];
            byte[] response = new byte[2];


            command[0] = 0X80; //CLA
            command[1] = 0XF4; //INS
            command[2] = 0X01; //OP (Operation Mode): 01 : Update, 00: Install, 02:Update Mifare access key  (DATA only contains the key)
            command[3] = 0X00; //SE If OP = ’00’ or OP= ‘01’ 
            command[4] = 0X0B; // Lc : key length  - is always ‘10’
            command[5] = 0XC2;
            command[6] = 0X09;
            command[7] = KeyID;




            Array.Copy(KeyData, 0, command, 8, 8);

            try
            {
                rd.Transmit(command, out response);
                if (response[0] == 0X90 && response[1] == 0X00) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        #endregion

        #region IKart Members

        #region Abone fonksyonları

        #region Yazılacak paketler
        public void YazilacakDuzenle0023(UInt32 CihazNo, UInt32 Katsayi1, UInt32 Katsayi2, UInt32 Katsayi3, UInt32 Limit1, UInt32 Limit2)
        {
            Integer4Byte Cihaz = new Integer4Byte(CihazNo);
            Integer4Byte Kat1 = new Integer4Byte(Katsayi1);
            Integer4Byte Kat2 = new Integer4Byte(Katsayi2);
            Integer4Byte Kat3 = new Integer4Byte(Katsayi3);
            Integer4Byte Lim1 = new Integer4Byte(Limit1);
            Integer4Byte Lim2 = new Integer4Byte(Limit2);

            Yazilacak[0] = Cihaz.bir;
            Yazilacak[1] = Cihaz.iki;
            Yazilacak[2] = Cihaz.uc;
            Yazilacak[3] = Cihaz.dort;

            Yazilacak[4] = Kat1.bir;
            Yazilacak[5] = Kat1.iki;
            Yazilacak[6] = Kat1.uc;
            Yazilacak[7] = Kat1.dort;

            Yazilacak[8] = Kat2.bir;
            Yazilacak[9] = Kat2.iki;
            Yazilacak[10] = Kat2.uc;
            Yazilacak[11] = Kat2.dort;

            Yazilacak[12] = Kat3.bir;
            Yazilacak[13] = Kat3.iki;
            Yazilacak[14] = Kat3.uc;
            Yazilacak[15] = Kat3.dort;

            Yazilacak[16] = Lim1.bir;
            Yazilacak[17] = Lim1.iki;
            Yazilacak[18] = Lim1.uc;
            Yazilacak[19] = Lim1.dort;

            Yazilacak[20] = Lim2.bir;
            Yazilacak[21] = Lim2.iki;
            Yazilacak[22] = Lim2.uc;
            Yazilacak[23] = Lim2.dort;
        }

        public void YazilacakDuzenle2447(Int32 AnaKredi, Int32 YedekKredi,
                                         byte AkoYko, byte IslemNo, byte KartNo, byte Cap,
                                         byte Tip, byte DonemGun, byte VanaPulseSure, byte VanaCntSure,
                                         byte Iade, UInt16 MaxdebiSiniri, byte HaftaSonuOnay,
                                         UInt32 AboneNo)
        {
            Integer4Byte AnaKredii = new Integer4Byte(AnaKredi);
            Integer4Byte YedekKredii = new Integer4Byte(YedekKredi);
            Integer4Byte AboneNoo = new Integer4Byte(AboneNo);
            Integer2Byte MaxdebiSinirii = new Integer2Byte(MaxdebiSiniri);


            Yazilacak[0] = AnaKredii.bir;
            Yazilacak[1] = AnaKredii.iki;
            Yazilacak[2] = AnaKredii.uc;
            Yazilacak[3] = AnaKredii.dort;

            Yazilacak[4] = YedekKredii.bir;
            Yazilacak[5] = YedekKredii.iki;
            Yazilacak[6] = YedekKredii.uc;
            Yazilacak[7] = YedekKredii.dort;

            Yazilacak[8] = AkoYko;
            Yazilacak[9] = IslemNo;
            Yazilacak[10] = KartNo;
            Yazilacak[11] = Cap;

            Yazilacak[12] = Tip;
            Yazilacak[13] = DonemGun;
            Yazilacak[14] = VanaPulseSure;
            Yazilacak[15] = VanaCntSure;

            Yazilacak[16] = Iade;
            Yazilacak[17] = MaxdebiSinirii.bir;
            Yazilacak[18] = MaxdebiSinirii.iki;

            Yazilacak[19] = HaftaSonuOnay;

            Yazilacak[20] = AboneNoo.bir;
            Yazilacak[21] = AboneNoo.iki;
            Yazilacak[22] = AboneNoo.uc;
            Yazilacak[23] = AboneNoo.dort;
        }

        public void YazilacakDuzenle4871(UInt16 Bayram1Tarih, UInt16 Bayram2Tarih, UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4, UInt32 Fix_Charge)
        {
            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1Tarih);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2Tarih);
            Integer4Byte Kat4 = new Integer4Byte(Katsayi4);
            Integer4Byte Kat5 = new Integer4Byte(Katsayi5);
            Integer4Byte Lim3 = new Integer4Byte(Limit3);
            Integer4Byte Lim4 = new Integer4Byte(Limit4);
            Integer4Byte FixC = new Integer4Byte(Fix_Charge);

            Yazilacak[0] = Bayram1Deger.bir;
            Yazilacak[1] = Bayram1Deger.iki;
            Yazilacak[2] = Bayram2Deger.bir;
            Yazilacak[3] = Bayram2Deger.iki;

            Yazilacak[4] = Kat4.bir;
            Yazilacak[5] = Kat4.iki;
            Yazilacak[6] = Kat4.uc;
            Yazilacak[7] = Kat4.dort;

            Yazilacak[8] = Kat5.bir;
            Yazilacak[9] = Kat5.iki;
            Yazilacak[10] = Kat5.uc;
            Yazilacak[11] = Kat5.dort;

            Yazilacak[12] = Lim3.bir;
            Yazilacak[13] = Lim3.iki;
            Yazilacak[14] = Lim3.uc;
            Yazilacak[15] = Lim3.dort;

            Yazilacak[16] = Lim4.bir;
            Yazilacak[17] = Lim4.iki;
            Yazilacak[18] = Lim4.uc;
            Yazilacak[19] = Lim4.dort;

            Yazilacak[20] = FixC.bir;
            Yazilacak[21] = FixC.iki;
            Yazilacak[22] = FixC.uc;
            Yazilacak[23] = FixC.dort;
        }

        #endregion

        public string AboneOku(byte ZoneIndex)
        {
            HataSet(0);
            string str = "";
            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status;

            Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);
            string IssuerArea = Hexcon.BytetoString(0, 4, response);

            if (IssuerArea == "\0\0\0\0") return Return(29);
            else if (IssuerArea.Substring(0, 2) != issue_area[0] + issue_area[1]) return Return(30);
            else if (IssuerArea != issue_area[0] + issue_area[1] + "A ") return Return(7);

            UInt32 CihazNo = Hexcon.Byte4toUInt32(response[8], response[9], response[10], response[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };
            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = VerifyPin(0X83, PinAbone);
            if (!Status) return Return(8);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            Abone000023 Adres000023 = new Abone000023(ReadBinary(0, 24));
            Abone024051 Adres024051 = new Abone024051(ReadBinary(24, 30));
            Abone051070 Adres051070 = new Abone051070(ReadBinary(52, 24));
            AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));
            Abone072095 Adres072095 = new Abone072095(ReadBinary(72, 24));
            Abone096119 Adres096119 = new Abone096119(ReadBinary(96, 24));
            Abone120143 Adres120143 = new Abone120143(ReadBinary(120, 24));
            Abone144171 Adres144171 = new Abone144171(ReadBinary(144, 28));

            Abone170197 Adres170197 = new Abone170197(ReadBinary(170, 28));
            Abone198226 Adres198226 = new Abone198226(ReadBinary(198, 30));
            Abone228239 Adres228239 = new Abone228239(ReadBinary(227, 20));

            string AnaKrediOkunma = "";
            if (((Adres024051.AkoYko & 128) >> 7) == 0) { AnaKrediOkunma = "*"; } else { AnaKrediOkunma = "b"; }
            string YedekKrediOkunma = "";
            if (((Adres024051.AkoYko & 64) >> 6) == 0) { YedekKrediOkunma = "*"; } else { YedekKrediOkunma = "b"; }

            Adres144171.SayacDurumu = Convert.ToByte((Adres144171.SayacDurumu & 128) + (Adres144171.SayacDurumu & 64) + (Adres144171.SayacDurumu & 32) + (Adres144171.SayacDurumu & 2) + (Adres144171.SayacDurumu & 1));
            string SayacCeza = "";
            if (Adres144171.SayacDurumu == 0) { SayacCeza = "0"; } else { SayacCeza = "1"; }

            byte IadeYapilmis = 0;
            switch (Adres024051.Iade)
            {
                case 0:
                    IadeYapilmis = 0;
                    break;
                case 0x96:
                    IadeYapilmis = 1;
                    break;
                case 0x99:
                    IadeYapilmis = 2;
                    break;
            }

            Int32 IadeKredi = 0;

            if (IadeYapilmis == 0)
            {
                IadeKredi = 0;
            }
            else if (IadeYapilmis == 1)
            {
                IadeKredi = Adres198226.IadeKalan;
                if (AnaKrediOkunma == "b")
                {
                    IadeKredi += Adres024051.AnaKredi;
                }
                if (YedekKrediOkunma == "b")
                {
                    IadeKredi += Adres024051.YedekKredi;
                }
            }

            string AnaPilSeviyesi = String.Format("{0,8:0.000}", (Adres144171.AnaPil * 6 / 255.0));

            UInt32 KritikKredi = 30;
            switch (Adres024051.Cap)
            {
                case 15:
                case 25:
                case 20:
                    KritikKredi = 30;
                    break;
                case 40:
                case 50:
                case 65:
                case 80:
                case 100:
                case 150:
                case 200:
                    KritikKredi = 300;
                    break;
            }

            byte SayacTipi = 2;

            byte Bayram1Ay = Convert.ToByte(Adres024051.Bayram1 & 0X000F);
            byte Bayram1Gun = Convert.ToByte((Adres024051.Bayram1 >> 4) & 0X001F);
            byte Bayram1Sure = Convert.ToByte(Adres024051.Bayram1 >> 9);

            byte Bayram2Ay = Convert.ToByte(Adres024051.Bayram2 & 0X000F);
            byte Bayram2Gun = Convert.ToByte((Adres024051.Bayram2 >> 4) & 0X001F);
            byte Bayram2Sure = Convert.ToByte(Adres024051.Bayram2 >> 9);

            FinishKart();

            #region dönüş değerleri

            str += "1";
            str += sp + Degerler.AboneNo;
            str += sp + Adres024051.KartNo;
            str += sp + CihazNo;
            str += sp + AnaKrediOkunma;
            str += sp + YedekKrediOkunma;
            str += sp + Adres024051.AnaKredi;
            str += sp + Adres024051.YedekKredi;
            str += sp + SayacCeza;
            str += sp + "*";
            str += sp + Adres024051.Tip;
            str += sp + Adres024051.Cap;
            str += sp + Adres024051.DonemGun;
            str += sp + Adres000023.Limit1;
            str += sp + Adres000023.Limit2;
            str += sp + Adres051070.Limit3;
            str += sp + Adres051070.Limit4;
            str += sp + IadeYapilmis;
            str += sp + String.Format("{0,10:0}", IadeKredi) + "_" + String.Format("{0,10:0}", Adres072095.HarcananKredi);
            str += sp + Adres144171.DonemGun;
            str += sp + AnaPilSeviyesi;
            str += sp + Adres072095.KalanKredi;
            str += sp + Adres072095.HarcananKredi;
            str += sp + KritikKredi;
            str += sp + Adres144171.VanaOperasyonSayisi;
            str += sp + Adres120143.SonKrediTarihi;
            str += sp + Adres120143.SonPulseTarihi;
            str += sp + Adres120143.SonCezaTarihi;
            str += sp + Adres120143.SonArizaTarihi;
            str += sp + (Adres144171.SayacDurumu & 1);
            str += sp + ((Adres144171.SayacDurumu & 2) >> 1);
            str += sp + ((Adres144171.SayacDurumu & 4) >> 2);
            str += sp + ((Adres144171.SayacDurumu & 8) >> 3);
            str += sp + ((Adres144171.SayacDurumu & 16) >> 4);
            str += sp + ((Adres144171.SayacDurumu & 32) >> 5);
            str += sp + ((Adres144171.SayacDurumu & 64) >> 6);
            str += sp + ((Adres144171.SayacDurumu & 128) >> 7);
            str += sp + Adres072095.GercekTuketim;
            str += sp + Adres072095.KademeTuketim1;
            str += sp + Adres072095.KademeTuketim2;
            str += sp + Adres072095.KademeTuketim3;
            str += sp + Adres228239.KademeTuketim4;
            str += sp + Adres228239.KademeTuketim5;
            str += sp + Adres096119.DonemTuketimi1;
            str += sp + Adres096119.DonemTuketimi2;
            str += sp + Adres096119.DonemTuketimi3;
            str += sp + Adres120143.DonemTuketimi4;
            str += sp + Adres120143.DonemTuketimi5;
            str += sp + Adres120143.DonemTuketimi6;
            str += sp + Adres096119.SayacTarihi;
            str += sp + SayacTipi;
            str += sp + Adres051070.FixCharge;
            str += sp + Adres228239.TotalFixCharge;
            str += sp + Adres144171.SonTakilanYetkiKartiOzellik1;
            str += sp + Adres144171.SonTakilanYetkiKartiOzellik2;
            str += sp + Adres144171.SonTakilanYetkiKartiOzellik3;
            str += sp + Adres144171.MaxDebiTarihi;
            str += sp + Adres170197.DonemTuketimi7;
            str += sp + Adres170197.DonemTuketimi8;
            str += sp + Adres170197.DonemTuketimi9;
            str += sp + Adres170197.DonemTuketimi10;
            str += sp + Adres170197.DonemTuketimi11;
            str += sp + Adres170197.DonemTuketimi12;
            str += sp + Adres170197.DonemTuketimi13;
            str += sp + Adres170197.DonemTuketimi14;
            str += sp + Adres198226.DonemTuketimi15;
            str += sp + Adres198226.DonemTuketimi16;
            str += sp + Adres198226.DonemTuketimi17;
            str += sp + Adres198226.DonemTuketimi18;
            str += sp + Adres198226.DonemTuketimi19;
            str += sp + Adres198226.DonemTuketimi20;
            str += sp + Adres198226.DonemTuketimi21;
            str += sp + Adres198226.DonemTuketimi22;
            str += sp + Adres198226.DonemTuketimi23;
            str += sp + Adres198226.DonemTuketimi24;
            str += sp + Adres198226.Mektuk;
            str += sp + Adres144171.Versiyon;
            //str += sp + Adres198226.IadeKalan;
            //str += sp + Adres198226.ResetSayisi;
            str += sp + Adres024051.HaftaSonuOnay;

            str += sp + Bayram1Gun;
            str += sp + Bayram1Ay;
            str += sp + Bayram1Sure;
            str += sp + Bayram2Gun;
            str += sp + Bayram2Ay;
            str += sp + Bayram2Sure;
            str += sp + Degerler.IslemNo;

            str += sp + Adres000023.Katsayi1;
            str += sp + Adres000023.Katsayi2;
            str += sp + Adres000023.Katsayi3;
            str += sp + Adres051070.Katsayi4;
            str += sp + Adres051070.Katsayi5;

            return str;

            #endregion



            #region Dosyaya Yazma İşlemleri

            //string DosyaAdi = "aoku.txt";
            //if (File.Exists(DosyaAdi))
            //{
            //    File.Delete(DosyaAdi);
            //}
            //StreamWriter Dosya = new StreamWriter(DosyaAdi);
            //Dosya.WriteLine("1");
            //Dosya.WriteLine(Degerler.AboneNo);
            //Dosya.WriteLine(Adres024051.KartNo);
            //Dosya.WriteLine(CihazNo);
            //Dosya.WriteLine(AnaKrediOkunma);
            //Dosya.WriteLine(YedekKrediOkunma);
            //Dosya.WriteLine(Adres024051.AnaKredi);
            //Dosya.WriteLine(Adres024051.YedekKredi);
            //Dosya.WriteLine(SayacCeza);
            //Dosya.WriteLine("*");
            //Dosya.WriteLine(Adres024051.Tip);
            //Dosya.WriteLine(Adres024051.Cap);
            //Dosya.WriteLine(Adres024051.DonemGun);
            //Dosya.WriteLine(Adres000023.Limit1);
            //Dosya.WriteLine(Adres000023.Limit2);
            //Dosya.WriteLine(Adres051070.Limit3);
            //Dosya.WriteLine(Adres051070.Limit4);
            //Dosya.WriteLine(IadeYapilmis);
            //Dosya.WriteLine(String.Format("{0,10:0}", IadeKredi) + "_" + String.Format("{0,10:0}", Adres072095.HarcananKredi));
            //Dosya.WriteLine(Adres144171.DonemGun);
            //Dosya.WriteLine(AnaPilSeviyesi);
            //Dosya.WriteLine(Adres072095.KalanKredi);
            //Dosya.WriteLine(Adres072095.HarcananKredi);
            //Dosya.WriteLine(KritikKredi);
            //Dosya.WriteLine(Adres144171.VanaOperasyonSayisi);
            //Dosya.WriteLine(Adres120143.SonKrediTarihi);
            //Dosya.WriteLine(Adres120143.SonPulseTarihi);
            //Dosya.WriteLine(Adres120143.SonCezaTarihi);
            //Dosya.WriteLine(Adres120143.SonArizaTarihi);
            //Dosya.WriteLine(Adres144171.SayacDurumu & 1);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 2) >> 1);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 4) >> 2);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 8) >> 3);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 16) >> 4);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 32) >> 5);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 64) >> 6);
            //Dosya.WriteLine((Adres144171.SayacDurumu & 128) >> 7);
            //Dosya.WriteLine(Adres072095.GercekTuketim);
            //Dosya.WriteLine(Adres072095.KademeTuketim1);
            //Dosya.WriteLine(Adres072095.KademeTuketim2);
            //Dosya.WriteLine(Adres072095.KademeTuketim3);
            //Dosya.WriteLine(Adres228239.KademeTuketim4);
            //Dosya.WriteLine(Adres228239.KademeTuketim5);
            //Dosya.WriteLine(Adres096119.DonemTuketimi1);
            //Dosya.WriteLine(Adres096119.DonemTuketimi2);
            //Dosya.WriteLine(Adres096119.DonemTuketimi3);
            //Dosya.WriteLine(Adres120143.DonemTuketimi4);
            //Dosya.WriteLine(Adres120143.DonemTuketimi5);
            //Dosya.WriteLine(Adres120143.DonemTuketimi6);
            //Dosya.WriteLine(Adres096119.SayacTarihi);
            //Dosya.WriteLine(SayacTipi);
            //Dosya.WriteLine(Adres051070.FixCharge);
            //Dosya.WriteLine(Adres228239.TotalFixCharge);
            //Dosya.WriteLine(Adres144171.SonTakilanYetkiKartiOzellik1);
            //Dosya.WriteLine(Adres144171.SonTakilanYetkiKartiOzellik2);
            //Dosya.WriteLine(Adres144171.SonTakilanYetkiKartiOzellik3);
            //Dosya.WriteLine(Adres144171.MaxDebiTarihi);
            //Dosya.WriteLine(Adres170197.DonemTuketimi7);
            //Dosya.WriteLine(Adres170197.DonemTuketimi8);
            //Dosya.WriteLine(Adres170197.DonemTuketimi9);
            //Dosya.WriteLine(Adres170197.DonemTuketimi10);
            //Dosya.WriteLine(Adres170197.DonemTuketimi11);
            //Dosya.WriteLine(Adres170197.DonemTuketimi12);
            //Dosya.WriteLine(Adres170197.DonemTuketimi13);
            //Dosya.WriteLine(Adres170197.DonemTuketimi14);
            //Dosya.WriteLine(Adres198226.DonemTuketimi15);
            //Dosya.WriteLine(Adres198226.DonemTuketimi16);
            //Dosya.WriteLine(Adres198226.DonemTuketimi17);
            //Dosya.WriteLine(Adres198226.DonemTuketimi18);
            //Dosya.WriteLine(Adres198226.DonemTuketimi19);
            //Dosya.WriteLine(Adres198226.DonemTuketimi20);
            //Dosya.WriteLine(Adres198226.DonemTuketimi21);
            //Dosya.WriteLine(Adres198226.DonemTuketimi22);
            //Dosya.WriteLine(Adres198226.DonemTuketimi23);
            //Dosya.WriteLine(Adres198226.DonemTuketimi24);
            //Dosya.WriteLine(Adres198226.Mektuk);
            //Dosya.WriteLine(Adres144171.Versiyon);
            //Dosya.Close();

            //return "1";

            #endregion


        }

        public string AboneYap(UInt32 CihazNo, UInt32 AboneNo, byte KartNo,
                              byte Cap, byte Tip, byte DonemGun,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                              UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                              byte Bayram1Gun, byte Bayram1Ay, byte Bayram1Sure,
                              byte Bayram2Gun, byte Bayram2Ay, byte Bayram2Sure,
                              byte AvansOnay, UInt16 Maxdebi, byte ZoneIndex, UInt32 FixCharge)
        {
            HataSet(0);

            #region fiyat,limit,çap kontrol

            if (Fiyat1 <= 0 || Fiyat2 <= 0 || Fiyat3 <= 0 || Fiyat4 <= 0 || Fiyat5 <= 0) return Return(25);
            if (Limit1 <= 0 || Limit2 <= 0 || Limit3 <= 0 || Limit4 <= 0) return Return(26);
            if (Cap <= 0) return Return(27);
            else
            {
                if (Cap == 15 || Cap == 20 || Cap == 25 || Cap == 40 || Cap == 50 || Cap == 65 || Cap == 80 || Cap == 100 || Cap == 150 || Cap == 200) { } else return Return(28);
            }

            #endregion

            UInt16 Bayram1, Bayram2;

            byte[] PinAbone = new byte[8];

            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            UInt32 CihazNooo = Hexcon.Byte4toUInt32(response[8], response[9], response[10], response[11]);

            if (IssuerArea != "\0\0\0\0")
            {
                return Return(11);
            }

            GetIssuer(ZoneIndex);

            Status = UpdateIssuer(issue_area[0] + issue_area[1] + "A ", CihazNo);
            if (!Status) return Return(10);

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = WriteKey(0X83, PinAbone);

            if (!Status)
            {
                Status = WriteKey(0X83, PinAbone);
                if (!Status) return Return(9);
            }

            Status = VerifyPin(0X83, PinAbone);
            if (!Status) return Return(8);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            UInt32 KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, KademeKatsayi4, KademeKatsayi5;
            KademeKatsayi1 = KademeKatsayi2 = KademeKatsayi3 = KademeKatsayi4 = KademeKatsayi5 = 0;
            switch (Cap)
            {
                case 15:
                case 25:
                case 20:
                    Integer4Byte Katsayi1 = new Integer4Byte(Convert.ToUInt32(1000));
                    Integer4Byte Katsayi2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 1000 / Fiyat1));
                    Integer4Byte Katsayi3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 1000 / Fiyat1));
                    Integer4Byte Katsayi4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 1000 / Fiyat1));
                    Integer4Byte Katsayi5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 1000 / Fiyat1));
                    KademeKatsayi1 = Katsayi1.value;
                    KademeKatsayi2 = Katsayi2.value;
                    KademeKatsayi3 = Katsayi3.value;
                    KademeKatsayi4 = Katsayi4.value;
                    KademeKatsayi5 = Katsayi5.value;
                    break;
                case 40:
                case 50:
                case 65:
                case 80:
                case 100:
                case 150:
                case 200:
                    Integer4Byte Katsayi_1 = new Integer4Byte(Convert.ToUInt32(10000));
                    Integer4Byte Katsayi_2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 10000 / Fiyat1));
                    KademeKatsayi1 = Katsayi_1.value;
                    KademeKatsayi2 = Katsayi_2.value;
                    KademeKatsayi3 = Katsayi_3.value;
                    KademeKatsayi4 = Katsayi_4.value;
                    KademeKatsayi5 = Katsayi_5.value;
                    break;
            }

            Bayram1 = Bayram1Sure;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gun;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ay;

            Bayram2 = Bayram2Sure;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gun;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ay;

            KartNo = Convert.ToByte((KartNo & 0x0F) + 0x40);

            YazilacakDuzenle0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);

            AvansOnay = 1;//Hafta sonu onay aktif max debi pasif olması için bu eklendi
            Maxdebi = 0;
            YazilacakDuzenle2447(0, 0, 0xC0, 0, KartNo, Cap, Tip, DonemGun, 25, 30, 0, Maxdebi, AvansOnay, AboneNo);
            Status = UpdateBinary(24, Yazilacak);
            if (!Status) return Return(14);

            YazilacakDuzenle4871(Bayram1, Bayram2, KademeKatsayi4, KademeKatsayi5, Limit3, Limit4, FixCharge);
            Status = UpdateBinary(48, Yazilacak);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string AboneYaz(UInt32 CihazNo, Int32 AnaKredi, Int32 YedekKredi,
                              byte Cap, byte DonemGun,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Fiyat4, UInt32 Fiyat5,
                              UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                              byte Bayram1Gun, byte Bayram1Ay, byte Bayram1Sure,
                              byte Bayram2Gun, byte Bayram2Ay, byte Bayram2Sure,
                              byte AvansOnay, UInt16 Maxdebi, byte ZoneIndex, UInt32 FixCharge)
        {
            HataSet(0);

            #region fiyat,limit,çap kontrol

            if (Fiyat1 <= 0 || Fiyat2 <= 0 || Fiyat3 <= 0 || Fiyat4 <= 0 || Fiyat5 <= 0) return Return(25);
            if (Limit1 <= 0 || Limit2 <= 0 || Limit3 <= 0 || Limit4 <= 0) return Return(26);
            if (Cap <= 0) return Return(27);
            else
            {
                if (Cap == 15 || Cap == 20 || Cap == 25 || Cap == 40 || Cap == 50 || Cap == 65 || Cap == 80 || Cap == 100 || Cap == 150 || Cap == 200) { } else return Return(28);
            }

            #endregion

            UInt16 Bayram1, Bayram2;

            byte[] PinAbone = new byte[8];

            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);

            if (IssuerArea == "\0\0\0\0") return Return(29);
            else if (IssuerArea.Substring(0, 2) != issue_area[0] + issue_area[1]) return Return(30);
            else if (IssuerArea != issue_area[0] + issue_area[1] + "A ") return Return(7);

            UInt32 CihazNooo = Hexcon.Byte4toUInt32(response[8], response[9], response[10], response[11]);

            if (CihazNo != CihazNooo) return Return(12);

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = VerifyPin(0X83, PinAbone);
            if (!Status) return Return(8);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));

            if (Cap != Degerler.Cap) return Return(13);

            if (Degerler.Iade == 0x99)
                return "10";
            if (Degerler.Iade == 0x96)
                return "11";


            UInt32 KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, KademeKatsayi4, KademeKatsayi5;
            KademeKatsayi1 = KademeKatsayi2 = KademeKatsayi3 = KademeKatsayi4 = KademeKatsayi5 = 0;
            switch (Cap)
            {
                case 15:
                case 25:
                case 20:
                    Integer4Byte Katsayi1 = new Integer4Byte(Convert.ToUInt32(1000));
                    Integer4Byte Katsayi2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 1000 / Fiyat1));
                    Integer4Byte Katsayi3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 1000 / Fiyat1));
                    Integer4Byte Katsayi4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 1000 / Fiyat1));
                    Integer4Byte Katsayi5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 1000 / Fiyat1));
                    KademeKatsayi1 = Katsayi1.value;
                    KademeKatsayi2 = Katsayi2.value;
                    KademeKatsayi3 = Katsayi3.value;
                    KademeKatsayi4 = Katsayi4.value;
                    KademeKatsayi5 = Katsayi5.value;
                    break;
                case 40:
                case 50:
                case 65:
                case 80:
                case 100:
                case 150:
                case 200:
                    Integer4Byte Katsayi_1 = new Integer4Byte(Convert.ToUInt32(10000));
                    Integer4Byte Katsayi_2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 10000 / Fiyat1));
                    Integer4Byte Katsayi_5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 10000 / Fiyat1));
                    KademeKatsayi1 = Katsayi_1.value;
                    KademeKatsayi2 = Katsayi_2.value;
                    KademeKatsayi3 = Katsayi_3.value;
                    KademeKatsayi4 = Katsayi_4.value;
                    KademeKatsayi5 = Katsayi_5.value;
                    break;
            }

            Bayram1 = Bayram1Sure;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gun;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ay;

            Bayram2 = Bayram2Sure;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gun;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ay;



            if ((Degerler.AkoYko & 128) == 0) { Degerler.IslemNo++; }
            if ((Degerler.AkoYko & 64) == 0) { Degerler.IslemNo++; }

            YazilacakDuzenle0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);
           
            AvansOnay = 1;//Hafta sonu onay aktif max debi pasif olması için bu eklendi
            Maxdebi = 0;
            YazilacakDuzenle2447(AnaKredi, YedekKredi, 0xC0, Degerler.IslemNo, Degerler.KartNo, Cap, Degerler.Tip, DonemGun, 25, 30, 0, Maxdebi, AvansOnay, Degerler.AboneNo);
            Status = UpdateBinary(24, Yazilacak);
            if (!Status) return Return(14);

            YazilacakDuzenle4871(Bayram1, Bayram2, KademeKatsayi4, KademeKatsayi5, Limit3, Limit4, FixCharge);
            Status = UpdateBinary(48, Yazilacak);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string KrediOku(byte ZoneIndex)
        {
            HataSet(0);

            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);

            if (IssuerArea == "\0\0\0\0") return Return(29);
            else if (IssuerArea.Substring(0, 2) != issue_area[0] + issue_area[1]) return Return(30);
            else if (IssuerArea != issue_area[0] + issue_area[1] + "A ") return Return(7);

            UInt32 CihazNo = Hexcon.Byte4toUInt32(response[8], response[9], response[10], response[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };
            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = VerifyPin(0X83, PinAbone);
            if (!Status) return Return(8);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            Abone000023 Adres000023 = new Abone000023(ReadBinary(0, 24));
            Abone024051 Adres024051 = new Abone024051(ReadBinary(24, 28));
            AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));
            Abone072095 Adres072095 = new Abone072095(ReadBinary(72, 24));
            Abone096119 Adres096119 = new Abone096119(ReadBinary(96, 24));
            Abone120143 Adres120143 = new Abone120143(ReadBinary(120, 24));
            Abone144171 Adres144171 = new Abone144171(ReadBinary(144, 28));


            string AnaKrediOkunma = "";
            if (((Adres024051.AkoYko & 128) >> 7) == 0) { AnaKrediOkunma = "*"; } else { AnaKrediOkunma = "b"; }
            string YedekKrediOkunma = "";
            if (((Adres024051.AkoYko & 64) >> 6) == 0) { YedekKrediOkunma = "*"; } else { YedekKrediOkunma = "b"; }

            Adres144171.SayacDurumu = Convert.ToByte((Adres144171.SayacDurumu & 64) + (Adres144171.SayacDurumu & 32) + (Adres144171.SayacDurumu & 32) + (Adres144171.SayacDurumu & 2) + (Adres144171.SayacDurumu & 1));
            string SayacCeza = "";
            if (Adres144171.SayacDurumu == 0) { SayacCeza = "0"; } else { SayacCeza = "1"; }

            byte IadeYapilmis = 0;
            switch (Adres024051.Iade)
            {
                case 0:
                    IadeYapilmis = 0;
                    break;
                case 0x96:
                    IadeYapilmis = 1;
                    break;
                case 0x99:
                    IadeYapilmis = 2;
                    break;
            }

            Int32 IadeKredi = 0;

            if (IadeYapilmis == 0)
            {
                IadeKredi = 0;
            }
            else if (IadeYapilmis == 1)
            {
                IadeKredi = Adres072095.KalanKredi;
                if (AnaKrediOkunma == "b")
                {
                    IadeKredi += Adres024051.AnaKredi;
                }
                if (YedekKrediOkunma == "b")
                {
                    IadeKredi += Adres024051.YedekKredi;
                }
            }

            string AnaPilSeviyesi = String.Format("{0,8:0.000}", (Adres144171.AnaPil * 6 / 255.0));

            UInt32 KritikKredi = 300;
            byte SayacTipi = 2;

            FinishKart();

            #region dönüş değerleri

            string str = "";

            str += "1";
            str += sp + Degerler.AboneNo;
            str += sp + Adres024051.KartNo;
            str += sp + CihazNo;
            str += sp + AnaKrediOkunma;
            str += sp + YedekKrediOkunma;
            str += sp + Adres024051.AnaKredi;
            str += sp + Adres024051.YedekKredi;
            str += sp + SayacCeza;
            str += sp + "*";
            str += sp + Adres024051.Tip;
            str += sp + Adres024051.Cap;
            str += sp + Adres024051.DonemGun;
            str += sp + Adres000023.Limit1;
            str += sp + Adres000023.Limit2;
            str += sp + Adres072095.KalanKredi;
            str += sp + Adres072095.HarcananKredi;

            if (Degerler.KartNo > 15)
                str += sp + "*"; // yeni kart
            else
                str += sp + "b";
            str += sp + IadeYapilmis;

            return str;

            #endregion

            #region Dosyaya Yazma İşlemleri

            //string DosyaAdi = "krdoku.txt";
            //if (File.Exists(DosyaAdi))
            //{
            //    File.Delete(DosyaAdi);
            //}
            //StreamWriter Dosya = new StreamWriter(DosyaAdi);
            //Dosya.WriteLine("1");
            //Dosya.WriteLine(Degerler.AboneNo);
            //Dosya.WriteLine(Adres024051.KartNo);
            //Dosya.WriteLine(CihazNo);
            //Dosya.WriteLine(AnaKrediOkunma);
            //Dosya.WriteLine(YedekKrediOkunma);
            //Dosya.WriteLine(Adres024051.AnaKredi);
            //Dosya.WriteLine(Adres024051.YedekKredi);
            //Dosya.WriteLine(SayacCeza);
            //Dosya.WriteLine("*");
            //Dosya.WriteLine(Adres024051.Tip);
            //Dosya.WriteLine(Adres024051.Cap);
            //Dosya.WriteLine(Adres024051.DonemGun);
            //Dosya.WriteLine(Adres000023.Limit1);
            //Dosya.WriteLine(Adres000023.Limit2);
            //Dosya.WriteLine(Adres072095.KalanKredi);
            //Dosya.WriteLine(Adres072095.HarcananKredi);

            //if (Degerler.KartNo > 15)
            //{
            //    Dosya.WriteLine("*"); // yeni kart
            //}
            //else
            //{
            //    Dosya.WriteLine("b");
            //}

            //Dosya.WriteLine(IadeYapilmis);
            //Dosya.Close();

            //return "1";

            #endregion

        }

        public string Iade(UInt32 CihazNo, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == issue_area[0] + issue_area[1] + "A ")
            {
                if (Issuer.CihazNo == CihazNo)
                {
                    EFDataName1Eris(CihazNo);
                }
                else return Return(12);
            }
            else return Return(7);

            byte[] OkunanDegerler = ReadBinary(40, 1);
            byte Iade = OkunanDegerler[0];

            if (Iade == 0)
            {
                bool Status = UpdateBinary(40, 0x99);
                if (!Status) return Return(15);
            }

            FinishKart();

            return "1";

        }

        public string SatisIptal(UInt32 CihazNo,
                                 Int32 AnaKredi, Int32 YedekKredi,
                                 byte Ako, byte Yko,
                                 byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == issue_area[0] + issue_area[1] + "A ")
            {
                if (Issuer.CihazNo == CihazNo)
                {
                    EFDataName1Eris(CihazNo);
                }
                else return Return(12);
            }

            else return Return(7);

            //Verileri Değiştir..

            Integer4Byte KrediAna = new Integer4Byte(AnaKredi);
            Integer4Byte KrediYedek = new Integer4Byte(YedekKredi);

            UpdateBinary(24, KrediAna.bir, KrediAna.iki, KrediAna.uc, KrediAna.dort);
            UpdateBinary(28, KrediYedek.bir, KrediYedek.iki, KrediYedek.uc, KrediYedek.dort);

            byte[] OkunanDegerler = ReadBinary(32, 2);
            byte AkoYko = OkunanDegerler[0];
            byte IslemNo = OkunanDegerler[1];

            if (Ako == 1)
            {
                IslemNo--;
                AkoYko = Convert.ToByte(AkoYko & 127);
            }
            else if (Ako == 0)
            {
                AkoYko = Convert.ToByte(AkoYko | 128);
            }
            if (Yko == 1)
            {
                IslemNo--;
                AkoYko = Convert.ToByte(AkoYko & 191);
            }
            else if (Yko == 0)
            {
                AkoYko = Convert.ToByte(AkoYko | 64);
            }

            bool Status = UpdateBinary(32, AkoYko, IslemNo);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string AboneBosalt(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == issue_area[0] + issue_area[1] + "A ")
            {
                UpdateIssuer("\0\0\0\0", 0);//"\0\0\0\0"

                EFDataName1Eris(Issuer.CihazNo);
            }
            else return Return(7);

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            bool Status;
            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) return Return(16);
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status) return Return(16);

            FinishKart();

            return "1";
        }

        #endregion

        #region Ortak Fonksyonlar

        public string Eject()
        {
            return "1";
        }

        public string KartTipi(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            FinishKart();
            return TipTanimlama(Issuer.IssuerArea);
        }

        public string TipTanimlama(string IssuerArea)
        {
            bool bulundu = true;

#if DEBUG

            switch (IssuerArea)
            {
                case "ASUS":
                    IssuerArea = "Üretim Sıfırlama";
                    break;
                case "ASUR":
                    IssuerArea = "Üretim Reel";
                    break;
                case "ASUT":
                    IssuerArea = "Üretim Test";
                    break;
                case "ASUA":
                    IssuerArea = "Üretim Açma";
                    break;
                case "ASUK":
                    IssuerArea = "Üretim Kapama";
                    break;
                case "ASUF":
                    IssuerArea = "Üretim Format";
                    break;
                case "ASUZ":
                    IssuerArea = "Üretim Cihaz No";
                    break;
                case "BTLD":
                    IssuerArea = "BootLoader";
                    break;
                case "\0\0\0\0":
                    IssuerArea = "Boş Kart";
                    break;
                default:
                    bulundu = false;
                    break;
            }

            if (bulundu == false)
            {
                if (IssuerArea == issue_area[0] + issue_area[1] + "A ")
                    IssuerArea = "Abone Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YA")
                    IssuerArea = "Yetki Açma Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YE")
                    IssuerArea = "Yetki Bilgi Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YC")
                    IssuerArea = "Yetki İptal Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YK")
                    IssuerArea = "Yetki Kapama Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YR")
                    IssuerArea = "Yetki Reset Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YT")
                    IssuerArea = "Yetki Tüketim Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YS")
                    IssuerArea = "Yetki Saat Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "Y4")
                    IssuerArea = "Yetki Ceza4 Karti";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YH")
                    IssuerArea = "Yetki Harici EE";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YV")
                    IssuerArea = "Yetki Avans Karti";
            }

#else
             switch (IssuerArea)
            {
                case "ASUS":
                    IssuerArea = "Production initilation card";
                    break;
                case "ASUR":
                    IssuerArea = "Production Reel Mode Card";
                    break;
                case "ASUT":
                    IssuerArea = "Production Test Mode Card";
                    break;
                case "ASUA":
                    IssuerArea = "Production Open Card";
                    break;
                case "ASUK":
                    IssuerArea = "Production Close Card";
                    break;
                case "ASUF":
                    IssuerArea = "Production Format Card";
                    break;
                case "ASUZ":
                    IssuerArea = "Production Device Number Card";
                    break;
                case "BTLD":
                    IssuerArea = "BootLoader";
                    break;
                case "\0\0\0\0":
                    IssuerArea = "Empty Card";
                    break;
                default:
                    bulundu = false;
                    break;
            }

            if (bulundu == false)
            {
                if (IssuerArea == issue_area[0] + issue_area[1] + "A ")
                    IssuerArea = "Subscriber Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YA")
                    IssuerArea = "Authority Open Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YE")
                    IssuerArea = "Authority Info Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YC")
                    IssuerArea = "Authority Cancel Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YK")
                    IssuerArea = "Authority Close Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YR")
                    IssuerArea = "Authority Reset Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YT")
                    IssuerArea = "Authority Consumption Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YS")
                    IssuerArea = "Authority Time Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "Y4")
                    IssuerArea = "Authority Penalty4 Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YH")
                    IssuerArea = "Authority External EE Card";
                if (IssuerArea == issue_area[0] + issue_area[1] + "YV")
                    IssuerArea = "Authority Advance Card";
            }
#endif

            return IssuerArea;

        }


        public string KartBosaltKontrolsuz()
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Status = UpdateBinary(0, a);
            if (!Status) return Return(4);

            EFDataName1Eris("Yetki");

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) return Return(16);
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status) return Return(16);

            FinishKart();

            return "1";
        }

        #endregion

        #region Yetki fonksyonları

        #region yazılacak paketler

        public void Yetki0007(UInt32 CihazNo, params byte[] Veriler)
        {
            byte[] Temp = { 0, 0, 0, 0, 0, 0, 0, 0 };
            UretimYazilacak = Temp;

            Integer4Byte Cihaz = new Integer4Byte(CihazNo);

            YetkiYazilacak[0] = Cihaz.bir;
            YetkiYazilacak[1] = Cihaz.iki;
            YetkiYazilacak[2] = Cihaz.uc;
            YetkiYazilacak[3] = Cihaz.dort;

            for (int i = 0; i < Veriler.Length; i++)
            {
                YetkiYazilacak[4 + i] = Veriler[i];
            }
        }

        public void Yetki0007(UInt32 CihazNo, DateTime Tarih, params byte[] Veriler)
        {
            byte[] Temp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            UretimYazilacak = Temp;

            Integer4Byte Cihaz = new Integer4Byte(CihazNo);

            YetkiYazilacak[0] = Cihaz.bir;
            YetkiYazilacak[1] = Cihaz.iki;
            YetkiYazilacak[2] = Cihaz.uc;
            YetkiYazilacak[3] = Cihaz.dort;

            TarihAl GuncelTarih = new TarihAl(Tarih);

            YetkiYazilacak[4] = GuncelTarih.bir;
            YetkiYazilacak[5] = GuncelTarih.iki;
            YetkiYazilacak[6] = GuncelTarih.uc;
            YetkiYazilacak[7] = GuncelTarih.dort;

            for (int i = 0; i < Veriler.Length; i++)
            {
                YetkiYazilacak[8 + i] = Veriler[i];
            }
        }

        public void Yetki0007(DateTime Tarih, params byte[] Veriler)
        {
            byte[] Temp = { 0, 0, 0, 0, 0, 0, 0, 0 };
            UretimYazilacak = Temp;

            TarihAl GuncelTarih = new TarihAl(Tarih);

            YetkiYazilacak[0] = GuncelTarih.bir;
            YetkiYazilacak[1] = GuncelTarih.iki;
            YetkiYazilacak[2] = GuncelTarih.uc;
            YetkiYazilacak[3] = GuncelTarih.dort;

            YetkiYazilacak[4] = GuncelTarih.HaftaninGunu;

            for (int i = 0; i < Veriler.Length; i++)
            {
                YetkiYazilacak[5 + i] = Veriler[i];
            }
        }

        public void YazilacakDuzenleYetkiAvans(UInt32 CihazNo, UInt32 Katsayi1, UInt32 Katsayi2, UInt32 Katsayi3, UInt32 Limit1, UInt32 Limit2)
        {
            Integer4Byte Cih = new Integer4Byte(CihazNo);
            Integer4Byte Kat1 = new Integer4Byte(Katsayi1);
            Integer4Byte Kat2 = new Integer4Byte(Katsayi2);
            Integer4Byte Kat3 = new Integer4Byte(Katsayi3);
            Integer4Byte Lim1 = new Integer4Byte(Limit1);
            Integer4Byte Lim2 = new Integer4Byte(Limit2);

            YetkiAvansDizi[0] = Cih.bir;
            YetkiAvansDizi[1] = Cih.iki;
            YetkiAvansDizi[2] = Cih.uc;
            YetkiAvansDizi[3] = Cih.dort;

            YetkiAvansDizi[4] = Kat1.bir;
            YetkiAvansDizi[5] = Kat1.iki;
            YetkiAvansDizi[6] = Kat1.uc;
            YetkiAvansDizi[7] = Kat1.dort;

            YetkiAvansDizi[8] = Kat2.bir;
            YetkiAvansDizi[9] = Kat2.iki;
            YetkiAvansDizi[10] = Kat2.uc;
            YetkiAvansDizi[11] = Kat2.dort;

            YetkiAvansDizi[12] = Kat3.bir;
            YetkiAvansDizi[13] = Kat3.iki;
            YetkiAvansDizi[14] = Kat3.uc;
            YetkiAvansDizi[15] = Kat3.dort;

            YetkiAvansDizi[16] = Lim1.bir;
            YetkiAvansDizi[17] = Lim1.iki;
            YetkiAvansDizi[18] = Lim1.uc;
            YetkiAvansDizi[19] = Lim1.dort;

            YetkiAvansDizi[20] = Lim2.bir;
            YetkiAvansDizi[21] = Lim2.iki;
            YetkiAvansDizi[22] = Lim2.uc;
            YetkiAvansDizi[23] = Lim2.dort;
        }

        public void YetkiDoldur(UInt32 Deger, byte Index)
        {
            Integer4Byte Doldurulacak = new Integer4Byte(Deger);

            Yetki[Index] = Doldurulacak.bir;
            Yetki[Index + 1] = Doldurulacak.iki;
            Yetki[Index + 2] = Doldurulacak.uc;
            Yetki[Index + 3] = Doldurulacak.dort;
        }

        public void YetkiDoldur(Int32 Deger, byte Index)
        {
            Integer4Byte Doldurulacak = new Integer4Byte(Deger);

            Yetki[Index] = Doldurulacak.bir;
            Yetki[Index + 1] = Doldurulacak.iki;
            Yetki[Index + 2] = Doldurulacak.uc;
            Yetki[Index + 3] = Doldurulacak.dort;
        }

        public void YetkiDoldur(string Tarih, byte Index)
        {
            byte[] Degerler = { 0, 0, 0, 0 };
            byte Gun, Ay, Yil, Saat, Dakika, Bir, Iki;
            Gun = Ay = Yil = Saat = Dakika = Bir = Iki = 0;

            Gun = Convert.ToByte(Tarih.Substring(0, 2));
            Ay = Convert.ToByte(Tarih.Substring(3, 2));
            Yil = Convert.ToByte(Convert.ToInt16(Tarih.Substring(6, 4)) - 2000);

            Bir = Convert.ToByte(((Gun & 0x0F) << 4) + Ay);
            Iki = Convert.ToByte((Yil << 1) + ((Gun & 0xF0) >> 4));

            switch (Tarih.Length)
            {
                case 10:
                    Yetki[Index] = Bir;
                    Yetki[Index + 1] = Iki;
                    break;
                case 16:
                    Saat = Convert.ToByte(Tarih.Substring(11, 2));
                    Dakika = Convert.ToByte(Tarih.Substring(14, 2));

                    Yetki[Index] = Bir;
                    Yetki[Index + 1] = Iki;
                    Yetki[Index + 2] = Saat;
                    Yetki[Index + 3] = Dakika;
                    break;
            }
        }

        #endregion

        public string YetkiHazirla(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status;

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YA", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(11);

            Uretim0023();
            Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiSaat(DateTime date, byte DonemGun, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YS", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            Yetki0007(date, DonemGun, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiAcma(UInt32 CihazNo, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YA", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            Yetki0007(CihazNo, 0, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiKapat(UInt32 CihazNo, DateTime Tarih, byte KontrolDegeri, byte KapatmaEmri, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return FinishKart();

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YK", 0);
                EFDataName1Eris("Yetki");
            }
            else { return FinishKart(); }

            Yetki0007(CihazNo, Tarih, KontrolDegeri, 1, KapatmaEmri);

            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) { return FinishKart(); }

            FinishKart();
            return "1";

        }

        public string YetkiKapama(UInt32 CihazNo, byte KapamaGun, byte KapamaAy, byte KapamaYil, byte KapamaSaat, byte KapamaEmri, byte ZoneIndex)
        {
            HataSet(0);

            byte kapatarih1;
            byte kapatarih2;
            byte kapatarih3;
            byte kapatarih4;

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YK", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            kapatarih4 = 0;
            kapatarih3 = KapamaSaat;
            kapatarih2 = KapamaYil;
            kapatarih2 <<= 1;

            if (KapamaGun > 15)
                kapatarih2 |= 0x01;

            kapatarih1 = KapamaGun;
            kapatarih1 <<= 4;
            kapatarih1 |= KapamaAy;

            Yetki0007(CihazNo, kapatarih1, kapatarih2, kapatarih3, kapatarih4, 0, 1, KapamaEmri);

            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";

        }

        public string BilgiYap(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YE", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            byte[] yb = { 0, 1 };
            bool Status = UpdateBinary(0, yb);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string BilgiOku(byte ZoneIndex)
        {
            HataSet(0);

            byte[] PartOku = new byte[33];
            byte[] KartOku = new byte[256];

            int init = InitKart();
            if (init == 0) return Return(11);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea.Substring(0, 4) != issue_area[0] + issue_area[1] + "YE") return Return(19);

            Status = VerifyPin(0x82, PinYetki);
            if (!Status) return Return(20);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            //Verileri Oku..

            for (int i = 0; i < 8; i++)
            {
                PartOku = ReadBinary(Convert.ToByte(i * 32), 32);
                Array.Copy(PartOku, 0, KartOku, i * 32, 32);
            }

            FinishKart();

            YetkiKarti BilgiOku = new YetkiKarti(KartOku);

            if (BilgiOku.BilgiTipi != 0x05)
            {
                return Return(21);
            }

            #region dönüş değerleri

            string str = "1";

            str += sp + BilgiOku.AboneNo;
            str += sp + BilgiOku.KartNo;
            str += sp + BilgiOku.CihazNo;
            str += sp + BilgiOku.Tip;
            str += sp + BilgiOku.Cap;
            str += sp + BilgiOku.KalanKredi;
            str += sp + BilgiOku.HarcananKredi;
            str += sp + BilgiOku.GerCekTuketim;
            str += sp + BilgiOku.DonemTuketimi;
            str += sp + BilgiOku.KritikKredi;
            str += sp + BilgiOku.Limit1;
            str += sp + BilgiOku.Limit2;
            str += sp + BilgiOku.Limit3;
            str += sp + BilgiOku.Limit4;
            str += sp + BilgiOku.DonemGun;
            str += sp + BilgiOku.DonemGunNo;
            str += sp + BilgiOku.AnaPil;
            str += sp + BilgiOku.Vop;
            str += sp + BilgiOku.SayacTarihi;
            str += sp + BilgiOku.SonKrediTarihi;
            str += sp + BilgiOku.SonPulseTarihi;
            str += sp + BilgiOku.SonCezaTarihi;
            str += sp + BilgiOku.SonArizaTarihi;
            str += sp + BilgiOku.Ceza3Tarihi;
            str += sp + BilgiOku.Ceza4Tarihi;
            str += sp + (BilgiOku.SayacDurumu & 1);
            str += sp + ((BilgiOku.SayacDurumu & 2) >> 1);
            str += sp + ((BilgiOku.SayacDurumu & 4) >> 2);
            str += sp + ((BilgiOku.SayacDurumu & 8) >> 3);
            str += sp + ((BilgiOku.SayacDurumu & 16) >> 4);
            str += sp + ((BilgiOku.SayacDurumu & 32) >> 5);
            str += sp + ((BilgiOku.SayacDurumu & 64) >> 6);
            str += sp + ((BilgiOku.SayacDurumu & 128) >> 7);
            str += sp + BilgiOku.Kademe1Tuketimi;
            str += sp + BilgiOku.Kademe2Tuketimi;
            str += sp + BilgiOku.Kademe3Tuketimi;
            str += sp + BilgiOku.Kademe4Tuketimi;
            str += sp + BilgiOku.Kademe5Tuketimi;
            str += sp + BilgiOku.DonemTuketim1;
            str += sp + BilgiOku.DonemTuketim2;
            str += sp + BilgiOku.DonemTuketim3;
            str += sp + BilgiOku.DonemTuketim4;
            str += sp + BilgiOku.DonemTuketim5;
            str += sp + BilgiOku.DonemTuketim6;
            str += sp + BilgiOku.FixCharge;
            str += sp + BilgiOku.ToplamFixCharge;
            str += sp + BilgiOku.Versiyon;

            return str;

            #endregion


            #region Dosyaya Yazma İşlemleri

            //string DosyaAdi = "biloku.txt";
            //if (File.Exists(DosyaAdi))
            //{
            //    File.Delete(DosyaAdi);
            //}
            //StreamWriter Dosya = new StreamWriter(DosyaAdi);

            //Dosya.WriteLine("1");
            //Dosya.WriteLine(BilgiOku.AboneNo);
            //Dosya.WriteLine(BilgiOku.KartNo);
            //Dosya.WriteLine(BilgiOku.CihazNo);
            //Dosya.WriteLine(BilgiOku.Tip);
            //Dosya.WriteLine(BilgiOku.Cap);
            //Dosya.WriteLine(BilgiOku.KalanKredi);
            //Dosya.WriteLine(BilgiOku.HarcananKredi);
            //Dosya.WriteLine(BilgiOku.GerCekTuketim);
            //Dosya.WriteLine(BilgiOku.DonemTuketimi);
            //Dosya.WriteLine(BilgiOku.KritikKredi);
            //Dosya.WriteLine(BilgiOku.Limit1);
            //Dosya.WriteLine(BilgiOku.Limit2);
            //Dosya.WriteLine(BilgiOku.Limit3);
            //Dosya.WriteLine(BilgiOku.Limit4);
            //Dosya.WriteLine(BilgiOku.DonemGun);
            //Dosya.WriteLine(BilgiOku.DonemGunNo);
            //Dosya.WriteLine(BilgiOku.AnaPil);
            //Dosya.WriteLine(BilgiOku.Vop);
            //Dosya.WriteLine(BilgiOku.SayacTarihi);
            //Dosya.WriteLine(BilgiOku.SonKrediTarihi);
            //Dosya.WriteLine(BilgiOku.SonPulseTarihi);
            //Dosya.WriteLine(BilgiOku.SonCezaTarihi);
            //Dosya.WriteLine(BilgiOku.SonArizaTarihi);
            //Dosya.WriteLine(BilgiOku.Ceza3Tarihi);
            //Dosya.WriteLine(BilgiOku.Ceza4Tarihi);
            //Dosya.WriteLine(BilgiOku.SayacDurumu & 1);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 2) >> 1);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 4) >> 2);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 8) >> 3);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 16) >> 4);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 32) >> 5);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 64) >> 6);
            //Dosya.WriteLine((BilgiOku.SayacDurumu & 128) >> 7);
            //Dosya.WriteLine(BilgiOku.Kademe1Tuketimi);
            //Dosya.WriteLine(BilgiOku.Kademe2Tuketimi);
            //Dosya.WriteLine(BilgiOku.Kademe3Tuketimi);
            //Dosya.WriteLine(BilgiOku.Kademe4Tuketimi);
            //Dosya.WriteLine(BilgiOku.Kademe5Tuketimi);
            //Dosya.WriteLine(BilgiOku.DonemTuketim1);
            //Dosya.WriteLine(BilgiOku.DonemTuketim2);
            //Dosya.WriteLine(BilgiOku.DonemTuketim3);
            //Dosya.WriteLine(BilgiOku.DonemTuketim4);
            //Dosya.WriteLine(BilgiOku.DonemTuketim5);
            //Dosya.WriteLine(BilgiOku.DonemTuketim6);
            //Dosya.WriteLine(BilgiOku.FixCharge);
            //Dosya.WriteLine(BilgiOku.ToplamFixCharge);
            //Dosya.WriteLine(BilgiOku.Versiyon);
            //Dosya.Close();

            //return "1";

            #endregion


        }

        public string YetkiBilgiOkuETrans(byte ZoneIndex, ref YetkiKarti data)
        {
            HataSet(0);

            byte[] PartOku = new byte[33];
            byte[] KartOku = new byte[256];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea.Substring(0, 4) != issue_area[0] + issue_area[1] + "YE") return Return(19);

            Status = VerifyPin(0x82, PinYetki);
            if (!Status) return Return(20);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            //Verileri Oku..

            for (int i = 0; i < 8; i++)
            {
                PartOku = ReadBinary(Convert.ToByte(i * 32), 32);
                Array.Copy(PartOku, 0, KartOku, i * 32, 32);
            }

            FinishKart();

            YetkiKarti BilgiOku = new YetkiKarti(KartOku);
            data = BilgiOku;

            if (BilgiOku.BilgiTipi != 0x05)
            {
                return Return(21);
            }

            return "1";

        }

        public string ETrans(YetkiKarti BilOkunupDegistirilen, byte ZoneIndex)
        {
            HataSet(0);

            string str = "";
            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea.Substring(0, 3) != issue_area[0] + issue_area[1] + "Y") return Return(19);

            Status = VerifyPin(0x82, PinYetki);
            if (!Status) return Return(20);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            byte Index = 0;
            BilOkunupDegistirilen.BilgiTipi = 12;  //ETRANS  cihaz no değişmez
            Yetki[Index] = BilOkunupDegistirilen.BilgiTipi; Index++;
            Yetki[Index] = BilOkunupDegistirilen.KartKodu; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Cap; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Tip; Index++;
            YetkiDoldur(BilOkunupDegistirilen.CihazNo, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.AboneNo, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.KalanKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.HarcananKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.KritikKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.NegatifTuketim, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.GerCekTuketim, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SonYuklenenKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe1Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe2Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe3Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim5, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim6, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SayacTarihi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SonKrediTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonPulseTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonCezaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonArizaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.Ceza3Tarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.Ceza4Tarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.BorcTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.VanaAcilmaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.VanaKapanmaTarihi, Index); Index += 2;
            Yetki[Index] = Convert.ToByte(Convert.ToDouble(BilOkunupDegistirilen.MotorPil) * 255 / 6); Index++;
            Yetki[Index] = BilOkunupDegistirilen.HaftaninGunu; Index++;

            switch (BilOkunupDegistirilen.ArizaTipi)
            {
                case "ServisA":
                    Yetki[Index] = 0x10; Index++;
                    break;
                case "ServisK":
                    Yetki[Index] = 0x20; Index++;
                    break;
                case "ServisS":
                    Yetki[Index] = 0x30; Index++;
                    break;
                case "ServisP":
                    Yetki[Index] = 0x40; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            switch (BilOkunupDegistirilen.ETransDurum)
            {
                case "Olumlu":
                    Yetki[Index] = 11; Index++;
                    break;
                case "Olumsuz":
                    Yetki[Index] = 0x00; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.VanaPulseSure; Index++;
            Yetki[Index] = BilOkunupDegistirilen.VanaCntSure; Index++;
            Yetki[Index] = BilOkunupDegistirilen.KartNo; Index++;
            Yetki[Index] = BilOkunupDegistirilen.IslemNo; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Vop; Index++;
            Yetki[Index] = BilOkunupDegistirilen.TestSaat; Index++;

            switch (BilOkunupDegistirilen.TestReel)
            {
                case "Test Mode":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Reel Mode":
                    Yetki[Index] = 0xFF; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.SayacDurumu; Index++;
            Yetki[Index] = Convert.ToByte(Convert.ToDouble(BilOkunupDegistirilen.AnaPil) * 255 / 6); Index++;

            switch (BilOkunupDegistirilen.Format)
            {
                case "Sayaç Formatlı":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Sayaç Formatsız":
                    Yetki[Index] = 0x01; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.Versiyon; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Kademe; Index++;
            Yetki[Index] = BilOkunupDegistirilen.DonemGun; Index++;
            Yetki[Index] = BilOkunupDegistirilen.DonemGunNo; Index++;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiSeviye, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiSinir, Index); Index += 2;

            switch (BilOkunupDegistirilen.VanaDurumu)
            {
                case "Kapalı":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Açık":
                    Yetki[Index] = 0x01; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim7, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim8, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim9, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim10, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim11, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim12, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.MekanikTuketim, Index); Index += 4;

            Yetki[Index] = BilOkunupDegistirilen.HaftaSonuOnay; Index++;
            Yetki[Index] = BilOkunupDegistirilen.ResetSayisi; Index++;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi5, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe4Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe5Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.FixCharge, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.ToplamFixCharge, Index); Index += 4;



            for (int i = 0; i < 8; i++)
            {
                UpdateBinaryAdet(Convert.ToByte(i * 32), 32, Yetki);
            }

            FinishKart();

            return "1";


        }

        public string EDegis(YetkiKarti BilOkunupDegistirilen, byte ZoneIndex)
        {
            HataSet(0);

            string str = "";
            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea.Substring(0, 3) != issue_area[0] + issue_area[1] + "Y") return Return(18);

            Status = VerifyPin(0x82, PinYetki);
            if (!Status) return Return(20);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            byte Index = 0;
            BilOkunupDegistirilen.BilgiTipi = 10; //EDEĞİŞ
            Yetki[Index] = BilOkunupDegistirilen.BilgiTipi; Index++;
            Yetki[Index] = BilOkunupDegistirilen.KartKodu; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Cap; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Tip; Index++;
            YetkiDoldur(BilOkunupDegistirilen.CihazNo, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.AboneNo, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.KalanKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.HarcananKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.KritikKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.NegatifTuketim, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.GerCekTuketim, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SonYuklenenKredi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe1Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe2Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe3Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim1, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim2, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim5, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim6, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SayacTarihi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.SonKrediTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonPulseTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonCezaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.SonArizaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.Ceza3Tarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.Ceza4Tarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.BorcTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.VanaAcilmaTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.VanaKapanmaTarihi, Index); Index += 2;
            Yetki[Index] = Convert.ToByte(Convert.ToDouble(BilOkunupDegistirilen.MotorPil) * 255 / 6); Index++;
            Yetki[Index] = BilOkunupDegistirilen.HaftaninGunu; Index++;

            switch (BilOkunupDegistirilen.ArizaTipi)
            {
                case "ServisA":
                    Yetki[Index] = 0x10; Index++;
                    break;
                case "ServisK":
                    Yetki[Index] = 0x20; Index++;
                    break;
                case "ServisS":
                    Yetki[Index] = 0x30; Index++;
                    break;
                case "ServisP":
                    Yetki[Index] = 0x40; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            switch (BilOkunupDegistirilen.ETransDurum)
            {
                case "Olumlu":
                    Yetki[Index] = 11; Index++;
                    break;
                case "Olumsuz":
                    Yetki[Index] = 0x00; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.VanaPulseSure; Index++;
            Yetki[Index] = BilOkunupDegistirilen.VanaCntSure; Index++;
            Yetki[Index] = BilOkunupDegistirilen.KartNo; Index++;
            Yetki[Index] = BilOkunupDegistirilen.IslemNo; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Vop; Index++;
            Yetki[Index] = BilOkunupDegistirilen.TestSaat; Index++;

            switch (BilOkunupDegistirilen.TestReel)
            {
                case "Test Mode":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Reel Mode":
                    Yetki[Index] = 0xFF; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.SayacDurumu; Index++;
            Yetki[Index] = Convert.ToByte(Convert.ToDouble(BilOkunupDegistirilen.AnaPil) * 255 / 6); Index++;

            switch (BilOkunupDegistirilen.Format)
            {
                case "Sayaç Formatlı":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Sayaç Formatsız":
                    Yetki[Index] = 0x01; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            Yetki[Index] = BilOkunupDegistirilen.Versiyon; Index++;
            Yetki[Index] = BilOkunupDegistirilen.Kademe; Index++;
            Yetki[Index] = BilOkunupDegistirilen.DonemGun; Index++;
            Yetki[Index] = BilOkunupDegistirilen.DonemGunNo; Index++;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiTarihi, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiSeviye, Index); Index += 2;
            YetkiDoldur(BilOkunupDegistirilen.MaxdebiSinir, Index); Index += 2;

            switch (BilOkunupDegistirilen.VanaDurumu)
            {
                case "Kapalı":
                    Yetki[Index] = 0x00; Index++;
                    break;
                case "Açık":
                    Yetki[Index] = 0x01; Index++;
                    break;
                default:
                    Yetki[Index] = 0x00; Index++;
                    break;
            }

            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim7, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim8, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim9, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim10, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim11, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.DonemTuketim12, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.MekanikTuketim, Index); Index += 4;

            Yetki[Index] = BilOkunupDegistirilen.HaftaSonuOnay; Index++;
            Yetki[Index] = BilOkunupDegistirilen.ResetSayisi; Index++;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Katsayi5, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit3, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Limit4, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe4Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.Kademe5Tuketimi, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.FixCharge, Index); Index += 4;
            YetkiDoldur(BilOkunupDegistirilen.ToplamFixCharge, Index); Index += 4;



            for (int i = 0; i < 8; i++)
            {
                UpdateBinaryAdet(Convert.ToByte(i * 32), 32, Yetki);
            }

            FinishKart();

            return "1";


        }

        public string YetkiCeza4(byte Ceza4, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "Y4", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            byte[] c4 = { Ceza4 };
            bool Status = UpdateBinary(0, c4);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiTuketim(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YT", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            byte[] yt = { 0, 1 };
            bool Status = UpdateBinary(0, yt);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiTuketimOku(byte ZoneIndex)
        {
            HataSet(0);

            byte[] PartOku = new byte[33];
            byte[] KartOku = new byte[256];

            int init = InitKart();
            if (init == 0) return Return(1);

            bool Status = SelectFile(DFName);
            if (!Status) return Return(2);

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) return Return(3);

            Status = SelectFile(EFIssuerName);
            if (!Status) return Return(4);

            byte[] response = ReadBinary(0, 12);
            if (!Status) return Return(5);

            GetIssuer(ZoneIndex);

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea.Substring(0, 4) != issue_area[0] + issue_area[1] + "YT") return Return(22);

            Status = VerifyPin(0x82, PinYetki);
            if (!Status) return Return(20);

            Status = SelectFile(EFDataName1);
            if (!Status) return Return(6);

            //Verileri Oku..

            for (int i = 0; i < 8; i++)
            {
                PartOku = ReadBinary(Convert.ToByte(i * 32), 32);
                Array.Copy(PartOku, 0, KartOku, i * 32, 32);
            }

            FinishKart();

            YetkiKartiTuketim TuketimOku = new YetkiKartiTuketim(KartOku);

            if (TuketimOku.Durum != 0x05)
            {
                return Return(21);
            }

            string str = "1";

            str += sp + TuketimOku.Durum;
            str += sp + TuketimOku.KartKodu;
            str += sp + TuketimOku.DonemGun;
            str += sp + TuketimOku.DonemGunNo;
            str += sp + TuketimOku.CihazNo;
            str += sp + TuketimOku.AboneNo;
            str += sp + TuketimOku.KalanKredi;
            str += sp + TuketimOku.HarcananKredi;
            str += sp + TuketimOku.DonemTuketimi;
            str += sp + TuketimOku.NegatifTuketim;
            str += sp + TuketimOku.GercekTuketim;
            str += sp + TuketimOku.SonYuklenenKredi;
            str += sp + TuketimOku.Kademe1Tuketimi;
            str += sp + TuketimOku.Kademe2Tuketimi;
            str += sp + TuketimOku.Kademe3Tuketimi;
            str += sp + TuketimOku.DonemTuketim1;
            str += sp + TuketimOku.DonemTuketim2;
            str += sp + TuketimOku.DonemTuketim3;
            str += sp + TuketimOku.DonemTuketim4;
            str += sp + TuketimOku.DonemTuketim5;
            str += sp + TuketimOku.DonemTuketim6;
            str += sp + TuketimOku.DonemTuketim7;
            str += sp + TuketimOku.DonemTuketim8;
            str += sp + TuketimOku.DonemTuketim9;
            str += sp + TuketimOku.DonemTuketim10;
            str += sp + TuketimOku.DonemTuketim11;
            str += sp + TuketimOku.DonemTuketim12;
            str += sp + TuketimOku.DonemTuketim13;
            str += sp + TuketimOku.DonemTuketim14;
            str += sp + TuketimOku.DonemTuketim15;
            str += sp + TuketimOku.DonemTuketim16;
            str += sp + TuketimOku.DonemTuketim17;
            str += sp + TuketimOku.DonemTuketim18;
            str += sp + TuketimOku.DonemTuketim19;
            str += sp + TuketimOku.DonemTuketim20;
            str += sp + TuketimOku.DonemTuketim21;
            str += sp + TuketimOku.DonemTuketim22;
            str += sp + TuketimOku.DonemTuketim23;
            str += sp + TuketimOku.DonemTuketim24;

            return str;
        }

        public string YetkiReset(UInt32 CihazNo, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YR", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            Yetki0007(CihazNo, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiIptal(UInt32 CihazNo, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(18);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YC", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            Yetki0007(CihazNo, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiHariciEE(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YH", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            byte[] yh = { 0, 1 };
            bool Status = UpdateBinary(0, yh);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiAvans(UInt32 CihazNo, UInt32 kat1, UInt32 kat2, UInt32 kat3, UInt32 lim1, UInt32 lim2, byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer(issue_area[0] + issue_area[1] + "YV", 0);
                EFDataName1Eris("Yetki");
            }
            else return Return(18);

            YazilacakDuzenleYetkiAvans(CihazNo, kat1, kat2, kat3, lim1, lim2);
            bool Status = UpdateBinary(0, YetkiAvansDizi);
            if (!Status) return Return(14);

            FinishKart();
            return "1";
        }

        public string YetkiBosalt(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            string IssuerIlk3Harf = Issuer.IssuerArea.Substring(0, 3);

            if (IssuerIlk3Harf == issue_area[0] + issue_area[1] + "Y")
            {
                UpdateIssuer("\0\0\0\0", 0);
                EFDataName1Eris("Yetki");
            }
            else if (IssuerIlk3Harf == "ASU")
            {
                UpdateIssuer("\0\0\0\0", 0);
                EFDataName1Eris("Uretim");
            }
            else
                return Return(18);

            //Verileri Değiştir..

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            bool Status;

            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) return Return(16);
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status) return Return(16);

            FinishKart();

            return "1";
        }

        #endregion

        #region Üretim fonksyonları

        #region yazılacak paketler

        private void Uretim0023()
        {
            byte[] Bosalt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Yazilacak = Bosalt;
        }

        public void Uretim0023(UInt32 CihazNo, UInt32 Katsayi1, UInt32 Katsayi2, UInt32 Katsayi3, UInt32 Limit1, UInt32 Limit2)
        {
            Integer4Byte Cihaz = new Integer4Byte(CihazNo);
            Integer4Byte Kat1 = new Integer4Byte(Katsayi1);
            Integer4Byte Kat2 = new Integer4Byte(Katsayi2);
            Integer4Byte Kat3 = new Integer4Byte(Katsayi3);
            Integer4Byte Lim1 = new Integer4Byte(Limit1);
            Integer4Byte Lim2 = new Integer4Byte(Limit2);

            Yazilacak[0] = Cihaz.bir;
            Yazilacak[1] = Cihaz.iki;
            Yazilacak[2] = Cihaz.uc;
            Yazilacak[3] = Cihaz.dort;

            Yazilacak[4] = Kat1.bir;
            Yazilacak[5] = Kat1.iki;
            Yazilacak[6] = Kat1.uc;
            Yazilacak[7] = Kat1.dort;

            Yazilacak[8] = Kat2.bir;
            Yazilacak[9] = Kat2.iki;
            Yazilacak[10] = Kat2.uc;
            Yazilacak[11] = Kat2.dort;

            Yazilacak[12] = Kat3.bir;
            Yazilacak[13] = Kat3.iki;
            Yazilacak[14] = Kat3.uc;
            Yazilacak[15] = Kat3.dort;

            Yazilacak[16] = Lim1.bir;
            Yazilacak[17] = Lim1.iki;
            Yazilacak[18] = Lim1.uc;
            Yazilacak[19] = Lim1.dort;

            Yazilacak[20] = Lim2.bir;
            Yazilacak[21] = Lim2.iki;
            Yazilacak[22] = Lim2.uc;
            Yazilacak[23] = Lim2.dort;
        }

        public void Uretim2435()
        {
            byte[] Bosalt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            UretimYazilacak = Bosalt;
        }

        public void Uretim2435(UInt32 KritikKredi, byte DonemGun, byte VanaPulseSure, byte VanaCntSure)
        {
            Integer4Byte KritikKredii = new Integer4Byte(KritikKredi);
            TarihAl GuncelTarih = new TarihAl(DateTime.Now);

            UretimYazilacak[0] = KritikKredii.bir;
            UretimYazilacak[1] = KritikKredii.iki;
            UretimYazilacak[2] = KritikKredii.uc;
            UretimYazilacak[3] = KritikKredii.dort;

            UretimYazilacak[4] = GuncelTarih.bir;
            UretimYazilacak[5] = GuncelTarih.iki;
            UretimYazilacak[6] = GuncelTarih.uc;
            UretimYazilacak[7] = GuncelTarih.dort;

            UretimYazilacak[8] = GuncelTarih.HaftaninGunu;
            UretimYazilacak[9] = DonemGun;
            UretimYazilacak[10] = VanaPulseSure;
            UretimYazilacak[11] = VanaCntSure;
        }

        public void UretimCihazNo2437(byte DonemGun, UInt32 Mek_Tuk, byte SayacCapi, byte Issue1, byte Issue2)
        {
            TarihAl GuncelTarih = new TarihAl(DateTime.Now);
            Integer4Byte MekTuketim = new Integer4Byte(Mek_Tuk);

            UretimYazilacak[0] = GuncelTarih.bir;
            UretimYazilacak[1] = GuncelTarih.iki;
            UretimYazilacak[2] = GuncelTarih.uc;
            UretimYazilacak[3] = GuncelTarih.dort;

            UretimYazilacak[4] = GuncelTarih.HaftaninGunu;
            UretimYazilacak[5] = DonemGun;
            UretimYazilacak[6] = 0xFF;

            UretimYazilacak[7] = MekTuketim.bir;
            UretimYazilacak[8] = MekTuketim.iki;
            UretimYazilacak[9] = MekTuketim.uc;
            UretimYazilacak[10] = MekTuketim.dort;
            UretimYazilacak[11] = SayacCapi;
            UretimYazilacak[12] = Issue1;
            UretimYazilacak[13] = Issue2;
        }

        public void UretimCihazNo3853(UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4)
        {
            Integer4Byte Kat4 = new Integer4Byte(Katsayi4);
            Integer4Byte Kat5 = new Integer4Byte(Katsayi5);
            Integer4Byte Lim3 = new Integer4Byte(Limit3);
            Integer4Byte Lim4 = new Integer4Byte(Limit4);

            Yazilacak[0] = Kat4.bir;
            Yazilacak[1] = Kat4.iki;
            Yazilacak[2] = Kat4.uc;
            Yazilacak[3] = Kat4.dort;

            Yazilacak[4] = Kat5.bir;
            Yazilacak[5] = Kat5.iki;
            Yazilacak[6] = Kat5.uc;
            Yazilacak[7] = Kat5.dort;

            Yazilacak[8] = Lim3.bir;
            Yazilacak[9] = Lim3.iki;
            Yazilacak[10] = Lim3.uc;
            Yazilacak[11] = Lim3.dort;

            Yazilacak[12] = Lim4.bir;
            Yazilacak[13] = Lim4.iki;
            Yazilacak[14] = Lim4.uc;
            Yazilacak[15] = Lim4.dort;
        }

        public void Uretim3751(UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4)
        {
            Integer4Byte Kat4 = new Integer4Byte(Katsayi4);
            Integer4Byte Kat5 = new Integer4Byte(Katsayi5);
            Integer4Byte Lim3 = new Integer4Byte(Limit3);
            Integer4Byte Lim4 = new Integer4Byte(Limit4);

            Yazilacak[0] = Kat4.bir;
            Yazilacak[1] = Kat4.iki;
            Yazilacak[2] = Kat4.uc;
            Yazilacak[3] = Kat4.dort;

            Yazilacak[4] = Kat5.bir;
            Yazilacak[5] = Kat5.iki;
            Yazilacak[6] = Kat5.uc;
            Yazilacak[7] = Kat5.dort;

            Yazilacak[8] = Lim3.bir;
            Yazilacak[9] = Lim3.iki;
            Yazilacak[10] = Lim3.uc;
            Yazilacak[11] = Lim3.dort;

            Yazilacak[12] = Lim4.bir;
            Yazilacak[13] = Lim4.iki;
            Yazilacak[14] = Lim4.uc;
            Yazilacak[15] = Lim4.dort;
        }

        public void Uretim0005(UInt32 CihazNo, byte KontrolDegeri)
        {
            Integer4Byte Cihaz = new Integer4Byte(CihazNo);

            UretimYazilacak5Byte[0] = Cihaz.bir;
            UretimYazilacak5Byte[1] = Cihaz.iki;
            UretimYazilacak5Byte[2] = Cihaz.uc;
            UretimYazilacak5Byte[3] = Cihaz.dort;

            UretimYazilacak5Byte[4] = KontrolDegeri;
        }

        public void Uretim0005()
        {
            TarihAl GuncelTarih = new TarihAl(DateTime.Now);

            UretimYazilacak5Byte[0] = GuncelTarih.bir;
            UretimYazilacak5Byte[1] = GuncelTarih.iki;
            UretimYazilacak5Byte[2] = GuncelTarih.uc;
            UretimYazilacak5Byte[3] = GuncelTarih.dort;

            UretimYazilacak5Byte[4] = GuncelTarih.HaftaninGunu;
        }


        #endregion

        public string FormUret()
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                UpdateIssuer("ASUF", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(11);


            Uretim0023();
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);

            Uretim2435();
            Status = UpdateBinary(24, UretimYazilacak);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimFormat(UInt32 CihazNo,
                                   UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                   UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4, UInt32 KritikKredi,
                                   byte DonemGun, byte VanaPulseSure, byte VanaCntSure, byte Cap)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUF", CihazNo);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            Uretim0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);

            Uretim2435(KritikKredi, DonemGun, VanaPulseSure, VanaCntSure);
            Status = UpdateBinary(24, UretimYazilacak);
            if (!Status) return Return(14);

            Status = UpdateBinary(36, Cap);
            if (!Status) return Return(14);

            Uretim3751(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4);
            Status = UpdateBinary(37, Yazilacak);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimCihazNo(UInt32 CihazNo,
                                    UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                    UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4, UInt32 KritikKredi,
                                    byte DonemGun, UInt32 MekanikTuketim, byte Cap, Byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUZ", CihazNo);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            Uretim0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status) return Return(14);

            byte[] YazilacakVeriler = new byte[4];

            GetIssuer(ZoneIndex);

            YazilacakVeriler = Hexcon.String4to4Byte(issue_area[0] + issue_area[1] + "  ");

            UretimCihazNo2437(DonemGun, MekanikTuketim, Cap, YazilacakVeriler[0], YazilacakVeriler[1]);
            Status = UpdateBinary(24, UretimYazilacak);
            if (!Status) return Return(14);

            UretimCihazNo3853(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4);
            Status = UpdateBinary(38, Yazilacak);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimReelMod()
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUR", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            FinishKart();

            return "1";
        }

        public string UretimTestMod(UInt32 CihazNo)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUT", CihazNo);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            Uretim0005(CihazNo, 0xFF);
            bool Status = UpdateBinary(0, UretimYazilacak5Byte);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimAc(byte KontrolDegeri)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUA", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            bool Status = UpdateBinary(0, KontrolDegeri);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimKapat(byte KontrolDegeri)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUK", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            bool Status = UpdateBinary(0, KontrolDegeri);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimSifirlama()
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUS", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            Uretim0005();
            bool Status = UpdateBinary(0, UretimYazilacak5Byte);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimZone(byte ZoneIndex)
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            byte[] YazilacakVeriler = new byte[4];

            GetIssuer(ZoneIndex);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUI", 0);
                EFDataName1Eris("Uretim");
            }
            else return Return(23);

            YazilacakVeriler = Hexcon.String4to4Byte(issue_area[0] + issue_area[1] + "  ");

            bool Status = UpdateBinary(0, YazilacakVeriler[0], YazilacakVeriler[1]);
            if (!Status) return Return(14);

            FinishKart();

            return "1";
        }

        public string UretimBosalt()
        {
            HataSet(0);

            int init = InitKart();
            if (init == 0) return Return(1);

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            string IssuerIlk3Harf = Issuer.IssuerArea.Substring(0, 3);

            if (IssuerIlk3Harf == "ASU")
            {
                UpdateIssuer("\0\0\0\0", 0);
                EFDataName1Eris("Uretim");
            }
            else
                return Return(23);

            //Verileri Değiştir..

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            bool Status;

            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) return Return(16);
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status) return Return(16);

            FinishKart();

            return "1";
        }

        #endregion

        #endregion
    }
}
