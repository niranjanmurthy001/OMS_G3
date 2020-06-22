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
using Ordermanagement_01.Masters;
using System.Net.Http;
using Ordermanagement_01.Models;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Copy : DevExpress.XtraEditors.XtraForm
    {
        int Pid;
        string _clientname;
        int _Targetclientid, Targetclientid,_clientid;
        public Efficiency_Copy(int projectid, string Cname,int clientid)
        {
            InitializeComponent();
            Pid = projectid;
            _clientname = Cname;
            _clientid = clientid;
        }

        private void Efficiency_Copy_Load(object sender, EventArgs e)
        {

            BindClientTo(Pid);
            txt_Clientname.Text = _clientname;
            txt_Clientname.Enabled = false;

        }


        private async void BindClientTo(int Projectid)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_CLIENT_NAMES_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",Pid }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindClientTo", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Chk_Targetclient.DataSource = dt;
                                Chk_Targetclient.DisplayMember = "Client_Name";
                                Chk_Targetclient.ValueMember = "Client_Id";
                            }


                        }
                    }
                    else
                    {
                        Chk_Targetclient.DataSource = null;
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                DataRowView r1 = Chk_Targetclient.GetItem(Chk_Targetclient.SelectedIndex) as DataRowView;
                _Targetclientid = Convert.ToInt32(r1["Client_Id"]);
                foreach (object itemChecked in Chk_Targetclient.CheckedItems)
                {
                    DataRowView _castedItem = itemChecked as DataRowView;
                    string Order_type = _castedItem["Client_Name"].ToString();
                    Targetclientid = Convert.ToInt32(_castedItem["Client_Id"]);
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictonary = new Dictionary<string, object>()
                    {
                        {"@Trans","Insert" },
                        {"@from_Client",_clientid },
                        {"@Targett_Client",Targetclientid }


                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/Insertdata", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Submitted Successfully");
                            }
                        }

                    }
                }
               
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
             
    }
}