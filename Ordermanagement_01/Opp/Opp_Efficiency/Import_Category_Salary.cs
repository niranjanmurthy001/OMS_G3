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
using Newtonsoft.Json;
using DevExpress.XtraSplashScreen;
using System.Net;
using Ordermanagement_01.Models;
using System.Net.Http;
using Ordermanagement_01.Masters;
using System.IO;
using System.Diagnostics;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Import_Category_Salary : DevExpress.XtraEditors.XtraForm
    {
        string txtFilename;
        double Col_Name;
            int _ProjectId;
        DataTable _dtcol = new DataTable();
        public Import_Category_Salary()
        {
            InitializeComponent();
        }

        private async void btn_download_Click(object sender, EventArgs e)
        {
            if (Validation() != false)
            {
                try
                {
                    _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Select Headers" },
                    {"@Project_Type_Id" ,_ProjectId}

                };

                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindHeaders", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                _dtcol.Columns.Add("Project Type");
                                _dtcol.Columns.Add("Client Name");
                                _dtcol.Columns.Add("Order Task");
                                _dtcol.Columns.Add("Order Type");
                                _dtcol.Columns.Add("Order Source Type");
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    Col_Name = Convert.ToDouble(dt.Rows[i]["Category_Name"]);
                                    _dtcol.Columns.Add(Col_Name.ToString());
                                }
                                string filePath = @"C:\Category Salary Bracket\";
                                string fileName = filePath + "Category Salary Bracket-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                                StreamWriter wr = new StreamWriter(fileName);

                                try
                                {

                                    for (int i = 0; i < _dtcol.Columns.Count; i++)
                                    {
                                        wr.Write(_dtcol.Columns[i].ToString().ToUpper() + "\t");
                                    }

                                    wr.WriteLine();

                                    //write rows to excel file
                                    for (int i = 0; i < (_dtcol.Rows.Count); i++)
                                    {
                                        for (int j = 0; j < _dtcol.Columns.Count; j++)
                                        {
                                            if (dt.Rows[i][j] != null)
                                            {
                                                wr.Write(Convert.ToString(_dtcol.Rows[i][j]) + "\t");
                                            }
                                            else
                                            {
                                                wr.Write("\t");
                                            }
                                        }
                                        //go to next line
                                        wr.WriteLine();
                                    }
                                    //close file
                                    wr.Close();
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
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

        private void Import_Category_Salary_Load(object sender, EventArgs e)
        {
            BindProjectType();
        }

        private void btn_choosefile_Click(object sender, EventArgs e)
        {
            if (Validation() == true)
            {
                try
                {
                    OpenFileDialog fileup = new OpenFileDialog();
                    fileup.Title = "Select Error Tab File";
                    fileup.InitialDirectory = @"c:\";

                    fileup.Filter = "Excel Sheet(*.xlsx)|*.xlsx|Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                    fileup.FilterIndex = 1;
                    fileup.RestoreDirectory = true;

                    if (fileup.ShowDialog() == DialogResult.OK)
                    {
                        txtFilename = fileup.FileName;

                        lbl_Uploadfilename.Text = txtFilename;

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
        }

        private bool Validation()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                XtraMessageBox.Show("Select Project_Type");
                ddl_Project_Type.Focus();
                return false;
            }
            return true;
        }

        private void grd_Category_Salary_Bracket_Click(object sender, EventArgs e)
        {

        }
    }
}