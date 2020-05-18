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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Project_Type_Order_Task : DevExpress.XtraEditors.XtraForm
    {
        int Project_type, Task;
        DataTable _dtLoad = new DataTable();
        public Project_Type_Order_Task()
        {
            InitializeComponent();
        }


        private async void BindProjecttype()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_PROJECT_TYPE" }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Master/BindClients", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = "Select";
                                dr[1] = 0;
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
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void Project_Type_Order_Task_Load(object sender, EventArgs e)
        {
            BindProjecttype();
            BindDepartmentType();
            grid_Project_Type_Details();
        }
        private bool Validates()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Project_Type");
                return false;
            }
            if (checkedListBoxControl_Task.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Select Task Type");
                return false;
            }
            return true;
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            Project_type = Convert.ToInt32(ddl_Project_Type.EditValue);
            if (btn_Save.Text == "Submit" && Validates() != false)
            {
                try
                {
                    DataRowView r1 = checkedListBoxControl_Task.GetItem(checkedListBoxControl_Task.SelectedIndex) as DataRowView;
                    Task = Convert.ToInt32(r1["Order_Status_ID"]);
                    DataTable dtmulti = new DataTable();
                    dtmulti.Columns.AddRange(new DataColumn[4]
                    {
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Order_Task_ID",typeof(int)),
                        new  DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime))
                    });
                    foreach (object itemChecked in checkedListBoxControl_Task.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Order_Status"].ToString();
                        int Task = Convert.ToInt32(castedItem["Order_Status_ID"]);
                        int projecttype = Project_type;
                        int status = 1;
                        DateTime date = DateTime.Now;
                        dtmulti.Rows.Add(projecttype, Task, status, date);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Projecttypeordertask/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Order Task is Submitted");
                                grid_Project_Type_Details();
                                btn_Clear_Click(sender, e);
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
            else if (btn_Save.Text == "Update" && Validates() != false)
            {
                try
                {
                    DataRowView r1 = checkedListBoxControl_Task.GetItem(checkedListBoxControl_Task.SelectedIndex) as DataRowView;
                    Task = Convert.ToInt32(r1["Order_Status_ID"]);
                    //var dictionary = new Dictionary<string, object>
                    //{
                    //{"@Trans","Update" },
                    //{"@Project_Type_Id",Project_type},
                    //{"@Order_Task_Id", Task}
                    //};
                    DataTable dtmulti1 = new DataTable();
                    dtmulti1.Columns.AddRange(new DataColumn[3]
                    {
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Order_Task_ID",typeof(int)),
                        new DataColumn("Modified_Date",typeof(DateTime))
                    });
                    foreach (object itemChecked in checkedListBoxControl_Task.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Order_Status"].ToString();
                        int Task = Convert.ToInt32(castedItem["Order_Status_ID"]);
                        int projecttype = Project_type;
                        DateTime date = DateTime.Now;
                        dtmulti1.Rows.Add(projecttype, Task, date);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtmulti1), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Projecttypeordertask/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Order Task is Updated");
                                grid_Project_Type_Details();
                                btn_Clear_Click(sender, e);
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
        private async void BindDepartmentType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ORDER_TASK" }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Projecttypeordertask/BindOrderTask", data);
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
                                    checkedListBoxControl_Task.DataSource = dt;
                                    checkedListBoxControl_Task.DisplayMember = "Order_Status";
                                    checkedListBoxControl_Task.ValueMember = "Order_Status_ID";
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

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.Column.FieldName == "Project_Type")
                {
                    btn_Save.Text = "Update";
                    ddl_Project_Type.Enabled = false;
                    // GridView view = grd_projectType.MainView as GridView;
                    //var index = view.GetDataRow(view.GetSelectedRows()[0]);
                    var row = _dtLoad.AsEnumerable().Where(dr => dr.Field<string>("Project_Type") == e.CellValue.ToString());
                    var index = row.FirstOrDefault();
                    ddl_Project_Type.EditValue = index.ItemArray[2];
                    int _client = Convert.ToInt32(ddl_Project_Type.EditValue);
                    int Task = Convert.ToInt32(index.ItemArray[3]);
                    checkedListBoxControl_Task.SelectedValue = Task;
                }
                int _task = checkedListBoxControl_Task.SelectedIndex;
                checkedListBoxControl_Task.SetItemChecked(_task, true);
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

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_Project_Type.ItemIndex = 0;
            checkedListBoxControl_Task.UnCheckAll();
            checkedListBoxControl_Task.SelectedIndex = 0;
            btn_Save.Text = "Submit";
            ddl_Project_Type.Enabled = true;
        }

        private async void grid_Project_Type_Details()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Projecttypeordertask/BindProjectsDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dtLoad = JsonConvert.DeserializeObject<DataTable>(result);
                            //DataTable uniqueCols = dt.DefaultView.ToTable(true, dt.Columns[0].ColumnName);
                            if (_dtLoad.Rows.Count > 0)
                            {
                                //.DefaultView.ToTable(true,dt.Columns[0].ColumnName,dt.Columns[2].ColumnName)


                                grd_projectType.DataSource = _dtLoad.DefaultView.ToTable(true, _dtLoad.Columns[0].ColumnName);
                                gridView1.BestFitColumns();
                            }
                            else
                            {
                                grd_projectType.DataSource = null;
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

        private void ddl_Project_Type_EditValueChanged(object sender, EventArgs e)
        {
            //btn_Save.Text = "Submit";
            Getdata();
            checkedListBoxControl_Task.UnCheckAll();
            //btn_Clear_Click(sender, e);
        }

      
        private async void Getdata()
        {
            int INdex = Convert.ToInt32(ddl_Project_Type.EditValue);
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GetItems"},
                        {"@Project_Type_Id",INdex }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Projecttypeordertask/BindItems", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    int item_ = Convert.ToInt32(row.ItemArray[0]);
                                    checkedListBoxControl_Task.SelectedValue = item_;
                                    int _task = checkedListBoxControl_Task.SelectedIndex;
                                    checkedListBoxControl_Task.SetItemChecked(_task, true);
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
}