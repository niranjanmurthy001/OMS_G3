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
    public partial class CheckList_Question_Setup_Entry : XtraForm
    {
        string Oper_Type;
        string BtnNameValue;
        int ProjectIdValue;
        int MasterChklistIdValue;
        int ProductTypeAbbrIdValue;
        string QuestionValue;
        int checklistValue;
        string PrdTypeAbbrValue;
        public CheckList_Master_View mainForm = null;

        public CheckList_Question_Setup_Entry(string OperType, string BtnName, int ProjectId, int ChkListMasterId, int ProductTyeAbbrId, string ProductAbbr, int chkListType, string Question, Form CallingFrom)
        {
            InitializeComponent();
            this.Oper_Type = OperType;
            this.BtnNameValue = BtnName;
            this.ProjectIdValue = ProjectId;
            this.MasterChklistIdValue = ChkListMasterId;
            this.ProductTypeAbbrIdValue = ProductTyeAbbrId;
            this.checklistValue = chkListType;
            this.PrdTypeAbbrValue = ProductAbbr;

            this.QuestionValue = Question;
            mainForm = CallingFrom as CheckList_Master_View;

        }

        private void CheckList_Question_Setup_Entry_Load(object sender, EventArgs e)
        {
            BindProjectTypeForSetUp();
            if (Oper_Type == "Update")
            {
                ddl_projectTypeQuesSetup.EditValue = ProjectIdValue;

                BindOrderTypeAbbrForQuesSetUp(ProjectIdValue);
                BindCheckListTabName(PrdTypeAbbrValue);
                txtQuestionQs.Text = QuestionValue;
                btn_SaveQs.Text = BtnNameValue;
            }
        }
        public async void BindProjectTypeForSetUp()
        {
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Get_Project_Type" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindProjectType", data);
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

                                ddl_projectTypeQuesSetup.Properties.DataSource = dt;
                                ddl_projectTypeQuesSetup.Properties.DisplayMember = "Project_Type";
                                ddl_projectTypeQuesSetup.Properties.ValueMember = "Project_Type_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                                ddl_projectTypeQuesSetup.Properties.Columns.Add(col);
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
        private async void BindOrderTypeAbbrForQuesSetUp(int ProjectId_)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_ORDER_ABS"},
                    {"@Project_Type_Id",ProjectId_ }
                };
               // ddl_ProdductTypeAbbrQs.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindOrderTaskAbbr", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0 && Oper_Type =="QuestionSetUp")
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[1] = "Select";
                                dt.Rows.InsertAt(dr, 0);

                                ddl_ProdductTypeAbbrQs.Properties.DataSource = dt;
                                ddl_ProdductTypeAbbrQs.Properties.DisplayMember = "Order_Type_Abbreviation";
                                ddl_ProdductTypeAbbrQs.Properties.ValueMember = "OrderType_ABS_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                ddl_ProdductTypeAbbrQs.Properties.Columns.Add(col);
                            }
                            else if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[0] = 0;
                                dr[1] = "Select";

                                dt.Rows.InsertAt(dr, 0);
                                ddl_ProdductTypeAbbrQs.Properties.DataSource = dt;
                                ddl_ProdductTypeAbbrQs.Properties.DisplayMember = "Order_Type_Abbreviation";
                                ddl_ProdductTypeAbbrQs.Properties.ValueMember = "OrderType_ABS_Id";
                                ddl_ProdductTypeAbbrQs.ItemIndex = ProductTypeAbbrIdValue;
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col1;
                                col1 = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                ddl_ProdductTypeAbbrQs.Properties.Columns.Add(col1);
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
        private async void BindCheckListTabName(string ProductTypeAbbr)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SelectCheckListTabName"},
                    {"@Product_Type_Abbr" , ProductTypeAbbr}

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
                            if (dt != null && dt.Rows.Count > 0 && Oper_Type == "QuestionSetUp")
                            {
                                chk_TabNamesQs.DataSource = dt;
                                chk_TabNamesQs.DisplayMember = "Checklist_Master_Type";
                                chk_TabNamesQs.ValueMember = "ChecklistType_Id";


                            }
                            else if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    chk_TabNamesQs.DataSource = dt;
                                    chk_TabNamesQs.DisplayMember = "Checklist_Master_Type";
                                    chk_TabNamesQs.ValueMember = "ChecklistType_Id";
                                    chk_TabNamesQs.SelectedValue = checklistValue;
                                    int TabValue = chk_TabNamesQs.SelectedIndex;
                                    chk_TabNamesQs.SetItemChecked(TabValue, true);
                                }
                                
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
        private async void btn_SaveQs_Click(object sender, EventArgs e)
        {
            string QuestionEnterValue = txtQuestionQs.Text;
            int ProjectValue = Convert.ToInt32(ddl_projectTypeQuesSetup.EditValue);
            string btnProductTypeAbbrValue = ddl_ProdductTypeAbbrQs.Text.ToString();

            if (btn_SaveQs.Text == "Save" && validate() != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    DataTable dtInsert = new DataTable();
                    dtInsert.Columns.AddRange(new DataColumn[]
                      {
                        
                        new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                        new DataColumn("Question",typeof(string)) ,
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Abbr",typeof(string)) ,

                        new DataColumn("Is_Active",typeof(bool))

                    });
                    foreach (object itemChecked in chk_TabNamesQs.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string TabName = castedItem["Checklist_Master_Type"].ToString();
                        int CheckListId = Convert.ToInt32(castedItem["ChecklistType_Id"]);
                        dtInsert.Rows.Add(CheckListId, QuestionEnterValue, ProjectValue, btnProductTypeAbbrValue, true);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtInsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/InsertQuestion", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                Clear();
                                this.mainForm.BindCheckListQuesSetUpTab();
                                
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Sucessfully", "Success");

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
            if (btn_SaveQs.Text == "Edit" && validate() != false && MasterChklistIdValue != 0)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    DataTable dtupdate = new DataTable();
                    dtupdate.Columns.AddRange(new DataColumn[]
                    {  
                        new DataColumn ("Checklist_Id",typeof(int)),
                        new DataColumn("Ref_Checklist_Master_Type_Id", typeof(int)),
                        new DataColumn("Question", typeof(string)) ,
                        new DataColumn("Project_Type_Id", typeof(int)),
                        new DataColumn("Product_Type_Abbr",typeof(string)),
                        new DataColumn("Is_Active",typeof(bool))

                    });
                    foreach (object itemChecked in chk_TabNamesQs.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        int ChecklistType_IdValue = Convert.ToInt32(castedItem["ChecklistType_Id"].ToString());
                        dtupdate.Rows.Add( MasterChklistIdValue,ChecklistType_IdValue, QuestionEnterValue, ProjectValue, btnProductTypeAbbrValue, true);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtupdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/CheckListMaster/UpdateQuestionSetup", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show(" Updated Successfully", " Record Updated", MessageBoxButtons.OK);
                                Clear();

                                this.mainForm.BindCheckListQuesSetUpTab();

                                MasterChklistIdValue = 0;
                                btn_SaveQs.Text = "Save";
                                Oper_Type = "QuestionSetUp";
                                this.Close();
                                this.mainForm.Enabled = true;
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
            ddl_projectTypeQuesSetup.ItemIndex = 0;
            txtQuestionQs.Text = "";
            ddl_ProdductTypeAbbrQs.ItemIndex = 0;
            chk_TabNamesQs.DataSource = null;
            Oper_Type = "QuestionSetUp";
            btn_SaveQs.Text = "Save";
        }

        private void ddl_projectTypeQuesSetup_EditValueChanged(object sender, EventArgs e)
        {

            int ProjectId = Convert.ToInt32(ddl_projectTypeQuesSetup.EditValue);
            ddl_ProdductTypeAbbrQs.Properties.Columns.Clear();
            BindOrderTypeAbbrForQuesSetUp(ProjectId);
        }

        private void ddl_ProdductTypeAbbrQs_EditValueChanged(object sender, EventArgs e)
        {
            string ProductAbbr = ddl_ProdductTypeAbbrQs.Text.ToString();
            ddl_ProdductTypeAbbrQs.Properties.Columns.Clear();
            BindCheckListTabName(ProductAbbr);
        }
        public bool validate()
        {
            if (Convert.ToInt32(ddl_projectTypeQuesSetup.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (chk_TabNamesQs.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check Tab Names", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (txtQuestionQs.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter Question", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Convert.ToInt32(ddl_ProdductTypeAbbrQs.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select ProductType Abbr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btn_ClearQs_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void CheckList_Question_Setup_Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainForm.Enabled = true;
        }
    }
}
