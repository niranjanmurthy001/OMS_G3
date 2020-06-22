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
using System.Windows;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Sub_Product_Type_Entry : DevExpress.XtraEditors.XtraForm
    {
        string Operation_Type, SubproductType, _Operation, BtnName;
        int ProjectId, ProductId, SubproductTypeAbsId, ProjectId_;
        int userid, ID;
        private Sub_Product_Type_View Mainform = null;

        public Sub_Product_Type_Entry(string _OperationType, string operation, int _projectId, int _productId, string _subproductType, int _subproductTypeAbsId, string _btnName, int User_Id, int _ID, Form CallingForm)
        {
            InitializeComponent();
            Operation_Type = _OperationType;
            _Operation = operation;
            ProjectId = _projectId;
            ProductId = _productId;
            SubproductType = _subproductType;
            SubproductTypeAbsId = _subproductTypeAbsId;
            BtnName = _btnName;
            Mainform = CallingForm as Sub_Product_Type_View;
            userid = User_Id;
            ID = _ID;
        }

        private void Sub_Product_Type_Entry_Load(object sender, EventArgs e)
        {  // userid = this.Mainform._UserId; 
            btn_Save_Type.Text = "Save";
            ClearType();
            ClearAbs();
            if (Operation_Type == "Sub Product Type")
            {
                pannelControl_Type.Visible = true;
               panelControl_Abs.Visible = false;
                if (_Operation == "View")
                {
                    btn_Save_Type.Text = BtnName;
                    ddl_ProjectType_Type.EditValue = ProjectId;
                    ddl_Product_Type.EditValue = ProductId;
                    txt_Type.Text = SubproductType;
                    ddl_Type_Abs.EditValue = SubproductTypeAbsId;
                }
            }
           else if(Operation_Type == "Sub Product Type Abbreviation")
            {
                panelControl_Abs.Visible = true;
              //  pannelControl_Type.Visible = false;
              if(_Operation=="View")
                {
                    btn_Save_Abs.Text = BtnName;
                    ddl_ProjectType_Abs.EditValue = ProjectId;
                    ddl_Product_Type_Abs.EditValue = ProductId;
                    txt_Abs.Text = SubproductType;
                }
            }           
            BindProjectType();
            //BindProdctType(ProjectId);
            //BindTypeAbs(ProjectId);
        }
        public async void BindProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","GET_PROJECT_TYPE" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/SubProductType/BindProjectType", data);
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
                            if (Operation_Type == "Sub Product Type")
                            {
                                ddl_ProjectType_Type.Properties.DataSource = dt;
                                ddl_ProjectType_Type.Properties.DisplayMember = "Project_Type";
                                ddl_ProjectType_Type.Properties.ValueMember = "Project_Type_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                                ddl_ProjectType_Type.Properties.Columns.Add(col);
                            }
                            else if (Operation_Type == "Sub Product Type Abbreviation")
                            {
                                ddl_ProjectType_Abs.Properties.DataSource = dt;
                                ddl_ProjectType_Abs.Properties.DisplayMember = "Project_Type";
                                ddl_ProjectType_Abs.Properties.ValueMember = "Project_Type_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                                ddl_ProjectType_Abs.Properties.Columns.Add(col);
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
        private void ddl_ProjectType_Type_EditValueChanged(object sender, EventArgs e)
        {
            if (ddl_ProjectType_Type.ItemIndex >0)
            {
                ProjectId_ = Convert.ToInt32(ddl_ProjectType_Type.EditValue);
                ddl_Product_Type.Properties.Columns.Clear();
                ddl_Type_Abs.Properties.Columns.Clear();
                BindProdctType(ProjectId_);
                BindTypeAbs(ProjectId_);

            }
        }
        private async void BindProdctType(int ProjectId_)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"GET_PRODUCT_TYPE"},
                    {"@Project_Type_Id",ProjectId_ }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/SubProductType/BindProductType", data);
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
                            if (Operation_Type == "Sub Product Type")
                            {
                                if (_Operation == "View")
                                {
                                    ddl_Product_Type.Properties.DataSource = dt;
                                    ddl_Product_Type.Properties.DisplayMember = "Product_Type";
                                    ddl_Product_Type.Properties.ValueMember = "ProductType_Id";
                                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                                    ddl_Product_Type.Properties.Columns.Add(col);
                                    ddl_Product_Type.EditValue = ProductId;
                                }
                                ddl_Product_Type.Properties.DataSource = dt;
                                ddl_Product_Type.Properties.DisplayMember = "Product_Type";
                                ddl_Product_Type.Properties.ValueMember = "ProductType_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col1;
                                col1 = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                                ddl_Product_Type.Properties.Columns.Add(col1);
                            }
                            else if (Operation_Type == "Sub Product Type Abbreviation")
                            {
                                if (_Operation == "View")
                                {
                                    ddl_Product_Type_Abs.Properties.DataSource = dt;
                                    ddl_Product_Type_Abs.Properties.DisplayMember = "Product_Type";
                                    ddl_Product_Type_Abs.Properties.ValueMember = "ProductType_Id";
                                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                                    ddl_Product_Type_Abs.Properties.Columns.Add(col);
                                    ddl_Product_Type_Abs.EditValue = ProductId;
                                }
                                ddl_Product_Type_Abs.Properties.DataSource = dt;
                                ddl_Product_Type_Abs.Properties.DisplayMember = "Product_Type";
                                ddl_Product_Type_Abs.Properties.ValueMember = "ProductType_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col1;
                                col1 = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                                ddl_Product_Type_Abs.Properties.Columns.Add(col1);
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
        private async void BindTypeAbs(int ProjectId_)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"GET_TYPE_ABS"},
                    {"@Project_Type_Id",ProjectId_ }
                };
                ddl_Type_Abs.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/SubProductType/BindTypeAbs", data);
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
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (_Operation == "View")
                                {
                                    ddl_Type_Abs.Properties.DataSource = dt;
                                    ddl_Type_Abs.Properties.DisplayMember = "Order_Type_Abbreviation";
                                    ddl_Type_Abs.Properties.ValueMember = "OrderType_ABS_Id";
                                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                    ddl_Type_Abs.Properties.Columns.Add(col);
                                    ddl_Type_Abs.EditValue = SubproductTypeAbsId;
                                }
                                ddl_Type_Abs.Properties.DataSource = dt;
                                ddl_Type_Abs.Properties.DisplayMember = "Order_Type_Abbreviation";
                                ddl_Type_Abs.Properties.ValueMember = "OrderType_ABS_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col1;
                                col1 = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                ddl_Type_Abs.Properties.Columns.Add(col1);
                            }
                        }
                    }
                    else
                    {
                        ddl_Type_Abs.Properties.DataSource = null;
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
        public async void btn_Save_Type_Click(object sender, EventArgs e)
        {
            try
            {
                int _status = 1;
                //int userid = 1;
                string _abbrivation = ddl_Type_Abs.Text;
                DateTime _insertedby = DateTime.Now;
                if (btn_Save_Type.Text == "Save" && ValidateType() != false && (await CheckType()) != false)
                {                   
                        var dictinsert = new Dictionary<string, object>();
                        {
                            dictinsert.Add("@Trans", "INSERT_TYPE");
                            dictinsert.Add("@Order_Type", txt_Type.Text);
                            dictinsert.Add("@Order_Type_Abrivation", _abbrivation);
                            dictinsert.Add("@Project_Type_Id", ddl_ProjectType_Type.EditValue);
                            dictinsert.Add("@ProductType_Id", ddl_Product_Type.EditValue);
                            dictinsert.Add("@OrderType_ABS_Id", ddl_Type_Abs.EditValue);
                            dictinsert.Add("@Status", _status);
                            dictinsert.Add("@Inserted_By", userid);
                            dictinsert.Add("@Instered_Date", _insertedby);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictinsert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/InsertType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(" Inserted Successfully ");
                                    this.Mainform.BindGridType();
                                    ClearType();
                                    this.Close();
                                }
                            }
                        }
                    }
                
                    if (btn_Save_Type.Text == "Edit" && ValidateType() != false && (await CheckType()) != false)
                    {

                        var dictionaryedit = new Dictionary<string, object>();
                        {
                            dictionaryedit.Add("@Trans", "UPDATE_TYPE");
                            dictionaryedit.Add("@Order_Type_ID", ID);
                            dictionaryedit.Add("@Order_Type", txt_Type.Text);
                            dictionaryedit.Add("@Order_Type_Abrivation", _abbrivation);
                            dictionaryedit.Add("@Project_Type_Id", ddl_ProjectType_Type.EditValue);
                            dictionaryedit.Add("@ProductType_Id", ddl_Product_Type.EditValue);
                            dictionaryedit.Add("@OrderType_ABS_Id", ddl_Type_Abs.EditValue);
                            dictionaryedit.Add("@Status", _status);
                            dictionaryedit.Add("@Modified_By", userid);
                            dictionaryedit.Add("@Modified_Date", _insertedby);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryedit), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PutAsync(Base_Url.Url + "/SubProductType/UpdateType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Edited Successfully ");
                                    this.Mainform.BindGridType();
                                    ClearType();
                                    this.Close();
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


        private void btn_Clear_Type_Click(object sender, EventArgs e)
        {
            ClearType();
        }

        public async void btn_Save_Abs_Click(object sender, EventArgs e)
        {
            try
            {
                int _status = 1;

                DateTime _inserteddate = DateTime.Now;               
                    if (btn_Save_Abs.Text == "Save" && ValidateAbs() != false && (await CheckAbs()) != false)
                    {
                        var dictinsert = new Dictionary<string, object>();
                        {
                            dictinsert.Add("@Trans", "INSERT_ABS");
                            dictinsert.Add("@Order_Type_Abbreviation", txt_Abs.Text);
                            dictinsert.Add("@Project_Type_Id", ddl_ProjectType_Abs.EditValue);
                            dictinsert.Add("@ProductType_Id", ddl_Product_Type_Abs.EditValue);
                            dictinsert.Add("@Status", _status);
                            dictinsert.Add("@Inserted_by", userid);
                            dictinsert.Add("@Inserted_Date", _inserteddate);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictinsert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/InsertAbs", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(" Inserted Successfully ");
                                    this.Mainform.BindGridAbs();
                                    ClearAbs();
                                    this.Close();
                                }
                            }
                        }
                    }
                    if (btn_Save_Abs.Text == "Edit" && ValidateAbs() != false && (await CheckAbs()) != false)
                    {

                        var dictionaryedit = new Dictionary<string, object>();
                        {
                            dictionaryedit.Add("@Trans", "UPDATE_ABS");
                            dictionaryedit.Add("@OrderType_ABS_Id", ID);
                            dictionaryedit.Add("@Order_Type_Abbreviation", txt_Abs.Text);
                            dictionaryedit.Add("@Project_Type_Id", ddl_ProjectType_Abs.EditValue);
                            dictionaryedit.Add("@ProductType_Id", ddl_Product_Type_Abs.EditValue);
                            dictionaryedit.Add("@Status", _status);
                            dictionaryedit.Add("@Modified_by", userid);
                            dictionaryedit.Add("@Modified_Date", _inserteddate);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryedit), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PutAsync(Base_Url.Url + "/SubProductType/UpdateAbs", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Edited Successfully ");
                                    this.Mainform.BindGridAbs();
                                    ClearType();
                                    this.Close();
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


        private void ddl_ProjectType_Abs_EditValueChanged(object sender, EventArgs e)
        {
            if (Operation_Type == "Sub Product Type Abbreviation")
            {
                ProjectId_ = Convert.ToInt32(ddl_ProjectType_Abs.EditValue);
                ddl_Product_Type_Abs.Properties.Columns.Clear();
                BindProdctType(ProjectId_);
            }
        }

        private void btn_Clear_Abs_Click(object sender, EventArgs e)
        {
            ClearAbs();
        }

        private bool ValidateType()
        {
            if(ddl_ProjectType_Type.EditValue==null)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddl_ProjectType_Type.Focus();
                return false;
            }
            if (ddl_Product_Type.EditValue == null)
            {
                XtraMessageBox.Show("Please Select Product Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddl_Product_Type.Focus();
                return false;
            }
            if (txt_Type.Text =="")
            {
                XtraMessageBox.Show("Please Enter Sub Product Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_Type.Focus();
                return false;
            }
            if (ddl_Type_Abs.EditValue == null)
            {
                XtraMessageBox.Show("Please Select Sub Product Type Abs ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddl_Type_Abs.Focus();
                return false;
            }
            return true;
        }
        private bool ValidateAbs()
        {

            if (ddl_ProjectType_Abs.EditValue == null)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddl_ProjectType_Abs.Focus();
                return false;
            }
            if (ddl_Product_Type_Abs.EditValue == null)
            {
                XtraMessageBox.Show("Please Select Product Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ddl_Product_Type_Abs.Focus();
                return false;
            }
            if (txt_Abs.Text =="")
            {
                XtraMessageBox.Show("Please Enter Sub Product Type Abs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_Abs.Focus();
                return false;
            }
            return true;
        }
        private async Task<bool> CheckType()
        {
         DataTable dt = new DataTable();
            try
            {
                if (ValidateType()!=false)
                {
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "CHECK_TYPE" },
                    { "@Order_Type", txt_Type.Text.Trim()},
                    { "@Project_Type_Id",ddl_ProjectType_Type.EditValue},
                    { "@OrderType_ABS_Id",ddl_Type_Abs.EditValue } ,
                    {  "@ProductType_Id",ddl_Product_Type.EditValue }                                    
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/CheckType", data);
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
                                    XtraMessageBox.Show("Sub Product Type Already Exists", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
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
        private async Task<bool> CheckAbs()
        {
            DataTable dt = new DataTable();
            try
            {
                if (ValidateAbs() != false)
                {
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "CHECK_ABS" },
                    { "@Order_Type_Abbreviation", txt_Abs.Text.Trim()},
                    { "@Project_Type_Id", ddl_ProjectType_Abs.EditValue},
                    {  "@ProductType_Id",ddl_Product_Type_Abs.EditValue }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/CheckAbs", data);
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
                                    XtraMessageBox.Show("Sub Product Type Abs Already Exists", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return false;
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
        private void ClearType()
        {
            ddl_ProjectType_Type.ItemIndex = 0;
            ddl_ProjectType_Type.EditValue = null;
            ddl_Type_Abs.EditValue = null;
            ddl_Type_Abs.ItemIndex = 0;
            ddl_Product_Type.ItemIndex = 0;
            ddl_Product_Type.EditValue = null;
            txt_Type.Text = null;
        }
        private void ClearAbs()
        {
            ddl_ProjectType_Abs.ItemIndex = 0;
            ddl_ProjectType_Abs.EditValue = null;
            ddl_Product_Type_Abs.ItemIndex = 0;
            ddl_Product_Type_Abs.EditValue = null;
            txt_Abs.Text = null;
        }
      
      
    }
}