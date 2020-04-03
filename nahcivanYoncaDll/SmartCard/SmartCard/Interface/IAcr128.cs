using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCard.Interface
{
    public interface IAcr128
    {
        int InitKart();

        string FinishKart();

        string HataSet(int Kod);
    }
}
