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
    public partial class ProjectType_Notification_Setitings_Entry : XtraForm
    {

       
       private ProjectType_Notification_Settings_View mainform = null;
        public ProjectType_Notification_Setitings_Entry(int UserId,Form CallingFrom)
        {
            InitializeComponent();
            mainform = CallingFrom as ProjectType_Notification_Settings_View;
        }

        private void ProjectType_Notification_Setitings_Entry_Load(object sender, EventArgs e)
        {

        }
        private void BindProjectType()
        {
            try
            {

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
