using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class ProjectType_Notification_Settings_View : XtraForm
    {
        public int User_Id { get; private set; }

        int ProjectId;
        string MessageEditText;
        string Btn_Name;
        string OperType;
        int Id;
        public ProjectType_Notification_Settings_View( int User_id)
        {
            InitializeComponent();
            this.User_Id = User_id;
        }

        private void ProjectType_Notification_Settings_View_Load(object sender, EventArgs e)
        {
            BindGrid();
            btnMultiselectDelete.Visible = false;
        }

        private void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                this.Enabled = false;
                OperType = "Insert";
                ProjectType_Notification_Setitings_Entry ns = new ProjectType_Notification_Setitings_Entry(OperType, Id, ProjectId, MessageEditText, User_Id, Btn_Name, this);
                ns.Show();
            }
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);

               XtraMessageBox.Show("SomeThing Went Wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }


        public async void BindGrid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BindGridData" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/BindGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                           DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                gridNotification.DataSource = _dt;
                                //gridView1.BestFitColumns();
                                this.gridColumn1.Width = 15;
                                this.gridColumn2.Width = 160;
                                this.gridColumn4.Width = 5;
                                this.gridColumn5.Width =5;

                            }
                            else
                            {
                                gridNotification.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                SplashScreenManager.CloseForm(false);
                    
                XtraMessageBox.Show("Something Went Wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }

    
        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle > 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try {
                if (gridView1.RowCount > 0)
                {
                    string filePath = @"C:\Temp\";
                    string fileName = filePath + "Export Project Wise Nofications Settings-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    gridView1.ExportToXlsx(fileName);
                    System.Diagnostics.Process.Start(fileName);
                }
                else
                {
                    SplashScreenManager.CloseForm(false);

                } XtraMessageBox.Show("Records Not Found To Export","Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridView1.SelectedRowsCount!=0)
            {
                btnMultiselectDelete.Visible = true;
            }
            else
            {
                btnMultiselectDelete.Visible = false;
            }
        }

        private async void btnMultiselectDelete_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount != 0)
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
                            int Uploadid = int.Parse(row["Gen_Update_ID"].ToString());                           
                            var dictionary = new Dictionary<string, object>()
                          {
                            { "@Trans", "DELETE"},
                            { "@Gen_Update_ID", Uploadid },
                            { "@Modified_By", User_Id },
                            { "@Modified_Date", DateTime.Now },
                            { "@Status", "False" }
                         };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/Delete", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                    }
                                }
                            }
                        }
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Records Deleted Successfully");
                        btnMultiselectDelete.Visible = false;
                        BindGrid();
                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Error", "Please Contact Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }

                }
            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Any Record To Delete");
            }
        }

        private async void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "View")
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    ProjectId = int.Parse(row["Project_Type_Id"].ToString());
                    Id = int.Parse(row["Gen_Update_ID"].ToString());
                    MessageEditText = row["Message"].ToString();

                    Btn_Name = "Edit";
                    OperType = "Update";
                    this.Enabled = false;
                    ProjectType_Notification_Setitings_Entry ns = new ProjectType_Notification_Setitings_Entry(OperType, Id, ProjectId, MessageEditText, User_Id, Btn_Name, this);
                    ns.Show();
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something Went Wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else if (e.Column.Caption == "Delete")
            {
                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                        int uploadid = int.Parse(row["Gen_Update_ID"].ToString());
                        var dictionary = new Dictionary<string, object>()
                  {
                        { "@Trans", "DELETE"},
                       { "@Gen_Update_ID", uploadid },
                       { "@Modified_By", User_Id },
                        { "@Modified_Date", DateTime.Now },
                        { "@Status", "False" }
                  };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ProjectTypeNotificationSetting/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Record Deleted Successfully");
                                    BindGrid();
                                    btnMultiselectDelete.Visible = false;

                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Please Select Message To Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something Went Wrong");
                        //throw ex;
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);
                    }
                }
                else if (show == DialogResult.No)
                {

                }

            }
        }

       
    }
}
