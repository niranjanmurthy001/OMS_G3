using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class ChekList_Master_Entry : XtraForm
    {
        string Oper_Type;
        string btn_name;
        int ProjectType_IdValue;
        int ChklistIdValue;
        int ProdTypeAbbrValue;
        string chkListTypeVaue;
        private CheckList_Master_View mainform = null;

        public ChekList_Master_Entry(string OperType, string BtnName, int ProjectId, int ChkListId, int ProductTyeAbbr, string chkListType, Form CallingFrom)
        {
            InitializeComponent();
            mainform = CallingFrom as CheckList_Master_View;
            this.Oper_Type = OperType;
            this.btn_name = BtnName;
            this.ProjectType_IdValue = ProjectId;
            this.chkListTypeVaue = chkListType;
            this.ChklistIdValue = ChkListId;
            this.ProdTypeAbbrValue = ProductTyeAbbr;
        }

        private void ChekList_Master_Entry_Load(object sender, EventArgs e)
        {
            BindProjectType();
            if (Oper_Type == "Update")
            {
                ddl_ProjectType.EditValue = ProjectType_IdValue;
                BindOrderTypeAbs(ProjectType_IdValue);
                txtTabName.Text = chkListTypeVaue;
                btn_Save.Text = btn_name;
            }
        }
        public async void BindProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
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
                XtraMessageBox.Show("Something Went Wrong ! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private async void BindOrderTypeAbs(int ProjectId_)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_ORDER_ABS"},
                    {"@Project_Type_Id",ProjectId_ }
                };

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
                            if (dt != null & dt.Rows.Count > 0 && Oper_Type == "CheckListMaster")
                            {
                                chk_ProductType_Abbr.DataSource = dt;
                                chk_ProductType_Abbr.DisplayMember = "Order_Type_Abbreviation";
                                chk_ProductType_Abbr.ValueMember = "OrderType_ABS_Id";


                            }
                            else if (dt != null && dt.Rows.Count > 0)
                            {

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    chk_ProductType_Abbr.DataSource = dt;
                                    chk_ProductType_Abbr.DisplayMember = "Order_Type_Abbreviation";
                                    chk_ProductType_Abbr.ValueMember = "OrderType_ABS_Id";

                                    chk_ProductType_Abbr.SelectedValue = ProdTypeAbbrValue;
                                    int ProdtValue = chk_ProductType_Abbr.SelectedIndex;
                                    chk_ProductType_Abbr.SetItemChecked(ProdtValue, true);
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

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            int ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);

            BindOrderTypeAbs(ProjectId);
        }
        public bool validate()
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (chk_ProductType_Abbr.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check ProductType Abbr", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtTabName.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter CheckList Tab Name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            string TabName = txtTabName.Text;
            int ProjectValue = Convert.ToInt32(ddl_ProjectType.EditValue);
            if (btn_Save.Text == "Save" && validate() != false && (await CheckCheckListType()) != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    DataTable dtInsert = new DataTable();
                    dtInsert.Columns.AddRange(new DataColumn[]
                      {

                        new DataColumn("Checklist_Master_Type",typeof(string)),
                        new DataColumn("Project_Type_Id",typeof(int)) ,
                        new DataColumn("Product_Type_Abbr",typeof(string)),
                        new DataColumn("Is_Active",typeof(bool))

                    });
                    foreach (object itemChecked in chk_ProductType_Abbr.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string abbr = castedItem["Order_Type_Abbreviation"].ToString();
                        dtInsert.Rows.Add(TabName, ProjectValue, abbr, true);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtInsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();


                                Clear();
                                this.mainform.BindCheckListTypeMaster();
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
            else if (btn_Save.Text == "Edit" && validate() != false && ChklistIdValue != 0 && (await CheckCheckListType())!=false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    DataTable dtupdate = new DataTable();
                    dtupdate.Columns.AddRange(new DataColumn[]
                    {
                       new DataColumn("ChecklistType_Id",typeof(int)),
                       new DataColumn("Checklist_Master_Type",typeof(string)),
                        new DataColumn("Project_Type_Id",typeof(int)) ,
                        new DataColumn("Product_Type_Abbr",typeof(string)),
                        new DataColumn("Is_Active",typeof(bool))

                    });
                    foreach (object itemChecked in chk_ProductType_Abbr.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string Prodabbr = castedItem["Order_Type_Abbreviation"].ToString();
                        dtupdate.Rows.Add(ChklistIdValue, TabName, ProjectValue, Prodabbr, true);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtupdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/CheckListMaster/UpdateCheckListMaster", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show(" Updated Successfully", "Record Updated ", MessageBoxButtons.OK);
                                Clear();

                                this.mainform.BindCheckListTypeMaster();

                                ChklistIdValue = 0;
                                btn_Save.Text = "Save";
                                Oper_Type = "CheckListMaster";
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
            ddl_ProjectType.ItemIndex = 0;
            txtTabName.Text = "";
            chk_ProductType_Abbr.DataSource = null;
            Oper_Type = "CheckListMaster";
            btn_Save.Text = "Save";
        }
        private async Task<bool> CheckCheckListType()
        {
            DataTable dt = new DataTable();
            try
            {
                if (validate() != false)
                {
                    foreach (object itemChecked in chk_ProductType_Abbr.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string abbr = castedItem["Order_Type_Abbreviation"].ToString();
                     
                    
                       var dictionary = new Dictionary<string, object>()
                     {
                        { "@Trans", "CheckCheckListType" },
                        { "@Project_Type_Id",ddl_ProjectType.EditValue },
                         { "@Product_Type_Abbr",abbr},
                        { "@Checklist_Master_Type",txtTabName.Text } ,
                  
                     };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/Check", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                    int count = Convert.ToInt32(dt1.Rows[0]["count"].ToString());
                                    if (count > 0)
                                    {
                                        SplashScreenManager.CloseForm(false);
                                        XtraMessageBox.Show("CheckList Type Already Exists", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void ChekList_Master_Entry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.mainform.Enabled = true;
        }
    }

}
