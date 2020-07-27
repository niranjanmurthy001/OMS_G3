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

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckList : DevExpress.XtraEditors.XtraForm
    {
        int Order_Id, Order_Task, user_ID, Work_Type_Id, OrderType_ABS_Id, Sub_Client_Id, Project_Type_Id, ClientId;
        string Comments;
        int Ref_Checklist_Master_Type_Id, Checklist_Id, Check_List_Tran_ID;
        string Question;

        public CheckList(int User_Id, int ProjectType_Id,int Order_Type_Abs_Id, int OrderType_Id, int Client_Id, int SubClient_Id, int OrderTask_Id,int WorkType_Id)
        {
            InitializeComponent();
            Project_Type_Id = ProjectType_Id;
            Order_Id = OrderType_Id;
            Order_Task = OrderTask_Id;
            user_ID = User_Id;
            Work_Type_Id = WorkType_Id;
            OrderType_ABS_Id = Order_Type_Abs_Id;
            Sub_Client_Id = SubClient_Id;
            ClientId = Client_Id;

        }

        private void CheckList_Load(object sender, EventArgs e)
        {
            Bind_GenralView();
            Bind_AssessorView();
            Bind_DeedView();
            Bind_MortgageView();
            Bind_JudgmentLienView();
            Bind_OthersView();
            Grid_Bind_All_Clients();
            Bind_Client_View();
        }

        // General Tab
        public async void Bind_GenralView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 1},
                        { "@Order_Id", Order_Id},
                        { "@Order_Task", Order_Task },
                        { "@User_id", user_ID },
                        { "@Work_Type", Work_Type_Id}

            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGeneralView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count != 0)
                            {
                                General_View();
                            }
                            else
                            {
                               Grid_Bind_All_General();
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
        public async void Grid_Bind_All_General()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 1},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }                         
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
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_General.DataSource = dt;
                            }
                            else
                            {
                                grd_General.DataSource = null;
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
        public async void General_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        { "@Ref_Checklist_Master_Type_Id", 1},
                        { "@Order_Task", Order_Task },
                        { "@Order_Id", Order_Id },
                        { "@User_Id", user_ID },
                        { "@Work_Type", Work_Type_Id }
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
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_General.DataSource = dt;
                            }
                            else
                            {
                                grd_General.DataSource = null;
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
        private void gridView_General_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                    e.Column.OptionsColumn.AllowEdit = false;
                    e.Column.OptionsColumn.ReadOnly = true;              
            }
       
            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;                       
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;                      
                    }                
                }
            }
        }
        private void repositoryItemCheckEdit2_EditValueChanged(object sender, EventArgs e)
        {           
            for (int i = 0; i < gridView_General.DataRowCount; i++)
            {
                if ((bool)gridView_General.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["No"], false);
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["Comments"], "");
                    gridView_General.PostEditor();
                    if (gridView_General.PostEditor())
                    {
                        gridView_General.UpdateCurrentRow();
                    }
                }
                else if((bool)gridView_General.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["No"], true);
                    gridView_General.PostEditor();
                    if (gridView_General.PostEditor())
                    {
                        gridView_General.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit3_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_General.DataRowCount; i++)
            {
                if ((bool)gridView_General.GetRowCellValue(i, "No") == true)
                {
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["Yes"], false);
                    gridView_General.PostEditor();
                    if (gridView_General.PostEditor())
                    {
                        gridView_General.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_General.GetRowCellValue(i, "No") == false)
                {
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["Yes"], true);
                    gridView_General.SetRowCellValue(i, gridView_General.Columns["Comments"], "");
                    gridView_General.PostEditor();
                    if (gridView_General.PostEditor())
                    {
                        gridView_General.UpdateCurrentRow();
                    }
                }
            }
        }


        //private bool Validate_Genral_Question_New()
        //{
        //    int Checked_Cell_Count = 0;
        //    for (int i = 0; i < gridView_General.RowCount; i++)
        //    {
        //        bool chk_yes = Convert.ToBoolean(gridView_General.Rows[i].Cells["Yes"].FormattedValue);
        //        bool chk_no = Convert.ToBoolean(gridView_General.Rows[i].Cells["No"].FormattedValue);
        //        if (chk_yes == true)
        //        {
        //            int check_Count = 1;
        //            Checked_Cell_Count += check_Count;

        //        }
        //        if (chk_no == true)
        //        {
        //            int check_Count = 1;

        //            Checked_Cell_Count += check_Count;
        //        }
        //        if (chk_yes == false && chk_no == false)
        //        {
        //            gridView_General.Rows[i].Cells[1].Style.BackColor = Color.Red;
        //            gridView_General.Rows[i].Cells[2].Style.BackColor = Color.Red;
        //            Error_Count = 1;
        //            Error_Tab_Count = 1;
        //        }
        //        else
        //        {
        //            gridView_General.Rows[i].Cells[1].Style.BackColor = SystemColors.Control;
        //            gridView_General.Rows[i].Cells[2].Style.BackColor = SystemColors.Control;
        //        }
        //        if (gridView_General[3, i].Value == null || gridView_General[3, i].Value == "")
        //        {
        //            if (chk_no == true)
        //            {
        //                gridView_General[3, i].Style.BackColor = Color.Red;
        //            }
        //            Comments = "";


        //        }
        //        else
        //        {
        //            grd_General_Checklist[7, i].Style.BackColor = Color.White;
        //            Comments = grd_General_Checklist.Rows[i].Cells[7].Value.ToString();
        //        }
        //        if (chk_no == true && Comments.Trim().ToString() == "")                {
        //            Error_Count = 1;
        //            Error_Tab_Count = 1;
        //            gridView_General[3, i].Style.BackColor = Color.Red;
        //            break;
        //        }
        //        else
        //        {
        //            Error_Count = 0;
        //        }
        //    }
        //    if (gridView_General.RowCount <= 0)
        //    {
        //        return true;
        //    }
        //    if (gridView_General.RowCount == Checked_Cell_Count && Error_Count != 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        Error_Count = 1;
        //        Error_Tab_Count = 1;
        //        Defined_Tab_Index = 1;
        //        SplashScreenManager.CloseForm(false);
        //        XtraMessageBox.Show("Please Enter all Fields");
        //        return false;
        //    }
        //}
        private async void Save_General_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);               
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });     
                for (int i = 0; i < gridView_General.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_General.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_General.GetRowCellValue(i, "No"));
                    Comments = gridView_General.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_General.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_General.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_General.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertGeneral", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }                                                                                                                                                                          
        private void btn_General_Next_Click(object sender, EventArgs e)
        {
            if (gridView_General.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_General_List();
                tabPane_CheckList.SelectedPage = Page_AsseAndTax;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_General;
            }
        }
   

        //Assesor and Taxes Tab
        public async void Bind_AssessorView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 2 },
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAssessorView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                Assessor_View();
                            }
                            else
                            {
                                Grid_Bind_All_AssessorTax();
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
        public async void Assessor_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 2 },
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllAssessorView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_AsseAndTax.DataSource = dt;
                            }
                            else
                            {
                                grd_AsseAndTax.DataSource = null;
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
        public async void Grid_Bind_All_AssessorTax()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 2},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGriAssessorView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_AsseAndTax.DataSource = dt;
                            }
                            else
                            {
                                grd_AsseAndTax.DataSource = null;
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
        private async void Save_Assessor_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });
                for (int i = 0; i < gridView_AsseAndTax.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_AsseAndTax.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_AsseAndTax.GetRowCellValue(i, "No"));
                    Comments = gridView_AsseAndTax.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_AsseAndTax.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_AsseAndTax.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_AsseAndTax.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertAssessor", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_AssesTax_Next_Click(object sender, EventArgs e)
        {
            if (gridView_AsseAndTax.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_Assessor_List();
                tabPane_CheckList.SelectedPage = Page_Deed;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_AsseAndTax;
            }
        }
        private void repositoryItemCheckEdit_Yes_Assessor_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_AsseAndTax.DataRowCount; i++)
            {
                if ((bool)gridView_AsseAndTax.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["No"], false);
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["Comments"], "");
                    gridView_AsseAndTax.PostEditor();
                    if (gridView_AsseAndTax.PostEditor())
                    {
                        gridView_AsseAndTax.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_AsseAndTax.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["No"], true);
                    gridView_AsseAndTax.PostEditor();
                    if (gridView_AsseAndTax.PostEditor())
                    {
                        gridView_AsseAndTax.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Assessor_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_AsseAndTax.DataRowCount; i++)
            {
                if ((bool)gridView_AsseAndTax.GetRowCellValue(i, "No") == true)
                {
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["Yes"], false);
                    gridView_AsseAndTax.PostEditor();
                    if (gridView_AsseAndTax.PostEditor())
                    {
                        gridView_AsseAndTax.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_AsseAndTax.GetRowCellValue(i, "No") == false)
                {
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["Yes"], true);
                    gridView_AsseAndTax.SetRowCellValue(i, gridView_AsseAndTax.Columns["Comments"], "");
                    gridView_AsseAndTax.PostEditor();
                    if (gridView_AsseAndTax.PostEditor())
                    {
                        gridView_AsseAndTax.UpdateCurrentRow();
                    }
                }
            }
        }
        private void gridView_AsseAndTax_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }


        //Deed Tab
        public async void Bind_DeedView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 3 },
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindDeedView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                Deed_View();
                            }
                            else
                            {
                                Grid_Bind_All_Deed();
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
        public async void Deed_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 3 },
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllDeedView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Deed.DataSource = dt;
                            }
                            else
                            {
                                grd_Deed.DataSource = null;
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
        public async void Grid_Bind_All_Deed()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 3},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridDeedView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Deed.DataSource = dt;
                            }
                            else
                            {
                                grd_Deed.DataSource = null;
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
        private async void Save_Deed_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });
                for (int i = 0; i < gridView_Deed.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_Deed.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_Deed.GetRowCellValue(i, "No"));
                    Comments = gridView_Deed.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_Deed.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_Deed.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_Deed.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertDeed", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void Btn_Deed_Next_Click(object sender, EventArgs e)
        {
            if (gridView_Deed.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_Deed_List();
                tabPane_CheckList.SelectedPage = Page_Mortgage;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter All The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_Deed;
            }
        }
        private void repositoryItemCheckEdit_Yes_Deed_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Deed.DataRowCount; i++)
            {
                if ((bool)gridView_Deed.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["No"], false);
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["Comments"], "");
                    gridView_Deed.PostEditor();
                    if (gridView_Deed.PostEditor())
                    {
                        gridView_Deed.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Deed.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["No"], true);
                    gridView_Deed.PostEditor();
                    if (gridView_Deed.PostEditor())
                    {
                        gridView_Deed.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Deed_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Deed.DataRowCount; i++)
            {
                if ((bool)gridView_Deed.GetRowCellValue(i, "No") == true)
                {
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["Yes"], false);
                    gridView_Deed.PostEditor();
                    if (gridView_Deed.PostEditor())
                    {
                        gridView_Deed.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Deed.GetRowCellValue(i, "No") == false)
                {
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["Yes"], true);
                    gridView_Deed.SetRowCellValue(i, gridView_Deed.Columns["Comments"], "");
                    gridView_Deed.PostEditor();
                    if (gridView_Deed.PostEditor())
                    {
                        gridView_Deed.UpdateCurrentRow();
                    }
                }
            }
        }
        private void gridView_Deed_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }


        //Mortgage Tab
        public async void Bind_MortgageView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 4},
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindMortgageView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                Mortgage_View();
                            }
                            else
                            {

                                Grid_Bind_All_Mortgage();
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
        public async void Mortgage_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 4},
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllMortgageView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Mortgage.DataSource = dt;
                            }
                            else
                            {
                                grd_Mortgage.DataSource = null;
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
        public async void Grid_Bind_All_Mortgage()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 4},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridMortgageView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Mortgage.DataSource = dt;
                            }
                            else
                            {
                                grd_Mortgage.DataSource = null;
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
        private async void Save_Mortgage_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });
                for (int i = 0; i < gridView_Mortgage.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_Mortgage.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_Mortgage.GetRowCellValue(i, "No"));
                    Comments = gridView_Mortgage.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_Mortgage.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_Mortgage.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_Mortgage.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertMortgage", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_Mortgage_Next_Click(object sender, EventArgs e)
        {
            if (gridView_Mortgage.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_Mortgage_List();
                tabPane_CheckList.SelectedPage = Page_Judgment;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter All The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_Mortgage;
            }
        }
        private void repositoryItemCheckEdit_Yes_Mortgage_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Mortgage.DataRowCount; i++)
            {
                if ((bool)gridView_Mortgage.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["No"], false);
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["Comments"], "");
                    gridView_Mortgage.PostEditor();
                    if (gridView_Mortgage.PostEditor())
                    {
                        gridView_Mortgage.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Mortgage.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["No"], true);
                    gridView_Mortgage.PostEditor();
                    if (gridView_Mortgage.PostEditor())
                    {
                        gridView_Mortgage.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Mortgage_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Mortgage.DataRowCount; i++)
            {
                if ((bool)gridView_Mortgage.GetRowCellValue(i, "No") == true)
                {
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["Yes"], false);
                    gridView_Mortgage.PostEditor();
                    if (gridView_Mortgage.PostEditor())
                    {
                        gridView_Mortgage.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Mortgage.GetRowCellValue(i, "No") == false)
                {
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["Yes"], true);
                    gridView_Mortgage.SetRowCellValue(i, gridView_Mortgage.Columns["Comments"], "");
                    gridView_Mortgage.PostEditor();
                    if (gridView_Mortgage.PostEditor())
                    {
                        gridView_Mortgage.UpdateCurrentRow();
                    }
                }
            }
        }
        private void gridView_Mortgage_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }
      


        //Judgment / Liens Tab
        public async void Bind_JudgmentLienView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 5 },
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindJudgmentView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                JudgmentLien_View();
                            }
                            else
                            {

                                Grid_Bind_All_JudgmLien();
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
        public async void JudgmentLien_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 5 },
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllJudgmentView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Judgement.DataSource = dt;
                            }
                            else
                            {
                                grd_Judgement.DataSource = null;
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
        public async void Grid_Bind_All_JudgmLien()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 5},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridJudgmentView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Judgement.DataSource = dt;
                            }
                            else
                            {
                                grd_Judgement.DataSource = null;
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
        private async void Save_Judgment_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });
                for (int i = 0; i < gridView_Judgement.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_Judgement.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_Judgement.GetRowCellValue(i, "No"));
                    Comments = gridView_Judgement.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_Judgement.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_Judgement.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_Judgement.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertJudgement", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_Judgement_Next_Click(object sender, EventArgs e)
        {
            if (gridView_Judgement.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_Judgment_List();
                tabPane_CheckList.SelectedPage = Page_Others;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter All The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_Judgment;
            }           
        }
        private void repositoryItemCheckEdit_Yes_Judgement_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Judgement.DataRowCount; i++)
            {
                if ((bool)gridView_Judgement.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["No"], false);
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["Comments"], "");
                    gridView_Judgement.PostEditor();
                    if (gridView_Judgement.PostEditor())
                    {
                        gridView_Judgement.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Judgement.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["No"], true);
                    gridView_Judgement.PostEditor();
                    if (gridView_Judgement.PostEditor())
                    {
                        gridView_Judgement.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Judgement_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Judgement.DataRowCount; i++)
            {
                if ((bool)gridView_Judgement.GetRowCellValue(i, "No") == true)
                {
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["Yes"], false);
                    gridView_Judgement.PostEditor();
                    if (gridView_Judgement.PostEditor())
                    {
                        gridView_Judgement.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Judgement.GetRowCellValue(i, "No") == false)
                {
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["Yes"], true);
                    gridView_Judgement.SetRowCellValue(i, gridView_Judgement.Columns["Comments"], "");
                    gridView_Judgement.PostEditor();
                    if (gridView_Judgement.PostEditor())
                    {
                        gridView_Judgement.UpdateCurrentRow();
                    }
                }
            }
        }
        private void gridView_Judgement_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }


        
        // Others Tab
        public async void Bind_OthersView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 6 },
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindOthersView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                Others_View();
                            }
                            else
                            {

                                Grid_Bind_All_Others();
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
        public async void Others_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 6},
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllOthersView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Others.DataSource = dt;
                            }
                            else
                            {
                                grd_Others.DataSource = null;
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
        public async void Grid_Bind_All_Others()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                         { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 6},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridOthersView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Others.DataSource = dt;
                            }
                            else
                            {
                                grd_Others.DataSource = null;
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
        private async void Save_Others_List()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[16]
                {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task_Id",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("WorkType_Id",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool))
                });
                for (int i = 0; i < gridView_Others.DataRowCount; i++)
                {
                    bool chk_yes = (bool)(gridView_Others.GetRowCellValue(i, "Yes"));
                    bool chk_no = (bool)(gridView_Others.GetRowCellValue(i, "No"));
                    Comments = gridView_Others.GetRowCellValue(i, "Comments").ToString();
                    Ref_Checklist_Master_Type_Id = int.Parse(gridView_Others.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                    Checklist_Id = int.Parse(gridView_Others.GetRowCellValue(i, "Checklist_Id").ToString());
                    Question = (gridView_Others.GetRowCellValue(i, "Question").ToString());

                    dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, chk_yes, chk_no, Order_Id,
                                      Order_Task, OrderType_ABS_Id, Work_Type_Id, Comments, user_ID, user_ID,
                                      DateTime.Now, user_ID, DateTime.Now, Project_Type_Id, "True");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertOthers", data);
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
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_Others_Next_Click(object sender, EventArgs e)
        {
            if (gridView_Others.Columns["Comments"].AppearanceCell.BackColor != Color.Red)
            {
                Save_Others_List();
                tabPane_CheckList.SelectedPage = Page_ClientSpecification;
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter All The Comments Field In Red");
                tabPane_CheckList.SelectedPage = Page_Others;
            }          
        }
        private void repositoryItemCheckEdit_Yes_Others_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Others.DataRowCount; i++)
            {
                if ((bool)gridView_Others.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["No"], false);
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["Comments"], "");
                    gridView_Others.PostEditor();
                    if (gridView_Others.PostEditor())
                    {
                        gridView_Others.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Others.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["No"], true);
                    gridView_Others.PostEditor();
                    if (gridView_Others.PostEditor())
                    {
                        gridView_Others.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Others_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_Others.DataRowCount; i++)
            {
                if ((bool)gridView_Others.GetRowCellValue(i, "No") == true)
                {
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["Yes"], false);
                    gridView_Others.PostEditor();
                    if (gridView_Others.PostEditor())
                    {
                        gridView_Others.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_Others.GetRowCellValue(i, "No") == false)
                {
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["Yes"], true);
                    gridView_Others.SetRowCellValue(i, gridView_Others.Columns["No"], "");
                    gridView_Others.PostEditor();
                    if (gridView_Others.PostEditor())
                    {
                        gridView_Others.UpdateCurrentRow();
                    }
                }
            }

        }
        private void gridView_Others_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }

 
        
        // Client Specification Tab
        public async void Bind_Client_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        {"@Trans", "CHECK_ORDER_ID_TASK_USER_WISE"},
                        {"@Ref_Checklist_Master_Type_Id", 7 },
                        {"@Order_Id", Order_Id },
                        {"@Order_Task", Order_Task },
                        {"@User_id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindClientView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                Client_View();
                            }
                            else
                            {

                                Grid_Bind_All_Clients();
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
        public async void Client_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        {"@Ref_Checklist_Master_Type_Id", 7 },
                        { "@Order_Task", Order_Task},
                        {"@Order_Id", Order_Id },
                        {"@User_Id", user_ID },
                        {"@Work_Type", Work_Type_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllClientView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_ClientSpecification.DataSource = dt;
                            }
                            else
                            {
                                grd_ClientSpecification.DataSource = null;
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
        public async void Grid_Bind_All_Clients()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                          { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", 7},
                        { "@Order_Task", Order_Task},
                        { "@OrderType_ABS_Id", OrderType_ABS_Id },
                        { "@Project_Type_Id", Project_Type_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", Sub_Client_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridClientView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_ClientSpecification.DataSource = dt;
                            }
                            else
                            {
                                grd_ClientSpecification.DataSource = null;
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
        private void repositoryItemCheckEdit_Yes_Client_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_ClientSpecification.DataRowCount; i++)
            {
                if ((bool)gridView_ClientSpecification.GetRowCellValue(i, "Yes") == true)
                {
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["No"], false);
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["Comments"],"");
                    gridView_ClientSpecification.PostEditor();
                    if (gridView_ClientSpecification.PostEditor())
                    {
                        gridView_ClientSpecification.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_ClientSpecification.GetRowCellValue(i, "Yes") == false)
                {
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["No"], true);
                    gridView_ClientSpecification.PostEditor();
                    if (gridView_ClientSpecification.PostEditor())
                    {
                        gridView_ClientSpecification.UpdateCurrentRow();
                    }
                }
            }
        }
        private void repositoryItemCheckEdit_No_Client_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView_ClientSpecification.DataRowCount; i++)
            {
                if ((bool)gridView_ClientSpecification.GetRowCellValue(i, "No") == true)
                {
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["Yes"], false);
                    gridView_ClientSpecification.PostEditor();
                    if (gridView_ClientSpecification.PostEditor())
                    {
                        gridView_ClientSpecification.UpdateCurrentRow();
                    }
                }
                else if ((bool)gridView_ClientSpecification.GetRowCellValue(i, "No") == false)
                {
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["Yes"], true);
                    gridView_ClientSpecification.SetRowCellValue(i, gridView_ClientSpecification.Columns["Comments"], "");
                    gridView_ClientSpecification.PostEditor();
                    if (gridView_ClientSpecification.PostEditor())
                    {
                        gridView_ClientSpecification.UpdateCurrentRow();
                    }
                }
            }
        }
        private void gridView_ClientSpecification_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.FieldName == "Comments")
            {
                bool val_Yes = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "Yes"));
                if (val_Yes)
                    e.Appearance.BackColor = Color.White;
                e.Column.OptionsColumn.AllowEdit = false;
                e.Column.OptionsColumn.ReadOnly = true;
            }

            if (e.Column.FieldName == "Comments")
            {
                bool value = Convert.ToBoolean(currentView.GetRowCellValue(e.RowHandle, "No"));
                if (value)
                {
                    string text = (currentView.GetRowCellValue(e.RowHandle, "Comments").ToString());
                    e.Column.OptionsColumn.AllowEdit = true;
                    e.Column.OptionsColumn.ReadOnly = false;
                    if (text == "" || text == null)
                    {
                        e.Appearance.BackColor = Color.Red;
                    }
                    else if (text != "" || text != null)
                    {
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }

        }




        //  Buttons Previous
        private void btn_AssesTax_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_General;
        }
        private void btn_Deed_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_AsseAndTax;
        }
        private void btn_Mortgage_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_Deed;
        }
        private void btn_Judgement_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_Mortgage;
        }
        private void btn_Others_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_Judgment;
        }
        private void btn_CltSpecification_Previous_Click(object sender, EventArgs e)
        {
            tabPane_CheckList.SelectedPage = Page_Others;
        }
 
     
        private void btn_SaveClient_Click(object sender, EventArgs e)
        {

        }

      

        // Display Row Number
        private void gridView_General_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_AsseAndTax_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_Deed_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_Mortgage_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_Judgement_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_Others_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void gridView_ClientSpecification_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
    }
}