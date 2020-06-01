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
using System.Net.Http;
using Ordermanagement_01.Models;
using System.Net;
using Ordermanagement_01.Masters;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Category_Salary_Bracket_ProjectWise : DevExpress.XtraEditors.XtraForm
    {
        public Category_Salary_Bracket_ProjectWise()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_addnew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Category_Salary_Bracket_EntryForm EF = new Category_Salary_Bracket_EntryForm();
            EF.Show();
        }

        private async void BindCategorySalaryBracket()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/BindGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {

                                Grd_Category_Salary.DataSource = _dt;

                            }
                            else
                            {
                                Grd_Category_Salary.DataSource = null;

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

        private void Category_Salary_Bracket_ProjectWise_Load(object sender, EventArgs e)
        {
            BindCategorySalaryBracket();
        }
    }
}