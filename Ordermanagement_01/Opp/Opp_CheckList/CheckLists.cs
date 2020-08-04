using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
using DevExpress.XtraEditors.Repository;

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckLists : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_tabName;
        int UserId, ProjectType_Id, OrderTypeAbs_Id, OrderId, ClientId, SubClientId, OrderTask, WorkType_Id;     
        int tabid, count;
        DataTable dt1, dt_All1,dt_VIEW1,dt_Check;
        int index = 0;      
        string Comments;
        int Ref_Checklist_Master_Type_Id, Checklist_Id, Check_List_Tran_ID;
        string Question;
        private Color colour;
        int visit = 0;
        RepositoryItemCheckEdit ritem;

        private bool IsButton { get; set; }


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
            IsButton = false;
            grd_CheckList.Visible = false;
            btn_Previous.Visible = false;
            btn_Save.Visible = false;
            btn_Next.Visible = true;
            BindTabs();
            visit = 0;
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
                            if (tabPane1.Pages.Count > 0)
                             {

                                    IsButton = true;
                                    tabPane1.SelectedPageIndex = index;
                                    string tabname = tabPane1.SelectedPage.Caption;
                                    GetTabId(tabname);

                            }
                            if (tabPane1.Pages.Count == 1)
                            {
                                btn_Next.Visible = false;
                                btn_Save.Visible = true;
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
        //   if (tabPane1.Pages.Count > 0)
        //    {              
        //            string tabname = tabPane1.SelectedPage.Caption;
        //            GetTabId(tabname); 
        //    }
        }

        private void grd_CheckList_Click(object sender, EventArgs e)
        {
           
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
           
            //if (e.Column.Caption == "Yes")
            //{
            //    bool Yes = (bool)gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["Yes"]);
            //    if(Yes)
            //    {
            //        gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["No"], "False");
            //    }
            //    else if (!Yes) {
            //        gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["No"], "True");

            //    }
            //}
            //if (e.Column.Caption == "No")
            //{
            //    bool No = (bool)gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["No"]);
            //    if(No)
            //    {
            //        gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["No"], "False");
            //    }
            //    else if (!No)
            //    {
            //        gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["Yes"], "True");
            //        gridView1.SetRowCellValue(e.RowHandle, gridView1.Columns["Comments"], "");
            //    }
            //}
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
           // DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;
            if (e.RowHandle != -1)
            {

                if (e.Column.Caption == "Comments")
                {
                    bool chk_yes = (bool)gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["Yes"]);
                    bool chk_no = (bool)gridView1.GetRowCellValue(e.RowHandle, gridView1.Columns["No"]);
                    if (chk_yes != false)
                    {
                        e.Column.OptionsColumn.AllowEdit = false;
                        e.Column.OptionsColumn.ReadOnly = true;
                        gridView1.PostEditor();
                        gridView1.UpdateCurrentRow();
                    }
                    else if (chk_yes == false&& chk_no==true)
                    {
                        e.Column.OptionsColumn.AllowEdit = true;
                        e.Column.OptionsColumn.ReadOnly = false;
                        gridView1.PostEditor();
                        gridView1.UpdateCurrentRow();
                    }
                    if (chk_yes == false && chk_no == false)
                    {
                        e.Column.OptionsColumn.AllowEdit = false;
                        e.Column.OptionsColumn.ReadOnly = true;
                        gridView1.PostEditor();
                        gridView1.UpdateCurrentRow();
                    } 
                }
             
            }
          }

        private void repositoryItemCheckEdit1_EditValueChanged(object sender, EventArgs e)
        {
            ////bool chk_Yes = (bool)(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Yes"));
            ////if (chk_Yes)
            ////{
            ////    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "No", false);
            ////    gridView1.PostEditor();
            ////    gridView1.UpdateCurrentRow();
            ////}
            //else
            //{
            //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "No", true);
            //}

            //bool chk_Yes = (bool)gridView1.GetRowCellValue(i, "Yes");
            //bool chk_No = (bool)gridView1.GetRowCellValue(i, "No");
            //if (chk_Yes == true)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["No"], false);
            //    gridView1.SetRowCellValue(i, gridView1.Columns["Comments"], "");
            //    gridView1.PostEditor();
            //    if (gridView1.PostEditor())
            //    {
            //        gridView1.UpdateCurrentRow();
            //    }
            //}
            //else if (chk_Yes == false)
            //{
            //    gridView1.SetRowCellValue(i, gridView1.Columns["No"], true);
            //    gridView1.PostEditor();
            //    if (gridView1.PostEditor())
            //    {
            //        gridView1.UpdateCurrentRow();
            //    }
            //}
        }
     

        private void repositoryItemCheckEdit2_EditValueChanged(object sender, EventArgs e)
        {
            ////bool chk_No = (bool)(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "Yes"));
            ////if (chk_No)
            ////{
            ////    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Yes", false);
            ////    gridView1.PostEditor();
            ////    gridView1.UpdateCurrentRow();
            ////}
            //else
            //{
            //    gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Yes", true);
            //}
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{
            //    bool chk_Yes = (bool)gridView1.GetRowCellValue(i, "Yes");
            //    bool chk_No = (bool)gridView1.GetRowCellValue(i, "No");
            //    if (chk_No == true)
            //    {
            //        gridView1.SetRowCellValue(i, gridView1.Columns["Yes"], false);
            //        gridView1.PostEditor();
            //        if (gridView1.PostEditor())
            //        {
            //            gridView1.UpdateCurrentRow();
            //        }
            //    }
            //    else if (chk_No == false)
            //    {
            //        gridView1.SetRowCellValue(i, gridView1.Columns["Yes"], true);
            //        gridView1.SetRowCellValue(i, gridView1.Columns["Comments"], "");
            //        gridView1.PostEditor();
            //        if (gridView1.PostEditor())
            //        {
            //            gridView1.UpdateCurrentRow();
            //        }
            //    }
            //}
        }


        private void gridView1_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (e.Column.FieldName != "UnitPrice")
            //{ e.Cancel = (bool)gridView1.GetCellValue(e.RowHandle, "Discontinued"); }
            //for(int i=0;i<gridView1.DataRowCount;i++ )
            //{
            //    bool chk_yes = (bool)gridView1.GetRowCellValue(i, "Yes");
            //    bool chk_no = (bool)gridView1.GetRowCellValue(i, "No");
            //    if (chk_yes)
            //{
            //        if (gridView1.FocusedColumnName == "colCarrier")
            //        {
            //            e.Cancel = true;
            //        }
            //    }
            //    else if(chk_no)
            //    {

                //    }

        }

        private void tabPane1_SelectedPageChanging(object sender, SelectedPageChangingEventArgs e)
        {
            if (IsButton) e.Cancel = false;
            if (!IsButton)
            {
                e.Cancel = true;
            }
            IsButton = false;
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //if (e.RowHandle == GridControl.NewItemRowHandle && e.Column.FieldName == "Yes" && e.Column.FieldName == "No")
            //    e.RepositoryItem = ritem;
        }

        private void repositoryItemCheckEdit1_CheckedChanged(object sender, EventArgs e)
        {
            
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
                                    {"@Trans","CHECK_ORDER_ID_TASK_USER_WISE" },
                                    {"@Ref_Checklist_Master_Type_Id",tabid },                                    
                                    {"@Order_Task",OrderTask },
                                    {"@Order_Id",OrderId },
                                    {"@User_Id",UserId },
                                    {"@Work_Type",WorkType_Id }                                   
                                };                              
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/CheckLists/CheckOrderIdExist", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            dt_Check = JsonConvert.DeserializeObject<DataTable>(result1);
                                            if (dt_Check.Rows.Count != 0)
                                            {
                                                TabData_View();
                                            }
                                            else
                                            {
                                                Grid_Bind_All_TabData();
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
        public async void Grid_Bind_All_TabData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", tabid },
                        { "@Order_Task", OrderTask},
                        { "@OrderType_ABS_Id", OrderTypeAbs_Id },
                        { "@Project_Type_Id", ProjectType_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", SubClientId }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridGeneralView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_All = JsonConvert.DeserializeObject<DataTable>(result);
                            dt_All1 = dt_All;
                            if (dt_All != null && dt_All.Rows.Count > 0)
                            {
                                //if (visit == 0)
                                //{
                                    for (int i = 0; i < dt_All.Rows.Count; i++)
                                    {
                                    dt_All.Rows[i]["Yes"] = "False";
                                    dt_All.Rows[i]["No"] = "False";
                                    dt_All.Rows[i]["Comments"] = "";
                                    }
                               // }
                                grd_CheckList.Visible = true;
                                if (visit == 1)
                                { grd_CheckList.DataSource = dt_All1; }
                                else { grd_CheckList.DataSource = dt_All; }
                                grd_CheckList.Dock = DockStyle.Fill;
                                tabPane1.SelectedPage.Controls.Add(grd_CheckList);
                               
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

        public async void TabData_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        { "@Ref_Checklist_Master_Type_Id", tabid},
                        { "@Order_Task", OrderTask },
                        { "@Order_Id", OrderId },
                        { "@User_Id", UserId },
                        { "@Work_Type", WorkType_Id },
                        { "@Project_Type_Id",ProjectType_Id }
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllGeneralView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_VIEW = JsonConvert.DeserializeObject<DataTable>(result);
                            dt_VIEW1 = dt_VIEW;
                            if (dt_VIEW != null && dt_VIEW.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt_VIEW.Rows.Count; i++)
                                {
                                    dt_VIEW.Rows[i]["Yes"] = "False";
                                    dt_VIEW.Rows[i]["No"] = "False";
                                    dt_VIEW.Rows[i]["Comments"] = "";
                                }
                                grd_CheckList.Visible = true;
                                if (visit == 1)
                                { grd_CheckList.DataSource = dt_VIEW1; }
                                else { grd_CheckList.DataSource = dt_VIEW; }
                                grd_CheckList.Dock = DockStyle.Fill;
                                tabPane1.SelectedPage.Controls.Add(grd_CheckList);
                                //for (int i = 0; i < dt.Rows.Count; i++)
                                //{
                                //   // gridView1.Columns[i].ColumnEdit=rpeo
                                //}
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


        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.FieldName == "Comments")
                {
                    //bool val_Yes = (bool)gridView1.GetRowCellValue(e.RowHandle, "Yes");
                    //if (val_Yes)
                    //    e.Appearance.BackColor = Color.White;
                    //e.Column.OptionsColumn.AllowEdit = false;
                    //e.Column.OptionsColumn.ReadOnly = true;                                        
                    if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == true)
                    {
                        string Comments = view.Row["Comments"].ToString();
                        if (Comments != "")
                        {
                            e.Appearance.BackColor = Color.White;
                            e.Column.OptionsColumn.AllowEdit = true;
                            e.Column.OptionsColumn.ReadOnly = false;
                            gridView1.PostEditor();
                            gridView1.UpdateCurrentRow();

                        }
                        else if (Comments == "")
                        {
                            e.Appearance.BackColor = Color.Red;
                            e.Column.OptionsColumn.AllowEdit = true;
                            e.Column.OptionsColumn.ReadOnly = false;
                            gridView1.PostEditor();
                            gridView1.UpdateCurrentRow();

                        }
                    }
                   else if ((bool)view.Row["No"] == false && (bool)view.Row["Yes"] == true)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "Comments","");                    
                        e.Appearance.BackColor = Color.White;
                        e.Column.OptionsColumn.AllowEdit = false;
                        e.Column.OptionsColumn.ReadOnly = true;
                        gridView1.PostEditor();
                        gridView1.UpdateCurrentRow();

                    }
                    //if (e.Column.FieldName == "Comments")
                    //{
                    //    bool val_No = (bool)gridView1.GetRowCellValue(e.RowHandle, "No");
                    //    if (val_No)
                    //    {
                    //        string text = (gridView1.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    //        e.Column.OptionsColumn.AllowEdit = true;
                    //        e.Column.OptionsColumn.ReadOnly = false;
                    //        if (text == "" || text == null)
                    //        {
                    //            e.Appearance.BackColor = Color.Red;
                    //        }
                    //        else if (text != "" || text != null)
                    //        {
                    //            e.Appearance.BackColor = Color.White;
                    //        }
                    //    }
                    //}
                    ////  System.Drawing.Color colour = e.Appearance.BackColor;
                }
            }
        }
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
      
        private async void SaveTabData()
        {
            IsButton = true;
            try
            {
                if (Validate() == true)
                {
                    if (grd_CheckList.DataSource != null)
                    {             
                       DataTable dtinsert = new DataTable();
                       dtinsert.Columns.AddRange(new DataColumn[17]
                            {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Question",typeof(string)),                    
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("Work_Type",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                            });
                            for (int i = 0; i < gridView1.DataRowCount; i++)
                            {
                                bool chk_yes = (bool)(gridView1.GetRowCellValue(i, "Yes"));
                                bool chk_no = (bool)(gridView1.GetRowCellValue(i, "No"));
                                Comments = gridView1.GetRowCellValue(i, "Comments").ToString();
                                Ref_Checklist_Master_Type_Id = int.Parse(gridView1.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                                Checklist_Id = int.Parse(gridView1.GetRowCellValue(i, "Checklist_Id").ToString());
                                Question = (gridView1.GetRowCellValue(i, "Question").ToString());

                                dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, Question, chk_yes, chk_no, OrderId,
                                                  OrderTask, OrderTypeAbs_Id, WorkType_Id, Comments, UserId, UserId,
                                                  DateTime.Now, UserId, DateTime.Now, ProjectType_Id, "True");
                            }
                            var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/Insert", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                    tabPane1.SelectedPageIndex += 1;                                   
                                        string tabname = tabPane1.SelectedPage.Caption;
                                        GetTabId(tabname);
                                    if (tabPane1.SelectedPageIndex > 0)
                                    {
                                        btn_Previous.Visible = true;
                                    }
                                    else
                                    {
                                        btn_Previous.Visible = false;
                                    }
                                    count = tabPane1.Pages.Count;
                                    if (tabPane1.SelectedPageIndex == count - 1)
                                    {
                                        btn_Save.Visible = true;
                                        btn_Next.Visible = false;
                                    }
                                    else
                                    {
                                        btn_Next.Visible = true;
                                        btn_Save.Visible = false;
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
                XtraMessageBox.Show("Please contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
    
        private void btn_Previous_Click_1(object sender, EventArgs e)
        {
            visit = 1;
            IsButton = true;
            btn_Next.Visible = true;
            btn_Save.Visible = false;
            tabPane1.SelectedPageIndex -= 1;
            index = tabPane1.SelectedPageIndex;
            if (index == 0)
            {
                btn_Previous.Visible = false;
            }
            BindTabs();
            //if (dt_Check.Rows.Count != 0)
            //{
            //    grd_CheckList.DataSource = dt_VIEW1;
            //}
            //else
            //{
            //    grd_CheckList.DataSource = dt_All1;
            //}
        }

        private void btn_Next_Click_1(object sender, EventArgs e)
        {
            //DataTable dtgrid = new DataTable();
            //dtgrid = grd_CheckList.DataSource as DataTable;
            //List<DataTable> dtList = new List<DataTable>();
            //dtList.Add(dtgrid);

            //DataTable[] dtArray = new DataTable[tabid];
            //for (int i = 0; i < tabid; i++)
            //    dtArray[i] = new DataTable("dtgrid" + i);
            visit = 0;
                SaveTabData();
        }

        private void btn_Save_Click_1(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SaveTabData();
               // this.Close();
            }
            catch
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }         
           
        }
        private bool Validate()
        {

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                bool val_Yes = (bool)gridView1.GetRowCellValue(i, "Yes");
                bool val_No = (bool)gridView1.GetRowCellValue(i, "No");
                string Comments= gridView1.GetRowCellValue(i, "Comments").ToString();
                if (val_Yes==true || val_No==true)
                {
                    if (val_Yes == true && val_No==true)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Check Either 'Yes' or 'No' ");
                        return false;
                    }
                    else if(val_Yes==false && val_No==true && Comments=="")
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Enter Comment if 'No' ");
                        return false;
                    }
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Please Check Either 'Yes' or 'No' For All Rows ");
                    return false;
                }
            }
            //if (colour==Color.Red)
            //{
            //    SplashScreenManager.CloseForm(false);
            //    XtraMessageBox.Show("Please Enter Comment Field in Red Color");
            //    return false;
            //}
                return true;
        }

    }
}