using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class ProjectType_Notification_Setitings_Entry : XtraForm
    {
        int projectvalue;
        string Messagetext;
        int User_id;
        string Operation_Type;
        int ProjectTypeId;
        string ViewMsgtext;
        string Editbtn_name;
        int Upload_Id;
        private ProjectType_Notification_Settings_View mainform = null;
        public ProjectType_Notification_Setitings_Entry(string OperationType, int uploadId, int ProjectTypeId, string message, int User_Id, string btn_name, Form CallingFrom)
        {
            InitializeComponent();
            mainform = CallingFrom as ProjectType_Notification_Settings_View;
            this.User_id = User_Id;
            this.Operation_Type = OperationType;
            this.ProjectTypeId = ProjectTypeId;
            this.ViewMsgtext = message;
            this.Editbtn_name = btn_name;
            this.Upload_Id = uploadId;
        }

        private void ProjectType_Notification_Setitings_Entry_Load(object sender, EventArgs e)
        {
            BindProjectType();
            
            if (Operation_Type == "Update")
            {
                ddlProjectType.EditValue = ProjectTypeId;
                btnSave.Text = Editbtn_name;
                txtMessage.Text = ViewMsgtext;
            }
            else
            Clear();
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/BindProjectType", data);
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

                            ddlProjectType.Properties.DataSource = dt;
                            ddlProjectType.Properties.DisplayMember = "Project_Type";
                            ddlProjectType.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddlProjectType.Properties.Columns.Add(col);

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

        public bool validation()
        {
            if (Convert.ToInt32(ddlProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtMessage.Text == "")
            {
                XtraMessageBox.Show("Please Enter Message", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;

        }
        private async void btnSave_Click(object sender, EventArgs e)
        {
            projectvalue = Convert.ToInt32(ddlProjectType.EditValue);
            Messagetext = txtMessage.Text;
            if (btnSave.Text == "Save" && validation() != false)
            {
                try
                {
                    DataTable dtadd = new DataTable();
                    dtadd.Columns.AddRange(new DataColumn[]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Message",typeof(string)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new  DataColumn("Status",typeof(bool))


                    });
                    dtadd.Rows.Add(projectvalue, Messagetext, User_id, DateTime.Now, "True");
                    var data = new StringContent(JsonConvert.SerializeObject(dtadd), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Inserted Successfully");
                                this.mainform.BindGrid();
                                Clear();
                                this.Close();
                                this.mainform.Enabled = true;

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Somethinfg Went Wrong");
                }
            }
            else if (btnSave.Text == "Edit" && validation() != false && Upload_Id != 0)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "UPDATE" },
                        { "@Gen_Update_ID", Upload_Id },
                        {"@Project_Type_Id",projectvalue},
                        { "@Message",Messagetext  },
                        { "@Modified_By", User_id },
                       { "@Modified_Date", DateTime.Now }
                    };

                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                Clear();
                                XtraMessageBox.Show("Updated Successfully");
                                this.mainform.BindGrid();
                               
                                Upload_Id = 0;
                                btnSave.Text = "Save";
                                Operation_Type = "Save";
                                this.Close();
                                this.mainform.Enabled = true;
                            }
                        }



                    }


                }

                catch (Exception ex)
                {
                    //throw ex;
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something Went Wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
        }
        private void Clear()
        {
            ddlProjectType.ItemIndex = 0;
            txtMessage.Text = "";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void ProjectType_Notification_Setitings_Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainform.Enabled = true;
        }
    }
}
