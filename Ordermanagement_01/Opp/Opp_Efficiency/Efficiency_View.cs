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
using System.IO;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_View : DevExpress.XtraEditors.XtraForm
    {
        DataTable _dtcol;
        string Col_Name;
        int _ProjectId;
        DataTable _dt, dt;
        public Efficiency_View()
        {
            InitializeComponent();
        }

        private void Efficiency_Entry_Load(object sender, EventArgs e)
        {
            ddl_ProjectType.EditValue = 1;
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
                XtraMessageBox.Show("Please contact with Admin");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_Addnew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Create Efficiency_Entry = new Efficiency_Create();
            Efficiency_Entry.Show();
        }
        private async void BindColumnstogrid()
        {
            try
            {
                _dtcol = new DataTable();
                grd_Efficiency_Form.DataSource = null;
                gridView1.Columns.Clear();
                _ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Select Headers" },
                    {"@Project_Type_Id" ,_ProjectId}

                };
                this.grd_Efficiency_Form.DataSource = new DataTable();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindHeaders", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                             dt = JsonConvert.DeserializeObject<DataTable>(result);
                            _dtcol.Columns.Add("Client_Name");
                            _dtcol.Columns.Add("Order_Task");
                            _dtcol.Columns.Add("Order_Type");
                            _dtcol.Columns.Add("Order_Source_Type");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                Col_Name = Convert.ToDouble(dt.Rows[i]["Category_Name"]).ToString();
                                _dtcol.Columns.Add(Col_Name.ToString());
                                //count = Convert.ToInt32(_dt.Rows.Count.ToString());

                            }
                            grd_Efficiency_Form.DataSource = _dtcol;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
                //BindRows();
                BindColumnstogrid();
                BindCategorySalaryBracket();
            }

        }

        private async void BindCategorySalaryBracket()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_DATA_TO_GRID" },
                    {"@Project_Type_Id",_ProjectId}
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindDataGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                DataView dv = new DataView(_dt);
                                DataTable _dtcolumns = dv.ToTable(true, "Category_Name", "Allocated_Time");
                                DataTable dtcol = dv.ToTable(true, "Category_Name");
                                DataTable _dtrows = dv.ToTable(true, "Client_Name", "Order_Task", "Order_Type", "Order_Source_Type");
                               // _dtcolumns = dv.ToTable(true, "Allocated_Time");
                                for (int i = 0; i < dtcol.Rows.Count; i++)
                                {
                                    Col_Name = Convert.ToDouble(dtcol.Rows[i]["Category_Name"]).ToString();

                                    List<string> myList = new List<string>();
                                    foreach (DataColumn column in dtcol.Columns)
                                    {
                                        myList.Add(column.ColumnName);
                                    }
                                    myList.Add(Col_Name);
                                    if (IsAllColumnExist(dtcol, myList))
                                    {
                                        _dtrows.Columns.Add(Col_Name.ToString());
                                        // int  index =0;
                                    }
                                }
                                for (int i = 0; i < _dtrows.Rows.Count; i++)
                                {
                                    for (int j = 0; j < _dtcolumns.Rows.Count; j++)
                                    {
                                        Col_Name = Convert.ToDouble(_dtcolumns.Rows[j]["Category_Name"]).ToString();

                                        if (_dtrows.Rows[i][Col_Name].ToString() == "")
                                        {
                                            _dtrows.Rows[i][Col_Name] = (_dtcolumns.Rows[j]["Allocated_Time"]).ToString();
                                        }
                                        else
                                        {
                                            i = i + 1;
                                            _dtrows.Rows[i][Col_Name] = (_dtcolumns.Rows[j]["Allocated_Time"]).ToString();
                                        }
                                    }
                                }
                                grd_Efficiency_Form.DataSource = _dtrows;

                            }
                            else
                            {
                                grd_Efficiency_Form.DataSource = null;

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

        private void btn_Export_Click(object sender, EventArgs e)
        {
            if (grd_Efficiency_Form.DataSource != null)
            {
                string filePath = @"C:\OMS\";
                string fileName = filePath + "Efficiency-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                grd_Efficiency_Form.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                XtraMessageBox.Show("Select Project Type to Export");
            }
        }

        private bool IsAllColumnExist(DataTable tableNameToCheck, List<string> columnsNames)
        {
            if (tableNameToCheck != null)
            {
                foreach (string columnName in columnsNames)
                {
                    if (!(tableNameToCheck.Columns.Contains(columnName.Trim())))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

      
    }
}