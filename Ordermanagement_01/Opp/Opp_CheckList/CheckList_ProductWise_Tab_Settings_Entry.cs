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

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckList_ProductWise_Tab_Settings_Entry : XtraForm
    {
        CheckList_Master_View mainform = null;
        public CheckList_ProductWise_Tab_Settings_Entry(Form CallingFrom)
        {
            InitializeComponent();
            mainform = CallingFrom as CheckList_Master_View;

        }

        private void CheckList_ProductWise_Tab_Settings_Entry_Load(object sender, EventArgs e)
        {
            BindProjectType();
        }
        public async void BindProjectType()
        {
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                ddl_ProjectType.Properties.Columns.Clear();
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Get_Project_Type" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindProject", data);
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
                                dr[1] = "SELECT";
                                dt.Rows.InsertAt(dr, 0);

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

        private async void BindProductType(int _ProjectID)
        {
            try
            {
                ddl_ProductType.Properties.Columns.Clear();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_ORDER_ABS"},
                    {"@Project_Type_Id",_ProjectID }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindProductTypeAbs", data);
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
                                dr[1] = "SELECT";
                                dt.Rows.InsertAt(dr, 0);
                                ddl_ProductType.Properties.DataSource = dt;
                                ddl_ProductType.Properties.DisplayMember = "Order_Type_Abbreviation";
                                ddl_ProductType.Properties.ValueMember = "OrderType_ABS_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                ddl_ProductType.Properties.Columns.Add(col);
                            }
                            else
                            {
                                ddl_ProductType.Properties.DataSource = null;
                            }
                        }
                    }
                    else
                    {
                        ddl_ProductType.Properties.DataSource = null;
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

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
                int ProjId = Convert.ToInt32(ddl_ProjectType.EditValue);

                ddl_ProductType.Properties.Columns.Clear();
                ddl_ProductType.ItemIndex = 0;
                BindProductType(ProjId);
            }
        }

        private void ddl_ProductType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProductType.EditValue) != 0)
            {
                int ProjId = Convert.ToInt32(ddl_ProjectType.EditValue);
                int prodid = Convert.ToInt32(ddl_ProductType.EditValue);
               // chk_CheckListTab.DataSource = null;
                BindCheckListTabName(ProjId,prodid);
            }
        }

        private async void BindCheckListTabName(int projectType_id,int ProductType_Id)
        {
            try
            {
                chk_CheckListTab.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"bindCheckListTabData"},
                    {"@Project_Type_Id" , projectType_id},
                    {"@Product_Type_Abbr_Id" ,ProductType_Id}

                };

                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindTabNames", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                chk_CheckListTab.DataSource = dt;
                                chk_CheckListTab.DisplayMember = "Checklist_Master_Type";
                                chk_CheckListTab.ValueMember = "ChecklistType_Id";


                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong ! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void CheckList_ProductWise_Tab_Settings_Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainform.Enabled = true;
        }

        private async void btn_Add_Click(object sender, EventArgs e)
        {


            int ProjectValue = Convert.ToInt32(ddl_ProjectType.EditValue);
            int ProductValue = Convert.ToInt32(ddl_ProductType.EditValue);
            if (btn_Add.Text == "Save" && validate() != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    DataTable dtInsert = new DataTable();
                    dtInsert.Columns.AddRange(new DataColumn[]
                      {


                        new DataColumn("Project_Type_Id",typeof(int)) ,
                        new DataColumn("Product_Type_Abbr_Id",typeof(int)),
                        new DataColumn("CheckList_Type_Id",typeof(int)),
                        new DataColumn("Status",typeof(bool))

                    });
                    foreach (object itemChecked in chk_CheckListTab.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        int chk_List_Id = int.Parse(castedItem["ChecklistType_Id"].ToString());
                        dtInsert.Rows.Add(ProjectValue, ProductValue, chk_List_Id, true);
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dtInsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/InsertProductTypeTab", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();


                                Clear();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Sucessfully", "Success");
                                this.Close();
                                this.mainform.Enabled = true;
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

        private void Clear()
        {
            
                chk_CheckListTab.DataSource = null;
           

                ddl_ProductType.ItemIndex = 0;
                ddl_ProjectType.ItemIndex = 0;

        }

        public bool validate()
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_ProductType.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Product Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_CheckListTab.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check CheckList Tab ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void chk_AllTabs_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_AllTabs.Checked==true)
            {
                chk_CheckListTab.CheckAll();
            }
           else  if(chk_AllTabs.Checked==false)
            {
                chk_CheckListTab.UnCheckAll();
            }
        }
    }
}

