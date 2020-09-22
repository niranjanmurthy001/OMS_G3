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
using System.IO;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Order_Source_Type : DevExpress.XtraEditors.XtraForm
    {
        int User_Id;
        int User_Role;
        public Efficiency_Order_Source_Type(int User_ID)
        {
            InitializeComponent();
            User_Id = User_ID;
        }

        private void Efficiency_Order_Source_Type_Load(object sender, EventArgs e)
        {
            BindGrid();
            btn_Delete_Multiple.Visible = false;
                     
        }
        public async void BindGrid()
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
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/BindGrid", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Efficiency_Src.DataSource = dt;
                            }
                            else
                            {
                                grd_Efficiency_Src.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void btn_Delete_Multiple_Click(object sender, EventArgs e)
        {

            if (gridView_Efficiency_Src.SelectedRowsCount != 0)
            {
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView_Efficiency_Src.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView_Efficiency_Src.GetDataRow(gridViewSelectedRows[i]);
                            //int Source_Id = int.Parse(row["EmpEff_OrderSourceType_Id"].ToString());
                            int ProjectID = int.Parse(row["Project_Type_Id"].ToString());
                            int SourceId = int.Parse(row["Employee_Source_id"].ToString());
                            int StateId = int.Parse(row["State_ID"].ToString());
                            int CountyId = int.Parse(row["County_ID"].ToString());
                            var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "DELETE" },
                    { "@Project_Type_Id", ProjectID },
                    { "@Employee_Source_id", SourceId },
                    { "@State_ID", StateId },
                    { "@County_ID", CountyId },
                    {"@Modified_By" ,User_Id}
                };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/Delete", data);
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
                        XtraMessageBox.Show("Record Deleted Successfully");
                        BindGrid();
                        btn_Delete_Multiple.Visible = false;
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

        private void btn_Export_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\Efficiency Order Source Type\";
            string fileName = filePath + "Efficiency Order Source Type-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            gridView_Efficiency_Src.ExportToXlsx(fileName);
            System.Diagnostics.Process.Start(fileName);
        }

        private void btn_Add_New_Click(object sender, EventArgs e)
        {
            this.Enabled = false;             
            Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Order_Source_Type_Entry Efficiency = new Efficiency_Order_Source_Type_Entry(User_Id, this);
            Efficiency.Show();         
        }

        private void gridView_Efficiency_Src_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView_Efficiency_Src_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView_Efficiency_Src.SelectedRowsCount != 0)
            {
                btn_Delete_Multiple.Visible = true;
            }
            else
            {
                btn_Delete_Multiple.Visible = false;
            }
        }
    }
}