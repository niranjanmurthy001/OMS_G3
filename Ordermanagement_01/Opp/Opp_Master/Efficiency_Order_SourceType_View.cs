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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Efficiency_Order_SourceType_View : DevExpress.XtraEditors.XtraForm
    {
         int User_Id;
         int User_Role;
         string operation_Type;
         int _projectId;
         int _sourceId;
         int _stateId;
         int _countyId;
         string _btnName;

        public Efficiency_Order_SourceType_View(int User_Role)
        {
            InitializeComponent();
            User_Id = User_Role;
        }

     
        private void gridView_Efficiency_Src_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void btn_Add_New_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Master.Efficiency_Order_SourceType_Entry Efficiency = new Efficiency_Order_SourceType_Entry(User_Role, this);
            Efficiency.Show();
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            //System.Data.DataRow row = gridView_Efficiency_Src.GetDataRow(gridView_Efficiency_Src.FocusedRowHandle);
            //string _btnName = "Edit";
            //int _projectId = int.Parse(row["Project_Type_Id"].ToString());
            //int _sourceId = int.Parse(row["Employee_Source_id"].ToString());
            //int _stateId = int.Parse(row["State_ID"].ToString());
            //int _countyId = int.Parse(row["County_ID"].ToString());
            //int User_Id = User_Role;
            //string operation_Type = "View";         
            //Ordermanagement_01.Opp.Opp_Master.Efficiency_Order_SourceType_Entry Efficiency = new Efficiency_Order_SourceType_Entry(operation_Type, _projectId, _sourceId, _stateId, _countyId, _btnName, User_Id);
            //Efficiency.Show();

        }

        private void Efficiency_Order_SourceType_View_Load(object sender, EventArgs e)
        {
            BindGrid();
            btn_Delete_Multiple.Visible = true;
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
                //throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
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

        private async void btn_Delete_Multiple_Click(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Delete Record";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    List<int> gridViewSelectedRows = gridView_Efficiency_Src.GetSelectedRows().ToList();
                    for (int i = 0; i < gridViewSelectedRows.Count; i++)
                    {
                        DataRow row = gridView_Efficiency_Src.GetDataRow(gridViewSelectedRows[i]);
                        int Source_Id = int.Parse(row["EmpEff_OrderSourceType_Id"].ToString());
                        var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "DELETE" },
                    { "@EmpEff_OrderSourceType_Id", Source_Id }
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
            else if (show == DialogResult.No)
            {
               
            }
        }
    }
}