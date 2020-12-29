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
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;


namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Source_Type : DevExpress.XtraEditors.XtraForm
    {
        
        int User_Id, _ProjectId , _ID;
        string Operation_Type, _BtnName, _SourceType; 
        public Efficiency_Source_Type(int User_ID)
        {
            InitializeComponent();
            User_Id = User_ID;
        }

        private void gridView_Efficiency_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        public async void BindEfficiencySource()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencySource/BindGrid", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Efficiency.DataSource = dt;
                            }
                            else
                            {
                                grd_Efficiency.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show( "Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void Efficiency_Source_Type_Load(object sender, EventArgs e)
        {
            btn_Delete_Multiple_Efficiency.Visible = false;
            BindEfficiencySource();
        }

        private void btn_Add_New_Eff_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Source_Type_Entry Eff_Source = new Efficiency_Source_Type_Entry(Operation_Type,_ID, _ProjectId, _SourceType, _BtnName, User_Id, this);
            this.Enabled = false;
            Eff_Source.Show();
        }

        private async void btn_Delete_Multiple_Efficiency_Click(object sender, EventArgs e)
        {
            if (gridView_Efficiency.SelectedRowsCount != 0)
            {
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView_Efficiency.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView_Efficiency.GetDataRow(gridViewSelectedRows[i]);
                            int ID = int.Parse(row["Order_Source_Type_ID"].ToString());                          
                            var dictionary = new Dictionary<string, object>()
                            {
                                { "@Trans", "DELETE" },
                                { "@Order_Source_Type_ID", ID },
                                {"@Modified_By",User_Id }                  
                           };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencySource/Delete", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                    }
                                }
                            }
                        }
                        SplashScreenManager.CloseForm(false);
                        btn_Delete_Multiple_Efficiency.Visible = false;
                        XtraMessageBox.Show("Record Deleted Successfully");
                        BindEfficiencySource();
                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show( "Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }

                }
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Any Record To Delete");
            }
        }

        private void btn_Export_Eff_Click(object sender, EventArgs e)
        {
            if (gridView_Efficiency.RowCount > 0)
            {
                string filePath = @"C:\Efficiency Source Type\";
                string fileName = filePath + "Efficiency Source Type-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                grd_Efficiency.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show( "No data to export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void gridView_Efficiency_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "View")
            {
                System.Data.DataRow row = gridView_Efficiency.GetDataRow(gridView_Efficiency.FocusedRowHandle);
                string _btnName = "Edit";
                int _ID = int.Parse(row["Order_Source_Type_ID"].ToString());
                int _projectId = int.Parse(row["Project_Type_Id"].ToString());               
                string _sourceType = row["Order_Source_Type_Name"].ToString();
                int user_Id = User_Id;
                string operation_Type = "View";
                Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Source_Type_Entry Eff_Source = new Efficiency_Source_Type_Entry(operation_Type, _ID, _projectId, _sourceType, _btnName, user_Id, this);
                this.Enabled = false;
                Eff_Source.Show();
            }
        }

        private void gridView_Efficiency_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView_Efficiency.SelectedRowsCount != 0)
            {
                btn_Delete_Multiple_Efficiency.Visible = true;
            }
            else
            {
                btn_Delete_Multiple_Efficiency.Visible = false;
            }
        }
    }
}