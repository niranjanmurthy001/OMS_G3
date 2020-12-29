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

            
            AlertInfo info1 = new AlertInfo("Alert", "Incorrect Credientials",true);
            info1.Image = Properties.Resources.Alertico;
            alertControl1.Show(this, info1);

            AlertInfo info2 = new AlertInfo("Warning", "Cannot Intiate these Process", true);
            info2.Image = Properties.Resources.Warningpng;
            alertControl1.Show(this, info2);

            AlertInfo info3 = new AlertInfo("Information", "Incorrect Information", true);
            info3.Image = Properties.Resources.Info_png;
            alertControl1.Show(this, info3);

            AlertInfo info4 = new AlertInfo("Delete", "OrderId:1001 Deleted Sucessfully", true);
            info4.Image = Properties.Resources.cancel ;
            alertControl1.Show(this, info4);

            AlertInfo info = new AlertInfo("Login Successfully","User", true);
            info.Image = Properties.Resources.ok_Icon;
            alertControl1.Show(this, info);

            //popup notifier
            //PopupNotifier notifier = new PopupNotifier();
            //notifier.TitleText = "Alert";
            //notifier.ContentText = "Login Successfully";
            //notifier.Image = Properties.Resources.Status2;
            //// notifier.BodyColor = Color.Blue;
            //notifier.TitleColor = Color.Black;
            //notifier.ContentColor = Color.Black;

            //notifier.Popup();

            

        }

        
    }
}
