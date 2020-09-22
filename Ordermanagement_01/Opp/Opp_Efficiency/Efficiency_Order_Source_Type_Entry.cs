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
using DevExpress.XtraLayout.Helpers;
using DevExpress.XtraBars.Alerter;
using System.Collections;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Order_Source_Type_Entry : DevExpress.XtraEditors.XtraForm
    {
        private int Project_Id;
        private int State_Id;
        int ProjectValue;
        int SourceValue;
        int StateId;
        int CountyId;
        int User_Id;
        int Task;

        private Efficiency_Order_Source_Type Mainfrom = null;
        private int county_Id;
       
        public Efficiency_Order_Source_Type_Entry(int User_ID, Form CallingFrom)
        {
            InitializeComponent();
            Mainfrom = CallingFrom as Efficiency_Order_Source_Type;          
            User_Id = User_ID;         
        }

        private void Efficiency_Order_Source_Type_Entry_Load(object sender, EventArgs e)
        {
           // Clear();
            ddl_Source_Type.EditValue = null;    
            BindProjectType();
            BindState();
            btn_Save.Text = "Save";          
        }
        public async void BindProjectType()
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/BindProjectType", data);
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

                            ddl_ProjectType.Properties.DataSource = dt;
                            ddl_ProjectType.Properties.DisplayMember = "Project_Type";
                            ddl_ProjectType.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddl_ProjectType.Properties.Columns.Add(col);
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
        private async void BindSourceType(int Project_Id)
        {
            try
            {
               ddl_Source_Type.Properties.Columns.Clear();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_SOURCE_TYPE" },
                    {"@Project_Type_Id ",Project_Id}

                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/BindSourceType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[1] = "Select";
                                dr[0] = 0;
                                dt.Rows.InsertAt(dr, 0);
                                ddl_Source_Type.Properties.DataSource = dt;
                                ddl_Source_Type.Properties.DisplayMember = "Employee_source";
                                ddl_Source_Type.Properties.ValueMember = "Employee_Source_id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Employee_source");
                                ddl_Source_Type.Properties.Columns.Add(col);
                            }
                            else
                            {
                                ddl_Source_Type.Properties.DataSource = null;
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

        private async void BindState()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_STATE" }

                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/BindState", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[1] = "Select";
                                dr[0] = 0;
                                dt.Rows.InsertAt(dr, 0);

                                ddl_State.Properties.DataSource = dt;
                                ddl_State.Properties.DisplayMember = "State";
                                ddl_State.Properties.ValueMember = "State_ID";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("State");
                                ddl_State.Properties.Columns.Add(col);
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
        private async void BindCounty(int State_Id)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_COUNTY"},
                    {"@State_ID",State_Id },
                    {"@Project_Type_Id",ddl_ProjectType.EditValue },
                    {"@Order_Source_Type_ID",ddl_Source_Type.EditValue }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/BindCounty", data);
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
                                    chk_County.DataSource = dt;
                                    chk_County.DisplayMember = "County";
                                    chk_County.ValueMember = "County_ID";
                                }
                               // StateExist();
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
        //private async void StateExist()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        if (ddl_State.EditValue != null)
        //        {
        //            var dictionary = new Dictionary<string, object>()
        //        {
        //            { "@Trans", "StateExist" },
        //            { "@State_ID", ddl_State.EditValue },
        //            { "@Project_Type_Id", ddl_ProjectType.EditValue },
        //            { "@Order_Source_Type_ID", ddl_Source_Type.EditValue}
        //        };
        //            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
        //            using (var httpClient = new HttpClient())
        //            {
        //                var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/CheckState", data);
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    if (response.StatusCode == HttpStatusCode.OK)
        //                    {
        //                        var result = await response.Content.ReadAsStringAsync();
        //                        DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
        //                        if (dt1.Rows.Count > 0)
        //                        {
        //                            foreach (DataRow row in dt1.Rows)
        //                            {
        //                                int county_Id = Convert.ToInt32(row.ItemArray[0]);
        //                                chk_County.SelectedValue = county_Id;
        //                                int _task = chk_County.SelectedIndex;
        //                                chk_County.SetItemChecked(_task, true);
        //                              btn_Save.Text = "Update";
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        XtraMessageBox.Show("Error", "Please Contact Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);
        //    }
        //}
     
        private bool Validate()
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_Source_Type.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Source Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_State.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select State", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_County.CheckedItems == null || chk_County.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check any of the County", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void Clear()
        {
            ddl_ProjectType.EditValue = null;
            ddl_Source_Type.EditValue = null;
            ddl_State.EditValue = null;
            chk_County.UnCheckAll();
            chk_County.DataSource = null;

        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            ProjectValue = Convert.ToInt32(ddl_ProjectType.EditValue);
            SourceValue = Convert.ToInt32(ddl_Source_Type.EditValue);
            StateId = Convert.ToInt32(ddl_State.EditValue);
           
            if (btn_Save.Text == "Save" && Validate() != false)
            {
                try
                {
                    DataRowView r1 = chk_County.GetItem(chk_County.SelectedIndex) as DataRowView;
                    CountyId = Convert.ToInt32(r1["County_ID"]);
                    DataTable dtinsert = new DataTable();
                    dtinsert.Columns.AddRange(new DataColumn[7]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Order_Source_Type_ID",typeof(int)),
                     new DataColumn("State_ID",typeof(int)),
                     new DataColumn("County_ID",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                    });
                    foreach (object itemchecked in chk_County.CheckedItems)
                    {
                        //DataRowView CastedItems = itemchecked as DataRowView;
                        //CountyId = Convert.ToInt32(CastedItems["County_ID"]);

                        DataRowView castedItem = itemchecked as DataRowView;
                        string sub = castedItem["County"].ToString();
                        int CountyId = Convert.ToInt32(castedItem["County_ID"]);
                        int projecttype = ProjectValue;
                        int sourcetype = SourceValue;
                        int state = StateId;
                        dtinsert.Rows.Add(projecttype, sourcetype, state, CountyId, User_Id, DateTime.Now, "True");
                    }
                    //   dtinsert.Rows.Add(ProjectValue, SourceValue, StateId, CountyId, User_Id, DateTime.Now, "True");                                    
                    var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencyOrderSourceType/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Sucessfully", "Success", MessageBoxButtons.OK);
                                Clear();
                                this.Mainfrom.BindGrid();
                                this.Mainfrom.Enabled = true;
                                this.Close();                                                          
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
            else if (btn_Save.Text == "Update" && Validate() != false)
            {
                try
                {

                    DataRowView r1 = chk_County.GetItem(chk_County.SelectedIndex) as DataRowView;
                    CountyId = Convert.ToInt32(r1["County_ID"]);
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    DataTable dtupdate = new DataTable();
                    dtupdate.Columns.AddRange(new DataColumn[7]
                    {
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Order_Source_Type_ID",typeof(int)),
                     new DataColumn("State_ID",typeof(int)),
                     new DataColumn("County_ID",typeof(int)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool))
                    });
                    foreach (object item in chk_County.CheckedItems)
                    {
                        DataRowView castedItem = item as DataRowView;
                        int _CountyId = Convert.ToInt32(castedItem["County_ID"]);
                        int _ProjectValue = ProjectValue;
                        int _SourceValue = SourceValue;
                        int _StateId = StateId;
                      //  int _status = 1;
                        dtupdate.Rows.Add(_ProjectValue, _SourceValue, _StateId, _CountyId, User_Id, DateTime.Now, "True");
                    }
                    for (int i = 0; i < chk_County.ItemCount; i++)
                    {
                        if (chk_County.GetItemChecked(i) == false)
                        {
                            //  DataRowView castedItem = i as DataRowView;
                            int _CountyId = Convert.ToInt32(chk_County.GetItemValue(i));
                            int _ProjectValue = ProjectValue;
                            int _SourceValue = SourceValue;
                            int _StateId = StateId;
                            //int status = 0;
                            dtupdate.Rows.Add(_ProjectValue, _SourceValue, _StateId, _CountyId, User_Id, DateTime.Now, "False");
                        }
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dtupdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/EfficiencyOrderSourceType/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Updated Successfully");
                                Clear();
                                this.Mainfrom.BindGrid();
                                this.Mainfrom.Enabled = true;
                                this.Close();                                                  
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
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }
     
        private void ddl_State_EditValueChanged_1(object sender, EventArgs e)
        {

            if (ddl_State.ItemIndex > 0)
            {
                State_Id = Convert.ToInt32(ddl_State.EditValue);
                chk_County.DataSource = null;
                BindCounty(State_Id);               
            }
        }

        private void chk_All_CheckedChanged_1(object sender, EventArgs e)
        {

            if (chk_All.Checked == true)
            {
                chk_County.CheckAll();
            }
            if (chk_All.Checked == false)
            {
                chk_County.UnCheckAll();
            }
        }

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddl_ProjectType.ItemIndex > 0)
            {
                Project_Id = Convert.ToInt32(ddl_ProjectType.EditValue);
                ddl_Source_Type.EditValue = null;
                BindSourceType(Project_Id);               
            }
        }

        private void Efficiency_Order_Source_Type_Entry_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Mainfrom.Enabled = true;
        }
    }
}