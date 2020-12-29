namespace Ordermanagement_01.Gen_Forms
{
    partial class PopUp_Notify
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
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl();
            this.SuspendLayout();
            // 
            // alertControl1
            // 
            this.alertControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alertControl1.AppearanceCaption.Options.UseFont = true;
            this.alertControl1.AppearanceText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alertControl1.AppearanceText.Options.UseFont = true;
            this.alertControl1.AppearanceText.Options.UseTextOptions = true;
            this.alertControl1.AppearanceText.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Hide;
            this.alertControl1.AutoFormDelay = 20000;
            this.alertControl1.AutoHeight = true;
            this.alertControl1.ControlBoxPosition = DevExpress.XtraBars.Alerter.AlertFormControlBoxPosition.Right;
            this.alertControl1.FormShowingEffect = DevExpress.XtraBars.Alerter.AlertFormShowingEffect.MoveHorizontal;
            this.alertControl1.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alertControl1.LookAndFeel.SkinMaskColor2 = System.Drawing.Color.Red;
            this.alertControl1.ShowPinButton = false;
            this.alertControl1.ShowToolTips = false;
            // 
            // PopUp_Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 82);
            this.Name = "PopUp_Notify";
            this.Text = "PopUp_Notify";
            this.Load += new System.EventHandler(this.PopUp_Notify_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
    }
}