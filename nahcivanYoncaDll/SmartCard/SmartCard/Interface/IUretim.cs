using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Interface
{
    public interface IUretim
    {
        string FormUret();
        string UretimFormat(UInt32 CihazNo,
                                UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                                UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                                UInt32 KritikKredi, byte DonemGun, byte VanaPulseSure, byte VanaCntSure, byte Cap, byte VanaBekleme, byte Dil, byte Sensor, byte NoktaHane, byte Carpan, byte LcdType, byte BaglantiPeriyot);
        string UretimCihazNo(UInt32 CihazNo,
                            UInt32 KademeKatsayi1, UInt32 KademeKatsayi2, UInt32 KademeKatsayi3, UInt32 KademeKatsayi4, UInt32 KademeKatsayi5,
                            UInt32 Limit1, UInt32 Limit2, UInt32 Limit3, UInt32 Limit4,
                            byte DonemGun, UInt32 MekanikTuketim, byte Cap, byte VanaBekleme, byte Dil, byte Sensor, byte NoktaHane, byte Carpan, string APN, string ServerIP, string Port);
        string UretimAc(byte KontrolDegeri);
        string UretimKapat(byte KontrolDegeri);

        string UretimTestMod(UInt32 devNo);

        string UretimLcd(byte LcdType);
    }
}
