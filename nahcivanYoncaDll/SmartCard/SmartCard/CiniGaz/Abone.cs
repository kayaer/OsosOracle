using SmartCard.CiniGaz.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.CiniGaz
{
    class Abone
    {
        CardProcess cp;

        private byte[] DKAbone = { 0x12, 0x53, 0x83, 0x26, 0x26, 0x61, 0x15, 0x98, 0x42, 0x11, 0x45, 0x76, 0x13, 0x62, 0x77, 0x19 };
        private byte[] PinAbone = { 0x01, 0x01, 0x48, 0x29, 0x53, 0x72, 0x49, 0x06 };
        private string DFName = "NN";
        private string EFIssuerName = "EI";
        private string EFDataName1 = "E1";
        private string EFDataName2 = "E2";
        private string EFDataName3 = "E3";
        private string EFDataName4 = "E4";

        public Abone(CardProcess cp)
        {
            this.cp = cp;
        }

        #region CONTACTLESS
        public string AboneOku_CL()
        {
            string result = "";
            int r = 0;

            r = cp.SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.ExternalAuthenticate(0x84, cp.GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            r = cp.ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            if (Converter.BytetoString(0, 4, cp.inBuf) != cp.zone + "A ")
                return ((int)Enums.ResultSC.ABONE_KARTI_DEGIL).ToString();

            result += "ISSUER(HX)|" + cp.inBuf[0] + " " + cp.inBuf[1] + " " + cp.inBuf[2] + " " + cp.inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)cp.inBuf[0] + " " + (Char)cp.inBuf[1] + " " + (Char)cp.inBuf[2] + " " + (Char)cp.inBuf[3] + "\n";

            UInt32 CihazNo = Converter.Byte4toUInt32(cp.inBuf[8], cp.inBuf[9], cp.inBuf[10], cp.inBuf[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] sendData = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort, Convert.ToByte(CihazNoo.bir ^ 0x0F), Convert.ToByte(CihazNoo.iki ^ 0x0F), Convert.ToByte(CihazNoo.uc ^ 0x0F), Convert.ToByte(CihazNoo.dort ^ 0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, sendData, PinAbone);

            r = cp.VerifyCard(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.SelectBlock(EFDataName2);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.ReadCard(0, 230);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            Map.Abone abone = new Map.Abone(cp.inBuf);

            foreach (System.Reflection.FieldInfo item in abone.GetType().GetFields())
            {
                result += item.Name.ToString() + "|" + item.GetValue(abone).ToString() + "\n";
            }

            return result;
        }

        public int AboneYap_CL(UInt32 devNo, UInt32 AboneNo, byte KartNo,
                            byte Cap, byte Tip, byte Donem,
                            UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3,
                            UInt32 Limit1, UInt32 Limit2,
                            byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                            byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                            byte AvansOnayi)
        {
            int r = 0;

            UInt16 Bayram1, Bayram2;
            UInt32 KademeKatsayi1, KademeKatsayi2, KademeKatsayi3;

            KademeKatsayi1 = KademeKatsayi2 = KademeKatsayi3 = 0;

            r = cp.SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.ExternalAuthenticate(0x84, cp.GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            r = cp.ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (Converter.BytetoString(0, 4, cp.inBuf) != "\0\0\0\0")
                return (int)Enums.ResultSC.BOS_KART_DEGIL;

            r = cp.UpdateIssuer(cp.zone + "A ", devNo);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            byte[] PinAbone = new byte[8];

            Integer4Byte cihazNo = new Integer4Byte(devNo);
            byte[] sendData = { cihazNo.bir, cihazNo.iki, cihazNo.uc, cihazNo.dort,
                                    Convert.ToByte(cihazNo.bir^0x0F), Convert.ToByte(cihazNo.iki^0x0F),
                                    Convert.ToByte(cihazNo.uc^0x0F), Convert.ToByte(cihazNo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, sendData, PinAbone);

            r = cp.WriteKey(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.VerifyCard(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFDataName2);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            switch (Cap)
            {
                case 20:
                    Integer4Byte Katsayi2 = new Integer4Byte(Convert.ToUInt32(1000));
                    Integer4Byte Katsayi1 = new Integer4Byte(Convert.ToUInt32(Fiyat1 * 1000 / Fiyat2));
                    Integer4Byte Katsayi3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 1000 / Fiyat2));
                    KademeKatsayi1 = Katsayi1.value;
                    KademeKatsayi2 = Katsayi2.value;
                    KademeKatsayi3 = Katsayi3.value;
                    break;
                case 40:
                case 50:
                case 80:
                case 100:
                case 150:
                case 200:
                    Integer4Byte Katsayi_2 = new Integer4Byte(Convert.ToUInt32(10000));
                    Integer4Byte Katsayi_1 = new Integer4Byte(Convert.ToUInt32(Fiyat1 * 10000 / Fiyat2));
                    Integer4Byte Katsayi_3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 10000 / Fiyat2));
                    KademeKatsayi1 = Katsayi_1.value;
                    KademeKatsayi2 = Katsayi_2.value;
                    KademeKatsayi3 = Katsayi_3.value;
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

            Integer4Byte Cihaz = new Integer4Byte(devNo);
            Integer4Byte Kat1 = new Integer4Byte(KademeKatsayi1);
            Integer4Byte Kat2 = new Integer4Byte(KademeKatsayi2);
            Integer4Byte Kat3 = new Integer4Byte(KademeKatsayi3);
            Integer4Byte Lim1 = new Integer4Byte(Limit1);
            Integer4Byte Lim2 = new Integer4Byte(Limit2);
            Integer4Byte AnaKredii = new Integer4Byte(0);
            Integer4Byte YedekKredii = new Integer4Byte(0);
            Integer4Byte AboneNoo = new Integer4Byte(AboneNo);
            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            cp.outBuf[0] = Cihaz.bir;
            cp.outBuf[1] = Cihaz.iki;
            cp.outBuf[2] = Cihaz.uc;
            cp.outBuf[3] = Cihaz.dort;
            cp.outBuf[4] = Kat1.bir;
            cp.outBuf[5] = Kat1.iki;
            cp.outBuf[6] = Kat1.uc;
            cp.outBuf[7] = Kat1.dort;
            cp.outBuf[8] = Kat2.bir;
            cp.outBuf[9] = Kat2.iki;
            cp.outBuf[10] = Kat2.uc;
            cp.outBuf[11] = Kat2.dort;
            cp.outBuf[12] = Kat3.bir;
            cp.outBuf[13] = Kat3.iki;
            cp.outBuf[14] = Kat3.uc;
            cp.outBuf[15] = Kat3.dort;
            cp.outBuf[16] = Lim1.bir;
            cp.outBuf[17] = Lim1.iki;
            cp.outBuf[18] = Lim1.uc;
            cp.outBuf[19] = Lim1.dort;
            cp.outBuf[20] = Lim2.bir;
            cp.outBuf[21] = Lim2.iki;
            cp.outBuf[22] = Lim2.uc;
            cp.outBuf[23] = Lim2.dort;
            cp.outBuf[24] = AnaKredii.bir;
            cp.outBuf[25] = AnaKredii.iki;
            cp.outBuf[26] = AnaKredii.uc;
            cp.outBuf[27] = AnaKredii.dort;
            cp.outBuf[28] = YedekKredii.bir;
            cp.outBuf[29] = YedekKredii.iki;
            cp.outBuf[30] = YedekKredii.uc;
            cp.outBuf[31] = YedekKredii.dort;
            cp.outBuf[32] = 0XC0;
            cp.outBuf[33] = 0;
            cp.outBuf[34] = KartNo;
            cp.outBuf[35] = Cap;
            cp.outBuf[36] = Tip;
            cp.outBuf[37] = Donem;
            cp.outBuf[38] = 25;
            cp.outBuf[39] = 30;
            cp.outBuf[40] = 0;
            cp.outBuf[41] = 0;
            cp.outBuf[42] = AvansOnayi;
            cp.outBuf[43] = 0;
            cp.outBuf[44] = AboneNoo.bir;
            cp.outBuf[45] = AboneNoo.iki;
            cp.outBuf[46] = AboneNoo.uc;
            cp.outBuf[47] = AboneNoo.dort;
            cp.outBuf[48] = Bayram1Deger.bir;
            cp.outBuf[49] = Bayram1Deger.iki;
            cp.outBuf[50] = Bayram2Deger.bir;
            cp.outBuf[51] = Bayram2Deger.iki;
            cp.refLength = 52;

            r = cp.UpdateCard(0);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            return r;
        }

        public string KrediOku_CL()
        {
            string result = "";
            int r = 0;

            r = cp.SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.ExternalAuthenticate(0x84, cp.GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            r = cp.ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            if (Converter.BytetoString(0, 4, cp.inBuf) != cp.zone + "A ")
                return ((int)Enums.ResultSC.ABONE_KARTI_DEGIL).ToString();

            result += "ISSUER(HX)|" + cp.inBuf[0] + " " + cp.inBuf[1] + " " + cp.inBuf[2] + " " + cp.inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)cp.inBuf[0] + " " + (Char)cp.inBuf[1] + " " + (Char)cp.inBuf[2] + " " + (Char)cp.inBuf[3] + "\n";

            UInt32 CihazNo = Converter.Byte4toUInt32(cp.inBuf[8], cp.inBuf[9], cp.inBuf[10], cp.inBuf[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] sendData = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort, Convert.ToByte(CihazNoo.bir ^ 0x0F), Convert.ToByte(CihazNoo.iki ^ 0x0F), Convert.ToByte(CihazNoo.uc ^ 0x0F), Convert.ToByte(CihazNoo.dort ^ 0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, sendData, PinAbone);

            r = cp.VerifyCard(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.SelectBlock(EFDataName2);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
            r = cp.ReadCard(0, 230);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            Map.Abone abone = new Map.Abone(cp.inBuf);

            result += nameof(abone.AboneNo) + "|" + abone.AboneNo + "\n";
            result += nameof(abone.KartNo) + "|" + abone.KartNo + "\n";
            result += nameof(abone.CihazNo) + "|" + abone.CihazNo + "\n";
            result += nameof(abone.AnaKredi) + "|" + abone.AnaKredi + "\n";
            result += nameof(abone.YedekKredi) + "|" + abone.YedekKredi + "\n";
            result += nameof(abone.AnaKrediDurum) + "|" + abone.AnaKrediDurum + "\n";
            result += nameof(abone.YedekKrediDurum) + "|" + abone.YedekKrediDurum + "\n";
            result += nameof(abone.Tip) + "|" + abone.Tip + "\n";
            result += nameof(abone.Cap) + "|" + abone.Cap + "\n";
            result += nameof(abone.KalanKredi) + "|" + abone.KalanKredi + "\n";
            result += nameof(abone.HarcananKredi) + "|" + abone.HarcananKredi + "\n";

            return result;
        }

        public int KrediYaz_CL(UInt32 devNo, Int32 AnaKredi, Int32 YedekKredi,
                              byte Cap, byte Donem,
                              UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3,
                              UInt32 Limit1, UInt32 Limit2,
                              byte Bayram1Gunn, byte Bayram1Ayy, byte Bayram1Suree,
                              byte Bayram2Gunn, byte Bayram2Ayy, byte Bayram2Suree,
                              byte AvansOnayi)
        {
            int r = 0;

            UInt16 Bayram1, Bayram2;
            UInt32 KademeKatsayi1, KademeKatsayi2, KademeKatsayi3;

            KademeKatsayi1 = KademeKatsayi2 = KademeKatsayi3 = 0;

            r = cp.SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.ExternalAuthenticate(0x84, cp.GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            r = cp.ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            UInt32 CihazNo = Converter.Byte4toUInt32(cp.inBuf[8], cp.inBuf[9], cp.inBuf[10], cp.inBuf[11]);
            Integer4Byte CihazNoo = new Integer4Byte(CihazNo);
            byte[] sendData = { CihazNoo.bir, CihazNoo.iki, CihazNoo.uc, CihazNoo.dort, Convert.ToByte(CihazNoo.bir ^ 0x0F), Convert.ToByte(CihazNoo.iki ^ 0x0F), Convert.ToByte(CihazNoo.uc ^ 0x0F), Convert.ToByte(CihazNoo.dort ^ 0x0F) };

            if (Converter.BytetoString(0, 4, cp.inBuf) != cp.zone + "A ")
                return (int)Enums.ResultSC.ISSUER_HATA;

            if (CihazNo != devNo)
            {
                r = (int)Enums.ResultSC.ABONE_KARTI_DEGIL;
                cp.ResetCard();
                return r;
            }

            PinAbone = classDes.TripleDes(DKAbone, sendData, PinAbone);

            r = cp.VerifyCard(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFDataName2);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.ReadCard(0, 230);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            Map.Abone abone = new Map.Abone(cp.inBuf);

            if (abone.Cap != Cap)
                return (int)Enums.ResultSC.CAP_HATA;

            switch (Cap)
            {
                case 20:
                    Integer4Byte Katsayi2 = new Integer4Byte(Convert.ToUInt32(1000));
                    Integer4Byte Katsayi1 = new Integer4Byte(Convert.ToUInt32(Fiyat1 * 1000 / Fiyat2));
                    Integer4Byte Katsayi3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 1000 / Fiyat2));
                    KademeKatsayi1 = Katsayi1.value;
                    KademeKatsayi2 = Katsayi2.value;
                    KademeKatsayi3 = Katsayi3.value;
                    break;
                case 40:
                case 50:
                case 80:
                case 100:
                case 150:
                case 200:
                    Integer4Byte Katsayi_2 = new Integer4Byte(Convert.ToUInt32(10000));
                    Integer4Byte Katsayi_1 = new Integer4Byte(Convert.ToUInt32(Fiyat1 * 10000 / Fiyat2));
                    Integer4Byte Katsayi_3 = new Integer4Byte(Convert.ToUInt32(Fiyat3 * 10000 / Fiyat2));
                    KademeKatsayi1 = Katsayi_1.value;
                    KademeKatsayi2 = Katsayi_2.value;
                    KademeKatsayi3 = Katsayi_3.value;
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

            Integer4Byte Cihaz = new Integer4Byte(devNo);
            Integer4Byte Kat1 = new Integer4Byte(KademeKatsayi1);
            Integer4Byte Kat2 = new Integer4Byte(KademeKatsayi2);
            Integer4Byte Kat3 = new Integer4Byte(KademeKatsayi3);
            Integer4Byte Lim1 = new Integer4Byte(Limit1);
            Integer4Byte Lim2 = new Integer4Byte(Limit2);
            Integer4Byte AnaKredii = new Integer4Byte(AnaKredi);
            Integer4Byte YedekKredii = new Integer4Byte(YedekKredi);
            Integer4Byte AboneNoo = new Integer4Byte(abone.AboneNo);
            Integer2Byte Bayram1Deger = new Integer2Byte(Bayram1);
            Integer2Byte Bayram2Deger = new Integer2Byte(Bayram2);

            if ((abone.AkoYko & 128) == 0)
                abone.IslemNo++;
            if ((abone.AkoYko & 64) == 0)
                abone.IslemNo++;

            cp.outBuf[0] = Cihaz.bir;
            cp.outBuf[1] = Cihaz.iki;
            cp.outBuf[2] = Cihaz.uc;
            cp.outBuf[3] = Cihaz.dort;
            cp.outBuf[4] = Kat1.bir;
            cp.outBuf[5] = Kat1.iki;
            cp.outBuf[6] = Kat1.uc;
            cp.outBuf[7] = Kat1.dort;
            cp.outBuf[8] = Kat2.bir;
            cp.outBuf[9] = Kat2.iki;
            cp.outBuf[10] = Kat2.uc;
            cp.outBuf[11] = Kat2.dort;
            cp.outBuf[12] = Kat3.bir;
            cp.outBuf[13] = Kat3.iki;
            cp.outBuf[14] = Kat3.uc;
            cp.outBuf[15] = Kat3.dort;
            cp.outBuf[16] = Lim1.bir;
            cp.outBuf[17] = Lim1.iki;
            cp.outBuf[18] = Lim1.uc;
            cp.outBuf[19] = Lim1.dort;
            cp.outBuf[20] = Lim2.bir;
            cp.outBuf[21] = Lim2.iki;
            cp.outBuf[22] = Lim2.uc;
            cp.outBuf[23] = Lim2.dort;
            cp.outBuf[24] = AnaKredii.bir;
            cp.outBuf[25] = AnaKredii.iki;
            cp.outBuf[26] = AnaKredii.uc;
            cp.outBuf[27] = AnaKredii.dort;
            cp.outBuf[28] = YedekKredii.bir;
            cp.outBuf[29] = YedekKredii.iki;
            cp.outBuf[30] = YedekKredii.uc;
            cp.outBuf[31] = YedekKredii.dort;
            cp.outBuf[32] = 0XC0;
            cp.outBuf[33] = abone.IslemNo;
            cp.outBuf[34] = abone.KartNo;
            cp.outBuf[35] = Cap;
            cp.outBuf[36] = abone.Tip;
            cp.outBuf[37] = Donem;
            cp.outBuf[38] = 25;
            cp.outBuf[39] = 30;
            cp.outBuf[40] = 0;
            cp.outBuf[41] = 0;
            cp.outBuf[42] = AvansOnayi;
            cp.outBuf[43] = 0;
            cp.outBuf[44] = AboneNoo.bir;
            cp.outBuf[45] = AboneNoo.iki;
            cp.outBuf[46] = AboneNoo.uc;
            cp.outBuf[47] = AboneNoo.dort;
            cp.outBuf[48] = Bayram1Deger.bir;
            cp.outBuf[49] = Bayram1Deger.iki;
            cp.outBuf[50] = Bayram2Deger.bir;
            cp.outBuf[51] = Bayram2Deger.iki;
            cp.refLength = 52;

            r = cp.UpdateCard(0);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            return r;
        }

        public int AboneBosalt_CL()
        {
            int r = 0;

            r = cp.SelectBlock(DFName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.ExternalAuthenticate(0x84, cp.GetChallenge(DKAbone));
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFIssuerName);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            r = cp.ReadCard(12, 12);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            UInt32 CihazNo = Converter.Byte4toUInt32(cp.inBuf[8], cp.inBuf[9], cp.inBuf[10], cp.inBuf[11]);

            if (Converter.BytetoString(0, 4, cp.inBuf) != cp.zone + "A ")
                return (int)Enums.ResultSC.ABONE_KARTI_DEGIL;

            r = cp.UpdateIssuer("\0\0\0\0", 0);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            byte[] PinAbone = new byte[8];

            Integer4Byte cihazNo = new Integer4Byte(CihazNo);
            byte[] sendData = { cihazNo.bir, cihazNo.iki, cihazNo.uc, cihazNo.dort,
                                    Convert.ToByte(cihazNo.bir^0x0F), Convert.ToByte(cihazNo.iki^0x0F),
                                    Convert.ToByte(cihazNo.uc^0x0F), Convert.ToByte(cihazNo.dort^0x0F) };

            PinAbone = classDes.TripleDes(DKAbone, sendData, PinAbone);

            //r = cp.WriteKey(0x85, PinAbone);
            //if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.VerifyCard(0x85, PinAbone);
            if (r != (int)Enums.ResultSC.BASARILI) return r;
            r = cp.SelectBlock(EFDataName2);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            for (int i = 0; i < cp.outBuf.Length; i++)
                cp.outBuf[i] = 0;

            cp.refLength = 255;

            r = cp.UpdateCard(0);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            return r;
        }
        #endregion

        #region CONTACT
        private byte EXOR, newEXOR;
        private UInt32 SendAboneCsCCN(UInt32 dn, UInt32 alfa)
        {
            UInt32 carry;
            UInt32 rmask, pmask, qmask;
            UInt32 t1, t2, t3, t4;
            UInt32 sr;
            UInt32 i;

            pmask = 2;
            rmask = 4;
            qmask = 0x10;

            sr = dn;
            t1 = alfa << 2;
            sr = sr + t1 + alfa + 2;

            if (sr == 0) sr = 1;

            for (i = 0; i < 16; i++)
            {
                if ((sr & pmask) != 0) t1 = 1; else t1 = 0;
                if ((sr & rmask) != 0) t2 = 1; else t2 = 0;
                if ((sr & qmask) != 0) t3 = 1; else t3 = 0;
                if ((sr & 0x8000) != 0) t4 = 1; else t4 = 0;
                //carry = t1 ^ t2 ^ t3 ^ t4;
                carry = t1;// ^ t2 ^ t3 ^ t4;
                carry ^= t2;// ^ t2 ^ t3 ^ t4;
                carry ^= t3;// ^ t2 ^ t3 ^ t4;
                carry ^= t4;// ^ t2 ^ t3 ^ t4;
                sr <<= 1;
                if (carry != 0)
                {
                    sr |= 1;
                }
            }

            return sr;
        }

        private void EXOR_PROCESS(byte x)
        {
            for (int i = 0; i < 4; i++)
            {
                cp.inBuf[i] ^= x;
                cp.outBuf[i] ^= x;
            }
        }

        public void Outbuf_Randomize()
        {
            UInt32 sId = (UInt32)((DateTime.Now.Ticks / 100000) % 0xffffffff);
            cp.outBuf[0] = (byte)(sId % (256 * 256 * 256));
            sId = sId / 256;
            cp.outBuf[1] = (byte)(sId % (256 * 256));
            sId = sId / 256;
            cp.outBuf[2] = (byte)(sId % 256);
            cp.outBuf[3] = (byte)(sId / 256);
        }

        public string AboneOku_CT()
        {
            //Fields
            string result = "";
            UInt32 sr;
            byte ako, yko, a1 = 0, a2 = 0, a3 = 0, a4 = 0, a5 = 0, a6 = 0, a6x = 0, a6y = 0, a7, a8, a9, a15;
            byte a20 = (byte)'*';
            byte a11 = (byte)'0';

            UInt32 abono = 0, aytuk, gertuk = 0, kritik = 300;
            Int32 krd = 0, ykrd = 0, kkrd, iadekrd = 0, kalan = 0;

            int kartNo;
            int islem, pilsev, gul1;
            Single pilsev1, pilsev2;
            int tip = 0, cap = 0;
            UInt32 vaa = 0;
            int donem = 0, vop = 0, karthata;

            Int32 iadekredi = 0, negtuk;

            UInt32 sdtuk1, sdtuk2, sdtuk3, sdtuk4, sdtuk5, sdtuk6, k1tuk = 0, k2tuk = 0, k3tuk = 0, donemtuk;

            long[] sdtuk = new long[6];
            byte iadekarti, iadeyapilmis = 0, versiyon, arztip, iadeyap;
            byte vers;
            byte counter_L = 0;
            UInt32 harcanan;

            int day, donemgun;

            UInt32 kad1, kad2, kad3, lim1 = 0, lim2 = 0;

            TarihAl bilgi2 = new TarihAl(cp.inBuf);

            TarihAl bilgi9 = new TarihAl(cp.inBuf);

            TarihAl bilgi10 = new TarihAl(cp.inBuf);

            TarihAl bilgi11 = new TarihAl(cp.inBuf);

            TarihAl saytar = new TarihAl(cp.buffer);

            //end Fields
            int r = 0;

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            if (Converter.BytetoString(0, 4, cp.inBuf) != (cp.zone + "A "))
            {
                cp.FinishCard();
                return Enums.ResultSC.ISSUER_HATA.ToString();
            }

            result += "ISSUER(HX)|" + cp.inBuf[0] + " " + cp.inBuf[1] + " " + cp.inBuf[2] + " " + cp.inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)cp.inBuf[0] + " " + (Char)cp.inBuf[1] + " " + (Char)cp.inBuf[2] + " " + (Char)cp.inBuf[3] + "\n";

            r = cp.ReadCard(0x3F);
            if ((cp.inBuf[1] == 0x52) && (cp.inBuf[2] == 0x1F))
            {
                iadeyap = 0;
                vop = 10;

                r = cp.ReadCard(0X3C);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR = cp.inBuf[3];

                if (cp.inBuf[0] != 0x58)
                {
                    if (cp.inBuf[0] == 0x99)  // {printf("\nKART SAYACA TAKILMAMIS!...");
                    {
                        iadeyap = 2;
                        cp.ResetCard();
                        return Enums.ResultSC.SIFRE_HATA.ToString();
                    }
                    else
                    {
                        if (cp.inBuf[0] != 0x96)   // {printf("\nKART IADE DEGIL");
                        {
                            cp.ResetCard();
                            return Enums.ResultSC.SIFRE_HATA.ToString();
                        }
                        else
                            iadeyap = 1;
                    }
                }

                r = cp.ReadCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(EXOR);

                byte[] buf = new byte[2];

                buf[0] = cp.inBuf[0];
                buf[1] = cp.inBuf[1];
                UInt16 alfa1 = Converter.Byte2toUInt16(buf[0], buf[1]);

                buf[0] = cp.inBuf[2];
                buf[1] = cp.inBuf[3];
                UInt16 alfa2 = Converter.Byte2toUInt16(buf[0], buf[1]);

                r = cp.ReadCard(0x3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(EXOR);

                buf[0] = cp.inBuf[0];
                buf[1] = cp.inBuf[1];
                UInt16 alfa3 = Converter.Byte2toUInt16(buf[0], buf[1]);

                buf[0] = cp.inBuf[2];
                buf[1] = cp.inBuf[3];
                UInt16 alfa4 = Converter.Byte2toUInt16(buf[0], buf[1]);

                ///CSC 0  verify

                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x07);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                ///CSC 1  verify

                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x39);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                ///////////////USER AREA1 ACCESS CONDITION

                r = cp.ReadCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                kkrd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4); //doğru mu ? kkrd = *(long*)cp.inBuf;

                r = cp.ReadCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                harcanan = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x12);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                gertuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x13);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                negtuk = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x14);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));

                versiyon = cp.inBuf[0];
                arztip = cp.inBuf[2]; //ariza+vanadurumu
                pilsev = cp.inBuf[3]; //motorpil
                cp.buffer[0] = cp.inBuf[1];

                pilsev2 = pilsev;
                pilsev2 = pilsev2 * 6 / 255;

                //int olmadığı için bool tipine dönüştürdüm **bykudu
                if ((Convert.ToByte(cp.buffer[0] & 0x80) == 0x00)) a8 = 0; else a8 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x40) == 0x00)) a7 = 0; else a7 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x20) == 0x00)) a6 = 0; else a6 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x10) == 0x00)) a5 = 0; else a5 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x08) == 0x00)) a4 = 0; else a4 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x04) == 0x00)) a3 = 0; else a3 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x02) == 0x00)) a2 = 0; else a2 = 1;
                if ((Convert.ToByte(cp.buffer[0] & 0x01) == 0x00)) a1 = 0; else a1 = 1;

                r = cp.ReadCard(0x15);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                // bilgi2 = *(LEARN2*)cp.buffer; learnlü olan yorum satırları eklenecek
                bilgi2 = new TarihAl(cp.buffer);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                //	bilgi9 = *(LEARN2*)cp.buffer;
                bilgi9 = new TarihAl(cp.buffer);

                r = cp.ReadCard(0x16);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                // bilgi10 = *(LEARN2*)cp.buffer;
                bilgi10 = new TarihAl(cp.buffer);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                //bilgi11 = *(LEARN2*)cp.buffer;
                bilgi11 = new TarihAl(cp.buffer);

                r = cp.ReadCard(0x17);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));

                r = cp.ReadCard(0x19);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                //saytar = *(LEARN2*)cp.buffer;
                saytar = new TarihAl(cp.buffer);

                r = cp.ReadCard(0x1b);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                k1tuk = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                k2tuk = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x1a);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                k3tuk = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];

                UInt16 alfa5 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa6 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                ///CSC 2  verify

                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x3b);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                ///////////////USER AREA2 ACCESS CONDITION

                r = cp.ReadCard(0x28);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                sdtuk1 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                sdtuk2 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x29);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                sdtuk3 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                sdtuk4 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x2a);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                sdtuk5 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                sdtuk6 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x2b);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));

                day = cp.inBuf[0];
                pilsev = cp.inBuf[1];
                donemgun = cp.inBuf[2];

                pilsev1 = pilsev;
                pilsev1 = pilsev1 * 6 / 255;

                r = cp.ReadCard(0x2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                lim1 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                lim2 = (UInt32)Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x18);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));

                tip = cp.inBuf[0];
                cap = cp.inBuf[1];
                karthata = cp.inBuf[2];
                donem = cp.inBuf[3];

                if (karthata == 0xff) a9 = (byte)'*';
                else a9 = (byte)'b';

                r = cp.ReadCard(0x19);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                abono = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));

                kartNo = cp.inBuf[2] & 0X0F;
                islem = cp.inBuf[3];

                if (cp.inBuf[0] == 0x66) ako = (byte)'*';
                else ako = (byte)'b';

                if (cp.inBuf[1] == 0x66) yko = (byte)'*';
                else yko = (byte)'b';

                r = cp.ReadCard(0x31);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                krd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x32);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                ykrd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x37);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 2)));
                donemtuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                //tüm sorumluluk nebi abi tarafından geç denilerek üstlenildi. Şahit Alim kudu
                if (krd > 99999999) krd = 99999999;
                if (ykrd > 99999999) ykrd = 99999999;
                if (kkrd > 99999999) kkrd = 99999999;
                if (devNo > 99999999) devNo = 99999999;

                if (krd < -9999999) krd = -9999999;
                if (ykrd < -9999999) ykrd = -9999999;
                if (kkrd < -9999999) kkrd = -9999999;
                if (devNo < -9999999) devNo = 0;

                if ((bilgi2.saat > 99))
                {
                    bilgi2.gun = 0;
                    bilgi2.ay = 0;
                    bilgi2.saat = 0;
                }

                if ((bilgi9.saat > 99))
                {
                    bilgi9.gun = 0;
                    bilgi9.ay = 0;
                    bilgi9.saat = 0;
                }

                if ((bilgi10.saat > 99))
                {
                    bilgi10.gun = 0;
                    bilgi10.ay = 0;
                    bilgi10.saat = 0;
                }

                if ((bilgi11.saat > 99))
                {
                    bilgi11.gun = 0;
                    bilgi11.ay = 0;
                    bilgi11.saat = 0;
                }

                if ((saytar.saat > 99))
                {
                    saytar.gun = 0;
                    saytar.ay = 0;
                    saytar.saat = 0;
                }

                if ((bilgi2.ay > 12))
                    bilgi2.ay = 0;

                if ((bilgi9.ay > 12))
                    bilgi9.ay = 0;

                if ((bilgi10.ay > 12))
                    bilgi10.ay = 0;

                if ((bilgi11.ay > 12))
                    bilgi11.ay = 0;

                if ((saytar.ay > 12))
                    saytar.ay = 0;

                a15 = (byte)'0';

                if ((a7 == 1) || (a6 == 1) || (a1 == 1) || (a2 == 1) && (ako == '*'))
                    a15 = (byte)'1';

                iadekrd = 0;

                if (iadeyap == 1)
                {
                    if (ako == '*')
                    {
                        if (yko == 'b')
                            iadekrd = kkrd + ykrd;
                        else
                            iadekrd = kkrd;
                    }
                    else
                    {
                        if (yko == 'b')
                            iadekrd = kkrd + krd + ykrd;
                        else
                            iadekrd = kkrd + krd;
                    }
                }

                if (cap >= 40)
                    cap += 1;

                a9 = (byte)'*';

                result += "AboneNo|" + abono + "\n";
                result += "KartNo|" + kartNo + "\n";
                result += "CihazNo|" + devNo + "\n";
                result += "AnaKrediOkundu|" + ((Convert.ToChar(ako) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                result += "YedekKrediOkundu|" + ((Convert.ToChar(yko) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                result += "AnaKredi|" + krd + "\n";
                result += "YedekKredi|" + ykrd + "\n";
                result += "Durum1|" + Convert.ToChar(a15) + "\n";
                result += "Durum2|" + Convert.ToChar(a9) + "\n";
                result += "Tip|" + tip + "\n";
                result += "Cap|" + cap + "\n";
                result += "Donem|" + donem + "\n";
                result += "Limit1|" + lim1 + "\n";
                result += "Limit2|" + lim2 + "\n";
                result += "Iade|" + iadeyap + "\n";
                result += "IadeKredi|" + iadekrd + "\n";
                result += "DonemGun|" + donemgun + "\n";
                result += "PilSeviyesi|" + pilsev1 + "\n";
                result += "KritikKredi|" + kkrd + "\n";
                result += "Harcanan|" + harcanan + "\n";
                result += "KritikKrediLimiti|" + kritik + "\n";
                result += "VanaOperasyonSayisi|" + vop + "\n";
                result += "Tarih1|" + bilgi2.gun + "." + bilgi2.ay + "." + bilgi2.yil + "\n";
                result += "Tarih2|" + bilgi9.gun + "." + bilgi9.ay + "." + bilgi9.yil + "\n";
                result += "Tarih3|" + bilgi10.gun + "." + bilgi10.ay + "." + bilgi10.yil + "\n";
                result += "Tarih4|" + bilgi11.gun + "." + bilgi11.ay + "." + bilgi11.yil + "\n";
                result += "Durum3|" + a1 + "\n";
                result += "Durum4|" + a2 + "\n";
                result += "Durum5|" + a3 + "\n";
                result += "Durum6|" + a4 + "\n";
                result += "Durum7|" + a5 + "\n";
                result += "Durum8|" + a6 + "\n";
                result += "Durum9|" + a7 + "\n";
                result += "GercekTuketim|" + gertuk + "\n";
                result += "Kademe1Tuketim|" + k1tuk + "\n";
                result += "Kademe2Tuketim|" + k2tuk + "\n";
                result += "Kademe3Tuketim|" + k3tuk + "\n";
                result += "Donem1Tuketim|" + sdtuk1 + "\n";
                result += "Donem2Tuketim|" + sdtuk2 + "\n";
                result += "Donem3Tuketim|" + sdtuk3 + "\n";
                result += "Donem4Tuketim|" + sdtuk4 + "\n";
                result += "Donem5Tuketim|" + sdtuk5 + "\n";
                result += "Donem6Tuketim|" + sdtuk6 + "\n";
                result += "SayacTarihi|" + saytar.gun + "." + saytar.ay + "." + saytar.yil + "\n";
            }
            else
            {
                r = cp.ReadCard(0x3F);
                if ((cp.inBuf[1] == 40) || (cp.inBuf[1] == 50))
                {
                    r = cp.ReadCard(0x3d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    pilsev = cp.inBuf[1];

                    pilsev1 = pilsev;
                    pilsev1 = pilsev1 / 255 * 6;

                    r = cp.ReadCard(0x3c);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                    if (devNo == 0)
                    {
                        //kart bos
                        cp.ResetCard();
                        return Enums.ResultSC.CIHAZNO_HATA.ToString();
                    }

                    sr = SendAboneCsCCN(devNo, 0x1f1f);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x2e2e);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(7);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    sr = SendAboneCsCCN(devNo, 0x3d3d);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x7c7c);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(0x39);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    r = cp.ReadCard(0x3d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    kartNo = cp.inBuf[2] & 0X0F;

                    if ((cp.inBuf[0] & 0x80) == 0) ako = (byte)'*';
                    else ako = (byte)'b';

                    if ((cp.inBuf[0] & 0x40) == 0) yko = (byte)'*';
                    else yko = (byte)'b';

                    if ((cp.inBuf[0] & 0x01) == 0) a11 = (byte)'1';   //sayac ceza veya arzada
                    else a11 = (byte)'0';

                    r = cp.ReadCard(0x13);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    harcanan = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                    if (ako == '*')
                    {
                        r = cp.ReadCard(10);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        krd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(11);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        ykrd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(12);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        kalan = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(14);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        gertuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(16);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        kritik = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x1A);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.buffer[0] = cp.inBuf[3];
                        cp.buffer[1] = cp.inBuf[2];
                        //bilgi7 = *(LEARN7*)cp.buffer; // dönüştürülecek
                        TarihAl bilgi7 = new TarihAl(cp.buffer);
                        vop = cp.buffer[1];

                        if ((cp.buffer[0] & 0x40) == 0) a6x = (byte)'0'; else a6x = (byte)'1';   //ceza1
                        if ((cp.buffer[0] & 0x20) == 0) a6 = (byte)'0'; else a6 = (byte)'1';
                        if ((cp.buffer[0] & 0x10) == 0) a5 = (byte)'0'; else a5 = (byte)'1';
                        if ((cp.buffer[0] & 0x08) == 0) a4 = (byte)'0'; else a4 = (byte)'1';
                        if ((cp.buffer[0] & 0x04) == 0) a3 = (byte)'0'; else a3 = (byte)'1';
                        if ((cp.buffer[0] & 0x02) == 0) a2 = (byte)'0'; else a2 = (byte)'1';
                        if ((cp.buffer[0] & 0x01) == 0) a1 = (byte)'0'; else a1 = (byte)'1';

                        a6y = (byte)'0';

                        r = cp.ReadCard(0x1B);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        //bilgi2 = *(LEARN2*)cp.inBuf;
                        bilgi2 = new TarihAl(cp.inBuf);
                        cp.inBuf[0] = cp.inBuf[2];
                        cp.inBuf[1] = cp.inBuf[3];
                        //bilgi9 = *(LEARN2*)cp.inBuf;
                        bilgi9 = new TarihAl(cp.inBuf);

                        if ((bilgi2.saat > 99))
                        {
                            bilgi2.gun = 0;
                            bilgi2.ay = 0;
                            bilgi2.saat = 0;
                        }

                        if ((bilgi9.saat > 99))
                        {
                            bilgi9.gun = 0;
                            bilgi9.ay = 0;
                            bilgi9.saat = 0;
                        }

                        r = cp.ReadCard(0x1A);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        //bilgi10 = *(LEARN2*)cp.inBuf;
                        bilgi10 = new TarihAl(cp.inBuf);
                        cp.inBuf[0] = cp.inBuf[2];
                        cp.inBuf[1] = cp.inBuf[3];
                        //bilgi11 = *(LEARN2*)cp.inBuf;
                        bilgi11 = new TarihAl(cp.inBuf);

                        if ((bilgi10.saat > 99))
                        {
                            bilgi10.gun = 0;
                            bilgi10.ay = 0;
                            bilgi10.saat = 0;
                        }

                        if ((bilgi11.saat > 99))
                        {
                            bilgi11.gun = 0;
                            bilgi11.ay = 0;
                            bilgi11.saat = 0;
                        }

                        if ((bilgi2.ay > 12))
                        {
                            bilgi2.ay = 0;
                        }

                        if ((bilgi9.ay > 12))
                        {
                            bilgi9.ay = 0;
                        }

                        if ((bilgi10.ay > 12))
                        {
                            bilgi10.ay = 0;
                        }

                        if ((bilgi11.ay > 12))
                        {
                            bilgi11.ay = 0;
                        }

                        r = cp.ReadCard(0x1E);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        lim1 = Converter.Byte2toUInt16(cp.inBuf[0], cp.inBuf[1]);

                        r = cp.ReadCard(0x1F);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        lim2 = Converter.Byte2toUInt16(cp.inBuf[0], cp.inBuf[1]);

                        r = cp.ReadCard(0x3E);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        abono = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x3F);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                        tip = cp.inBuf[0];
                        cap = cp.inBuf[1];
                        karthata = cp.inBuf[2];
                        donem = cp.inBuf[3];

                        //////////////
                        //////////A39R  begin

                        r = cp.ReadCard(0x36);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                        counter_L = cp.inBuf[0];
                        iadekarti = cp.inBuf[1];
                        vers = cp.inBuf[3];

                        iadeyapilmis = 0;
                        if (iadekarti == 0x1b) //iade okunmus
                        {
                            iadeyapilmis = 1;

                            r = cp.ReadCard(0x12);
                            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                            iadekredi = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                            if (ako == 'b')
                            {                      //ana kredi okunmamis
                                r = cp.ReadCard(0x10);
                                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                                iadekredi += (Int32)Converter.ByteToDecimal(cp.inBuf, 4);
                            }

                            if (yko == 'b')
                            {                      //ana kredi okunmamis
                                r = cp.ReadCard(0x11);
                                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                                iadekredi += (Int32)Converter.ByteToDecimal(cp.inBuf, 4);
                            }
                        }
                        else
                            if (iadekarti == 0x1a)
                        {
                            iadeyapilmis = 2;
                            iadekredi = 0;
                        }
                        else
                        {
                            iadekredi = 0;
                        }

                        r = cp.ReadCard(0x28);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        k1tuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x29);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        k2tuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x2a);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        k3tuk = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x2c);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        TarihAl saytar2 = new TarihAl(cp.inBuf);

                        if (saytar2.yil > 99) saytar2.yil = 99;
                        if (saytar2.ay > 12) saytar2.ay = 0;
                        if (saytar2.gun > 31) saytar2.gun = 0;
                        if (saytar2.saat > 23) saytar2.saat = 0;
                        if (saytar2.dakika > 59) saytar2.dakika = 0;

                        r = cp.ReadCard(0x2D);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[0] = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x2E);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[1] = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x2F);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[2] = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x30);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[3] = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x31);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[4] = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x32);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        cp.inBuf[3] = 0;
                        sdtuk[5] = Converter.ByteToDecimal(cp.inBuf, 4);
                    }
                    else if (ako == 'b')
                    {
                        r = cp.ReadCard(0x10);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        krd = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x11);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        ykrd = Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x1E);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        lim1 = Converter.Byte2toUInt16(cp.inBuf[0], cp.inBuf[1]);

                        r = cp.ReadCard(0x1F);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        lim2 = Converter.Byte2toUInt16(cp.inBuf[0], cp.inBuf[1]);

                        r = cp.ReadCard(0x3E);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        abono = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                        r = cp.ReadCard(0x3F);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                        tip = cp.inBuf[0];
                        cap = cp.inBuf[1];
                        donem = cp.inBuf[3];

                        r = cp.ReadCard(0x36);
                        if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                        counter_L = cp.inBuf[0];
                        iadekarti = cp.inBuf[1];
                        vers = cp.inBuf[3];

                        iadeyapilmis = 0;

                        if (iadekarti == 0x1b) //iade okunmus
                        {
                            iadeyapilmis = 1;
                            r = cp.ReadCard(0x12);
                            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                            iadekredi = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                            if (ako == 'b')
                            {                      //ana kredi okunmamis
                                r = cp.ReadCard(0x10);
                                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                                iadekredi += (Int32)Converter.ByteToDecimal(cp.inBuf, 4);
                            }
                            if (yko == 'b')
                            {                      //ana kredi okunmamis
                                r = cp.ReadCard(0x11);
                                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                                iadekredi += (Int32)Converter.ByteToDecimal(cp.inBuf, 4);
                            }
                        }
                        else if (iadekarti == 0X1a)
                        {
                            iadeyapilmis = 2;
                            iadekredi = 0;
                        }
                        else
                        {
                            iadekredi = 0;
                        }
                    }
                    vaa = 1;

                    if ((ako == '*') && (vaa == 1))
                    {
                        if (a20 == '*')
                        {
                            if ((a1 == (byte)'1') || (a2 == (byte)'1') || (a6x == (byte)'1') || (a6 == (byte)'1') && (ako == '*'))
                                a11 = (byte)'1';
                            else
                                a11 = (byte)'0';
                        }
                    }
                    else if ((ako == 'b'))
                    {
                        a20 = (byte)'*';
                        a11 = (byte)'0';
                    }

                    result += "AboneNo|" + abono + "\n";
                    result += "KartNo|" + kartNo + "\n";
                    result += "CihazNo|" + devNo + "\n";
                    result += "AnaKrediOkundu|" + ((Convert.ToChar(ako) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                    result += "YedekKrediOkundu|" + ((Convert.ToChar(yko) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                    result += "AnaKredi|" + krd + "\n";
                    result += "YedekKredi|" + ykrd + "\n";
                    result += "Tip|" + tip + "\n";
                    result += "Cap|" + cap + "\n";
                    result += "Donem|" + donem + "\n";
                    result += "Limit1|" + lim1 + "\n";
                    result += "Limit2|" + lim2 + "\n";
                    result += "IadeKredi|" + iadekrd + "\n";
                    result += "PilSeviyesi|" + pilsev1 + "\n";
                    result += "Harcanan|" + harcanan + "\n";
                    result += "KritikKrediLimiti|" + kritik + "\n";
                    result += "VanaOperasyonSayisi|" + vop + "\n";
                    result += "Tarih1|" + bilgi2.gun + "." + bilgi2.ay + "." + bilgi2.yil + "\n";
                    result += "Tarih2|" + bilgi9.gun + "." + bilgi9.ay + "." + bilgi9.yil + "\n";
                    result += "Tarih3|" + bilgi10.gun + "." + bilgi10.ay + "." + bilgi10.yil + "\n";
                    result += "Tarih4|" + bilgi11.gun + "." + bilgi11.ay + "." + bilgi11.yil + "\n";
                    result += "Durum3|" + a1 + "\n";
                    result += "Durum4|" + a2 + "\n";
                    result += "Durum5|" + a3 + "\n";
                    result += "Durum6|" + a4 + "\n";
                    result += "Durum7|" + a5 + "\n";
                    result += "Durum8|" + a6 + "\n";
                    result += "Durum9|" + a6x + "\n";
                    result += "Durum9|" + a6y + "\n";
                    result += "GercekTuketim|" + gertuk + "\n";
                    result += "Kademe1Tuketim|" + k1tuk + "\n";
                    result += "Kademe2Tuketim|" + k2tuk + "\n";
                    result += "Kademe3Tuketim|" + k3tuk + "\n";
                    result += "SayacTarihi|" + saytar.gun + "." + saytar.ay + "." + saytar.yil + "\n";
                }
            }

            return result;
        }

        public int AboneYap_CT(UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Limit1, UInt32 Limit2, UInt32 devNo, byte KartNo, UInt32 AboneNo, byte Cap, byte Tip, byte Donem)
        {
            //FİELDS
            UInt32 kad1, kad2, kad3;
            int rslem;
            UInt32 akrd, ykrd;
            UInt32 beta1, fsc1, fsc2, fsc3;
            Int32 yangin_saat, oto_kapama_gun, pulse_gun, maxdebi;
            byte sayac_tip;

            UInt32 sr;
            string str = "";
            int r = 0;
            byte[] buf = new byte[2];

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (Converter.BytetoString(0, 4, cp.inBuf) != "\0\0\0\0")
            {
                cp.FinishCard();
                return (int)Enums.ResultSC.BOS_KART_DEGIL;
            }

            /*if (cp.inBuf[0] == issue_area[0])
            {
                FinishCard();
                return "8";
            }*/

            cp.outBuf[0] = 0xAA;
            cp.outBuf[1] = 0xAA;
            cp.outBuf[2] = 0xAA;
            cp.outBuf[3] = 0xAA;

            r = cp.VerifyCard(7);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            cp.outBuf[0] = 0x11;
            cp.outBuf[1] = 0x11;
            cp.outBuf[2] = 0x11;
            cp.outBuf[3] = 0x11;

            r = cp.VerifyCard(0x39);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            cp.outBuf[0] = 0x22;
            cp.outBuf[1] = 0x22;
            cp.outBuf[2] = 0x22;
            cp.outBuf[3] = 0x22;

            r = cp.VerifyCard(0x3b);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            cp.outBuf[0] = (byte)cp.zone[0];
            cp.outBuf[1] = (byte)cp.zone[1];
            cp.outBuf[2] = (byte)'A';
            cp.outBuf[3] = (byte)' ';

            r = cp.UpdateCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (Convert.ToByte(Cap % 10) != 0x00)
            {
                sayac_tip = 2;
                Cap -= 1;
            }
            else
                sayac_tip = 1;

            cp.outBuf[0] = sayac_tip;
            cp.outBuf[1] = 0;
            cp.outBuf[2] = 0;
            cp.outBuf[3] = 0;

            r = cp.UpdateCard(0x05);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if ((Cap == 20) || (sayac_tip == 2))
            {
                Random random_numb = new Random();

                byte random_number = Convert.ToByte(random_numb.Next(0x00, 0xFE));

                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0x58;
                cp.outBuf[1] = 0x52;
                cp.outBuf[2] = 0x1f;
                cp.outBuf[3] = random_number;

                newEXOR = cp.outBuf[3];
                r = cp.UpdateCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///////////////////////////////////////////////////
                ///NEW ALFASSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
                ///NEW CSCSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
                ///////////////////////////////////////////////////

                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                UInt16 alfa1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                UInt16 alfa2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(newEXOR);
                r = cp.UpdateCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                // ---------------------------
                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                UInt16 alfa3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                UInt16 alfa4 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(newEXOR);
                r = cp.UpdateCard(0x3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                // ---------------------------
                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                UInt16 alfa5 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                UInt16 alfa6 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc0[0] = cp.outBuf[0];
                cp.csc0[1] = cp.outBuf[1];
                cp.csc0[2] = cp.outBuf[2];
                cp.csc0[3] = cp.outBuf[3];

                r = cp.UpdateCard(6);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc0[0] = cp.outBuf[0];
                cp.csc0[1] = cp.outBuf[1];
                cp.csc0[2] = cp.outBuf[2];
                cp.csc0[3] = cp.outBuf[3];

                r = cp.UpdateCard(0x38);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc0[0] = cp.outBuf[0];
                cp.csc0[1] = cp.outBuf[1];
                cp.csc0[2] = cp.outBuf[2];
                cp.csc0[3] = cp.outBuf[3];

                r = cp.UpdateCard(0x3a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = Convert.ToByte(random_numb.Next(0x00, 0xFE));
                cp.outBuf[1] = Convert.ToByte(random_numb.Next(0x00, 0xFE));

                beta1 = Converter.Byte2toUInt16(cp.outBuf[0], cp.outBuf[1]);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///////FOURTH_SECRET_CODE();///update new code

                cp.buffer[0] = cp.csc0[0];
                cp.buffer[1] = cp.csc0[2];
                fsc1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.csc1[1];
                cp.buffer[1] = cp.csc1[3];
                fsc2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.csc2[0];
                cp.buffer[1] = cp.csc2[2];
                fsc3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                sr = SendAboneCsCCN(devNo, fsc1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, fsc2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x33);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, fsc3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, beta1);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x34);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                //////////fourth secret code finished

                /// FINISHED ///
                ///////////////////////////////////////////////////
                ///NEW ALFASSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
                ///NEW CSCSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS
                ///////////////////////////////////////////////////

                ///////////////////////

                yangin_saat = 6;
                oto_kapama_gun = 30;
                pulse_gun = 25;
                maxdebi = 10;

                Converter.UInt32toByte4(devNo, cp.outBuf);

                r = cp.UpdateCard(0x3c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                KartNo = (byte)(KartNo & 0x0f);
                KartNo = (byte)(KartNo | 0x60);//yeni kart biti
                cp.outBuf[0] = 0x99;
                cp.outBuf[1] = 0x99;
                cp.outBuf[2] = 0x60;
                cp.outBuf[3] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[0] = 0;
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[0] = 0;
                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x31);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x32);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt16toByte2((UInt16)Limit2, cp.outBuf);
                cp.outBuf[2] = cp.outBuf[0];
                cp.outBuf[3] = cp.outBuf[1];
                Converter.UInt16toByte2((UInt16)Limit1, cp.outBuf);

                //cp.outBuf[0] = BYTEOF(limm1, 0);
                //cp.outBuf[1] = BYTEOF(limm1, 1);
                //cp.outBuf[2] = BYTEOF(limm2, 0);
                //cp.outBuf[3] = BYTEOF(limm2, 1);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                kad1 = 10000;
                kad2 = 10000 * Fiyat2 / Fiyat1;
                kad3 = 10000 * Fiyat3 / Fiyat1;

                Converter.UInt16toByte2((UInt16)kad2, cp.outBuf);
                cp.outBuf[2] = cp.outBuf[0];
                cp.outBuf[3] = cp.outBuf[1];
                Converter.UInt16toByte2((UInt16)kad1, cp.outBuf);

                //cp.outBuf[0] = BYTEOF(kad1, 0);
                //cp.outBuf[1] = BYTEOF(kad1, 1);
                //cp.outBuf[2] = BYTEOF(kad2, 0);
                //cp.outBuf[3] = BYTEOF(kad2, 1);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x2d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x17);
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                Converter.UInt16toByte2((UInt16)kad3, cp.outBuf);
                cp.outBuf[2] = cp.outBuf[0];
                cp.outBuf[3] = cp.outBuf[1];
                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];
                //cp.outBuf[2] = BYTEOF(kad3, 0);
                //cp.outBuf[3] = BYTEOF(kad3, 1);
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x17);

                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;
                cp.outBuf[0] = 0;
                cp.outBuf[1] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x2f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt32toByte4(AboneNo, cp.outBuf);
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x19);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = (byte)Tip;
                cp.outBuf[1] = (byte)Cap;
                cp.outBuf[2] = 0x00;///hatali hazirla
                cp.outBuf[3] = (byte)Donem;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x18);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = (byte)pulse_gun;
                cp.outBuf[1] = (byte)oto_kapama_gun;
                cp.outBuf[2] = (byte)yangin_saat;
                cp.outBuf[3] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x36);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                // cp.outBuf = cp.inBuf;
                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = cp.inBuf[2];
                cp.outBuf[3] = cp.inBuf[3];
                cp.outBuf[2] = (byte)maxdebi;
                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
            }
            else if ((Cap == 40) || (Cap == 50))
            {
                sr = SendAboneCsCCN(devNo, 0x1f1f);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);

                sr = SendAboneCsCCN(devNo, 0x2e2e);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.UpdateCard(6);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, 0x3d3d);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);

                sr = SendAboneCsCCN(devNo, 0x7c7c);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.UpdateCard(0x38);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt32toByte4(devNo, cp.outBuf);
                r = cp.UpdateCard(0x3c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                KartNo = (byte)(KartNo & 0x0f);
                KartNo = (byte)(KartNo | 0x40);
                cp.outBuf[0] = 0xff;
                cp.outBuf[1] = 0xff;
                cp.outBuf[2] = (byte)KartNo;
                cp.outBuf[3] = 0;

                r = cp.UpdateCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[0] = 0;
                r = cp.UpdateCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                kad1 = (10000 * Fiyat1 / Fiyat1);
                kad2 = (10000 * Fiyat2 / Fiyat1);

                Converter.UInt16toByte2((UInt16)kad1, cp.outBuf);

                //   cp.outBuf[0] = BYTEOF(kad1, 0);
                //  cp.outBuf[1] = BYTEOF(kad1, 1);

                Converter.UInt16toByte2((UInt16)kad2, cp.inBuf);

                // cp.inBuf[0] = BYTEOF(kad2, 0);
                // cp.inBuf[1] = BYTEOF(kad2, 1);

                cp.outBuf[2] = cp.inBuf[0];
                cp.outBuf[3] = cp.inBuf[1];

                r = cp.UpdateCard(0x1D);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                kad3 = (10000 * Fiyat3 / Fiyat1);

                Converter.UInt16toByte2((UInt16)kad3, cp.outBuf);
                //  cp.outBuf[0] = BYTEOF(kad3, 0);
                // cp.outBuf[1] = BYTEOF(kad3, 1);
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                r = cp.UpdateCard(0x2B);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[3] = 0;
                Converter.UInt32toByte4(Limit1, cp.outBuf);
                r = cp.UpdateCard(0x1E);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[3] = 0;
                //*(long*)cp.outBuf = limm2;
                Converter.UInt32toByte4(Limit2, cp.outBuf);
                r = cp.UpdateCard(0x1F);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                //cp.outBuf[3] = (byte)abono;
                Converter.UInt32toByte4(AboneNo, cp.outBuf);
                r = cp.UpdateCard(0x3E);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = (byte)Tip;
                cp.outBuf[1] = (byte)Cap;
                cp.outBuf[2] = 0x00;     //kart yazma hata var
                cp.outBuf[3] = (byte)Donem;

                r = cp.UpdateCard(0x3F);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
            }

            return (int)Enums.ResultSC.BASARILI;
        }

        public string KrediOku_CT()
        {
            #region Fields

            // FILE* fp;
            string result = "";
            UInt32 kart_no; ////islem;
            byte a11 = (byte)'0';
            byte a1, a2, a3, a4, a5, a6, a8, a9, a15, a6x, a6y, ax;
            byte a7 = (byte)'*';
            Int32 kalan, harcanan;
            byte iadekarti;
            byte iadeyapilmis;
            Int32 abono = 0;
            byte ako, yko;
            Int32 krd, ykrd;
            int karthata;
            UInt32 tip = 0, cap = 0, donem = 0;
            Int32 lim1 = 0, lim2 = 0;
            Int32 vaa = 0;
            byte[] buf = new byte[2];
            buf = null;

            #endregion Fields

            UInt32 sr;
            string str = "";
            int r = 0;

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

            if (Converter.BytetoString(0, 4, cp.inBuf) != (cp.zone + "A "))
            {
                cp.FinishCard();
                return Enums.ResultSC.ABONE_KARTI_DEGIL.ToString();
            }

            result += "ISSUER(HX)|" + cp.inBuf[0] + " " + cp.inBuf[1] + " " + cp.inBuf[2] + " " + cp.inBuf[3] + "\n";
            result += "ISSUER(CH)|" + (Char)cp.inBuf[0] + " " + (Char)cp.inBuf[1] + " " + (Char)cp.inBuf[2] + " " + (Char)cp.inBuf[3] + "\n";

            r = cp.ReadCard(0x3F);
            if ((cp.inBuf[1] == 0x52) && (cp.inBuf[2] == 0x1F))
            {
                r = cp.ReadCard(0x3c);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                iadeyapilmis = 0;
                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR = cp.inBuf[3];

                if (cp.inBuf[0] != 0x58)
                {
                    if (cp.inBuf[0] == 0x99)
                    {
                        iadeyapilmis = 2;
                        cp.ResetCard();
                        return Enums.ResultSC.SIFRE_HATA.ToString();
                    }
                    else
                    {
                        if (cp.inBuf[0] != 0x96)
                        {
                            cp.ResetCard();
                            return Enums.ResultSC.SIFRE_HATA.ToString();
                        }
                        else
                            iadeyapilmis = 1;
                    }
                }
                r = cp.ReadCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(EXOR);

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);
                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(EXOR);

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);
                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa4 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(7);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x39);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                r = cp.ReadCard(0x1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 1));

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa5 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);
                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa6 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x3b);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                r = cp.ReadCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 2));

                if (cp.inBuf[0] == 0x66) ako = (byte)'*';
                else ako = (byte)'b';

                if (cp.inBuf[1] == 0x66) yko = (byte)'*';
                else yko = (byte)'b';

                r = cp.ReadCard(0x31);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR_PROCESS(Convert.ToByte(EXOR + 2));
                krd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x32);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR_PROCESS(Convert.ToByte(EXOR + 2));
                ykrd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x2f);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR_PROCESS(Convert.ToByte(EXOR + 2));
                abono = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR_PROCESS(Convert.ToByte(EXOR + 2));

                kart_no = (UInt32)(cp.inBuf[2] & 0X0F);

                if ((cp.inBuf[2] & 0x40) == 0) ax = (byte)'b';
                else ax = (byte)'*';

                r = cp.ReadCard(0x2e);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 2));

                tip = cp.inBuf[0];
                cap = cp.inBuf[1];
                karthata = cp.inBuf[2];
                donem = cp.inBuf[3];

                r = cp.ReadCard(0x2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 2));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                //lim1 = (Int32)Converter.ByteToDecimal(cp.buffer, 2);
                lim1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                //lim2 = (Int32)Converter.ByteToDecimal(cp.buffer, 2);
                lim2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 1));
                kalan = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR_PROCESS(Convert.ToByte(EXOR + 1));
                harcanan = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                EXOR = cp.inBuf[3];

                r = cp.ReadCard(0x14);
                if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                EXOR_PROCESS(Convert.ToByte(EXOR + 1));

                cp.buffer[0] = cp.inBuf[1];

                if (!Convert.ToBoolean((cp.buffer[0] & 0x80))) a8 = 0; else a8 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x40))) a7 = 0; else a7 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x20))) a6 = 0; else a6 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x10))) a5 = 0; else a5 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x08))) a4 = 0; else a4 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x04))) a3 = 0; else a3 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x02))) a2 = 0; else a2 = 1;
                if (!Convert.ToBoolean((cp.buffer[0] & 0x01))) a1 = 0; else a1 = 1;

                a15 = 0;

                if ((a7 == 1) || (a6 == 1) || (a1 == 1) || (a2 == 1))
                {
                    a15 = 1;
                }
                a9 = (byte)'*';

                result += "AboneNo|" + abono + "\n";
                result += "KartNo|" + kart_no + "\n";
                result += "CihazNo|" + devNo + "\n";
                result += "AnaKrediOkundu|" + ((Convert.ToChar(ako) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                result += "YedekKrediOkundu|" + ((Convert.ToChar(yko) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                result += "AnaKredi|" + krd + "\n";
                result += "YedekKredi|" + ykrd + "\n";
                result += "DURUM|" + a15 + "\n";
                result += "DURUM2|" + Convert.ToChar(a9) + "\n";
                result += "Tip|" + tip + "\n";
                result += "Cap|" + cap + "\n";
                result += "Donem|" + donem + "\n";
                result += "Limit1|" + lim1 + "\n";
                result += "Limit2|" + lim2 + "\n";
                result += "KalanKredi|" + kalan + "\n";
                result += "YeniKart|" + Convert.ToChar(ax) + "\n"; //yeni kart bilgisi
                result += "Iade|" + iadeyapilmis + "\n"; //iade bilgisi
            }
            else
            {
                r = cp.ReadCard(0x3F);
                if ((cp.inBuf[1] == 40) || (cp.inBuf[1] == 50))
                {
                    r = cp.ReadCard(0X3C);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                    sr = SendAboneCsCCN(devNo, 0x1f1f);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x2e2e);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(7);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    sr = SendAboneCsCCN(devNo, 0x3d3d);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x7c7c);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(0X39);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    r = cp.ReadCard(0x3d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    kart_no = (UInt32)(cp.inBuf[2] & 0X0F);

                    if ((cp.inBuf[0] & 0x80) == 0) ako = (byte)'*';
                    else ako = (byte)'b';

                    if ((cp.inBuf[0] & 0x40) == 0) yko = (byte)'*';
                    else yko = (byte)'b';

                    if ((cp.inBuf[0] & 0x01) == 0)
                        a11 = (byte)'1';   //sayac ceza veya arzada
                    else a11 = (byte)'0';

                    if ((cp.inBuf[2] & 0x40) == 0) ax = (byte)'b';
                    else ax = (byte)'*';

                    r = cp.ReadCard(0x10);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                    krd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                    r = cp.ReadCard(0x11);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                    ykrd = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                    r = cp.ReadCard(0x12);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                    kalan = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                    r = cp.ReadCard(0x13);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                    harcanan = (Int32)Converter.ByteToDecimal(cp.inBuf, 4);

                    r = cp.ReadCard(0x36);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    EXOR_PROCESS(Convert.ToByte(Convert.ToInt16(EXOR + 1)));
                    iadekarti = (byte)Converter.ByteToDecimal(cp.inBuf, 4);

                    iadeyapilmis = 0;
                    if (iadekarti == 0x1b) //iade okunmus
                    {
                        iadeyapilmis = 1;
                    }

                    if (iadekarti == 0x1A) //iade okunmus
                    {
                        iadeyapilmis = 2;
                    }

                    r = cp.ReadCard(0x3f);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();
                    karthata = cp.inBuf[2];

                    if (karthata != 0xff) a7 = (byte)'b';
                    r = cp.ReadCard(0x1A);
                    if (r != (int)Enums.ResultSC.BASARILI) return r.ToString();

                    cp.buffer[0] = cp.inBuf[3];

                    if ((cp.buffer[0] & 0x80) == 0) a6y = (byte)'*'; else a6y = (byte)'b';  //cezamag1
                    if ((cp.buffer[0] & 0x40) == 0) a6x = (byte)'*'; else a6x = (byte)'b';   //ceza1
                    if ((cp.buffer[0] & 0x20) == 0) a6 = (byte)'*'; else a6 = (byte)'b';
                    if ((cp.buffer[0] & 0x02) == 0) a2 = (byte)'*'; else a2 = (byte)'b';

                    if (ako == 'b') a7 = (byte)'*';
                    if (ako == '*')
                    {
                        if ((a7 == (byte)'b') || (a2 == 'b') || (a6x == 'b') || (a6y == 'b') || (a6 == 'b'))
                            a11 = (byte)'1';
                        else
                            a11 = (byte)'0';
                    }

                    result += "AboneNo|" + abono + "\n";
                    result += "KartNo|" + kart_no + "\n";
                    result += "CihazNo|" + devNo + "\n";
                    result += "AnaKrediOkundu|" + ((Convert.ToChar(ako) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                    result += "YedekKrediOkundu|" + ((Convert.ToChar(yko) == '*') ? "YÜKLENDİ" : "YÜKLENMEDİ") + "\n";
                    result += "AnaKredi|" + krd + "\n";
                    result += "YedekKredi|" + ykrd + "\n";
                    result += "DURUM|" + a11 + "\n";
                    result += "DURUM2|" + Convert.ToChar(a7) + "\n";
                    result += "Tip|" + tip + "\n";
                    result += "Cap|" + cap + "\n";
                    result += "Donem|" + donem + "\n";
                    result += "Limit1|" + lim1 + "\n";
                    result += "Limit2|" + lim2 + "\n";
                    result += "KalanKredi|" + kalan + "\n";
                    result += "HarcananKredi|" + harcanan + "\n";
                    result += "YeniKart|" + Convert.ToChar(ax) + "\n"; //yeni kart bilgisi
                    result += "Iade|" + iadeyapilmis + "\n"; //iade bilgisi
                }
            }

            return result;
        }

        public int KrediYaz_CT(UInt32 Fiyat1, UInt32 Fiyat2, UInt32 Fiyat3, UInt32 Limit1, UInt32 Limit2, UInt32 devNo, Int32 AnaKredi, Int32 YedekKredi, byte Donem, byte Cap)
        {
            //FİELDS
            UInt32 kad1, kad2, kad3 = 0;
            UInt32 devno1;
            Int32 degisim = 1;
            Int32 yangin_saat = 6;
            Int32 oto_kapama_gun = 30;
            Int32 pulse_gun = 25;
            Int32 tip;
            int islem_no;
            UInt32 beta1, beta2;
            UInt32 fsc1, fsc2, fsc3;
            byte sayac_tip;
            UInt32 sr;
            string str = "";
            int r = 0;
            byte[] buf = new byte[2];

            if ((Fiyat1 == 0) || (Fiyat2 == 0) || (Fiyat3 == 0) || (Limit1 == 0) || (Limit2 == 0) || (Donem == 0))
            {
                cp.FinishCard();
                return (int)Enums.ResultSC.PARAMETRE_HATA;
            }

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (Converter.BytetoString(0, 4, cp.inBuf) != (cp.zone + "A "))
            {
                cp.FinishCard();
                return (int)Enums.ResultSC.ABONE_KARTI_DEGIL;
            }

            r = cp.ReadCard(0x05);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (cp.inBuf[0] == 2)
            {
                sayac_tip = 2;
            }
            else
            {
                sayac_tip = 1;
            }

            if ((Cap == 20) || (sayac_tip == 2))
            {
                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                /*ERTAN
                if ((cp.inBuf[1] != 0x52) || (cp.inBuf[2] != 0x1F))
                {
                    FinishCard();
                    return "0";
                }*/
            }
            else
            {
                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                if ((cp.inBuf[1] == 0x52) && (cp.inBuf[2] == 0x1F))
                {
                    cp.ResetCard();
                    return (int)Enums.ResultSC.SIFRE_HATA;
                }
            }

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if ((Cap == 20) || (sayac_tip == 2))
            {
                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR = cp.inBuf[3];

                if (cp.inBuf[0] != 0x58)
                {
                    if (cp.inBuf[0] == 0x99)
                    {
                        cp.ResetCard();
                        return (int)Enums.ResultSC.SIFRE_HATA;
                    }
                    if (cp.inBuf[0] == 0x96)
                    {
                        cp.ResetCard();
                        return (int)Enums.ResultSC.SIFRE_HATA;
                    }
                }

                if (Convert.ToByte(Cap % 10) != 0x00)
                {
                    Cap -= 1;
                }

                r = cp.ReadCard(0x19);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(EXOR + 1));
                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                TarihAl saytar = new TarihAl(cp.buffer);

                r = cp.ReadCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(EXOR);

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                r = cp.ReadCard(0x3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(EXOR);

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa4 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                ///CSC 0  verify

                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(7);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///CSC 1  verify

                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x39);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(EXOR + 1));

                cp.buffer[0] = cp.inBuf[0];
                cp.buffer[1] = cp.inBuf[1];
                UInt16 alfa5 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.inBuf[2];
                cp.buffer[1] = cp.inBuf[3];
                UInt16 alfa6 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x3b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(EXOR + 2));

                //TarihAl islem_yeni = new TarihAl(cp.buffer);

                cp.buffer[0] = cp.inBuf[2];

                byte islem_yeni = 0;
                islem_yeni = cp.inBuf[2];

                islem_no = cp.inBuf[3];
                if (cp.inBuf[0] == 0x66)
                    islem_no++;
                if (cp.inBuf[1] == 0x66)
                    islem_no++;

                Random random_numb = new Random();

                byte random_number = Convert.ToByte(random_numb.Next(0x00, 0xFE));

                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0x58;
                cp.outBuf[1] = 0x52;
                cp.outBuf[2] = 0x1f;
                cp.outBuf[3] = EXOR;

                newEXOR = EXOR;

                r = cp.UpdateCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                alfa1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                alfa2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(newEXOR);

                r = cp.UpdateCard(0x3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                alfa3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                alfa4 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(newEXOR);

                r = cp.UpdateCard(0x3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Outbuf_Randomize();

                cp.buffer[0] = cp.outBuf[0];
                cp.buffer[1] = cp.outBuf[1];
                alfa5 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.outBuf[2];
                cp.buffer[1] = cp.outBuf[3];
                alfa6 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

                r = cp.UpdateCard(0x1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc0[0] = cp.outBuf[0];
                cp.csc0[1] = cp.outBuf[1];
                cp.csc0[2] = cp.outBuf[2];
                cp.csc0[3] = cp.outBuf[3];

                r = cp.UpdateCard(6);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc1[0] = cp.outBuf[0];
                cp.csc1[1] = cp.outBuf[1];
                cp.csc1[2] = cp.outBuf[2];
                cp.csc1[3] = cp.outBuf[3];

                r = cp.UpdateCard(0x38);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                cp.csc2[0] = cp.outBuf[0];
                cp.csc2[1] = cp.outBuf[1];
                cp.csc2[2] = cp.outBuf[2];
                cp.csc2[3] = cp.outBuf[3];

                r = cp.UpdateCard(0x3a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = Convert.ToByte(random_numb.Next(0x00, 0xFE));
                cp.outBuf[1] = Convert.ToByte(random_numb.Next(0x00, 0xFE));
                cp.outBuf[2] = cp.inBuf[2];
                cp.outBuf[3] = cp.inBuf[3];
                beta1 = Converter.Byte2toUInt16(cp.outBuf[0], cp.outBuf[1]);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.buffer[0] = cp.csc0[0];
                cp.buffer[1] = cp.csc0[2];
                fsc1 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.csc1[1];
                cp.buffer[1] = cp.csc1[3];
                fsc2 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                cp.buffer[0] = cp.csc2[0];
                cp.buffer[1] = cp.csc2[2];
                fsc3 = Converter.Byte2toUInt16(cp.buffer[0], cp.buffer[1]);

                sr = SendAboneCsCCN(devNo, fsc1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, fsc2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x33);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, fsc3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, beta1);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x34);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

                r = cp.UpdateCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x12);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x13);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x14);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x15);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x16);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x17);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x18);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x19);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x1b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

                cp.outBuf[0] = cp.inBuf[0]; //versiyon
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

                r = cp.UpdateCard(0x14);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x18);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = cp.inBuf[0]; //tip
                cp.outBuf[1] = cp.inBuf[1]; //cap
                cp.outBuf[2] = 0;//kyh
                cp.outBuf[3] = cp.inBuf[3]; //donem

                if (degisim == 1)
                {
                    cp.outBuf[0] = cp.outBuf[0];
                    cp.outBuf[1] = (byte)Cap;
                    cp.outBuf[3] = (byte)Donem;
                }

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));

                r = cp.UpdateCard(0x18);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x28);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x29);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x2a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x37);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0x2b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = cp.inBuf[0]; //lim1
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = cp.inBuf[2];  //lim2
                cp.outBuf[3] = cp.inBuf[3];

                if (degisim == 1)
                {
                    Converter.UInt16toByte2((UInt16)Limit2, cp.outBuf);
                    cp.outBuf[2] = cp.outBuf[0];
                    cp.outBuf[3] = cp.outBuf[1];
                    Converter.UInt16toByte2((UInt16)Limit1, cp.outBuf);
                    // cp.outBuf[0] = BYTEOF(lim1, 0);
                    // cp.outBuf[1] = BYTEOF(lim1, 1);
                    // cp.outBuf[2] = BYTEOF(lim2, 0);
                    // cp.outBuf[3] = BYTEOF(lim2, 1);
                }

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x2d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = cp.inBuf[0]; //kad1
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = cp.inBuf[2];  //kad2
                cp.outBuf[3] = cp.inBuf[3];

                if (degisim == 1)
                {
                    kad1 = 10000;
                    kad2 = 10000 * Fiyat2 / Fiyat1;
                    kad3 = 10000 * Fiyat3 / Fiyat1;

                    Converter.UInt16toByte2((UInt16)kad2, cp.outBuf);
                    cp.outBuf[2] = cp.outBuf[0];
                    cp.outBuf[3] = cp.outBuf[1];
                    Converter.UInt16toByte2((UInt16)kad1, cp.outBuf);
                    //cp.outBuf[0] = BYTEOF(kad1, 0);
                    //cp.outBuf[1] = BYTEOF(kad1, 1);
                    //cp.outBuf[2] = BYTEOF(kad2, 0);
                    //cp.outBuf[3] = BYTEOF(kad2, 1);
                }

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x2d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x17);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt16toByte2((UInt16)kad3, cp.outBuf);
                cp.outBuf[2] = cp.outBuf[0];
                cp.outBuf[3] = cp.outBuf[1];
                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];

                //cp.outBuf[2] = BYTEOF(kad3, 0);
                //cp.outBuf[3] = BYTEOF(kad3, 1);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x17);

                Converter.Int32toByte4(AnaKredi, cp.outBuf);

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));
                r = cp.UpdateCard(0x31);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.Int32toByte4(YedekKredi, cp.outBuf);

                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x32);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = 0x99;
                cp.outBuf[1] = 0x99;
                //if (degisim == 1) cp.outBuf[1] = 0xff;
                cp.outBuf[2] = cp.inBuf[2];
                cp.outBuf[3] = (byte)islem_no;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x30);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x36);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = cp.inBuf[2];
                cp.outBuf[3] = 0;
                if (degisim == 1)
                {
                    cp.outBuf[0] = (byte)pulse_gun;
                    cp.outBuf[1] = (byte)oto_kapama_gun;
                    cp.outBuf[2] = (byte)yangin_saat;
                    cp.outBuf[3] = 0;
                }

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x36);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                /* kendisi yorum satırıydi düzeltilmeyecek...
                    kapama_tarihi.yil=yill;
                    kapama_tarihi.gun=gunn;
                    kapama_tarihi.ay =ayy;
                    kapama_tarihi.saat=saatt;
                    kapama_tarihi.dakika=0;
                        if(tarih_degisim==1)   kapama_tarihi.dakika=0xff;
                    *(LEARN*)cp.outBuf = kapama_tarihi;
                */

                EXOR_PROCESS(Convert.ToByte(newEXOR + 1));
                r = cp.UpdateCard(0x1f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = cp.inBuf[2];
                cp.outBuf[3] = cp.inBuf[3];

                cp.outBuf[2] = 10;//maxdebi;

                EXOR_PROCESS(Convert.ToByte(newEXOR + 2));

                r = cp.UpdateCard(0x35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
            }
            else if ((Cap == 40) || (Cap == 50))
            {
                r = cp.ReadCard(0x36);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                if (cp.inBuf[1] == 0x1a)
                {
                    cp.ResetCard();
                    return (int)Enums.ResultSC.SIFRE_HATA;
                }
                if (cp.inBuf[1] == 0x1b)
                {
                    cp.ResetCard();
                    return (int)Enums.ResultSC.SIFRE_HATA;
                }

                r = cp.ReadCard(0x3C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                devno1 = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                if (devno1 != devNo)
                {
                    cp.ResetCard();
                    return (int)Enums.ResultSC.CIHAZNO_HATA;
                }

                sr = SendAboneCsCCN(devNo, 0x1f1f);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, 0x2e2e);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x07);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                sr = SendAboneCsCCN(devNo, 0x3d3d);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, 0x7c7c);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0X39);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0x22;
                cp.outBuf[1] = 0x22;
                cp.outBuf[2] = 0x22;
                cp.outBuf[3] = 0x22;

                r = cp.VerifyCard(0x3B);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.ReadCard(0x3D);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                byte islem_no1 = 0;

                islem_no1 = cp.inBuf[3];

                if (Convert.ToByte(cp.inBuf[0] & 0x80) == 0X00) islem_no1 = Convert.ToByte(islem_no1 + 1);

                if (Convert.ToByte(cp.inBuf[0] & 0x40) == 0X00) islem_no1 = Convert.ToByte(islem_no1 + 1);

                //cp.buffer[0] = cp.inBuf[2];

                byte islem_yeni1;

                islem_yeni1 = cp.inBuf[2];

                Converter.Int32toByte4(AnaKredi, cp.outBuf);
                r = cp.UpdateCard(0x10);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.Int32toByte4(YedekKredi, cp.outBuf);
                r = cp.UpdateCard(0x11);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0xdf;
                cp.outBuf[1] = 0xff;

                //*(LEARN_isl_yeni1*)cp.buffer = islem_yeni1;

                cp.outBuf[2] = Convert.ToByte(islem_yeni1);// cp.buffer[0];

                // *(LEARN1*)cp.buffer = islem_no1;

                cp.outBuf[3] = Convert.ToByte(islem_no1);// cp.buffer[0];
                r = cp.UpdateCard(0x3D);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                kad1 = (10000 * Fiyat1 / Fiyat1);
                kad2 = (10000 * Fiyat2 / Fiyat1);

                Converter.UInt16toByte2((UInt16)kad1, cp.outBuf);
                Converter.UInt16toByte2((UInt16)kad2, cp.inBuf);

                cp.outBuf[2] = cp.inBuf[0];
                cp.outBuf[3] = cp.inBuf[1];

                r = cp.UpdateCard(0x1D);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                kad3 = (10000 * Fiyat3 / Fiyat1);
                Converter.UInt16toByte2((UInt16)kad3, cp.outBuf);
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                r = cp.UpdateCard(0x2B);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt32toByte4((UInt32)Limit1, cp.outBuf);
                r = cp.UpdateCard(0x1E);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                Converter.UInt32toByte4((UInt32)Limit2, cp.outBuf);
                r = cp.UpdateCard(0x1F);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                r = cp.ReadCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = cp.inBuf[0];
                cp.outBuf[1] = cp.inBuf[1];
                cp.outBuf[2] = 0x00;
                cp.outBuf[3] = (byte)Donem;

                r = cp.UpdateCard(0x3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
            }

            return (int)Enums.ResultSC.BASARILI;
        }

        public int AboneBosalt_CT()
        {
            int r = 0;
            UInt32 sr;

            r = cp.ReadCard(1);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if (Converter.BytetoString(0, 4, cp.inBuf) != (cp.zone + "A "))
            {
                cp.FinishCard();
                return (int)Enums.ResultSC.ABONE_KARTI_DEGIL;
            }

            r = cp.ReadCard(0x3F);
            if (r != (int)Enums.ResultSC.BASARILI) return r;

            if ((cp.inBuf[1] == 0x52) && (cp.inBuf[2] == 0x1F))
            {
                r = cp.ReadCard(0X3C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                r = cp.ReadCard(0X3F);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                EXOR = cp.inBuf[3];

                r = cp.ReadCard(0X3D);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                EXOR_PROCESS(EXOR);

                byte[] buf = new byte[2];
                buf[0] = cp.inBuf[0];
                buf[1] = cp.inBuf[1];
                UInt16 alfa1 = Converter.Byte2toUInt16(buf[0], buf[1]);

                buf[0] = cp.inBuf[2];
                buf[1] = cp.inBuf[3];
                UInt16 alfa2 = Converter.Byte2toUInt16(buf[0], buf[1]);

                r = cp.ReadCard(0X3E);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                EXOR_PROCESS(EXOR);

                buf[0] = cp.inBuf[0];
                buf[1] = cp.inBuf[1];
                UInt16 alfa3 = Converter.Byte2toUInt16(buf[0], buf[1]);

                buf[0] = cp.inBuf[2];
                buf[1] = cp.inBuf[3];
                UInt16 alfa4 = Converter.Byte2toUInt16(buf[0], buf[1]);

                ///CSC 0  verify
                sr = SendAboneCsCCN(devNo, alfa1);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa2);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(7);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///CSC 1  verify
                sr = SendAboneCsCCN(devNo, alfa3);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa4);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0X39);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///////////////USER AREA1 ACCESS CONDITION

                r = cp.ReadCard(0X1C);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                EXOR_PROCESS(Convert.ToByte(EXOR + 1));

                buf[0] = cp.inBuf[0];
                buf[1] = cp.inBuf[1];
                UInt16 alfa5 = Converter.Byte2toUInt16(buf[0], buf[1]);

                buf[0] = cp.inBuf[2];
                buf[1] = cp.inBuf[3];
                UInt16 alfa6 = Converter.Byte2toUInt16(buf[0], buf[1]);

                /////vop??????????????

                ///CSC 2  verify
                sr = SendAboneCsCCN(devNo, alfa5);
                cp.outBuf[0] = (byte)(sr / 256);
                cp.outBuf[1] = (byte)(sr % 256);
                sr = SendAboneCsCCN(devNo, alfa6);
                cp.outBuf[2] = (byte)(sr / 256);
                cp.outBuf[3] = (byte)(sr % 256);

                r = cp.VerifyCard(0x3b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                ///////////////USER AREA2 ACCESS CONDITION
                cp.outBuf[0] = 0;
                cp.outBuf[1] = 0;
                cp.outBuf[2] = 0;
                cp.outBuf[3] = 0;

                r = cp.UpdateCard(0X01);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X10);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X11);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X12);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X13);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X14);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X15);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X16);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X17);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X18);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X19);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X1a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X1b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X1c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
                //
                r = cp.UpdateCard(0X1d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X1e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X1f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X28);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X29);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2b);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X2f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X30);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X31);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X32);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X33);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X34);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X35);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X36);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X37);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X3c);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X3d);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X3e);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X3f);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                r = cp.UpdateCard(0X05);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0xaa;
                cp.outBuf[1] = 0xaa;
                cp.outBuf[2] = 0xaa;
                cp.outBuf[3] = 0xaa;

                r = cp.UpdateCard(0X06);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0x11;
                cp.outBuf[1] = 0x11;
                cp.outBuf[2] = 0x11;
                cp.outBuf[3] = 0x11;

                r = cp.UpdateCard(0X38);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                cp.outBuf[0] = 0x22;
                cp.outBuf[1] = 0x22;
                cp.outBuf[2] = 0x22;
                cp.outBuf[3] = 0x22;

                r = cp.UpdateCard(0X3a);
                if (r != (int)Enums.ResultSC.BASARILI) return r;
            }
            else
            {
                r = cp.ReadCard(0x3F);
                if (r != (int)Enums.ResultSC.BASARILI) return r;

                //ERTAN if ((cp.inBuf[1] == 0x40) || (cp.inBuf[1] == 0x50))
                {
                    r = cp.ReadCard(0X3C);

                    if (r != (int)Enums.ResultSC.BASARILI) return r;
                    UInt32 devNo = (UInt32)Converter.ByteToDecimal(cp.inBuf, 4);

                    sr = SendAboneCsCCN(devNo, 0x1F1F);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x2E2E);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(7);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    sr = SendAboneCsCCN(devNo, 0x3D3D);
                    cp.outBuf[0] = (byte)(sr / 256);
                    cp.outBuf[1] = (byte)(sr % 256);
                    sr = SendAboneCsCCN(devNo, 0x7C7C);
                    cp.outBuf[2] = (byte)(sr / 256);
                    cp.outBuf[3] = (byte)(sr % 256);

                    r = cp.VerifyCard(0x39);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    cp.outBuf[0] = 0x22;
                    cp.outBuf[1] = 0x22;
                    cp.outBuf[2] = 0x22;
                    cp.outBuf[3] = 0x22;
                    r = cp.VerifyCard(0x3b);

                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    //MessageBox(0,"  SIFRELEME DOGRU   ","MESSAGE ",MB_OK);

                    cp.outBuf[0] = 0x00;
                    cp.outBuf[1] = 0x00;
                    cp.outBuf[2] = 0x00;
                    cp.outBuf[3] = 0x00;

                    r = cp.ReadCard(4);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;
                    if (cp.inBuf[3] != 0x80)
                    {
                        r = cp.UpdateCard(0x01);
                        if (r != (int)Enums.ResultSC.BASARILI) return r;
                    }

                    r = cp.UpdateCard(0X10);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X11);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X12);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X13);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X14);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X15);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X16);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X17);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X18);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X19);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1a);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1b);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1c);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1e);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X1f);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    //////////////////////////////////////////////////////////////////////////

                    r = cp.UpdateCard(0X28);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X29);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2a);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2b);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2c);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2e);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X2f);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X30);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X31);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X32);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    //MessageBox(0,"  USER AREA II ADRES 32 BOSALTILDI   ","MESSAGE ",MB_OK);

                    r = cp.UpdateCard(0X33);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    // MessageBox(0,"  USER AREA II ADRES 33 BOSALTILDI   ","MESSAGE ",MB_OK);

                    r = cp.UpdateCard(0X34);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X35);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X36);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X37);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    /*****************************************/
                    //MessageBox(0,"  USER AREA II BOSALTILDI   ","MESSAGE ",MB_OK);

                    r = cp.UpdateCard(0X3c);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X3d);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X3e);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    r = cp.UpdateCard(0X3f);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    /*****************************************/

                    //MessageBox(0,"  PROTECTED AREA BOSALTILDI  ","MESSAGE ",MB_OK);

                    cp.outBuf[0] = 0xaa;
                    cp.outBuf[1] = 0xaa;
                    cp.outBuf[2] = 0xaa;
                    cp.outBuf[3] = 0xaa;

                    r = cp.UpdateCard(0X06);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    cp.outBuf[0] = 0x11;
                    cp.outBuf[1] = 0x11;
                    cp.outBuf[2] = 0x11;
                    cp.outBuf[3] = 0x11;

                    r = cp.UpdateCard(0X38);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;

                    cp.outBuf[0] = 0x22;
                    cp.outBuf[1] = 0x22;
                    cp.outBuf[2] = 0x22;
                    cp.outBuf[3] = 0x22;

                    r = cp.UpdateCard(0X3a);
                    if (r != (int)Enums.ResultSC.BASARILI) return r;
                }
            }

            return (int)Enums.ResultSC.BASARILI;
        }
        #endregion
    }
}
