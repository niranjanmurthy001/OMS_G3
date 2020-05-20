using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Product_Type_Settings : XtraForm
    {
        private DataTable _dt;
        int ProjectValue;
       
        

        public object ProductValue { get; private set; }

        public Product_Type_Settings()
        {
            InitializeComponent();
        }

        private void Product_Type_Settings_Load(object sender, EventArgs e)
        {
            BindProjectType();
            BindProductTypeGrid();

        }

        private async void BindProductTypeGrid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Select_ProductType" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProdutTypeSettings/BindGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                grdProductType.DataSource = _dt.DefaultView.ToTable(true, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName);
                                gridView1.BestFitColumns();
                            }
                            else
                            {
                                grdProductType.DataSource = null;
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProdutTypeSettings/BindProjectType", data);
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
        public bool validate()
        {
            if (Convert.ToInt32(ddlProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select ProjectType");
                return false;
            }
            if (txtProductType.Text == "")
            {
                XtraMessageBox.Show("Please Enter ProductTypeValue");
                return false;
            }
            return true;
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            ProjectValue = Convert.ToInt32(ddlProjectType.EditValue);
            ProductValue = txtProductType.Text;
            if (btnSubmit.Text == "Submit" && Validate() != false)
            {
                try
                {
                    DataTable dtproduct = new DataTable();
                    dtproduct.Columns.AddRange(new DataColumn[2]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type",typeof(string))

                    });
                    dtproduct.Rows.Add(ProjectValue,ProductValue);
                    var data = new StringContent(JsonConvert.SerializeObject(dtproduct), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ProdutTypeSettings/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                            
                              


                                 SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("product Type is Submitted");
                                BindProductTypeGrid();
                                Clear();

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
            else if(btnSubmit.Text=="Edit" && Validate()!=false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictionary = new Dictionary<string, object>()
                    {
                        {"@Trans" ,"Update"},
                        {"@Project_Type_Id" ,ProjectValue},
                        {"@Product_Type",txtProductType.Text }
                    };
                   
                  
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/ProdutTypeSettings/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("ProductType Updated Successfully");
                                BindProductTypeGrid();
                                Clear();
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
        public void Clear()
        {
            ddlProjectType.ItemIndex = 0;
            txtProductType.Text = "";
            btnSubmit.Text = "Submit";
        }

      

        private void gridView1_RowCellClick_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.Column.FieldName == "Product_Type")
                {
                    btnSubmit.Text = "Edit";
                    var row = _dt.AsEnumerable().Where(dr => dr.Field<string>("Product_Type") == e.CellValue.ToString());
                    var index = row.FirstOrDefault();
                    ddlProjectType.EditValue = index.ItemArray[2];
                    txtProductType.Text = index.ItemArray[0].ToString();

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
         
            try
            {
                int projectId = Convert.ToInt32(ddlProjectType.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                var dictionary = new Dictionary<string, object>
                {
                    { "@Trans", "Delete" },
                    
                    { "@Product_Type", txtProductType.Text}

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProdutTypeSettings/Delete", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("productType Deleted Successfully");
                            BindProductTypeGrid();
                            Clear();
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

