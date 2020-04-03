namespace OmniTest
{
    partial class FrmIade
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelIadeIc = new System.Windows.Forms.Panel();
            this.txtKartTakilimiIade = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txtHarcananKrediIade = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.txtKrediIade = new System.Windows.Forms.TextBox();
            this.txtDevNoIade = new System.Windows.Forms.TextBox();
            this.panelIadeIc.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelIadeIc
            // 
            this.panelIadeIc.BackColor = System.Drawing.Color.White;
            this.panelIadeIc.Controls.Add(this.txtKartTakilimiIade);
            this.panelIadeIc.Controls.Add(this.label33);
            this.panelIadeIc.Controls.Add(this.txtHarcananKrediIade);
            this.panelIadeIc.Controls.Add(this.label34);
            this.panelIadeIc.Controls.Add(this.label35);
            this.panelIadeIc.Controls.Add(this.label36);
            this.panelIadeIc.Controls.Add(this.txtKrediIade);
            this.panelIadeIc.Controls.Add(this.txtDevNoIade);
            this.panelIadeIc.Location = new System.Drawing.Point(0, 0);
            this.panelIadeIc.Name = "panelIadeIc";
            this.panelIadeIc.Size = new System.Drawing.Size(216, 125);
            this.panelIadeIc.TabIndex = 1;
            // 
            // txtKartTakilimiIade
            // 
            this.txtKartTakilimiIade.Location = new System.Drawing.Point(112, 85);
            this.txtKartTakilimiIade.Multiline = true;
            this.txtKartTakilimiIade.Name = "txtKartTakilimiIade";
            this.txtKartTakilimiIade.Size = new System.Drawing.Size(90, 20);
            this.txtKartTakilimiIade.TabIndex = 12;
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(9, 87);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(106, 20);
            this.label33.TabIndex = 13;
            this.label33.Text = "Card Mounted :";
            // 
            // txtHarcananKrediIade
            // 
            this.txtHarcananKrediIade.Location = new System.Drawing.Point(112, 62);
            this.txtHarcananKrediIade.Multiline = true;
            this.txtHarcananKrediIade.Name = "txtHarcananKrediIade";
            this.txtHarcananKrediIade.Size = new System.Drawing.Size(90, 20);
            this.txtHarcananKrediIade.TabIndex = 7;
            // 
            // label34
            // 
            this.label34.Location = new System.Drawing.Point(19, 64);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(89, 20);
            this.label34.TabIndex = 14;
            this.label34.Text = "Spent NIS :";
            // 
            // label35
            // 
            this.label35.Location = new System.Drawing.Point(47, 39);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(61, 20);
            this.label35.TabIndex = 15;
            this.label35.Text = "NIS   :";
            // 
            // label36
            // 
            this.label36.Location = new System.Drawing.Point(46, 17);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(66, 20);
            this.label36.TabIndex = 16;
            this.label36.Text = "Dev No  :";
            // 
            // txtKrediIade
            // 
            this.txtKrediIade.Location = new System.Drawing.Point(112, 39);
            this.txtKrediIade.Multiline = true;
            this.txtKrediIade.Name = "txtKrediIade";
            this.txtKrediIade.Size = new System.Drawing.Size(90, 20);
            this.txtKrediIade.TabIndex = 2;
            // 
            // txtDevNoIade
            // 
            this.txtDevNoIade.Location = new System.Drawing.Point(112, 16);
            this.txtDevNoIade.Multiline = true;
            this.txtDevNoIade.Name = "txtDevNoIade";
            this.txtDevNoIade.Size = new System.Drawing.Size(90, 20);
            this.txtDevNoIade.TabIndex = 1;
            // 
            // FrmIade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(217, 124);
            this.Controls.Add(this.panelIadeIc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmIade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Iade";
            this.Load += new System.EventHandler(this.FrmIade_Load);
            this.panelIadeIc.ResumeLayout(false);
            this.panelIadeIc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelIadeIc;
        private System.Windows.Forms.TextBox txtKartTakilimiIade;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtHarcananKrediIade;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtKrediIade;
        private System.Windows.Forms.TextBox txtDevNoIade;
    }
}