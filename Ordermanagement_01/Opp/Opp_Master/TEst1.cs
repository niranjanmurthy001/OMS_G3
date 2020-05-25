using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class TEst1 : DevExpress.XtraEditors.XtraForm
    {
        public TEst1()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Master.Error_Field test = new Error_Field();
            test.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels tezt1 = new Error_Settingspanels();
            tezt1.Show();
        }
    }
}