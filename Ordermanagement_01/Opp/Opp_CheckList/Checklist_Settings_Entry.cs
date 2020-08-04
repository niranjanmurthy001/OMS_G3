﻿using System;
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
    public partial class Checklist_Settings_Entry : DevExpress.XtraEditors.XtraForm
    {
        int projectid;
        DataTable _dttabs;
        int tabid, k;
        int order_type, count, Order_Task;
        int index = 0;
        GridControl grid = new GridControl();
        DataTable dt1, dt_user, dtcopy, _dt;
        private bool IsButton { get; set; }
        private Checklist_Settings_View Mainform = null;
        public Checklist_Settings_Entry(Form CallingForm)
        {
            Mainform = CallingForm as Checklist_Settings_View;
            InitializeComponent();
        }

        private void Checklist_Settings_Load(object sender, EventArgs e)
        {
            BindProjectType();
            grd_Questions.Visible = false;
            btn_Previous.Visible = false;
            btn_Finish.Visible = false;

            // tabPane1.SelectedPageIndex = 0;
        }
        private async void Bind_Sub_Clients(int Client_Id)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_SUB_CLIENTS" },
                    {"@Client_Id",Client_Id }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindOrderTask", data).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                chk_SubClient.DataSource = dt;
                                chk_SubClient.DisplayMember = "Sub_ProcessName";
                                chk_SubClient.ValueMember = "Subprocess_Id";
                            }
                        }
                    }
                    else
                    {
                        chk_SubClient.DataSource = null;
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

        private async void BindProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_PROJECT_TYPE" }

                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[1] = "Select";
                                dt.Rows.InsertAt(dr, 0);
                            }
                            ddl_Project_Type.Properties.DataSource = dt;
                            ddl_Project_Type.Properties.DisplayMember = "Project_Type";
                            ddl_Project_Type.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddl_Project_Type.Properties.Columns.Add(col);

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
        private async void BindClientName(int _Proc)
        {
            try
            {
                ddl_Client.Properties.DataSource = null;

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_CLIENT_NAMES_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Proc }

                };
                ddl_Client.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {


                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[1] = "Select";
                                dr[2] = 0;
                                dt.Rows.InsertAt(dr, 0);
                                ddl_Client.Properties.DataSource = dt;
                                ddl_Client.Properties.DisplayMember = "Client_Name";
                                ddl_Client.Properties.ValueMember = "Client_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Client_Name");
                                ddl_Client.Properties.Columns.Add(col);



                            }


                        }
                    }
                    else
                    {
                        ddl_Client.Properties.DataSource = null;
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
        private async void BindOrderTask(int _Protid)
        {
            try
            {

                chk_OrderTask.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_ORDER_TASK_BASEDON_PROJECTID" },
                    {"@Project_Type_Id", _Protid }
                };


                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindOrderTask", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dttask = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dttask != null && dttask.Rows.Count > 0)
                            {
                                for (int i = 0; i < dttask.Rows.Count; i++)
                                {
                                    chk_OrderTask.DataSource = dttask;
                                    chk_OrderTask.DisplayMember = "Order_Status";
                                    chk_OrderTask.ValueMember = "Order_Status_ID";
                                }
                            }


                        }
                    }
                    else
                    {
                        chk_OrderTask.DataSource = null;
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
        private async void BindOrderType(int _Proc_id)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ORDER_TYPE_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Proc_id }
                };
                ddl_OrderType.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindOrderTask", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[2] = 0;
                                dr[3] = "Select";
                                dt.Rows.InsertAt(dr, 0);

                            }
                            ddl_OrderType.Properties.DataSource = dt;
                            ddl_OrderType.Properties.DisplayMember = "Order_Type_Abbreviation";
                            ddl_OrderType.Properties.ValueMember = "OrderType_ABS_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                            ddl_OrderType.Properties.Columns.Add(col);
                        }
                    }
                    else
                    {
                        ddl_OrderType.Properties.DataSource = null;
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

        private void ddl_Project_Type_EditValueChanged(object sender, EventArgs e)
        {
            projectid = Convert.ToInt32(ddl_Project_Type.EditValue.ToString());
            tabPane1.Pages.Clear();
            if (projectid > 0)
            {

                BindClientName(projectid);
                BindOrderTask(projectid);
                BindOrderType(projectid);
                chk_SubClient.DataSource = null;


            }
        }

        private void ddl_Client_EditValueChanged(object sender, EventArgs e)
        {
            int clientid = Convert.ToInt32(ddl_Client.EditValue.ToString());
            if (clientid > 0)
            {
                Bind_Sub_Clients(clientid);
            }
        }

        private async void BindTabs(int ordertypeid)
        {
            try
            {
                _dttabs = new DataTable();
                tabPane1.Pages.Clear();
                projectid = Convert.ToInt32(ddl_Project_Type.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_TABS" },
                    {"@Project_Type_Id" ,projectid},
                    {"@ProductType_Abs_Id",ordertypeid }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/Bindtabs", data);
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
                                        {"@Ref_Checklist_Id",Id },
                                    {"@Project_Type_Id",projectid },
                                    {"@ProductType_Abs_Id",ordertypeid }

                                    };
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data1);
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
                                                else if (count == 0)
                                                {
                                                    grd_Questions.Visible = false;
                                                    grd_Questions.DataSource = null;
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
                                if ((Convert.ToInt32(ddl_Project_Type.EditValue) > 0) && (Convert.ToInt32(ddl_OrderType.EditValue) > 0))
                                {

                                    string tabname = tabPane1.SelectedPage.Caption;
                                    Gettabid(tabname);

                                }
                            }
                            if (tabPane1.Pages.Count == 1)
                            {
                                btn_Add.Visible = false;
                                btn_Finish.Visible = true;
                            }
                            else
                            {
                                btn_Add.Visible = true;
                                btn_Finish.Visible = false;
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

        private async void Gettabid(string tabname)
        {
            try
            {
                dt1 = new DataTable();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","GET_TAB_ID" },
                    {"@Checklist_Type",tabname },
                    {"@Project_Type_Id",projectid }

                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data);
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
                                    {"@Trans","BIND_DATA_TO_GRID" },
                                    {"@Ref_Checklist_Id",tabid },
                                    {"@Project_Type_Id",projectid },
                                    {"@ProductType_Abs_Id",order_type }

                                };

                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            dt1 = JsonConvert.DeserializeObject<DataTable>(result1);
                                            DataView dv = new DataView(dt1);
                                            dt_user = dv.ToTable(true, "Question Number");
                                            // dt1.Columns.RemoveAt(0);
                                            if (dt1 != null && dt1.Rows.Count > 0)
                                            {
                                                // dt1.Columns.Add("Status", typeof(bool), "1");
                                                for (int i = 0; i < dt1.Rows.Count; i++)
                                                {
                                                    dt1.Rows[i]["Is_Active"] = "False";
                                                }
                                                grd_Questions.Visible = true;
                                                grd_Questions.DataSource = dt1;
                                                grd_Questions.Dock = DockStyle.Fill;
                                                tabPane1.SelectedPage.Controls.Add(grd_Questions);
                                                // dt1.Columns.Clear();
                                                getcheckdata();
                                            }

                                        }
                                    }
                                    else
                                    {
                                        grd_Questions.DataSource = null;
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

        private void ddl_OrderType_EditValueChanged(object sender, EventArgs e)
        {
            IsButton = true;
            //ddl_Client.ItemIndex = 0;
            //chk_SubClient.DataSource = null;
            order_type = Convert.ToInt32(ddl_OrderType.EditValue);

            if ((Convert.ToInt32(ddl_Project_Type.EditValue) > 0) && (Convert.ToInt32(ddl_OrderType.EditValue) > 0))
            {
                BindTabs(order_type);

            }

        }

        private async void btn_Add_Click(object sender, EventArgs e)
        {
            IsButton = true;
            try
            {
                if (validate() == true)
                {
                    if (grd_Questions.DataSource != null)
                    {


                        DataTable dt2 = new DataTable();
                        dt2 = grd_Questions.DataSource as DataTable;
                        dt2.Columns.Add("Checklist_Id", typeof(int));
                        for (int i = 0; i < dt_user.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt2.Rows.Count; j++)
                            {
                                dt2.Rows[i]["Checklist_Id"] = dt_user.Rows[i]["Question Number"];
                            }

                        }
                        DataView dv = new DataView(dt2);

                        DataTable dt3 = dv.ToTable(true, "Checklist_Id", "Is_Active");


                        DataRowView r1 = chk_SubClient.GetItem(chk_SubClient.SelectedIndex) as DataRowView;
                        int _subclient = Convert.ToInt32(r1["Subprocess_Id"]);
                        DataRowView r2 = chk_OrderTask.GetItem(chk_OrderTask.SelectedIndex) as DataRowView;
                        int _ordertask = Convert.ToInt32(r2["Order_Status_ID"]);
                        DataTable dtmulti = new DataTable();
                        dtmulti.Columns.AddRange(new DataColumn[9]
                        {

                        new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Client_Id",typeof(int)),
                        new  DataColumn("Sub_Client_Id",typeof(int)),
                        new DataColumn("Order_Task",typeof(int)),
                        new DataColumn("OrderType_ABS_Id",typeof(int)),
                        new DataColumn("User_id",typeof(int)),
                        new DataColumn("Status1",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime))
                        });

                        foreach (object itemChecked in chk_SubClient.CheckedItems)
                        {
                            DataRowView castedItem = itemChecked as DataRowView;
                            string sub = castedItem["Sub_ProcessName"].ToString();
                            int subclient = Convert.ToInt32(castedItem["Subprocess_Id"]);
                            foreach (object itemsChecked in chk_OrderTask.CheckedItems)
                            {
                                DataRowView _castedItem = itemsChecked as DataRowView;
                                string _sub = _castedItem["Order_Status"].ToString();
                                int ordertask = Convert.ToInt32(_castedItem["Order_Status_ID"]);
                                int _ProjectID = Convert.ToInt32(ddl_Project_Type.EditValue);
                                int client = Convert.ToInt32(ddl_Client.EditValue);
                                int ordertype = Convert.ToInt32(ddl_OrderType.EditValue);
                                int userid = 1;
                                int _status = 1;
                                DateTime _inserdate = DateTime.Now;
                                dtmulti.Rows.Add(tabid, _ProjectID, client, subclient, ordertask, ordertype, userid, _status, _inserdate);

                            }
                        }
                        DataTable dtfinal = new DataTable();
                        dtfinal.Columns.AddRange(new DataColumn[11]
                            {

                        new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Client_Id",typeof(int)),
                        new  DataColumn("Sub_Client_Id",typeof(int)),
                        new DataColumn("Order_Task",typeof(int)),
                        new DataColumn("OrderType_ABS_Id",typeof(int)),
                        new DataColumn("User_id",typeof(int)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime)),
                        new DataColumn("Quest_Checklist_Id",typeof(int)),
                        new DataColumn("Chk_Default",typeof(int))

                        });
                        var _results1 = from t1 in dtmulti.AsEnumerable()
                                        from t2 in dt3.AsEnumerable()
                                        select t1.ItemArray.Concat(t2.ItemArray).ToArray();
                        foreach (var allFields in _results1)
                        {
                            dtfinal.Rows.Add(allFields);
                        }
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        var data = new StringContent(JsonConvert.SerializeObject(dtfinal), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ChecklistSettings/Insert", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    this.Mainform.BindDataToGrid();

                                    tabPane1.SelectedPageIndex += 1;
                                    if ((Convert.ToInt32(ddl_Project_Type.EditValue) > 0) && (Convert.ToInt32(ddl_OrderType.EditValue) > 0))
                                    {
                                        string tabname = tabPane1.SelectedPage.Caption;
                                        Gettabid(tabname);

                                    }

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
                                        btn_Finish.Visible = true;
                                        btn_Add.Visible = false;
                                    }
                                    else
                                    {
                                        btn_Add.Visible = true;
                                        btn_Finish.Visible = false;
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
                XtraMessageBox.Show("Please contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void btn_Previous_Click(object sender, EventArgs e)
        {

            IsButton = true;
            btn_Add.Visible = true;
            btn_Finish.Visible = false;
            tabPane1.SelectedPageIndex -= 1;
            index = tabPane1.SelectedPageIndex;
            if (index == 0)
            {
                btn_Previous.Visible = false;
            }
            Previous_Data();
            ddl_OrderType_EditValueChanged(sender, e);
        }

        private void chk_SubClient_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (chk_SubClient.CheckedItems.Count == 1 && chk_OrderTask.CheckedItems.Count == 1)
            {
                getcheckdata();
            }
            else
            {
                grd_Questions.DataSource = dt1;
            }
        }

        private void tabPane1_SelectedPageChanging(object sender, SelectedPageChangingEventArgs e)
        {
            if (IsButton) e.Cancel = false;
            if (!IsButton)
            {
                e.Cancel = true;
            }
            IsButton = false;
            ///tabPane1.Pages[tabPane1.SelectedPageIndex].Focus();
            //tabPane1.Pages[tabPane1.SelectedPageIndex].CausesValidation = true;
        }

        private void Checklist_Settings_Entry_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Mainform.Enabled = true;
        }

        private void chk_OrderTask_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            getcheckdata();
        }

        private void btn_Finish_Click(object sender, EventArgs e)
        {
            btn_Add_Click(sender, e);
            if (validate() == true)
            {
                XtraMessageBox.Show("Submitted Successfully");
                this.Close();
                this.Mainform.Enabled = true;
            }

        }

        private bool validate()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Client.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_OrderType.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Order Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_SubClient.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please check Sub-Client ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_OrderTask.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please check Order Task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private async void BindDataToGrid()
        {
            try
            {
                dtcopy = new DataTable();
                DataRowView r1 = chk_SubClient.GetItem(chk_SubClient.SelectedIndex) as DataRowView;
                int _subclient1 = Convert.ToInt32(r1["Subprocess_Id"]);
                foreach (object itemChecked in chk_SubClient.CheckedItems)
                {
                    DataRowView castedItem = itemChecked as DataRowView;
                    string sub = castedItem["Sub_ProcessName"].ToString();
                    int subclient = Convert.ToInt32(castedItem["Subprocess_Id"]);

                    foreach (object checkeditem in chk_OrderTask.CheckedItems)
                    {
                        DataRowView ItemCasted = checkeditem as DataRowView;
                        Order_Task = Convert.ToInt32(ItemCasted["Order_Status_ID"]);
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        var dictonary = new Dictionary<string, object>()
                        {
                        {"@Trans","BIND_DATA_BASED_ON_SUB_CLIENT" },
                        {"@Ref_Checklist_Id",tabid },
                        {"@Project_Type_Id",projectid },
                        {"@ProductType_Abs_Id",order_type },
                        {"@subcliet_Id",subclient },
                        {"@Order_Task_Id",Order_Task }

                        };

                        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                        using (var httpclient = new HttpClient())
                        {
                            var response = await httpclient.PostAsync(Base_Url.Url + "/ChecklistSettings/BindProject", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {


                                    var result = await response.Content.ReadAsStringAsync();
                                    _dt = JsonConvert.DeserializeObject<DataTable>(result);
                                    if (_dt != null && _dt.Rows.Count > 0)
                                    {
                                        dtcopy = dt1.Copy();
                                        for (k = 0; k < dtcopy.Rows.Count; k++)
                                        {
                                            string _questionno = dtcopy.Rows[k]["Question Number"].ToString();
                                            IsMatch(_questionno);

                                        }
                                        grd_Questions.DataSource = dtcopy;

                                    }
                                }
                            }
                            else
                            {
                                grd_Questions.DataSource = dt1;
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

        private bool IsMatch(string _QNAME)
        {
            for (int j = 0; j < _dt.Rows.Count; j++)
            {
                if (_dt.Rows[j]["Question Number"].ToString() == _QNAME)
                {
                    dtcopy.Rows[k]["Is_Active"] = "True";
                    return true;
                }


            }
            return false;
        }

        private void getcheckdata()
        {
            if (chk_SubClient.CheckedItems.Count == 1 && chk_OrderTask.CheckedItems.Count == 1)
            {
                BindDataToGrid();
                // BindTabs(order_type);
            }
            else
            {
                grd_Questions.DataSource = dt1;
            }
        }

        private async void Previous_Data()
        {
            if (validate() == true)
            {
                if (grd_Questions.DataSource != null)
                {


                    DataTable dt2 = new DataTable();
                    dt2 = grd_Questions.DataSource as DataTable;
                    dt2.Columns.Add("Checklist_Id", typeof(int));
                    for (int i = 0; i < dt_user.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            dt2.Rows[i]["Checklist_Id"] = dt_user.Rows[i]["Question Number"];
                        }

                    }
                    DataView dv = new DataView(dt2);

                    DataTable dt3 = dv.ToTable(true, "Checklist_Id", "Is_Active");


                    DataRowView r1 = chk_SubClient.GetItem(chk_SubClient.SelectedIndex) as DataRowView;
                    int _subclient = Convert.ToInt32(r1["Subprocess_Id"]);
                    DataRowView r2 = chk_OrderTask.GetItem(chk_OrderTask.SelectedIndex) as DataRowView;
                    int _ordertask = Convert.ToInt32(r2["Order_Status_ID"]);
                    DataTable dtmulti = new DataTable();
                    dtmulti.Columns.AddRange(new DataColumn[9]
                    {

                        new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Client_Id",typeof(int)),
                        new  DataColumn("Sub_Client_Id",typeof(int)),
                        new DataColumn("Order_Task",typeof(int)),
                        new DataColumn("OrderType_ABS_Id",typeof(int)),
                        new DataColumn("User_id",typeof(int)),
                        new DataColumn("Status1",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime))
                    });

                    foreach (object itemChecked in chk_SubClient.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Sub_ProcessName"].ToString();
                        int subclient = Convert.ToInt32(castedItem["Subprocess_Id"]);
                        foreach (object itemsChecked in chk_OrderTask.CheckedItems)
                        {
                            DataRowView _castedItem = itemsChecked as DataRowView;
                            string _sub = _castedItem["Order_Status"].ToString();
                            int ordertask = Convert.ToInt32(_castedItem["Order_Status_ID"]);
                            int _ProjectID = Convert.ToInt32(ddl_Project_Type.EditValue);
                            int client = Convert.ToInt32(ddl_Client.EditValue);
                            int ordertype = Convert.ToInt32(ddl_OrderType.EditValue);
                            int userid = 1;
                            int _status = 1;
                            DateTime _inserdate = DateTime.Now;
                            dtmulti.Rows.Add(tabid, _ProjectID, client, subclient, ordertask, ordertype, userid, _status, _inserdate);

                        }
                    }
                    DataTable dtfinal = new DataTable();
                    dtfinal.Columns.AddRange(new DataColumn[11]
                        {

                        new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Client_Id",typeof(int)),
                        new  DataColumn("Sub_Client_Id",typeof(int)),
                        new DataColumn("Order_Task",typeof(int)),
                        new DataColumn("OrderType_ABS_Id",typeof(int)),
                        new DataColumn("User_id",typeof(int)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime)),
                        new DataColumn("Quest_Checklist_Id",typeof(int)),
                        new DataColumn("Chk_Default",typeof(int))

                    });
                    var _results1 = from t1 in dtmulti.AsEnumerable()
                                    from t2 in dt3.AsEnumerable()
                                    select t1.ItemArray.Concat(t2.ItemArray).ToArray();
                    foreach (var allFields in _results1)
                    {
                        dtfinal.Rows.Add(allFields);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtfinal), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ChecklistSettings/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                            }
                        }
                    }
                }
            }
        }
    }
}