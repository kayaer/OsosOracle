using System;
using System.Runtime.InteropServices;
using SCLibWin;
using System.Collections;
using SmartCard.Interface;
using SmartCard.Model;

namespace SmartCard
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class Mcm : ISmartCard, IAcr128, IUretim, IYetki
    {

        #region Degiskenler
        public SCResMgr mng;
        public SCReader rd;
        public string rdName;
        public string DFName = "NN";
        public string EFIssuerName = "EI";
        public string EFDataName1 = "E1";
        public string EFDataName2 = "E2";
        public string EFDataName3 = "E3";
        public string EFDataName4 = "E4";
        public byte[] DKAbone = { 0x12, 0x53, 0x83, 0x26, 0x26, 0x61, 0x15, 0x98, 0x42, 0x11, 0x45, 0x76, 0x13, 0x62, 0x77, 0x19 };
        //public byte[] DKAbone = { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF };
        public byte[] PinUretim = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        public byte[] PinYetki = { 0x34, 0x61, 0x29, 0x56, 0x25, 0x07, 0x13, 0x68 };
        public string _hata;
        string sp = "#";
        byte[] Yazilacak = new byte[24];
        byte[] YetkiYazilacak = new byte[12];
        #endregion

        #region ISmartCard
        public string AboneOku()
        {
            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();

                return HataSet(1);
            }

            bool Status;

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                FinishKart();
                return HataSet(29);
            }
            //else if (Issuer.IssuerArea.Substring(0, 2) != GlobalAyarlar.GetInstance().Issuer)
            //{
            //    FinishKart();
            //    return HataSet(30);
            //}
            //else if (Issuer.IssuerArea != GlobalAyarlar.GetInstance().Issuer + "A ")
            //{
            //    FinishKart();
            //    return HataSet(7);
            //}

            Integer4Byte CihazNoo = new Integer4Byte(Issuer.CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);
            Status = VerifyPin(0X83, PinAbone);
            if (!Status)
            {
                FinishKart();
                return HataSet(8);
            }

            Status = SelectFile(EFDataName1);
            if (!Status)
            {
                FinishKart();
                return HataSet(6);
            }

            Abone abo = new Abone(ReadBinary(0, 255));

            Status = SelectFile(EFDataName2);
            if (!Status) { HataSet(6); return FinishKart(); }
            Abone000048 Adres242290 = new Abone000048(ReadBinary(0, 48));

            Status = SelectFile(EFDataName2);
            if (!Status)
            {
                FinishKart();
                return HataSet(31);
            }
            AylikSicaklikBilgisi saatlikSicaklik = new AylikSicaklikBilgisi(ReadBinary(48, 24));

            byte[] a = ReadBinary(72, 1);
            byte BaglantiPeriyot = a[0];

            FinishKart();
            string AnaKrediOkunma = "";
            if (((abo.AkoYko & 128) >> 7) == 0)
            { AnaKrediOkunma = "*"; }
            else
            {
                AnaKrediOkunma = "b";
            }
            string YedekKrediOkunma = "";
            if (((abo.AkoYko & 64) >> 6) == 0)
            {
                YedekKrediOkunma = "*";
            }
            else
            {
                YedekKrediOkunma = "b";
            }

            abo.SayacDurumu = Convert.ToByte((abo.SayacDurumu & 128) + (abo.SayacDurumu & 64) + (abo.SayacDurumu & 32) + (abo.SayacDurumu & 2) + (abo.SayacDurumu & 1));
            string SayacCeza = "";
            if (abo.SayacDurumu == 0)
            {
                SayacCeza = "0";
            }
            else
            {
                SayacCeza = "1";
            }

            byte IadeYapilmis = 0;
            switch (abo.Iade)
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
                IadeKredi = abo.IadeKalan;
                if (AnaKrediOkunma == "b")
                {
                    IadeKredi += abo.AnaKredi;
                }
                if (YedekKrediOkunma == "b")
                {
                    IadeKredi += abo.YedekKredi;
                }
            }

            string AnaPilSeviyesi = String.Format("{0,8:0.000}", (abo.AnaPil * 6 / 255.0));


            byte SayacTipi = 2;

            byte Bayram1Ay = Convert.ToByte(abo.Bayram1 & 0X000F);
            byte Bayram1Gun = Convert.ToByte((abo.Bayram1 >> 4) & 0X001F);
            byte Bayram1Sure = Convert.ToByte(abo.Bayram1 >> 9);

            byte Bayram2Ay = Convert.ToByte(abo.Bayram2 & 0X000F);
            byte Bayram2Gun = Convert.ToByte((abo.Bayram2 >> 4) & 0X001F);
            byte Bayram2Sure = Convert.ToByte(abo.Bayram2 >> 9);

            Integer2Byte krt1 = new Integer2Byte(Adres242290.krediTarihi1);
            Integer2Byte krt2 = new Integer2Byte(Adres242290.krediTarihi2);
            Integer2Byte krt3 = new Integer2Byte(Adres242290.krediTarihi3);
            Integer2Byte krt4 = new Integer2Byte(Adres242290.krediTarihi4);
            Integer2Byte krt5 = new Integer2Byte(Adres242290.krediTarihi5);


            var ako0 = Adres242290.ako0 == 192 ? "b" : Adres242290.ako0 == 67 ? "C" : "O";
            var ako1 = Adres242290.ako1 == 192 ? "b" : Adres242290.ako1 == 67 ? "C" : "O";
            var ako2 = Adres242290.ako2 == 192 ? "b" : Adres242290.ako2 == 67 ? "C" : "O";
            var ako3 = Adres242290.ako3 == 192 ? "b" : Adres242290.ako3 == 67 ? "C" : "O";
            var ako4 = Adres242290.ako4 == 192 ? "b" : Adres242290.ako4 == 67 ? "C" : "O";
            var ako5 = Adres242290.ako5 == 192 ? "b" : Adres242290.ako5 == 67 ? "C" : "O";



            #region dönüş değerleri
            string sp = "#";
            string sReturn = "1";
            sReturn += sp + abo.AboneNo;
            sReturn += sp + abo.KartNo;
            sReturn += sp + Issuer.CihazNo;
            sReturn += sp + AnaKrediOkunma;
            sReturn += sp + YedekKrediOkunma;
            sReturn += sp + ako0;
            sReturn += sp + ako1;
            sReturn += sp + ako2;
            sReturn += sp + ako3;
            //sReturn += sp + ako4;
            //sReturn += sp + ako5;
            sReturn += sp + Adres242290.kredi1;
            sReturn += sp + Adres242290.kredi2;
            sReturn += sp + Adres242290.kredi3;
            sReturn += sp + Adres242290.kredi4;
            sReturn += sp + Adres242290.kredi5;
            sReturn += sp + Hexcon.TarihDuzenle(krt1.bir, krt1.iki);
            sReturn += sp + Hexcon.TarihDuzenle(krt2.bir, krt2.iki);
            sReturn += sp + Hexcon.TarihDuzenle(krt3.bir, krt3.iki);
            sReturn += sp + Hexcon.TarihDuzenle(krt4.bir, krt4.iki);
            sReturn += sp + Hexcon.TarihDuzenle(krt5.bir, krt5.iki);
            sReturn += sp + Adres242290.unique0;
            sReturn += sp + Adres242290.unique1;
            sReturn += sp + Adres242290.unique2;
            sReturn += sp + Adres242290.unique3;
            sReturn += sp + Adres242290.unique4;
            sReturn += sp + Adres242290.unique5;
            sReturn += sp + abo.AnaKredi;
            sReturn += sp + abo.YedekKredi;
            sReturn += sp + SayacCeza;
            sReturn += sp + "*";
            sReturn += sp + abo.Tip;
            sReturn += sp + abo.Cap;
            sReturn += sp + abo.DonemGun;
            sReturn += sp + abo.Limit1;
            sReturn += sp + abo.Limit2;
            sReturn += sp + IadeYapilmis;
            sReturn += sp + String.Format("{0,10:0}", IadeKredi) + "_" + String.Format("{0,10:0}", abo.HarcananKredi);
            //  sReturn += sp + abo.DonemGun;
            sReturn += sp + AnaPilSeviyesi;

            sReturn += sp + abo.KalanKredi;
            sReturn += sp + abo.HarcananKredi;
            // sReturn += sp + KritikKredi;
            sReturn += sp + abo.VanaOperasyonSayisi;

            sReturn += sp + abo.SonKrediTarihi;
            sReturn += sp + abo.SonPulseTarihi;
            sReturn += sp + abo.SonCezaTarihi;
            sReturn += sp + abo.SonArizaTarihi;

            sReturn += sp + (abo.SayacDurumu & 1);          // ceza 2 
            sReturn += sp + ((abo.SayacDurumu & 2) >> 1);   // arıza
            sReturn += sp + ((abo.SayacDurumu & 4) >> 2);   // iptal
            sReturn += sp + ((abo.SayacDurumu & 8) >> 3);   // pil az
            sReturn += sp + ((abo.SayacDurumu & 16) >> 4);  // pil son
            sReturn += sp + ((abo.SayacDurumu & 32) >> 5);  // ceza 4
            sReturn += sp + ((abo.SayacDurumu & 64) >> 6);  // ceza 3 
            sReturn += sp + ((abo.SayacDurumu & 128) >> 7); // pulse hata

            sReturn += sp + abo.GercekTuketim;
            sReturn += sp + abo.KademeTuketim1;
            sReturn += sp + abo.KademeTuketim2;
            sReturn += sp + abo.KademeTuketim3;
            sReturn += sp + abo.DonemTuketimi1;
            sReturn += sp + abo.DonemTuketimi2;
            sReturn += sp + abo.DonemTuketimi3;
            sReturn += sp + abo.DonemTuketimi4;
            sReturn += sp + abo.DonemTuketimi5;
            sReturn += sp + abo.DonemTuketimi6;
            sReturn += sp + abo.SayacTarihi;
            sReturn += sp + SayacTipi;
            sReturn += sp + abo.SonTakilanYetkiKartiOzellik1;
            sReturn += sp + abo.SonTakilanYetkiKartiOzellik2;
            sReturn += sp + abo.SonTakilanYetkiKartiOzellik3;
            sReturn += sp + abo.MaxDebiTarihi;

            sReturn += sp + abo.DonemTuketimi7;
            sReturn += sp + abo.DonemTuketimi8;
            sReturn += sp + abo.DonemTuketimi9;
            sReturn += sp + abo.DonemTuketimi10;
            sReturn += sp + abo.DonemTuketimi11;
            sReturn += sp + abo.DonemTuketimi12;
            sReturn += sp + abo.DonemTuketimi13;
            sReturn += sp + abo.DonemTuketimi14;
            sReturn += sp + abo.DonemTuketimi15;
            sReturn += sp + abo.DonemTuketimi16;
            sReturn += sp + abo.DonemTuketimi17;
            sReturn += sp + abo.DonemTuketimi18;
            sReturn += sp + abo.DonemTuketimi19;
            sReturn += sp + abo.DonemTuketimi20;
            sReturn += sp + abo.DonemTuketimi21;
            sReturn += sp + abo.DonemTuketimi22;
            sReturn += sp + abo.DonemTuketimi23;
            sReturn += sp + abo.DonemTuketimi24;

            sReturn += sp + abo.Mektuk;
            sReturn += sp + abo.IadeKalan;
            sReturn += sp + abo.ResetSayisi;
            sReturn += sp + abo.HaftaSonuOnay;
            sReturn += sp + abo.Sizinti;
            sReturn += sp + abo.SizintiMiktar;

            sReturn += sp + abo.Limit3;
            sReturn += sp + abo.Limit4;
            sReturn += sp + abo.KademeTuketim4;
            sReturn += sp + abo.KademeTuketim5;

            sReturn += sp + Bayram1Gun;
            sReturn += sp + Bayram1Ay;
            sReturn += sp + Bayram1Sure;
            sReturn += sp + Bayram2Gun;
            sReturn += sp + Bayram2Ay;
            sReturn += sp + Bayram2Sure;
            sReturn += sp + abo.IslemNo;

            sReturn += sp + abo.Katsayi1;
            sReturn += sp + abo.Katsayi2;
            sReturn += sp + abo.Katsayi3;
            sReturn += sp + abo.Katsayi4;
            sReturn += sp + abo.Katsayi5;

            if (abo.KartNo > 15)
                sReturn += sp + "*"; // yeni kart
            else
                sReturn += sp + "b";

            sReturn += sp + abo.SicaklikHataSeviyesi;
            sReturn += sp + abo.SicaklikHataTarihi;
            sReturn += sp + abo.YanginModuSuresi;
            sReturn += sp + abo.MaxdebiSiniri;
            sReturn += sp + BaglantiPeriyot;
            //sReturn += sp + saatlikSicaklik.Dergerler[0];
            //sReturn += sp + saatlikSicaklik.Dergerler[1];
            //sReturn += sp + saatlikSicaklik.Dergerler[2];
            //sReturn += sp + saatlikSicaklik.Dergerler[3];
            //sReturn += sp + saatlikSicaklik.Dergerler[4];
            //sReturn += sp + saatlikSicaklik.Dergerler[5];
            //sReturn += sp + saatlikSicaklik.Dergerler[6];
            //sReturn += sp + saatlikSicaklik.Dergerler[7];
            //sReturn += sp + saatlikSicaklik.Dergerler[8];
            //sReturn += sp + saatlikSicaklik.Dergerler[9];
            //sReturn += sp + saatlikSicaklik.Dergerler[10];
            //sReturn += sp + saatlikSicaklik.Dergerler[11];

            #endregion

            return sReturn;

        }

        public string AboneYap(uint devNo, uint AboneNo, byte KartNo, byte Cap, byte Tip, byte Donem,
            uint Fiyat1, uint Fiyat2, uint Fiyat3, uint Fiyat4, uint Fiyat5,
            uint Limit1, uint Limit2, uint Limit3, uint Limit4,
            byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
            byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri, UInt32 KritikKredi, byte BaglantiPeriyot)
        {

            string durum = CapLimitFiyatKontrol(Fiyat1, Fiyat2, Fiyat3, Fiyat4, Fiyat5, Limit1, Limit2, Limit3, Limit4, Cap);
            if (durum != "1")
            {
                FinishKart();
                return HataSet(Convert.ToInt32(durum));
            }

            UInt16 Bayram1, Bayram2;

            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            bool Status;

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea != "\0\0\0\0")
            {
                FinishKart();
                return HataSet(11);
            }

            Integer4Byte CihazNoo = new Integer4Byte(devNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            Status = UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "A ", devNo);
            if (!Status)
            {
                FinishKart();
                return HataSet(10);
            }

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = WriteKey(0X83, PinAbone);

            if (!Status)
            {
                Status = WriteKey(0X83, PinAbone);
                if (!Status)
                {
                    FinishKart();
                    return HataSet(9);
                }
            }

            Status = VerifyPin(0X83, PinAbone);
            if (!Status)
            {
                FinishKart();
                return HataSet(8);
            }

            Status = SelectFile(EFDataName1);
            if (!Status)
            {
                FinishKart();
                return HataSet(6);
            }

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
                case 125:
                case 150:
                case 200:
                case 250:
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

            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            KartNo = Convert.ToByte((KartNo & 0x0F) + 0x40);

            YazilacakDuzenle0023(devNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            if (IleriSaat == 1) IleriSaat = 0;
            else if (IleriSaat == 0) IleriSaat = 0X90;

            YazilacakDuzenle2447(0, KritikKredi, 0xC0, 0, KartNo, Cap, Tip, Donem, 25, 30, 0, MaxDebiSiniri, AvansOnayi, AboneNo, IleriSaat);
            Status = UpdateBinary(24, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            YazilacakDuzenle4851(Bayram1, Bayram2);
            Status = UpdateBinary(48, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            YazilacakDuzenle5271(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4, 0);
            Status = UpdateBinary(52, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = SelectFile(EFDataName1);
            Status = UpdateBinary(241, new byte[] { YanginModuSuresi });
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            #region İşlem no sıfırlama

            Status = YazilacakDuzenle242290(0, 192,
                           0, 0, 0, 192,
                           0, 0, 0, 192,
                           0, 0, 0, 192,
                           0, 0, 0, 192,
                           0, 0, 0, 192);

            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            #endregion


            #region BaglantiPeriyot
            Status = SelectFile(EFDataName2);
            Status = UpdateBinary(72, new byte[] { BaglantiPeriyot });
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            #endregion

            FinishKart();

            return "1";
        }


        //public string AboneYaz(uint devNo, uint AnaKredi, byte Cap, byte Donem, uint Fiyat1, uint Fiyat2, uint Fiyat3, uint Fiyat4, uint Fiyat5,
        //                       uint Limit1, uint Limit2, uint Limit3, uint Limit4,
        //                       byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
        //                       byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri,
        //                       ushort Unique0)
        //{
        //    #region Kullanılmayan alanlar
        //    UInt32 YedekKredi = 0;
        //    ushort Unique1 = 0, Unique2 = 0, Unique3 = 0, Unique4 = 0, Unique5 = 0;
        //    int Kredi1 = 0, Kredi2 = 0, Kredi3 = 0, Kredi4 = 0, Kredi5 = 0;
        //    ushort KrediTarihi1 = 0, KrediTarihi2 = 0, KrediTarihi3 = 0, KrediTarihi4 = 0, KrediTarihi5 = 0;
        //    byte Ako1 = 0, Ako2 = 0, Ako3 = 0, Ako4 = 0, Ako5 = 0;
        //    #endregion


        //    string durum = CapLimitFiyatKontrol(Fiyat1, Fiyat2, Fiyat3, Fiyat4, Fiyat5, Limit1, Limit2, Limit3, Limit4, Cap);
        //    if (durum != "1")
        //    {
        //        FinishKart();
        //        return HataSet(Convert.ToInt32(durum));
        //    }


        //    byte[] PinAbone = new byte[8];

        //    int init = InitKart();
        //    if (init == 0)
        //    {
        //        FinishKart();
        //        return HataSet(1);
        //    }

        //    bool Status;

        //    IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

        //    if (Issuer.IssuerArea == "\0\0\0\0")
        //    {
        //        FinishKart();
        //        return HataSet(29);
        //    }
        //    if (devNo != Issuer.CihazNo)
        //    {
        //        FinishKart();
        //        return HataSet(12);
        //    }


        //    Integer4Byte CihazNoo = new Integer4Byte(devNo);
        //    byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
        //                            Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
        //                            Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

        //    PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);
        //    Status = VerifyPin(0X83, PinAbone);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(8);
        //    }

        //    Status = SelectFile(EFDataName1);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(6);
        //    }

        //    AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));

        //    if (Cap != Degerler.Cap)
        //    {
        //        FinishKart();
        //        return HataSet(13);
        //    }

        //    UInt32 KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, KademeKatsayi4, KademeKatsayi5;
        //    KademeKatsayi1 = KademeKatsayi2 = KademeKatsayi3 = KademeKatsayi4 = KademeKatsayi5 = 0;
        //    switch (Cap)
        //    {
        //        case 15:
        //        case 25:
        //        case 20:
        //            Integer4Byte Katsayi1 = new Integer4Byte(Convert.ToUInt32(1000));
        //            Integer4Byte Katsayi2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 1000 / Fiyat1));
        //            Integer4Byte Katsayi3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 1000 / Fiyat1));
        //            Integer4Byte Katsayi4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 1000 / Fiyat1));
        //            Integer4Byte Katsayi5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 1000 / Fiyat1));
        //            KademeKatsayi1 = Katsayi1.value;
        //            KademeKatsayi2 = Katsayi2.value;
        //            KademeKatsayi3 = Katsayi3.value;
        //            KademeKatsayi4 = Katsayi4.value;
        //            KademeKatsayi5 = Katsayi5.value;
        //            break;
        //        case 40:
        //        case 50:
        //        case 65:
        //        case 80:
        //        case 100:
        //        case 125:
        //        case 150:
        //        case 200:
        //        case 250:
        //            Integer4Byte Katsayi_1 = new Integer4Byte(Convert.ToUInt32(10000));
        //            Integer4Byte Katsayi_2 = new Integer4Byte(Convert.ToUInt32(Fiyat2 * 10000 / Fiyat1));
        //            Integer4Byte Katsayi_3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 10000 / Fiyat1));
        //            Integer4Byte Katsayi_4 = new Integer4Byte(Convert.ToUInt32(Fiyat4 * 10000 / Fiyat1));
        //            Integer4Byte Katsayi_5 = new Integer4Byte(Convert.ToUInt32(Fiyat5 * 10000 / Fiyat1));
        //            KademeKatsayi1 = Katsayi_1.value;
        //            KademeKatsayi2 = Katsayi_2.value;
        //            KademeKatsayi3 = Katsayi_3.value;
        //            KademeKatsayi4 = Katsayi_4.value;
        //            KademeKatsayi5 = Katsayi_5.value;
        //            break;
        //    }
        //    string split = "-";
        //    UInt16 Bayram1, Bayram2;
        //    Bayram1 = Bayram1Suree;
        //    Bayram1 <<= 5;
        //    Bayram1 |= Bayram1Gunn;
        //    Bayram1 <<= 4;
        //    Bayram1 |= Bayram1Ayy;

        //    Bayram2 = Bayram2Suree;
        //    Bayram2 <<= 5;
        //    Bayram2 |= Bayram2Gunn;
        //    Bayram2 <<= 4;
        //    Bayram2 |= Bayram2Ayy;

        //    if ((Degerler.AkoYko & 128) == 0)
        //    {
        //        Degerler.IslemNo++;
        //    }
        //    if ((Degerler.AkoYko & 64) == 0)
        //    {
        //        Degerler.IslemNo++;
        //    }

        //    Abone024051 Adres024051 = new Abone024051(ReadBinary(24, 28));
        //    Abone120143 Adres120143 = new Abone120143(ReadBinary(120, 24));

        //    Status = SelectFile(EFDataName2);
        //    Abone000048 Adres000048 = new Abone000048(ReadBinary(0, 48));
        //    Status = SelectFile(EFDataName1);
        //    // sonra kaldırılacak çünkü satış biriminden gelmesi gerekiyor.

        //    YazilacakDuzenle0023(devNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
        //    Status = UpdateBinary(0, Yazilacak);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    if (IleriSaat == 1) IleriSaat = 0;
        //    else if (IleriSaat == 0) IleriSaat = 0X90;

        //    YazilacakDuzenle2447(AnaKredi, YedekKredi, 192, Degerler.IslemNo, Degerler.KartNo, Cap, Degerler.Tip, Donem, 25, 30, 0, MaxDebiSiniri, AvansOnayi, Degerler.AboneNo, IleriSaat);
        //    Status = UpdateBinary(24, Yazilacak);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    YazilacakDuzenle4851(Bayram1, Bayram2);
        //    Status = UpdateBinary(48, Yazilacak);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    Abone072095 Adres072095 = new Abone072095(ReadBinary(72, 24));

        //    YazilacakDuzenle5271(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4, Adres072095.KalanKredi);
        //    Status = UpdateBinary(52, Yazilacak);
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    Status = YazilacakDuzenle242290(Unique0, 192,
        //                    Unique1, Kredi1, KrediTarihi1, Ako1,
        //                    Unique2, Kredi2, KrediTarihi2, Ako2,
        //                    Unique3, Kredi3, KrediTarihi3, Ako3,
        //                    Unique4, Kredi4, KrediTarihi4, Ako4,
        //                    Unique5, Kredi5, KrediTarihi5, Ako5);

        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    Status = SelectFile(EFDataName1);
        //    UpdateBinary(241, new byte[] { YanginModuSuresi }); //Status = UpdateBinaryOzel(241, YanginModuSuresi);//
        //    if (!Status)
        //    {
        //        FinishKart();
        //        return HataSet(14);
        //    }

        //    FinishKart();

        //    return "1";

        //}

        public string AboneYaz(uint devNo, uint AnaKredi, byte Cap, byte Donem,
            uint Fiyat1, uint Fiyat2, uint Fiyat3, uint Fiyat4, uint Fiyat5,
            uint Limit1, uint Limit2, uint Limit3, uint Limit4,
            byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree, byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree, byte AvansOnayi, byte IleriSaat, byte YanginModuSuresi, byte MaxDebiSiniri, UInt32 KritikKredi)
        {

            string durum = CapLimitFiyatKontrol(Fiyat1, Fiyat2, Fiyat3, Fiyat4, Fiyat5, Limit1, Limit2, Limit3, Limit4, Cap);
            if (durum != "1")
            {
                FinishKart();
                return HataSet(Convert.ToInt32(durum));
            }


            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            bool Status;

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                FinishKart();
                return HataSet(29);
            }
            if (devNo != Issuer.CihazNo)
            {
                FinishKart();
                return HataSet(12);
            }

            Integer4Byte CihazNoo = new Integer4Byte(devNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);
            Status = VerifyPin(0X83, PinAbone);
            if (!Status)
            {
                FinishKart();
                return HataSet(8);
            }

            Status = SelectFile(EFDataName1);
            if (!Status)
            {
                FinishKart();
                return HataSet(6);
            }


            #region iade Kartı Kontrol
            Abone abo = new Abone(ReadBinary(0, 255));

            byte IadeYapilmis = 0;
            switch (abo.Iade)
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

            if (IadeYapilmis == 1)
            {
                FinishKart();
                return HataSet(32);
            }

            #endregion



            AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));

            if (Cap != Degerler.Cap)
            {
                FinishKart();
                return HataSet(13);
            }

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
                case 125:
                case 150:
                case 200:
                case 250:
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
            string split = "-";
            UInt16 Bayram1, Bayram2;
            Bayram1 = Bayram1Suree;
            Bayram1 <<= 5;
            Bayram1 |= Bayram1Gunn;
            Bayram1 <<= 4;
            Bayram1 |= Bayram1Ayy;

            Bayram2 = Bayram2Suree;
            Bayram2 <<= 5;
            Bayram2 |= Bayram2Gunn;
            Bayram2 <<= 4;
            Bayram2 |= Bayram2Ayy;

            if ((Degerler.AkoYko & 128) == 0)
            {
                Degerler.IslemNo++;
            }
            if ((Degerler.AkoYko & 64) == 0)
            {
                Degerler.IslemNo++;
            }

            Abone024051 Adres024051 = new Abone024051(ReadBinary(24, 28));
            Abone120143 Adres120143 = new Abone120143(ReadBinary(120, 24));

            Status = SelectFile(EFDataName2);
            Abone000048 Adres000048 = new Abone000048(ReadBinary(0, 48));

            if (Adres000048.ako0 == 192)
            {
                AnaKredi = AnaKredi;// + Adres024051.AnaKredi;
            }
            else
            {
                Adres000048.unique0 = Convert.ToUInt16(Adres000048.unique0 + 1);
            }
            Status = SelectFile(EFDataName1);
            // sonra kaldırılacak çünkü satış biriminden gelmesi gerekiyor.

            YazilacakDuzenle0023(devNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            if (IleriSaat == 1) IleriSaat = 0;
            else if (IleriSaat == 0) IleriSaat = 0X90;

            YazilacakDuzenle2447(AnaKredi, KritikKredi, 192, Degerler.IslemNo, Degerler.KartNo, Cap, Degerler.Tip, Donem, 25, 30, 0, MaxDebiSiniri, AvansOnayi, Degerler.AboneNo, IleriSaat);
            Status = UpdateBinary(24, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            YazilacakDuzenle4851(Bayram1, Bayram2);
            Status = UpdateBinary(48, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Abone072095 Adres072095 = new Abone072095(ReadBinary(72, 24));

            YazilacakDuzenle5271(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4, Adres072095.KalanKredi);
            Status = UpdateBinary(52, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }




            Status = YazilacakDuzenle242290(Adres000048.unique0, 192,
                            Adres000048.unique1, Adres000048.kredi1, Adres000048.krediTarihi1, Adres000048.ako1,
                            Adres000048.unique2, Adres000048.kredi2, Adres000048.krediTarihi2, Adres000048.ako2,
                            Adres000048.unique3, Adres000048.kredi3, Adres000048.krediTarihi3, Adres000048.ako3,
                            Adres000048.unique4, Adres000048.kredi4, Adres000048.krediTarihi4, Adres000048.ako4,
                            Adres000048.unique5, Adres000048.kredi5, Adres000048.krediTarihi5, Adres000048.ako5);

            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = SelectFile(EFDataName1);
            UpdateBinary(241, new byte[] { YanginModuSuresi }); //Status = UpdateBinaryOzel(241, YanginModuSuresi);//
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }


      
       

        public string KartBosaltKontrolsuz()
        {
            int init = InitKart();
            if (init == 0) { HataSet(1); return FinishKart(); }

            bool Status = SelectFile(DFName);
            if (!Status) { HataSet(2); return FinishKart(); }

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status) { HataSet(3); return FinishKart(); }

            Status = SelectFile(EFIssuerName);
            if (!Status) { HataSet(4); return FinishKart(); }

            byte[] a = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Status = UpdateBinary(0, a);
            if (!Status) { HataSet(4); return FinishKart(); }

            EFDataName1Eris("Yetki");

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) { HataSet(16); return FinishKart(); }
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status) { HataSet(16); return FinishKart(); }


            Status = SelectFile(EFDataName2);
            if (!Status) { HataSet(2); return FinishKart(); }

            for (int i = 0; i < 2; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status) { HataSet(2); return FinishKart(); };
            }

            FinishKart();

            return "1";
        }

        public string AboneBosalt()
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == GlobalAyarlar.GetInstance().Issuer + "A ")
            {
                UpdateIssuer("\0\0\0\0", 0);//"\0\0\0\0"

                EFDataName1Eris(Issuer.CihazNo);
            }
            else
            {
                FinishKart();
                return HataSet(7);
            }

            byte[] YazilacakDatalar24 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] YazilacakDatalar16 = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            bool Status;
            for (int i = 0; i < 10; i++)
            {
                Status = UpdateBinary(Convert.ToByte(i * 24), YazilacakDatalar24);
                if (!Status)
                {
                    FinishKart();
                    return HataSet(16);
                }
            }

            Status = UpdateBinary(240, YazilacakDatalar16);
            if (!Status)
            {
                FinishKart();
                return HataSet(16);
            }

            FinishKart();

            return "1";
        }

        public string KartTipi()
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            FinishKart();
            return TipTanimlama(Issuer.IssuerArea);
        }

        public string Eject()
        {
            return "1";
        }

        public string KrediOku()
        {
            byte[] PinAbone = new byte[8];

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            bool Status = SelectFile(DFName);
            if (!Status)
            {
                FinishKart();
                return HataSet(2);
            }

            Status = ExternalAuthenticate(0X84, GetChallenge(DKAbone));
            if (!Status)
            {
                FinishKart();
                return HataSet(3);
            }

            Status = SelectFile(EFIssuerName);
            if (!Status) { HataSet(4); return FinishKart(); }

            byte[] response = ReadBinary(0, 12);
            if (!Status)
            {
                FinishKart();
                return HataSet(5);
            }

            string IssuerArea = Hexcon.BytetoString(0, 4, response);
            if (IssuerArea != GlobalAyarlar.GetInstance().Issuer + "A ")
            {
                FinishKart();
                return HataSet(7);
            }

            UInt32 CihazNo = Hexcon.Byte4toUInt32(response[8], response[9], response[10], response[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] Gonderilecek = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort,
                                    Convert.ToByte(CihazNoo.bir^0x0F), Convert.ToByte(CihazNoo.iki^0x0F),
                                    Convert.ToByte(CihazNoo.uc^0x0F), Convert.ToByte(CihazNoo.dort^0x0F) };
            PinAbone = classDes.TripleDes(DKAbone, Gonderilecek, PinAbone);

            Status = VerifyPin(0X83, PinAbone);
            if (!Status) { HataSet(8); return FinishKart(); }

            Status = SelectFile(EFDataName1);
            if (!Status) { HataSet(6); return FinishKart(); }

            Abone000023 Adres000023 = new Abone000023(ReadBinary(0, 24));
            Abone024051 Adres024051 = new Abone024051(ReadBinary(24, 28));
            AkoYkoIslemNoKartNoCapTip Degerler = new AkoYkoIslemNoKartNoCapTip(ReadBinary(32, 20));
            Abone052071 Adres051071 = new Abone052071(ReadBinary(52, 20));
            Abone072095 Adres072095 = new Abone072095(ReadBinary(72, 24));
            Abone096119 Adres096119 = new Abone096119(ReadBinary(96, 24));
            Abone120143 Adres120143 = new Abone120143(ReadBinary(120, 24));
            Abone144171 Adres144171 = new Abone144171(ReadBinary(144, 28));

            Status = SelectFile(EFDataName2);
            Abone000048 Adres242290 = new Abone000048(ReadBinary(0, 48));
            Status = SelectFile(EFDataName1);

            //string AnaKrediOkunma = "";
            //if (((Adres024051.AkoYko & 128) >> 7) == 0) { AnaKrediOkunma = "*"; } else { AnaKrediOkunma = "b"; }
            //string YedekKrediOkunma = "";
            //if (((Adres024051.AkoYko & 64) >> 6) == 0) { YedekKrediOkunma = "*"; } else { YedekKrediOkunma = "b"; }

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
                //if (AnaKrediOkunma == "b")
                //{
                //    IadeKredi += Adres024051.AnaKredi;
                //}
                //if (YedekKrediOkunma == "b")
                //{
                //    IadeKredi += Adres024051.YedekKredi;
                //}
            }

            string AnaPilSeviyesi = String.Format("{0,8:0.000}", (Adres144171.AnaPil * 6 / 255.0));


            FinishKart();
            Integer2Byte krt1 = new Integer2Byte(Adres242290.krediTarihi1);
            Integer2Byte krt2 = new Integer2Byte(Adres242290.krediTarihi2);
            Integer2Byte krt3 = new Integer2Byte(Adres242290.krediTarihi3);
            Integer2Byte krt4 = new Integer2Byte(Adres242290.krediTarihi4);
            Integer2Byte krt5 = new Integer2Byte(Adres242290.krediTarihi5);
            var ako0 = Adres242290.ako0 == 192 ? "b" : Adres242290.ako0 == 67 ? "C" : "O";
            var ako1 = Adres242290.ako1 == 192 ? "b" : Adres242290.ako1 == 67 ? "C" : "O";
            var ako2 = Adres242290.ako2 == 192 ? "b" : Adres242290.ako2 == 67 ? "C" : "O";
            var ako3 = Adres242290.ako3 == 192 ? "b" : Adres242290.ako3 == 67 ? "C" : "O";
            var ako4 = Adres242290.ako4 == 192 ? "b" : Adres242290.ako4 == 67 ? "C" : "O";
            var ako5 = Adres242290.ako5 == 192 ? "b" : Adres242290.ako5 == 67 ? "C" : "O";
            #region dönüş değerleri

            string str = "";

            str += "1";
            str += sp + Degerler.AboneNo;
            str += sp + Adres024051.KartNo;
            str += sp + CihazNo;
            str += sp + Adres024051.AnaKredi;
            str += sp + ako0;
            str += sp + ako1;
            str += sp + ako2;
            str += sp + ako3;
            str += sp + ako4;
            str += sp + ako5;
            str += sp + Adres242290.kredi1;
            str += sp + Adres242290.kredi2;
            str += sp + Adres242290.kredi3;
            str += sp + Adres242290.kredi4;
            str += sp + Adres242290.kredi5;
            str += sp + Hexcon.TarihDuzenle(krt1.bir, krt1.iki);
            str += sp + Hexcon.TarihDuzenle(krt2.bir, krt2.iki);
            str += sp + Hexcon.TarihDuzenle(krt3.bir, krt3.iki);
            str += sp + Hexcon.TarihDuzenle(krt4.bir, krt4.iki);
            str += sp + Hexcon.TarihDuzenle(krt5.bir, krt5.iki);
            str += sp + Adres242290.unique0;
            str += sp + Adres242290.unique1;
            str += sp + Adres242290.unique2;
            str += sp + Adres242290.unique3;
            str += sp + Adres242290.unique4;
            str += sp + Adres242290.unique5;
            str += sp + SayacCeza;
            str += sp + "*";
            str += sp + Adres024051.Tip;
            str += sp + Adres024051.Cap;
            str += sp + Adres024051.DonemGun;
            str += sp + Adres000023.Limit1;
            str += sp + Adres000023.Limit2;
            str += sp + Adres051071.Limit3;
            str += sp + Adres051071.Limit4;
            str += sp + Adres072095.KalanKredi;
            str += sp + Adres072095.HarcananKredi;

            if (Degerler.KartNo > 15)
                str += sp + "*"; // yeni kart
            else
                str += sp + "b";
            str += sp + IadeYapilmis;

            //Integer2Byte krt1 = new Integer2Byte(Adres242290.krediTarihi1);
            //Integer2Byte krt2 = new Integer2Byte(Adres242290.krediTarihi2);
            //Integer2Byte krt3 = new Integer2Byte(Adres242290.krediTarihi3);
            //Integer2Byte krt4 = new Integer2Byte(Adres242290.krediTarihi4);
            //Integer2Byte krt5 = new Integer2Byte(Adres242290.krediTarihi5);



            return str;

            #endregion
        }

        public string Iade(UInt32 devNo)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == GlobalAyarlar.GetInstance().Issuer + "A ")
            {
                if (Issuer.CihazNo == devNo)
                {
                    EFDataName1Eris(devNo);
                }
                else
                {
                    FinishKart();
                    return HataSet(12);
                }
            }
            else
            {
                FinishKart();
                return HataSet(7);
            }

            byte[] OkunanDegerler = ReadBinary(40, 1);
            byte Iade = OkunanDegerler[0];

            if (Iade == 0)
            {
                bool Status = UpdateBinary(40, 0x99);
                if (!Status) { HataSet(15); return FinishKart(); }
            }

            FinishKart();

            return "1";

        }
        #endregion

        #region IAcr128
        public int InitKart()
        {

            try
            {
                mng = new SCResMgr();
                mng.EstablishContext(SCardContextScope.User);
                rd = new SCReader(mng);
                ArrayList al = new ArrayList();
                mng.ListReaders(al);
                foreach (string st in al)
                {
                    rdName = st;
                    if (!rd.IsConnected) rd.Connect(rdName, SCardAccessMode.Shared, SCardProtocolIdentifiers.CL);
                    else break;
                }



                rd.BeginTransaction();

                return Convert.ToInt32(rd.IsConnected);
            }
            catch (Exception ex)
            {
                return Convert.ToInt32(rd.IsConnected);
            }
        }

        public string FinishKart()
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

        public string HataSet(int Kod)
        {
            _hata = Kod + ":";
            switch (Kod)
            {
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
                    _hata += "Hatalı Parametre : Çapı değeri;15,20,25,40,50,65,80,100,125,150,200,250 olabilir";
                    break;
                case 29:
                    _hata += "Boş Kart";
                    break;
                case 30:
                    _hata += "Farklı Bölge Kartı";
                    break;
                case 31:
                    _hata += "Kart EFDataName2 hata";
                    break;
                case 32:
                    _hata += "İade Kartı: Abone Kartı tanımlayınız";
                    break;


                default:
                    _hata = "0:";
                    _hata += "BAŞARILI";
                    break;
            }

            return _hata;
        }

        #endregion


        #region Uretim Fonksiyonlari

        byte[] UretimYazilacak = new byte[15];
        byte[] UretimYazilacak5Byte = new byte[5];

        public string FormUret()
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);

            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                UpdateIssuer("ASUF", 0);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(11);
            }


            Uretim0023();
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Uretim2435();
            Status = UpdateBinary(24, UretimYazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }
        public string UretimFormat(UInt32 CihazNo,
                                UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                                UInt32 KritikKredi, byte DonemGun, byte VanaPulseSure, byte VanaCntSure, byte Cap, byte VanaBekleme, byte Dil, byte Sensor, byte NoktaHane, byte Carpan, byte LcdType, byte BaglantiPeriyot)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUF", CihazNo);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);

            }

            if (Cap == 20)
            {
                KademeKatsayi1 *= 10;
                KademeKatsayi2 *= 10;
                KademeKatsayi3 *= 10;
                KademeKatsayi4 *= 10;
                KademeKatsayi5 *= 10;
            }

            Uretim0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Uretim2435(KritikKredi, DonemGun, VanaPulseSure, VanaCntSure);
            Status = UpdateBinary(24, UretimYazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);

            }

            Status = UpdateBinary(36, Cap);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Uretim3652(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4);
            Status = UpdateBinary(37, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(53, VanaBekleme);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(54, Dil);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(55, Sensor);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(56, NoktaHane);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(57, Carpan);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            Status = UpdateBinary(58, LcdType);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            #region BaglantiPeriyot
            Status = UpdateBinary(59, new byte[] { BaglantiPeriyot });
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            #endregion


            FinishKart();

            return "1";
        }

        public string UretimCihazNo(UInt32 CihazNo,
                                  UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                  UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                                  byte DonemGun, UInt32 MekanikTuketim, byte Cap, byte VanaBekleme, byte Dil, byte Sensor, byte NoktaHane, byte Carpan, string APN, string ServerIP, string Port)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUZ", CihazNo);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);
            }

            Uretim0023(CihazNo, KademeKatsayi1, KademeKatsayi2, KademeKatsayi3, Limit1, Limit2);
            bool Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            string Iss = GlobalAyarlar.GetInstance().Issuer;

            if ((Iss == "XX") && (Cap == 20))
            {
                Iss = "XW";
            }

            byte[] issuerDizi = new byte[2];
            issuerDizi[0] = (byte)Convert.ToChar(Iss.Substring(0, 1));
            issuerDizi[1] = (byte)Convert.ToChar(Iss.Substring(1, 1));

            UretimCihazNo2435(DonemGun, MekanikTuketim, Cap, issuerDizi[0], issuerDizi[1]);
            Status = UpdateBinary(24, UretimYazilacak);//24
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            UretimCihazNo3652(KademeKatsayi4, KademeKatsayi5, Limit3, Limit4);
            Status = UpdateBinary(38, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            Status = UpdateBinary(54, VanaBekleme);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            Status = UpdateBinary(55, Dil);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            Status = UpdateBinary(56, Sensor);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            Status = UpdateBinary(57, NoktaHane);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            Status = UpdateBinary(58, Carpan);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }


            int i;
            for (i = 0; i < APN.Length; i++)
            {
                Yazilacak[i] = (byte)APN[i];
            }

            Yazilacak[i] = 0x00;

            Status = UpdateBinary(59, Yazilacak);//16 byte
            if (!Status) { HataSet(14); return FinishKart(); }

            for (i = 0; i < ServerIP.Length; i++)
            {
                Yazilacak[i] = (byte)ServerIP[i];
            }
            Yazilacak[i] = 0x00;
            Status = UpdateBinary(75, Yazilacak); //16 byte
            if (!Status) { HataSet(14); return FinishKart(); }

            for (i = 0; i < Port.Length; i++)
            {
                Yazilacak[i] = (byte)Port[i];
            }
            Yazilacak[i] = 0x00;
            Status = UpdateBinary(91, Yazilacak); //6 byte
            if (!Status) { HataSet(14); return FinishKart(); }


            FinishKart();

            return "1";
        }

        public string UretimAc(byte KontrolDegeri)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUA", 0);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);
            }

            bool Status = UpdateBinary(0, KontrolDegeri);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }

        public string UretimKapat(byte KontrolDegeri)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUK", 0);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);
            }

            bool Status = UpdateBinary(0, KontrolDegeri);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }

        public string UretimTestMod(uint devNo)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUT", devNo);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);

            }

            Uretim0005(devNo, 0xFF);
            bool Status = UpdateBinary(0, UretimYazilacak5Byte);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }

        public string UretimLcd(byte LcdType)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == "ASU")
            {
                UpdateIssuer("ASUL", 0);
                EFDataName1Eris("Uretim");
            }
            else
            {
                FinishKart();
                return HataSet(23);

            }

            bool Status = UpdateBinary(0, LcdType);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();

            return "1";
        }
        public void Uretim0023()
        {
            byte[] Bosalt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Yazilacak = Bosalt;
        }

        public void Uretim2435()
        {
            byte[] Bosalt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            UretimYazilacak = Bosalt;
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

        private void Uretim0023(UInt32 CihazNo, UInt32 Katsayi1, UInt32 Katsayi2, UInt32 Katsayi3, UInt32 Limit1, UInt32 Limit2)
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
        private void Uretim2435(UInt32 KritikKredi, byte DonemGun, byte VanaPulseSure, byte VanaCntSure)
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

        private void Uretim3652(UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4)
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

        private void UretimCihazNo2435(byte DonemGun, UInt32 Mek_Tuk, byte SayacCapi, byte Issue1, byte Issue2)
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

        private void UretimCihazNo3652(UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4)
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
        #endregion



        #region Yetki İslemleri
        public string YetkiHazirla()
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            bool Status;

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea == "\0\0\0\0")
            {
                UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "YA", 0);
                EFDataName1Eris("Yetki");
            }
            else
            {
                FinishKart();
                return HataSet(11);
            }

            Uretim0023();
            Status = UpdateBinary(0, Yazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();
            return "1";
        }

        public string YetkiAcma(uint devNo, byte KontrolDegeri)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == GlobalAyarlar.GetInstance().Issuer + "Y")
            {
                UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "YA", 0);
                EFDataName1Eris("Yetki");
            }
            else
            {
                FinishKart();
                return HataSet(18);
            }


            Yetki0007(devNo, KontrolDegeri, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();
            return "1";
        }

        public string YetkiKapama(uint devNo, string Tarih, byte KontrolDegeri, byte KapatmaEmri)
        {

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == GlobalAyarlar.GetInstance().Issuer + "Y")
            {
                UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "YK", 0);
                EFDataName1Eris("Yetki");
            }
            else
            {
                FinishKart();
                return HataSet(18);
            }

            Yetki0007(devNo, Tarih, KontrolDegeri, KapatmaEmri);

            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();
            return "1";

        }

        public string YetkiIptal(UInt32 devNo)
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == GlobalAyarlar.GetInstance().Issuer + "Y")
            {
                UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "YC", 0);
                EFDataName1Eris("Yetki");
            }
            else
            {
                FinishKart();
                return HataSet(18);
            }

            Yetki0007(devNo, 1);
            bool Status = UpdateBinary(0, YetkiYazilacak);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            FinishKart();
            return "1";
        }

        public string BilgiYap()
        {
            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea.Substring(0, 3) == GlobalAyarlar.GetInstance().Issuer + "Y")
            {
                UpdateIssuer(GlobalAyarlar.GetInstance().Issuer + "YE", 0);
                EFDataName1Eris("Yetki");
            }
            else
            {
                FinishKart();
                return HataSet(18);
            }

            byte[] yb = { 0, 1 };
            bool Status = UpdateBinary(0, yb);
            if (!Status)
            {
                FinishKart();
                return HataSet(14);
            }

            /*Status = UpdateBinary(213, 0);
            if (!Status) return Return(14);*/

            FinishKart();
            return "1";
        }

        public string BilgiOku()
        {
            byte[] PartOku = new byte[33];
            byte[] KartOku = new byte[256];

            int init = InitKart();
            if (init == 0)
            {
                FinishKart();
                return HataSet(1);
            }

            bool Status;

            IssuerAreaa Issuer = new IssuerAreaa(EFDataName1DosyasinaErisimAyarlari());

            if (Issuer.IssuerArea != GlobalAyarlar.GetInstance().Issuer + "YE")
            {
                FinishKart();
                return HataSet(19);
            }

            Status = VerifyPin(0x82, PinYetki);
            if (!Status)
            {
                FinishKart();
                return HataSet(20);
            }

            Status = SelectFile(EFDataName1);
            if (!Status)
            {
                FinishKart();
                return HataSet(6);
            }

            //Verileri Oku..
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    PartOku = ReadBinary(Convert.ToByte(i * 32), 32);
                    Array.Copy(PartOku, 0, KartOku, i * 32, 32);
                }
            }
            catch (Exception ex)
            {
                return FinishKart();
            }

            FinishKart();

            YetkiKarti BilgiOku = new YetkiKarti(KartOku);

            if (BilgiOku.BilgiTipi != 0x05)
            {
                FinishKart();
                return HataSet(21);
            }

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
            str += sp + BilgiOku.DonemGun;
            str += sp + BilgiOku.DonemGunNo;
            str += sp + BilgiOku.AnaPil;
            str += sp + BilgiOku.Vop;
            str += sp + BilgiOku.SayacTarihi;
            str += sp + BilgiOku.SonKrediTarihi;
            str += sp + BilgiOku.SonPulseTarihi;
            str += sp + BilgiOku.SonCezaTarihi;
            str += sp + BilgiOku.SonArizaTarihi;
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
            str += sp + BilgiOku.DonemTuketim1;
            str += sp + BilgiOku.DonemTuketim2;
            str += sp + BilgiOku.DonemTuketim3;
            str += sp + BilgiOku.DonemTuketim4;
            str += sp + BilgiOku.DonemTuketim5;
            str += sp + BilgiOku.DonemTuketim6;

            str += sp + BilgiOku.ToplamYuklenenKredi;
            str += sp + BilgiOku.YazilimVersiyon;
            str += sp + BilgiOku.IlkPulseTarihi;
            str += sp + BilgiOku.Katsayi4;
            str += sp + BilgiOku.Katsayi5;
            str += sp + BilgiOku.Limit3;
            str += sp + BilgiOku.Limit4;
            str += sp + BilgiOku.Kademe4Tuketimi;
            str += sp + BilgiOku.Kademe5Tuketimi;

            str += sp + BilgiOku.Katsayi1;
            str += sp + BilgiOku.Katsayi2;
            str += sp + BilgiOku.Katsayi3;
            str += sp + BilgiOku.IslemNo;


            return str;
        }
        #endregion

        private string[] EFDataName1DosyasinaErisimAyarlari()
        {
            string[] DonusDegeri = { "0", "0", "0" };

            bool Status = SelectFile(DFName);
            if (!Status)
            {
                HataSet(2);
                DonusDegeri[0] = "0";
                return DonusDegeri;
            }

            Status = ExternalAuthenticate(0x84, GetChallenge(DKAbone));
            if (!Status)
            {
                HataSet(3);
                DonusDegeri[0] = "0";
                return DonusDegeri;
            }

            Status = SelectFile(EFIssuerName);
            if (!Status)
            {
                HataSet(4);
                DonusDegeri[0] = "0";
                return DonusDegeri;
            }

            byte[] OkunanDegerler = ReadBinary(0, 12);

            string IssuerArea = Hexcon.BytetoString(0, 4, OkunanDegerler);
            DonusDegeri[1] = IssuerArea;

            UInt32 CihazNo = Hexcon.Byte4toUInt32(OkunanDegerler[8], OkunanDegerler[9], OkunanDegerler[10], OkunanDegerler[11]);
            DonusDegeri[2] = CihazNo.ToString();

            DonusDegeri[0] = "1";

            return DonusDegeri;
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

        private bool SelectFile(string File)
        {
            //string ss = GetSerial();

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

        }

        public void YazilacakDuzenle0023(UInt32 CihazNo, UInt32 Katsayi1, UInt32 Katsayi2, UInt32 Katsayi3, UInt32 Limit1, UInt32 Limit2)
        {
            YazilacakBosalt();

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

        void YazilacakBosalt()
        {
            byte[] Bosalt = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Array.Copy(Bosalt, Yazilacak, 24);
        }

        public void YazilacakDuzenle2447(UInt32 AnaKredi, UInt32 YedekKredi,
                                        byte AkoYko, byte IslemNo, byte KartNo, byte Cap,
                                        byte Tip, byte DonemGun, byte VanaPulseSure, byte VanaCntSure,
                                        byte Iade, byte MaxdebiSiniri, byte HaftaSonuOnay,
                                        UInt32 AboneNo, byte IleriSaat)
        {
            YazilacakBosalt();

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
            Yazilacak[17] = MaxdebiSiniri;
            Yazilacak[18] = HaftaSonuOnay;

            Yazilacak[19] = IleriSaat;

            Yazilacak[20] = AboneNoo.bir;
            Yazilacak[21] = AboneNoo.iki;
            Yazilacak[22] = AboneNoo.uc;
            Yazilacak[23] = AboneNoo.dort;
        }

        public void YazilacakDuzenle4851(UInt16 Bayram1Tarih, UInt16 Bayram2Tarih)
        {
            YazilacakBosalt();

            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1Tarih);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2Tarih);

            Yazilacak[0] = Bayram1Deger.bir;
            Yazilacak[1] = Bayram1Deger.iki;
            Yazilacak[2] = Bayram2Deger.bir;
            Yazilacak[3] = Bayram2Deger.iki;
        }

        public void YazilacakDuzenle5271(UInt32 Katsayi4, UInt32 Katsayi5, UInt32 Limit3, UInt32 Limit4, Int32 KalanKredi)
        {
            YazilacakBosalt();

            Integer4Byte Kat4 = new Integer4Byte(Katsayi4);
            Integer4Byte Kat5 = new Integer4Byte(Katsayi5);
            Integer4Byte Lim3 = new Integer4Byte(Limit3);
            Integer4Byte Lim4 = new Integer4Byte(Limit4);
            Integer4Byte Kal = new Integer4Byte(KalanKredi);


            Yazilacak[0] = 0;
            Yazilacak[1] = 0;
            Yazilacak[2] = 0X55; // KADEME Byteları 5 kademe
            Yazilacak[3] = 0XED;

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

            Yazilacak[20] = Kal.bir;
            Yazilacak[21] = Kal.iki;
            Yazilacak[22] = Kal.uc;
            Yazilacak[23] = Kal.dort;
        }

        public bool YazilacakDuzenle242290(UInt16 Unique0, byte Ako0,
                          UInt16 Unique1, Int32 Kredi1, UInt16 KrediTarihi1, byte Ako1,
                          UInt16 Unique2, Int32 Kredi2, UInt16 KrediTarihi2, byte Ako2,
                          UInt16 Unique3, Int32 Kredi3, UInt16 KrediTarihi3, byte Ako3,
                          UInt16 Unique4, Int32 Kredi4, UInt16 KrediTarihi4, byte Ako4,
                          UInt16 Unique5, Int32 Kredi5, UInt16 KrediTarihi5, byte Ako5)
        {
            bool Status = false;

            Integer2Byte un0 = new Integer2Byte(Unique0);
            Integer2Byte un1 = new Integer2Byte(Unique1);
            Integer2Byte un2 = new Integer2Byte(Unique2);
            Integer2Byte un3 = new Integer2Byte(Unique3);
            Integer2Byte un4 = new Integer2Byte(Unique4);
            Integer2Byte un5 = new Integer2Byte(Unique5);

            Integer4Byte kr1 = new Integer4Byte(Kredi1);
            Integer4Byte kr2 = new Integer4Byte(Kredi2);
            Integer4Byte kr3 = new Integer4Byte(Kredi3);
            Integer4Byte kr4 = new Integer4Byte(Kredi4);
            Integer4Byte kr5 = new Integer4Byte(Kredi5);

            Integer2Byte krt1 = new Integer2Byte(KrediTarihi1);
            Integer2Byte krt2 = new Integer2Byte(KrediTarihi2);
            Integer2Byte krt3 = new Integer2Byte(KrediTarihi3);
            Integer2Byte krt4 = new Integer2Byte(KrediTarihi4);
            Integer2Byte krt5 = new Integer2Byte(KrediTarihi5);

            YazilacakBosalt();

            Yazilacak[0] = un0.bir;
            Yazilacak[1] = un0.iki;
            Yazilacak[2] = Ako0;// 0XC0;
            Yazilacak[3] = un1.bir;
            Yazilacak[4] = un1.iki;
            Yazilacak[5] = kr1.bir;
            Yazilacak[6] = kr1.iki;
            Yazilacak[7] = kr1.uc;
            Yazilacak[8] = kr1.dort;
            Yazilacak[9] = krt1.bir;
            Yazilacak[10] = krt1.iki;
            Yazilacak[11] = Ako1;
            Yazilacak[12] = un2.bir;
            Yazilacak[13] = un2.iki;
            Yazilacak[14] = kr2.bir;
            Yazilacak[15] = kr2.iki;
            Yazilacak[16] = kr2.uc;
            Yazilacak[17] = kr2.dort;
            Yazilacak[18] = krt2.bir;
            Yazilacak[19] = krt2.iki;
            Yazilacak[20] = Ako2;
            Yazilacak[21] = un3.bir;
            Yazilacak[22] = un3.iki;
            Yazilacak[23] = kr3.bir;

            Status = SelectFile(EFDataName2);
            Status = UpdateBinary(0, Yazilacak); // unique ve kredi log
            Status = SelectFile(EFDataName1);

            if (!Status)
                return Status;

            YazilacakBosalt();

            Yazilacak[0] = kr3.iki;
            Yazilacak[1] = kr3.uc;
            Yazilacak[2] = kr3.dort;
            Yazilacak[3] = krt3.bir;
            Yazilacak[4] = krt3.iki;
            Yazilacak[5] = Ako3;
            Yazilacak[6] = un4.bir;
            Yazilacak[7] = un4.iki;
            Yazilacak[8] = kr4.bir;
            Yazilacak[9] = kr4.iki;
            Yazilacak[10] = kr4.uc;
            Yazilacak[11] = kr4.dort;
            Yazilacak[12] = krt4.bir;
            Yazilacak[13] = krt4.iki;
            Yazilacak[14] = Ako4;
            Yazilacak[15] = un5.bir;
            Yazilacak[16] = un5.iki;
            Yazilacak[17] = kr5.bir;
            Yazilacak[18] = kr5.iki;
            Yazilacak[19] = kr5.uc;
            Yazilacak[20] = kr5.dort;
            Yazilacak[21] = krt5.bir;
            Yazilacak[22] = krt5.iki;
            Yazilacak[23] = Ako5;

            Status = SelectFile(EFDataName2);
            Status = UpdateBinary(24, Yazilacak); // unique ve kredi log
            Status = SelectFile(EFDataName1);

            return Status;
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
            if (!Status) { HataSet(8); return false; }

            Status = SelectFile(EFDataName1);
            if (!Status) { HataSet(6); return false; }

            return true;
        }
        private bool EFDataName1Eris(string KartTipi)
        {
            bool Status = true;

            switch (KartTipi)
            {
                case "Uretim":
                    Status = VerifyPin(0x81, PinUretim);
                    if (!Status) { HataSet(24); return false; }
                    break;
                case "Yetki":
                    Status = VerifyPin(0x82, PinYetki);
                    if (!Status) { HataSet(20); return false; }
                    break;
            }

            Status = SelectFile(EFDataName1);
            if (!Status) { HataSet(6); return false; }

            return true;
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

        }


        public string TipTanimlama(string IssuerArea)
        {
            string stIssuer = GlobalAyarlar.GetInstance().Issuer;

            if (stIssuer + "A " == IssuerArea)
            {
                IssuerArea = "Abone Karti";
                return IssuerArea;
            }
            if (stIssuer + "YA" == IssuerArea)
            {
                IssuerArea = "Yetki Açma Karti";
                return IssuerArea;
            }
            if (stIssuer + "YE" == IssuerArea)
            {
                IssuerArea = "Yetki Bilgi Karti";
                return IssuerArea;
            }
            if (stIssuer + "YC" == IssuerArea)
            {
                IssuerArea = "Yetki İptal Karti";
                return IssuerArea;
            }
            if (stIssuer + "YK" == IssuerArea)
            {
                IssuerArea = "Yetki Kapama";
                return IssuerArea;
            }
            if (stIssuer + "YR" == IssuerArea)
            {
                IssuerArea = "Yetki Reset";
                return IssuerArea;
            }
            if (stIssuer + "YT" == IssuerArea)
            {
                IssuerArea = "Yetki Tüketim";
                return IssuerArea;
            }
            if (stIssuer + "YS" == IssuerArea)
            {
                IssuerArea = "Yetki Saat";
                return IssuerArea;
            }
            if (stIssuer + "Y4" == IssuerArea)
            {
                IssuerArea = "Yetki Ceza4";
                return IssuerArea;
            }
            if (stIssuer + "YV" == IssuerArea)
            {
                IssuerArea = "Yetki Avans";
                return IssuerArea;
            }

            switch (IssuerArea)
            {

                case "KSA ":
                    IssuerArea = "Abone Karti";
                    break;
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
                case "ASUI":
                    IssuerArea = "Zone Kartı";
                    break;
                case "BTLD":
                    IssuerArea = "BootLoader";
                    break;
                case "\0\0\0\0":
                    IssuerArea = "Bos Kart";
                    break;
            }
            return IssuerArea;

        }

        public string CapLimitFiyatKontrol(uint Fiyat1, uint Fiyat2, uint Fiyat3, uint Fiyat4, uint Fiyat5,
                               uint Limit1, uint Limit2, uint Limit3, uint Limit4, byte Cap)
        {


            if (Fiyat1 <= 0 || Fiyat2 <= 0 || Fiyat3 <= 0 || Fiyat4 <= 0 || Fiyat5 <= 0)
            {
                return "25";

            }
            if (Limit1 <= 0 || Limit2 <= 0 || Limit3 <= 0 || Limit4 <= 0)
            {
                return "26";
            }
            if (Cap <= 0)
            {
                return "27";
            }
            else
            {
                if (Cap == 15 || Cap == 20 || Cap == 25 || Cap == 40 || Cap == 50 || Cap == 65 || Cap == 80 || Cap == 100 || Cap == 125 || Cap == 150 || Cap == 200 || Cap == 250)
                {
                }
                else
                {
                    return "28";
                }
            }

            return "1";

        }

        public string Versiyon()
        {
            return "Versiyon: 2.2";
        }



        #region Yetki Fonksiyonlari
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

        public void Yetki0007(UInt32 CihazNo, string Tarih, params byte[] Veriler)
        {
            byte[] Temp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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

        #endregion

    }
}
