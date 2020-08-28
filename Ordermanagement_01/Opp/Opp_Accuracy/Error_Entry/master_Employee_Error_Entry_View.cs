using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Accuracy.Error_Entry
{
    public partial class master_Employee_Error_Entry_View : XtraForm
    {
        string operation_Type = "";
        int ProjectTypeId, ProductTypeId;
        int WorkTypeId, UserId, AdminStatus, OrderId, ClientId, ErrorInfoId;
        string OrderNumber, UserRole;
        string ProductionDate, OrderTask;
        public master_Employee_Error_Entry_View(int project_Type_Id, int Product_Type_Id, int user_Id, int work_Type_Id, int admin_Status, int order_Id, string order_Number, string user_Role, int client_Id, int error_Info_ID, string production_Date, string Session_Order_Task)
        {
            InitializeComponent();
            ProjectTypeId = project_Type_Id;
            ProductTypeId = Product_Type_Id;
            UserId = user_Id;
            WorkTypeId = work_Type_Id;
            AdminStatus = admin_Status;
            OrderId = order_Id;
            OrderNumber = order_Number;
            UserRole = user_Role;
            ClientId = client_Id;
            ErrorInfoId = error_Info_ID;
            ProductionDate = production_Date;
            OrderTask = Session_Order_Task;
        }



        private void master_Employee_Error_Entry_View_Load(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                rb_Internal_Error_Type.SelectedIndex = 0;
                navigationFrame1.SelectedPage = navigationPage1;
                lbl_Header.Text = "Internal Error Entry-"+ OrderId;
                BindgrdError();

                rb_External_Error_Type.Visible = false;
                if (UserRole == "1" || UserRole == "4" || UserRole == "6")
                {
                    rb_External_Error_Type.Visible = true;
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("SomeThing Went Wrong! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void rb_Internal_Error_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1;
            rb_External_Error_Type.SelectedIndex = -1;
            BindgrdError();

            lbl_Header.Text = "Internal Error Entry" + OrderId;
        }

        private void rb_Internal_Error_Type_MouseClick(object sender, MouseEventArgs e)
        {

            rb_Internal_Error_Type.SelectedIndex = 0;
        }



        private void rb_External_Error_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2;
            rb_Internal_Error_Type.SelectedIndex = -1;
            lbl_Header.Text = "External Error Entry" + OrderId;
            BindGridExternalErrors();
        }

        

        private void rb_External_Error_Type_MouseClick(object sender, MouseEventArgs e)
        {
            rb_External_Error_Type.SelectedIndex = 0;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (rb_Internal_Error_Type.SelectedIndex != -1)
            {

                operation_Type = "Internal Error Entry";
                Ordermanagement_01.Opp.Opp_Accuracy.Error_Entry.master_Employee_Error_Entry en = new master_Employee_Error_Entry(ProductTypeId, ProductTypeId, WorkTypeId, UserId, OrderId, OrderNumber, ClientId, operation_Type, ProductionDate, OrderTask, AdminStatus, this);
                this.Enabled = false;
                en.Show();
            }
            else if (rb_External_Error_Type.SelectedIndex != -1)
            {
                operation_Type = "External Error Entry";
                Ordermanagement_01.Opp.Opp_Accuracy.Error_Entry.master_Employee_Error_Entry en1 = new master_Employee_Error_Entry(ProductTypeId, ProductTypeId, WorkTypeId, UserId, OrderId, OrderNumber, ClientId, operation_Type, ProductionDate, OrderTask, AdminStatus, this);
                this.Enabled = false;
                en1.Show();
            }
        }
        public async void BindgrdError()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                DataTable dt1 = new DataTable();
                if (AdminStatus == 2)
                {
                    dictionary.Add("@Trans", "Select_Internal");
                    dictionary.Add("@Order_ID", OrderId);
                    dictionary.Add("@User_id", UserId);
                    dictionary.Add("@Work_Type", WorkTypeId);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindGrdInternalError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt1 = JsonConvert.DeserializeObject<DataTable>(result);

                                if (dt1.Rows.Count > 0)
                                {
                                    grdCtrl_Internal_Error.DataSource = dt1;
                                    grdIntCol_Remove.Width = 60;
                                    grdIntCol_Task.Width = 110;
                                    grdIntCol_Error_Field.Width = 200;
                                    grdIntCol_Comments.Width = 300;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i]["Order_Status"].ToString() == "")
                                        {

                                            gridView_Internal_Error.SetRowCellValue(i, gridView_Internal_Error.Columns.ColumnByFieldName("Order_Status"), "AdminTask");
                                        }
                                    }
                                    if (UserRole == "1" || UserRole == " 6")
                                    {
                                        grdIntCol_User_Name.Visible = true;
                                        grdIntCol_Remove.Visible = true;
                                    }
                                    else
                                    {
                                        grdIntCol_User_Name.Visible = false;
                                        grdIntCol_Remove.Visible = false;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    dictionary.Add("@Trans", "BIND_Live");
                    dictionary.Add("@Order_ID", OrderId);
                    dictionary.Add("@Work_Type", WorkTypeId);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindGrdInternalError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt1 = JsonConvert.DeserializeObject<DataTable>(result);

                                if (dt1.Rows.Count > 0)
                                {
                                    grdCtrl_Internal_Error.DataSource = dt1;
                                    grdIntCol_Remove.Width = 60;
                                    grdIntCol_Task.Width = 110;
                                    grdIntCol_Error_Field.Width = 200;
                                    grdIntCol_Comments.Width = 300;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i]["Order_Status"].ToString() == "")
                                        {
                                            gridView_Internal_Error.SetRowCellValue(i, gridView_Internal_Error.Columns.ColumnByFieldName("Order_Status"), "Admin Task");
                                        }
                                    }
                                    if (UserRole == "1" || UserRole == "6")
                                    {
                                        grdIntCol_User_Name.Visible = true;
                                        grdIntCol_Remove.Visible = true;
                                    }
                                    else
                                    {
                                        grdIntCol_User_Name.Visible = false;
                                        grdIntCol_Remove.Visible = false;
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
                XtraMessageBox.Show("Something Went Wrong!Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }
        public async void BindGridExternalErrors()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dt = new DataTable();
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                if (AdminStatus == 2)
                {
                    dictionary.Add("@Trans", "SELECT_EXTERNAL");
                    dictionary.Add("@Order_ID", OrderId);
                    dictionary.Add("@User_id", UserId);
                    dictionary.Add("@Work_Type", WorkTypeId);

                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindGrdExternalError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt.Rows.Count >= 0)
                                {
                                    grdCtrl_External_Error.DataSource = dt;

                                    grdCol_Remove.Width = 60;
                                    grdCol_Task.Width = 110;
                                    grdCol_Error_Field.Width = 200;
                                    grdCol_Comments.Width = 300;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["Order_Status"].ToString() == "")
                                        {
                                            gridView_External_Error.SetRowCellValue(i, gridView_External_Error.Columns.ColumnByFieldName("Order_Status"), "Admin Task");
                                        }
                                    }
                                    if (UserRole == "1" || UserRole == "6" || UserRole == "4")
                                    {
                                        grdCol_User_Name.Visible = true;
                                        grdCol_Remove.Visible = true;
                                    }
                                    else
                                    {
                                        grdCol_User_Name.Visible = false;
                                        grdCol_Remove.Visible = false;
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    dictionary.Add("@Trans", "BIND_Live_External");
                    dictionary.Add("@Order_ID", OrderId);
                    dictionary.Add("@Work_Type", WorkTypeId);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindGrdExternalError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt.Rows.Count >= 0)
                                {
                                    grdCtrl_External_Error.DataSource = dt;
                                    grdCol_Remove.Width = 60;
                                    grdCol_Task.Width = 110;
                                    grdCol_Error_Field.Width = 200;
                                    grdCol_Comments.Width = 300;
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["Order_Status"].ToString() == "")
                                        {
                                            gridView_External_Error.SetRowCellValue(i, gridView_External_Error.Columns.ColumnByFieldName("Order_Status"), "Admin Task");
                                        }
                                    }
                                    if (UserRole == "1" || UserRole == "6" || UserRole == "4")
                                    {
                                        grdCol_User_Name.Visible = true;
                                        grdCol_Remove.Visible = true;
                                    }
                                    else
                                    {
                                        grdCol_User_Name.Visible = false;
                                        grdCol_Remove.Visible = false;
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
                XtraMessageBox.Show("Something Went Wrong! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void gridView_External_Error_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private void gridView_Internal_Error_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private async void gridView_Internal_Error_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "Remove")
            {
                try
                {

                    DataRow row = gridView_Internal_Error.GetDataRow(gridView_Internal_Error.FocusedRowHandle);
                    int ErrorInfo_id_value = int.Parse(row["ErrorInfo_ID"].ToString());
                    int UserIdValue = int.Parse(row["User_id"].ToString());
                    int OrderIdValue = int.Parse(row["Order_ID"].ToString());
                    DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (show == DialogResult.Yes)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        IDictionary<string, object> dict_del = new Dictionary<string, object>();
                        dict_del.Add("@Trans", "Delete_Error_Info");
                        dict_del.Add("@Error_Info_Id", ErrorInfo_id_value);
                        var data = new StringContent(JsonConvert.SerializeObject(dict_del), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/GridDelete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                }
                            }
                        }
                        IDictionary<string, object> dicterr_history = new Dictionary<string, object>();
                        dicterr_history.Add("@Trans", "Insert_Order_History");
                        dicterr_history.Add("@Order_Id", OrderIdValue);
                        dicterr_history.Add("@Error_Info_Id", ErrorInfo_id_value);
                        dicterr_history.Add("@Comments", "Error Deleted");
                        dicterr_history.Add("@User_Id", UserIdValue);
                        var data1 = new StringContent(JsonConvert.SerializeObject(dicterr_history), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/GridHistory", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                }
                            }
                        }
                        SplashScreenManager.CloseForm(false);

                        BindgrdError();
                    }

                    else if (show == DialogResult.Yes)
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something Went Wrong! Please Contact Admin ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }
        private async void gridView_External_Error_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "Remove")
            {
                try
                {

                    DataRow row = gridView_External_Error.GetDataRow(gridView_External_Error.FocusedRowHandle);
                    int ErrorInfo_id_value = int.Parse(row["ErrorInfo_ID"].ToString());
                    int UserIdValue = int.Parse(row["User_id"].ToString());
                    int OrderIdValue = int.Parse(row["Order_ID"].ToString());
                    DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (show == DialogResult.Yes)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        IDictionary<string, object> dict_del = new Dictionary<string, object>();
                        dict_del.Add("@Trans", "Delete_Error_Info");
                        dict_del.Add("@Error_Info_Id", ErrorInfo_id_value);
                        var data = new StringContent(JsonConvert.SerializeObject(dict_del), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/GridDelete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                }
                            }
                        }
                        IDictionary<string, object> dicterr_history = new Dictionary<string, object>();
                        dicterr_history.Add("@Trans", "Insert_Order_History");
                        dicterr_history.Add("@Order_Id", OrderIdValue);
                        dicterr_history.Add("@Error_Info_Id", ErrorInfo_id_value);
                        dicterr_history.Add("@Comments", "Error Deleted");
                        dicterr_history.Add("@User_Id", UserIdValue);
                        var data1 = new StringContent(JsonConvert.SerializeObject(dicterr_history), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/GridHistory", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                }
                            }
                        }
                        SplashScreenManager.CloseForm(false);

                        BindGridExternalErrors();
                    }

                    else if (show == DialogResult.Yes)
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something Went Wrong! Please Contact Admin ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            if (rb_Internal_Error_Type.SelectedIndex != -1)
            {
                string filepath = @"C:\Temp\";
                string fileName = filepath + "Internal Error Entry-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                gridView_Internal_Error.ExportToXlsx(fileName);
                Process.Start(fileName);
            }
            else if(rb_External_Error_Type.SelectedIndex!=-1)
            {
                string filepath = @"C:\Temp\";
                string fileName = filepath + "External Error Entry-" + DateTime.Now.ToString("dd-MM-hh-mm-ss") + ".xlsx";
                if(!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);

                }
                gridView_External_Error.ExportToXlsx(fileName);
                Process.Start(fileName);
            } 
        }

    }


}



