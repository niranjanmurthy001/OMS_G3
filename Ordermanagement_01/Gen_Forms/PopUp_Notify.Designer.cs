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
            this.components = new System.ComponentModel.Container();
            this.toastNotificationsManager1 = new DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager(this.components);
            this.alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.toastNotificationsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // toastNotificationsManager1
            // 
            this.toastNotificationsManager1.ApplicationId = "89651b4f-1117-47ef-92d5-77191c3b0955";
            this.toastNotificationsManager1.ApplicationName = "Ordermanagement_01";
            this.toastNotificationsManager1.Notifications.AddRange(new DevExpress.XtraBars.ToastNotifications.IToastNotificationProperties[] {
            new DevExpress.XtraBars.ToastNotifications.ToastNotification("03209bd1-98ac-4329-9ce1-25d558aecb0e", global::Ordermanagement_01.Properties.Resources.Status, "", "Login Successfully", "", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText01)});
            // 
            // alertControl1
            // 
            this.alertControl1.AppearanceCaption.BackColor = System.Drawing.Color.Lime;
            this.alertControl1.AppearanceCaption.BackColor2 = System.Drawing.Color.Blue;
            this.alertControl1.AppearanceCaption.BorderColor = System.Drawing.Color.White;
            this.alertControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alertControl1.AppearanceCaption.Image = global::Ordermanagement_01.Properties.Resources.status3;
            this.alertControl1.AppearanceCaption.Options.UseImage = true;
            this.alertControl1.AppearanceText.TextOptions.HotkeyPrefix = DevExpress.Utils.HKeyPrefix.Hide;
            this.alertControl1.AutoFormDelay = 6000;
            this.alertControl1.AutoHeight = true;
            this.alertControl1.ControlBoxPosition = DevExpress.XtraBars.Alerter.AlertFormControlBoxPosition.Right;
            this.alertControl1.FormShowingEffect = DevExpress.XtraBars.Alerter.AlertFormShowingEffect.MoveHorizontal;
            this.alertControl1.LookAndFeel.SkinMaskColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.alertControl1.LookAndFeel.SkinMaskColor2 = System.Drawing.Color.Red;
            this.alertControl1.ShowPinButton = false;
            this.alertControl1.ShowToolTips = false;
            this.alertControl1.BeforeFormShow += new DevExpress.XtraBars.Alerter.AlertFormEventHandler(this.alertControl1_BeforeFormShow);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // PopUp_Notify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 82);
            this.Name = "PopUp_Notify";
            this.Text = "PopUp_Notify";
            this.Load += new System.EventHandler(this.PopUp_Notify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.toastNotificationsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager toastNotificationsManager1;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}