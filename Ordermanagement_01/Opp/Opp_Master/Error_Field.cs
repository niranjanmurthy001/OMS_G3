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
using System.Net.Http;
using Ordermanagement_01.Models;
using Newtonsoft.Json;
using System.Net;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Field : DevExpress.XtraEditors.XtraForm
    {
        string _btnname;
        int _Projectid;
        int _productid;
        string errortext;
        int checkederror;
        string Operation_Type;
        private Error_Settings Mainform = null;
        public Error_Field(string _operationType,string btnname,int _pro,int _prd,string _text,int _Cerror,Form Callingform)
        {
            InitializeComponent();
            _btnname = btnname;
            _Projectid = _pro;
            _productid = _prd;
            errortext = _text;
            checkederror = _Cerror;
            Operation_Type = _operationType;
            Mainform = Callingform as Error_Settings;


        }

        private void Error_Field_Load(object sender, EventArgs e)
            {
            BindErrordetails();
            BindProjectType();
            if (Operation_Type == "Error Field")
            {
                
            }
            else
            {
                
                btn_Save.Text = _btnname;
                ddl_ProjectType.EditValue = _Projectid;
                BindProdctType(_Projectid);
                ddl_ProductType.EditValue = _productid;
                txt_Errorfield.Text = errortext;
              
            }

        }

        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (btn_Save.Text == "Save" && Validates() != false)
            {
                try
                {
                    DataRowView r1 = checkedListBoxControl_Errortab.GetItem(checkedListBoxControl_Errortab.SelectedIndex) as DataRowView;
                    int Task = Convert.ToInt32(r1["Error_Type_Id"]);
                    DataTable dtmulti = new DataTable();
                    dtmulti.Columns.AddRange(new DataColumn[7]
                    {
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new  DataColumn("Error_Type_Id",typeof(string)),
                        new DataColumn("Error_description",typeof(string)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("Instered_Date",typeof(DateTime))
                    });
                    foreach (object itemChecked in checkedListBoxControl_Errortab.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Error_Type"].ToString();
                        int Error_description = Convert.ToInt32(castedItem["Error_Type_Id"]);
                        int _ProjectID = Convert.ToInt32(ddl_ProjectType.EditValue);
                        int _ProductType = Convert.ToInt32(ddl_ProductType.EditValue);
                        string _Error = txt_Errorfield.Text;
                        int _status = 1;
                        int _Insertedby = 1;
                        DateTime _inserdate = DateTime.Now;
                        dtmulti.Rows.Add(_ProjectID, _ProductType, Error_description, _Error, _status, _Insertedby, _inserdate);

                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTab/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error details are Submitted","Submit Record",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                // BindErrorGrid();
                                btn_Clear_Click(sender, e);
                                this.Mainform.BindErrorGrid();
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
                    DataRowView r1 = checkedListBoxControl_Errortab.GetItem(checkedListBoxControl_Errortab.SelectedIndex) as DataRowView;
                    int _Task = Convert.ToInt32(r1["Error_Type_Id"]);

                    DataTable _dtmulti1 = new DataTable();
                    _dtmulti1.Columns.AddRange(new DataColumn[7]
                    {
                       new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new  DataColumn("Error_Type_Id",typeof(string)),
                        new DataColumn("Error_description",typeof(string)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Modified_by",typeof(int)),
                        new DataColumn("Modified_Date",typeof(DateTime))
                    });
                    foreach (object itemChecked in checkedListBoxControl_Errortab.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Error_Type"].ToString();
                        int Error_description = Convert.ToInt32(castedItem["Error_Type_Id"]);
                        int _ProjectID = Convert.ToInt32(ddl_ProjectType.EditValue);
                        int _ProductType = Convert.ToInt32(ddl_ProductType.EditValue);
                        string _Error = txt_Errorfield.Text;
                        int _status = 1;
                        int _Modifiedby = 1;
                        DateTime _modifydate = DateTime.Now;
                        _dtmulti1.Rows.Add(_ProjectID, _ProductType, Error_description, _Error, _status, _Modifiedby, _modifydate);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(_dtmulti1), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTab/UpdateError", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error is Updated","Update Record",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                                // BindErrorGrid();
                                btn_Clear_Click(sender, e);
                                this.Mainform.BindErrorGrid();

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

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_ProductType.ItemIndex = 0;
            ddl_ProjectType.ItemIndex = 0;
            checkedListBoxControl_Errortab.UnCheckAll();
            checkedListBoxControl_Errortab.SelectedIndex = 0;
            txt_Errorfield.Text = "";
            btn_Save.Text = "Save";
        }

        private async void BindProdctType(int Project_Id)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"Select_Product_Type"},
                    {"@Project_Type_Id",Project_Id }
                };
                //ddlProductType.Properties.DataSource = null;
                ddl_ProductType.Properties.Columns.Clear();

                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindDetails", data);
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

                            ddl_ProductType.Properties.DataSource = dt;
                            ddl_ProductType.Properties.DisplayMember = "Product_Type";
                            ddl_ProductType.Properties.ValueMember = "ProductType_Id";
                            // ddlProductType.Properties.KeyMember = "Product_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                            ddl_ProductType.Properties.Columns.Add(col);
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
        private async void BindErrordetails()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_Error_Type"}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindErrorfield", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null & dt.Rows.Count > 0 && Operation_Type=="Error Field")
                            { 
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    checkedListBoxControl_Errortab.DataSource = dt;
                                    checkedListBoxControl_Errortab.DisplayMember = "Error_Type";
                                    checkedListBoxControl_Errortab.ValueMember = "Error_Type_Id";
                                    
                                }

                            }
                            else if(dt != null & dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    checkedListBoxControl_Errortab.DataSource = dt;
                                    checkedListBoxControl_Errortab.DisplayMember = "Error_Type";
                                    checkedListBoxControl_Errortab.ValueMember = "Error_Type_Id";
                                    checkedListBoxControl_Errortab.SelectedValue = checkederror;
                                    int _erroe = checkedListBoxControl_Errortab.SelectedIndex;
                                    checkedListBoxControl_Errortab.SetItemChecked(_erroe, true);
                                }
                            }

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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindDetails", data);
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
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        //private async void BindErrorGrid()
        //{
        //    try
        //    {

        //        //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //        var dictonary = new Dictionary<string, object>()
        //        {
        //            {"@Trans","SELECT_Error_description_grd" }


        //        };

        //        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
        //        using (var httpclient = new HttpClient())
        //        {
        //            var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindErrors", data);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    var result = await response.Content.ReadAsStringAsync();
        //                    DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
        //                    if (_dt.Rows.Count > 0)
        //                    {

        //                        Grd_ErrorDes.DataSource = _dt;

        //                    }
        //                    else
        //                    {
        //                        Grd_ErrorDes.DataSource = null;

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);

        //    }
        //}

        private bool Validates()
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Project_Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (Convert.ToInt32(ddl_ProductType.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Product_Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (checkedListBoxControl_Errortab.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Select Error Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txt_Errorfield.Text == "")
            {
                XtraMessageBox.Show("Error Field Must not be Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddl_ProjectType.ItemIndex > 0)
            {
                int ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
                BindProdctType(ProjectId);

            }
        }

       
    }
}