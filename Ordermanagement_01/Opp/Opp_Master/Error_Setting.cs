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
        public Error_Setting(int user_id,int roleid)
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

        private async void  Bind_ProjectType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Bind_Project_Type" }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url +"/ErrorSetting/BindProjectType", data);
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
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
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
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/BindProductType", data);
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
            catch(Exception ex)
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
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/BindErrorDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dtError = JsonConvert.DeserializeObject<DataTable>(result);                          
                            if (_dtError.Rows.Count > 0)
                            {                                                 
                                grd_Error_Type.DataSource = _dtError.DefaultView.ToTable(true, _dtError.Columns[0].ColumnName);
                               // _Inserted_By = Convert.ToInt32(_dtError.Rows[0]["Inserted_By"].ToString());
                               // _Inserted_Date = Convert.ToDateTime(_dtError.Rows[0]["Inserted_Date"].ToString());
                                gridView1.BestFitColumns();
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

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

  

        private void Tile_Error_Type_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage1_Type;
        }

        private void Tile_Error_Tab_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage2_Tab;
        }

        private void Tile_Error_Field_ItemClick(object sender, TileItemEventArgs e)
        {
            navigationFrame1.SelectedPage = navigationPage3_Field;
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

                        new DataColumn("Project_Type",typeof(string)),
                        new DataColumn("Product_Type",typeof(string)),
                        new DataColumn("New_Error_Type",typeof(string)),
                        new DataColumn("Status",typeof(bool)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("InsertedDate",typeof(DateTime))
            });
            foreach (object itemChecked in checkedListBox_ProductType.CheckedItems)
            {
                DataRowView castedItem = itemChecked as DataRowView;
                string sub = castedItem["Product_Type"].ToString();
                int productid = Convert.ToInt32(castedItem["ProductType_Id"]);

                dtproducttype.Rows.Add(_ProjectType, productid, _Errortype, true, User_Id, date);
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
                        XtraMessageBox.Show("Error Type is Submitted");
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
            DataRowView row = checkedListBox_ProductType.GetItem(checkedListBox_ProductType.SelectedIndex) as DataRowView;
            _ProductType = Convert.ToInt32(row["ProductType_Id"]);
            DataTable dtproducttypeupdate = new DataTable();
            dtproducttypeupdate.Columns.AddRange(new DataColumn[6]
              {
                        new DataColumn("Project_Type",typeof(string)),
                        new DataColumn("Product_Type",typeof(string)),
                        new DataColumn("New_Error_Type",typeof(string)),
                        new DataColumn("Status",typeof(bool)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("InsertedDate",typeof(DateTime))
            });
            foreach (object itemChecked in checkedListBox_ProductType.CheckedItems)
            {
                        DataRowView castedItem = itemChecked as DataRowView;
                        string sub = castedItem["Product_Type"].ToString();
                        int productid = Convert.ToInt32(castedItem["ProductType_Id"]);

                        dtproducttypeupdate.Rows.Add(_ProjectType, productid, _Errortype, true, User_Id, date);
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dtproducttypeupdate), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/UpdateType", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error Type is Submitted");
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

        private void btn_Clear_Type_Click_1(object sender, EventArgs e)
        {
            Clear();

        }
        private void Clear()
        {
            lookUpEdit1_ProjectType.ItemIndex = 0;
            checkedListBox_ProductType.UnCheckAll();
            checkedListBox_ProductType.SelectedIndex = 0;
            btn_SubmitType.Text = "Submit";
            txt_ErrorType.Text = "";
        }

        private async void repositoryItemHyperLinkEdit3_Click(object sender, EventArgs e)
        {           
          try
            {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            Error_Id = int.Parse(row["New_Error_Type_Id"].ToString());
            btn_SubmitType.Text = "Edit";
            
            checkedListBox_ProducType.UnCheckAll();                 
            var dictionary = new Dictionary<string, object>
                    {
                        {"@Trans","View" },
                        {"@New_Error_Type_Id",Error_Id}

                    };
            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/ViewError", data);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lookUpEdit1_ProjectType.EditValue = dt.Rows[0]["From_Email_Id"];                              
                            //checkedListBox_ProducType.SelectedValue = ClientID;
                            //_Client = checkedListBox_ProducType.SelectedIndex;
                            //checkedListBox_ProducType.SetItemChecked(_Client, true);
                            txt_ErrorType.Text = dt.Rows[0]["New_Error_Type"].ToString();
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
                    { "@U_Id", Error_Id }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorSetting/Delete", data);
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
    
    }
}