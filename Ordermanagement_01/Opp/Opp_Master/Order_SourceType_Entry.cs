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
    public partial class Order_SourceType_Entry : DevExpress.XtraEditors.XtraForm
    {
        private int Project_Id;
        int userid;
        string SourceTypeTxt;
        int ProjectValue;
        int Productvalue;      
        string _BtnName;
        int _ProjectId;
        string _SourceType;
        int _ProductId;
        string _Operaion_Id;
        int User_Id;
        private Order_SourceType_View Mainform = null;


        public Order_SourceType_Entry(string _Oid,int ProjId, int ProdId, string SrcType, string btnname,int User_Id, Form CallingForm)
        {
            InitializeComponent();
            _ProjectId = ProjId;
            _ProductId = ProdId;
            _SourceType = SrcType;
            _BtnName = btnname;
            _Operaion_Id = _Oid;
            userid = User_Id;
            Mainform = CallingForm as Order_SourceType_View;                      
        }

        private void Order_SourceType_Entry_Load(object sender, EventArgs e)
        {
            Clear();
            BindProjectType();
            BindProdctType(_ProjectId);
            if (_Operaion_Id == "View")
            {
                btn_SaveSource.Text = _BtnName;
                lookUpEdit_Project_Type.EditValue = _ProjectId;
                txt_Source_Type.Text = _SourceType;
                userid = User_Id;
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderSourceType/BindProjectType", data);
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

                            lookUpEdit_Project_Type.Properties.DataSource = dt;
                            lookUpEdit_Project_Type.Properties.DisplayMember = "Project_Type";
                            lookUpEdit_Project_Type.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            lookUpEdit_Project_Type.Properties.Columns.Add(col);
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
        private async void BindProdctType(int Project_Id)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_PRODUCT_TYPE"},
                    {"@Project_Type_Id",Project_Id }
                };               
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())

                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/OrderSourceType/BindProductType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0 )
                            {
                                DataRow dr = dt.NewRow();                              
                                if (_Operaion_Id == "View")
                                {
                                    checkbox_Product_Type.DataSource = dt;
                                    checkbox_Product_Type.DisplayMember = "Product_Type";
                                    checkbox_Product_Type.ValueMember = "ProductType_Id";
                                    checkbox_Product_Type.SelectedValue = _ProductId;
                                    int _Prodcut = checkbox_Product_Type.SelectedIndex;
                                    checkbox_Product_Type.SetItemChecked(_Prodcut, true);
                                }
                                else
                                {
                                    checkbox_Product_Type.DataSource = dt;
                                    checkbox_Product_Type.DisplayMember = "Product_Type";
                                    checkbox_Product_Type.ValueMember = "ProductType_Id";
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

        private void lookUpEdit_Project_Type_EditValueChanged(object sender, EventArgs e)
        {
            if (lookUpEdit_Project_Type.ItemIndex > 0)
            {
                Project_Id = Convert.ToInt32(lookUpEdit_Project_Type.EditValue);
                BindProdctType(Project_Id);
            }
        }

        private void btn_ClearSource_Click(object sender, EventArgs e)
        {
            Clear();         
        }
        private void Clear()
        {
            lookUpEdit_Project_Type.EditValue = null;
            lookUpEdit_Project_Type.ItemIndex = 0;
            checkbox_Product_Type.SelectedIndex = 0;
            checkbox_Product_Type.UnCheckAll();
            txt_Source_Type.Text = "";
            checkbox_Product_Type.DataSource = null;
        }
        private bool Validate()
        {
            if(lookUpEdit_Project_Type.EditValue==null)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return false;
            }
            if(checkbox_Product_Type.CheckedItems==null|| checkbox_Product_Type.CheckedItemsCount==0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Product Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if(txt_Source_Type.Text=="")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter Source Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private async Task<bool> CheckSourcre()
        {
            DataTable dt = new DataTable();
            try
            {
                if (Validate() != false)
                {
                    foreach (object itemchecked in checkbox_Product_Type.CheckedItems)
                    {
                        DataRowView CastedItems = itemchecked as DataRowView;
                        Productvalue = Convert.ToInt32(CastedItems["ProductType_Id"]);
                        var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "CHECK_SOURCE" },
                    { "@Project_Type_Id", lookUpEdit_Project_Type.EditValue},
                    { "@ProductType_Id",Productvalue},
                    { "@Employee_source",txt_Source_Type.Text.Trim() }
                };

                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/OrderSourceType/CheckSource", data);
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
                                        XtraMessageBox.Show("Source Type Already Exists", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public async void btn_SaveSource_Click(object sender, EventArgs e)
        {
           
            SourceTypeTxt = txt_Source_Type.Text;
            ProjectValue = Convert.ToInt32(lookUpEdit_Project_Type.EditValue);
            if (btn_SaveSource.Text == "Save" && Validate() != false && (await CheckSourcre()) != false)
            {
                try
                {
                    DataTable dtinsert = new DataTable();
                    dtinsert.Columns.AddRange(new DataColumn[6]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("ProductType_Id",typeof(int)),
                     new DataColumn("Employee_source",typeof(string)),
                     new DataColumn("Inserted_by",typeof(int)),
                     new DataColumn("Inserted_date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                    });
                    foreach (object itemchecked in checkbox_Product_Type.CheckedItems)
                    {
                        DataRowView CastedItems = itemchecked as DataRowView;
                        Productvalue = Convert.ToInt32(CastedItems["ProductType_Id"]);
                        dtinsert.Rows.Add(ProjectValue, Productvalue, SourceTypeTxt, userid, DateTime.Now, "True");
                    }                    
                   
                    var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/OrderSourceType/InsertSource", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);                            
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Sucessfully");                             
                                Clear();
                                this.Mainform.BindSourceTypes();
                                this.Close();                        
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
            else if (btn_SaveSource.Text == "Edit" && Validate() != false && (await CheckSourcre()) != false)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);


                    DataTable dtupdate = new DataTable();
                    dtupdate.Columns.AddRange(new DataColumn[6]
                    {
                      new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("ProductType_Id",typeof(int)),
                     new DataColumn("Employee_source",typeof(string)),
                     //new DataColumn("Inserted_by",typeof(int)),
                     //new DataColumn("Inserted_date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Modified_by",typeof(int)),
                     new DataColumn("Modified_date",typeof(DateTime))
                    });
                    foreach (object item in checkbox_Product_Type.CheckedItems)
                    {
                        DataRowView castedItem = item as DataRowView;
                        int Productval = Convert.ToInt32(castedItem["ProductType_Id"]);
                        int projecttype = ProjectValue;
                        dtupdate.Rows.Add(ProjectValue, Productval, SourceTypeTxt, "True", userid, DateTime.Now);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtupdate), Encoding.UTF8, "application/json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PutAsync(Base_Url.Url + "/OrderSourceType/UpdatSource", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Edited Successfully");
                                Clear();
                                this.Mainform.BindSourceTypes();
                                this.Close();
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
}