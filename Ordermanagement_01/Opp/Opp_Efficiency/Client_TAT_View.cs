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
using Newtonsoft.Json;
using System.Net;
using Ordermanagement_01.Models;
using System.Net.Http;
using Ordermanagement_01.Masters;
using System.IO;
using DevExpress.XtraGrid.Columns;
using DevExpress.Office.Crypto;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Client_TAT_View : DevExpress.XtraEditors.XtraForm
    {
        int _userid;
        int _ProjectId;
        DataTable _dt;
        string _UserRole;
        public Client_TAT_View(int USER_ID, string USER_ROLE)
        {
            InitializeComponent();
            _userid = USER_ID;
            _UserRole = USER_ROLE;
        }

        private void Client_TAT_View_Load(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            btn_Delete.Visible = false;
            BindProjectType();
            ddl_ProjectType.EditValue = 1;
            SplashScreenManager.CloseForm(false);
        }
        private async void BindProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_PROJECT_TYPE" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ClientTAT/BindProject", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[1] = "SELECT";
                                dt.Rows.InsertAt(dr, 0);
                            }
                            ddl_ProjectType.Properties.DataSource = dt;
                            ddl_ProjectType.Properties.DisplayMember = "Project_Type";
                            ddl_ProjectType.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddl_ProjectType.Properties.Columns.Add(col);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
               BindClientTAT();
            }
            else
            {
                grd_ClientTAT.DataSource = null;
            }
        }

        public async void BindClientTAT()
        {
            try
            {
                _dt = new DataTable();
                grd_ClientTAT.DataSource = null;
                gridView_ClientTAT.Columns.Clear();
                int Proj_id = Convert.ToInt32(ddl_ProjectType.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_DATA_TO_GRID" },
                    {"@Project_Type_Id",Proj_id}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ClientTAT/BindDataGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dt = JsonConvert.DeserializeObject<DataTable>(result);

                            if (_dt.Rows.Count > 0)
                            {
                                grd_ClientTAT.DataSource = _dt;
                                gridView_ClientTAT.Columns[1].Visible = false;
                                gridView_ClientTAT.Columns[4].Visible = false;
                               // gridView_ClientTAT.Columns[6].Visible = false;
                                if (_UserRole=="1")
                                {
                                    gridView_ClientTAT.Columns[0].Visible = true;
                                    gridView_ClientTAT.Columns[2].Visible = false;
                                    gridView_ClientTAT.Columns[3].Visible = true;
                                    gridView_ClientTAT.Columns[5].Visible = false;
                                }
                                else                                
                                {
                                    gridView_ClientTAT.Columns[2].Visible = true;
                                    gridView_ClientTAT.Columns[0].Visible = false;
                                    gridView_ClientTAT.Columns[5].Visible = true;
                                    gridView_ClientTAT.Columns[3].Visible = false;
                                    gridView_ClientTAT.Columns[2].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                    gridView_ClientTAT.Columns[5].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                }
                                for (int i = 0; i < gridView_ClientTAT.Columns.Count; i++)
                                {
                                    gridView_ClientTAT.Columns[i].AppearanceHeader.Font = new Font(gridView_ClientTAT.Columns[i].AppearanceHeader.Font, FontStyle.Bold);
                                    gridView_ClientTAT.Columns[i].AppearanceHeader.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView_ClientTAT.Columns[i].AppearanceCell.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView_ClientTAT.Columns[i].OptionsColumn.AllowEdit = false;                                   
                                    if (i > 5)
                                    {
                                        gridView_ClientTAT.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        gridView_ClientTAT.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                      
                                    }                                 
                                }
                            }
                        }
                    }                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }

        private void btn_AddNew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Client_TAT_Entry Entry = new Ordermanagement_01.Opp.Opp_Efficiency.Client_TAT_Entry(this, _userid, _UserRole);
            this.Enabled = false;
            Entry.Show();                        
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {         
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
                if (grd_ClientTAT.DataSource != null)
                {
                    string filePath = @"C:\OPP\";
                    string fileName = filePath + "Client TAT-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    for (int i = 6; i < gridView_ClientTAT.Columns.Count; i++)
                    {
                        GridColumn colModelPrice = gridView_ClientTAT.Columns[i];                      
                        //gridView_ClientTAT.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colModelPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        colModelPrice.DisplayFormat.FormatString = "D";
                    }
                    grd_ClientTAT.ExportToXlsx(fileName);
                    System.Diagnostics.Process.Start(fileName);
                }
                else
                {
                    XtraMessageBox.Show( "No Data To Export", "Note", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            else
            {
                XtraMessageBox.Show( "Please Select Project Type To Export", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridView_ClientTAT_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView_ClientTAT.SelectedRowsCount > 0)
            {
                btn_Delete.Visible = true;
            }
            else
            {
                btn_Delete.Visible = false;
            }
        }

        private void gridView_ClientTAT_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private async void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                _ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {
                    if (gridView_ClientTAT.SelectedRowsCount > 0)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView_ClientTAT.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView_ClientTAT.GetDataRow(gridViewSelectedRows[i]);
                            int Client_Id = int.Parse(row["Client_Id"].ToString());
                            int Subprocess_Id = int.Parse(row["Subprocess_Id"].ToString());
                            var dictionary = new Dictionary<string, object>()
                            {
                                { "@Trans", "DELETE" },
                                { "@Client_Id",Client_Id },
                                {"@Subprocess_Id",Subprocess_Id },
                                {"@Modified_By",_userid }
                            };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/ClientTAT/Delete", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        SplashScreenManager.CloseForm(false);
                                    }
                                }
                            }
                        }

                        XtraMessageBox.Show("Record Deleted Successfully");
                        BindClientTAT();
                        btn_Delete.Visible = false;
                    }
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show( "Please Select Any Record To Delete", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }          
        }


    }
}