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
using WaitForm_SetDescription;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Sub_Product_Type_View : DevExpress.XtraEditors.XtraForm
    {
        string Operation_Type;
        int Project_Id;

        public Sub_Product_Type_View()
        {
            InitializeComponent();
        }

        private void Sub_Product_Type_View_Load(object sender, EventArgs e)
        {
            btn_delete_Multiple_Abs.Visible = false;
            BindGridType();
            BindGridAbs();
            btn_Delete_MultipleType.Visible = false;
            //if (tabPane1.SelectedPage == tabNavigationPage1)
            //{
               
            //}
            //else if (tabPane1.SelectedPage==tabNavigationPage2)
            //{
               
            //}
        }

        private void btn_Add_New_Type_Click(object sender, EventArgs e)
        {
            Operation_Type = "Sub Product Type";
            Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SPT = new Sub_Product_Type_Entry(Operation_Type);
            SPT.Show();
        }

        private void btn_Add_New_Abs_Click_1(object sender, EventArgs e)
        {
            Operation_Type = "Sub Product Type Abbreviation";
            Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SPT = new Sub_Product_Type_Entry(Operation_Type);
            SPT.Show();
        }
        //public async void BindProjectType()
        //{
        //    try
        //    {
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //        var dictonary = new Dictionary<string, object>()
        //        {
        //            {"@Trans","SELECT_PROJECT_TYPE" }
        //        };

        //        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
        //        using (var httpclient = new HttpClient())
        //        {
        //            var response = await httpclient.PostAsync(Base_Url.Url + "/SubProductType/BindProjectType", data);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    var result = await response.Content.ReadAsStringAsync();
        //                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
        //                    if (dt != null && dt.Rows.Count > 0)
        //                    {
        //                        DataRow dr = dt.NewRow();
        //                        dr[0] = 0;
        //                        dr[1] = "Select";
        //                        dt.Rows.InsertAt(dr, 0);
        //                    }

        //                    ddl_Project_Type.Properties.DataSource = dt;
        //                    ddl_Project_Type.Properties.DisplayMember = "Project_Type";
        //                    ddl_Project_Type.Properties.ValueMember = "Project_Type_Id";
        //                    DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
        //                    col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
        //                    ddl_Project_Type.Properties.Columns.Add(col);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);
        //    }
        //}

        public async void BindGridType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT_SUB_PRODUCT_TYPE"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/BindGridType", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Type.DataSource = dt;
                            }
                            else
                            {
                                grd_Type.DataSource = null;
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
        public async void BindGridAbs()
        {         
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT_SUB_PRODUCT_TYPE_ABS"},                       
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/BindGridAbs", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Abs.DataSource = dt;
                            }
                            else
                            {
                                grd_Abs.DataSource = null;
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

        private void gridView_Type_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView_Abs_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
    }
}