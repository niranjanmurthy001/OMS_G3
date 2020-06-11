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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Order_SourceType_View : DevExpress.XtraEditors.XtraForm
    {
        int User_Id;
        string _BtnName;
        int _ProjectId;
        string _SourceType;
        int _ProductId;
        private int User_Role;
        string Operation_Type;

        public Order_SourceType_View(int User_Role)
        {
            InitializeComponent();
            User_Id = User_Role;
        }

        private void gridViewSource_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void Order_SourceType_View_Load(object sender, EventArgs e)
        {
            btn_Delete_MultipleSource.Visible = false;
            BindSourceTypes();
        }
        public async void BindSourceTypes()
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
                    var response = await httpClient.PostAsync(Base_Url.Url + "/OrderSourceType/BindSourceTypes", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_SourceType.DataSource = dt;                               
                            }
                            else
                            {
                                grd_SourceType.DataSource = null;
                            }
                        }
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

      
      
        private void btn_Add_NewSource_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Master.Order_SourceType_Entry OrderSourceEntry = new Order_SourceType_Entry(Operation_Type,_ProjectId, _ProductId, _SourceType, _BtnName, User_Id,this);
            OrderSourceEntry.Show();
        }

        private async void btn_Delete_MultipleSource_Click(object sender, EventArgs e)
        {
            if (gridViewSource.SelectedRowsCount != 0)
            {
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridViewSource.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridViewSource.GetDataRow(gridViewSelectedRows[i]);
                            int Source_Id = int.Parse(row["Employee_Source_id"].ToString());
                            var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "DELETE" },
                    { "@Employee_Source_id", Source_Id }
                };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/OrderSourceType/Delete", data);
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
                        BindSourceTypes();
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
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Any Record To Delete");
            }
        }

        private void gridViewSource_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridViewSource.SelectedRowsCount != 0)
            {
                btn_Delete_MultipleSource.Visible = true;
            }
            else
            {
                btn_Delete_MultipleSource.Visible = false;
            }
        }

        private async void repositoryItemHyperLinkEdit2_Click(object sender, EventArgs e)
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
                    System.Data.DataRow row = gridViewSource.GetDataRow(gridViewSource.FocusedRowHandle);
                    int Source_Id = int.Parse(row["Employee_Source_id"].ToString());
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "DELETE" },
                    { "@Employee_Source_id", Source_Id }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/OrderSourceType/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                            }
                            BindSourceTypes();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    throw ex;
                }
            }
            else if (show == DialogResult.No)
            {
            }

        }

        private async void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridViewSource.GetDataRow(gridViewSource.FocusedRowHandle);
            string _btnName = "Edit";
            int _projectId = int.Parse(row["Project_Type_Id"].ToString());
            int _productId = int.Parse(row["ProductType_Id"].ToString());
            string _sourceType = row["Employee_source"].ToString();
            int user_Id = User_Role;
            string operation_Type = "View";
            //GridView view = grd_SourceType.MainView as GridView;
            //var index = view.GetDataRow(view.GetSelectedRows()[0]);          
            //_BtnName = "Edit";
            //_ProjectId = Convert.ToInt32(index.ItemArray[1]);
            //_ProductId = Convert.ToInt32(index.ItemArray[2]);
            //_SourceType = index.ItemArray[3].ToString();
            Ordermanagement_01.Opp.Opp_Master.Order_SourceType_Entry SourceEntry = new Order_SourceType_Entry(operation_Type, _projectId, _productId, _sourceType, _btnName, user_Id, this);
            SourceEntry.Show();

        }
    }
}