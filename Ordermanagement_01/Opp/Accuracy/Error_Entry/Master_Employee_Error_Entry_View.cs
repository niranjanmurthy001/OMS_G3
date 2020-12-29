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

namespace Ordermanagement_01.Opp.Accuracy.Error_Entry
{
    public partial class Master_Employee_Error_Entry_View : XtraForm
    {
        public Master_Employee_Error_Entry_View()
        {
            InitializeComponent();
        }

        private void Employee_Error_Entry_View_Load(object sender, EventArgs e)
        {
            rb_Internal_Error.SelectedIndex = 0;
            navigationFrame1.SelectedPage = navigationPage1;
            lbl_Header.Text = "Internal Error Entry";
        }

        private void rb_Internal_Error_SelectedIndexChanged(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1;
            rb_External_Error.SelectedIndex = -1;
            
            lbl_Header.Text = "Internal Error Entry";
        }

        private void rb_External_Error_SelectedIndexChanged(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2;
            rb_Internal_Error.SelectedIndex = -1;
            lbl_Header.Text = "External Error Entry";
        }

       
            
        

        

        private void rb_External_Error_MouseClick(object sender, MouseEventArgs e)
        {
            rb_External_Error.SelectedIndex = 0;
        }

        private void rb_Internal_Error_MouseClick(object sender, MouseEventArgs e)
        {
            rb_Internal_Error.SelectedIndex = 0;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Accuracy.Error_Entry.Master_Employee_Error_Entry en = new Master_Employee_Error_Entry(1,1, 234204,1,1,"Drn101",this);
            this.Enabled = false;
            en.Show();
        }
    }
}
