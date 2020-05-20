﻿using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01
{
    public partial class Check_List_New : Form
    {
        SqlConnection con = new SqlConnection();
        Commonclass Comclass = new Commonclass();
        DataAccess dataaccess = new DataAccess();
        DropDownistBindClass dbc = new DropDownistBindClass();
        DialogResult dialogResult;
        int user_ID, Check, Check_List_Tran_ID = 0, User_ID;
        string User_Name;
        bool check_yes, check_no, ch_Yes, ch_No, Status;
        int Ref_Checklist_Master_Type_Id, Checklist_Id, User_id, Order_Id, Order_ID, Order_Task, OrderType_ABS_Id, Chklist_Client_Trans_ID;
        string Comments, Question;
        int Entered_Count, Question_Count;
        int User_count, Available_count; string Task_id, Order_Type, Order_Type_ABS, Task_Type;

        ReportDocument rpt_Doc = new ReportDocument();
        TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
        TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
        ConnectionInfo crConnectionInfo = new ConnectionInfo();
        Tables CrTables;
        InfiniteProgressBar.clsProgress cProbar = new InfiniteProgressBar.clsProgress();
        string[] FName;
        string Document_Name;

        bool IsOpen = false;

        string Path1;
        string File_Name;
        string Directory_Path;

        string VIew_Type;
        string ORDER_NUMBER, CLIENT_NAME, SUB_CLIENT_NAME;
        int ORDER_STATUS, clientid, subprocessid;
        int Work_Type_Id, Work_Type, Order_Type_Abs_Id, Count = 0;
        string ordernumber, ordertasktype;
        int Error_Count = 0;
        int Error_Tab_Count = 0;
        int Select_Tab_Index;
        int Check_Count = 0;
        int Defined_Tab_Index = 0;

        private string year;
        private string month;
        private string Ftp_Domain_Name;
        private string Ftp_User_Name;
        private string Ftp_Password;
        private string Ftp_Path;
        private string mainPath;
        private string ftpfullpath;
        private NetworkCredential credentials;
        public Check_List_New(int userid, int Clientid, int Subprocessid, string Taskid, string order_type, string tasktype, int orderid, int user_count, int available_count, string Order_Number, string client_Name, string Sub_Client_Name, int Order_Status, int WORK_TYPE_ID, string order_no, string order_task_type, int Ordertype_ABS_ID)
        {
            InitializeComponent();

            clientid = Clientid;
            subprocessid = Subprocessid;
            user_ID = userid;
            ordernumber = order_no;
            ordertasktype = order_task_type;
            Task_id = Taskid;
            Order_Id = orderid;
            Task_Type = tasktype;
            Order_Type = order_type;
            User_count = user_count;
            ORDER_NUMBER = Order_Number;
            CLIENT_NAME = client_Name;
            SUB_CLIENT_NAME = Sub_Client_Name;
            ORDER_STATUS = Order_Status;
            Available_count = available_count;
            Work_Type_Id = WORK_TYPE_ID;
            OrderType_ABS_Id = Ordertype_ABS_ID;
            Order_Task = Order_Status;
        }

        private async void CheckList_Load(object sender, EventArgs e)
        {
            try
            {

              
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                await Bind_Check_List_Questions(1, grd_General_Checklist, 0);
                await Bind_Check_List_Questions(2, grd_AssessorTaxes_Chklist, 0);
                await Bind_Check_List_Questions(3, grd_Deed_Checklist, 0);
                await Bind_Check_List_Questions(4, grd_Mortgage_Checklist, 0);
                await Bind_Check_List_Questions(5, grd_Judgment_Liens_Checklist, 0);
                await Bind_Check_List_Questions(6, grd_Others_Checklist, 0);
                await Bind_Check_List_Questions(7, grd_Client_Specification, clientid);
                await Bind_Check_List_Questions(7, grd_Client_Specification, clientid);

                var dt = dbc.Get_Month_Year();
                if (dt != null && dt.Rows.Count > 0)
                {
                    year = dt.Rows[0]["Year"].ToString();
                    month = dt.Rows[0]["Month"].ToString();
                }
                DataTable dt_ftp_Details = dbc.Get_Ftp_Details();
                if (dt_ftp_Details.Rows.Count > 0)
                {
                    Ftp_Domain_Name = dt_ftp_Details.Rows[0]["Ftp_Host_Name"].ToString();
                    Ftp_User_Name = dt_ftp_Details.Rows[0]["Ftp_User_Name"].ToString();
                    string Ftp_pass = dt_ftp_Details.Rows[0]["Ftp_Password"].ToString();
                    if (Ftp_pass != "")
                    {
                        Ftp_Password = dbc.Decrypt(Ftp_pass);
                    }
                    credentials = new NetworkCredential(@"" + Ftp_User_Name + "", Ftp_Password);
                }
                else
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Ftp File Path was not found; You cannot upload the documents please check with administrator");
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

        private void Bind_Client_View()
        {
            Hashtable ht_Check = new Hashtable();
            DataTable dt_Check = new System.Data.DataTable();

            ht_Check.Add("@Trans", "CHECK_ORDER_ID_TASK_USER_WISE");
            ht_Check.Add("@Ref_Checklist_Master_Type_Id", 7);
            ht_Check.Add("@Order_Id", Order_Id);
            ht_Check.Add("@Order_Task", Order_Task);
            ht_Check.Add("@User_id", user_ID);

            ht_Check.Add("@Work_Type", Work_Type_Id);
            dt_Check = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Check);
            if (dt_Check.Rows.Count != 0)
            {
                Client_View();
            }
            else
            {

                Grid_Bind_All_Clients();
            }
        }

        private async void Grid_Bind_All_Clients()
        {
     
            DataTable dt_Clnt = new System.Data.DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
                {

                    { "@Trans", "GET_CLIENT_DETAILS" },
                    { "@Ref_Checklist_Master_Type_Id", 7},
                    { "@Order_Task", Order_Task },
                    { "@OrderType_ABS_Id", OrderType_ABS_Id},
                    { "@Client_Id", clientid}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllClients", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_Clnt = JsonConvert.DeserializeObject<DataTable>(result);
                            //dt_Clnt = dataaccess.ExecuteSP("SP_Checklist_ClientSpecification_QEntry", ht_Clnt);
                            if (dt_Clnt.Rows.Count > 0)
                            {
                                grd_Client_Specification.Rows.Clear();
                                for (int i = 0; i < dt_Clnt.Rows.Count; i++)
                                {
                                    grd_Client_Specification.Rows.Add();
                                    grd_Client_Specification.Rows[i].Cells[0].Value = i + 1;
                                    grd_Client_Specification.Rows[i].Cells[2].Value = dt_Clnt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[3].Value = dt_Clnt.Rows[i]["Question"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[4].Value = dt_Clnt.Rows[i]["Checklist_Id"].ToString();

                                    grd_Client_Specification.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Client_Specification.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Client_Specification.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                                }
                            }
                            else
                            {
                                grd_Client_Specification.Rows.Clear();
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

        private void Client_View()
        {
            Hashtable ht_Client_List = new Hashtable();
            DataTable dt_Client_List = new DataTable();
            ht_Client_List.Add("@Trans", "GET_ALL_VIEW");
            ht_Client_List.Add("@Ref_Checklist_Master_Type_Id", 7);
            ht_Client_List.Add("@Order_Task", Order_Task);
            ht_Client_List.Add("@Order_Id", Order_Id);
            ht_Client_List.Add("@User_Id", user_ID);
            ht_Client_List.Add("@Work_Type", Work_Type_Id);
            dt_Client_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Client_List);
            if (dt_Client_List.Rows.Count > 0)
            {
                grd_Client_Specification.Rows.Clear();
                for (int i = 0; i < dt_Client_List.Rows.Count; i++)
                {
                    grd_Client_Specification.Rows.Add();
                    grd_Client_Specification.Rows[i].Cells[0].Value = i + 1;
                    grd_Client_Specification.Rows[i].Cells[1].Value = dt_Client_List.Rows[i]["Check_List_Tran_ID"].ToString();
                    grd_Client_Specification.Rows[i].Cells[2].Value = dt_Client_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                    grd_Client_Specification.Rows[i].Cells[3].Value = dt_Client_List.Rows[i]["Question"].ToString();
                    grd_Client_Specification.Rows[i].Cells[4].Value = dt_Client_List.Rows[i]["Checklist_Id"].ToString();
                    grd_Client_Specification.Rows[i].Cells[5].Value = dt_Client_List.Rows[i]["Yes"].ToString();
                    grd_Client_Specification.Rows[i].Cells[6].Value = dt_Client_List.Rows[i]["No"].ToString();

                    string chk_yes = grd_Client_Specification.Rows[i].Cells[5].Value.ToString();
                    string chk_no = grd_Client_Specification.Rows[i].Cells[6].Value.ToString();
                    if (chk_yes == "true")
                    {
                        grd_Client_Specification[5, i].Value = true;
                    }
                    else if (chk_yes == "")
                    {
                        grd_Client_Specification[5, i].Value = null;
                    }
                    if (chk_no == "true")
                    {
                        grd_Client_Specification[6, i].Value = true;
                    }
                    else if (chk_no == "")
                    {
                        grd_Client_Specification[6, i].Value = null;
                    }
                    grd_Client_Specification.Rows[i].Cells[7].Value = dt_Client_List.Rows[i]["Comments"].ToString();
                }
            }
            else
            {
                grd_Client_Specification.Rows.Clear();

                Grid_Bind_All_Clients();
            }
        }

        private async Task Bind_GenralView()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                { "@Trans", "CHECK_ORDER_ID_TASK_USER_WISE" },
                { "@Ref_Checklist_Master_Type_Id", 1 },
                { "@Order_Id", Order_Id },
                { "@Order_Task", Order_Task },
                { "@User_id", user_ID },
                { "@Work_Type", Work_Type_Id }
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindMasterTaskWise", data);
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
                throw ex;

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        public async Task Grid_Bind_All_General()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary dicDetails = new Dictionary<string, object>()
            {

                { "@Trans", "GET_ALL_DETAILS" },
                { "@Ref_Checklist_Master_Type_Id", 1 },
                { "@Order_Task", Order_Task},
                { "@OrderType_ABS_Id", OrderType_ABS_Id}
            };
                var data = new StringContent(JsonConvert.SerializeObject(dicDetails), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindMasterDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_General_Checklist.Rows.Clear();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    grd_General_Checklist.Rows.Add();
                                    grd_General_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_General_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                                    grd_General_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_General_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_General_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                                }
                            }
                            else
                            {
                                grd_General_Checklist.Rows.Clear();
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

        private async Task General_View()
        {
            //Hashtable ht_general_list = new Hashtable();
            //DataTable dt_general_list = new DataTable();
            try
            {
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
              {
                 {"@Trans", "GET_ALL_VIEW" },
                {"@Ref_Checklist_Master_Type_Id", 1 },
                {"@Order_Task", Order_Task },
                {"@Order_Id", Order_Id },
                {"@User_Id", user_ID },
                {"@Work_Type", Work_Type_Id },
             };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindMasterDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    grd_General_Checklist.Rows.Clear();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        grd_General_Checklist.Rows.Add();
                                        grd_General_Checklist.Rows[i].Cells[0].Value = i + 1;
                                        grd_General_Checklist.Rows[i].Cells[1].Value = dt.Rows[i]["Check_List_Tran_ID"].ToString();
                                        grd_General_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                        grd_General_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                                        grd_General_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();
                                        grd_General_Checklist.Rows[i].Cells[5].Value = dt.Rows[i]["Yes"].ToString();
                                        grd_General_Checklist.Rows[i].Cells[6].Value = dt.Rows[i]["No"].ToString();

                                        string chk_yes = grd_General_Checklist.Rows[i].Cells[5].Value.ToString();
                                        string chk_no = grd_General_Checklist.Rows[i].Cells[6].Value.ToString();
                                        if (chk_yes == "true")
                                        {
                                            grd_General_Checklist[5, i].Value = true;
                                        }
                                        else if (chk_yes == "")
                                        {
                                            grd_General_Checklist[5, i].Value = null;
                                        }
                                        if (chk_no == "true")
                                        {
                                            grd_General_Checklist[6, i].Value = true;
                                        }
                                        else if (chk_no == "")
                                        {
                                            grd_General_Checklist[6, i].Value = null;
                                        }
                                        grd_General_Checklist.Rows[i].Cells[7].Value = dt.Rows[i]["Comments"].ToString();

                                        grd_General_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_General_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_General_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    }
                                }
                                else
                                {
                                    grd_General_Checklist.Rows.Clear();
                                    Grid_Bind_All_General();

                                }
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

            }
        }


        private async Task Save_General_List()
        {
            int inertval = 0;
            int error = 0;
            int comm_error = 0;
            int empty = 0;
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                for (int i = 0; i < grd_General_Checklist.Rows.Count; i++)
                {

                    grd_General_Checklist.Rows[i].DefaultCellStyle.BackColor = SystemColors.Control;

                    bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column5"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column7"].FormattedValue);

                    if (chk_yes != null && chk_yes != false)
                    {
                        chk_yes = true;
                    }
                    else
                    {
                        chk_yes = false;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        chk_no = true;
                    }
                    else
                    {
                        chk_no = false;
                    }


                    if (grd_General_Checklist[7, i].Value == null || grd_General_Checklist[7, i].Value == "")
                    {
                        if (chk_no == true)
                        {
                            grd_General_Checklist[7, i].Style.BackColor = Color.Red;

                        }

                        Comments = "";


                    }
                    else
                    {
                        grd_General_Checklist[7, i].Style.BackColor = Color.White;
                        Comments = grd_General_Checklist.Rows[i].Cells[7].Value.ToString();
                    }

                    if (grd_General_Checklist.Rows[i].Cells[5].Style.BackColor != Color.Red && grd_General_Checklist.Rows[i].Cells[6].Style.BackColor != Color.Red)
                    {
                        if (grd_General_Checklist[7, i].Style.BackColor != Color.Red)
                        {
                            //  Check_List_Tran_ID = int.Parse(grd_General_Checklist.Rows[i].Cells[1].Value.ToString());
                            Ref_Checklist_Master_Type_Id = int.Parse(grd_General_Checklist.Rows[i].Cells[2].Value.ToString());
                            Checklist_Id = int.Parse(grd_General_Checklist.Rows[i].Cells[4].Value.ToString());
                            Question = grd_General_Checklist.Rows[i].Cells[3].Value.ToString();

                            IDictionary<string, object> dic_Check = new Dictionary<string, object>();
                            {

                                dic_Check.Add("@Trans", "CHECK");
                                dic_Check.Add("@Checklist_Id", Checklist_Id);
                                dic_Check.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                                //  htcheck.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                                dic_Check.Add("@User_id", user_ID);
                                dic_Check.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                                dic_Check.Add("@Order_Id", Order_Id);
                                dic_Check.Add("@Order_Task", Order_Task);
                                dic_Check.Add("@Work_Type", Work_Type_Id);
                            }

                            var data = new StringContent(JsonConvert.SerializeObject(dic_Check), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/CheckCheckList", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                                        if (dt.Rows.Count > 0)
                                        {

                                            Check_List_Tran_ID = int.Parse(dt.Rows[0]["Check_List_Tran_ID"].ToString());
                                            //Hashtable ht_Chklist = new Hashtable();
                                            //DataTable dt_Chklist = new DataTable();
                                            IDictionary<string, object> dic_update = new Dictionary<string, object>();
                                            {
                                                dic_update.Add("@Trans", "UPDATE");
                                                dic_update.Add("@Check_List_Tran_ID", Check_List_Tran_ID);
                                                dic_update.Add("@Checklist_Id", Checklist_Id);
                                                dic_update.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                                                //  ht_Chklist.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                                                dic_update.Add("@Yes", chk_yes);
                                                dic_update.Add("@No", chk_no);
                                                dic_update.Add("@Order_Id", Order_Id);
                                                dic_update.Add("@Order_Task", Order_Task);
                                                dic_update.Add("@Work_Type", Work_Type_Id);
                                                dic_update.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                                                dic_update.Add("@Comments", Comments);
                                                dic_update.Add("@Status", "True");
                                                dic_update.Add("@User_id", user_ID);
                                                dic_update.Add("@Modified_By", user_ID);
                                                dic_update.Add("@Modified_Date", DateTime.Now);
                                            }
                                            var data1 = new StringContent(JsonConvert.SerializeObject(dic_update), Encoding.UTF8, "application/json");
                                            using (var httpClient1 = new HttpClient())
                                            {
                                                var response1 = await httpClient1.PutAsync(Base_Url.Url + "/CheckList/UpdateGeneralList", data1);
                                                if (response1.IsSuccessStatusCode)
                                                {
                                                    if (response1.StatusCode == HttpStatusCode.OK)
                                                    {
                                                        var result1 = await response.Content.ReadAsStringAsync();
                                                    }
                                                }
                                            }
                                        }
                                        else if (dt.Rows.Count == 0)
                                        {
                                            //    Hashtable ht_Chk_list = new Hashtable();
                                            //    DataTable dt_Chk_list = new DataTable();
                                            IDictionary<string, object> dic_insert = new Dictionary<string, object>();
                                            {
                                                dic_insert.Add("@Trans", "INSERT");
                                                dic_insert.Add("@Checklist_Id", Checklist_Id);
                                                dic_insert.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                                                //ht_Chk_list.Add("@Question", Question);
                                                dic_insert.Add("@Yes", chk_yes);
                                                dic_insert.Add("@No", chk_no);
                                                dic_insert.Add("@Order_Id", Order_Id);
                                                dic_insert.Add("@Order_Task", Order_Task);
                                                dic_insert.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                                                dic_insert.Add("@Work_Type", Work_Type_Id);
                                                dic_insert.Add("@Comments", Comments);
                                                dic_insert.Add("@Status", "True");
                                                dic_insert.Add("@User_id", user_ID);
                                                dic_insert.Add("@Inserted_Date", DateTime.Now);
                                            }
                                            var data2 = new StringContent(JsonConvert.SerializeObject(dic_insert), Encoding.UTF8, "application/json");
                                            using (var httpClient2 = new HttpClient())
                                            {
                                                var response2 = await httpClient2.PostAsync(Base_Url.Url + "/CheckList/InsertGeneralList", data2);
                                                if (response2.IsSuccessStatusCode)
                                                {
                                                    if (response2.StatusCode == HttpStatusCode.OK)
                                                    {
                                                        object result2 = await response.Content.ReadAsStringAsync();
                                                        int checklistId = int.Parse(result2.ToString());

                                                    }
                                                }
                                            }
                                            //object dtcount = dataaccess.ExecuteSPForScalar("Sp_Checklist_Detail", ht_Chk_list);

                                            //int checklistId = int.Parse(dtcount.ToString());

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
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }
        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (Validate_Genral_Question_New() != false)
            {

                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage2");

                //  Save_General_List();

            }
        }
        private bool Validate_Genral_Question_New()
        {

            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_General_Checklist.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column5"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column7"].FormattedValue);




                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_General_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_General_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;

                    Error_Tab_Count = 1;
                }
                else
                {

                    grd_General_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_General_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;
                }

                if (grd_General_Checklist[7, i].Value == null || grd_General_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_General_Checklist[7, i].Style.BackColor = Color.Red;

                    }

                    Comments = "";


                }
                else
                {
                    grd_General_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_General_Checklist.Rows[i].Cells[7].Value.ToString();
                }

                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_General_Checklist[7, i].Style.BackColor = Color.Red;
                    break;
                }
                else

                {

                    Error_Count = 0;
                }



            }

            if (grd_General_Checklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_General_Checklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 1;
                MessageBox.Show("Need to Enter all Fields");
                return false;
            }

        }
        private void btn_General_Save_Click(object sender, EventArgs e)
        {
            Save_General_List();
        }

        private async void btn_General_View_Detail_Click(object sender, EventArgs e)
        {
            try
            {

                           IDictionary<string, object> dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "GET_ALL_VIEW" },
                    { "@Ref_Checklist_Master_Type_Id", 1}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_general_list = JsonConvert.DeserializeObject<DataTable>(result);

                            if (dt_general_list.Rows.Count > 0)
                            {
                                grd_General_Checklist.Rows.Clear();
                                for (int i = 0; i < dt_general_list.Rows.Count; i++)
                                {
                                    grd_General_Checklist.Rows.Add();
                                    grd_General_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_General_Checklist.Rows[i].Cells[1].Value = dt_general_list.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[2].Value = dt_general_list.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[3].Value = dt_general_list.Rows[i]["Question"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[4].Value = dt_general_list.Rows[i]["Checklist_Id"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[5].Value = dt_general_list.Rows[i]["Yes"].ToString();
                                    grd_General_Checklist.Rows[i].Cells[6].Value = dt_general_list.Rows[i]["No"].ToString();

                                    string chk_yes = grd_General_Checklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_General_Checklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_General_Checklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_General_Checklist[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_General_Checklist[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_General_Checklist[6, i].Value = null;
                                    }
                                    grd_General_Checklist.Rows[i].Cells[7].Value = dt_general_list.Rows[i]["Comments"].ToString();

                                    grd_General_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_General_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_General_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }
                            else
                            {
                                grd_General_Checklist.Rows.Clear();
                                Grid_Bind_All_General();
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



        private void grd_General_Checklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[e.RowIndex].Cells["Column5"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[e.RowIndex].Cells["Column7"].FormattedValue);

                    if (chk_yes != false)
                    {
                        grd_General_Checklist[6, e.RowIndex].Value = false;
                        grd_General_Checklist[5, e.RowIndex].Value = true;

                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = true;


                    }
                    else if (chk_yes != true)
                    {
                        grd_General_Checklist[5, e.RowIndex].Value = true;
                        grd_General_Checklist[6, e.RowIndex].Value = false;
                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        //MessageBox.Show("Enter Comments");
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_General_Checklist[7, e.RowIndex].Value = "";
                    }

                }

                if (e.ColumnIndex == 6)
                {
                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[e.RowIndex].Cells["Column7"].FormattedValue);

                    if (chk_no != false)
                    {
                        grd_General_Checklist[6, e.RowIndex].Value = true;
                        grd_General_Checklist[5, e.RowIndex].Value = false;
                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_General_Checklist[6, e.RowIndex].Value = false;
                            grd_General_Checklist[5, e.RowIndex].Value = false;
                            grd_General_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_General_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_General_Checklist[7, e.RowIndex].Value = "";
                        }

                    }
                    else if (chk_no != true)
                    {
                        grd_General_Checklist[5, e.RowIndex].Value = false;
                        grd_General_Checklist[6, e.RowIndex].Value = true;
                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_General_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_General_Checklist[7, e.RowIndex].Value = "";
                    }

                }

                if (e.ColumnIndex == 7)
                {


                    bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[e.RowIndex].Cells["Column5"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[e.RowIndex].Cells["Column7"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = true;
                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

                    }
                    else if (chk_yes != true)
                    {

                        grd_General_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_General_Checklist[7, e.RowIndex].Style.BackColor = Color.White;

                    }

                    if (chk_yes == false && chk_no == false)
                    {

                        grd_General_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_General_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_General_Checklist[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_General_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_General_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }



                }
            }

        }

        private void grd_General_Checklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

       
        }


        private void grd_General_Checklist_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {


            //if ((char)(((System.Windows.Forms.DataGridView)(sender)).CurrentCell.ColumnIndex) == 7)
            //{
            //    e.Control.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextboxNumeric_KeyPress1);

            //}

        }


        // Assessor and taxes
        public async void Grid_Bind_All_AssessorTax()
        {
            //Hashtable ht = new Hashtable();
            DataTable dt = new DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                    { "@Trans", "GET_ALL_DETAILS" },
                    { "@Ref_Checklist_Master_Type_Id", 2 },
                    { "@Order_Task", Order_Task },
                    { "@OrderType_ABS_Id", OrderType_ABS_Id }
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindMasterDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                //dt = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht);
                                if (dt.Rows.Count > 0)
                                {
                                    grd_AssessorTaxes_Chklist.Rows.Clear();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        grd_AssessorTaxes_Chklist.Rows.Add();
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[0].Value = i + 1;
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    }
                                }
                                else
                                {
                                    grd_AssessorTaxes_Chklist.Rows.Clear();
                                }
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

        private bool Validate_AssessorTax_New()
        {
            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_AssessorTaxes_Chklist.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column14"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column15"].FormattedValue);




                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_AssessorTaxes_Chklist[7, i].Value == null || grd_AssessorTaxes_Chklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_AssessorTaxes_Chklist[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";

                }
                else
                {
                    grd_AssessorTaxes_Chklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value.ToString();
                }


                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_AssessorTaxes_Chklist[7, i].Style.BackColor = Color.Red;

                    break;
                }

                else
                {

                    Error_Count = 0;
                }




            }

            if (grd_AssessorTaxes_Chklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_AssessorTaxes_Chklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 2;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }


        }

        private void btn_Assessor_Tax_Next_Click(object sender, EventArgs e)
        {
            //Save_AssessorTaxes_List();               //07-07-2017
            //tabControl1.SelectTab("tabPage3");          //07-07-2017

            if (Validate_AssessorTax_New() != false)
            {
                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage3");

            }

            //  Save_Assessor_Tax_List();                         // 10-07-2017

        }

        private async void btn_AssessorTaxe_Save_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                int inertval = 0;
                for (int i = 0; i < grd_AssessorTaxes_Chklist.Rows.Count; i++)
                {

                    if (grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value != "" && grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value != null)
                    {
                        Comments = grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value.ToString();
                    }
                    bool check = false;
                    bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column14"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column15"].FormattedValue);

                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_AssessorTaxes_Chklist.Rows[i].Cells["Column15"].ReadOnly = true;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        check = false;
                    }
                    Ref_Checklist_Master_Type_Id = int.Parse(grd_AssessorTaxes_Chklist.Rows[i].Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(grd_AssessorTaxes_Chklist.Rows[i].Cells[4].Value.ToString());
                    Question = grd_AssessorTaxes_Chklist.Rows[i].Cells[3].Value.ToString();

                    if (Check_List_Tran_ID == 0)
                    {
                        //Hashtable ht_Chklist = new Hashtable();
                        //DataTable dt_Chklist = new DataTable();
                        IDictionary<string, object> dictionary = new Dictionary<string, object>()
                      {

                        { "@Trans", "INSERT" },
                         { "@Checklist_Id", Checklist_Id},
                         { "@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id},
                         { "@Order_Type_Abs_Id", OrderType_ABS_Id},
                         { "@Checked", check},
                         { "@Order_Id", Order_Id},
                         { "@Order_Task", Order_Task},
                         { "@Comments", Comments},
                         { "@Status", "True"},
                         { "@User_id", user_ID},
                         { "@Inserted_Date", DateTime.Now}
                      };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertTabsClick", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    //dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
                                    inertval = 1;
                                }

                            }
                        }
                    }
                }
                if (inertval == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Assessor/Taxes CheckList Added Successfully");
                    Grid_Bind_All_AssessorTax();

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

        private async void btn_Assessor_Liens_View_Click(object sender, EventArgs e)
        {

            //Hashtable ht_Asses_Tax_List = new Hashtable();
            //DataTable dt_Asses_Tax_List = new DataTable();
            //ht_Asses_Tax_List.Add("@Trans", "ALL_ASSESSOR_TAXES");
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                  { "@Trans", "GET_ALL_VIEW" },
                  { "@Ref_Checklist_Master_Type_Id", 2 }
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_Asses_Tax_List = JsonConvert.DeserializeObject<DataTable>(result);
                            //dt_Asses_Tax_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Asses_Tax_List);
                            if (dt_Asses_Tax_List.Rows.Count > 0)
                            {
                                grd_AssessorTaxes_Chklist.Rows.Clear();
                                for (int i = 0; i < dt_Asses_Tax_List.Rows.Count; i++)
                                {
                                    grd_AssessorTaxes_Chklist.Rows.Add();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[1].Value = dt_Asses_Tax_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[2].Value = dt_Asses_Tax_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[3].Value = dt_Asses_Tax_List.Rows[i]["Question"].ToString();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[4].Value = dt_Asses_Tax_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Value = dt_Asses_Tax_List.Rows[i]["Yes"].ToString();
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Value = dt_Asses_Tax_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "True")
                                    {
                                        grd_AssessorTaxes_Chklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_AssessorTaxes_Chklist[5, i].Value = null;
                                    }
                                    if (chk_no == "False")
                                    {
                                        grd_AssessorTaxes_Chklist[6, i].Value = false;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_AssessorTaxes_Chklist[6, i].Value = null;
                                    }
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value = dt_Asses_Tax_List.Rows[i]["Comments"].ToString();


                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_AssessorTaxes_Chklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }
                            else
                            {
                                grd_AssessorTaxes_Chklist.Rows.Clear();
                                // Grid_Bind_Assessor_Taxes_CheckList();
                                Grid_Bind_All_AssessorTax();
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



        private void grd_AssessorTaxes_Chklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);

                    if (chk_yes != false)
                    {
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = true;
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Value = "";
                    }


                }
                if (e.ColumnIndex == 6)
                {
                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);

                    if (chk_no != false)
                    {
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
                            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
                            grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
                            grd_AssessorTaxes_Chklist[7, e.RowIndex].Value = "";
                        }
                    }
                    else if (chk_no != true)
                    {
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_AssessorTaxes_Chklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Value = "";
                    }
                }

                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

                    }
                    else if (chk_yes != true)
                    {
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = Color.White;
                    }

                    if (chk_yes == false && chk_no == false)
                    {
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }
                }
            }





            //*************11-07-2017***************

            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex == 5)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);

            //        //if (chk_yes != null && chk_yes != false)
            //        //{
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

            //        //}
            //        //else if (chk_yes != true)
            //        //{
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = true;
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //}

            //        //if (chk_yes != false)
            //        //{
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = true;

            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;

            //        //}
            //        //else if (chk_yes != true)
            //        //{
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = true;
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    grd_AssessorTaxes_Chklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    grd_AssessorTaxes_Chklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
            //        //    //MessageBox.Show("Enter Comments");
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
            //        //    grd_AssessorTaxes_Chklist[7, e.RowIndex].Value = "";
            //        //}




            //    }
            //    if (e.ColumnIndex == 6)
            //    {
            //        bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);
            //        if (chk_no != null && chk_no != false)
            //        {
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;

            //        }
            //        else if (chk_no != true)
            //        {
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = Color.White;
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
            //        }

            //    }

            //    if (e.ColumnIndex == 7)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);
            //        if (chk_yes != null && chk_yes != false)
            //        {
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

            //        }
            //        else if (chk_yes != true)
            //        {

            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = false;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].Style.BackColor = Color.White;

            //        }

            //        if (chk_yes == false && chk_no == false)
            //        {

            //            grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
            //            grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
            //            grd_AssessorTaxes_Chklist[7, e.RowIndex].ReadOnly = true;
            //        }
            //        else
            //        {
            //            grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
            //            grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
            //        }

            //    }
            //}

            //***********************************************************************
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex == 5)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);

            //        if (chk_yes != null && chk_yes != false)
            //        {
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //        }
            //        else if (chk_yes != true)
            //        {
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = true;
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = false;
            //        }
            //    }
            //    if (e.ColumnIndex == 6)
            //    {
            //        bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);
            //        if (chk_no != null && chk_no != false)
            //        {
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //        }
            //        else if (chk_no != true)
            //        {
            //            grd_AssessorTaxes_Chklist[5, e.RowIndex].Value = false;
            //            grd_AssessorTaxes_Chklist[6, e.RowIndex].Value = true;
            //        }
            //    }

            //}
        }

        //private void Save_AssessorTaxes_List()
        //{
        //    for (int i = 0; i < grd_AssessorTaxes_Chklist.Rows.Count; i++)
        //    {
        //        if (grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value != "" && grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value != null)
        //        {
        //            Comments = grd_AssessorTaxes_Chklist.Rows[i].Cells[7].Value.ToString();
        //        }
        //        bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column14"].FormattedValue);
        //        bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells["Column15"].FormattedValue);

        //        if (chk_yes != null && chk_yes != false)
        //        {
        //            chk_yes = true;

        //        }
        //        else
        //        {
        //            chk_yes = false;
        //        }
        //        if (chk_no != null && chk_no != false)
        //        {
        //            chk_no = true;
        //        }
        //        else
        //        {
        //            chk_no = false;
        //        }
        //        Ref_Checklist_Master_Type_Id = int.Parse(grd_AssessorTaxes_Chklist.Rows[i].Cells[2].Value.ToString());
        //        Checklist_Id = int.Parse(grd_AssessorTaxes_Chklist.Rows[i].Cells[4].Value.ToString());
        //        Question = grd_AssessorTaxes_Chklist.Rows[i].Cells[3].Value.ToString();

        //        Hashtable htcheck = new Hashtable();
        //        DataTable dtcheck = new DataTable();
        //        htcheck.Add("@Trans", "CHECK");
        //        htcheck.Add("@Checklist_Id", Checklist_Id);
        //        htcheck.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
        //       htcheck.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
        //        htcheck.Add("@User_id", user_ID);
        //        htcheck.Add("@Order_Id", Order_Id);
        //        htcheck.Add("@Order_Task", Order_Task);

        //        dtcheck = dataaccess.ExecuteSP("Sp_Checklist_Detail", htcheck);
        //        if (dtcheck.Rows.Count > 0)
        //        {
        //            Check_List_Tran_ID = int.Parse(dtcheck.Rows[0]["Check_List_Tran_ID"].ToString());
        //            Hashtable ht_Chklist = new Hashtable();
        //            DataTable dt_Chklist = new DataTable();

        //            ht_Chklist.Add("@Trans", "UPDATE");
        //            ht_Chklist.Add("@Check_List_Tran_ID", Check_List_Tran_ID);
        //            ht_Chklist.Add("@Checklist_Id", Checklist_Id);
        //            ht_Chklist.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
        //             ht_Chklist.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
        //            ht_Chklist.Add("@Yes", chk_yes);
        //            ht_Chklist.Add("@No", chk_no);
        //            ht_Chklist.Add("@Order_Id", Order_Id);
        //            ht_Chklist.Add("@Order_Task", Order_Task);
        //            ht_Chklist.Add("@Comments", Comments);
        //            ht_Chklist.Add("@Status", "True");
        //            ht_Chklist.Add("@User_id", user_ID);
        //            ht_Chklist.Add("@Inserted_Date", DateTime.Now);
        //            dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);

        //        }
        //        else
        //        {
        //            Hashtable ht_Chklist = new Hashtable();
        //            DataTable dt_Chklist = new DataTable();

        //            ht_Chklist.Add("@Trans", "INSERT");
        //            ht_Chklist.Add("@Checklist_Id", Checklist_Id);
        //            ht_Chklist.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
        //             ht_Chklist.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
        //            //ht_Chklist.Add("@Checked", check);
        //            ht_Chklist.Add("@Yes", chk_yes);
        //            ht_Chklist.Add("@No", chk_no);
        //            ht_Chklist.Add("@Order_Id", Order_Id);
        //            ht_Chklist.Add("@Order_Task", Order_Task);
        //            ht_Chklist.Add("@Comments", Comments);
        //            ht_Chklist.Add("@Status", "True");
        //            ht_Chklist.Add("@User_id", user_ID);
        //            ht_Chklist.Add("@Inserted_Date", DateTime.Now);
        //            dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
        //        }

        //    }
        //    if (Validate_AssessorTax_Question() != false)
        //    {
        //        tabControl1.SelectTab("tabPage3");
        //    }
        //}

        private void grd_AssessorTaxes_Chklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex != -1)
                {
                    bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].FormattedValue);
                    bool check = false;
                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column15"].ReadOnly = true;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column16"].ReadOnly = true;

                    }
                    if (chk_no != null && chk_no != false)
                    {

                        check = false;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column14"].ReadOnly = true;
                        grd_AssessorTaxes_Chklist.Rows[e.RowIndex].Cells["Column16"].ReadOnly = false;   // comments column
                    }

                }
            }
        }

        private void btn_Assesor_tex_Previous_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < grd_General_Checklist.Rows.Count; i++)
            //{
            //    bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column5"].FormattedValue);
            //    bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells["Column7"].FormattedValue);

            //    if (chk_yes != null && chk_yes != false)
            //    {
            //        chk_yes = true;
            //    }
            //    else
            //    {
            //        chk_yes = false;
            //    }
            //    if (chk_no != null && chk_no != false)
            //    {
            //        chk_no = true;
            //    }
            //    else
            //    {
            //        chk_no = false;
            //    }

            //    if (chk_yes == false && chk_no == false)
            //    {
            //        grd_General_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
            //        grd_General_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
            //    }
            //    else
            //    {
            //        grd_General_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
            //        grd_General_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;
            //    }

            //    if (grd_General_Checklist[7, i].Value == null || grd_General_Checklist[7, i].Value == "")
            //    {
            //        if (chk_no == true)
            //        {
            //            grd_General_Checklist[7, i].Style.BackColor = Color.Red;
            //        }
            //        Comments = "";

            //    }
            //    else
            //    {
            //        grd_General_Checklist[7, i].Style.BackColor = Color.White;
            //        Comments = grd_General_Checklist.Rows[i].Cells[7].Value.ToString();
            //    }


            //}



            tabControl1.SelectTab("tabPage1");

        }

        
        public void Grid_Bind_All_Deed()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = new System.Data.DataTable();
            ht.Add("@Trans", "GET_ALL_DETAILS");
            ht.Add("@Ref_Checklist_Master_Type_Id", 3);
            ht.Add("@Order_Task", Order_Task);
            ht.Add("@OrderType_ABS_Id", OrderType_ABS_Id);
            dt = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht);
            if (dt.Rows.Count > 0)
            {
                grd_Deed_Checklist.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    grd_Deed_Checklist.Rows.Add();
                    grd_Deed_Checklist.Rows[i].Cells[0].Value = i + 1;
                    grd_Deed_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                    grd_Deed_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                    grd_Deed_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                    grd_Deed_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Deed_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Deed_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                grd_Deed_Checklist.Rows.Clear();
            }
        }

        private bool Validate_Deed_Question_New()
        {
            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_Deed_Checklist.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells["Column22"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells["Column23"].FormattedValue);



                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_Deed_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_Deed_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_Deed_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_Deed_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_Deed_Checklist[7, i].Value == null || grd_Deed_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_Deed_Checklist[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";


                }
                else
                {
                    grd_Deed_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_Deed_Checklist.Rows[i].Cells[7].Value.ToString();
                }

                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_Deed_Checklist[7, i].Style.BackColor = Color.Red;
                    break;
                }

                else
                {

                    Error_Count = 0;
                }


            }

            if (grd_Deed_Checklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_Deed_Checklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 3;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }


        }

        private void btn_Deed_Next_Click(object sender, EventArgs e)
        {
            //Save_DeedList();                            07-07-2017
            //tabControl1.SelectTab("tabPage4");         07-07-2017
            if (Validate_Deed_Question_New() != false)
            {
                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage4");
            }
            //Save_Deed_List();                           //  10-07-2017
        }

        private void grd_Deed_Checklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column22"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column23"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Deed_Checklist[6, e.RowIndex].Value = false;
                        grd_Deed_Checklist[5, e.RowIndex].Value = true;

                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Deed_Checklist[5, e.RowIndex].Value = true;
                        grd_Deed_Checklist[6, e.RowIndex].Value = false;
                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        //MessageBox.Show("Enter Comments");
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Deed_Checklist[7, e.RowIndex].Value = "";
                    }


                }
                if (e.ColumnIndex == 6)
                {
                    bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column23"].FormattedValue);
                    if (chk_no != false)
                    {
                        grd_Deed_Checklist[6, e.RowIndex].Value = true;
                        grd_Deed_Checklist[5, e.RowIndex].Value = false;
                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_Deed_Checklist[6, e.RowIndex].Value = false;
                            grd_Deed_Checklist[5, e.RowIndex].Value = false;
                            grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_Deed_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_Deed_Checklist[7, e.RowIndex].Value = "";
                        }
                    }
                    else if (chk_no != true)
                    {
                        grd_Deed_Checklist[5, e.RowIndex].Value = false;
                        grd_Deed_Checklist[6, e.RowIndex].Value = true;
                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Deed_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = false;
                    }
                }

                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column22"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column23"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = true;
                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                    }
                    else if (chk_yes != true)
                    {

                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Deed_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                    }

                    if (chk_yes == false && chk_no == false)
                    {
                        grd_Deed_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_Deed_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_Deed_Checklist[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_Deed_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_Deed_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }

                }
            }

        }

        private async void btn_Deed_View_Detail_Click(object sender, EventArgs e)
        {
            DataTable dt_Deed_List = new DataTable();
            try
            {
                //ht_Deed_List.Add("@Trans", "ALL_DEED");
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                Dictionary<string, object> dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "GET_ALL_VIEW" },
                    { "@Ref_Checklist_Master_Type_Id", 3}

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_Deed_List = JsonConvert.DeserializeObject<DataTable>(result);
                            //dt_Deed_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Deed_List);
                            if (dt_Deed_List.Rows.Count > 0)
                            {
                                grd_Deed_Checklist.Rows.Clear();
                                for (int i = 0; i < dt_Deed_List.Rows.Count; i++)
                                {
                                    grd_Deed_Checklist.Rows.Add();
                                    grd_Deed_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_Deed_Checklist.Rows[i].Cells[1].Value = dt_Deed_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_Deed_Checklist.Rows[i].Cells[2].Value = dt_Deed_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Deed_Checklist.Rows[i].Cells[3].Value = dt_Deed_List.Rows[i]["Question"].ToString();
                                    grd_Deed_Checklist.Rows[i].Cells[4].Value = dt_Deed_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_Deed_Checklist.Rows[i].Cells[5].Value = dt_Deed_List.Rows[i]["Yes"].ToString();
                                    grd_Deed_Checklist.Rows[i].Cells[6].Value = dt_Deed_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_Deed_Checklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_Deed_Checklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_Deed_Checklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_Deed_Checklist[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_Deed_Checklist[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_Deed_Checklist[6, i].Value = null;
                                    }
                                    grd_Deed_Checklist.Rows[i].Cells[7].Value = dt_Deed_List.Rows[i]["Comments"].ToString();

                                    grd_Deed_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Deed_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Deed_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }

                            }
                            else
                            {
                                grd_Deed_Checklist.Rows.Clear();
                                //Grid_Bind_Deed_CheckList();
                                Grid_Bind_All_Deed();
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

        private async void btn_Deed_Save_Click(object sender, EventArgs e)
        {
            int inertval = 0;
            try
            {

                for (int i = 0; i < grd_Deed_Checklist.Rows.Count; i++)
                {

                    if (grd_Deed_Checklist.Rows[i].Cells[7].Value != "" && grd_Deed_Checklist.Rows[i].Cells[7].Value != null)
                    {
                        Comments = grd_Deed_Checklist.Rows[i].Cells[7].Value.ToString();
                    }
                    bool check = false;
                    bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells["Column22"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells["Column23"].FormattedValue);

                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_Deed_Checklist.Rows[i].Cells["Column23"].ReadOnly = true;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        check = false;
                    }
                    Ref_Checklist_Master_Type_Id = int.Parse(grd_Deed_Checklist.Rows[i].Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(grd_Deed_Checklist.Rows[i].Cells[4].Value.ToString());
                    Question = grd_Deed_Checklist.Rows[i].Cells[3].Value.ToString();

                    if (Check_List_Tran_ID == 0)
                    {

                        DataTable dt_Chklist = new DataTable();
                        Dictionary<string, object> dictionary = new Dictionary<string, object>()
                        {

                            { "@Trans", "INSERT" },
                            { "@Checklist_Id", Checklist_Id},
                            { "@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id},
                            { "@Order_Type_Abs_Id", OrderType_ABS_Id},
                            { "@Checked", check},
                            { "@Order_Id", Order_Id},
                            { "@Order_Task", Order_Task },
                            { "@Comments", Comments },
                            { "@Status", "True"},
                            { "@User_id", user_ID},
                            { "@Inserted_Date", DateTime.Now}
                       };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertTabsClick", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                    // dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
                                    inertval = 1;
                                }

                            }
                        }
                    }
                }
                if (inertval == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Deed CheckList Added Successfully");
                    //Grid_Bind_Deed_CheckList();
                    Grid_Bind_All_Deed();
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


        private void grd_Deed_Checklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex != -1)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column22"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column23"].FormattedValue);
            //        bool check = false;
            //        if (chk_yes != null && chk_yes != false)
            //        {
            //            check = true;
            //            grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column23"].ReadOnly = true;
            //            grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column24"].ReadOnly = true;

            //        }
            //        if (chk_no != null && chk_no != false)
            //        {

            //            check = false;
            //            grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column22"].ReadOnly = true;
            //            grd_Deed_Checklist.Rows[e.RowIndex].Cells["Column24"].ReadOnly = false;   // comments column
            //        }

            //    }
            //}
        }



        //Mortgage

        public void Grid_Bind_All_Mortgage()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = new System.Data.DataTable();
            ht.Add("@Trans", "GET_ALL_DETAILS");
            ht.Add("@Ref_Checklist_Master_Type_Id", 4);
            ht.Add("@Order_Task", Order_Task);
            ht.Add("@OrderType_ABS_Id", OrderType_ABS_Id);

            dt = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht);
            if (dt.Rows.Count > 0)
            {
                grd_Mortgage_Checklist.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    grd_Mortgage_Checklist.Rows.Add();
                    grd_Mortgage_Checklist.Rows[i].Cells[0].Value = i + 1;
                    grd_Mortgage_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                    grd_Mortgage_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                    grd_Mortgage_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                    grd_Mortgage_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Mortgage_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Mortgage_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                grd_Mortgage_Checklist.Rows.Clear();
            }
        }

        private bool Validate_Mortgage_Question_New()

        {
            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_Mortgage_Checklist.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column30"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column31"].FormattedValue);



                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_Mortgage_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_Mortgage_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_Mortgage_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_Mortgage_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_Mortgage_Checklist[7, i].Value == null || grd_Mortgage_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_Mortgage_Checklist[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";

                }
                else
                {
                    grd_Mortgage_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_Mortgage_Checklist.Rows[i].Cells[7].Value.ToString();
                }

                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_Mortgage_Checklist[7, i].Style.BackColor = Color.Red;
                    break;
                }

                else
                {

                    Error_Count = 0;
                }


            }

            if (grd_Mortgage_Checklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_Mortgage_Checklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 4;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }


        }
        private void Save_Mortgage_List()
        {
            for (int i = 0; i < grd_Mortgage_Checklist.Rows.Count; i++)
            {
                grd_Mortgage_Checklist.Rows[i].DefaultCellStyle.BackColor = SystemColors.Control;
                //if (grd_Mortgage_Checklist.Rows[i].Cells[7].Value != "" && grd_Mortgage_Checklist.Rows[i].Cells[7].Value != null)
                //{
                //    Comments = grd_Mortgage_Checklist.Rows[i].Cells[7].Value.ToString();
                //}
                bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column30"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column31"].FormattedValue);

                if (chk_yes != null && chk_yes != false)
                {
                    chk_yes = true;
                }
                else
                {
                    chk_yes = false;
                }
                if (chk_no != null && chk_no != false)
                {
                    chk_no = true;
                }
                else
                {
                    chk_no = false;
                }



                if (grd_Mortgage_Checklist[7, i].Value == null || grd_Mortgage_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_Mortgage_Checklist[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";

                }
                else
                {
                    grd_Mortgage_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_Mortgage_Checklist.Rows[i].Cells[7].Value.ToString();
                }


                if (grd_Mortgage_Checklist.Rows[i].Cells[5].Style.BackColor != Color.Red && grd_Mortgage_Checklist.Rows[i].Cells[6].Style.BackColor != Color.Red)
                {
                    if (grd_Mortgage_Checklist[7, i].Style.BackColor != Color.Red)
                    {
                        Ref_Checklist_Master_Type_Id = int.Parse(grd_Mortgage_Checklist.Rows[i].Cells[2].Value.ToString());
                        Checklist_Id = int.Parse(grd_Mortgage_Checklist.Rows[i].Cells[4].Value.ToString());
                        Question = grd_Mortgage_Checklist.Rows[i].Cells[3].Value.ToString();

                        Hashtable htcheck = new Hashtable();
                        DataTable dtcheck = new DataTable();
                        htcheck.Add("@Trans", "CHECK");
                        htcheck.Add("@Checklist_Id", Checklist_Id);
                        htcheck.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                        htcheck.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                        htcheck.Add("@User_id", user_ID);
                        htcheck.Add("@Order_Id", Order_Id);
                        htcheck.Add("@Order_Task", Order_Task);
                        htcheck.Add("@Work_Type", Work_Type_Id);
                        dtcheck = dataaccess.ExecuteSP("Sp_Checklist_Detail", htcheck);

                        if (dtcheck.Rows.Count > 0)
                        {

                            Check_List_Tran_ID = int.Parse(dtcheck.Rows[0]["Check_List_Tran_ID"].ToString());
                            Hashtable ht_Chklist = new Hashtable();
                            DataTable dt_Chklist = new DataTable();

                            ht_Chklist.Add("@Trans", "UPDATE");
                            ht_Chklist.Add("@Check_List_Tran_ID", Check_List_Tran_ID);
                            ht_Chklist.Add("@Checklist_Id", Checklist_Id);
                            ht_Chklist.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                            ht_Chklist.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                            ht_Chklist.Add("@Yes", chk_yes);
                            ht_Chklist.Add("@No", chk_no);
                            ht_Chklist.Add("@Order_Id", Order_Id);
                            ht_Chklist.Add("@Order_Task", Order_Task);
                            ht_Chklist.Add("@Comments", Comments);
                            ht_Chklist.Add("@Status", "True");
                            ht_Chklist.Add("@User_id", user_ID);
                            ht_Chklist.Add("@Modified_By", user_ID);
                            ht_Chklist.Add("@Modified_Date", DateTime.Now);
                            ht_Chklist.Add("@Work_Type", Work_Type_Id);
                            dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);

                        }
                        else if (dtcheck.Rows.Count == 0)
                        {

                            Hashtable ht_Chklist = new Hashtable();
                            DataTable dt_Chklist = new DataTable();

                            ht_Chklist.Add("@Trans", "INSERT");
                            ht_Chklist.Add("@Checklist_Id", Checklist_Id);
                            ht_Chklist.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id);
                            ht_Chklist.Add("@Order_Type_Abs_Id", OrderType_ABS_Id);
                            ht_Chklist.Add("@Yes", chk_yes);
                            ht_Chklist.Add("@No", chk_no);
                            ht_Chklist.Add("@Order_Id", Order_Id);
                            ht_Chklist.Add("@Order_Task", Order_Task);
                            ht_Chklist.Add("@Comments", Comments);
                            ht_Chklist.Add("@Status", "True");
                            ht_Chklist.Add("@User_id", user_ID);
                            ht_Chklist.Add("@Inserted_Date", DateTime.Now);
                            ht_Chklist.Add("@Work_Type", Work_Type_Id);

                            object dtcount = dataaccess.ExecuteSPForScalar("Sp_Checklist_Detail", ht_Chklist);

                            int checklistId = int.Parse(dtcount.ToString());
                        }
                    }
                }

            }



        }

        private void btn_Mortgage_Next_Click(object sender, EventArgs e)
        {
            //MortgageList();                              //07-07-2017
            //tabControl1.SelectTab("tabPage5");          //07-07-2017

            if (Validate_Mortgage_Question_New() != false)
            {
                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage5");


            }
            // Save_Mortgage_List();                      //07-07-2017
        }

        private void grd_Mortgage_Checklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column30"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column31"].FormattedValue);

                    if (chk_yes != false)
                    {
                        grd_Mortgage_Checklist[6, e.RowIndex].Value = false;
                        grd_Mortgage_Checklist[5, e.RowIndex].Value = true;

                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Mortgage_Checklist[5, e.RowIndex].Value = true;
                        grd_Mortgage_Checklist[6, e.RowIndex].Value = false;
                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Mortgage_Checklist[7, e.RowIndex].Value = "";
                    }
                }
                if (e.ColumnIndex == 6)
                {
                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column31"].FormattedValue);
                    if (chk_no != false)
                    {
                        grd_Mortgage_Checklist[6, e.RowIndex].Value = true;
                        grd_Mortgage_Checklist[5, e.RowIndex].Value = false;
                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_Mortgage_Checklist[6, e.RowIndex].Value = false;
                            grd_Mortgage_Checklist[5, e.RowIndex].Value = false;
                            grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_Mortgage_Checklist[7, e.RowIndex].Value = "";
                        }
                    }
                    else if (chk_no != true)
                    {
                        grd_Mortgage_Checklist[5, e.RowIndex].Value = false;
                        grd_Mortgage_Checklist[6, e.RowIndex].Value = true;
                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Mortgage_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Mortgage_Checklist[7, e.RowIndex].Value = "";
                    }
                }

                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column30"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column31"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = true;
                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                    }
                    else if (chk_yes != true)
                    {
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Mortgage_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                    }

                    if (chk_yes == false && chk_no == false)
                    {
                        grd_Mortgage_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_Mortgage_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_Mortgage_Checklist[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_Mortgage_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_Mortgage_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }

                }
            }

        }

        private async void btn_Mortgage_View_detail_Click(object sender, EventArgs e)
        {
            //Hashtable ht_Mortgage_List = new Hashtable();
            DataTable dt_Mortgage_List = new DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                //ht_Mortgage_List.Add("@Trans", "ALL_MORTGAGE");
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "GET_ALL_VIEW" },
                    { "@Ref_Checklist_Master_Type_Id", 4 }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_Mortgage_List = JsonConvert.DeserializeObject<DataTable>(result);
                            // dt_Mortgage_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Mortgage_List);
                            if (dt_Mortgage_List.Rows.Count > 0)
                            {
                                grd_Mortgage_Checklist.Rows.Clear();
                                for (int i = 0; i < dt_Mortgage_List.Rows.Count; i++)
                                {
                                    grd_Mortgage_Checklist.Rows.Add();
                                    grd_Mortgage_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_Mortgage_Checklist.Rows[i].Cells[1].Value = dt_Mortgage_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_Mortgage_Checklist.Rows[i].Cells[2].Value = dt_Mortgage_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Mortgage_Checklist.Rows[i].Cells[3].Value = dt_Mortgage_List.Rows[i]["Question"].ToString();
                                    grd_Mortgage_Checklist.Rows[i].Cells[4].Value = dt_Mortgage_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_Mortgage_Checklist.Rows[i].Cells[5].Value = dt_Mortgage_List.Rows[i]["Yes"].ToString();
                                    grd_Mortgage_Checklist.Rows[i].Cells[6].Value = dt_Mortgage_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_Mortgage_Checklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_Mortgage_Checklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_Mortgage_Checklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_Mortgage_Checklist[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_Mortgage_Checklist[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_Mortgage_Checklist[6, i].Value = null;
                                    }
                                    grd_Mortgage_Checklist.Rows[i].Cells[7].Value = dt_Mortgage_List.Rows[i]["Comments"].ToString();

                                    grd_Mortgage_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Mortgage_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Mortgage_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }
                            else
                            {
                                grd_Mortgage_Checklist.Rows.Clear();
                                //Grid_Bind_Mortgage_CheckList();
                                Grid_Bind_All_Mortgage();
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

        private async void btn_Mortgage_Save_Click(object sender, EventArgs e)
        {
            try
            {
                int inertval = 0;
                for (int i = 0; i < grd_Mortgage_Checklist.Rows.Count; i++)
                {

                    if (grd_Mortgage_Checklist.Rows[i].Cells[7].Value != "" && grd_Mortgage_Checklist.Rows[i].Cells[7].Value != null)
                    {
                        Comments = grd_Mortgage_Checklist.Rows[i].Cells[7].Value.ToString();
                    }
                    bool check = false;
                    bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column30"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells["Column31"].FormattedValue);

                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_Mortgage_Checklist.Rows[i].Cells["Column31"].ReadOnly = true;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        check = false;
                    }
                    Ref_Checklist_Master_Type_Id = int.Parse(grd_Mortgage_Checklist.Rows[i].Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(grd_Mortgage_Checklist.Rows[i].Cells[4].Value.ToString());
                    Question = grd_Mortgage_Checklist.Rows[i].Cells[3].Value.ToString();

                    if (Check_List_Tran_ID == 0)
                    {
                        //Hashtable ht_Chklist = new Hashtable();
                        DataTable dt_Chklist = new DataTable();
                        Dictionary<string, object> dictionary = new Dictionary<string, object>()
                        {

                             { "@Trans", "INSERT" },
                             { "@Checklist_Id", Checklist_Id },
                             { "@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id },
                             { "@Order_Type_Abs_Id", OrderType_ABS_Id },
                             { "@Checked", check },
                             { "@Order_Id", Order_Id },
                             { "@Order_Task", Order_Task },
                              { "@Comments", Comments },
                              { "@Status", "True" },
                              { "@User_id", user_ID },
                              { "@Inserted_Date", DateTime.Now },
                      };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertTabsClick", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    //dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
                                    inertval = 1;
                                }

                            }
                        }
                    }
                }
                if (inertval == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Mortgage CheckList Added Successfully");
                    //Grid_Bind_Mortgage_CheckList();
                    Grid_Bind_All_Mortgage();
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

        

        private void grd_Mortgage_Checklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex != -1)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column30"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column31"].FormattedValue);

            //        if (chk_yes != false)
            //        {

            //            grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column31"].ReadOnly = true;
            //            grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column32"].ReadOnly = true;

            //        }
            //        if (chk_no != false)
            //        {


            //            grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column30"].ReadOnly = true;
            //            grd_Mortgage_Checklist.Rows[e.RowIndex].Cells["Column32"].ReadOnly = false;   // comments column
            //        }

            //    }
            //}
        }

        //Judgment/Liens

        public void Grid_Bind_All_JudgmLien()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = new System.Data.DataTable();
            ht.Add("@Trans", "GET_ALL_DETAILS");
            ht.Add("@Ref_Checklist_Master_Type_Id", 5);
            ht.Add("@Order_Task", Order_Task);
            ht.Add("@OrderType_ABS_Id", OrderType_ABS_Id);
            dt = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht);
            if (dt.Rows.Count > 0)
            {
                grd_Judgment_Liens_Checklist.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    grd_Judgment_Liens_Checklist.Rows.Add();
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[0].Value = i + 1;
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                    grd_Judgment_Liens_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                grd_Judgment_Liens_Checklist.Rows.Clear();
            }
        }

        private void btn_Judgment_Liens_Next_Click(object sender, EventArgs e)
        {
            //JudgmentLiensList();                            // 07-07-2017
            //tabControl1.SelectTab("tabPage6");             // 07-07-2017
            if (Validate_Judgment_Liens_Question_New() != false)
            {
                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage6");

            }
            // Save_Judgment_Liens_List();                         // 10-07-2017
        }

        private bool Validate_Judgment_Liens_Question()
        {
            Hashtable htgetmax_num = new Hashtable();
            DataTable dtgetmax_num = new DataTable();

            htgetmax_num.Add("@Trans", "CHECK_COUNT");
            htgetmax_num.Add("@Order_Id", Order_Id);
            htgetmax_num.Add("@Order_Task", Order_Task);
            htgetmax_num.Add("@Ref_Checklist_Master_Type_Id", 5);
            htgetmax_num.Add("@Work_Type", Work_Type_Id);
            htgetmax_num.Add("@User_id", user_ID);
            dtgetmax_num = dataaccess.ExecuteSP("Sp_Checklist_Detail", htgetmax_num);

            if (dtgetmax_num.Rows.Count > 0)
            {
                Entered_Count = int.Parse(dtgetmax_num.Rows[0]["count"].ToString());
            }
            else
            {
                Entered_Count = 0;
            }
            Question_Count = int.Parse(grd_Judgment_Liens_Checklist.Rows.Count.ToString());
            if (Entered_Count == Question_Count && Error_Count == 0)
            {
                return true;
            }

            else
            {
                Error_Count = 0;
                Defined_Tab_Index = 5;


                if (Defined_Tab_Index != 0)
                {

                }
                else
                {
                    MessageBox.Show("Need to Enter All the Fields");
                }

                return false;
            }
        }

        private bool Validate_Judgment_Liens_Question_New()
        {

            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_Judgment_Liens_Checklist.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells["Column38"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells["Column39"].FormattedValue);


                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_Judgment_Liens_Checklist[7, i].Value == null || grd_Judgment_Liens_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_Judgment_Liens_Checklist[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";

                }
                else
                {
                    grd_Judgment_Liens_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_Judgment_Liens_Checklist.Rows[i].Cells[7].Value.ToString();
                }

                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_Judgment_Liens_Checklist[7, i].Style.BackColor = Color.Red;
                    break;

                }
                else
                {

                    Error_Count = 0;
                }

            }

            if (grd_Judgment_Liens_Checklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_Judgment_Liens_Checklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 5;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }

        }

         private void grd_Judgment_Liens_Checklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column38"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column39"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = false;
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = true;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = true;
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = false;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Value = "";
                    }
                }
                if (e.ColumnIndex == 6)
                {

                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column39"].FormattedValue);

                    if (chk_no != false)
                    {
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = true;
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = false;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = false;
                            grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = false;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].Value = "";
                        }

                    }
                    else if (chk_no != true)
                    {
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = false;
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = true;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Judgment_Liens_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Value = "";
                    }

                }

                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column38"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column39"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

                    }
                    else if (chk_yes != true)
                    {

                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = Color.White;

                    }

                    if (chk_yes == false && chk_no == false)
                    {

                        grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }

                }
            }

        }

        private async void btn_JudgmentLiens_View_Detail_Click(object sender, EventArgs e)
        {
            //Hashtable ht_JudgLiens_List = new Hashtable();
            DataTable dt_JudgLiens_List = new DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
                {
                    //  ht_JudgLiens_List.Add("@Trans", "ALL_JUDGMENT_LIENS");
                    { "@Trans", "GET_ALL_VIEW"},
                    { "@Ref_Checklist_Master_Type_Id", 5 }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_JudgLiens_List = JsonConvert.DeserializeObject<DataTable>(result);
                            //dt_JudgLiens_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_JudgLiens_List);
                            if (dt_JudgLiens_List.Rows.Count > 0)
                            {
                                grd_Judgment_Liens_Checklist.Rows.Clear();
                                for (int i = 0; i < dt_JudgLiens_List.Rows.Count; i++)
                                {
                                    grd_Judgment_Liens_Checklist.Rows.Add();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[1].Value = dt_JudgLiens_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[2].Value = dt_JudgLiens_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[3].Value = dt_JudgLiens_List.Rows[i]["Question"].ToString();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[4].Value = dt_JudgLiens_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Value = dt_JudgLiens_List.Rows[i]["Yes"].ToString();
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Value = dt_JudgLiens_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_Judgment_Liens_Checklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_Judgment_Liens_Checklist[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_Judgment_Liens_Checklist[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_Judgment_Liens_Checklist[6, i].Value = null;
                                    }
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[7].Value = dt_JudgLiens_List.Rows[i]["Comments"].ToString();

                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Judgment_Liens_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }

                            else
                            {
                                grd_Judgment_Liens_Checklist.Rows.Clear();
                                //Grid_Bind_JudgmentLiens_CheckList();
                                Grid_Bind_All_JudgmLien();
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

        private async void btn_Judgment_Liens_Save_Click(object sender, EventArgs e)
        {
            int inertval = 0;
            try
            {
                for (int i = 0; i < grd_Judgment_Liens_Checklist.Rows.Count; i++)
                {

                    if (grd_Judgment_Liens_Checklist.Rows[i].Cells[7].Value != "" && grd_Judgment_Liens_Checklist.Rows[i].Cells[7].Value != null)
                    {
                        Comments = grd_Judgment_Liens_Checklist.Rows[i].Cells[7].Value.ToString();
                    }
                    bool check = false;
                    bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells["Column38"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells["Column39"].FormattedValue);
                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_Judgment_Liens_Checklist.Rows[i].Cells["Column39"].ReadOnly = true;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        check = false;
                    }
                    Ref_Checklist_Master_Type_Id = int.Parse(grd_Judgment_Liens_Checklist.Rows[i].Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(grd_Judgment_Liens_Checklist.Rows[i].Cells[4].Value.ToString());
                    Question = grd_Judgment_Liens_Checklist.Rows[i].Cells[3].Value.ToString();

                    if (Check_List_Tran_ID == 0)
                    {
                        //Hashtable ht_Chklist = new Hashtable();
                        DataTable dt_Chklist = new DataTable();
                        IDictionary<string, object> dictionary = new Dictionary<string, object>()
                    {
                            { "@Trans", "INSERT" },
                            {"@Checklist_Id", Checklist_Id },
                            {"@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id },
                            { "@Order_Type_Abs_Id", OrderType_ABS_Id },
                            { "@Checked", check },
                            { "@Order_Id", Order_Id },
                            { "@Order_Task", Order_Task },
                            { "@Comments", Comments },
                            { "@Status", "True" },
                            { "@User_id", user_ID },
                            { "@Inserted_Date", DateTime.Now }
                    };
                        //dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/JSon");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertTabsClick", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    inertval = 1;
                                }

                            }
                        }
                    }

                }
                if (inertval == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Judgemnet/Liens CheckList Added Successfully");
                    //Grid_Bind_JudgmentLiens_CheckList();
                    Grid_Bind_All_JudgmLien();
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

      private void grd_Judgment_Liens_Checklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex != -1)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column38"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column39"].FormattedValue);
            //        bool check = false;
            //        if (chk_yes != null && chk_yes != false)
            //        {
            //            check = true;
            //            grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column39"].ReadOnly = true;
            //            grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column40"].ReadOnly = true;

            //        }
            //        if (chk_no != null && chk_no != false)
            //        {

            //            check = false;
            //            grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column38"].ReadOnly = true;
            //            grd_Judgment_Liens_Checklist.Rows[e.RowIndex].Cells["Column40"].ReadOnly = false;   // comments column
            //        }

            //    }
            //}
        }


        //others
        public void Grid_Bind_All_Others()
        {
            Hashtable ht = new Hashtable();
            DataTable dt = new System.Data.DataTable();
            ht.Add("@Trans", "GET_ALL_DETAILS");
            ht.Add("@Ref_Checklist_Master_Type_Id", 6);
            ht.Add("@Order_Task", Order_Task);
            ht.Add("@OrderType_ABS_Id", OrderType_ABS_Id);
            dt = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht);
            if (dt.Rows.Count > 0)
            {
                grd_Others_Checklist.Rows.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    grd_Others_Checklist.Rows.Add();
                    grd_Others_Checklist.Rows[i].Cells[0].Value = i + 1;
                    grd_Others_Checklist.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                    grd_Others_Checklist.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                    grd_Others_Checklist.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                    grd_Others_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Others_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grd_Others_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            else
            {
                grd_Others_Checklist.Rows.Clear();
            }
        }

        private bool Validate_Others_Question_New()
        {

            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_Others_Checklist.Rows.Count; i++)
            {


                bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells["Column46"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells["Column47"].FormattedValue);



                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_Others_Checklist.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_Others_Checklist.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_Others_Checklist.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_Others_Checklist.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_Others_Checklist[7, i].Value == null || grd_Others_Checklist[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        //Error_Count = 1;
                        grd_Others_Checklist[7, i].Style.BackColor = Color.Red;

                    }
                    Comments = "";

                }
                else
                {
                    grd_Others_Checklist[7, i].Style.BackColor = Color.White;
                    Comments = grd_Others_Checklist.Rows[i].Cells[7].Value.ToString();
                }

                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_Others_Checklist[7, i].Style.BackColor = Color.Red;
                    break;

                }
                else
                {

                    Error_Count = 0;
                }



            }

            if (grd_Others_Checklist.Rows.Count <= 0)
            {
                return true;
            }

            if (grd_Others_Checklist.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                Defined_Tab_Index = 6;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }

        }
        

        private void btn_Others_Finish_Click(object sender, EventArgs e)
        {
            //Others_List();                //7-07-2017
            if (Validate_Others_Question_New() != false)
            {
                Error_Tab_Count = 0;
                tabControl1.SelectTab("tabPage7");

            }
            // Save_Others_List();                             //10-07-2017
        }


        private void grd_Others_Checklist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column46"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column47"].FormattedValue);

                    if (chk_yes != false)
                    {
                        grd_Others_Checklist[6, e.RowIndex].Value = false;
                        grd_Others_Checklist[5, e.RowIndex].Value = true;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Others_Checklist[5, e.RowIndex].Value = true;
                        grd_Others_Checklist[6, e.RowIndex].Value = false;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Others_Checklist[7, e.RowIndex].Value = "";
                    }
                }
                if (e.ColumnIndex == 6)
                {
                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column47"].FormattedValue);

                    if (chk_no != false)
                    {
                        grd_Others_Checklist[6, e.RowIndex].Value = true;
                        grd_Others_Checklist[5, e.RowIndex].Value = false;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_Others_Checklist[6, e.RowIndex].Value = false;
                            grd_Others_Checklist[5, e.RowIndex].Value = false;
                            grd_Others_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_Others_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_Others_Checklist[7, e.RowIndex].Value = "";
                        }

                    }
                    else if (chk_no != true)
                    {
                        grd_Others_Checklist[5, e.RowIndex].Value = false;
                        grd_Others_Checklist[6, e.RowIndex].Value = true;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Others_Checklist[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Others_Checklist[7, e.RowIndex].Value = "";
                    }
                }
                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column46"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column47"].FormattedValue);
                    string comments = grd_Others_Checklist.Rows[e.RowIndex].Cells["Column48"].ToString();

                    if (chk_yes != false)
                    {
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = true;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = false;
                        grd_Others_Checklist[7, e.RowIndex].Style.BackColor = Color.White;
                    }

                    if (chk_yes == false && chk_no == false)
                    {
                        grd_Others_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_Others_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_Others_Checklist[7, e.RowIndex].ReadOnly = true;

                    }
                    else
                    {
                        grd_Others_Checklist.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_Others_Checklist.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }
                }
            }

        }

        private async void btn_Others_View_Detail_Click(object sender, EventArgs e)
        {

            DataTable dt_Others_List = new DataTable();
            try
            {
                // ht_Others_List.Add("@Trans", "ALL_OTHERS");

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary dictionary = new Dictionary<string, object>()
            {
                {"@Trans", "GET_ALL_VIEW" },
                { "@Ref_Checklist_Master_Type_Id", 6}
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_Others_List = JsonConvert.DeserializeObject<DataTable>(result);
                            //dt_Others_List = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Others_List);
                            if (dt_Others_List.Rows.Count > 0)
                            {
                                grd_Others_Checklist.Rows.Clear();
                                for (int i = 0; i < dt_Others_List.Rows.Count; i++)
                                {
                                    grd_Others_Checklist.Rows.Add();
                                    grd_Others_Checklist.Rows[i].Cells[0].Value = i + 1;
                                    grd_Others_Checklist.Rows[i].Cells[1].Value = dt_Others_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_Others_Checklist.Rows[i].Cells[2].Value = dt_Others_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Others_Checklist.Rows[i].Cells[3].Value = dt_Others_List.Rows[i]["Question"].ToString();
                                    grd_Others_Checklist.Rows[i].Cells[4].Value = dt_Others_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_Others_Checklist.Rows[i].Cells[5].Value = dt_Others_List.Rows[i]["Yes"].ToString();
                                    grd_Others_Checklist.Rows[i].Cells[6].Value = dt_Others_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_Others_Checklist.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_Others_Checklist.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_Others_Checklist[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_Others_Checklist[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_Others_Checklist[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_Others_Checklist[6, i].Value = null;
                                    }
                                    grd_Others_Checklist.Rows[i].Cells[7].Value = dt_Others_List.Rows[i]["Comments"].ToString();

                                    grd_Others_Checklist.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Others_Checklist.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                    grd_Others_Checklist.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                }
                            }
                            else
                            {
                                grd_Others_Checklist.Rows.Clear();
                                //Grid_Bind_Others_CheckList();
                                Grid_Bind_All_Others();
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


        private async void btn_Others_Save_Click(object sender, EventArgs e)
        {
            int inertval = 0;
            try
            {
                for (int i = 0; i < grd_Others_Checklist.Rows.Count; i++)
                {

                    if (grd_Others_Checklist.Rows[i].Cells[7].Value != "" && grd_Others_Checklist.Rows[i].Cells[7].Value != null)
                    {
                        Comments = grd_Others_Checklist.Rows[i].Cells[7].Value.ToString();
                    }
                    bool check = false;
                    bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells["Column46"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells["Column47"].FormattedValue);

                    if (chk_yes != null && chk_yes != false)
                    {
                        check = true;
                        grd_Others_Checklist.Rows[i].Cells["Column47"].ReadOnly = true;
                    }
                    if (chk_no != null && chk_no != false)
                    {
                        check = false;
                    }
                    Ref_Checklist_Master_Type_Id = int.Parse(grd_Others_Checklist.Rows[i].Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(grd_Others_Checklist.Rows[i].Cells[4].Value.ToString());
                    Question = grd_Others_Checklist.Rows[i].Cells[3].Value.ToString();

                    if (Check_List_Tran_ID == 0)
                    {
                        //Hashtable ht_Chklist = new Hashtable();
                        DataTable dt_Chklist = new DataTable();
                        IDictionary<string, object> dictionary = new Dictionary<string, object>()
                    {
                            { "@Trans", "INSERT" },
                            { "@Checklist_Id", Checklist_Id },
                            { "@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id },
                            { "@Order_Type_Abs_Id", OrderType_ABS_Id },
                            { "@Checked", check },
                            { "@Order_Id", Order_Id},
                            { "@Order_Task", Order_Task},
                            { "@Comments", Comments},
                            { "@Status", "True"},
                            { "@User_id", user_ID},
                            { "@Inserted_Date", DateTime.Now}
                    };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/JSon");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertTabsClick", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    //dt_Chklist = dataaccess.ExecuteSP("Sp_Checklist_Detail", ht_Chklist);
                                    inertval = 1;

                                }

                            }
                        }
                    }
                }
                if (inertval == 1)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Others CheckList Added Successfully");
                    //Grid_Bind_Others_CheckList();
                    Grid_Bind_All_Others();
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


        private void grd_Others_Checklist_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    if (e.ColumnIndex != -1)
            //    {
            //        bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column46"].FormattedValue);
            //        bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[e.RowIndex].Cells["Column47"].FormattedValue);
            //        bool check = false;
            //        if (chk_yes != null && chk_yes != false)
            //        {
            //            check = true;
            //            grd_Others_Checklist.Rows[e.RowIndex].Cells["Column47"].ReadOnly = true;
            //            grd_Others_Checklist.Rows[e.RowIndex].Cells["Column48"].ReadOnly = true;

            //        }
            //        if (chk_no != null && chk_no != false)
            //        {

            //            check = false;
            //            grd_Others_Checklist.Rows[e.RowIndex].Cells["Column46"].ReadOnly = true;
            //            grd_Others_Checklist.Rows[e.RowIndex].Cells["Column48"].ReadOnly = false;   // comments column
            //        }

            //    }
            //}
        }



        // tab control index change event
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Validate_Tab_Selcted_Index();

            //if (Check_Count != 1)
            //{
            //    int aa = Error_Count;
            //    if (tabControl1.SelectedIndex == 1)
            //    {
            //        Save_General_List();
            //        if (Validate_Genral_Question() != false)
            //        {

            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;


            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;

            //            tabControl1.SelectTab("tabPage1");
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }
            //    }
            //    else if (tabControl1.SelectedIndex == 2)
            //    {

            //        Save_Assessor_Tax_List();
            //        if (Validate_Genral_Question() != false && Validate_AssessorTax_Question() != false)
            //        {


            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage2");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;

            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;

            //            if (Defined_Tab_Index == 1)
            //            {
            //                Save_General_List();
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            else if (Defined_Tab_Index == 2)
            //            {
            //                Save_Assessor_Tax_List();
            //                tabControl1.SelectTab("tabPage2");

            //            }

            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }
            //    }
            //    else if (tabControl1.SelectedIndex == 3 )
            //    {
            //        Save_Deed_List();
            //        if (Validate_Genral_Question() != false && Validate_AssessorTax_Question() != false && Validate_Deed_Question()!=false)
            //        {

            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage3");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;

            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;
            //            if (Defined_Tab_Index == 1)
            //            {
            //                Save_General_List();
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            else if (Defined_Tab_Index == 2)
            //            {
            //                Save_Assessor_Tax_List();
            //                tabControl1.SelectTab("tabPage2");

            //            }
            //            else if (Defined_Tab_Index == 3)
            //            {
            //                Save_Deed_List();
            //                //tabControl1.SelectTab("tabPage3");

            //            }



            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }

            //    }

            //    else if (tabControl1.SelectedIndex == 4)
            //    {
            //        Save_Mortgage_List();
            //        if (Validate_Genral_Question() != false && Validate_AssessorTax_Question() != false && Validate_Deed_Question() != false && Validate_Mortgage_Question() != false)
            //        {

            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage4");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;

            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;
            //            if (Defined_Tab_Index == 1)
            //            {
            //                Save_General_List();
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            else if (Defined_Tab_Index == 2)
            //            {
            //                Save_Assessor_Tax_List();
            //                tabControl1.SelectTab("tabPage2");

            //            }
            //            else if (Defined_Tab_Index == 3)
            //            {
            //                Save_Deed_List();
            //                tabControl1.SelectTab("tabPage3");

            //            }
            //            else if (Defined_Tab_Index == 4)
            //            {
            //                Save_Mortgage_List();
            //                tabControl1.SelectTab("tabPage4");

            //            }



            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }


            //    }

            //    else if (tabControl1.SelectedIndex == 5)
            //    {
            //        Save_Judgment_Liens_List();
            //        if (Validate_Genral_Question() != false && Validate_AssessorTax_Question() != false && Validate_Deed_Question() != false && Validate_Mortgage_Question() != false && Validate_Judgment_Liens_Question() != false)
            //        {

            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage5");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;

            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;
            //            if (Defined_Tab_Index == 1)
            //            {
            //                Save_General_List();
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            else if (Defined_Tab_Index == 2)
            //            {
            //                Save_Assessor_Tax_List();
            //                tabControl1.SelectTab("tabPage2");

            //            }
            //            else if (Defined_Tab_Index == 3)
            //            {
            //                Save_Deed_List();
            //                tabControl1.SelectTab("tabPage3");

            //            }
            //            else if (Defined_Tab_Index == 4)
            //            {
            //                Save_Mortgage_List();
            //               tabControl1.SelectTab("tabPage4");

            //            }
            //            else if (Defined_Tab_Index == 5)
            //            {
            //                Save_Judgment_Liens_List();
            //                tabControl1.SelectTab("tabPage5");

            //            }



            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }

            //    }

            //    else if (tabControl1.SelectedIndex == 6)
            //    {
            //        Save_Others_List();
            //        if (Validate_Genral_Question() != false && Validate_AssessorTax_Question() != false && Validate_Deed_Question() != false && Validate_Mortgage_Question() != false && Validate_Judgment_Liens_Question() != false && Validate_Others_Question() != false)
            //        {

            //            if (Error_Tab_Count == 1)
            //            {

            //                Check_Count = 1;
            //                tabControl1.SelectTab("tabPage6");
            //            }
            //            Error_Tab_Count = 0;
            //            Check_Count = 0;

            //            return;
            //        }
            //        else
            //        {
            //            Check_Count = 1;
            //            if (Defined_Tab_Index == 1)
            //            {
            //                Save_General_List();
            //                tabControl1.SelectTab("tabPage1");
            //            }
            //            else if (Defined_Tab_Index == 2)
            //            {
            //                Save_Assessor_Tax_List();
            //                tabControl1.SelectTab("tabPage2");

            //            }
            //            else if (Defined_Tab_Index == 3)
            //            {
            //                Save_Deed_List();
            //                tabControl1.SelectTab("tabPage3");

            //            }
            //            else if (Defined_Tab_Index == 4)
            //            {
            //                Save_Mortgage_List();
            //                tabControl1.SelectTab("tabPage4");

            //            }
            //            else if (Defined_Tab_Index == 5)
            //            {
            //                Save_Judgment_Liens_List();
            //                tabControl1.SelectTab("tabPage5");

            //            }
            //            else if (Defined_Tab_Index == 6)
            //            {
            //                Save_Others_List();
            //                tabControl1.SelectTab("tabPage6");

            //            }


            //            Error_Tab_Count = 0;
            //            Check_Count = 0;
            //            return;
            //        }

            //    }

            //}



        }

        private void Validate_Tab_Selcted_Index()
        {

            if (Check_Count != 1)
            {
                int aa = Error_Count;
                if (tabControl1.SelectedIndex == 1)
                {

                    if (Validate_Genral_Question_New() != false)
                    {

                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage1");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;


                        return;
                    }
                    else
                    {
                        Check_Count = 1;

                        tabControl1.SelectTab("tabPage1");
                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }
                }
                else if (tabControl1.SelectedIndex == 2)
                {


                    if (Validate_Genral_Question_New() != false && Validate_AssessorTax_New() != false)
                    {


                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage2");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;

                        return;
                    }
                    else
                    {
                        Check_Count = 1;

                        if (Defined_Tab_Index == 1)
                        {

                            tabControl1.SelectTab("tabPage1");
                        }
                        else if (Defined_Tab_Index == 2)
                        {

                            tabControl1.SelectTab("tabPage2");

                        }

                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }
                }
                else if (tabControl1.SelectedIndex == 3)
                {

                    if (Validate_Genral_Question_New() != false && Validate_AssessorTax_New() != false && Validate_Deed_Question_New() != false)
                    {

                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage3");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;

                        return;
                    }
                    else
                    {
                        Check_Count = 1;
                        if (Defined_Tab_Index == 1)
                        {

                            tabControl1.SelectTab("tabPage1");
                        }
                        else if (Defined_Tab_Index == 2)
                        {

                            tabControl1.SelectTab("tabPage2");

                        }
                        else if (Defined_Tab_Index == 3)
                        {

                            //tabControl1.SelectTab("tabPage3");

                        }



                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }

                }

                else if (tabControl1.SelectedIndex == 4)
                {

                    if (Validate_Genral_Question_New() != false && Validate_AssessorTax_New() != false && Validate_Deed_Question_New() != false && Validate_Mortgage_Question_New() != false)
                    {

                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage4");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;

                        return;
                    }
                    else
                    {
                        Check_Count = 1;
                        if (Defined_Tab_Index == 1)
                        {

                            tabControl1.SelectTab("tabPage1");
                        }
                        else if (Defined_Tab_Index == 2)
                        {

                            tabControl1.SelectTab("tabPage2");

                        }
                        else if (Defined_Tab_Index == 3)
                        {

                            tabControl1.SelectTab("tabPage3");

                        }
                        else if (Defined_Tab_Index == 4)
                        {

                            tabControl1.SelectTab("tabPage4");

                        }



                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }


                }

                else if (tabControl1.SelectedIndex == 5)
                {

                    if (Validate_Genral_Question_New() != false && Validate_AssessorTax_New() != false && Validate_Deed_Question_New() != false && Validate_Mortgage_Question_New() != false && Validate_Judgment_Liens_Question_New() != false)
                    {

                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage5");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;

                        return;
                    }
                    else
                    {
                        Check_Count = 1;
                        if (Defined_Tab_Index == 1)
                        {

                            tabControl1.SelectTab("tabPage1");
                        }
                        else if (Defined_Tab_Index == 2)
                        {

                            tabControl1.SelectTab("tabPage2");

                        }
                        else if (Defined_Tab_Index == 3)
                        {

                            tabControl1.SelectTab("tabPage3");

                        }
                        else if (Defined_Tab_Index == 4)
                        {

                            tabControl1.SelectTab("tabPage4");

                        }
                        else if (Defined_Tab_Index == 5)
                        {

                            tabControl1.SelectTab("tabPage5");

                        }



                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }

                }

                else if (tabControl1.SelectedIndex == 6)
                {

                    if (Validate_Genral_Question_New() != false && Validate_AssessorTax_New() != false && Validate_Deed_Question_New() != false && Validate_Mortgage_Question_New() != false && Validate_Judgment_Liens_Question_New() != false && Validate_Others_Question_New() != false)
                    {

                        if (Error_Tab_Count == 1)
                        {

                            Check_Count = 1;
                            tabControl1.SelectTab("tabPage6");
                        }
                        Error_Tab_Count = 0;
                        Check_Count = 0;

                        return;
                    }
                    else
                    {
                        Check_Count = 1;
                        if (Defined_Tab_Index == 1)
                        {

                            tabControl1.SelectTab("tabPage1");
                        }
                        else if (Defined_Tab_Index == 2)
                        {

                            tabControl1.SelectTab("tabPage2");

                        }
                        else if (Defined_Tab_Index == 3)
                        {

                            tabControl1.SelectTab("tabPage3");

                        }
                        else if (Defined_Tab_Index == 4)
                        {

                            tabControl1.SelectTab("tabPage4");

                        }
                        else if (Defined_Tab_Index == 5)
                        {

                            tabControl1.SelectTab("tabPage5");

                        }
                        else if (Defined_Tab_Index == 6)
                        {

                            tabControl1.SelectTab("tabPage6");

                        }


                        Error_Tab_Count = 0;
                        Check_Count = 0;
                        return;
                    }

                }

            }
        }

        private void btn_Refresh_All_Click(object sender, EventArgs e)
        {
            //Grid_Bind_All_General();
            //Grid_Bind_Assessor_Taxes_CheckList();
            //Grid_Bind_Deed_CheckList();
            //Grid_Bind_Mortgage_CheckList();
            //Grid_Bind_JudgmentLiens_CheckList();
            //Grid_Bind_Others_CheckList();


            SplashScreenManager.ShowForm(this, typeof(Ordermanagement_01.Masters.WaitForm1), true, true, false);
            try
            {
                Grid_Bind_All_General();
                Grid_Bind_All_AssessorTax();
                Grid_Bind_All_Deed();
                Grid_Bind_All_Mortgage();
                Grid_Bind_All_Others();
                Grid_Bind_All_JudgmLien();
            }
            catch (Exception ex)
            {

                //Close Wait Form
                SplashScreenManager.CloseForm(false);

                MessageBox.Show("Error Occured Please Check With Administrator");
            }
            finally
            {
                //Close Wait Form
                SplashScreenManager.CloseForm(false);
            }


        }

        private void btn_Deed_Previous_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage2");
            //btn_Assessor_Liens_View_Click(sender, e);

        }

        private void btn_Mortgage_Previous_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage3");
        }

        private void btn_JudgLiens_Previous_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage4");
        }

        private void btn_Others_Previous_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage5");
        }

        private void grd_General_Checklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {

                for (int i = 0; i < grd_General_Checklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_General_Checklist.Rows[i].Cells[6].FormattedValue);



                    if (chk_yes == true)
                    {
                        grd_General_Checklist[7, i].ReadOnly = true;
                        grd_General_Checklist[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }

        private void grd_AssessorTaxes_Chklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_AssessorTaxes_Chklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_AssessorTaxes_Chklist.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_AssessorTaxes_Chklist[7, i].ReadOnly = true;
                        grd_AssessorTaxes_Chklist[7, i].Style.BackColor = SystemColors.Control;
                    }
                }
                return;
            }
        }

        private void grd_Deed_Checklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_Deed_Checklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Deed_Checklist.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_Deed_Checklist[7, i].ReadOnly = true;
                        grd_Deed_Checklist[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }

        private void grd_Mortgage_Checklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_Mortgage_Checklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Mortgage_Checklist.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_Mortgage_Checklist[7, i].ReadOnly = true;
                        grd_Mortgage_Checklist[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }

        private void grd_Judgment_Liens_Checklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_Judgment_Liens_Checklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Judgment_Liens_Checklist.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_Judgment_Liens_Checklist[7, i].ReadOnly = true;
                        grd_Judgment_Liens_Checklist[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }

        private void grd_Others_Checklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_Others_Checklist.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Others_Checklist.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_Others_Checklist[7, i].ReadOnly = true;
                        grd_Others_Checklist[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }




        private async void btn_Client_Sumbit_Click(object sender, EventArgs e)
        {
            if (Validate_Client_Question_New() != false)
            {
                //cProbar.startProgress();
               
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    this.Enabled = false;
                    await  Save_General_List_New();
                    await Save_Assessor_Tax_List_New();
                    await Save_Deed_List_New();
                    await Save_Mortgage_List_New();
                    await Save_Judgment_Liens_List_New();
                    await Save_Others_List_New();
                    await Save_Client_List_New();
                    await Copy_Check_List_To_Server();

                    // cProbar.stopProgress();
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Check List is Updated Successfully");
                    if (InvokeRequired == false)
                    {
                        this.Invoke(new MethodInvoker(delegate
                        {
                            foreach (Form f in Application.OpenForms)
                            {
                                if (f.Text == "Employee_Order_Entry")
                                {
                                    IsOpen = true;
                                    f.Focus();
                                    f.Enabled = true;
                                    f.Show();
                                    break;
                                }
                            }
                        }));
                    }
                    else
                    {
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f.Text == "Employee_Order_Entry")
                            {
                                IsOpen = true;
                                f.Focus();
                                f.Enabled = true;
                                f.Show();
                                break;
                            }
                        }
                    }



                    this.Close();
                }
                catch (Exception ex)
                {
                    //Close Wait Form
                    this.Enabled = true;
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show("Error Occured Please Check With Administrator");
                }
                finally
                {
                    //Close Wait Form
                    this.Enabled = true;
                    SplashScreenManager.CloseForm(false);
                }
            }


        }

        private bool Validate_Client_Question_New()
        {

            int Checked_Cell_Count = 0;

            for (int i = 0; i < grd_Client_Specification.Rows.Count; i++)
            {



                bool chk_yes = Convert.ToBoolean(grd_Client_Specification.Rows[i].Cells["Column55"].FormattedValue);
                bool chk_no = Convert.ToBoolean(grd_Client_Specification.Rows[i].Cells["Column56"].FormattedValue);


                if (chk_yes == true)
                {
                    int check_Count = 1;
                    Checked_Cell_Count += check_Count;

                }
                if (chk_no == true)
                {
                    int check_Count = 1;

                    Checked_Cell_Count += check_Count;

                }


                if (chk_yes == false && chk_no == false)
                {
                    grd_Client_Specification.Rows[i].Cells[5].Style.BackColor = Color.Red;
                    grd_Client_Specification.Rows[i].Cells[6].Style.BackColor = Color.Red;
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                }
                else
                {
                    grd_Client_Specification.Rows[i].Cells[5].Style.BackColor = SystemColors.Control;
                    grd_Client_Specification.Rows[i].Cells[6].Style.BackColor = SystemColors.Control;

                }

                if (grd_Client_Specification[7, i].Value == null || grd_Client_Specification[7, i].Value == "")
                {
                    if (chk_no == true)
                    {
                        grd_Client_Specification[7, i].Style.BackColor = Color.Red;
                    }
                    Comments = "";


                }
                else
                {
                    grd_Client_Specification[7, i].Style.BackColor = Color.White;
                    Comments = grd_Client_Specification.Rows[i].Cells[7].Value.ToString();
                }


                if (chk_no == true && Comments.Trim().ToString() == "")
                {
                    Error_Count = 1;
                    Error_Tab_Count = 1;
                    grd_Client_Specification[7, i].Style.BackColor = Color.Red;

                    break;
                }
                else
                {

                    Error_Count = 0;

                }



            }

            if (grd_Client_Specification.Rows.Count <= 0)
            {
                return true;
            }



            if (grd_Client_Specification.Rows.Count == Checked_Cell_Count && Error_Count != 1)
            {

                return true;
            }
            else
            {
                Error_Count = 1;
                Error_Tab_Count = 1;
                MessageBox.Show("Need to Enter All the Fields");
                return false;
            }
        }

        private void grd_Client_Specification_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (e.ColumnIndex == 5)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Client_Specification.Rows[e.RowIndex].Cells["Column55"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Client_Specification.Rows[e.RowIndex].Cells["Column56"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Client_Specification[6, e.RowIndex].Value = false;
                        grd_Client_Specification[5, e.RowIndex].Value = true;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = true;

                    }
                    else if (chk_yes != true)
                    {
                        grd_Client_Specification[5, e.RowIndex].Value = true;
                        grd_Client_Specification[6, e.RowIndex].Value = false;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = false;
                        grd_Client_Specification[7, e.RowIndex].Value = "";
                    }
                }
                if (e.ColumnIndex == 6)
                {

                    bool chk_no = false;
                    chk_no = Convert.ToBoolean(grd_Client_Specification.Rows[e.RowIndex].Cells["Column56"].FormattedValue);

                    if (chk_no != false)
                    {
                        grd_Client_Specification[6, e.RowIndex].Value = true;
                        grd_Client_Specification[5, e.RowIndex].Value = false;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = true;

                        if (chk_no != false)
                        {
                            grd_Judgment_Liens_Checklist[6, e.RowIndex].Value = false;
                            grd_Judgment_Liens_Checklist[5, e.RowIndex].Value = false;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].Style.BackColor = SystemColors.Control;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].ReadOnly = true;
                            grd_Judgment_Liens_Checklist[7, e.RowIndex].Value = "";
                        }

                    }
                    else if (chk_no != true)
                    {
                        grd_Client_Specification[5, e.RowIndex].Value = false;
                        grd_Client_Specification[6, e.RowIndex].Value = true;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = Color.White;
                        grd_Client_Specification[6, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[5, e.RowIndex].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = false;
                        grd_Client_Specification[7, e.RowIndex].Value = "";
                    }

                }

                if (e.ColumnIndex == 7)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Client_Specification.Rows[e.RowIndex].Cells["Column55"].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Client_Specification.Rows[e.RowIndex].Cells["Column56"].FormattedValue);
                    if (chk_yes != false)
                    {
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = true;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = SystemColors.Control;

                    }
                    else if (chk_yes != true)
                    {

                        grd_Client_Specification[7, e.RowIndex].ReadOnly = false;
                        grd_Client_Specification[7, e.RowIndex].Style.BackColor = Color.White;

                    }

                    if (chk_yes == false && chk_no == false)
                    {

                        grd_Client_Specification.Rows[e.RowIndex].Cells[5].Style.BackColor = Color.Red;
                        grd_Client_Specification.Rows[e.RowIndex].Cells[6].Style.BackColor = Color.Red;
                        grd_Client_Specification[7, e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        grd_Client_Specification.Rows[e.RowIndex].Cells[5].Style.BackColor = SystemColors.Control;
                        grd_Client_Specification.Rows[e.RowIndex].Cells[6].Style.BackColor = SystemColors.Control;
                    }

                }
            }

        }

        private void grd_Client_Specification_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grd_Client_Specification_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Tab)
            {
                for (int i = 0; i < grd_Client_Specification.Rows.Count; i++)
                {
                    bool chk_yes = Convert.ToBoolean(grd_Client_Specification.Rows[i].Cells[5].FormattedValue);
                    bool chk_no = Convert.ToBoolean(grd_Client_Specification.Rows[i].Cells[6].FormattedValue);


                    if (chk_yes == true)
                    {
                        grd_Client_Specification[7, i].ReadOnly = true;
                        grd_Client_Specification[7, i].Style.BackColor = SystemColors.Control;
                    }

                }
                return;
            }
        }

        private async void btn_Client_View_Click(object sender, EventArgs e)
        {
            //Hashtable ht_Client_List = new Hashtable();
            DataTable dt_Client_List = new DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dictionary = new Dictionary<string, object>()
              {
                    { "@Trans", "GET_ALL_CLIENT_VIEW" },
                    { "@Ref_Checklist_Master_Type_Id", 7},
                    { "@Client_Id", clientid}
              };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/SelectAllClientView", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt_Client_List = JsonConvert.DeserializeObject<DataTable>(result);
                            // dt_Client_List = dataaccess.ExecuteSP("Sp_CheckList_ClientSpecification_Detail", ht_Client_List);
                            if (dt_Client_List.Rows.Count > 0)
                            {
                                grd_Client_Specification.Rows.Clear();
                                for (int i = 0; i < dt_Client_List.Rows.Count; i++)
                                {
                                    grd_Client_Specification.Rows.Add();
                                    grd_Client_Specification.Rows[i].Cells[0].Value = i + 1;
                                    grd_Client_Specification.Rows[i].Cells[1].Value = dt_Client_List.Rows[i]["Check_List_Tran_ID"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[2].Value = dt_Client_List.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[3].Value = dt_Client_List.Rows[i]["Question"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[4].Value = dt_Client_List.Rows[i]["Checklist_Id"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[5].Value = dt_Client_List.Rows[i]["Yes"].ToString();
                                    grd_Client_Specification.Rows[i].Cells[6].Value = dt_Client_List.Rows[i]["No"].ToString();

                                    string chk_yes = grd_Client_Specification.Rows[i].Cells[5].Value.ToString();
                                    string chk_no = grd_Client_Specification.Rows[i].Cells[6].Value.ToString();
                                    if (chk_yes == "true")
                                    {
                                        grd_Client_Specification[5, i].Value = true;
                                    }
                                    else if (chk_yes == "")
                                    {
                                        grd_Client_Specification[5, i].Value = null;
                                    }
                                    if (chk_no == "true")
                                    {
                                        grd_Client_Specification[6, i].Value = true;
                                    }
                                    else if (chk_no == "")
                                    {
                                        grd_Client_Specification[6, i].Value = null;
                                    }
                                    grd_Client_Specification.Rows[i].Cells[7].Value = dt_Client_List.Rows[i]["Comments"].ToString();
                                }
                            }
                            else
                            {
                                grd_Client_Specification.Rows.Clear();

                                Grid_Bind_All_Clients();
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

        private void btn_Client_Previous_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab("tabPage6");
        }

        private void CreateDirectory(string mainPath, string directoryPath)
        {
            try
            {
                string Ftp_Host_Name = Ftp_Domain_Name;
                Ftp_Path = Ftp_Host_Name + "/Ftp_Application_Files/OMS/" + mainPath + "";
                string[] folderArray = directoryPath.Split('/');
                string folderName = "";
                for (int i = 0; i < folderArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(folderArray[i]))
                    {
                        try
                        {
                            folderName = string.IsNullOrEmpty(folderName) ? folderArray[i] : folderName + "/" + folderArray[i];
                            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://" + Ftp_Path + "/" + folderName);
                            ftp.Credentials = credentials;
                            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                            FtpWebResponse directoryResponse = (FtpWebResponse)ftp.GetResponse();
                            if (directoryResponse.StatusCode == FtpStatusCode.PathnameCreated)
                            {
                            }
                        }
                        catch (WebException ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        //Copying Source File Into Destional Folder
        private async Task Copy_Check_List_To_Server()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                string dirTemp = "C:\\OMS\\Temp";
                if (!Directory.Exists(dirTemp))
                {
                    var dirInfo = Directory.CreateDirectory(dirTemp);
                }
                if (!File.Exists(dirTemp + "\\Order Check List Report.pdf"))
                {
                    string sourcePath = "ftp://" + Ftp_Domain_Name + "/Ftp_Application_Files/Oms_Reports/Order Check List Report.pdf";
                    Download_Ftp_File("Order Check List Report.pdf", sourcePath);
                }
                FileInfo newFile = new FileInfo(dirTemp + "\\Order Check List Report.pdf");

                rpt_Doc = new Reports.CrystalReport.Checklist_Detail_Report();
                Logon_Cr();
                rpt_Doc.SetParameterValue("@Trans", "SELECT_USER_TASK_WISE");
                rpt_Doc.SetParameterValue("@Order_Id", Order_Id);
                rpt_Doc.SetParameterValue("@Order_Task", Order_Task);
                rpt_Doc.SetParameterValue("@Log_In_Userid", 0);
                rpt_Doc.SetParameterValue("@Work_Type_Id", Work_Type_Id);//

                DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                PdfFormatOptions CrFormatTypeOptions = new PdfFormatOptions();
                CrDiskFileDestinationOptions.DiskFileName = newFile.ToString();
                ExportOptions CrExportOptions = rpt_Doc.ExportOptions;
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                rpt_Doc.Export();

                if (Work_Type_Id == 1)
                {
                    File_Name = "" + Order_Id + "-" + Task_Type.ToString() + "CheckList Report" + ".pdf";
                }
                else if (Work_Type_Id == 2)
                {
                    File_Name = "" + Order_Id + " - " + " REWORK " + Task_Type.ToString() + "CheckList" + ".pdf";
                }
                else if (Work_Type_Id == 3)
                {
                    File_Name = "" + Order_Id + " - " + " SUPER QC " + Task_Type.ToString() + "CheckList" + ".pdf";
                }

                string homeFolder = year + "/" + month + "/" + clientid + "/" + Order_Id + "";
                mainPath = "Orders_Files";
                ftpfullpath = "ftp://" + Ftp_Domain_Name + "/Ftp_Application_Files/OMS/" + mainPath + "/" + homeFolder + "";
                CreateDirectory(mainPath, homeFolder);
                string ftpUploadFullPath = "" + ftpfullpath + "/" + File_Name + "";

                // Checking File Exist or not
                FtpWebRequest ftpReq = (FtpWebRequest)WebRequest.Create(ftpfullpath); // FTP 
                ftpReq.Credentials = credentials; // Credentials  
                ftpReq.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpReq.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());
                HashSet<string> directories = new HashSet<string>(); // create list to store directories.   
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    directories.Add(line); // Add Each Directory to the List.  
                    line = streamReader.ReadLine();
                }
                if (!directories.Contains(File_Name))
                {
                    FtpWebRequest ftpUpLoadFile = (FtpWebRequest)WebRequest.Create(ftpUploadFullPath);
                    ftpUpLoadFile.Credentials = credentials;
                    ftpUpLoadFile.KeepAlive = true;
                    ftpUpLoadFile.UseBinary = true;
                    ftpUpLoadFile.Method = WebRequestMethods.Ftp.UploadFile;
                    Stream ftpstream = ftpUpLoadFile.GetRequestStream();
                    FileStream stream = new FileStream(dirTemp + "\\Order Check List Report.pdf", FileMode.Open);
                    stream.CopyTo(ftpstream);
                    ftpstream.Close();
                    //Hashtable htorderkb = new Hashtable();
                    //DataTable dtorderkb = new DataTable();
                    IDictionary<string, object> dic_OrderKb = new Dictionary<string, object>();
                    {
                        dic_OrderKb.Add("@Trans", "INSERT");
                        if (Work_Type_Id == 1)
                        {
                            dic_OrderKb.Add("@Instuction", "" + Order_Task.ToString() + "Check List Report");
                        }
                        else if (Work_Type_Id == 2)
                        {
                            dic_OrderKb.Add("@Instuction", "REWORK -" + Order_Task.ToString() + "Check List Report");
                        }
                        else if (Work_Type_Id == 2)
                        {
                            dic_OrderKb.Add("@Instuction", "SUPER QC -" + Order_Task.ToString() + "Check List Report");
                        }
                        dic_OrderKb.Add("@Order_ID", Order_Id);
                        dic_OrderKb.Add("@Document_Name", File_Name);
                        dic_OrderKb.Add("@Document_Path", ftpUploadFullPath);
                        dic_OrderKb.Add("@Inserted_By", user_ID);
                        dic_OrderKb.Add("@Inserted_date", DateTime.Now);
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dic_OrderKb), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response1 = await httpClient.PostAsync(Base_Url.Url + "/CheckList/InsertGeneralList", data);
                        if (response1.IsSuccessStatusCode)
                        {
                            if (response1.StatusCode == HttpStatusCode.OK)
                            {
                                var result2 = await response1.Content.ReadAsStringAsync();


                            }
                        }
                    }
                    //dic_OrderKb = dataaccess.ExecuteSP("Sp_Document_Upload", htorderkb);
                }
                else
                {
                    FtpWebRequest ftpDeleteFile = (FtpWebRequest)WebRequest.Create(ftpUploadFullPath);
                    ftpDeleteFile.Credentials = credentials;
                    ftpDeleteFile.KeepAlive = true;
                    ftpDeleteFile.UseBinary = true;
                    ftpDeleteFile.Method = WebRequestMethods.Ftp.DeleteFile;
                    FtpWebResponse deleteResponse = (FtpWebResponse)ftpDeleteFile.GetResponse();
                    if (deleteResponse.StatusCode == FtpStatusCode.FileActionOK)
                    {
                        FtpWebRequest ftpUpLoadFile = (FtpWebRequest)WebRequest.Create(ftpUploadFullPath);
                        ftpUpLoadFile.Credentials = credentials;
                        ftpUpLoadFile.KeepAlive = true;
                        ftpUpLoadFile.UseBinary = true;
                        ftpUpLoadFile.Method = WebRequestMethods.Ftp.UploadFile;
                        Stream ftpstream = ftpUpLoadFile.GetRequestStream();
                        FileStream stream = new FileStream(dirTemp + "\\Order Check List Report.pdf", FileMode.Open);
                        stream.CopyTo(ftpstream);
                        ftpstream.Close();
                        //Hashtable htorderkb = new Hashtable();
                        //DataTable dtorderkb = new DataTable();
                        IDictionary<string, object> dic_insert = new Dictionary<string, object>();
                        {
                            dic_insert.Add("@Trans", "INSERT");
                            if (Work_Type_Id == 1)
                            {
                                dic_insert.Add("@Instuction", "" + Order_Task.ToString() + "Check List Report");
                            }
                            else if (Work_Type_Id == 2)
                            {
                                dic_insert.Add("@Instuction", "REWORK -" + Order_Task.ToString() + "Check List Report");

                            }
                            else if (Work_Type_Id == 2)
                            {
                                dic_insert.Add("@Instuction", "SUPER QC -" + Order_Task.ToString() + "Check List Report");
                            }
                            dic_insert.Add("@Order_ID", Order_Id);
                            dic_insert.Add("@Document_Name", File_Name);
                            dic_insert.Add("@Document_Path", ftpUploadFullPath);
                            dic_insert.Add("@Inserted_By", user_ID);
                            dic_insert.Add("@Inserted_date", DateTime.Now);
                        }
                        var data2 = new StringContent(JsonConvert.SerializeObject(dic_insert), Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var response2 = await httpClient2.PostAsync(Base_Url.Url + "/CheckList/InsertGeneralList", data2);
                            if (response2.IsSuccessStatusCode)
                            {
                                if (response2.StatusCode == HttpStatusCode.OK)
                                {
                                    var result2 = await response2.Content.ReadAsStringAsync();
                                }
                            }
                        }
                        //dic_insert = dataaccess.ExecuteSP("Sp_Document_Upload", dic_insert);
                    }
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        throw new WebException("Unable to delete file");
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

        private void Download_Ftp_File(string p, string Source_Path)
        {
            try
            {
                FtpWebRequest reqFTP;
                FileStream outputStream = new FileStream("C:\\OMS\\Temp" + "\\" + p, FileMode.Create);
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(Source_Path));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(Ftp_User_Name, Ftp_Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.CopyTo(outputStream);
                //int bufferSize = 2048;
                //int readCount;
                //byte[] buffer = new byte[bufferSize];
                //readCount = ftpStream.Read(buffer, 0, bufferSize);
                //while (readCount > 0)
                //{
                //    outputStream.Write(buffer, 0, readCount);
                //    readCount = ftpStream.Read(buffer, 0, bufferSize);
                //}
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Logon_Cr()
        {
            List<string> cl_Lgoin = Comclass.Crystal_report_Login();
            crConnectionInfo.ServerName = cl_Lgoin[0].ToString();
            crConnectionInfo.DatabaseName = cl_Lgoin[1].ToString();
            crConnectionInfo.UserID = cl_Lgoin[2].ToString();
            crConnectionInfo.Password = cl_Lgoin[3].ToString();
            CrTables = rpt_Doc.Database.Tables;
            foreach (Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }
        }


        // -------------- 19 apr 2019

        //1



        private async Task Save_General_List_New()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_General_Checklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    // string Question = row.Cells[1].Value.ToString();
                    ch_Yes = Convert.ToBoolean(row.Cells["Column5"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column7"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                await Save_Check_List_New(dt);
                //IDictionary<string, object> dictionary = new Dictionary<string, object>()
                //{
                //    { "@Trans", "CHECK_FOR_ALL_TAB" },
                //    { "@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id},
                //    { "@User_id", User_id },
                //    { "@Order_Id", Order_Id },
                //    { "@Order_Task", Order_Task},
                //    { "@Work_Type", Work_Type}
                //};
                //var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                //using (var httpClient = new HttpClient())
                //{
                //    var response = await httpClient.PostAsync(Base_Url.Url + "/Check_List/CheckAllTabs", data);
                //    if (response.IsSuccessStatusCode)
                //    {
                //        if (response.StatusCode == HttpStatusCode.OK)
                //        {
                //            var result = await response.Content.ReadAsStringAsync();
                //            DataTable dtcheck = JsonConvert.DeserializeObject<DataTable>(result);

                //            if (dtcheck.Rows.Count > 0)
                //            {
                //                Count = 1;
                //            }
                //            else
                //            {
                //                Count = 0;
                //            }

                //            if (Count == 0)
                //            {

                //               await  Save_Check_List_New(dtcheck);
                //            }
                //        }
                //    }
                //}
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



        private async Task Save_Assessor_Tax_List_New()
        {
            try
            {
              
                DataTable dt_Assessor = new DataTable();
                dt_Assessor.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_AssessorTaxes_Chklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column14"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column15"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt_Assessor.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                await Save_Check_List_New(dt_Assessor);
            }

            catch (Exception ex)
            {
                throw ex;
                // Handle exception properly
            }
           
        }

        
        private async Task Save_Deed_List_New()
        {
            DataTable dt_Deed = new DataTable();
            try
            {
       
                dt_Deed.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_Deed_Checklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column22"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column23"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt_Deed.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                await Save_Check_List_New(dt_Deed);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //    IDictionary<string, object> dic_Clients = new Dictionary<string, object>()
        //{
        //    { "@Trans", "CHECK_FOR_ALL_TAB" },
        //    {"@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type_Id },
        //    { "@User_id", User_id },
        //    { "@Order_Id", Order_Id },
        //    { "@Order_Task", Order_Task },
        //    { "@Work_Type", Work_Type }
        //};
        //    var data = new StringContent(JsonConvert.SerializeObject(dic_Clients), Encoding.UTF8, "application/json");
        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.PostAsync(Base_Url.Url + "/Check_List/CheckAllTabs", data);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            if (response.StatusCode == HttpStatusCode.OK)
        //            {
        //                var result = await response.Content.ReadAsStringAsync();
        //                DataTable dtcheck_deed = JsonConvert.DeserializeObject<DataTable>(result);

        //                if (dtcheck_deed.Rows.Count > 0)
        //                {
        //                    Count = 1;
        //                }
        //                else
        //                {
        //                    Count = 0;
        //                }

        //                if (Count == 0)
        //                {

        //                    Save_Check_List_New(dtcheck_deed);

        //                }
        //            }
        //        }
        //    }


        private async Task Save_Mortgage_List_New()
        {
            DataTable dt_Mortgage = new DataTable();
            try
            {
              
                dt_Mortgage.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_Mortgage_Checklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column30"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column31"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt_Mortgage.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);
                }
                await Save_Check_List_New(dt_Mortgage);
                //    }
            }

            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private async Task Save_Judgment_Liens_List_New()
        {
            try
            {
               
                DataTable dt_Judgment = new DataTable();
                dt_Judgment.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_Judgment_Liens_Checklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column38"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column39"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt_Judgment.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                await Save_Check_List_New(dt_Judgment);
            }

            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        private async Task Save_Others_List_New()
        {
            DataTable dt_Others = new DataTable();
            try
            {
      
                dt_Others.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_Others_Checklist.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column46"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column47"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }

                    dt_Others.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                Save_Check_List_New(dt_Others);

            }

            catch (Exception ex)
            {
                throw ex;
            }
         
        }
        private async Task Save_Client_List_New()
        {
            DataTable dt_Client = new DataTable();
            try
            {
                
                dt_Client.Columns.AddRange(new DataColumn[11] {
                    new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                    new DataColumn("Checklist_Id", typeof(int)),
                    new DataColumn("Yes", typeof(Boolean)),
                    new DataColumn("No", typeof(Boolean)),
                    new DataColumn("Comments", typeof(string)),
                    new DataColumn("Order_Task", typeof(int)),
                    new DataColumn("Work_Type", typeof(int)),
                    new DataColumn("Order_Id", typeof(int)),
                    new DataColumn("Order_Type_Abs_Id", typeof(int)),
                    new DataColumn("User_id",typeof(int)) ,
                    new DataColumn("Status",typeof(Boolean)) ,
                   });

                foreach (DataGridViewRow row in grd_Client_Specification.Rows)
                {
                    Check_List_Tran_ID = int.Parse(row.Cells[0].Value.ToString());
                    Ref_Checklist_Master_Type_Id = int.Parse(row.Cells[2].Value.ToString());
                    Checklist_Id = int.Parse(row.Cells[4].Value.ToString());
                    ch_Yes = Convert.ToBoolean(row.Cells["Column55"].FormattedValue);
                    ch_No = Convert.ToBoolean(row.Cells["Column56"].FormattedValue);
                    Comments = row.Cells[7].Value.ToString();
                    Order_Task = int.Parse(Task_id);
                    Work_Type = Work_Type_Id;
                    Order_Type_Abs_Id = OrderType_ABS_Id;
                    User_id = user_ID;
                    //Order_ID = Order_Id;
                    Status = true;

                    if (ch_Yes != null && ch_Yes != false)
                    {
                        ch_Yes = true;
                    }
                    else
                    {
                        ch_Yes = false;
                    }
                    if (ch_No != null && ch_No != false)
                    {
                        ch_No = true;
                    }
                    else
                    {
                        ch_No = false;
                    }

                    if (ch_No == true && ch_Yes == false)
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    else
                    {
                        Comments = row.Cells[7].Value.ToString();
                    }
                    dt_Client.Rows.Add(Ref_Checklist_Master_Type_Id, Checklist_Id, ch_Yes, ch_No, Comments, Order_Task, Work_Type, Order_Id, Order_Type_Abs_Id, User_id, Status);

                }
                Save_Check_List_New(dt_Client);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        private async Task Save_Check_List_New(DataTable dt_Check_List)
        {
            try
            {

                using (var httpClient = new HttpClient())
                {

                    var data = new StringContent(JsonConvert.SerializeObject(dt_Check_List), Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BulkCheckListInsert", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                        }
                        UpdateModifiedDate();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task Bind_Check_List_Questions(int Ref_Checklist_Master_Type, DataGridView grd_Name, int clientid)
        {
            DataTable dtResult = new DataTable();
            DataTable dt_Check = new System.Data.DataTable();
            try
            {
                if (clientid == 0)
                {
                    Dictionary<string, object> dic_TaskWise = new Dictionary<string, object>();
                    dic_TaskWise.Add("@Trans", "CHECK_ORDER_ID_TASK_USER_WISE");
                    dic_TaskWise.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type);
                    dic_TaskWise.Add("@Order_Id", Order_Id);
                    dic_TaskWise.Add("@Order_Task", Order_Task);
                    dic_TaskWise.Add("@User_id", user_ID);
                    dic_TaskWise.Add("@Work_Type", Work_Type_Id);
                    var data = new StringContent(JsonConvert.SerializeObject(dic_TaskWise), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CheckList/BindMasterTaskWise", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                dt_Check = JsonConvert.DeserializeObject<DataTable>(result);

                            }
                        }
                    }

                    int Check = int.Parse(dt_Check.Rows[0]["count"].ToString());

                    if (Check != 0)
                    {
                        // One Ap1

                        // resukt
                        Dictionary<string, object> dic_All_Views = new Dictionary<string, object>();
                        {
                            dic_All_Views.Add("@Trans", "GET_ALL_VIEW");

                            dic_All_Views.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type);
                            dic_All_Views.Add("@Order_Task", Order_Task);
                            dic_All_Views.Add("@Order_Id", Order_Id);
                            dic_All_Views.Add("@User_Id", user_ID);
                            dic_All_Views.Add("@Work_Type", Work_Type_Id);
                        }
                        var data1 = new StringContent(JsonConvert.SerializeObject(dic_All_Views), Encoding.UTF8, "application/json");
                        using (var httpClient1 = new HttpClient())
                        {
                            var response1 = await httpClient1.PostAsync(Base_Url.Url + "/CheckList/BindAllViews", data1);
                            if (response1.IsSuccessStatusCode)
                            {
                                if (response1.StatusCode == HttpStatusCode.OK)
                                {
                                    var result1 = await response1.Content.ReadAsStringAsync();
                                    dtResult = JsonConvert.DeserializeObject<DataTable>(result1);

                                    if (dtResult.Rows.Count > 0)
                                    {
                                        grd_Name.Rows.Clear();
                                        for (int i = 0; i < dtResult.Rows.Count; i++)
                                        {
                                            grd_Name.Rows.Add();
                                            grd_Name.Rows[i].Cells[0].Value = i + 1;
                                            grd_Name.Rows[i].Cells[1].Value = dtResult.Rows[i]["Check_List_Tran_ID"].ToString();
                                            grd_Name.Rows[i].Cells[2].Value = dtResult.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                            grd_Name.Rows[i].Cells[3].Value = dtResult.Rows[i]["Question"].ToString();
                                            grd_Name.Rows[i].Cells[4].Value = dtResult.Rows[i]["Checklist_Id"].ToString();
                                            grd_Name.Rows[i].Cells[5].Value = dtResult.Rows[i]["Yes"].ToString();
                                            grd_Name.Rows[i].Cells[6].Value = dtResult.Rows[i]["No"].ToString();

                                            string chk_yes = grd_Name.Rows[i].Cells[5].Value.ToString();
                                            string chk_no = grd_Name.Rows[i].Cells[6].Value.ToString();
                                            if (chk_yes == "True")
                                            {
                                                grd_Name[5, i].Value = true;
                                            }
                                            else if (chk_yes == "")
                                            {
                                                grd_Name[5, i].Value = null;
                                            }
                                            if (chk_no == "False")
                                            {
                                                grd_Name[6, i].Value = false;
                                            }
                                            else if (chk_no == "")
                                            {
                                                grd_Name[6, i].Value = null;
                                            }
                                            grd_Name.Rows[i].Cells[7].Value = dtResult.Rows[i]["Comments"].ToString();


                                            grd_Name.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                            grd_Name.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                            grd_Name.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        }
                                    }
                                }
                                else
                                {
                                    grd_Name.Rows.Clear();
                                }
                            }
                        }
                    }
                    else if (Check == 0)
                    {
                        // resukt
                        Dictionary<string, object> dic_All_Details = new Dictionary<string, object>();
                        {
                            dic_All_Details.Add("@Trans", "GET_ALL_DETAILS");
                            dic_All_Details.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type);
                            dic_All_Details.Add("@Order_Task", Order_Task);
                            dic_All_Details.Add("@Order_Id", Order_Id);
                            dic_All_Details.Add("@Work_Type", Work_Type_Id);
                            dic_All_Details.Add("@OrderType_ABS_Id", OrderType_ABS_Id);

                        };
                        var data2 = new StringContent(JsonConvert.SerializeObject(dic_All_Details), Encoding.UTF8, "application/json");
                        using (var httpClient2 = new HttpClient())
                        {
                            var response2 = await httpClient2.PostAsync(Base_Url.Url + "/CheckList/BindMasterDetails", data2);
                            if (response2.IsSuccessStatusCode)
                            {
                                if (response2.StatusCode == HttpStatusCode.OK)
                                {
                                    var result2 = await response2.Content.ReadAsStringAsync();
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result2);
                                    if (dt.Rows.Count > 0)
                                    {
                                        grd_Name.Rows.Clear();
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            grd_Name.Rows.Add();
                                            grd_Name.Rows[i].Cells[0].Value = i + 1;
                                            grd_Name.Rows[i].Cells[2].Value = dt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                            grd_Name.Rows[i].Cells[3].Value = dt.Rows[i]["Question"].ToString();
                                            grd_Name.Rows[i].Cells[4].Value = dt.Rows[i]["Checklist_Id"].ToString();

                                            grd_Name.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                            grd_Name.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                            grd_Name.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                                        }
                                    }
                                    else
                                    {
                                        grd_Name.Rows.Clear();
                                    }

                                }
                            }
                        }
                    }

                }

                if (clientid > 0)
                {
                    Dictionary<string, object> dic_Clients = new Dictionary<string, object>();
                    {
                        dic_Clients.Add("@Trans", "GET_CLIENT_DETAILS");
                        dic_Clients.Add("@Ref_Checklist_Master_Type_Id", Ref_Checklist_Master_Type);
                        dic_Clients.Add("@Order_Task", Order_Task);
                        dic_Clients.Add("@OrderType_ABS_Id", OrderType_ABS_Id);
                        dic_Clients.Add("@Client_Id", clientid);
                    }
                    var data3 = new StringContent(JsonConvert.SerializeObject(dic_Clients), Encoding.UTF8, "application/json");
                    using (var httpClient3 = new HttpClient())
                    {
                        var response3 = await httpClient3.PostAsync(Base_Url.Url + "/CheckList/BindAllClients", data3);
                        if (response3.IsSuccessStatusCode)
                        {
                            if (response3.StatusCode == HttpStatusCode.OK)
                            {
                                var result3 = await response3.Content.ReadAsStringAsync();
                                DataTable dt_Clnt = JsonConvert.DeserializeObject<DataTable>(result3);
                                if (dt_Clnt.Rows.Count > 0)
                                {
                                    grd_Name.Rows.Clear();
                                    for (int i = 0; i < dt_Clnt.Rows.Count; i++)
                                    {
                                        grd_Name.Rows.Add();
                                        grd_Name.Rows[i].Cells[0].Value = i + 1;

                                        grd_Name.Rows[i].Cells[2].Value = dt_Clnt.Rows[i]["Ref_Checklist_Master_Type_Id"].ToString();
                                        grd_Name.Rows[i].Cells[3].Value = dt_Clnt.Rows[i]["Question"].ToString();
                                        grd_Name.Rows[i].Cells[4].Value = dt_Clnt.Rows[i]["Checklist_Id"].ToString();

                                        grd_Name.Rows[i].Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_Name.Rows[i].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                        grd_Name.Rows[i].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;


                                    }
                                }
                                else
                                {
                                    grd_Name.Rows.Clear();
                                }

                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private async void UpdateModifiedDate()
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>()
            {
                {"@Trans","UPDATE_Modified_Date"},
                { "@User_id", User_id },
                { "@Order_Id", Order_Id },
                { "@Order_Task", Order_Task },
                { "@Work_Type", Work_Type },
                { "@Modified_By",User_id}
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/JSon");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PutAsync(Base_Url.Url + "/CheckList/UpdateModifyDate", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            
    }
}
