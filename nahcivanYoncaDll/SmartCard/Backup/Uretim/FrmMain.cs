using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SmartCard;
using System.Collections;


namespace OmniTest
{
    public partial class FrmMain : Form
    {

        #region değişkenler
            
            IKart kart = null;
        
            string sonuc = "";
            Int32 Issuer,Acma;
            UInt32 AnaKredi, YedekKredi;
            UInt32 CihazNo, AboneNo, Limit1, Limit2, KritikKredi, MekanikEndeks;
            UInt32 Fiyat1, Fiyat2, Fiyat3, Katsayi1, Katsayi2, Katsayi3, OverLimit;
            byte Cap, Tip, KartNo, DonemGun, VanaPulseSure, VanaCntSure, AvansOnay, AcmaX, KapamaX;
            byte Ceza4, Ako, Yko, KapamaEmri, Bayram1Gun, Bayram1Ay, Bayram1Sure, Bayram2Gun, Bayram2Ay, Bayram2Sure;
            string TarihBilgisi;
            DateTime dateTarih;
            ITarife trf;

        #endregion

        #region INIT
            
            public FrmMain()
            {
                InitializeComponent();
            }
         
            private void Form1_Load(object sender, EventArgs e)
            {
                InitDegerler();
            }
          

        #endregion

        #region Genel Fonksyonlar

            public void InitDegerler()
            {
                PortDoldur(cmbPorts);
                cmbReader.SelectedIndex = 1;
                cmbPorts.SelectedIndex = 0;
                tcDegiskenler.SelectedTab = tabPageAboneUst;
            }

            public void KartFactory(int index)
            {
                kart = null;

                if (index == 0) kart = new Kart(cmbPorts.SelectedItem.ToString(),Issuer);
                else if (index == 1) kart = new OmniKart(Issuer);
                
            }
        
            public void PortDoldur(ComboBox cmb)
            {
                for (int i = 1; i < 100; i++)
                {
                    cmb.Items.Add("COM" + i);
                }
            }

            public bool ComPortKontrol(ComboBox cmb)
            {
                if (cmb.SelectedIndex == -1)
                {
                    MessageBox.Show("Select COM PORT !");
                    return false;
                }
                else return true;
            }

            public void StatusTextBosalt()
            {
                statusText.Text = "";
                Application.DoEvents();
            }

            public void StatusYaz(string text)
            {
                statusText.Text = "";
                Application.DoEvents();
                statusText.Text = text;
            }

            private bool DegiskenlerinDegerleriniAl()
            {
                Issuer = Convert.ToInt32(txtIssuer.Text);
                CihazNo = Convert.ToUInt32(txtDevNo.Text);
                AnaKredi = Convert.ToUInt32(txtAnaKredi.Text);
                KritikKredi = Convert.ToUInt32(txtKritikKredi.Text);
                Katsayi1 = Convert.ToUInt32(txtKat1.Text);
                Katsayi2 = Convert.ToUInt32(txtKat2.Text);
                Katsayi3 = Convert.ToUInt32(txtKat3.Text);
                Limit1 = Convert.ToUInt32(txtUretimLim1.Text);
                Limit2 = Convert.ToUInt32(txtUretimLim2.Text);
                OverLimit = Convert.ToUInt32(txtOverLim.Text);

                if (rbAcma.Checked) Acma = 1;
                else if (rbAcmaX.Checked) Acma = 2;
                if (chbAcmaX.Checked) AcmaX = 0x05; else AcmaX = 0x00;
                if (chbKapamaX.Checked) KapamaX = 0x05; else KapamaX = 0x00;

                if (chkTarih.Checked) dateTarih = Convert.ToDateTime(dateTimePickerTarih.Value.ToShortDateString() + " " + txtSaat.Text);
                else dateTarih = DateTime.Now;


                #region tarife
                try
                {
                    trf = new Tarife();
                    trf.Fiyat1 = Convert.ToUInt32(txtFiyat1.Text);
                    trf.Fiyat2 = Convert.ToUInt32(txtFiyat2.Text);
                    trf.Fiyat3 = Convert.ToUInt32(txtFiyat3.Text);
                    trf.Lim1 = Convert.ToUInt32(txtLim1.Text);
                    trf.Lim2 = Convert.ToUInt32(txtLim2.Text);
                    trf.LoadLimit = Convert.ToUInt32(txtLoadLimit.Text);
                    trf.Aksam = Convert.ToUInt32(txtAksam.Text);
                    trf.Sabah = Convert.ToUInt32(txtSabah.Text);
                    trf.HaftaSonuAksam = Convert.ToUInt32(txtHaftaSonuAksam.Text);
                    trf.FixCharge = Convert.ToUInt32(txtFixCharge.Text);
                    //trf.OtherCharge = Convert.ToUInt32(txtOtherCharge.Text);
                    //trf.DailyDebt = Convert.ToUInt32(txtDailyDebt.Text);
                    //trf.TotalDebt = Convert.ToUInt32(txtTotalDebt.Text);
                    //trf.DebtMode = Convert.ToUInt32(txtDebtMode.Text);
                    //trf.LowVoltage = Convert.ToUInt16(txtOverVoltajMin.Text);
                    //trf.HighVoltage = Convert.ToUInt16(txtOverVoltajMax.Text);
                    //trf.BuzzerInterval = Convert.ToUInt16(txtBuzzerInterval.Text);
                    //trf.BuzzerDuration = Convert.ToUInt16(txtBuzzerDuration.Text);
                }
                catch (Exception ex)
                {
                    StatusYaz(ex.Message);
                    return false;
                }
                
                #endregion

                return true;
            }

            private bool DevNoKontrol(string deger)
            {
                if (string.IsNullOrEmpty(deger))
                {
                    MessageBox.Show("Please Input Numbers !");
                    return false;
                }

                if (!SayiKontrol(deger))
                {
                    MessageBox.Show("Please Input Numeric Numbers !");
                    return false;
                }

                return true;
            }

            public bool SayiKontrol(string sayi)
            {
                System.Text.RegularExpressions.Regex allow = new System.Text.RegularExpressions.Regex("^[0-9.]*$");
                if (!allow.IsMatch(sayi)) return false;
                else return true;
            }

      #endregion

        #region Üretim

            private void btnUretimForm_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.FormUret();
            }

            private void btnUretimCihazNo_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.CihazNo(CihazNo, KritikKredi, 0XFFFF, 0XFFFF, 0XFFFF, 0XFFFF);
            }

            private void btnUretimFormat_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.Format(CihazNo, KritikKredi, Katsayi1, Katsayi2, Katsayi3, Limit1, Limit2, OverLimit);
            }
            
            private void btnUretimAcma_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.UretimAc(Acma);
            }

            private void btnUretimKapat_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiKapat(CihazNo);
            }

            void btnUretimFormIssuer_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.FormIssuer(CihazNo);
            }

            private void btnUretimIssuer_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.Issuer();
            }

            private void btnUretimTestMode_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.TestMod(CihazNo);
            }

            private void btnUretimReelMode_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.ReelMod();
            }
       


      #endregion

        #region Ortak Fonksyonlar

            private void btnEject_Click(object sender, EventArgs e)
        {
            KartFactory(cmbReader.SelectedIndex);
            statusText.Text = kart.Eject();
        }

        #endregion

        #region Yetki İşlemleri

          
            
            private void btnYetkiForm_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.FormYet();
            }

            private void btnYetkiAc_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                if (cmbReader.SelectedIndex==2) statusText.Text = kart.YetkiAc(CihazNo);
                else statusText.Text = kart.YetkiAc(CihazNo);
            }

            private void btnYetkiKapat_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiKapat(CihazNo);
            }

            private void btnYetkiBilgiYap_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiBilgiYap();
            }

            private void btnYetkiBilgiOku_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiBilgiOku();

                if (statusText.Text.Substring(0, 1) == "1")
                {
                    FrmBilgiOku fBilgi = new FrmBilgiOku(statusText.Text.Substring(2));
                    fBilgi.ShowDialog();
                }
            }

            private void btnYetkiIadeYap_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiIadeYap(CihazNo);
            }

            private void btnYetkiIadeOku_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiIadeOku();

                if (statusText.Text.Substring(0, 1) == "1")
                {
                    FrmIade fIade = new FrmIade(statusText.Text.Substring(2));
                    fIade.ShowDialog();
                }

            }

            private void btnYetkiSaat_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                if (chkTarih.Checked)  statusText.Text = kart.YetkiSaat(dateTarih);
                else statusText.Text = kart.YetkiSaat();
            }

            private void btnYetkiEDegis_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.YetkiBilgiOku();

                if (statusText.Text.Length > 3)
                {
                    FrmInput input = new FrmInput();
                    input.ShowDialog();
                    if (!DevNoKontrol(input.deger)) return;

                    try
                    {
                        string datalar = statusText.Text.Substring(2);
                        string[] data = datalar.Split(new char[] { '_' });
                        //util.Uyar(datalar,"B");
                        //util.Uyar(data[12].PadLeft(2, '0') + "/" + data[13].PadLeft(2, '0') + "/20" + data[14], "W");
                        //Convert.ToDateTime(data[12].PadLeft(2, '0') + "/" + data[13].PadLeft(2, '0') + "/20" + data[14])
                       string st = kart.ETrans(Convert.ToUInt32(input.deger), Convert.ToDateTime(data[7]), Convert.ToUInt32(data[6]),
                                       Convert.ToUInt32(data[1]), Convert.ToUInt32(data[2]), Convert.ToByte(data[32]), Convert.ToByte(data[12]), Convert.ToByte(data[23]), Convert.ToByte(data[20]),
                                       Convert.ToByte(data[21]), Convert.ToByte(data[22]), Convert.ToByte(data[30]), Convert.ToUInt32(data[33]), Convert.ToUInt32(data[30]), Convert.ToUInt32(data[31]),
                                       Convert.ToUInt32(data[3]), Convert.ToUInt32(data[4]), Convert.ToUInt32(data[5]), Convert.ToUInt32(data[17]), Convert.ToUInt32(data[18]),
                                       Convert.ToUInt32(data[14]), Convert.ToUInt32(data[15]), Convert.ToUInt32(data[16]));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Info Card couldn't read !\r\n " + ex.Message);
                        return;
                    }
                }
            
            }

            private void btnYetkiIptal_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                if (chkTarih.Checked) statusText.Text = kart.YetkiIptal(CihazNo);
                else statusText.Text = kart.YetkiSaat();
            }

            #endregion

        #region Abone İşlemleri

            private void btnAboneYap_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.AboneYap(CihazNo,trf);
            }

            private void btnAboneOku_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.AboneOku();
                if (statusText.Text.Substring(0, 1) == "1")
                {
                    FrmAOku fAoku = new FrmAOku(statusText.Text.Substring(2));
                    fAoku.ShowDialog();
                }
            }

            private void btnKrediYaz_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.KrediYaz(CihazNo,AnaKredi,trf);
            }

            private void btnKrediOku_Click(object sender, EventArgs e)
            {
                if (!ComPortKontrol(cmbPorts)) return;
                if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                StatusTextBosalt();

                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.KrediOku();

                if (statusText.Text.Substring(0, 1) == "1")
                {
                    FrmKredi fKoku = new FrmKredi(statusText.Text.Substring(2));
                    fKoku.ShowDialog();
                }
            }

            private void btnAboneBosalt_Click(object sender, EventArgs e)
            {
                DialogResult dr = MessageBox.Show("Kart Boşaltılacak, Emin misiniz ?", "Kart Boşalt", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.OK)
                {
                    if (!ComPortKontrol(cmbPorts)) return;
                    if (!DegiskenlerinDegerleriniAl()) { StatusYaz("Değerler Hatalı !"); return; }
                    StatusTextBosalt();

                    KartFactory(cmbReader.SelectedIndex);
                    statusText.Text = kart.Bosalt();
                }
            }

        #endregion

            private void button1_Click(object sender, EventArgs e)
            {
                KartFactory(cmbReader.SelectedIndex);
                statusText.Text = kart.AboneOku();
            }

           

    }
}
