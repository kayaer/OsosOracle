using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OmniTest
{
    public partial class FrmIade : Form
    {

        string datalar = "";

        public FrmIade(string bilgi)
        {
            InitializeComponent();
            datalar = bilgi;
        }

        private void FrmIade_Load(object sender, EventArgs e)
        {
            string[] data = datalar.Split(new char[] { '_' });

            txtDevNoIade.Text = data[1];
            txtKrediIade.Text = (Convert.ToDouble(data[2]) / 100).ToString(); ;
            txtHarcananKrediIade.Text = (Convert.ToDouble(data[3]) / 100).ToString();

            if (data[0] == "0") txtKartTakilimiIade.Text = "NO";
            if (data[0] == "1") txtKartTakilimiIade.Text = "YES";
        }
    }
}
