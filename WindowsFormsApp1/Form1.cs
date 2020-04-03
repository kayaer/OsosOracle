using SmartCard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KartIslem asd = new KartIslem();

            //var t = asd.KartTipi();
            //var asf = asd.KrediOku();



            var a = asd.AboneYazSu(20191004, 1, 1, 1, 1, 1, 1, 1);
            MessageBox.Show(a);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Mercan mercan = new Mercan();
          var h=  mercan.AboneOku();

            var a = mercan.KartTipi();
            MessageBox.Show(a);
        }
    }
}
