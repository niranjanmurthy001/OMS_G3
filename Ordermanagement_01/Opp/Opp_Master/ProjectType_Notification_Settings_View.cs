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
    public partial class ProjectType_Notification_Settings_View : XtraForm
    {
        public int User_Id { get; private set; }

        public ProjectType_Notification_Settings_View( int User_id)
        {
            InitializeComponent();
            this.User_Id = User_id;
        }

        private void ProjectType_Notification_Settings_View_Load(object sender, EventArgs e)
        {

        }

        private void btnAddnew_Click(object sender, EventArgs e)
        {
            ProjectType_Notification_Setitings_Entry ns = new ProjectType_Notification_Setitings_Entry(User_Id, this);
            ns.Show();
        }

    }
}
