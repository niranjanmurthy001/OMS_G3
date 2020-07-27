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
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;
using Newtonsoft.Json;
using System.Net.Http;
using Ordermanagement_01.Models;
using System.Net;
using System.IO;
using DevExpress.XtraGrid.Columns;

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class Checklist_Settings_View : DevExpress.XtraEditors.XtraForm
    {
        DataTable _dtUpdatecell;
        public Checklist_Settings_View()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void Checklist_Settings_View_Load(object sender, EventArgs e)
        {
            BindDataToGrid();
            btn_Update.Visible = false;
        }
        public async void BindDataToGrid()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_TO_GRID" }

                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                grd_Data.DataSource = dt;
                            }
                        }
                    }
                    else
                    {
                        grd_Data.DataSource = null;
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void brn_AddNew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_CheckList.Checklist_Settings_Entry chk_Entry = new Checklist_Settings_Entry();
            chk_Entry.Show();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (grd_Data.DataSource != null)
            {
                string filePath = @"C:\OMS\";
                string fileName = filePath + "Checklist-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    GridColumn colModelPrice = gridView1.Columns[i];
                    colModelPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    colModelPrice.DisplayFormat.FormatString = "D";
                }
                grd_Data.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            btn_Update.Visible = true;
            _dtUpdatecell = ((DataView)gridView1.DataSource).Table;
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {

        }
    }
}