using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OmniTest
{
    public partial class FrmBilgiOku : Form
    {
        string datalar = "";

        public FrmBilgiOku(string bilgi)
        {
            InitializeComponent();
            datalar = bilgi;
        }

        private void FrmBilgiOku_Load(object sender, EventArgs e)
        {
            string[] data = datalar.Split(new char[] { '_' });


            txtDevNo.Text = data[0];
            txtKalan.Text = (Convert.ToDouble(data[1]) / 100).ToString();
            txtHarcanan.Text = (Convert.ToDouble(data[2]) / 100).ToString();
            txtK1Tuketim.Text = (Convert.ToDouble(data[3]) / 100).ToString();
            txtK2Tuketim.Text = (Convert.ToDouble(data[4]) / 100).ToString();
            txtK3Tuketim.Text = (Convert.ToDouble(data[5]) / 100).ToString();
            txtGercekTuketim.Text = (Convert.ToDouble(data[6]) / 100).ToString();
            txtSayacTarih.Text = data[7] + "." + data[8] + "." + data[9] + " " + data[10] + ":" + data[11];
            txtSonCezaTarih.Text = data[12] + "." + data[13] + "." + data[14];
            txtSonPulseTarih.Text = data[15] + "." + data[16] + "." + data[17];
            txtSonArizaTarih.Text = data[18] + "." + data[19] + "." + data[20];
            txtSonKrediTarih.Text = data[21] + "." + data[22] + "." + data[23];
            txtIslemNo.Text = data[24];
            txtHaftaninGunu.Text = data[25];
            txtFiyat1.Text = (Convert.ToDouble(data[26]) / 100).ToString();
            txtFiyat2.Text = (Convert.ToDouble(data[27]) / 100).ToString();
            txtFiyat3.Text = (Convert.ToDouble(data[28]) / 100).ToString();
            txtLim1.Text = (Convert.ToDouble(data[29]) / 1000).ToString();
            txtLim2.Text = (Convert.ToDouble(data[30]) / 1000).ToString();
            txtLoadLimit.Text = data[32];
            txtAksam.Text = data[33];
            txtSabah.Text = data[34];
            txtKademe.Text = data[35];

            if (data[36] == "1") chkKlemensCesa.Checked = true; else chkKlemensCesa.Checked = false;
            if (data[37] == "1") chkAriza.Checked = true; else chkAriza.Checked = false;
            if (data[38] == "1") chkPilZayif.Checked = true; else chkPilZayif.Checked = false;
            if (data[39] == "1") chkPilBitik.Checked = true; else chkPilBitik.Checked = false;

            txtEkimTuketim.Text = (Convert.ToDouble(data[40]) / 100).ToString();
            txtAralikTuketim.Text = (Convert.ToDouble(data[41]) / 100).ToString();
            txtHaftasonuAksam.Text = data[42];
            txtFixCharge.Text = (Convert.ToDouble(data[43]) / 100).ToString();
            txtTotalFix.Text = (Convert.ToDouble(data[44]) / 100).ToString();
            txtOtherCharge.Text = (Convert.ToDouble(data[45]) / 100).ToString();
            txtOtherTotalCharge.Text = (Convert.ToDouble(data[46]) / 100).ToString();
            //txtDailyDebt.Text = (Convert.ToDouble(data[47]) / 100).ToString();
            //txtRemainingDebt.Text = (Convert.ToDouble(data[48]) / 100).ToString();
            //txtTotalDebt.Text = (Convert.ToDouble(data[49]) / 100).ToString();
        }
    }
}
