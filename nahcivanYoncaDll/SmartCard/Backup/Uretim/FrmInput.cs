using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OmniTest
{
    public partial class FrmInput : Form
    {
        public string deger = "";

        public FrmInput()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (SayiKontrol(txtDeger.Text))
            {
                deger = txtDeger.Text;
                this.Close();
                Application.DoEvents();
            }
            else
                MessageBox.Show("Please Enter Numeric Numbers !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public bool SayiKontrol(string sayi)
        {
            System.Text.RegularExpressions.Regex allow = new System.Text.RegularExpressions.Regex("^[0-9.]*$");
            if (!allow.IsMatch(sayi)) return false;
            else return true;
        }
    }
}
