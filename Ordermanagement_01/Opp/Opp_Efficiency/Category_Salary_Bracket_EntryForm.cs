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
using Ordermanagement_01.Masters;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Category_Salary_Bracket_EntryForm : DevExpress.XtraEditors.XtraForm
    {
        int User_Id, Project_Type_Id;
        public Category_Salary_Bracket_EntryForm()
        {
            InitializeComponent();
        }

        private void Category_Salary_Bracket_EntryForm_Load(object sender, EventArgs e)
        {
            BindProjectType();
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/BindProject", data);
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
                            ddl_Project_Type.Properties.DataSource = dt;
                            ddl_Project_Type.Properties.DisplayMember = "Project_Type";
                            ddl_Project_Type.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddl_Project_Type.Properties.Columns.Add(col);

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
                if (btn_Submit.Text == "Submit" && Validation() == true)
                {
                    User_Id = 1;
                    Project_Type_Id = Convert.ToInt32(ddl_Project_Type.EditValue);

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictionary = new Dictionary<string, object>();
                    {
                        dictionary.Add("@Trans", "INSERT");
                        dictionary.Add("@Category_Name", txt_Category.Text);
                        dictionary.Add("@Salary_From", txt_salryfrom.Text);
                        dictionary.Add("@Salary_To", txt_SalaryTo.Text);
                        dictionary.Add("@Inserted_By", User_Id);
                        dictionary.Add("@Project_Type_Id", Project_Type_Id);

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show(" Project Wise Category Salary Bracket Submitted Successfully ");
                                btn_Clear_Click(sender, e);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                SplashScreenManager.CloseForm(false);
                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            ddl_Project_Type.ItemIndex = 0;
            txt_Category.Text = "";
            txt_SalaryTo.Text = "";
            txt_salryfrom.Text = "";
        }

        private bool Validation()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Project_Type");
                ddl_Project_Type.Focus();
                return false;
            }
            if (txt_Category.Text == "")
            {
                XtraMessageBox.Show("Category Field Must not be Empty");
                txt_Category.Focus();
                return false;
            }
            if(txt_salryfrom.Text=="")
            {
                XtraMessageBox.Show("Salary From Field Must not be Empty");
                txt_salryfrom.Focus();
                return false;
            }
            if (txt_SalaryTo.Text == "")
            {
                XtraMessageBox.Show("Salary To Field Must not be Empty");
                txt_SalaryTo.Focus();
                return false;
            }
            return true;
        }
    }
}