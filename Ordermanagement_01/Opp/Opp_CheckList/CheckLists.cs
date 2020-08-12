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
        DataTable dt1, dt_All1, dt_VIEW1, dt_Check;
        int index = 0;
        string Comments;
        int Ref_Checklist_Master_Type_Id, Checklist_Id, Check_List_Tran_ID;
        string Question;
        private Color colour;
        int visit = 0;
        int next, current, previous = 0;
        RepositoryItemCheckEdit ritem;

        private bool IsButton { get; set; }


        public CheckLists(int User_Id, int Project_Type_Id, int Product_Type_Id, int Order_Id, int Client_Id, int SubClient_Id, int Order_Task, int Work_Type_Id)
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
            labelControl2.Text = Convert.ToString(OrderId);

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
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


    

        private bool CheckNo(GridView view, int row)
        {
            GridColumn col = gridView1.Columns["No"];
            bool val_No = (bool)gridView1.GetRowCellValue(row, col);
            return (val_No == true);
        }
        private bool CheckYes(GridView view, int row)
        {
            GridColumn col = gridView1.Columns["Yes"];
            bool val_Yes = (bool)gridView1.GetRowCellValue(row, col);
            return (val_Yes == true);
        }

        private void gridView1_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //     disable/enable editing for yes / No  
            GridView gridView1 = sender as GridView;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckNo(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = false;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckYes(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = true;

        }

        private void gridView1_CellValueChanging_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Yes")
            {

                gridView1.SetRowCellValue(e.RowHandle, "Yes", true);
                gridView1.SetRowCellValue(e.RowHandle, "No", false);
                gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
              


            }
            else if (e.Column.FieldName == "No")
            {
                gridView1.SetRowCellValue(e.RowHandle, "Yes", false);
                gridView1.SetRowCellValue(e.RowHandle, "No", true);

            }
        }

        private void gridView1_ShowingEditor_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //     disable/enable editing for yes / No  
            GridView gridView1 = sender as GridView;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckNo(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = false;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckYes(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = true;
        }

        private void gridView1_RowCellStyle_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.Caption == "Comments")
            {
                bool value = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                    e.Appearance.BackColor = Color.Red;
                string comment = gridView1.GetRowCellValue(e.RowHandle, "Comments").ToString();
                if(comment=="" && comment== null)
                    e.Appearance.BackColor = Color.Red;
                else if(comment!="")
                    e.Appearance.BackColor = Color.White;
            }
           
            //DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;
            //if (e.RowHandle >= 0)
            //{
            //    if (e.Column.Caption == "Comments")
            //    {
            //        if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == true)
            //        {
            //            string Comments = view.Row["Comments"].ToString();
            //            if (Comments != "")
            //            {
            //                e.Appearance.BackColor = Color.White;
            //            }
            //            else if (Comments == "")
            //            {
            //                e.Appearance.BackColor = Color.Red;
            //            }
            //        }
            //        else if ((bool)view.Row["No"] == false && (bool)view.Row["Yes"] == true)
            //        {
            //            gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
            //            e.Appearance.BackColor = Color.White;
            //        }
            //        else if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == false)
            //        {
            //            gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
            //            e.Appearance.BackColor = Color.White;
            //        }
            //    }
            //}
        }

        private void repositoryItemCheckEdit1_EditValueChanged_2(object sender, EventArgs e)
        {
            
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Yes", true);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "No", false);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Comments", "");
            if (gridView1.PostEditor())
            {
                gridView1.UpdateCurrentRow();
            }
          
        }

        private void gridView1_CustomDrawRowIndicator_1(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void repositoryItemCheckEdit2_EditValueChanged_1(object sender, EventArgs e)
        {

          
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "No", true);
            gridView1.SetRowCellValue(gridView1.FocusedRowHandle, "Yes", false);
            if (gridView1.PostEditor())
            {
                gridView1.UpdateCurrentRow();
            }

        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //if (e.Column.FieldName == "Yes")
            //{
              
            //    gridView1.SetRowCellValue(e.RowHandle, "Yes", true);
            //    gridView1.SetRowCellValue(e.RowHandle, "No", false);
            //    gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
               

            //}
            //else if (e.Column.FieldName == "No")
            //{
            //    gridView1.SetRowCellValue(e.RowHandle, "Yes", false);
            //    gridView1.SetRowCellValue(e.RowHandle, "No", true);
              
            //}
          

           
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
                                    {"@Order_Type_Abs_Id",OrderTypeAbs_Id },
                                    {"@Work_Type",WorkType_Id },
                                    { "@Project_Type_Id",ProjectType_Id } ,
                                    { "@Client_Id",ClientId} ,
                                    {"@Sub_Client_Id",SubClientId }
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
                                            int count = Convert.ToInt32(dt_Check.Rows[0]["Count"].ToString());
                                            if (count > 0)
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
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            //  dt_All1 = dt_All.Copy();
                            dt_All.Columns.Add("Yes", typeof(bool));
                            dt_All.Columns.Add("No", typeof(bool));
                            dt_All.Columns.Add("Comments", typeof(string));
                            if (dt_All != null && dt_All.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt_All.Rows.Count; i++)
                                {
                                    dt_All.Rows[i]["Yes"] = "False";
                                    dt_All.Rows[i]["No"] = "False";
                                    dt_All.Rows[i]["Comments"] = "";
                                }

                                grd_CheckList.Visible = true;
                                //if (visit == 1)
                                //{ BindPreviousData(tabid, OrderTask, OrderTypeAbs_Id, ProjectType_Id, ClientId, SubClientId); }
                                grd_CheckList.DataSource = dt_All;
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
                        { "@Order_Task",OrderTask },
                        { "@Order_Id",OrderId },
                        { "@Order_Type_Abs_Id",OrderTypeAbs_Id },
                        { "@Work_Type",WorkType_Id },
                        { "@Project_Type_Id",ProjectType_Id } ,
                        { "@Client_Id",ClientId} ,
                        { "@Sub_Client_Id",SubClientId }
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
                            dt_VIEW1 = dt_VIEW.Copy();
                            if (dt_VIEW != null && dt_VIEW.Rows.Count > 0)
                            {                              
                                grd_CheckList.Visible = true;                             
                                grd_CheckList.DataSource = dt_VIEW;                              
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
                if (e.Column.Caption == "Comments")
                {                                                      
                    if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == true)
                    {
                        string Comments = view.Row["Comments"].ToString();
                        if (Comments != "")
                        {
                            e.Appearance.BackColor = Color.White;
                        }
                        else if (Comments == "")
                        {
                            e.Appearance.BackColor = Color.Red;
                        }
                    }
                    else if ((bool)view.Row["No"] == false && (bool)view.Row["Yes"] == true)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
                        e.Appearance.BackColor = Color.White;
                    }
                    else if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == false)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }
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
                        dtinsert.Columns.AddRange(new DataColumn[19]
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
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Client_Id",typeof(int)),
                     new DataColumn("Sub_Client_Id",typeof(int))
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
                                              DateTime.Now, UserId, DateTime.Now, ProjectType_Id, "True", ClientId, SubClientId);
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
            SavePreviousData();         
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
        }

        private void btn_Next_Click_1(object sender, EventArgs e)
        {           
            SaveTabData();

        }

        private void btn_Save_Click_1(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                SaveTabData();
                if (Validate() == true)
                {
                    XtraMessageBox.Show("Submitted Successfully", "Success",MessageBoxButtons.OK,MessageBoxIcon.None);
                    this.Close();
                 
                }

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
                string Comments = gridView1.GetRowCellValue(i, "Comments").ToString();
                if (val_Yes == true || val_No == true)
                {
                    if (val_Yes == true && val_No == true)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Check Either 'Yes' or 'No' ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (val_Yes == false && val_No == true && Comments == "")
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Enter Comment if 'No' ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Please Check Either 'Yes' or 'No' For All Rows ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        //private async void  BindPreviousData(int _tabid, int _OrderTask,int _OrderTypeAbs_Id,int _ProjectType_Id,int _ClientId,int _SubClientId)
        //  {
        //      try
        //      {
        //          SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //          var dictionary = new Dictionary<string, object>()
        //              {
        //                  { "@Trans", "BIND_PREVIOUS_DATA"},
        //                  { "@Ref_Checklist_Master_Type_Id", _tabid },
        //                  { "@Order_Task", _OrderTask},
        //                  { "@OrderType_ABS_Id", _OrderTypeAbs_Id },
        //                  { "@Project_Type_Id", _ProjectType_Id },
        //                  { "@Client_Id", _ClientId },
        //                  { "@Sub_Client_Id", _SubClientId }
        //              };
        //          var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
        //          using (var httpClient = new HttpClient())
        //          {
        //              var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindPreviousData", data);

        //              if (response.IsSuccessStatusCode)
        //              {
        //                  if (response.StatusCode == HttpStatusCode.OK)
        //                  {
        //                      var result = await response.Content.ReadAsStringAsync();
        //                      DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);                         
        //                      if (dt != null && dt.Rows.Count > 0)
        //                      {                               
        //                          grd_CheckList.Visible = true;
        //                          grd_CheckList.DataSource = dt; 
        //                          grd_CheckList.Dock = DockStyle.Fill;
        //                          tabPane1.SelectedPage.Controls.Add(grd_CheckList);
        //                      }
        //                  }
        //              }
        //          }
        //      }
        //      catch (Exception ex)
        //      {
        //          SplashScreenManager.CloseForm(false);
        //          XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //      }
        //      finally
        //      {
        //          SplashScreenManager.CloseForm(false);
        //      }
        //  }

        private async void SavePreviousData()
        {
            IsButton = true;
            try
            {
                if (grd_CheckList.DataSource != null)
                {
                    DataTable dtinsert = new DataTable();
                    dtinsert.Columns.AddRange(new DataColumn[19]
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
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Client_Id",typeof(int)),
                     new DataColumn("Sub_Client_Id",typeof(int))
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
                                          DateTime.Now, UserId, DateTime.Now, ProjectType_Id, "True", ClientId, SubClientId);
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

    }
}