using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;


namespace Ordermanagement_01.Opp
{
    public partial class Stm_Form : Form
    {
        int User_Id = 1;
        int Work_Type_Id = 1;
        //string FieldName;
        //int VisibleIndex;
        //bool _Visible;
        public Stm_Form()
        {
            InitializeComponent();
        }

        private async void gridView1_ColumnPositionChanged(object sender, EventArgs e)
        {
            DataTable dtbulk = new DataTable();
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                dtbulk.Clear();
                dtbulk.Columns.AddRange(new DataColumn[4]
                    {
                        new DataColumn("User_ID",typeof(int)),
                        new DataColumn("Column_Name",typeof(string)),
                        new DataColumn("Column_Visible_Index",typeof(int)),
                        new DataColumn("Column_Visible_Status",typeof(bool))
                    });

                gridView1.Columns.ToList().ForEach(col => dtbulk.Rows.Add(User_Id, col.FieldName, col.VisibleIndex, col.Visible));

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var data = new StringContent(JsonConvert.SerializeObject(dtbulk), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ProcessingOrders/Insert", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            SplashScreenManager.CloseForm(false);
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
            gridView1.BestFitColumns(true);
        }

        private async void Bind_Gridview_Columns_Status()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                using (var Client = new HttpClient())
                {
                    Dictionary<string, object> dist_Column_List = new Dictionary<string, object>();
                    if (Work_Type_Id == 1)
                    {
                        dist_Column_List.Add("@Trans", "SELECT");
                        dist_Column_List.Add("@User_Id", User_Id);
                        var Serialised_Data = JsonConvert.SerializeObject(dist_Column_List);
                        var content = new StringContent(Serialised_Data, Encoding.UTF8, "application/json");
                        var result = await Client.PostAsync(Base_Url.Url + "/ProcessingOrders/Get_Column_Data", content);
                        if (result.IsSuccessStatusCode)
                        {
                            string DataJsonString = await result.Content.ReadAsStringAsync();
                            if (DataJsonString != null)
                            {
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(DataJsonString);
                                dt.AsEnumerable().Select(row => new ColumnData()
                                {
                                    FieldName = row["Column_Name"].ToString(),
                                    VisibleIndex = Convert.ToInt32(row["Column_Visible_Index"]),
                                    Visible = Convert.ToBoolean(row["Column_Visible_Status"])
                                }).ToList()?.ForEach(col =>
                                {
                                    gridView1.Columns[col.FieldName].VisibleIndex = col.VisibleIndex;
                                    gridView1.Columns[col.FieldName].Visible = col.Visible;
                                });
                                SplashScreenManager.CloseForm(false);
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

        private void Stm_Form_Load(object sender, EventArgs e)
        {
            Bind_Gridview_Columns_Status();
        }
    }
}
