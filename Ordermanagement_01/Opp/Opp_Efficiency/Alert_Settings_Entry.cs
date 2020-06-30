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
using Newtonsoft.Json;
using System.Net.Http;
using Ordermanagement_01.Masters;
using System.Net;
using Ordermanagement_01.Models;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Alert_Settings_Entry : DevExpress.XtraEditors.XtraForm
    {
        int UserroleId, userid;
        private Alert_Setings Mainfrom = null;
        public Alert_Settings_Entry(int User_Id,Form callingfrom)
        {
            userid = User_Id;
            Mainfrom =callingfrom as  Alert_Setings;
            InitializeComponent();
        }

        private void Alert_Settings_Load(object sender, EventArgs e)
        {
            BindUsers();
            

        }
        private async void BindUsers()
        {
            try
            {
                ddl_user_role.Properties.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_USERS" }
                };
                ddl_user_role.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/AlertType/BindUsers", data);
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
                            ddl_user_role.Properties.DataSource = dt;
                            ddl_user_role.Properties.DisplayMember = "Role_Name";
                            ddl_user_role.Properties.ValueMember = "Role_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Role_Name");
                            ddl_user_role.Properties.Columns.Add(col);

                        }
                    }
                    else
                    {
                        ddl_user_role.Properties.DataSource = null;
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
        private async void BindAlertType()
        {
            try
            {
                UserroleId = Convert.ToInt32(ddl_user_role.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_ALERT_TYPE" },
                    {"@User_Role_Id",UserroleId }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/AlertType/BindAlertType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                chk_Alert_Type.DataSource = dt;
                                chk_Alert_Type.DisplayMember = "Alert_Type";
                                chk_Alert_Type.ValueMember = "Alert_Type_Id";
                            }
                            

                        }
                    }
                    else
                    {
                        chk_Alert_Type.DataSource = null;
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

        private void ddl_user_role_EditValueChanged(object sender, EventArgs e)
        {
            BindAlertType();
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_user_role.ItemIndex = 0;
            chk_Alert_Type.DataSource = null;
        }
        private bool Validation()
        {

            if(Convert.ToInt32(ddl_user_role.EditValue)==0)
            {
                XtraMessageBox.Show("Please Select User-Role", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(chk_Alert_Type.CheckedItems.Count==0)
            {
                XtraMessageBox.Show("Please Check Alert-Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void Alert_Settings_Entry_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Mainfrom.Enabled = true;
        }

        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            if (Validation() == true)
            {
                try
                {
                    UserroleId = Convert.ToInt32(ddl_user_role.EditValue);
                    DataRowView r1 = chk_Alert_Type.GetItem(chk_Alert_Type.SelectedIndex) as DataRowView;
                   int Alert_type = Convert.ToInt32(r1["Alert_Type_Id"]);
                    DataTable dtmulti = new DataTable();
                    dtmulti.Columns.AddRange(new DataColumn[5]
                    {
                        new DataColumn("Alert_Type_Id",typeof(int)),
                        new DataColumn("User_Role_Id",typeof(int)),
                        new  DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("Inserted_Date",typeof(DateTime))
                    });
                    foreach (object itemChecked in chk_Alert_Type.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Alert_Type"].ToString();
                        int Alertid = Convert.ToInt32(castedItem["Alert_Type_Id"]);
                        int status = 1;
                        int _Inserted_By = userid;
                        DateTime date = DateTime.Now;
                        int userrole_id = UserroleId;
                        dtmulti.Rows.Add(Alertid, userrole_id, status, _Inserted_By, date);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/AlertType/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Sucessfully");
                                btn_Clear_Click(sender, e);
                                this.Mainfrom.BindUserwiseAlertType();
                                this.Close();
                                this.Mainfrom.Enabled = true;
                                
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
}