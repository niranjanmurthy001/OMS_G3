using ClosedXML.Excel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ordermanagement_01.New_Dashboard.Orders
{
    public partial class ImportOrders : XtraForm
    {
        private DataTable dtProcessSettings;
        private int projectTypeId;
        public ImportOrders()
        {
            InitializeComponent();
        }
        private async void ImportOrders_Load(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                await BindClients();
                GetProcessSettings();
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private async Task BindClients()
        {
            try
            {
                lookUpEditClient.Properties.DataSource = null;
                lookUpEditClient.Properties.Columns.Clear();
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_CLIENTS" }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Master/BindClients", data);
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
                                dr[1] = "SELECT";
                                dt.Rows.InsertAt(dr, 0);
                                lookUpEditClient.Properties.DataSource = dt;
                                lookUpEditClient.Properties.DisplayMember = "Client_Name";
                                lookUpEditClient.Properties.ValueMember = "Client_Id";
                                lookUpEditClient.Properties.Columns.Add(new LookUpColumnInfo("Client_Name", "Client"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void BindSubClients(object subClientId)
        {
            lookUpEditSubClient.Properties.DataSource = null;
            lookUpEditSubClient.Properties.Columns.Clear();
            lookUpEditSubClient.EditValue = 0;
            var dictionary = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_SUB_CLIENTS" },
                    {"@Client_Id",subClientId }
                };
            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var response = httpClient.PostAsync(Base_Url.Url + "/Master/BindSubClients", data).Result;

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.NewRow();
                            dr[1] = 0;
                            dr[0] = "SELECT";
                            dt.Rows.InsertAt(dr, 0);
                            lookUpEditSubClient.Properties.DataSource = dt;
                            lookUpEditSubClient.Properties.DisplayMember = "Sub_ProcessName";
                            lookUpEditSubClient.Properties.ValueMember = "Subprocess_Id";
                            lookUpEditSubClient.Properties.Columns.Add(new LookUpColumnInfo("Sub_ProcessName", "Sub Client"));

                        }
                    }
                }
            }
        }
        private void GetProcessSettings()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(Base_Url.Url + "/Master/ProcessSettings").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(response.Content.ReadAsStringAsync().Result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dtProcessSettings = dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void lookUpEditClient_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lookUpEditClient.EditValue) > 0)
            {
                BindSubClients(lookUpEditClient.EditValue);
            }
            else
            {
                lookUpEditSubClient.Properties.DataSource = null;
                lookUpEditSubClient.Properties.Columns.Clear();
            }
        }
        private void lookUpEditSubClient_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lookUpEditSubClient.EditValue) > 0)
            {
                projectTypeId = 0;
                labelControlProjectType.Text = "Project type not set";
                var settingsRow = dtProcessSettings
                                        .AsEnumerable()
                                        .FirstOrDefault(row => row.Field<long>("Client_Id") == Convert.ToInt64(lookUpEditClient.EditValue)
                                         && row.Field<long>("Subprocess_Id") == Convert.ToInt64(lookUpEditSubClient.EditValue));
                if (settingsRow != null)
                {
                    projectTypeId = Convert.ToInt32(settingsRow["Project_Type_Id"]);
                    labelControlProjectType.Text = settingsRow["Project_Type"].ToString();
                }
            }
        }
        private async void buttonExportTemplate_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lookUpEditClient.EditValue) < 1)
            {
                XtraMessageBox.Show("Select Client");
                lookUpEditClient.Focus();
                return;
            }
            if (Convert.ToInt32(lookUpEditSubClient.EditValue) < 1)
            {
                XtraMessageBox.Show("Select Sub Client");
                lookUpEditSubClient.Focus();
                return;
            }
            try
            {
                var dictionary = new Dictionary<string, object>
                {
                    {"@Type", labelControlProjectType.Text }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Master/columns", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var list = JsonConvert.DeserializeObject<List<object>>(response.Content.ReadAsStringAsync().Result);
                            if (list != null && list.Count > 0)
                            {
                                XLWorkbook xlWorkBook = new XLWorkbook();
                                var ws = xlWorkBook.Worksheets.Add(labelControlProjectType.Text);
                                for (int i = 0; i < list.Count; i++) ws.Cell(1, i + 1).Value = list[i];
                                string fileName = @"C:\OMS\Temp\Template.xlsx";
                                xlWorkBook.SaveAs(fileName);
                                Process.Start(fileName);
                            }
                            else
                            {
                                XtraMessageBox.Show("Template not found");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Something went wrong " + ex.Message);
            }
        }
    }
}