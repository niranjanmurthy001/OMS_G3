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
using Ordermanagement_01.Masters;
using DevExpress.XtraSplashScreen;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraEditors.Repository;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Create : DevExpress.XtraEditors.XtraForm
    {
        DataTable _dtcol;
        int _ProjectId;
        string Col_Name;
        string _clodata;
        int _ordertype, _ordersourcetype, _ClientName, _ordertask, order_type, _categoryid;
        DataTable dt = new DataTable();
        DataTable dtmulti = new DataTable();
        private Efficiency_View Mainform = null;
        int user_id;
        int _UserRole;

        public Efficiency_Create(Form callingform, int userid, int userrole)
        {
            InitializeComponent();
            Mainform = callingform as Efficiency_View;
            user_id = userid;
            _UserRole = userrole;
        }

        private void Import_Category_Salary_Entry_Load(object sender, EventArgs e)
        {
            BindProjectType();
            //BindClientName();
            //BindOrderTask();
            //BindOrderType();
            // BindOrderSourceType();
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindProject", data);
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
                ddl_Client_Name.Properties.DataSource = null;
               
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_CLIENT_NAMES_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Proc }

                };
                ddl_Client_Name.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindProject", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                               
                                if (_UserRole == 1)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr[0] = 0;
                                    dr[1] = "Select";
                                    dr[2] = 0;
                                    dt.Rows.InsertAt(dr, 0);
                                    ddl_Client_Name.Properties.DataSource = dt;
                                    ddl_Client_Name.Properties.DisplayMember = "Client_Name";
                                    ddl_Client_Name.Properties.ValueMember = "Client_Id";
                                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Client_Name");
                                    ddl_Client_Name.Properties.Columns.Add(col);

                                }
                                else
                                {

                                    ddl_Client_Name.Properties.DataSource = dt;
                                    ddl_Client_Name.Properties.DisplayMember = "Client_Number";
                                    ddl_Client_Name.Properties.ValueMember = "Client_Id";
                                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Client_Number");
                                    ddl_Client_Name.Properties.Columns.Add(col);

                                }
                            }


                        }
                    }
                    else
                    {
                        ddl_Client_Name.Properties.DataSource = null;
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
                ddl_Order_task.Properties.Columns.Clear();
               
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_ORDER_TASK_BASEDON_PROJECTID" },
                    {"@Project_Type_Id", _Protid }
                };
                ddl_Order_task.Properties.Columns.Clear();
               
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindOrderTask", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                DataRow dr = dt1.NewRow();
                                dr[0] = 0;
                                dr[1] = "Select";
                                dt1.Rows.InsertAt(dr, 0);
                            }
                            ddl_Order_task.Properties.DataSource = dt1;
                            ddl_Order_task.Properties.DisplayMember = "Order_Status";
                            ddl_Order_task.Properties.ValueMember = "Order_Status_ID";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Status");
                            ddl_Order_task.Properties.Columns.Add(col);

                        }
                    }
                    else
                    {
                        ddl_Order_task.Properties.DataSource = null;
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
                chk_Ordertype.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ORDER_TYPE_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Proc_id }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindChecked", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    chk_Ordertype.DataSource = dt;
                                    chk_Ordertype.DisplayMember = "Order_Type_Abbreviation";
                                    chk_Ordertype.ValueMember = "OrderType_ABS_Id";
                                }
                            }
                        }
                    }
                    else
                    {
                        chk_Ordertype.DataSource = null;
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
        private async void BindOrderSourceType(int _Pro_Id)
        {
            try
            {

                chk_OrderSourceType.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ORDER_SOURCE_TYPE_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Pro_Id }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindChecked", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    chk_OrderSourceType.DataSource = dt;
                                    chk_OrderSourceType.DisplayMember = "Order_Source_Type_Name";
                                    chk_OrderSourceType.ValueMember = "Order_Source_Type_ID";
                                }
                            }

                        }
                    }
                    else
                    {
                        chk_OrderSourceType.DataSource = null;
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
        private async void BindColumnstogrid()
        {
            try
            {
                _dtcol = new DataTable();
                grd_CategorySalaryEntry.DataSource = null;
                gridView1.Columns.Clear();
                _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Select Headers" },
                    {"@Project_Type_Id" ,_ProjectId}

                };
                this.grd_CategorySalaryEntry.DataSource = new DataTable();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindHeaders", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt = JsonConvert.DeserializeObject<DataTable>(result);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Col_Name = Convert.ToString(dt.Rows[i]["Category_Name"]);
                                _dtcol.Columns.Add(Col_Name.ToString());
                            }
                            grd_CategorySalaryEntry.DataSource = _dtcol;
                            _dtcol.Rows.Add().ItemArray[0] = null;
                            for (int i = 0; i < gridView1.Columns.Count; i++)
                            {
                                RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
                                textEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                textEdit.Mask.EditMask = "([1-9][0-9]{0,2})";
                                textEdit.Mask.UseMaskAsDisplayFormat = true;
                                grd_CategorySalaryEntry.RepositoryItems.Add(textEdit);
                                gridView1.Columns[i].ColumnEdit = textEdit;

                                gridView1.Columns[i].AppearanceHeader.Font = new Font(gridView1.Columns[i].AppearanceHeader.Font, FontStyle.Bold);
                                gridView1.Columns[i].AppearanceHeader.ForeColor = Color.FromArgb(30, 57, 81);
                                gridView1.Columns[i].AppearanceCell.ForeColor = Color.FromArgb(30, 57, 81);
                                gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

                            }


                        }
                    }
                    else
                    {
                        grd_CategorySalaryEntry.DataSource = null;
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

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {


        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {


        }

        private void ddl_Client_Name_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void Efficiency_Create_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Mainform.Enabled = true;
        }

        private void ddl_Project_Type_EditValueChanged(object sender, EventArgs e)
        {
            ddl_Client_Name.EditValue = null;
            ddl_Order_task.EditValue = null;
            if (Convert.ToInt32(ddl_Project_Type.EditValue) != 0)
            {
                int _PID = Convert.ToInt32(ddl_Project_Type.EditValue);
                BindColumnstogrid();
                BindOrderSourceType(_PID);
                BindClientName(_PID);
                BindOrderType(_PID);
                BindOrderTask(_PID);
            }
            else
            {
                grd_CategorySalaryEntry.DataSource = null;
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_Project_Type.ItemIndex = 0;
            ddl_Order_task.ItemIndex = 0;
            ddl_Client_Name.ItemIndex = 0;
            chk_OrderSourceType.DataSource = null;
            chk_Ordertype.DataSource = null;
            _dtcol = new DataTable();
            gridView1.Columns.Clear();
        }

        private bool IsMatch(string colName)
        {
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (gridView1.Columns[i].ToString() == colName)
                {
                    _clodata = gridView1.GetRowCellValue(0, gridView1.Columns[i]).ToString();
                    //if (clodata.tostring() == "")
                    //{
                    //    //splashscreenmanager.closeform();
                    //    xtramessagebox.show("cells values must not be empty");
                    //    return false;
                    //}
                    //else
                    //{
                    //    _clodata = clodata;
                    //}
                    return true;
                }
            }
            return false;
        }
        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            dtmulti.Rows.Clear();
            if (validate() == true)
            {
                try
                {
                    var dictonary = new Dictionary<string, object>()
                    {
                        {"@Trans","GET_CATEGORY_DETAILS" }

                    };

                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindProject", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    Col_Name = Convert.ToString(dt1.Rows[i]["Category_Name"]);
                                    _categoryid = Convert.ToInt32(dt1.Rows[i]["Category_ID"]);
                                    if (IsMatch(Col_Name.ToString()))
                                    {

                                        int[] SelectedRowHandles = gridView1.GetSelectedRows();
                                        _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);
                                        _ClientName = Convert.ToInt32(ddl_Client_Name.EditValue);
                                        _ordertask = Convert.ToInt32(ddl_Order_task.EditValue);
                                        DataRowView r1 = chk_Ordertype.GetItem(chk_Ordertype.SelectedIndex) as DataRowView;
                                        _ordertype = Convert.ToInt32(r1["OrderType_ABS_Id"]);
                                        DataRowView r2 = chk_OrderSourceType.GetItem(chk_OrderSourceType.SelectedIndex) as DataRowView;
                                        _ordersourcetype = Convert.ToInt32(r2["Order_Source_Type_ID"]);
                                        if (dtmulti.Columns.Count <= 0)
                                        {
                                            dtmulti.Columns.AddRange(new DataColumn[10]
                                                                                   {
                                         new DataColumn("Project_Type_Id",typeof(int)),
                                         new DataColumn("Client",typeof(int)),
                                         new DataColumn("Order_Status_ID",typeof(int)),
                                         new DataColumn("OrderType_ABS_Id",typeof(int)),
                                         new DataColumn("Order_Source_Type_ID",typeof(int)),
                                         new DataColumn("Category_ID",typeof(int)),
                                         new DataColumn("Allocated_Time",typeof(string)),
                                         new DataColumn("Status",typeof(int)),
                                         new DataColumn("Inserted_By",typeof(int)),
                                         new DataColumn("Inserted_Date",typeof(DateTime))
                                                                                   });
                                        }

                                        foreach (object itemChecked in chk_Ordertype.CheckedItems)
                                        {
                                            DataRowView _castedItem = itemChecked as DataRowView;
                                            string Order_type = _castedItem["Order_Type_Abbreviation"].ToString();
                                            order_type = Convert.ToInt32(_castedItem["OrderType_ABS_Id"]);
                                            foreach (object itemsChecked in chk_OrderSourceType.CheckedItems)
                                            {
                                                DataRowView castedItem = itemsChecked as DataRowView;
                                                string Order_sourcetype = castedItem["Order_Source_Type_Name"].ToString();
                                                int _Order_sourcetype = Convert.ToInt32(castedItem["Order_Source_Type_ID"]);
                                                int projecttype = _ProjectId;
                                                int client = _ClientName;
                                                int ordertask = _ordertask;
                                                int _ordertype = order_type;
                                                int _category = _categoryid;
                                                string _allocatedtime = _clodata;
                                                int status = 1;
                                                int insertedby = user_id;
                                                DateTime inserteddate = DateTime.Now;
                                                dtmulti.Rows.Add(projecttype, client, ordertask, _ordertype, _Order_sourcetype, _category, _allocatedtime, status, insertedby, inserteddate);
                                            }
                                        }
                                    }
                                }
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var _data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response1 = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/Insert", _data);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var _result = await response.Content.ReadAsStringAsync();
                                            SplashScreenManager.CloseForm(false);
                                            XtraMessageBox.Show("Submitted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                                            btn_Clear_Click(sender, e);
                                            this.Mainform.BindCategorySalaryBracket();
                                            this.Mainform.Enabled = true;
                                            this.Close();
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
                    throw ex;
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        private bool validate()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Client_Name.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Order_task.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Order Task", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_Ordertype.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please check Order Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_OrderSourceType.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please check Order Source Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                string data = gridView1.GetRowCellValue(0, gridView1.Columns[i]).ToString();
                if (data == "")
                {
                    XtraMessageBox.Show("Column value must not be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gridView1.Columns[i].AppearanceCell.BackColor = Color.Red;
                    return false;
                }
                else
                {
                    gridView1.Columns[i].AppearanceCell.BackColor = Color.White;
                }
            }
            return true;
        }
    }
}