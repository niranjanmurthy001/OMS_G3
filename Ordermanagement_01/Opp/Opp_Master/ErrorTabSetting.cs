using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class ErrorTabSetting : XtraForm
    {

        int ProjectValue;
        int Productvalue, User_Id, Error_Type_Id;
        string ErrorTypeTxt;
        int InsertedByvalue;
        DateTime InsertedDatevalue;
        private DataTable _dt;
        int ProjectId;
        public ErrorTabSetting()
        {
            InitializeComponent();
        }

        private void ErrorTabSetting_Load(object sender, EventArgs e)
        {
            BindProjectType();
            Bind_Error_Tab_Grid();
            BindProdctType(ProjectId);
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTabSettings/BindProjectType", data);
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
        private async void Bind_Error_Tab_Grid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BindErrorDetails" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTabSettings/GridErrorTabDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                grdErrorTab.DataSource = _dt.DefaultView.ToTable(true,_dt.Columns[3].ColumnName, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName, _dt.Columns[2].ColumnName,_dt.Columns[5].ColumnName,_dt.Columns[6].ColumnName);
                                gridView1.BestFitColumns();
                            }
                            else
                            {
                                grdErrorTab.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

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
                chkProductType.UnCheckAll();

                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTabSettings/BindProductType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                chkProductType.DataSource = dt;
                                chkProductType.DisplayMember = "Product_Type";
                                chkProductType.ValueMember = "ProductType_Id";
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

        private void ddlProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddlProjectType.ItemIndex > 0)
            {
                ProjectId = Convert.ToInt32(ddlProjectType.EditValue);
                BindProdctType(ProjectId);

            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorTypeTxt = txtErrorTab.Text;
            ProjectValue = Convert.ToInt32(ddlProjectType.EditValue);
            Error_Type_Id = 0;
            if (btnSubmit.Text == "Submit" && validate() != false)
            {
                try
                {
                    DataTable dtinsert = new DataTable();
                    dtinsert.Columns.AddRange(new DataColumn[6]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type_Id",typeof(int)),
                     new DataColumn("Error_Type",typeof(string)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Instered_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                    });
                    foreach (object itemchecked in chkProductType.CheckedItems)
                    {
                        DataRowView CastedItems = itemchecked as DataRowView;
                        Productvalue = Convert.ToInt32(CastedItems["ProductType_Id"]);
                    }
                    dtinsert.Rows.Add(ProjectValue, Productvalue, ErrorTypeTxt, User_Id, DateTime.Now, "True");
                    var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTabSettings/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                               DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                InsertedByvalue = Convert.ToInt32(dt.Rows[0]["Inserted_By"]);

                              InsertedDatevalue = Convert.ToDateTime(dt.Rows[0]["Instered_Date"]);

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error Type is Submitted Sucessfully");
                                Bind_Error_Tab_Grid();
                                clear();
                                Error_Type_Id = 0;

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
            else if (btnSubmit.Text == "Edit" && validate() != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    DataTable dtupdate = new DataTable();
                    dtupdate.Columns.AddRange(new DataColumn[8]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type_Id",typeof(int)),
                     new DataColumn("Error_Type",typeof(string)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Instered_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Modified_By",typeof(int)),

                     new DataColumn("Modified_Date",typeof(DateTime))
                    });
                    foreach (object item in chkProductType.CheckedItems)
                    {
                        DataRowView castedItem = item as DataRowView;

                        int Productval = Convert.ToInt32(castedItem["productType_Id"]);

                        int projecttype = ProjectValue;

                        dtupdate.Rows.Add(ProjectValue, Productval, ErrorTypeTxt,InsertedByvalue, InsertedDatevalue, "True",User_Id,DateTime.Now);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtupdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/ErrorTabSettings/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("ErrorTab Updated Successfully");
                                Bind_Error_Tab_Grid();
                                clear();
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


        }

        private void linkEdit_Click(object sender, EventArgs e)
        {
            btnSubmit.Text = "Edit";
            GridView view = grdErrorTab.MainView as GridView;
            var index = view.GetDataRow(view.GetSelectedRows()[0]);


            ddlProjectType.EditValue = index.ItemArray[4];

            int Project_Id = Convert.ToInt32(ddlProjectType.EditValue);
            GetcheckedProductData(Project_Id);

            txtErrorTab.Text = index.ItemArray[3].ToString();
            int ProductChk = Convert.ToInt32(index.ItemArray[5]);

            //chkProductType.SelectedValue = ProductChk;
            //int _task = chkProductType.SelectedIndex;
            //chkProductType.SetItemChecked(_task, true);
        }



        private async void linkDelete_Click(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {


                try
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    int ID = int.Parse(row["Error_Type_Id"].ToString());
                    var dictionarydelete = new Dictionary<string, object>();
                    {
                        dictionarydelete.Add("@Trans", "DELETE_Error_Type");
                        dictionarydelete.Add("@Error_Type_Id", ID);
                        dictionarydelete.Add("@Modified_By", User_Id);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictionarydelete), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTabSettings/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                Bind_Error_Tab_Grid();
                                clear();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if (show == DialogResult.No)
            {
                this.Close();
            }
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            clear();
        }

        public bool validate()
        {
            if (Convert.ToInt32(ddlProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Plese Select ProjectType");
                return false;
            }
            if (chkProductType.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please select ProductType");
                return false;
            }
            if (txtErrorTab.Text == "")
            {
                XtraMessageBox.Show("Please Enter ErrorTab");
                return false;
            }
            return true;
        }
        private void clear()
        {
            ddlProjectType.ItemIndex = 0;
            txtErrorTab.Text = "";
            chkProductType.UnCheckAll();
            chkProductType.DataSource = null;
        }
        private async void GetcheckedProductData(int projecttype_Id)
        {

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "CheckedProductItems"},
                        {"@Project_Type_Id",projecttype_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTabSettings/Bindcheckitems", data);
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
                                    chkProductType.SelectedValue = item_;
                                    int _task = chkProductType.SelectedIndex;
                                    chkProductType.SetItemChecked(_task, true);
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
