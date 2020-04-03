using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OmniTest
{
    public partial class FrmAOku : Form
    {

        string datalar = "";

        public FrmAOku(string bilgi)
        {
            InitializeComponent();
            datalar = bilgi;
        }

        private void FrmAOku_Load(object sender, EventArgs e)
        {
            string[] data = datalar.Split(new char[] { '_' });

            txtDevNo.Text = data[0];
            txtAnaKredi.Text = (Convert.ToDouble(data[1]) / 100).ToString();
            txtYedek.Text = "30";
            lblAko.Text = data[2];
            lblYko.Text = data[3];
            if (data[4] == "1") chkYeniKart.Checked = true; else chkYeniKart.Checked = false;

            txtKalan.Text = (Convert.ToDouble(data[5]) / 100).ToString();
            txtHarcanan.Text = (Convert.ToDouble(data[6]) / 100).ToString();
            txtSayacTarih.Text = data[7];

            if (data[8] == "1") chkKlemensCesa.Checked = true; else chkKlemensCesa.Checked = false;
            if (data[9] == "1") chkAriza.Checked = true; else chkAriza.Checked = false;
            if (data[10] == "1") chkPilZayif.Checked = true; else chkPilZayif.Checked = false;
            if (data[11] == "1") chkPilBitik.Checked = true; else chkPilBitik.Checked = false;

            txtK1Tuketim.Text = (Convert.ToDouble(data[12]) / 100).ToString();
            txtK2Tuketim.Text = (Convert.ToDouble(data[13]) / 100).ToString();
            txtK3Tuketim.Text = (Convert.ToDouble(data[14]) / 100).ToString();
            txtGercekTuketim.Text = (Convert.ToDouble(data[15]) / 100).ToString();
            txtEkimTuketim.Text = (Convert.ToDouble(data[16]) / 100).ToString();
            txtAralikTuketim.Text = (Convert.ToDouble(data[17]) / 1000).ToString();
            txtFiyat1.Text = (Convert.ToDouble(data[18]) / 100).ToString();
            txtFiyat2.Text = (Convert.ToDouble(data[19]) / 100).ToString();
            txtFiyat3.Text = (Convert.ToDouble(data[20]) / 100).ToString();
            txtLim1.Text = (Convert.ToDouble(data[21]) / 1000).ToString();
            txtLim2.Text = (Convert.ToDouble(data[22]) / 1000).ToString();
            txtLoadLimit.Text = data[23];
            txtAksam.Text = data[24];
            txtSabah.Text = data[25];
            txtKademe.Text = data[26];
            txtHaftasonuAksam.Text = data[27];
            txtFixCharge.Text = (Convert.ToDouble(data[28]) / 100).ToString();
            txtTotalFix.Text = (Convert.ToDouble(data[29]) / 100).ToString();
            //txtOtherCharge.Text = (Convert.ToDouble(data[30]) / 100).ToString();
            //txtOtherTotalCharge.Text = (Convert.ToDouble(data[31]) / 100).ToString();
            //txtDailyDebt.Text = (Convert.ToDouble(data[32]) / 100).ToString();
            //txtTotalDebt.Text = (Convert.ToDouble(data[33]) / 100).ToString();
            //txtRemainingDebt.Text = (Convert.ToDouble(data[34]) / 100).ToString();

            //txtIslemNo.Text = data[24];
            //txtHaftaninGunu.Text = data[25];

            //txtReverse.Text = data[35];
            //txtBuzzerInterval.Text = data[36];
            //txtBuzzerDuration.Text = data[37];
            //txtLowVoltage.Text = (Convert.ToDouble(data[38]) / 100).ToString();
            //txtHighVoltage.Text = (Convert.ToDouble(data[39]) / 100).ToString();
        }
    }
}
