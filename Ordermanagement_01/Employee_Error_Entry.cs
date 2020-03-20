using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Ordermanagement_01.Models;
using System.Net;
using System.Threading.Tasks;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;

namespace Ordermanagement_01
{
    public partial class Employee_Error_Entry : Form
    {
        Commonclass Comclass = new Commonclass();
        DataAccess dataaccess = new DataAccess();
        DropDownistBindClass dbc = new DropDownistBindClass();
        int result;
        int userid;
        string ErrorType, externalErrorType;
        string ErrorInfoID;
        string ORDERTASK, Username;
        int orderid, Tilte_Exam_Order_Id, SubProcessId, Error_User, AdminStatus, externalErrorUser;
        string Order_num;
        int Work_Type_Id;
        string User_Role;
        string Order_Number;
        string Ordernumber;
        int Order_ID, ErrorInfo_ID, externalErrorInfoId;
        int Client_Id, Sub_ProcessId;
        string Production_Date, erroronuser;

        public Employee_Error_Entry(int User_id, string USER_ROLE, string SESSIONORDERTASK, int Order_Id, int Admin_Status, int WORK_TYPE_ID, string Order_No, string PRODUCTION_DATE, int Error_Info_ID, int CLIENT_ID)
        {
            InitializeComponent();
            userid = User_id;
            Work_Type_Id = WORK_TYPE_ID;
            ORDERTASK = Convert.ToString(SESSIONORDERTASK);
            orderid = Order_Id;
            User_Role = USER_ROLE;
            AdminStatus = Admin_Status;
            Production_Date = PRODUCTION_DATE;
            ErrorInfo_ID = Error_Info_ID;
            Client_Id = CLIENT_ID;
            Order_Number = Order_No;

            //  string OrderNo = Order_Number;
            string OrderNo = "";
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
                LoadOrderNum();
                //Hashtable ht_get = new Hashtable();
                //DataTable dt_get = new DataTable();
                //ht_get.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                //ht_get.Add("@Client_Order_Number", Order_Number);

                //dt_get = dataaccess.ExecuteSP("Sp_Error_Info", ht_get);
                //if (dt_get.Rows.Count > 0)
                //{
                //    Order_num = dt_get.Rows[0]["Client_Order_Number"].ToString();
                //    Tilte_Exam_Order_Id = int.Parse(dt_get.Rows[0]["Order_ID"].ToString());
                //    Sub_ProcessId = int.Parse(dt_get.Rows[0]["Sub_ProcessId"].ToString());
                //}
            }


            // This is for Title Exam Orders 40 Client Id
            //==================================================

            if (Client_Id == 40)
            {
                LoadOrderNumbers();
                //Hashtable ht = new Hashtable();
                //DataTable dt = new DataTable();

                //ht.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                //ht.Add("@Client_Order_Number", OrderNo);   //OrderNo
                //dt = dataaccess.ExecuteSP("Sp_Error_Info", ht);
                //if (dt.Rows.Count > 0)
                //{
                //    Ordernumber = dt.Rows[0]["Client_Order_Number"].ToString();
                //    Tilte_Exam_Order_Id = int.Parse(dt.Rows[0]["Order_ID"].ToString());
                //    Sub_ProcessId = int.Parse(dt.Rows[0]["Sub_ProcessId"].ToString());
                //}
            }

        }
        private async void LoadOrderNum()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_get = new Dictionary<string, object>();
                {
                    dict_get.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                    dict_get.Add("@Client_Order_Number", Order_Number);

                }

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
                                Order_num = dt.Rows[0]["Client_Order_Number"].ToString();
                                Tilte_Exam_Order_Id = int.Parse(dt.Rows[0]["Order_ID"].ToString());
                                Sub_ProcessId = int.Parse(dt.Rows[0]["Sub_ProcessId"].ToString());

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }

        private async void LoadOrderNumbers()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_order = new Dictionary<string, object>();
                {
                    dict_order.Add("@Trans", "GET_ORDER_ID_BY_ORDER_NUM");
                    dict_order.Add("@Client_Order_Number", Order_Number);
                }

                var data = new StringContent(JsonConvert.SerializeObject(dict_order), Encoding.UTF8, "application/json");
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
                                Ordernumber = dt.Rows[0]["Client_Order_Number"].ToString();
                                Tilte_Exam_Order_Id = int.Parse(dt.Rows[0]["Order_ID"].ToString());
                                Sub_ProcessId = int.Parse(dt.Rows[0]["Sub_ProcessId"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }


        private async void Employee_Error_Entry_Load(object sender, EventArgs e)
        {
            try {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                tabPageExternalErrorEntry.PageVisible = false;
                if (User_Role == "1" || User_Role == "4" || User_Role == "6")
                {
                    tabPageExternalErrorEntry.PageVisible = true;
                    // BindExternalErrorEntry();
                }
                tabPaneErrorEntry.SelectedPage = tabPageInternalErrorEntry;

                //Hashtable htuserid = new Hashtable();
                //DataTable dtuserid = new DataTable();
                //htuserid.Add("@Trans", "USERNAME");
                //htuserid.Add("@User_id", userid);
                //dtuserid = dataaccess.ExecuteSP("Sp_Error_Info", htuserid);
                //if (dtuserid.Rows.Count > 0)
                //{
                //    Username = dtuserid.Rows[0]["User_Name"].ToString();
                //}

                IDictionary<string, object> dict_userid = new Dictionary<string, object>();
                dict_userid.Add("@Trans", "USERNAME");
                dict_userid.Add("@User_id", userid);
                var data = new StringContent(JsonConvert.SerializeObject(dict_userid), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindEmpId", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt1.Rows.Count > 0)
                            {
                                Username = dt1.Rows[0]["User_Name"].ToString();
                            }

                        }
                    }

                }


                Bind_New_ErrorType();
                BindErrorType();
                BindgrdError();

                BindExternalNewErrorType();
                BindExternalErrorType();
                BindGridExternalErrors();

                if (Work_Type_Id == 3)
                {
                    if (AdminStatus == 2)// This is For Super Qc
                    {
                        dbc.BindError_Task_Super_Qc(Cbo_Task, int.Parse(ORDERTASK));
                        dbc.BindError_Task_Super_Qc(ddlExternalTask, Convert.ToInt32(ORDERTASK));
                    }
                    else
                    {
                        dbc.BindOrderStatus(Cbo_Task);
                        dbc.BindOrderStatus(ddlExternalTask);
                    }
                }
                else
                {
                    if (AdminStatus == 2)
                    {
                        if (Client_Id == 40)// this is for Title Exam order 40 Client Id
                        {
                            dbc.BindError_Task_For_Title_Exam(Cbo_Task);
                            dbc.BindError_Task_For_Title_Exam(ddlExternalTask);
                        }
                        else
                        {
                            dbc.BindError_Task(Cbo_Task, int.Parse(ORDERTASK));
                            dbc.BindError_Task(ddlExternalTask, Convert.ToInt32(ORDERTASK));
                        }
                    }
                    else
                    {
                        dbc.BindOrderStatus(Cbo_Task);
                        dbc.BindOrderStatus(ddlExternalTask);
                    }
                }
                grd_Error.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                grd_Error.EnableHeadersVisualStyles = false;
                grd_Error.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;

                gridExternalError.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                gridExternalError.EnableHeadersVisualStyles = false;
                gridExternalError.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke;

                ddl_User.Visible = false;
                ddlExternalUser.Visible = false;
               
              
                cbo_ErrorCatogery.Focus();
                //Error info details
                if (ErrorInfo_ID != 0)
                {
                    Error_Info_Details();
                    grd_Error.Visible = false;
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
        private async void Error_Info_Details()
        {
            //Error Dispute
            //Hashtable ht_ErrorInfo_Edit = new Hashtable();
            //DataTable dt_ErrorInfo_Edit = new DataTable();
            //ht_ErrorInfo_Edit.Add("@Trans", "SELECT_BY_ORDER_ID");
            //ht_ErrorInfo_Edit.Add("@ErrorInfo_ID", ErrorInfo_ID);
            //dt_ErrorInfo_Edit = dataaccess.ExecuteSP("Sp_Error_Info", ht_ErrorInfo_Edit);
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                {"@Trans","SELECT_BY_ORDER_ID" },
                { "ErrorInfo_ID",ErrorInfo_ID}
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindErrorsId", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt1.Rows.Count > 0)
                            {
                                ddl_New_Error_Type.SelectedValue = int.Parse(dt1.Rows[0]["New_Error_Type_Id"].ToString());
                                cbo_ErrorCatogery.SelectedValue = dt1.Rows[0]["Error_Type_Id"].ToString();
                                cbo_ErrorDes.SelectedValue = dt1.Rows[0]["Error_description_Id"].ToString();
                                Cbo_Task.SelectedValue = dt1.Rows[0]["Error_Task_Id"].ToString();
                                txt_ErrorCmt.Text = dt1.Rows[0]["Comments"].ToString();
                                ErrorInfo_ID = int.Parse(dt1.Rows[0]["ErrorInfo_ID"].ToString());
                                Lbl_User.Text = dt1.Rows[0]["Error_User_Name"].ToString();
                                btn_ErrorSub.Text = "Edit";
                                Cbo_Task.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { SplashScreenManager.CloseForm(false); }
        }




        private async void BindgrdError()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                grd_Error.Rows.Clear();
                //Hashtable htselect = new Hashtable();
                //DataTable dtselect = new DataTable();
                //htselect.Add("@Trans", "SELECT");
                //htselect.Add("@Order_ID", orderid);
                //htselect.Add("@User_id", userid);
                //htselect.Add("@Work_Type", Work_Type_Id);
                //dtselect = dataaccess.ExecuteSP("Sp_Error_Info", htselect);
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                DataTable dt1 = new DataTable();
                if (AdminStatus == 2)
                {
                    dictionary.Add("@Trans", "SELECT");
                    dictionary.Add("@Order_ID", orderid);
                    dictionary.Add("@User_id", userid);
                    dictionary.Add("@Work_Type", Work_Type_Id);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindGrdError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            }
                        }
                    }
                }
                else
                {
                    //htselect.Add("@Trans", "BIND_Live");
                    //htselect.Add("@Order_ID", orderid);
                    //htselect.Add("@Work_Type", Work_Type_Id);
                    //dtselect = dataaccess.ExecuteSP("Sp_Error_Info", htselect);                
                    dictionary.Add("@Trans", "BIND_Live");
                    dictionary.Add("@Order_ID", orderid);
                    dictionary.Add("@Work_Type", Work_Type_Id);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindGrdError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            }
                        }
                    }
                }
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        grd_Error.Rows.Add();
                        grd_Error.Rows[i].Cells[0].Value = i + 1;
                        grd_Error.Rows[i].Cells[1].Value = dt1.Rows[i]["New_Error_Type"].ToString();
                        grd_Error.Rows[i].Cells[2].Value = dt1.Rows[i]["Error_Type"].ToString();
                        grd_Error.Rows[i].Cells[3].Value = dt1.Rows[i]["Error_Description"].ToString();
                        grd_Error.Rows[i].Cells[4].Value = dt1.Rows[i]["Comments"].ToString();
                        grd_Error.Rows[i].Cells[5].Value = dt1.Rows[i]["Error_Task"].ToString();
                        grd_Error.Rows[i].Cells[6].Value = dt1.Rows[i]["Error_User_Name"].ToString();
                        grd_Error.Rows[i].Cells[7].Value = "Delete";
                        grd_Error.Rows[i].Cells[8].Value = dt1.Rows[i]["Order_ID"].ToString();
                        grd_Error.Rows[i].Cells[9].Value = dt1.Rows[i]["User_id"].ToString();
                        grd_Error.Rows[i].Cells[10].Value = dt1.Rows[i]["ErrorInfo_ID"].ToString();
                        if (dt1.Rows[i]["Order_Status"].ToString() == "")
                        {
                            grd_Error.Rows[i].Cells[11].Value = "Admin Task";
                        }
                        else
                        {
                            grd_Error.Rows[i].Cells[11].Value = dt1.Rows[i]["Order_Status"].ToString();
                        }
                        grd_Error.Rows[i].Cells[12].Value = dt1.Rows[i]["User_name"].ToString();
                        grd_Error.Rows[i].Cells[13].Value = dt1.Rows[i]["New_Error_Type_Id"].ToString();
                        if (User_Role == "1" || User_Role == "6")
                        {
                            grd_Error.Columns[6].Visible = true;
                            grd_Error.Columns[7].Visible = true;
                        }
                        else
                        {
                            grd_Error.Columns[6].Visible = false;
                            grd_Error.Columns[7].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }
        private async void BindErrorType()
        {
            try
            {
                //Hashtable htselect = new Hashtable();
                //DataTable dtselect = new DataTable();
                //htselect.Add("@Trans", "SELECT_Error_Type");
                //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
                //DataRow dr = dt1.NewRow();
                //dr[0] = 0;
                //dr[0] = "SELECT";
                //dt1.Rows.InsertAt(dr, 0);
                //cbo_ErrorCatogery.DataSource = dt1;
                //cbo_ErrorCatogery.ValueMember = "Error_Type_Id";
                //cbo_ErrorCatogery.DisplayMember = "Error_Type";
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "SELECT_Error_Type");
                }

                // api url
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindErrorTab", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            //
                            DataRow dr = dt1.NewRow();
                            dr[0] = 0;
                            dr[0] = "SELECT";
                            dt1.Rows.InsertAt(dr, 0);
                            cbo_ErrorCatogery.DataSource = dt1;
                            cbo_ErrorCatogery.ValueMember = "Error_Type_Id";
                            cbo_ErrorCatogery.DisplayMember = "Error_Type";

                            //ddl_New_Error_Type.DataSource = dt1;
                            //ddl_New_Error_Type.ValueMember = "New_Error_Type_Id";
                            //ddl_New_Error_Type.DisplayMember = "New_Error_Type";
                        }
                    }

                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }
        }


        private async void Bind_New_ErrorType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true,false);
                //Hashtable ht_select = new Hashtable();
                //DataTable dt_select = new DataTable();
                //ht_select.Add("@Trans", "BIND_NEW_ERROR_TYPE");
                //dt_select = dataaccess.ExecuteSP("Sp_Error_Info", ht_select);
                //DataRow dr = dt_select.NewRow();
                //dr[0] = 0;
                //dr[1] = "SELECT";
                //dt_select.Rows.InsertAt(dr, 0);
                //ddl_New_Error_Type.DataSource = dt_select;
                //ddl_New_Error_Type.ValueMember = "New_Error_Type_Id";
                //ddl_New_Error_Type.DisplayMember = "New_Error_Type";

                IDictionary<string, object> dict_select = new Dictionary<string, object>();
                {
                    dict_select.Add("@Trans", "BIND_NEW_ERROR_TYPE");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict_select), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindNewErrorType", data);
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
                            ddl_New_Error_Type.DataSource = dt;
                            ddl_New_Error_Type.ValueMember = "New_Error_Type_Id";
                            ddl_New_Error_Type.DisplayMember = "New_Error_Type";
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }


        }
        private async void cbo_ErrorCatogery_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    string Error_Type = cbo_ErrorCatogery.Text;
            //    //Hashtable hterror = new Hashtable();
            //    //DataTable dterror = new DataTable();
            //    //hterror.Add("@Trans", "ERROR_TYPE");
            //    //hterror.Add("@Error_Type", Error_Type);
            //    //dterror = dataaccess.ExecuteSP("Sp_Errors_Details", hterror);
            //    //if (dterror.Rows.Count > 0)
            //    //{
            //    //    result = int.Parse(dterror.Rows[0]["Error_Type_Id"].ToString());
            //    //}
            //    IDictionary<string, object> dicterror = new Dictionary<string, object>();
            //    {
            //        dicterror.Add("@Trans", "ERROR_TYPE");
            //        dicterror.Add("@Error_Type", Error_Type);
            //    }
            //    var data = new StringContent(JsonConvert.SerializeObject(dicterror), Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {
            //        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/CboErrorCategorySelectIndexById", data);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            if (response.StatusCode == HttpStatusCode.OK)
            //            {
            //                var result = await response.Content.ReadAsStringAsync();
            //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    result = dt.Rows[0]["Error_Type_Id"].ToString();
            //                }
            //            }
            //        }
            //    }


            //    //Hashtable htselect = new Hashtable();
            //    //DataTable dtselect = new DataTable();
            //    //htselect.Add("@Trans", "SELECT_Error_description");
            //    //htselect.Add("@Error_Type_Id", result);
            //    //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
            //    //DataRow dr = dtselect.NewRow();
            //    //dr[0] = 0;
            //    //dr[0] = "SELECT";
            //    //dtselect.Rows.InsertAt(dr, 0);
            //    //cbo_ErrorDes.DataSource = dtselect;
            //    //cbo_ErrorDes.ValueMember = "Error_description_Id";
            //    //cbo_ErrorDes.DisplayMember = "Error_description";
            //    IDictionary<string, object> dictselect = new Dictionary<string, object>();
            //    {
            //        dicterror.Add("@Trans", "ERROR_TYPE");
            //        dicterror.Add("@Error_Type", Error_Type);
            //    }
            //    var data1 = new StringContent(JsonConvert.SerializeObject(dictselect), Encoding.UTF8, "application/json");
            //    using (var httpClient1 = new HttpClient())
            //    {
            //        var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/CboErrorCategorySelectIndexById", data1);
            //        if (response1.IsSuccessStatusCode)
            //        {
            //            if (response1.StatusCode == HttpStatusCode.OK)
            //            {
            //                var result = await response1.Content.ReadAsStringAsync();
            //                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
            //                DataRow dr = dt1.NewRow();
            //                dr[0] = 0;
            //                dr[0] = "SELECT";
            //                dt1.Rows.InsertAt(dr, 0);
            //                cbo_ErrorDes.DataSource = dt1;
            //                cbo_ErrorDes.ValueMember = "Error_description_Id";
            //                cbo_ErrorDes.DisplayMember = "Error_description";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ throw ex; }
            //finally { SplashScreenManager.CloseForm(false); }
        }
        private bool Validation()
        {
            try
            {
                if (cbo_ErrorCatogery.Text == "" || cbo_ErrorCatogery.SelectedIndex == 0)
                {
                    MessageBox.Show("Select Proper Error Tab");
                    cbo_ErrorCatogery.BackColor = Color.Red;
                    return false;
                }
                if (cbo_ErrorDes.Text == "" || cbo_ErrorDes.SelectedIndex == 0)
                {
                    MessageBox.Show("Select Proper Error Field");
                    cbo_ErrorDes.BackColor = Color.Red;
                    return false;
                }
                if (txt_ErrorCmt.Text == "")
                {
                    MessageBox.Show("Enter Error Comments");
                    txt_ErrorCmt.BackColor = Color.Red;
                    return false;
                }

                if (ddl_New_Error_Type.Text == "" || ddl_New_Error_Type.SelectedIndex == 0)
                {
                    MessageBox.Show("Select Proper  Error Type");
                    ddl_New_Error_Type.BackColor = Color.Red;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }
        private async void btn_ErrorSub_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (Validation() != false)
                {
                    if (btn_ErrorSub.Text == "Submit")
                    {
                        if (Cbo_Task.SelectedIndex != 0)
                        {
                            if (Error_User != 0)
                            {
                                int Ent_error_info_Id = 0;
                                //ErrorType = cbo_ErrorCatogery.Text;
                                //Hashtable htinsert = new Hashtable();
                                //DataTable dtinsert = new DataTable();
                                //htinsert.Add("@Trans", "INSERT");
                                //htinsert.Add("@New_Error_Type_Id", ddl_New_Error_Type.SelectedValue.ToString());  // Added one more column from master New_Error_Type_Id
                                //htinsert.Add("@Error_Type", cbo_ErrorCatogery.SelectedValue.ToString());  // Error_Type means Error_Tab new chnages Has done
                                //htinsert.Add("@Error_Description", cbo_ErrorDes.SelectedValue.ToString());  // error description means error Field new chnages has done
                                //htinsert.Add("@Comments", txt_ErrorCmt.Text);
                                //htinsert.Add("@Task", ORDERTASK);
                                //htinsert.Add("@User_name", Username);
                                //htinsert.Add("@Order_ID", orderid);
                                //htinsert.Add("@Error_Task", Cbo_Task.SelectedValue.ToString());
                                //htinsert.Add("@Error_Status", 1);//New Error
                                //htinsert.Add("@Error_User", Error_User);
                                //htinsert.Add("@User_ID", userid);
                                //htinsert.Add("@Entered_Date", DateTime.Now);
                                //htinsert.Add("@Status", "True");
                                //htinsert.Add("@Work_Type", Work_Type_Id);
                                //htinsert.Add("@Production_Date", Production_Date);
                                //htinsert.Add("@External_Error", false);  //false - It is Internal Error
                                //object Error_info_Id = dataaccess.ExecuteSPForScalar("Sp_Error_Info", htinsert);
                                //int Ent_error_info_Id = int.Parse(Error_info_Id.ToString());
                                IDictionary<string, object> dict_insert = new Dictionary<string, object>();
                                dict_insert.Add("@Trans", "INSERT");
                                dict_insert.Add("@New_Error_Type_Id", ddl_New_Error_Type.SelectedValue.ToString());  // Added one more column from master New_Error_Type_Id
                                dict_insert.Add("@Error_Type", cbo_ErrorCatogery.SelectedValue.ToString());  // Error_Type means Error_Tab new chnages Has done
                                dict_insert.Add("@Error_Description", cbo_ErrorDes.SelectedValue.ToString());  // error description means error Field new chnages has done
                                dict_insert.Add("@Comments", txt_ErrorCmt.Text);
                                dict_insert.Add("@Task", ORDERTASK);
                                dict_insert.Add("@User_name", Username);
                                dict_insert.Add("@Order_ID", orderid);
                                dict_insert.Add("@Error_Task", Cbo_Task.SelectedValue.ToString());
                                dict_insert.Add("@Error_Status", 1);//New Error
                                dict_insert.Add("@Error_User", Error_User);
                                dict_insert.Add("@User_ID", userid);
                                dict_insert.Add("@Entered_Date", DateTime.Now);
                                dict_insert.Add("@Status", "True");
                                dict_insert.Add("@Work_Type", Work_Type_Id);
                                dict_insert.Add("@Production_Date", Production_Date);
                                dict_insert.Add("@External_Error", false);  //false - It is Internal Error
                                var data = new StringContent(JsonConvert.SerializeObject(dict_insert), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/InsertErrorInfo", data);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result = await response.Content.ReadAsStringAsync();
                                            Ent_error_info_Id = JsonConvert.DeserializeObject<int>(result);
                                            
                                        }
                                    }
                                }
                                //Hashtable hterror_history = new Hashtable();
                                //DataTable dterror_history = new DataTable();
                                //hterror_history.Add("@Trans", "INSERT");
                                //hterror_history.Add("@Order_Id", orderid);
                                //hterror_history.Add("@Error_Info_Id", Ent_error_info_Id);
                                //hterror_history.Add("@Comments", "Error Created");
                                //hterror_history.Add("@User_Id", userid);
                                //dterror_history = dataaccess.ExecuteSP("Sp_Error_Info_History", hterror_history);
                                //MessageBox.Show("Error Info Added Successfully");
                                //BindgrdError();
                                //clear();
                                IDictionary<string, object> dict_history = new Dictionary<string, object>();
                                dict_history.Add("@Trans", "INSERT");
                                dict_history.Add("@Order_Id", orderid);
                                dict_history.Add("@Error_Info_Id", Ent_error_info_Id);
                                dict_history.Add("@Comments", "Error Created");
                                dict_history.Add("@User_Id", userid);
                                var data1 = new StringContent(JsonConvert.SerializeObject(dict_history), Encoding.UTF8, "application/json");
                                using (var httpClient1 = new HttpClient())
                                {
                                    var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/InsertErrorHistory", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            //DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result1);
                                            SplashScreenManager.CloseForm(false);
                                            MessageBox.Show("Error Info Added Successfully");
                                        }
                                    }
                                }
                                BindgrdError();
                                clear();
                            }

                            else
                            {
                                MessageBox.Show("Select Properly");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select Task");
                        }
                    }
                    else if (btn_ErrorSub.Text == "Edit" && ErrorInfo_ID != 0)
                    {
                        //Hashtable htupdate = new Hashtable();
                        //DataTable dtupdate = new DataTable();
                        //htupdate.Add("@Trans", "UPDATE");
                        //htupdate.Add("@ErrorInfo_ID", ErrorInfo_ID);
                        //htupdate.Add("@New_Error_Type_Id", ddl_New_Error_Type.SelectedValue.ToString());
                        //htupdate.Add("@Error_Type", cbo_ErrorCatogery.SelectedValue.ToString());
                        //htupdate.Add("@Error_Description", cbo_ErrorDes.SelectedValue.ToString());
                        //htupdate.Add("@Comments", txt_ErrorCmt.Text);
                        //dtupdate = dataaccess.ExecuteSP("Sp_Error_Info", htupdate);
                        IDictionary<string, object> dict_update = new Dictionary<string, object>();
                        dict_update.Add("@Trans", "UPDATE");
                        dict_update.Add("@ErrorInfo_ID", ErrorInfo_ID);
                        dict_update.Add("@New_Error_Type_Id", ddl_New_Error_Type.SelectedValue.ToString());
                        dict_update.Add("@Error_Type", cbo_ErrorCatogery.SelectedValue.ToString());
                        dict_update.Add("@Error_Description", cbo_ErrorDes.SelectedValue.ToString());
                        dict_update.Add("@Comments", txt_ErrorCmt.Text);
                        var data2 = new StringContent(JsonConvert.SerializeObject(dict_update), Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var response2 = await httpClient2.PostAsync(Base_Url.Url + "/Error/UpdateErrorInfo", data2);
                            if (response2.IsSuccessStatusCode)
                            {
                                if (response2.StatusCode == HttpStatusCode.OK)
                                {
                                    var result2 = await response2.Content.ReadAsStringAsync();
                                    DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(result2);
                                }
                            }
                        }
                        BindgrdError();
                        SplashScreenManager.CloseForm(false);
                        MessageBox.Show("Error Info Updated Successfully");
                        clear();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }

        }
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void clear()
        {
            ddl_New_Error_Type.SelectedIndex = 0;
            cbo_ErrorCatogery.SelectedIndex = 0;
            cbo_ErrorDes.SelectedIndex = 0;
            txt_ErrorCmt.Text = "";
            cbo_ErrorCatogery.BackColor = Color.WhiteSmoke;
            cbo_ErrorDes.BackColor = Color.WhiteSmoke;
            txt_ErrorCmt.BackColor = Color.WhiteSmoke;
            Cbo_Task.SelectedIndex = 0;
            Lbl_User.Text = "";
            Error_User = 0;
            btn_ErrorSub.Text = "Submit";
        }
        private async void grd_Error_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.ColumnIndex == 7)
                {
                    string message = "Do you want to delete?";
                    string title = "Close Window";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult show = MessageBox.Show(message, title, buttons);
                    if (show == DialogResult.Yes)
                    {
                        //Hashtable htdel = new Hashtable();
                        //DataTable dtdel = new DataTable();
                        //htdel.Add("@Trans", "DELETE");
                        //htdel.Add("@ErrorInfo_ID", grd_Error.Rows[e.RowIndex].Cells[10].Value);
                        //dtdel = dataaccess.ExecuteSP("Sp_Error_Info", htdel);
                        IDictionary<string, object> dict_del = new Dictionary<string, object>();
                        dict_del.Add("@Trans", "DELETE");
                        dict_del.Add("@ErrorInfo_ID", grd_Error.Rows[e.RowIndex].Cells[10].Value);
                        var data = new StringContent(JsonConvert.SerializeObject(dict_del), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/Error/GridDelete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                }
                            }
                        }

                        //Hashtable hterror_history = new Hashtable();
                        //DataTable dterror_history = new DataTable();
                        //hterror_history.Add("@Trans", "INSERT");
                        //hterror_history.Add("@Order_Id", orderid);
                        //hterror_history.Add("@Error_Info_Id", grd_Error.Rows[e.RowIndex].Cells[10].Value);
                        //hterror_history.Add("@Comments", "Error Deleted");
                        //hterror_history.Add("@User_Id", userid);
                        //dterror_history = dataaccess.ExecuteSP("Sp_Error_Info_History", hterror_history);
                        //MessageBox.Show(grd_Error.Rows[e.RowIndex].Cells[4].Value + "Deleted Successfully");
                        IDictionary<string, object> dicterr_history = new Dictionary<string, object>();
                        dicterr_history.Add("@Trans", "INSERT");
                        dicterr_history.Add("@Order_Id", orderid);
                        dicterr_history.Add("@Error_Info_Id", grd_Error.Rows[e.RowIndex].Cells[10].Value);
                        dicterr_history.Add("@Comments", "Error Deleted");
                        dicterr_history.Add("@User_Id", userid);
                        var data1 = new StringContent(JsonConvert.SerializeObject(dict_del), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/GridHistory", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                    DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result1);


                                }
                            }
                        }
                        SplashScreenManager.CloseForm(false);
                        MessageBox.Show(grd_Error.Rows[e.RowIndex].Cells[4].Value + "Deleted Successfully");
                        BindgrdError();
                    }

                    else if (show == DialogResult.Yes)
                    { SplashScreenManager.CloseForm(false); }
                }

            }
            catch (Exception ex)
            { throw ex; }
        }
        private void cbo_ErrorCatogery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cbo_ErrorDes.Focus();
            }
        }
        private void cbo_ErrorDes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_ErrorCmt.Focus();
            }
        }
        private void txt_ErrorCmt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ErrorSub.Focus();
            }
        }
        private void btn_ErrorSub_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Clear.Focus();
            }
        }
        private async void Cbo_Task_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            //    if (Cbo_Task.SelectedIndex != 0)
            //    {
            //        //Hashtable ht = new Hashtable();
            //        //DataTable dt = new DataTable();
            //        IDictionary<string, object> dict = new Dictionary<string, object>();
            //        if (Work_Type_Id == 1)
            //        {
            //            dict.Add("@Trans", "Task_User");
            //        }
            //        else if (Work_Type_Id == 2)
            //        {
            //            dict.Add("@Trans", "REWORK_TASK_USER");
            //        }
            //        else if (Work_Type_Id == 3)
            //        {
            //            dict.Add("@Trans", "Task_User");
            //        }

            //        dict.Add("@Task", Cbo_Task.SelectedValue.ToString());
            //        dict.Add("@Order_ID",orderid);

                    

            //        if (Sub_ProcessId != 330 && Sub_ProcessId > 0)
            //        {
            //            dict.Add("@Order_ID", Tilte_Exam_Order_Id);
            //        }
            //        else if (Sub_ProcessId > 0)
            //        {
            //            dict.Add("@Order_ID", orderid);
            //        }

            //        //              //  ht.Add("@Order_ID", Order_ID);
            //        //dt = dataaccess.ExecuteSP("Sp_Error_Info", ht);
            //        //if (dt.Rows.Count > 0)
            //        //{
            //        //    Lbl_User.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
            //        //    Error_User = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
            //        //    if (User_Role == "2")
            //        //    {
            //        //        Lbl_User.Text = "**********";
            //        //    }
            //        //}
            //        //else
            //        //{
            //        //    Lbl_User.Text = "";
            //        //    Error_User = 0;
            //        //    chk_Username.Visible = true;
            //        //    dbc.Bind_Users_For_Error_Info(ddl_User);
            //        //}
            //        var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
            //        using (var httpClient = new HttpClient())
            //        {
            //            var response = await httpClient.PostAsync(Base_Url.Url + "/Error/CboTaskSelectIndex", data);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                if (response.StatusCode == HttpStatusCode.OK)
            //                {
            //                    var result = await response.Content.ReadAsStringAsync();
            //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
            //                    if (dt.Rows.Count > 0)
            //                    {
            //                        Lbl_User.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
            //                        Error_User = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
            //                        if (User_Role == "2")
            //                        {
            //                            Lbl_User.Text = "**********";
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Lbl_User.Text = "";
            //                        Error_User = 0;
            //                        chk_Username.Visible = true;

            //                        dbc.Bind_Users_For_Error_Info(ddl_User);
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ddl_User.Visible = false;
            //        Lbl_User.Text = "";
            //        Error_User = 0;
            //    }
            //}
            //catch (Exception ex)
            //{ throw ex; }
            //finally { SplashScreenManager.CloseForm(false); }
        }
        private void ddl_User_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddl_User.SelectedIndex > 0)
            {
                Lbl_User.Text = ddl_User.Text.ToString();
                Error_User = int.Parse(ddl_User.SelectedValue.ToString());
                chk_Username.Checked = false;
            }
            else
            {
                Lbl_User.Text = "";
                Error_User = 0;
            }
        }
        private void chk_Username_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Username.Checked == true)
            {
                ddl_User.Visible = true;
            }
            else
            {
                ddl_User.Visible = false;
            }
        }

        //External Error Entry
        private void BindExternalErrorEntry()
        {
            Hashtable ht_ErrorInfo_Edit = new Hashtable();
            DataTable dt_ErrorInfo_Edit = new DataTable();
            ht_ErrorInfo_Edit.Add("@Trans", "SELECT_BY_ORDER_ID");
            ht_ErrorInfo_Edit.Add("@ErrorInfo_ID", externalErrorInfoId);
            dt_ErrorInfo_Edit = dataaccess.ExecuteSP("Sp_Error_Info", ht_ErrorInfo_Edit);

            if (dt_ErrorInfo_Edit.Rows.Count > 0)
            {
                ddlExternalNewErrorType.SelectedValue = int.Parse(dt_ErrorInfo_Edit.Rows[0]["New_Error_Type_Id"].ToString());
                ddlExternalErrorCategory.SelectedValue = dt_ErrorInfo_Edit.Rows[0]["Error_Type_Id"].ToString();
                ddlExternalErrorDesc.SelectedValue = dt_ErrorInfo_Edit.Rows[0]["Error_description_Id"].ToString();
                ddlExternalTask.SelectedValue = dt_ErrorInfo_Edit.Rows[0]["Error_Task_Id"].ToString();
                txtExternalErrorComment.Text = dt_ErrorInfo_Edit.Rows[0]["Comments"].ToString();

                externalErrorInfoId = int.Parse(dt_ErrorInfo_Edit.Rows[0]["ErrorInfo_ID"].ToString());
                lblExternalUser.Text = dt_ErrorInfo_Edit.Rows[0]["Error_User_Name"].ToString();
                btnSubmitExternalError.Text = "Edit";
                ddlExternalTask.Enabled = false;
            }
        }
        private async void BindGridExternalErrors()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DataTable dt = new DataTable();
                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                if (AdminStatus == 2)
                {
                  
                    dictionary.Add("@Trans", "SELECT_EXTERNAL");
                    dictionary.Add("@Order_ID", orderid);
                    dictionary.Add("@User_id", userid);
                    dictionary.Add("@Work_Type", Work_Type_Id);

                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindGrdError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt = JsonConvert.DeserializeObject<DataTable>(result);

                            }
                        }
                    }
                }
                else
                {
                    dictionary.Add("@Trans", "BIND_Live_External");
                    dictionary.Add("@Order_ID", orderid);
                    dictionary.Add("@Work_Type", Work_Type_Id);
                    //  dictionary = dataaccess.ExecuteSP("Sp_Error_Info", htselect);
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindGrdError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt = JsonConvert.DeserializeObject<DataTable>(result);

                            }
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    gridExternalError.Rows.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        gridExternalError.Rows.Add();
                        gridExternalError.Rows[i].Cells[0].Value = i + 1;
                        gridExternalError.Rows[i].Cells[1].Value = dt.Rows[i]["New_Error_Type"].ToString();
                        gridExternalError.Rows[i].Cells[2].Value = dt.Rows[i]["Error_Type"].ToString();
                        gridExternalError.Rows[i].Cells[3].Value = dt.Rows[i]["Error_Description"].ToString();
                        gridExternalError.Rows[i].Cells[4].Value = dt.Rows[i]["Comments"].ToString();
                        gridExternalError.Rows[i].Cells[5].Value = dt.Rows[i]["Error_Task"].ToString();
                        gridExternalError.Rows[i].Cells[6].Value = dt.Rows[i]["Error_User_Name"].ToString();
                        gridExternalError.Rows[i].Cells[7].Value = "Delete";
                        gridExternalError.Rows[i].Cells[8].Value = dt.Rows[i]["Order_ID"].ToString();
                        gridExternalError.Rows[i].Cells[9].Value = dt.Rows[i]["User_id"].ToString();
                        gridExternalError.Rows[i].Cells[10].Value = dt.Rows[i]["ErrorInfo_ID"].ToString();
                        if (dt.Rows[i]["Order_Status"].ToString() == "")
                        {
                            gridExternalError.Rows[i].Cells[11].Value = "Admin Task";
                        }
                        else
                        {
                            gridExternalError.Rows[i].Cells[11].Value = dt.Rows[i]["Order_Status"].ToString();
                        }
                        gridExternalError.Rows[i].Cells[12].Value = dt.Rows[i]["User_name"].ToString();
                        gridExternalError.Rows[i].Cells[13].Value = dt.Rows[i]["New_Error_Type_Id"].ToString();
                        if (User_Role == "1" || User_Role == "6" || User_Role == "4")
                        {
                            gridExternalError.Columns[6].Visible = true;
                            gridExternalError.Columns[7].Visible = true;
                        }
                        else
                        {
                            gridExternalError.Columns[6].Visible = false;
                            gridExternalError.Columns[7].Visible = false;
                        }
                    }
                }
                
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }
        }
        private async void BindExternalErrorType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                //Hashtable htselect = new Hashtable();
                //DataTable dtselect = new DataTable();
                //htselect.Add("@Trans", "SELECT_Error_Type");
                //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
                //DataRow dr = dtselect.NewRow();
                //dr[0] = 0;
                //dr[0] = "SELECT";
                //dtselect.Rows.InsertAt(dr, 0);
                //ddlExternalErrorCategory.DataSource = dtselect;
                //ddlExternalErrorCategory.ValueMember = "Error_Type_Id";
                //ddlExternalErrorCategory.DisplayMember = "Error_Type";

                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "SELECT_Error_Type");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindErrorTab", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            DataRow dr = dt.NewRow();
                            dr[0] = 0;
                            dr[0] = "SELECT";
                            dt.Rows.InsertAt(dr, 0);
                            ddlExternalErrorCategory.DataSource = dt;
                            ddlExternalErrorCategory.ValueMember = "Error_Type_Id";
                            ddlExternalErrorCategory.DisplayMember = "Error_Type";
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }
        }
        private void txtExternalErrorComment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSubmitExternalError.Focus();
            }
        }
        private void btnSubmitExternalError_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnClearExternalError.Focus();
            }
        }
        private async void gridExternalError_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                 int  erroinfoId=Convert.ToInt32(gridExternalError.Rows[e.RowIndex].Cells[10].Value);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.ColumnIndex == 7)
                {
                    string message = "Do you want to delete?";
                    string title = "Close Window";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult show = MessageBox.Show(message, title, buttons);
                    if (show == DialogResult.Yes)
                    {
                        //Hashtable htdel = new Hashtable();
                        //DataTable dtdel = new DataTable();
                        //htdel.Add("@Trans", "DELETE");
                        //htdel.Add("@ErrorInfo_ID", gridExternalError.Rows[e.RowIndex].Cells[10].Value);
                        //dtdel = dataaccess.ExecuteSP("Sp_Error_Info", htdel);
                        IDictionary<string, object> dict_del = new Dictionary<string, object>();
                        {
                            dict_del.Add("@Trans", "DELETE");
                            dict_del.Add("@ErrorInfo_ID", erroinfoId);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dict_del), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/Error/GridDelete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                   // DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                                }
                            }
                        }

                        //Hashtable hterror_history = new Hashtable();
                        //DataTable dterror_history = new DataTable();
                        //hterror_history.Add("@Trans", "INSERT");
                        //hterror_history.Add("@Order_Id", orderid);
                        //hterror_history.Add("@Error_Info_Id", grd_Error.Rows[e.RowIndex].Cells[10].Value);
                        //hterror_history.Add("@Comments", "Error Deleted");
                        //hterror_history.Add("@User_Id", userid);
                        //dterror_history = dataaccess.ExecuteSP("Sp_Error_Info_History", hterror_history);
                        //MessageBox.Show(grd_Error.Rows[e.RowIndex].Cells[4].Value + "Deleted Successfully");
                        //BindGridExternalErrors();
                        IDictionary<string, object> dicterr_history = new Dictionary<string, object>();
                        dicterr_history.Add("@Trans", "INSERT");
                        dicterr_history.Add("@Order_Id", orderid);
                        dicterr_history.Add("@Error_Info_Id", erroinfoId);
                        dicterr_history.Add("@Comments", "Error Deleted");
                        dicterr_history.Add("@User_Id", userid);
                        var data1 = new StringContent(JsonConvert.SerializeObject(dicterr_history), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/GridHistory", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                    

                                }
                            }
                        }

                        SplashScreenManager.CloseForm(false);
                        MessageBox.Show(gridExternalError.Rows[e.RowIndex].Cells[4].Value + " Deleted Successfully");
                        BindGridExternalErrors();
                    }

                    else if (show == DialogResult.No)
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }

        }

        private async void cbo_ErrorCatogery_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbo_ErrorCatogery.SelectedIndex > 0)
                {

                    
                    int Error_Type = int.Parse(cbo_ErrorCatogery.SelectedValue.ToString());

                    //Hashtable hterror = new Hashtable();
                    //DataTable dterror = new DataTable();
                    //hterror.Add("@Trans", "ERROR_TYPE");
                    //hterror.Add("@Error_Type", Error_Type);
                    //dterror = dataaccess.ExecuteSP("Sp_Errors_Details", hterror);
                    //if (dterror.Rows.Count > 0)
                    //{
                    //    result = int.Parse(dterror.Rows[0]["Error_Type_Id"].ToString());
                    //}
                    //IDictionary<string, object> dicterror = new Dictionary<string, object>();
                    //{
                    //    dicterror.Add("@Trans", "ERROR_TYPE");
                    //    dicterror.Add("@Error_Type", Error_Type);
                    //}
                    //var data = new StringContent(JsonConvert.SerializeObject(dicterror), Encoding.UTF8, "application/json");
                    //using (var httpClient = new HttpClient())
                    //{
                    //    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindInternalField", data);
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        if (response.StatusCode == HttpStatusCode.OK)
                    //        {
                    //            var result = await response.Content.ReadAsStringAsync();
                    //            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                    //            if (dt.Rows.Count > 0)
                    //            {
                    //                result = dt.Rows[0]["Error_Type_Id"].ToString();
                    //            }
                    //        }
                    //    }
                    //}


                    //Hashtable htselect = new Hashtable();
                    //DataTable dtselect = new DataTable();
                    //htselect.Add("@Trans", "SELECT_Error_description");
                    //htselect.Add("@Error_Type_Id", result);
                    //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
                    //DataRow dr = dtselect.NewRow();
                    //dr[0] = 0;
                    //dr[0] = "SELECT";
                    //dtselect.Rows.InsertAt(dr, 0);
                    //cbo_ErrorDes.DataSource = dtselect;
                    //cbo_ErrorDes.ValueMember = "Error_description_Id";
                    //cbo_ErrorDes.DisplayMember = "Error_description";

                    IDictionary<string, object> dictselect = new Dictionary<string, object>();
                    {
                        dictselect.Add("@Trans", "SELECT_Error_description");
                        dictselect.Add("@Error_Type_Id", Error_Type);
                    }
                    var data1 = new StringContent(JsonConvert.SerializeObject(dictselect), Encoding.UTF8, "application/json");
                    using (var httpClient1 = new HttpClient())
                    {
                        var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/BindInternalField", data1);
                        if (response1.IsSuccessStatusCode)
                        {
                            if (response1.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response1.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                DataRow dr = dt1.NewRow();
                                dr[0] = 0;
                                dr[0] = "SELECT";
                                dt1.Rows.InsertAt(dr, 0);
                                cbo_ErrorDes.DataSource = dt1;
                                cbo_ErrorDes.ValueMember = "Error_description_Id";
                                cbo_ErrorDes.DisplayMember = "Error_description";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }
        
        }

        private async void ddlExternalErrorCategory_SelectedValueChanged(object sender, EventArgs e)
         {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                //string externalErrorType = ddlExternalErrorCategory.Text;
                if (ddlExternalErrorCategory.SelectedIndex > 0)
                {
                      int externalErrorType = int.Parse(ddlExternalErrorCategory.SelectedValue.ToString());
                    ////Hashtable hterror = new Hashtable();
                    ////DataTable dterror = new DataTable();
                    ////hterror.Add("@Trans", "ERROR_TYPE");
                    ////hterror.Add("@Error_Type", externalErrorType);
                    ////dterror = dataaccess.ExecuteSP("Sp_Errors_Details", hterror);
                    ////if (dterror.Rows.Count > 0)
                    ////{
                    ////    result = int.Parse(dterror.Rows[0]["Error_Type_Id"].ToString());
                    ////}
                    //IDictionary<string, object> dict_error = new Dictionary<string, object>();
                    //dict_error.Add("@Trans", "ERROR_TYPE");
                    //dict_error.Add("@Error_Type", externalErrorType);
                    //var data = new StringContent(JsonConvert.SerializeObject(dict_error), Encoding.UTF8, "application/json");
                    //using (var httpClient = new HttpClient())
                    //{
                    //    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindExternalField", data);
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        if (response.StatusCode == HttpStatusCode.OK)
                    //        {
                    //            var result = await response.Content.ReadAsStringAsync();
                    //            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                    //            if (dt.Rows.Count > 0)
                    //            {
                    //                result = dt.Rows[0]["Error_Type_Id"].ToString();
                    //            }
                    //        }
                    //    }
                    //}



                    //Hashtable htselect = new Hashtable();
                    //DataTable dtselect = new DataTable();
                    //htselect.Add("@Trans", "SELECT_Error_description");
                    //htselect.Add("@Error_Type_Id", result);
                    //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
                    //DataRow dr = dtselect.NewRow();
                    //dr[0] = 0;
                    //dr[0] = "SELECT";
                    //dtselect.Rows.InsertAt(dr, 0);
                    //ddlExternalErrorDesc.DataSource = dtselect;
                    //ddlExternalErrorDesc.ValueMember = "Error_description_Id";
                    //ddlExternalErrorDesc.DisplayMember = "Error_description";
                    IDictionary<string, object> dict_select = new Dictionary<string, object>();
                    dict_select.Add("@Trans", "SELECT_Error_description");
                    dict_select.Add("@Error_Type_Id", externalErrorType);
                    var data1 = new StringContent(JsonConvert.SerializeObject(dict_select), Encoding.UTF8, "application/json");
                    using (var httpClient1 = new HttpClient())
                    {
                        var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/BindExternalField", data1);
                        if (response1.IsSuccessStatusCode)
                        {
                            if (response1.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response1.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[0] = "SELECT";
                                dt.Rows.InsertAt(dr, 0);
                                ddlExternalErrorDesc.DataSource = dt;
                                ddlExternalErrorDesc.ValueMember = "Error_description_Id";
                                ddlExternalErrorDesc.DisplayMember = "Error_description";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }

        private async void Cbo_Task_SelectedValueChanged(object sender, EventArgs e)
        {
            try
             {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (Cbo_Task.SelectedIndex != 0)
                {
                    //Hashtable ht = new Hashtable();
                    //DataTable dt = new DataTable();
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

                    dict.Add("@Task", Cbo_Task.SelectedValue.ToString());
                    dict.Add("@Order_ID", orderid);



                    if (Sub_ProcessId != 330 && Sub_ProcessId > 0)
                    {
                        dict.Add("@Order_ID", Tilte_Exam_Order_Id);
                    }
                    else if (Sub_ProcessId > 0)
                    {
                        dict.Add("@Order_ID", orderid);
                    }

                    //              //  ht.Add("@Order_ID", Order_ID);
                    //dt = dataaccess.ExecuteSP("Sp_Error_Info", ht);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    Lbl_User.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
                    //    Error_User = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
                    //    if (User_Role == "2")
                    //    {
                    //        Lbl_User.Text = "**********";
                    //    }
                    //}
                    //else
                    //{
                    //    Lbl_User.Text = "";
                    //    Error_User = 0;
                    //    chk_Username.Visible = true;
                    //    dbc.Bind_Users_For_Error_Info(ddl_User);
                    //}
                    var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindInternalUser", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt.Rows.Count > 0)
                                {
                                    Lbl_User.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
                                    Error_User = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
                                    if (User_Role == "2")
                                    {
                                        Lbl_User.Text = "**********";
                                    }
                                }
                                else
                                {
                                    Lbl_User.Text = "";
                                    Error_User = 0;
                                    chk_Username.Visible = true;

                                    dbc.Bind_Users_For_Error_Info(ddl_User);
                                }

                            }
                        }
                        else
                        {
                             
                                Lbl_User.Text = "";
                                Error_User = 0;
                                chk_Username.Visible = true;

                                dbc.Bind_Users_For_Error_Info(ddl_User);
                            
                        }
                    }
                }
                else
                {
                    ddl_User.Visible = false;
                    Lbl_User.Text = "";
                    Error_User = 0;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }
        }

        private async void ddlExternalTask_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlExternalTask.SelectedIndex != 0)
                {
                    //Hashtable ht = new Hashtable();
                    //DataTable dt = new DataTable();
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

                    dict.Add("@Task", ddlExternalTask.SelectedValue.ToString());
                    dict.Add("@Order_ID", orderid);

                    if (Sub_ProcessId != 330 && Sub_ProcessId > 0)
                    {
                        dict.Add("@Order_ID", Tilte_Exam_Order_Id);
                    }
                    else if (Sub_ProcessId > 0)
                    {
                        dict.Add("@Order_ID", orderid);
                    }

                    // //          //  ht.Add("@Order_ID", Order_ID);
                    //dt = dataaccess.ExecuteSP("Sp_Error_Info", ht);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    lblExternalUser.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
                    //    externalErrorUser = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
                    //    if (User_Role == "2")
                    //    {
                    //        lblExternalUser.Text = "**********";
                    //    }
                    //}
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
                                    lblExternalUser.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
                                    externalErrorUser = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
                                    if (User_Role == "2")
                                    {
                                        lblExternalUser.Text = "**********";
                                    } 
                                }
                                else
                                {
                                    lblExternalUser.Text = "";
                                    Error_User = 0;
                                    checkBoxExternalUsername.Visible = true;
                                    dbc.Bind_Users_For_Error_Info(ddlExternalUser);
                                }

                            }
                        }
                        else
                        {
                            lblExternalUser.Text = "";
                            Error_User = 0;
                            checkBoxExternalUsername.Visible = true;
                            dbc.Bind_Users_For_Error_Info(ddlExternalUser);
                        }
                    }
                }
                else
                {
                    ddlExternalUser.Visible = false;
                    lblExternalUser.Text = "";
                    externalErrorUser = 0;
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { SplashScreenManager.CloseForm(false); }
        }

        private async void ddlExternalTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (ddlExternalTask.SelectedIndex != 0)
            //    {
            //        //Hashtable ht = new Hashtable();
            //        //DataTable dt = new DataTable();
            //        IDictionary<string, object> dict = new Dictionary<string, object>();
            //        if (Work_Type_Id == 1)
            //        {
            //            dict.Add("@Trans", "Task_User");
            //        }
            //        else if (Work_Type_Id == 2)
            //        {
            //            dict.Add("@Trans", "REWORK_TASK_USER");
            //        }
            //        else if (Work_Type_Id == 3)
            //        {
            //            dict.Add("@Trans", "Task_User");
            //        }

            //        dict.Add("@Task", ddlExternalTask.SelectedValue.ToString());

            //        //         // ht.Add("@Order_ID", orderid);

            //        if (Sub_ProcessId != 330 && Sub_ProcessId > 0)
            //        {
            //            dict.Add("@Order_ID", Tilte_Exam_Order_Id);
            //        }
            //        else if (Sub_ProcessId > 0)
            //        {
            //            dict.Add("@Order_ID", orderid);
            //        }

            //        // //          //  ht.Add("@Order_ID", Order_ID);
            //        //dt = dataaccess.ExecuteSP("Sp_Error_Info", ht);
            //        //if (dt.Rows.Count > 0)
            //        //{
            //        //    lblExternalUser.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
            //        //    externalErrorUser = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
            //        //    if (User_Role == "2")
            //        //    {
            //        //        lblExternalUser.Text = "**********";
            //        //    }
            //        //}
            //        var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
            //        using (var httpClient = new HttpClient())
            //        {
            //            var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindExternalUser", data);
            //            if (response.IsSuccessStatusCode)
            //            {
            //                if (response.StatusCode == HttpStatusCode.OK)
            //                {
            //                    var result = await response.Content.ReadAsStringAsync();
            //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
            //                    if (dt.Rows.Count > 0)
            //                     {
            //                        lblExternalUser.Text = dt.Rows[dt.Rows.Count - 1]["User_Name"].ToString();
            //                        externalErrorUser = int.Parse(dt.Rows[dt.Rows.Count - 1]["User_id"].ToString());
            //                        if (User_Role == "2")
            //                        {
            //                            lblExternalUser.Text = "**********";
            //                        }
            //                        else
            //                        {
            //                            lblExternalUser.Text = "";
            //                            Error_User = 0;
            //                            checkBoxExternalUsername.Visible = true;
            //                            dbc.Bind_Users_For_Error_Info(ddlExternalUser);
            //                        }
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ddlExternalUser.Visible = false;
            //        lblExternalUser.Text = "";
            //        externalErrorUser = 0;
            //    }
            //}
            //catch (Exception ex)
            //{ throw ex;}
            //finally
            //{ SplashScreenManager.CloseForm(false); }
        }
        private void ddlExternalUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExternalUser.SelectedIndex > 0)
            {
                lblExternalUser.Text = ddlExternalUser.Text.ToString();
                externalErrorUser = int.Parse(ddlExternalUser.SelectedValue.ToString());
                checkBoxExternalUsername.Checked = false;
            }
            else
            {
                lblExternalUser.Text = "";
                externalErrorUser = 0;
            }
        }
        private void checkBoxExternalUsername_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExternalUsername.Checked)
            {
                ddlExternalUser.Visible = true;
            }
            else
            {
                ddlExternalUser.Visible = false;
            }
        }
        private async void BindExternalNewErrorType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                //    Hashtable ht_select = new Hashtable();
                //    DataTable dt_select = new DataTable();
                //    ht_select.Add("@Trans", "BIND_NEW_ERROR_TYPE");
                //    dt_select = dataaccess.ExecuteSP("Sp_Error_Info", ht_select);
                //    DataRow dr = dt_select.NewRow();
                //    dr[0] = 0;
                //    dr[1] = "SELECT";
                //    dt_select.Rows.InsertAt(dr, 0);
                //    ddlExternalNewErrorType.DataSource = dt_select;
                //    ddlExternalNewErrorType.ValueMember = "New_Error_Type_Id";
                //    ddlExternalNewErrorType.DisplayMember = "New_Error_Type";


                IDictionary<string, object> dictionary = new Dictionary<string, object>();
                {
                    dictionary.Add("@Trans", "BIND_NEW_ERROR_TYPE");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/BindNewErrorType", data);
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
                            ddlExternalNewErrorType.DataSource = dt;
                            ddlExternalNewErrorType.ValueMember = "New_Error_Type_Id";
                            ddlExternalNewErrorType.DisplayMember = "New_Error_Type";
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            finally { SplashScreenManager.CloseForm(false); }


        }
        private bool ValidateExternalErrors()
        {
            if (ddlExternalErrorCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Select error tab");
                ddlExternalErrorCategory.BackColor = Color.Red;
                return false;
            }
            if (ddlExternalErrorDesc.SelectedIndex == 0)
            {
                MessageBox.Show("Select error field");
                ddlExternalErrorDesc.BackColor = Color.Red;
                return false;
            }
            if (string.IsNullOrEmpty(txtExternalErrorComment.Text.Trim()))
            {
                MessageBox.Show("Enter error comments");
                txtExternalErrorComment.BackColor = Color.Red;
                return false;
            }
            if (ddlExternalNewErrorType.SelectedIndex == 0)
            {
                MessageBox.Show("Select error type");
                ddlExternalNewErrorType.BackColor = Color.Red;
                return false;
            }
            return true;
        }
        private void btnClearExternalError_Click(object sender, EventArgs e)
        {
            clearExternalFields();
        }
        private void clearExternalFields()
        {
            ddlExternalNewErrorType.SelectedIndex = 0;
            ddlExternalErrorCategory.SelectedIndex = 0;
            ddlExternalErrorDesc.SelectedIndex = 0;
            txtExternalErrorComment.Text = "";
            ddlExternalErrorCategory.BackColor = Color.WhiteSmoke;
            ddlExternalErrorDesc.BackColor = Color.WhiteSmoke;
            txtExternalErrorComment.BackColor = Color.WhiteSmoke;
            ddlExternalTask.SelectedIndex = 0;
            lblExternalUser.Text = "";
            externalErrorUser = 0;
            btnSubmitExternalError.Text = "Submit";
        }
        private async void ddlExternalErrorCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            //    string externalErrorType = ddlExternalErrorCategory.Text;
            //    //Hashtable hterror = new Hashtable();
            //    //DataTable dterror = new DataTable();
            //    //hterror.Add("@Trans", "ERROR_TYPE");
            //    //hterror.Add("@Error_Type", externalErrorType);
            //    //dterror = dataaccess.ExecuteSP("Sp_Errors_Details", hterror);
            //    //if (dterror.Rows.Count > 0)
            //    //{
            //    //    result = int.Parse(dterror.Rows[0]["Error_Type_Id"].ToString());
            //    //}
            //    IDictionary<string, object> dict_error = new Dictionary<string, object>();
            //    dict_error.Add("@Trans", "ERROR_TYPE");
            //    dict_error.Add("@Error_Type", externalErrorType);
            //    var data = new StringContent(JsonConvert.SerializeObject(dict_error), Encoding.UTF8, "application/json");
            //    using (var httpClient = new HttpClient())
            //    {
            //        var response = await httpClient.PostAsync(Base_Url.Url + "/Error/ddlExternalError", data);
            //        if (response.IsSuccessStatusCode)
            //        {
            //            if (response.StatusCode == HttpStatusCode.OK)
            //            {
            //                var result = await response.Content.ReadAsStringAsync();
            //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    result = dt.Rows[0]["Error_Type_Id"].ToString();
            //                }
            //            }
            //        }
            //    }



            //    //Hashtable htselect = new Hashtable();
            //    //DataTable dtselect = new DataTable();
            //    //htselect.Add("@Trans", "SELECT_Error_description");
            //    //htselect.Add("@Error_Type_Id", result);
            //    //dtselect = dataaccess.ExecuteSP("Sp_Errors_Details", htselect);
            //    //DataRow dr = dtselect.NewRow();
            //    //dr[0] = 0;
            //    //dr[0] = "SELECT";
            //    //dtselect.Rows.InsertAt(dr, 0);
            //    //ddlExternalErrorDesc.DataSource = dtselect;
            //    //ddlExternalErrorDesc.ValueMember = "Error_description_Id";
            //    //ddlExternalErrorDesc.DisplayMember = "Error_description";
            //    IDictionary<string, object> dict_select = new Dictionary<string, object>();
            //    dict_select.Add("@Trans", "SELECT_Error_description");
            //    dict_select.Add("@Error_Type_Id", result);
            //    var data1 = new StringContent(JsonConvert.SerializeObject(dict_error), Encoding.UTF8, "application/json");
            //    using (var httpClient1 = new HttpClient())
            //    {
            //        var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/ddlExternalError", data1);
            //        if (response1.IsSuccessStatusCode)
            //        {
            //            if (response1.StatusCode == HttpStatusCode.OK)
            //            {
            //                var result = await response1.Content.ReadAsStringAsync();
            //                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
            //                DataRow dr = dt.NewRow();
            //                dr[0] = 0;
            //                dr[0] = "SELECT";
            //                dt.Rows.InsertAt(dr, 0);
            //                ddlExternalErrorDesc.DataSource = dt;
            //                ddlExternalErrorDesc.ValueMember = "Error_description_Id";
            //                ddlExternalErrorDesc.DisplayMember = "Error_description";
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{ throw ex; }
            //finally
            //{ SplashScreenManager.CloseForm(false); }
        }
        private void ddlExternalErrorCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ddlExternalErrorDesc.Focus();
            }
        }
        private void ddlExternalErrorDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtExternalErrorComment.Focus();
            }
        }
        private async void btnSubmitExternalError_Click(object sender, EventArgs e)
        {
            try
            {
               
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (ValidateExternalErrors() != false)
                {
                    if (btnSubmitExternalError.Text == "Submit")
                    {
                        if (ddlExternalTask.SelectedIndex != 0)
                        {

                            if (externalErrorUser != 0)
                            {
                                int Error_info_Id = 0;
                               // externalErrorType = ddlExternalErrorCategory.Text;
                                //Hashtable htinsert = new Hashtable();
                                //DataTable dtinsert = new DataTable();
                                //htinsert.Add("@Trans", "INSERT");
                                //htinsert.Add("@New_Error_Type_Id", ddlExternalNewErrorType.SelectedValue.ToString());  // Added one more column from master New_Error_Type_Id
                                //htinsert.Add("@Error_Type", ddlExternalErrorCategory.SelectedValue.ToString());  // Error_Type means Error_Tab new chnages Has done
                                //htinsert.Add("@Error_Description", ddlExternalErrorDesc.SelectedValue.ToString());  // error description means error Field new chnages has done
                                //htinsert.Add("@Comments", txtExternalErrorComment.Text);
                                //htinsert.Add("@Task", ddlExternalTask.SelectedValue);
                                //htinsert.Add("@User_name", Username);
                                //htinsert.Add("@Order_ID", orderid);
                                //htinsert.Add("@Error_Task", ddlExternalTask.SelectedValue.ToString());
                                //htinsert.Add("@Error_Status", 1);//New Error
                                //htinsert.Add("@Error_User", externalErrorUser);
                                //htinsert.Add("@User_ID", userid);
                                //htinsert.Add("@Entered_Date", DateTime.Now);
                                //htinsert.Add("@Status", "True");
                                //htinsert.Add("@Work_Type", Work_Type_Id);
                                //htinsert.Add("@Production_Date", Production_Date);
                                //htinsert.Add("@External_Error", true); //ture - It is External Error
                                //object Error_info_Id = dataaccess.ExecuteSPForScalar("Sp_Error_Info", htinsert);
                                //int Ent_error_info_Id = int.Parse(Error_info_Id.ToString());
                                IDictionary<string, object> dict_insert = new Dictionary<string, object>();
                                dict_insert.Add("@Trans", "INSERT");
                                dict_insert.Add("@New_Error_Type_Id", ddlExternalNewErrorType.SelectedValue.ToString());  // Added one more column from master New_Error_Type_Id
                                dict_insert.Add("@Error_Type", ddlExternalErrorCategory.SelectedValue.ToString());  // Error_Type means Error_Tab new chnages Has done
                                dict_insert.Add("@Error_Description", ddlExternalErrorDesc.SelectedValue.ToString());  // error description means error Field new chnages has done
                                dict_insert.Add("@Comments", txtExternalErrorComment.Text);
                                dict_insert.Add("@Task", ddlExternalTask.SelectedValue);
                                dict_insert.Add("@User_name", Username);
                                dict_insert.Add("@Order_ID", orderid);
                                dict_insert.Add("@Error_Task", ddlExternalTask.SelectedValue.ToString());
                                dict_insert.Add("@Error_Status", 1);//New Error
                                dict_insert.Add("@Error_User", externalErrorUser);
                                dict_insert.Add("@User_ID", userid);
                                dict_insert.Add("@Entered_Date", DateTime.Now);
                                dict_insert.Add("@Status", "True");
                                dict_insert.Add("@Work_Type", Work_Type_Id);
                                dict_insert.Add("@Production_Date", Production_Date);
                                dict_insert.Add("@External_Error", true);
                                var data = new StringContent(JsonConvert.SerializeObject(dict_insert), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/Error/InsertErrorInfo", data);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result = await response.Content.ReadAsStringAsync();
                                            Error_info_Id = JsonConvert.DeserializeObject<int>(result);
                                        }
                                    }
                                }


                                //Hashtable hterror_history = new Hashtable();
                                //DataTable dterror_history = new DataTable();
                                //hterror_history.Add("@Trans", "INSERT");
                                //hterror_history.Add("@Order_Id", orderid);
                                //hterror_history.Add("@Error_Info_Id", Ent_error_info_Id);
                                //hterror_history.Add("@Comments", "Error Created");
                                //hterror_history.Add("@User_Id", userid);
                                //dterror_history = dataaccess.ExecuteSP("Sp_Error_Info_History", hterror_history);
                                //MessageBox.Show("Error Info Added Successfully");
                                IDictionary<string, object> dict_history = new Dictionary<string, object>();
                                dict_history.Add("@Trans", "INSERT");
                                dict_history.Add("@Order_Id", orderid);
                                dict_history.Add("@Error_Info_Id", Error_info_Id);
                                dict_history.Add("@Comments", "Error Created");
                                dict_history.Add("@User_Id", userid);
                                var data1 = new StringContent(JsonConvert.SerializeObject(dict_history), Encoding.UTF8, "application/json");
                                using (var httpClient1 = new HttpClient())
                                {
                                    var response1 = await httpClient1.PostAsync(Base_Url.Url + "/Error/InsertErrorHistory", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                          //  DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result1);
                                            SplashScreenManager.CloseForm(false);
                                            MessageBox.Show("Error Info Added Successfully");
                                        }
                                    }
                                }
                                BindGridExternalErrors();
                                clearExternalFields();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Select user");
                        }
                    }
                    else if (btnSubmitExternalError.Text == "Edit" && externalErrorInfoId != 0)
                    {
                        //Hashtable htupdate = new Hashtable();
                        //DataTable dtupdate = new DataTable();
                        //htupdate.Add("@Trans", "UPDATE");
                        //htupdate.Add("@ErrorInfo_ID", externalErrorInfoId);
                        //htupdate.Add("@New_Error_Type_Id", ddlExternalNewErrorType.SelectedValue.ToString());
                        //htupdate.Add("@Error_Type", ddlExternalErrorCategory.SelectedValue.ToString());
                        //htupdate.Add("@Error_Description", ddlExternalErrorDesc.SelectedValue.ToString());
                        //htupdate.Add("@Comments", txtExternalErrorComment.Text);
                        //dtupdate = dataaccess.ExecuteSP("Sp_Error_Info", htupdate);                   
                        //MessageBox.Show("Error Info Updated Successfully");
                        IDictionary<string, object> dict_update = new Dictionary<string, object>();
                        dict_update.Add("@Trans", "UPDATE");
                        dict_update.Add("@ErrorInfo_ID", externalErrorInfoId);
                        dict_update.Add("@New_Error_Type_Id", ddlExternalNewErrorType.SelectedValue.ToString());
                        dict_update.Add("@Error_Type", ddlExternalErrorCategory.SelectedValue.ToString());
                        dict_update.Add("@Error_Description", ddlExternalErrorDesc.SelectedValue.ToString());
                        dict_update.Add("@Comments", txtExternalErrorComment.Text);
                        var data2 = new StringContent(JsonConvert.SerializeObject(dict_update), Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var response2 = await httpClient2.PostAsync(Base_Url.Url + "/Error/UpdateErrorInfo", data2);
                            if (response2.IsSuccessStatusCode)
                            {
                                if (response2.StatusCode == HttpStatusCode.OK)
                                {
                                    var result2 = await response2.Content.ReadAsStringAsync();
                                    DataTable dt2 = JsonConvert.DeserializeObject<DataTable>(result2);
                                    SplashScreenManager.CloseForm(false);
                                    MessageBox.Show("Error Info Updated Successfully");
                                }
                            }
                        }
                        BindGridExternalErrors();
                        clearExternalFields();
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
