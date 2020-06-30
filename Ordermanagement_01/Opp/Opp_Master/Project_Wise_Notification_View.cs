using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Project_Wise_Notification_View : XtraForm
    {
        public object Login_Id { get; private set; }
        public object User_id { get; private set; }

        public Project_Wise_Notification_View(int User_Id,int LoginType)
        {
            InitializeComponent();
            this.Login_Id = LoginType;
            this.User_id = User_Id;
        }

        private void Notification_View_Based_on_the_Project_Type_Wise_Load(object sender, EventArgs e)
        {
            Bindnotification();
        }
        private async void Bindnotification()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                {"@Trans","SelectNotification" },
                   // {"@User_Id",User_id },
                { "@Application_Login_Type",Login_Id}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectWiseNotification/BindNotification", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                gridNotification.DataSource = dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong,please contact admin");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
