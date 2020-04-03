using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OmniTest
{
    public partial class FrmKredi : Form
    {
        string datalar = "";


        public FrmKredi(string bilgi)
        {
            InitializeComponent();
            datalar = bilgi;
        }

        private void FrmKredi_Load(object sender, EventArgs e)
        {
            string[] data = datalar.Split(new char[] { '_' });

            txtDevNo.Text = data[0];
            txtAnaKredi.Text = (Convert.ToDouble(data[1]) / 100).ToString();
            lblAko.Text = data[2];
            lblYko.Text = data[3];
            if (data[4] == "1") chkYeniKart.Checked = true; else chkYeniKart.Checked = false;
            txtFixCharge.Text = (Convert.ToDouble(data[5]) / 100).ToString();
            txtTotalFix.Text = (Convert.ToDouble(data[6]) / 100).ToString();
            txtYedek.Text = "30";
        }
    }
}
