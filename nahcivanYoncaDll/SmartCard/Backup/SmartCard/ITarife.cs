using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public interface ITarife
    {
        UInt32 Fiyat1 { get; set; }
        UInt32 Fiyat2 { get; set; }
        UInt32 Fiyat3 { get; set; }
        UInt32 Lim1 { get; set; }
        UInt32 Lim2 { get; set; }
        UInt32 LoadLimit { get; set; }
        UInt32 Aksam { get; set; }
        UInt32 Sabah { get; set; }
        UInt32 HaftaSonuAksam { get; set; }
        UInt32 FixCharge { get; set; }
        //UInt32 OtherCharge { get; set; }
        //UInt32 DailyDebt { get; set; }
        //UInt32 TotalDebt { get; set; }
        //UInt32 DebtMode { get; set; }
        //UInt16 LowVoltage { get; set; }
        //UInt16 HighVoltage { get; set; }
        //Int32 BuzzerInterval { get; set; }
        //Int32 BuzzerDuration { get; set; }
    }
}
