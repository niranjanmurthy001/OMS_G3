using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Project_Type_OrderStatus_Settings : XtraForm
    {
        int Projectvalue;
        int Productvalue;
        int OrderStatusvalue;
        int User_Id;
        int Role_Id;
        int OrderChk;
        int _OrderStatus;
        int Order_Id;
        DataTable dtload = new DataTable();
        DateTime date = DateTime.Now;
        DataTable _dt = new DataTable();
        public Project_Type_OrderStatus_Settings()
        {
            InitializeComponent();

        }

        private void OrderStatus_Load(object sender, EventArgs e)
        {
            BindOrderStatusGrid();
            BindProjectType();
            BindOrderStatus();


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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderStatus/BindProjectType", data);
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
                ddlProductType.Properties.Columns.Clear();

                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderStatus/BindProductType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataRow dr = dt.NewRow();
                                dr[1] = 0;
                                dr[0] = "Select";
                                dt.Rows.InsertAt(dr, 0);
                            }

                            ddlProductType.Properties.DataSource = dt;
                            ddlProductType.Properties.DisplayMember = "Product_Type";
                            ddlProductType.Properties.ValueMember = "ProductType_Id";
                            // ddlProductType.Properties.KeyMember = "Product_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Product_Type");
                            ddlProductType.Properties.Columns.Add(col);



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
        private async void BindOrderStatus()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>
                {
                    {"@Trans","Select_orderStatus"}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderStatus/BindOrderStatus", data);
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
                                    chkOrderStatus.DataSource = dt;
                                    chkOrderStatus.DisplayMember = "Progress_Status";
                                    chkOrderStatus.ValueMember = "Order_Progress_Id";
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

        private void ddlProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (ddlProjectType.ItemIndex > 0)
            {
                int ProjectId = Convert.ToInt32(ddlProjectType.EditValue);
                BindProdctType(ProjectId);
                BindOrderStatus();
            }
        }

        private void ddlProjectType_Validating(object sender, CancelEventArgs e)
        {
            if (ddlProjectType.EditValue == " ")
            {
                dxErrorProvider1.SetError(ddlProjectType, "Please Select Project Type");
            }

            else
            {
                dxErrorProvider1.SetError(ddlProjectType, "");
            }
        }


        private void chkOrderStatus_Validating(object sender, CancelEventArgs e)
        {
            if (chkOrderStatus.CheckedItems.Count == 0)
            {
                dxErrorProvider1.SetError(chkOrderStatus, "Please Select Type Of OrderStatus");
            }
            else
            {
                dxErrorProvider1.SetError(chkOrderStatus, "");
            }
        }

        private bool Validate()
        {
            if (Convert.ToInt32(ddlProjectType.EditValue) == 0)
            {
                XtraMessageBox.Show("Please Select ProjectType");
                return false;
            }
            if (Convert.ToInt32(ddlProductType.ItemIndex) == 0)
            {
                XtraMessageBox.Show("Please Select ProductType");
                return false;
            }
            if (chkOrderStatus.CheckedItems.Count == 0)
            {
                XtraMessageBox.Show("Please Select Type of OrderStatus");
                return false;
            }
            return true;


        }

        private async void btnadd_Click(object sender, EventArgs e)
        {
            Projectvalue = Convert.ToInt32(ddlProjectType.EditValue);
            Productvalue = Convert.ToInt32(ddlProductType.EditValue);
            int StatusId;

            if (btnadd.Text == "Submit" && Validate() != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    DataRowView row = chkOrderStatus.GetItem(chkOrderStatus.SelectedIndex) as DataRowView;
                    OrderStatusvalue = Convert.ToInt32(row["Order_Progress_Id"]);
                    DataTable dtorderStautus = new DataTable();
                    dtorderStautus.Columns.AddRange(new DataColumn[7]
                      {
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new DataColumn("User_Id",typeof(int)),
                        new DataColumn("Role_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("OrderStatus_Id",typeof(int)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("InsertedDate",typeof(DateTime))
                    });
                    foreach (object itemChecked in chkOrderStatus.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Progress_Status"].ToString();
                        int OrdStatus = Convert.ToInt32(castedItem["Order_Progress_Id"]);

                        dtorderStautus.Rows.Add(Productvalue, User_Id, Role_Id, Projectvalue, OrdStatus, 1, date);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtorderStautus), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/OrderStatus/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Order Status is Submitted");
                                BindOrderStatusGrid();
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
            else if (btnadd.Text == "Edit" && Validate() != false)
            {
                //    int projectId = Convert.ToInt32(ddlProjectType.EditValue);
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    DataRowView row = chkOrderStatus.GetItem(chkOrderStatus.SelectedIndex) as DataRowView;
                    OrderStatusvalue = Convert.ToInt32(row["Order_Progress_Id"]);
                    DataTable dtUpdate = new DataTable();
                    dtUpdate.Columns.AddRange(new DataColumn[7]
                      {
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new DataColumn("User_Id",typeof(int)),
                        new DataColumn("Role_Id",typeof(int)),
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("OrderStatus_Id",typeof(int)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("InsertedDate",typeof(DateTime))
                    });
                    foreach (object item in chkOrderStatus.CheckedItems)
                    {
                        DataRowView castedItem = item as DataRowView;
                        string sub = castedItem["Progress_Status"].ToString();
                        int OrdStatus = Convert.ToInt32(castedItem["Order_Progress_Id"]);
                        int producttype = Productvalue;
                        int projecttype = Projectvalue;

                        dtUpdate.Rows.Add(producttype, User_Id, Role_Id, projecttype, OrdStatus, 1, date);
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dtUpdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/OrderStatus/UpdateOrderStatus", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                //_dt = JsonConvert.DeserializeObject<DataTable>(result);
                                //if (_dt.Rows.Count > 0)
                                //{
                                //    grdOrderStatus.DataSource = _dt.DefaultView.ToTable(true, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName);
                                //    gridView1.BestFitColumns();
                                //}
                                //else
                                //{
                                //    grdOrderStatus.DataSource = null;
                                //}
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("OrderStatus Updated Successfully");
                                BindOrderStatusGrid();
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

        private async void BindOrderStatusGrid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Select_Master_Order_Status" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderStatus/GridOrderStatusDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                grdOrderStatus.DataSource = _dt.DefaultView.ToTable(true, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName);
                                gridView1.BestFitColumns();
                            }
                            else
                            {
                                grdOrderStatus.DataSource = null;
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
        private void Clear()
        {
            ddlProductType.ItemIndex = 0;
            chkOrderStatus.UnCheckAll();
            ddlProjectType.ItemIndex = 0;
            //chkOrderStatus.DataSource = null;
            btnadd.Text = "Submit";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                chkOrderStatus.UnCheckAll();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.Column.FieldName == "Product_Type")
                {
                    btnadd.Text = "Edit";

                    var row = _dt.AsEnumerable().Where(dr => dr.Field<string>("Product_Type") == e.CellValue.ToString());
                    var index = row.FirstOrDefault();
                    ddlProjectType.EditValue = index.ItemArray[5];
                    ddlProductType.EditValue = index.ItemArray[3];
                    int Project_Id = Convert.ToInt32(ddlProjectType.EditValue);
                    GetcheckedOrderStatusData(Project_Id);

                    int OrderChk = Convert.ToInt32(index.ItemArray[4]);
                    chkOrderStatus.SelectedValue = OrderChk;
                }
                int _task = chkOrderStatus.SelectedIndex;
                chkOrderStatus.SetItemChecked(_task, true);

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

        private async void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                int ProductType = Convert.ToInt32(ddlProductType.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                var dictionary = new Dictionary<string, object>
                {
                    { "@Trans", "Delete" },
                    { "@Product_Type_Id", ProductType}

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderStatus/Delete", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("OrderStatus Deleted Successfully");
                            BindOrderStatusGrid();
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
        private async void GetcheckedOrderStatusData(int projecttype_Id)
        {

            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GetStatusItems"},
                        {"@Project_Type_Id",projecttype_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/OrderStatus/Bindcheckitems", data);
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
                                    chkOrderStatus.SelectedValue = item_;
                                    int _task = chkOrderStatus.SelectedIndex;
                                    chkOrderStatus.SetItemChecked(_task, true);
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


        private void ddlProductType_Validating_1(object sender, CancelEventArgs e)
        {
            if (ddlProductType.EditValue == "")
            {
                dxErrorProvider1.SetError(ddlProductType, "please Select ProductType");
            }
            else
            {
                dxErrorProvider1.SetError(ddlProductType, "");
            }
        }


    }
}
