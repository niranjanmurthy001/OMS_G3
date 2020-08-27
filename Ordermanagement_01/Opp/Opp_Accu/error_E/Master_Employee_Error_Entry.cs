using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Accuracy.Error_Entry
{
    public partial class Master_Employee_Error_Entry : XtraForm
    {
        Master_Employee_Error_Entry_View mainform = null;
        int ProjectTypeId, ProductTypeId;
        int Work_Type_Id, Subprocess_id, Client_Id;
        int Tilte_Exam_Order_Id, orderid;
        int externalErrorUser, Error_User;
        string User_Role;

        string OrderNo = "", Order_Number = "", order_NumValue;

        public Master_Employee_Error_Entry(int Project_Type_Id, int Product_Type_Id, int Order_id, int work_type_id, int client_Id, string orderNo, Form Callingfrom)
        {
            InitializeComponent();
            mainform = Callingfrom as Master_Employee_Error_Entry_View;
            this.ProjectTypeId = Project_Type_Id;
            this.ProductTypeId = Product_Type_Id;
            this.Work_Type_Id = work_type_id;
            this.orderid = Order_id;
            Client_Id = client_Id;
            Order_Number = orderNo;
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

        private void LoadOrderNumbers()
        {

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
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindOrderNo", data);
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


        private void Master_Employee_Error_Entry_Load(object sender, EventArgs e)
        {
            ddl_Error_Type.Properties.Columns.Clear();
            Bind_ErrorType();

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

        private void txt_Comments_MouseEnter(object sender, EventArgs e)
        {
            if (txt_Comments.Text == "Enter Comments....")
            {
                txt_Comments.Text = "";
                txt_Comments.ForeColor = Color.Black;
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

        private void Master_Employee_Error_Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainform.Enabled = true;
        }

        private void ddl_Error_Type_EditValueChanged(object sender, EventArgs e)
        {
            ddl_Error_Tab.Properties.Columns.Clear();
            BindErrorTab();
        }

        private void ddl_Error_Tab_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_Error_Tab.EditValue) != 0)
            {
                int ErrorTabvalue = Convert.ToInt32(ddl_Error_Tab.EditValue);
                ddl_Error_Field.Properties.Columns.Clear();
                Bind_Error_Field(ErrorTabvalue);
            }
        }

        private void ddl_User_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_User.EditValue) != 0)
            {
                lbl_UserName.Text = ddl_User.Text.ToString();
                Error_User = int.Parse(ddl_User.EditValue.ToString());
                chk_User.Checked = false;
            }
            else
            {
                lbl_UserName.Text = "";
                Error_User = 0;
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
                            ddl_User.Properties.DataSource = dt1;
                            ddl_User.Properties.ValueMember = "User_id";
                            ddl_User.Properties.DisplayMember = "User_Name";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("User_Name");
                            ddl_User.Properties.Columns.Add(col);

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
                    dict.Add("@Order_ID", orderid);

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
                                    chk_User.Visible = true;
                                    Bind_Users_For_Error_Info();
                                }

                            }
                        }
                        else
                        {
                            lbl_UserName.Text = "";
                            Error_User = 0;
                            chk_User.Visible = true;
                            Bind_Users_For_Error_Info();
                        }
                    }
                }
                else
                {
                    ddl_User.Visible = false;
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


    }
}

