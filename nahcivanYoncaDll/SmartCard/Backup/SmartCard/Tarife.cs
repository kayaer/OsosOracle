using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard
{
    public class Tarife : ITarife
    {
        UInt32 _Fiyat1;
        UInt32 _Fiyat2;
        UInt32 _Fiyat3;
        UInt32 _Lim1;
        UInt32 _Lim2;
        UInt32 _LoadLimit;
        UInt32 _Aksam;
        UInt32 _Sabah;
        UInt32 _HaftaSonuAksam;
        UInt32 _FixCharge;
        UInt32 _OtherCharge;
        UInt32 _DailyDebt;
        UInt32 _TotalDebt;
        UInt32 _DebtMode;
        UInt16 _LowVoltage;
        UInt16 _HighVoltage;
        Int32 _BuzzerInterval;
        Int32 _BuzzerDuration;


        #region ITarife Members

        public uint Fiyat1
        {
            get
            {
                return _Fiyat1;
            }
            set
            {
                _Fiyat1 = value;
            }
        }

        public uint Fiyat2
        {
            get
            {
                return _Fiyat2;
            }
            set
            {
                _Fiyat2 = value;
            }
        }

        public uint Fiyat3
        {
            get
            {
                return _Fiyat3;
            }
            set
            {
                _Fiyat3 = value;
            }
        }

        public uint Lim1
        {
            get
            {
                return _Lim1;
            }
            set
            {
                _Lim1 = value;
            }
        }

        public uint Lim2
        {
            get
            {
                return _Lim2;
            }
            set
            {
                _Lim2 = value;
            }
        }

        public uint LoadLimit
        {
            get
            {
                return _LoadLimit;
            }
            set
            {
                _LoadLimit = value;
            }
        }

        public uint Aksam
        {
            get
            {
                return _Aksam;
            }
            set
            {
                _Aksam = value;
            }
        }

        public uint Sabah
        {
            get
            {
                return _Sabah;
            }
            set
            {
                _Sabah = value;
            }
        }

        public uint HaftaSonuAksam
        {
            get
            {
                return _HaftaSonuAksam;
            }
            set
            {
                _HaftaSonuAksam = value;
            }
        }

        public uint FixCharge
        {
            get
            {
                return _FixCharge;
            }
            set
            {
                _FixCharge = value;
            }
        }

        public uint OtherCharge
        {
            get
            {
                return _OtherCharge;
            }
            set
            {
                _OtherCharge = value;
            }
        }

        public uint DailyDebt
        {
            get
            {
                return _DailyDebt;
            }
            set
            {
                _DailyDebt = value;
            }
        }

        public uint TotalDebt
        {
            get
            {
                return _TotalDebt;
            }
            set
            {
                _TotalDebt = value;
            }
        }

        public uint DebtMode
        {
            get
            {
                return _DebtMode;
            }
            set
            {
                _DebtMode = value;
            }
        }


        public ushort LowVoltage
        {
            get
            {
                return _LowVoltage;
            }
            set
            {
                _LowVoltage = value;
            }
        }

        public ushort HighVoltage
        {
            get
            {
                return _HighVoltage;
            }
            set
            {
                _HighVoltage = value;
            }
        }

        public int BuzzerInterval
        {
            get
            {
                return _BuzzerInterval;
            }
            set
            {
                _BuzzerInterval = value;
            }
        }

        public int BuzzerDuration
        {
            get
            {
                return _BuzzerDuration;
            }
            set
            {
                _BuzzerDuration = value;
            }
        }

        #endregion
    }
}
