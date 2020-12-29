using Stimulsoft.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp
{
    public partial class stimultest : Form
    {
       

        public stimultest()
        {
            InitializeComponent();
        }

        private void stimultest_Load(object sender, EventArgs e)
        {
            int OrderId = 200226;
            StiReport Report = new StiReport();
            Report.Reset();
            Report.Dictionary.DataSources.Clear();
            Report.Load(@"C: \Users\SJeevan\Desktop\OMS_G3\Checklist_Report_Preview.mrt");
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Id"].Value = Convert.ToString(200226);
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Task"].Value = "3";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@ProductType_Abs_Id"].Value = "1";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Work_Type"].Value = "1";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Project_Type_Id"].Value = "1";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Client_Id"].Value = "4";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Sub_Client_Id"].Value = "24";
            Report.DataSources["Usp_CheckList_Report"].Parameters["@User_Id"].Value = "1";

            Report.Compile();
            Report.Render();
            Report.Show(true);
           // stiViewerControl1.Report = Report;
        }
    }
}
