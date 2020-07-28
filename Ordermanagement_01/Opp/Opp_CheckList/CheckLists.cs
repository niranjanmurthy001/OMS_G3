using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using System.Windows.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Columns;
using System.Linq;

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckLists : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_tabName;
        int UserId, ProjectType_Id, OrderTypeAbs_Id, OrderId, ClientId, SubClientId, OrderTask, WorkType_Id;     
        int tabid;
        DataTable dt1;


        public CheckLists(int User_Id, int Project_Type_Id, int Product_Type_Id, int Order_Id, int Client_Id,int SubClient_Id,int Order_Task, int Work_Type_Id)
        {
            InitializeComponent();
            UserId = User_Id;
            ProjectType_Id = Project_Type_Id;
            OrderTypeAbs_Id = Product_Type_Id;
            OrderId = Order_Id;
            ClientId = Client_Id;
            SubClientId = SubClient_Id;
            OrderTask = Order_Task;
            WorkType_Id = Work_Type_Id;
        }

        private void CheckLists_Load(object sender, EventArgs e)
        {
            BindTabs();
        }

        private async void BindTabs()
        {
            try
            {
                dt_tabName = new DataTable();
                tabPane1.Pages.Clear();             
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_TABS" },
                    {"@Project_Type_Id" ,ProjectType_Id},
                    {"@ProductType_Abs_Id",OrderTypeAbs_Id }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckLists/Bindtabs", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string Col_Name = dt.Rows[i]["Checklist_Master_Type"].ToString();
                                string name = "tabnav" + i;
                                int Id = Convert.ToInt32(dt.Rows[i]["ChecklistType_Id"].ToString());
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var dictonary1 = new Dictionary<string, object>()
                                    {
                                        {"@Trans","GET_COUNT" },
                                        {"@Ref_Checklist_Master_Type_Id",Id }

                                    };
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/CheckLists/BindTabNames", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            DataTable dtcount = JsonConvert.DeserializeObject<DataTable>(result1);
                                            if (dtcount != null && dtcount.Rows.Count > 0)
                                            {
                                                int count = Convert.ToInt32(dtcount.Rows[0]["Count"].ToString());
                                                if (count > 0)
                                                {
                                                    tabPane1.AddPage(Col_Name, name);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        private void tabPane1_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
           if (tabPane1.Pages.Count > 0)
            {              
                    string tabname = tabPane1.SelectedPage.Caption;
                    GetTabId(tabname); 
            }
        }
        private async void GetTabId(string tabname)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","GET_TAB_ID" },
                    {"@Checklist_Master_Type",tabname },
                    {"@Project_Type_Id",ProjectType_Id },
                    {"@ProductType_Abs_Id",OrderTypeAbs_Id }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckLists/GetTabIndex", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                tabid = Convert.ToInt32(dt.Rows[0]["ChecklistType_Id"].ToString());
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var dictonary1 = new Dictionary<string, object>()
                                {
                                    {"@Trans","BIND_GRID_DATA" },
                                    {"@Ref_Checklist_Master_Type_Id",tabid },
                                    {"@Project_Type_Id",ProjectType_Id },
                                    {"@ProductType_Abs_Id",OrderTypeAbs_Id },
                                    {"@Order_Task",OrderTask },
                                    {"@Order_Id",OrderId },
                                    {"@User_Id",UserId },
                                    {"@Work_Type",WorkType_Id }
                                };                              
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/CheckLists/BindDataToTabIndex", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            dt1 = JsonConvert.DeserializeObject<DataTable>(result1);
                                            //DataView dv = new DataView(dt1);
                                            //dt_user = dv.ToTable(true, "Question Number");                                           
                                            if (dt1 != null && dt1.Rows.Count > 0)
                                            {                                                                                             
                                                grd_CheckList.Visible = true;
                                                grd_CheckList.DataSource = dt1;
                                                grd_CheckList.Dock = DockStyle.Fill;
                                                tabPane1.SelectedPage.Controls.Add(grd_CheckList);
                                            }
                                        }
                                    }
                                }
                            }
                        }
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
        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {

        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }



    }
}