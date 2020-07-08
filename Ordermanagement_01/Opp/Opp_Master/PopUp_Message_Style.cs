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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class PopUp_Message_Style : XtraForm
    {
        private Timer timer;
        private int startPosX;
        private int startPosY;
        public PopUp_Message_Style()
        {
            InitializeComponent();
            TopMost = true;
            ShowInTaskbar = false;
            timer = new Timer();
            timer.Interval = 50;
            timer.Tick += timer1_Tick;

        }

        private void PopUp_Message_Style_Load(object sender, EventArgs e)
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            startPosX = Screen.PrimaryScreen.WorkingArea.Width- Width;
            startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(startPosX, startPosY);
            base.OnLoad(e);
            timer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            startPosY -= 50;
            if (startPosY < Screen.PrimaryScreen.WorkingArea.Height-50)
                timer.Stop();
            else
                SetDesktopLocation(startPosX, startPosY);
        }

        private void CloseAlert_Tick(object sender, EventArgs e)
        {
            if(Opacity > 0)
            {
                this.Opacity -= 0.01;
              
            }
            else
            {
                this.Close();

            }
        }

        private void pictureEdit2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
