﻿using System;
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
using System.Net;
using Ordermanagement_01.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Settingspanels : DevExpress.XtraEditors.XtraForm
    {
        int ProjectValue;
        int Productvalue, User_Id, Error_Type_Id;
        string ErrorTypeTxt;
        int InsertedByvalue;
        DateTime InsertedDatevalue;
        private DataTable _dt;
        string Operation_Type = "";
        int ProjectId;
        public Error_Settingspanels()
        {
            InitializeComponent();
        }

        private void Error_Settings_Load(object sender, EventArgs e)
        {
            BindProjectType();
           // Bind_Error_Tab_Grid();
            BindProdctType(ProjectId);
        }
       
        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            
            if (Operation_Type == "Error Tab")
            {
                Error_label.Text = "Error Tab";
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
                                    // Bind_Error_Tab_Grid();
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

                            dtupdate.Rows.Add(ProjectValue, Productval, ErrorTypeTxt, InsertedByvalue, InsertedDatevalue, "True", User_Id, DateTime.Now);
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
                                    //Bind_Error_Tab_Grid();
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
            else if(Operation_Type=="Error Type")
            {
                Error_label.Text = "Error Type";
                string Errortype = txtErrorTab.Text;
                
                if (btnSubmit.Text == "Submit" && Validate() != false)
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        DataRowView row = chkProductType.GetItem(chkProductType.SelectedIndex) as DataRowView;
                        int _ProductType = Convert.ToInt32(row["ProductType_Id"]);
                        DataTable dtproducttype = new DataTable();
                        dtproducttype.Columns.AddRange(new DataColumn[6]
                          {

                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new DataColumn("New_Error_Type",typeof(string)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("Instered_Date",typeof(DateTime))
                        });
                        foreach (object itemChecked in chkProductType.CheckedItems)
                        {
                            DataRowView castedItem = itemChecked as DataRowView;
                            string sub = castedItem["Product_Type"].ToString();
                            int productid = Convert.ToInt32(castedItem["ProductType_Id"]);
                            int _Statsu = 1;
                            User_Id = 1;
                            DateTime _Insertdate = DateTime.Now;
                            Errortype = txtErrorTab.Text;
                            int _ProjectId = Convert.ToInt32(ddlProjectType.EditValue);
                            dtproducttype.Rows.Add(_ProjectId, productid, Errortype, _Statsu, User_Id, _Insertdate);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dtproducttype), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/InsertType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Error Type is Submited");
                                    //BindErrorDetails();
                                    clear();
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
                else if (btnSubmit.Text == "Edit" && Validate() != false)
                {
                    //    int projectId = Convert.ToInt32(ddlProjectType.EditValue);
                    try
                    {

                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        DataRowView r1 = chkProductType.GetItem(chkProductType.SelectedIndex) as DataRowView;
                        int _Product_Id = Convert.ToInt32(r1["ProductType_Id"]);

                        DataTable dt_error_update = new DataTable();
                        dt_error_update.Columns.AddRange(new DataColumn[6]
                          {
                         new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new DataColumn("New_Error_Type",typeof(string)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Modified_By",typeof(int)),
                        new DataColumn("Modified_Date",typeof(DateTime))
                        });
                        foreach (object itemChecked in chkProductType.CheckedItems)
                        {
                            DataRowView castedItem = itemChecked as DataRowView;
                            string sub = castedItem["Product_Type"].ToString();
                            int productid = Convert.ToInt32(castedItem["ProductType_Id"]);
                            int _Statsu = 1;
                            User_Id = 1;
                            DateTime _Insertdate = DateTime.Now;
                           string _Errortype = txtErrorTab.Text;
                            int _ProjectId = Convert.ToInt32(ddlProjectType.EditValue);
                            dt_error_update.Rows.Add(_ProjectId, productid, _Errortype, _Statsu, User_Id, _Insertdate);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dt_error_update), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/UpdateErrorType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Error Type edited successfully");
                                    //BindErrorDetails();
                                    clear();
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
        private void clear()
        {
            ddlProjectType.ItemIndex = 0;
            txtErrorTab.Text = "";
            chkProductType.UnCheckAll();
            chkProductType.DataSource = null;
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

        private void ddlProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddlProjectType.ItemIndex > 0)
            {
                ProjectId = Convert.ToInt32(ddlProjectType.EditValue);
                BindProdctType(ProjectId);

            }
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
        //private async void Bind_Error_Tab_Grid()
        //{
        //    try
        //    {

        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //        var dictonary = new Dictionary<string, object>()
        //        {
        //            {"@Trans","BindErrorDetails" }


        //        };

        //        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
        //        using (var httpclient = new HttpClient())
        //        {
        //            var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTabSettings/GridErrorTabDetails", data);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    var result = await response.Content.ReadAsStringAsync();
        //                    _dt = JsonConvert.DeserializeObject<DataTable>(result);
        //                    if (_dt.Rows.Count > 0)
        //                    {
        //                        grdErrorTab.DataSource = _dt.DefaultView.ToTable(true, _dt.Columns[3].ColumnName, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName, _dt.Columns[2].ColumnName, _dt.Columns[5].ColumnName, _dt.Columns[6].ColumnName);
        //                        gridView1.BestFitColumns();
        //                    }
        //                    else
        //                    {
        //                        grdErrorTab.DataSource = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex;
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);

        //    }
        //}
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
    }
}