using DevExpress.XtraBars.Alerter;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace Ordermanagement_01.Gen_Forms
{
    public partial class PopUp_Notify : XtraForm
    {
        public PopUp_Notify()
        {
            InitializeComponent();
        }

        private void PopUp_Notify_Load(object sender, EventArgs e)
        {

            //toastNotificationsManager1.ShowNotification("03209bd1 - 98ac - 4329 - 9ce1 - 25d558aecb0e");
            //AlertInfo info = new AlertInfo("Login Successfully", "",true);
          
            //info.Image = Properties.Resources.status3;
            //alertControl1.Show(this, info);

            //popup notifier
           // PopupNotifier notifier = new PopupNotifier();
           // notifier.TitleText = "Alert";
           // notifier.ContentText = "Login Successfully";
           // notifier.Image = Properties.Resources.Status2;
           //// notifier.BodyColor = Color.Blue;
           // notifier.TitleColor = Color.Black;
           // notifier.ContentColor = Color.Black;
           
           // notifier.Popup();

            //using Notify Icon
            notifyIcon1.BalloonTipTitle = "Alert";
            notifyIcon1.BalloonTipText = "Login Succesfully";
            notifyIcon1.Icon = SystemIcons.Information;
            notifyIcon1.ShowBalloonTip(9000);

        }

        private void alertControl1_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            e.AlertForm.BackColor = Color.Red;
        }
    }
}
