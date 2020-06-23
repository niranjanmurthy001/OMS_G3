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
        string Order_type;
        int order_type;
        private Efficiency_View Mainform = null;
        public Efficiency_Copy(int projectid, string Cname,int clientid,Form callinfrom)
        {
            InitializeComponent();
            Pid = projectid;
            _clientname = Cname;
            _clientid = clientid;
            Mainform = callinfrom as Efficiency_View;
        }

        private void Efficiency_Copy_Load(object sender, EventArgs e)
        {
           
            BindClientTo(Pid);
            BindClientName(Pid);
           // Getdata();

        }
        private async void BindClientName(int _Proc)
        {
            try
            {
                ddl_client_from.Properties.DataSource = null;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_CLIENT_NAMES_BASEDON_PROJECTID" },
                    {"@Project_Type_Id",_Proc }

                };
                ddl_client_from.Properties.Columns.Clear();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindProject", data);
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
                            ddl_client_from.Properties.DataSource = dt;
                            ddl_client_from.Properties.DisplayMember = "Client_Name";
                            ddl_client_from.Properties.ValueMember = "Client_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Client_Name");
                            ddl_client_from.Properties.Columns.Add(col);

                        }
                    }
                    else
                    {
                        ddl_client_from.Properties.DataSource = null;
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
                                
                            
                           
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var dictionary = new Dictionary<string, object>()
                                {
                                    { "@Trans", "GetItems"},
                                    {"@Project_Type_Id",Pid }
                                };
                                var _data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response1 = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindColumns", _data);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result1);
                                            for (int i = dt.Rows.Count-1; i >= 0; i--)
                                            {
                                                for (int j = 0; j < _dt.Rows.Count; j++)
                                                {
                                                    DataRow _dr = _dt.Rows[j];
                                                    DataRow dr = dt.Rows[i];
                                                    if (dr["Client_Name"].ToString() == _dr["Client_Name"].ToString())
                                                    {
                                                        dr.Delete();
                                                    }
                                                    i = i - 1;
                                                    dt.AcceptChanges();
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                            Chk_Targetclient.DataSource = dt;
                            Chk_Targetclient.DisplayMember = "Client_Name";
                            Chk_Targetclient.ValueMember = "Client_Id";

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
            if (Validation() == true)
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
                        {"@Trans","INSERT" },
                        {"@from_Client",ddl_client_from.EditValue },
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
                                    clear();
                                    XtraMessageBox.Show("Submitted Successfully");
                                    this.Mainform.BindCategorySalaryBracket();
                                    this.Close();

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
        private void clear()
        {
            Chk_Targetclient.SelectedIndex = 0;
            Chk_Targetclient.UnCheckAll();
        }
         private bool Validation()
        {
            if(Convert.ToInt32(ddl_client_from.EditValue)==0)
            {
                XtraMessageBox.Show("Please Select From Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
           if(Chk_Targetclient.CheckedItems.Count<=0)
            {
                XtraMessageBox.Show("Please Check Target Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private async void Getdata()
        {
            
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GetItems"},
                        {"@Project_Type_Id",Pid }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindColumns", data);
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
                                    int item_ = Convert.ToInt32(row.ItemArray[1]);
                                    Chk_Targetclient.SelectedValue = item_;
                                    int _task = Chk_Targetclient.SelectedIndex;
                                    Chk_Targetclient.SetItemChecked(_task, true);                                  
                                }
                               
                            }
                            foreach (var item in Chk_Targetclient.CheckedItems.OfType<string>().ToList())
                            {
                                Chk_Targetclient.Items.Remove(item);
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
    }
}