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
using System.Net;
using Ordermanagement_01.Models;
using System.Net.Http;
using DevExpress.XtraGrid.Views.Grid;

namespace Ordermanagement_01.Masters
{
    public partial class Error_Field : DevExpress.XtraEditors.XtraForm
    {
        int _pid;
        int _error_D_id;
        public Error_Field()
        {
            InitializeComponent();
        }

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddl_ProjectType.ItemIndex > 0)
            {
                int ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
                BindProdctType(ProjectId);
                
            }
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindErrors", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null & dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    checkedListBoxControl_Errortab.DataSource = dt;
                                    checkedListBoxControl_Errortab.DisplayMember = "Error_Type";
                                    checkedListBoxControl_Errortab.ValueMember = "Error_Type_Id";
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

        private void Error_Field_Load(object sender, EventArgs e)
        {
            BindErrordetails();
            BindProjectType();
            BindErrorGrid();
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
        private async void BindErrorGrid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_Error_description_grd" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindErrors", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                               
                                Grd_ErrorDes.DataSource = _dt;

                            }
                            else
                            {
                                Grd_ErrorDes.DataSource = null;

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

        private bool Validates()
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Project_Type");
                return false;
            }
            if (Convert.ToInt32(ddl_ProductType.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Product_Type");
                return false;
            }
            if (checkedListBoxControl_Errortab.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Select Error Type");
                return false;
            }
            if(txt_Errorfield.Text=="")
            {
                XtraMessageBox.Show("Error Field Must not be Empty");
                return false;
            }
            return true;
        }
        private async void btn_Save_Click(object sender, EventArgs e)
        {
            if (btn_Save.Text == "Save" && Validates() !=false)
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
                        dtmulti.Rows.Add( _ProjectID, _ProductType, Error_description, _Error, _status, _Insertedby, _inserdate);

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
                                XtraMessageBox.Show("Error details are Submitted");
                                BindErrorGrid();
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
                                XtraMessageBox.Show("Error is Updated");
                                BindErrorGrid();
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

        private async void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            
            if (e.Column.Caption == "Delete")
            {
                try
                {
                    GridView view = Grd_ErrorDes.MainView as GridView;
                    var index = view.GetDataRow(view.GetSelectedRows()[0]);
                    //_pid = Convert.ToInt32(index.ItemArray[5]);
                    _error_D_id = Convert.ToInt32(index.ItemArray[1]);

                    var dictonary = new Dictionary<string, object>()
                     {
                        {"@Trans","DELETE_Error_description" },
                        
                        {"@Error_description_Id",_error_D_id }
                        
                    };
                    var op = XtraMessageBox.Show("Do You Want to Delete the Error Description", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (op == DialogResult.Yes)
                    {
                        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                        using (var httpclient = new HttpClient())
                        {
                            var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    
                                    XtraMessageBox.Show("Deleted Successfully");
                                    BindErrorGrid();

                                }
                            }
                        }
                    }
                    else
                    {
                        BindErrorGrid();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
               
            }
            if(e.Column.Caption=="View")
            {
                GridView view = Grd_ErrorDes.MainView as GridView;
                var index = view.GetDataRow(view.GetSelectedRows()[0]);
                //e.Column.ColumnEdit.NullText = "Edit";
                btn_Save.Text = "Update";
                ddl_ProjectType.EditValue = index.ItemArray[7];
                //int Pro = Convert.ToInt32(ddl_ProjectType.EditValue);
                //BindProdctType(Pro);
                ddl_ProductType.EditValue= index.ItemArray[8];
                txt_Errorfield.Text = index.ItemArray[0].ToString();
                int _ET=Convert.ToInt32 (index.ItemArray[3]);
                checkedListBoxControl_Errortab.SelectedValue = _ET;
                int _erroe = checkedListBoxControl_Errortab.SelectedIndex;
                checkedListBoxControl_Errortab.SetItemChecked(_erroe, true);
            }
        }
    }
}