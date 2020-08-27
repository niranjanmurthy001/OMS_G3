using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevExpress.XtraSplashScreen;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraEditors;
using System.Windows.Forms;
using System.Linq;

namespace Ordermanagement_01.Masters
{
    public partial class Order_Priority : DevExpress.XtraEditors.XtraForm
    {
        int Project_Type_Id, priority_Id;
        int User_Id=1;
        DataTable _dt = new DataTable();
        public Order_Priority()
        {
            InitializeComponent();
        }

        private void Order_Priority_Load(object sender, EventArgs e)
        {
            BindProjectType();
            BindCategorySalaryBracket();
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProjectWiseOrder/BindDetails", data);
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
                SplashScreenManager.CloseForm(false);
                throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_Submit.Text == "Submit" && Validation() == true )
                {
                    Project_Type_Id = Convert.ToInt32(ddl_ProjectType.EditValue);
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictionary = new Dictionary<string, object>();
                    {
                        dictionary.Add("@Trans", "INSERT");
                        dictionary.Add("@Project_Type_Id", Project_Type_Id);
                        dictionary.Add("@Order_Priority", txt_Priority.Text);
                        dictionary.Add("@Inserted_By", User_Id);

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectWiseOrder/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                BindCategorySalaryBracket();
                                XtraMessageBox.Show("Submitted Successfully");
                                btn_Clear_Click(sender, e);
                            }
                        }
                    }
                }
                else if (btn_Submit.Text == "Edit" && Validation() == true)
                {
                    Project_Type_Id = Convert.ToInt32(ddl_ProjectType.EditValue);
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictionary = new Dictionary<string, object>();
                    {
                        dictionary.Add("@Trans", "UPDATE");
                        dictionary.Add("@Order_Id", priority_Id);
                        dictionary.Add("@Order_Priority", txt_Priority.Text);
                        dictionary.Add("@Modified_By", User_Id);

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectWiseOrder/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                BindCategorySalaryBracket();
                                XtraMessageBox.Show("Updated Successfully");
                                btn_Clear_Click(sender, e);
                                ddl_ProjectType.Enabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private bool Validation()
        {
            if(Convert.ToInt32(ddl_ProjectType.EditValue)==0)
            {
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddl_ProjectType.Focus();
                return false;
            }
            if(txt_Priority.Text=="")
            {
                XtraMessageBox.Show("Please Enter Priority", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_Priority.Focus();
                return false;
            }
            return true;
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_ProjectType.ItemIndex = 0;
            txt_Priority.Text = "";
            btn_Submit.Text = "Submit";
            ddl_ProjectType.Enabled = true;
        }

        public async void BindCategorySalaryBracket()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_DATA_TO_GRID" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProjectWiseOrder/BindDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                             _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count >= 0)
                            {

                                grd_Priority.DataSource = _dt;

                            }
                            else
                            {
                                grd_Priority.DataSource = null;

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

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private async void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "Delete")
            {
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                        int Priority_Id = int.Parse(row["Priority_Id"].ToString());
                        var dictionary = new Dictionary<string, object>()
                        {
                        { "@Trans", "DELETE" },
                        { "@Order_Id", Priority_Id }
                        };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectWiseOrder/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Record Deleted Successfully");
                                    BindCategorySalaryBracket();

                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        throw ex;
                    }
                }
            }
            else if (e.Column.Caption == "View")
            {
                System.Data.DataRow rows = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                Project_Type_Id = Convert.ToInt32(rows["Project_Type_Id"].ToString());
                string priority = rows["Order_Priority"].ToString();
                btn_Submit.Text = "Edit";
                //var row = _dt.AsEnumerable().Where(dr => dr.Field<string>("Project_Type") == e.CellValue.ToString());
                //var index = row.FirstOrDefault();
                ddl_ProjectType.EditValue = rows.ItemArray[2];
                //ddl_ProjectType.EditValue = index.ItemArray[2] ;
                txt_Priority.Text = priority;
                priority_Id =int.Parse(rows["Priority_Id"].ToString());
                ddl_ProjectType.Enabled = false; 


            }
        }
    }
}