﻿using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Accuracy.Error_Entry
{
    public partial class master_Employee_Error_Entry : XtraForm
    {
        private master_Employee_Error_Entry_View mainform = null;
        int ProjectTypeId, ProductTypeId;
        int Work_Type_Id, Subprocess_id, Client_Id,UserId;
        int Tilte_Exam_Order_Id, orderid;
        int externalErrorUser, Error_User;
        string User_Role,operationType;

        string OrderNo = "", Order_Number = "", order_NumValue;
        string ProductionDate, OrderTask;
        string UserName;




        public master_Employee_Error_Entry(int project_Type_Id,int product_Type_Id,int work_Type_Id,int user_Id,int order_Id,string order_No,int client_Id,string operation_Type, string production_Date,string session_Order_Task,Form callingForm)
        {
            InitializeComponent();
            mainform = callingForm as master_Employee_Error_Entry_View;
            this.ProjectTypeId = project_Type_Id;
            this.ProductTypeId = product_Type_Id;
            this.Work_Type_Id = work_Type_Id;
            UserId = user_Id;
            this.orderid = order_Id;
            Client_Id = client_Id;
            Order_Number = order_No;
            operationType = operation_Type;
            ProductionDate = production_Date;
            OrderTask = session_Order_Task;
            if (Client_Id == 40)
            {
                if (Order_Number.Contains("-N"))
                {
                    OrderNo = Order_Number.Remove(Order_Number.Length - 2);
                }
                else
                {
                    OrderNo = Order_Number;
                }
            }
            // This is for All Client Orders
            //==================================================

            if (Client_Id != 40)
            {
                LoadOrdersNo();
            }


            // This is for Title Exam Orders 40 Client Id
            //==================================================
            if (Client_Id == 40)
            {
                LoadOrderNumbers();
            }
        }

        private async void LoadOrderNumbers()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_get = new Dictionary<string, object>();
                dict_get.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                dict_get.Add("@Client_Order_Number", Order_Number);
                var data = new StringContent(JsonConvert.SerializeObject(dict_get), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindOrderNo", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                order_NumValue = dt.Rows[0]["Client_Order_Number"].ToString();
                                Tilte_Exam_Order_Id = int.Parse(dt.Rows[0]["Order_ID"].ToString());
                                Subprocess_id = int.Parse(dt.Rows[0]["Sub_ProcessId"].ToString());
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

        private async void GetUsername()
        {
            Dictionary<string, object> dict_userid = new Dictionary<string, object>();
            dict_userid.Add("@Trans", "Get_User_Name");
            dict_userid.Add("@User_id", UserId);
            var data = new StringContent(JsonConvert.SerializeObject(dict_userid), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindUserName", data);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                        if (dt1.Rows.Count > 0)
                        {
                            UserName = dt1.Rows[0]["User_Name"].ToString();
                        }
                    }
                }
            }
        }

        private async void LoadOrdersNo()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_get = new Dictionary<string, object>();
                dict_get.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                dict_get.Add("@Client_Order_Number", Order_Number);
                var data = new StringContent(JsonConvert.SerializeObject(dict_get), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindOrderNo", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                order_NumValue = dt.Rows[0]["Client_Order_Number"].ToString();
                                Tilte_Exam_Order_Id = int.Parse(dt.Rows[0]["Order_ID"].ToString());
                                Subprocess_id = int.Parse(dt.Rows[0]["Sub_ProcessId"].ToString());
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


        private void master_Employee_Error_Entry_Load(object sender, EventArgs e)
        {
            ddl_Error_Type.Properties.Columns.Clear();
            Bind_ErrorType();
            ddl_User_Name.Visible = false;
            GetUsername();
        }

        

        private async void Bind_ErrorType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_select = new Dictionary<string, object>();
                {
                    dict_select.Add("@Trans", "BIND_ERROR_TYPE");
                    dict_select.Add("@Project_Type_Id", ProjectTypeId);
                    dict_select.Add("@Product_Type_Id", ProductTypeId);
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict_select), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindErrorType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt.NewRow();
                            dr[0] = 0;
                            dr[1] = "SELECT";
                            dt.Rows.InsertAt(dr, 0);
                            ddl_Error_Type.Properties.DataSource = dt;
                            ddl_Error_Type.Properties.ValueMember = "New_Error_Type_Id";
                            ddl_Error_Type.Properties.DisplayMember = "New_Error_Type";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("New_Error_Type");
                            ddl_Error_Type.Properties.Columns.Add(col);
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

        private void ddl_Error_Type_EditValueChanged(object sender, EventArgs e)
        {
            ddl_Error_Tab.Properties.Columns.Clear();
            BindErrorTab();
        }

       

        private async void BindErrorTab()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "SELECT_Error_Tab");
                    dictionary.Add("@Project_Type_Id", ProjectTypeId);
                    dictionary.Add("@Product_Type_Id", ProductTypeId);
                }
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindErrorTab", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt1.NewRow();
                            dr[0] = 0;
                            dr[1] = "SELECT";
                            dt1.Rows.InsertAt(dr, 0);
                            ddl_Error_Tab.Properties.DataSource = dt1;
                            ddl_Error_Tab.Properties.ValueMember = "Error_Type_Id";
                            ddl_Error_Tab.Properties.DisplayMember = "Error_Type";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Error_Type");
                            ddl_Error_Tab.Properties.Columns.Add(col);

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

        

        private void ddl_Error_Tab_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_Error_Tab.EditValue) != 0)
            {
                int ErrorTabvalue = Convert.ToInt32(ddl_Error_Tab.EditValue);
                ddl_Error_Field.Properties.Columns.Clear();
                Bind_Error_Field(ErrorTabvalue);
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(" Select Error Tab", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       

        private async void Bind_Error_Field(int Error_Tab_Id)
        {
            try
            {


                IDictionary<string, object> dictselect = new Dictionary<string, object>();
                {
                    dictselect.Add("@Trans", "SELECT_Error_Description");
                    dictselect.Add("@Error_Tab_Id", Error_Tab_Id);
                    dictselect.Add("@Project_Type_Id", ProjectTypeId);
                    dictselect.Add("@Product_Type_Id", ProductTypeId);
                }
                var data1 = new StringContent(JsonConvert.SerializeObject(dictselect), Encoding.UTF8, "application/json");
                using (var httpClient1 = new HttpClient())
                {
                    var response1 = await httpClient1.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindErrorField", data1);
                    if (response1.IsSuccessStatusCode)
                    {
                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response1.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt1.NewRow();
                            dr[0] = 0;
                            dr[1] = "SELECT";
                            dt1.Rows.InsertAt(dr, 0);
                            ddl_Error_Field.Properties.DataSource = dt1;
                            ddl_Error_Field.Properties.ValueMember = "Error_description_Id";
                            ddl_Error_Field.Properties.DisplayMember = "Error_description";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Error_description");
                            ddl_Error_Field.Properties.Columns.Add(col);
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

        private void ddl_Error_Field_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_Error_Field.EditValue) != 0)
            {
                ddl_Task.Properties.Columns.Clear();

                BindOrderTask();
            }
        }
        private async void BindOrderTask()
        {


            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "bind_Order_Task");
                    dictionary.Add("@Project_Type_Id", ProjectTypeId);

                }
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindErrorTask", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt1.NewRow();
                            dr[0] = 0;
                            dr[1] = "SELECT";
                            dt1.Rows.InsertAt(dr, 0);
                            ddl_Task.Properties.DataSource = dt1;
                            ddl_Task.Properties.ValueMember = "Order_Task_ID";
                            ddl_Task.Properties.DisplayMember = "Order_Status";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Status");
                            ddl_Task.Properties.Columns.Add(col);

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

        

        private async void Bind_Users_For_Error_Info()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "GET_USER");


                }
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/BindErrorUser", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt1.NewRow();
                            dr[0] = 0;
                            dr[1] = "SELECT";
                            dt1.Rows.InsertAt(dr, 0);
                            ddl_User_Name.Properties.DataSource = dt1;
                            ddl_User_Name.Properties.ValueMember = "User_id";
                            ddl_User_Name.Properties.DisplayMember = "User_Name";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("User_Name");
                            ddl_User_Name.Properties.Columns.Add(col);

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

      

        private async void ddl_Task_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                int OrderTaskValue = Convert.ToInt32(ddl_Task.EditValue);
                if (ddl_Task.ItemIndex != 0)
                {

                    IDictionary<string, object> dict = new Dictionary<string, object>();
                    if (Work_Type_Id == 1)
                    {
                        dict.Add("@Trans", "Task_User");
                    }
                    else if (Work_Type_Id == 2)
                    {
                        dict.Add("@Trans", "REWORK_TASK_USER");
                    }
                    else if (Work_Type_Id == 3)
                    {
                        dict.Add("@Trans", "Task_User");
                    }

                    dict.Add("@Task", OrderTaskValue);
                   // dict.Add("@Order_ID", orderid);

                    if (Subprocess_id != 330 && Subprocess_id > 0)
                    {
                        dict.Add("@Order_ID", Tilte_Exam_Order_Id);
                    }
                    else if (Subprocess_id > 0)
                    {
                        dict.Add("@Order_ID", orderid);
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindExternalUser", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt.Rows.Count > 0)
                                {
                                    lbl_UserName.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
                                    externalErrorUser = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
                                    if (User_Role == "2")
                                    {
                                        lbl_UserName.Text = "**********";
                                    }
                                }
                                else
                                {
                                    lbl_UserName.Text = "";
                                    Error_User = 0;
                                    chk_UserName.Visible = true;
                                    Bind_Users_For_Error_Info();
                                }

                            }
                        }
                        else
                        {
                            lbl_UserName.Text = "";
                            Error_User = 0;
                            chk_UserName.Visible = true;
                            Bind_Users_For_Error_Info();
                        }
                    }
                }
                else
                {
                    ddl_User_Name.Visible = false;
                    lbl_UserName.Text = "";
                    externalErrorUser = 0;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void ddl_User_Name_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_User_Name.EditValue) != 0)
            {
                lbl_UserName.Text = ddl_User_Name.Text.ToString();
                Error_User = int.Parse(ddl_User_Name.EditValue.ToString());
                chk_UserName.Checked = false;
            }
            else
            {
                lbl_UserName.Text = "";
                Error_User = 0;
            }
        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            int ErrorTypeValue = Convert.ToInt32(ddl_Error_Type.EditValue);
            int ErrorTabvalue = Convert.ToInt32(ddl_Error_Tab.EditValue);
            int Errorfieldvalue = Convert.ToInt32(ddl_Error_Field.EditValue);
            int Taskvalue = Convert.ToInt32(ddl_Task.EditValue);
            int Usernamevalue = Convert.ToInt32(ddl_User_Name.EditValue);
            string CommentValue = txt_Comments.Text;


            if (operationType == "Internal Error Entry")
            {
                if (validate() != false)
                {
                    if (Error_User != 0)
                    {
                        int Ent_error_info_Id = 0;
                        IDictionary<string, object> dict_insert = new Dictionary<string, object>();
                        dict_insert.Add("@Trans", "INSERT");
                        dict_insert.Add("@New_Error_Type_Id", ErrorTypeValue);  // Added one more column from master New_Error_Type_Id
                        dict_insert.Add("@Error_Type", ErrorTabvalue);  // Error_Type means Error_Tab new chnages Has done
                        dict_insert.Add("@Error_Description", Errorfieldvalue);  // error description means error Field new chnages has done
                        dict_insert.Add("@Comments", CommentValue);
                        dict_insert.Add("@Task", OrderTask);
                        dict_insert.Add("@User_name", UserName);
                        dict_insert.Add("@Order_ID", orderid);
                        dict_insert.Add("@Error_Task", Taskvalue);
                        dict_insert.Add("@Error_Status", 1);//New Error
                        dict_insert.Add("@Error_User", Error_User);
                        dict_insert.Add("@User_ID", UserId);
                        dict_insert.Add("@Entered_Date", DateTime.Now);
                        dict_insert.Add("@Status", "True");
                        dict_insert.Add("@Work_Type", Work_Type_Id);
                        dict_insert.Add("@Production_Date", ProductionDate);
                        dict_insert.Add("@External_Error", false);  //false - It is Internal Error
                        var data = new StringContent(JsonConvert.SerializeObject(dict_insert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/InsertInternalError", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                }

                            }
                        }
                        IDictionary<string, object> dict_history = new Dictionary<string, object>();
                        dict_history.Add("@Trans", "INSERT");
                        dict_history.Add("@Order_Id", orderid);
                        dict_history.Add("@Error_Info_Id", Ent_error_info_Id);
                        dict_history.Add("@Comments", "Error Created");
                        dict_history.Add("@User_Id", UserId);
                        var data1 = new StringContent(JsonConvert.SerializeObject(dict_history), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/EmployeeErrorEntry/InsertErrorHistory", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    MessageBox.Show("Error Info Added Successfully");
                                }
                            }
                        }
                        
                       
                    }
                }
            }

        }
            

        

        private bool validate()
        {
            if (Convert.ToInt32(ddl_Error_Type.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Error Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Error_Tab.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Error Tab", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Error_Field.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Error Field", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Task.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Error Task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_User_Name.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select User Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txt_Comments.Text=="")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter Comments","Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }

        private void chk_UserName_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_UserName.Checked == true)
            {
                ddl_User_Name.Visible = true;
            }
            else
            {
                ddl_User_Name.Visible = false;
            }
        }
        private void ddl_Error_Tab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ddl_Error_Field.Focus();
            }
        }

        private void ddl_Error_Type_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ddl_Error_Tab.Focus();
            }
        }
        private void txt_Comments_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                btn_Save.Focus();
            }
        }

        private void txt_Comments_MouseClick(object sender, MouseEventArgs e)
        {
            if (txt_Comments.Text == "Enter Comments....")
            {
                txt_Comments.Text = "";
                txt_Comments.ForeColor = Color.Black;
            }
        }

        private void ddl_Task_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                txt_Comments.Focus();
            }
        }
        private void ddl_Error_Field_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ddl_Task.Focus();
            }
        }
    }
}
