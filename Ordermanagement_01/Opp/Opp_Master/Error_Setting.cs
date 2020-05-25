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
using DevExpress.XtraGrid.Views.Grid;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Setting : DevExpress.XtraEditors.XtraForm
    {
        int _ProjectType;
        int _ProductType;
        string _Errortype;
        int User_Id;
        int Role_Id;
        int Error_Id = 0;
        DateTime date = DateTime.Now;
        DataTable _dtError = new DataTable();
        public Error_Setting(int user_id, int roleid)
        {
            User_Id = user_id;
            Role_Id = roleid;
            InitializeComponent();
        }

        private void Error_Setting_Load(object sender, EventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1_Type;
            Bind_ProjectType();
            Bind_ProductType();
            BindErrorDetails();

        }



        private void tileItem1_ErrorType_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1_Type;
        }

        private void Tile_Item_ErrorTab_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2_Tab;
        }

        private void Tile_Item_ErrorField_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage3_Field;
        }

        private async void Bind_ProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Bind_Project_Type" }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary),Encoding.UTF8,"Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url+ "/ErrorTypeSettings/BindProjectType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            lookUpEdit1_ProjectType.Properties.DataSource = dt;
                            lookUpEdit1_ProjectType.Properties.DisplayMember = "Project_Type";
                            lookUpEdit1_ProjectType.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            lookUpEdit1_ProjectType.Properties.Columns.Add(col);

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

        private async void Bind_ProductType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_bind = new Dictionary<string, object>();
                {
                    dict_bind.Add("@Trans", "Bind_Product_Type");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict_bind), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/BindProductType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    checkedListBox_ProductType.DataSource = dt;
                                    checkedListBox_ProductType.DisplayMember = "Product_Type";
                                    checkedListBox_ProductType.ValueMember = "ProductType_Id";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }


        private async void BindErrorDetails()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient()) 
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/BindErrorDetails", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dtError = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dtError.Rows.Count > 0)
                            {
                                grd_Error_Type.DataSource = _dtError;
                                // _Inserted_By = Convert.ToInt32(_dtError.Rows[0]["Inserted_By"].ToString());
                                // _Inserted_Date = Convert.ToDateTime(_dtError.Rows[0]["Inserted_Date"].ToString());

                            }
                            else
                            {
                                grd_Error_Type.DataSource = null;
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

        private void Tile_Error_Type_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1_Type;
        }

        private void Tile_Error_Tab_ItemClick(object sender, TileItemEventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Master.ErrorTabSetting _Error_tab = new ErrorTabSetting();
            _Error_tab.Show();
        }

        private void Tile_Error_Field_ItemClick(object sender, TileItemEventArgs e)
        {
            Ordermanagement_01.Masters.Error_Field _errorfield = new Error_Field();
            _errorfield.Show();
        }
        private void Clear()
        {
            lookUpEdit1_ProjectType.ItemIndex = 0;
            lookUpEdit1_ProjectType.EditValue = null;
            checkedListBox_ProductType.UnCheckAll();
            checkedListBox_ProductType.SelectedIndex = 0;
            btn_SubmitType.Text = "Submit";
            txt_ErrorType.Text = "";
        }

        private bool Validate()
        {
            if (checkedListBox_ProductType.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select Product Type");
                return false;
            }
            if (lookUpEdit1_ProjectType.EditValue == null)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select Project Type");
                return false;
            }
            if (txt_ErrorType.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Error Type");
                return false;
            }

            return true;

        }
        private async void btn_SubmitType_Click_1(object sender, EventArgs e)
        {

            _Errortype = txt_ErrorType.Text;
            _ProjectType = Convert.ToInt32(lookUpEdit1_ProjectType.EditValue);
            int StatusId;
            if (btn_SubmitType.Text == "Submit" && Validate() != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    DataRowView row = checkedListBox_ProductType.GetItem(checkedListBox_ProductType.SelectedIndex) as DataRowView;
                    _ProductType = Convert.ToInt32(row["ProductType_Id"]);
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
                    foreach (object itemChecked in checkedListBox_ProductType.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Product_Type"].ToString();
                        int productid = Convert.ToInt32(castedItem["ProductType_Id"]);
                        int _Statsu = 1;
                        User_Id = 1;
                        DateTime _Insertdate = DateTime.Now;
                        _Errortype = txt_ErrorType.Text;
                        int _ProjectId = Convert.ToInt32(lookUpEdit1_ProjectType.EditValue);
                        dtproducttype.Rows.Add(_ProjectId, productid, _Errortype, _Statsu, User_Id, _Insertdate);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtproducttype), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/InsertType", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error Type is Submited");
                                BindErrorDetails();
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
            else if (btn_SubmitType.Text == "Edit" && Validate() != false)
            {
                //    int projectId = Convert.ToInt32(ddlProjectType.EditValue);
                try
                {

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    DataRowView r1 = checkedListBox_ProductType.GetItem(checkedListBox_ProductType.SelectedIndex) as DataRowView;
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
                    foreach (object itemChecked in checkedListBox_ProductType.CheckedItems)
                    {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Product_Type"].ToString();
                        int productid = Convert.ToInt32(castedItem["ProductType_Id"]);
                        int _Statsu = 1;
                        User_Id = 1;
                        DateTime _Insertdate = DateTime.Now;
                        _Errortype = txt_ErrorType.Text;
                        int _ProjectId = Convert.ToInt32(lookUpEdit1_ProjectType.EditValue);
                        dt_error_update.Rows.Add(_ProjectId, productid, _Errortype, _Statsu, User_Id, _Insertdate);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dt_error_update), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/UpdateErrorType", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error Type edited successfully");
                                BindErrorDetails();
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
        }



        private void repositoryItemHyperLinkEdit3_Click(object sender, EventArgs e)
        {

            //  checkedListBox_ProductType.Sele
            GridView view = grd_Error_Type.MainView as GridView;
            var index = view.GetDataRow(view.GetSelectedRows()[0]);
            btn_SubmitType.Text = "Edit";
            lookUpEdit1_ProjectType.EditValue = index.ItemArray[1];

            txt_ErrorType.Text = index.ItemArray[3].ToString();

            int _ET = Convert.ToInt32(index.ItemArray[2]);
            checkedListBox_ProductType.SelectedValue = _ET;
            int _erroe = checkedListBox_ProductType.SelectedIndex;
            checkedListBox_ProductType.SetItemChecked(_erroe, true);
        }

        private async void repositoryItemHyperLinkEdit4_Click(object sender, EventArgs e)
        {

            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    Error_Id = int.Parse(row["New_Error_Type_Id"].ToString());
                    var dictionary = new Dictionary<string, object>()
                {
                   { "@Trans", "DELETE" },
                    { "@New_Error_Type_Id", Error_Id }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                BindErrorDetails();
                                Clear();

                            }
                        }
                        else
                        {
                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("Please Select Client To Delete");
                        }
                    }

                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    throw ex;
                }
            }
            else if (show == DialogResult.No)
            {
                this.Close();
            }

        }

        private void btn_ClearType_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}