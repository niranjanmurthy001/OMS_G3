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
using System.IO;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Alert_Setings : DevExpress.XtraEditors.XtraForm
    {
        int userid;
        public Alert_Setings(int User_Id)
        {
            userid = User_Id;
            InitializeComponent();
        }

        private void btn_Addnew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Alert_Settings_Entry Alert = new Alert_Settings_Entry(userid,this);
            Alert.Show();
        }

        private void Alert_Setings_Load(object sender, EventArgs e)
        {
            BindUserwiseAlertType();
        }
        public async void BindUserwiseAlertType()
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/AlertType/BindGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {

                                Grd_Alert.DataSource = _dt;

                            }
                            else
                            {
                                Grd_Alert.DataSource = null;

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

        private async void btn_Delete_Click(object sender, EventArgs e)
        {
            DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (show == DialogResult.Yes)
            {

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    List<int> gridViewSelectedRows = gridView1.GetSelectedRows().ToList();
                    for (int i = 0; i < gridViewSelectedRows.Count; i++)
                    {
                        DataRow row = gridView1.GetDataRow(gridViewSelectedRows[i]);
                        int Userroleid = int.Parse(row["User_Role_Id"].ToString());
                        int Alerttypeid= int.Parse(row["Alert_Type_Id"].ToString());
                        var dictionary = new Dictionary<string, object>()
                        {
                            { "@Trans", "DELETE" },
                            { "@User_Role_Id", Userroleid },
                            {"@Alert_Type_Id",Alerttypeid }
                        };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/AlertType/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);

                                    BindUserwiseAlertType(); 
                                    btn_Delete.Visible = false;

                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Please Select Any Record To Delete");
                            }
                        }

                    }
                    XtraMessageBox.Show("Record Deleted Successfully");
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

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridView1.SelectedRowsCount !=0)
            {
                btn_Delete.Visible = true;
            }
            else
            {
                btn_Delete.Visible = false;
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\OMS\";
            string fileName = filePath + "User_Role Wise_Alert Type" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Grd_Alert.ExportToXlsx(fileName);
            System.Diagnostics.Process.Start(fileName);
        }
    }
}