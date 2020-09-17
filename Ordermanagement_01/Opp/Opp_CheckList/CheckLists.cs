using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using System.Windows.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraGrid.Columns;
using System.Linq;
using DevExpress.XtraEditors.Repository;
using Stimulsoft.Report;
using System.DirectoryServices;

using System.IO;

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckLists : DevExpress.XtraEditors.XtraForm
    {
        DataTable dt_tabName;
        int UserId, ProjectType_Id, OrderTypeAbs_Id, OrderId, ClientId, SubClientId, OrderTask, WorkType_Id;
        int tabid, count;
        DataTable dt1, dt_All1, dt_VIEW1, dt_Check;
        int index = 0;
        string Comments;
        int Ref_Checklist_Master_Type_Id, Checklist_Id, Check_List_Tran_ID;
        string Question;
        string File_Name, Path, report_pdf_Path;
        string File_size, ftpfullpath, directoryPath;
        private bool IsButton { get; set; }
        StiReport Report;
        string Ftp_Domain_Name, Ftp_User_Name, Ftp_Password, Upload_Directory, Ftp_Path, ftpUploadFullPath;
        DropDownistBindClass dbc = new DropDownistBindClass();


        public CheckLists(int User_Id, int Project_Type_Id, int Product_Type_Id, int Order_Id, int Client_Id, int SubClient_Id, int Order_Task, int Work_Type_Id)
        {
            InitializeComponent();
            UserId = User_Id;
            ProjectType_Id = Project_Type_Id;
            OrderTypeAbs_Id = Product_Type_Id;
            OrderId = Order_Id;
            ClientId = Client_Id;
            SubClientId = SubClient_Id;
            OrderTask = Order_Task;
            WorkType_Id = Work_Type_Id;
            // to get sever login credentials
            DataTable dt_ftp_Details = dbc.Get_Ftp_Details();
            if (dt_ftp_Details.Rows.Count > 0)
            {
                try
                {
                    Ftp_Domain_Name = dt_ftp_Details.Rows[0]["Ftp_Host_Name"].ToString();
                    Ftp_User_Name = dt_ftp_Details.Rows[0]["Ftp_User_Name"].ToString();
                    string Ftp_pass = dt_ftp_Details.Rows[0]["Ftp_Password"].ToString();
                    if (Ftp_pass != "")
                    {
                        Ftp_Password = dbc.Decrypt(Ftp_pass);
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Something went wrong");
                }
            }
            else
            {
                XtraMessageBox.Show("Ftp File Path was not found; You cannot upload the documents please check with administrator");
            }

        }

        private void CheckLists_Load(object sender, EventArgs e)
        {
            stiViewerControl1.Visible = false;
            IsButton = false;
            grd_CheckList.Visible = false;
            btn_Previous.Visible = false;
            btn_Save.Visible = false;
            btn_Next.Visible = true;
            GetOrderNumber();
            BindTabs();                                       
        }
        private async  void GetOrderNumber()
        {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","GET_ORDER_NUMBER" },
                    {"@Order_ID" ,OrderId}
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckLists/Get_Order_No", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                           label1.Text = "CheckList Entry - "+ dt.Rows[0]["Order_Number"].ToString();
                          
                         }
                    }
                }
         }

        private async void BindTabs()
        {
            try
            {
                dt_tabName = new DataTable();
                tabPane1.Pages.Clear();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_TABS" },
                    {"@Project_Type_Id" ,ProjectType_Id},
                    {"@ProductType_Abs_Id",OrderTypeAbs_Id }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckLists/Bindtabs", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string Col_Name = dt.Rows[i]["Checklist_Master_Type"].ToString();
                                string name = "tabnav" + i;
                                int Id = Convert.ToInt32(dt.Rows[i]["ChecklistType_Id"].ToString());
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var dictonary1 = new Dictionary<string, object>()
                                    {
                                        {"@Trans","GET_COUNT" },
                                        {"@Ref_Checklist_Master_Type_Id",Id },
                                      {"@Project_Type_Id",ProjectType_Id }
                                    };
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/CheckLists/BindTabNames", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            DataTable dtcount = JsonConvert.DeserializeObject<DataTable>(result1);
                                            if (dtcount != null && dtcount.Rows.Count > 0)
                                            {
                                                int count = Convert.ToInt32(dtcount.Rows[0]["Count"].ToString());
                                                if (count > 0)
                                                {
                                                    tabPane1.AddPage(Col_Name, name);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (tabPane1.Pages.Count > 0)
                            {

                                IsButton = true;
                                tabPane1.SelectedPageIndex = index;
                                string tabname = tabPane1.SelectedPage.Caption;
                                GetTabId(tabname);

                            }
                            if (tabPane1.Pages.Count == 1)
                            {
                                btn_Next.Visible = false;
                                btn_Save.Visible = true;

                            }
                            tabPane1.AddPage("Report Preview", "Report Preview");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void tabPane1_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            //   if (tabPane1.Pages.Count > 0)
            //    {              
            //            string tabname = tabPane1.SelectedPage.Caption;
            //            GetTabId(tabname); 
            //    }
        }

        private bool CheckNo(GridView view, int row)
        {
            GridColumn col = gridView1.Columns["No"];
            bool val_No = (bool)gridView1.GetRowCellValue(row, col);
            return (val_No == true);
        }
        private bool CheckYes(GridView view, int row)
        {
            GridColumn col = gridView1.Columns["Yes"];
            bool val_Yes = (bool)gridView1.GetRowCellValue(row, col);
            return (val_Yes == true);
        }

        private void gridView1_CellValueChanging_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Yes")
            {
                gridView1.SetRowCellValue(e.RowHandle, "Yes", true);
                gridView1.SetRowCellValue(e.RowHandle, "No", false);
                gridView1.SetRowCellValue(e.RowHandle, "Comments", "");              
            }
            else if (e.Column.FieldName == "No")
            {
                gridView1.SetRowCellValue(e.RowHandle, "Yes", false);
                gridView1.SetRowCellValue(e.RowHandle, "No", true);             
            }
          
        }

        private void gridView1_ShowingEditor_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //     disable/enable editing for yes / No  
            GridView gridView1 = sender as GridView;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckNo(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = false;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && CheckYes(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = true;
            if (this.gridView1.FocusedColumn.FieldName == "Comments" && !CheckYes(gridView1, this.gridView1.FocusedRowHandle) && !CheckNo(gridView1, this.gridView1.FocusedRowHandle))
                e.Cancel = true;
        }

        private void gridView1_RowCellStyle_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            GridView currentView = sender as GridView;
            if (e.Column.Caption == "Comments")
            {
                bool value_no = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "No"));
                if (value_no)
                {
                    e.Appearance.BackColor = Color.Red;
                    string comment = gridView1.GetRowCellValue(e.RowHandle, "Comments").ToString();
                    if (string.IsNullOrWhiteSpace(comment))
                    { e.Appearance.BackColor = Color.Red; }
                    else if (!string.IsNullOrWhiteSpace(comment))
                    { e.Appearance.BackColor = Color.White; }
                }
                bool value_yes = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "Yes"));
                if (value_yes)
                { e.Appearance.BackColor = Color.White; }

            }
           
            //else if (e.Column.FieldName == "No")
            //{
            //    bool value_yes = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "Yes"));
            //    bool value_no = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "No"));
            //    if (value_yes == false && value_no == false)
            //    { e.Appearance.BackColor = Color.Red; }
            //    else
            //    {
            //        e.Appearance.BackColor = Color.White;
            //    }
            //}
            //else if (e.Column.FieldName == "Yes" )
            //{
            //    bool value_yes = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "Yes"));
            //    bool value_no = Convert.ToBoolean(gridView1.GetRowCellValue(e.RowHandle, "No"));
            //    if (value_yes == false && value_no == false)
            //    { e.Appearance.BackColor = Color.Red; }
            //    else
            //    {
            //        e.Appearance.BackColor = Color.White;
            //    }
            //}
            //DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;
            //if (e.RowHandle >= 0)
            //{
            //    if (e.Column.Caption == "Comments")
            //    {
            //        if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == true)
            //        {
            //            string Comments = view.Row["Comments"].ToString();
            //            if (Comments != "")
            //            {
            //                e.Appearance.BackColor = Color.White;
            //            }
            //            else if (Comments == "")
            //            {
            //                e.Appearance.BackColor = Color.Red;
            //            }
            //        }
            //        else if ((bool)view.Row["No"] == false && (bool)view.Row["Yes"] == true)
            //        {
            //            gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
            //            e.Appearance.BackColor = Color.White;
            //        }
            //        else if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == false)
            //        {
            //            gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
            //            e.Appearance.BackColor = Color.White;
            //        }
            //    }
            //}
        }

        private void Download_Ftp_File(string File_Name, string File_path)
        {
            try
            {
                FtpWebRequest reqFTP;
                FileStream outputStream = new FileStream(@"C:\\Temp" + "\\" + File_Name, FileMode.Create);             
                reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(File_path));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(@"" + Ftp_User_Name + "", Ftp_Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.CopyTo(outputStream);
                ftpStream.Dispose();
                outputStream.Dispose();
                response.Dispose();
              
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Problem in Downloading Files please Check with Administrator");
            }
        }
        private void Load_Report()
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
           // Login_Server_Credentials();          
            Download_Ftp_File("Checklist_Report_Preview.mrt", "ftp://titlelogy.com/FTP_Application_Files/OMS/Oms_reports/Checklist_Report_Preview.mrt");
            StiReport Report = new StiReport();
            Report.Reset();
            Report.Dictionary.DataSources.Clear();
            // Report.Load(@"C:\bindu\DRN OMS TEST SOFT CODE 1\Checklist_Report_Preview.mrt");
            //  Report.Load("~/Reports/StimulSoftReports/Checklist_Report_Preview.mrt");
            report_pdf_Path = @"C:\\Temp\\Checklist_Report_Preview.pdf";
            Report.Load(@"C:\\Temp\\Checklist_Report_Preview.mrt");
         
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Id"].Value = Convert.ToString(OrderId);
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Task"].Value = Convert.ToString(OrderTask);
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Work_Type"].Value = Convert.ToString(WorkType_Id);
            Report.DataSources["Usp_CheckList_Report"].Parameters["@Project_Type_Id"].Value = Convert.ToString(ProjectType_Id);
            Report.DataSources["Usp_CheckList_Report"].Parameters["@User_Id"].Value = Convert.ToString(UserId);
            Report.Compile();
            Report.Render();
            Report.ExportDocument(StiExportFormat.Pdf, report_pdf_Path);
            //  Report.Show(true);
            SplashScreenManager.CloseForm(false);
            stiViewerControl1.Visible = true;
            stiViewerControl1.Dock = DockStyle.Fill;
            stiViewerControl1.Report = Report;
            tabPane1.SelectedPage.Controls.Add(stiViewerControl1);
        }

        private void tabPane1_SelectedPageIndexChanged_1(object sender, EventArgs e)
        {
            //if(tabPane1.SelectedPage.Caption=="Report Preview")
            //{
            //   stiViewerControl1.Visible = true;
            //    Load_Report();
            //}
        }

        private void gridView1_CustomDrawRowIndicator_1(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void tabPane1_SelectedPageChanging(object sender, SelectedPageChangingEventArgs e)
        {
            if (IsButton) e.Cancel = false;
            if (!IsButton)
            {
                e.Cancel = true;
            }
            IsButton = false;
        }
        private async void GetTabId(string tabname)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","GET_TAB_ID" },
                    {"@Checklist_Master_Type",tabname },
                    {"@Project_Type_Id",ProjectType_Id },

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckLists/GetTabIndex", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                tabid = Convert.ToInt32(dt.Rows[0]["ChecklistType_Id"].ToString());
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var dictonary1 = new Dictionary<string, object>()
                                {
                                    {"@Trans","CHECK_ORDER_ID_TASK_USER_WISE" },
                                    {"@Ref_Checklist_Master_Type_Id",tabid },
                                    {"@Order_Task",OrderTask },
                                    {"@Order_Id",OrderId },
                                    {"@Order_Type_Abs_Id",OrderTypeAbs_Id },
                                    {"@Work_Type",WorkType_Id },
                                    { "@Project_Type_Id",ProjectType_Id } ,
                                    { "@Client_Id",ClientId} ,
                                    {"@Sub_Client_Id",SubClientId }
                                };
                                var data1 = new StringContent(JsonConvert.SerializeObject(dictonary1), Encoding.UTF8, "Application/Json");
                                using (var httpclient1 = new HttpClient())
                                {
                                    var response1 = await httpclient1.PostAsync(Base_Url.Url + "/CheckLists/CheckOrderIdExist", data1);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result1 = await response1.Content.ReadAsStringAsync();
                                            dt_Check = JsonConvert.DeserializeObject<DataTable>(result1);
                                            int count = Convert.ToInt32(dt_Check.Rows[0]["Count"].ToString());
                                            if (count > 0)
                                            {
                                                TabData_View();
                                            }
                                            else
                                            {
                                                Grid_Bind_All_TabData();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        public async void Grid_Bind_All_TabData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_DETAILS"},
                        { "@Ref_Checklist_Master_Type_Id", tabid },
                        { "@Order_Task", OrderTask},
                        { "@OrderType_ABS_Id", OrderTypeAbs_Id },
                        { "@Project_Type_Id", ProjectType_Id },
                        { "@Client_Id", ClientId },
                        { "@Sub_Client_Id", SubClientId }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindGridGeneralView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_All = JsonConvert.DeserializeObject<DataTable>(result);
                            //  dt_All1 = dt_All.Copy();
                            dt_All.Columns.Add("Yes", typeof(bool));
                            dt_All.Columns.Add("No", typeof(bool));
                            dt_All.Columns.Add("Comments", typeof(string));
                            if (dt_All != null && dt_All.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt_All.Rows.Count; i++)
                                {
                                    dt_All.Rows[i]["Yes"] = "False";
                                    dt_All.Rows[i]["No"] = "False";
                                    dt_All.Rows[i]["Comments"] = "";
                                }

                                grd_CheckList.Visible = true;
                                //if (visit == 1)
                                //{ BindPreviousData(tabid, OrderTask, OrderTypeAbs_Id, ProjectType_Id, ClientId, SubClientId); }
                                grd_CheckList.DataSource = dt_All;
                                grd_CheckList.Dock = DockStyle.Fill;
                                tabPane1.SelectedPage.Controls.Add(grd_CheckList);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        public async void TabData_View()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "GET_ALL_VIEW"},
                        { "@Ref_Checklist_Master_Type_Id", tabid},
                        { "@Order_Task",OrderTask },
                        { "@Order_Id",OrderId },
                        { "@Order_Type_Abs_Id",OrderTypeAbs_Id },
                        { "@Work_Type",WorkType_Id },
                        { "@Project_Type_Id",ProjectType_Id } ,
                        { "@Client_Id",ClientId} ,
                        { "@Sub_Client_Id",SubClientId }
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindAllGeneralView", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_VIEW = JsonConvert.DeserializeObject<DataTable>(result);
                            dt_VIEW1 = dt_VIEW.Copy();
                            if (dt_VIEW != null && dt_VIEW.Rows.Count > 0)
                            {
                                grd_CheckList.Visible = true;
                                grd_CheckList.DataSource = dt_VIEW;
                                grd_CheckList.Dock = DockStyle.Fill;
                                tabPane1.SelectedPage.Controls.Add(grd_CheckList);
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;
            if (e.RowHandle >= 0)
            {
                if (e.Column.Caption == "Comments")
                {
                    if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == true)
                    {
                        string Comments = view.Row["Comments"].ToString();
                        if (Comments != "")
                        {
                            e.Appearance.BackColor = Color.White;
                        }
                        else if (Comments == "")
                        {
                            e.Appearance.BackColor = Color.Red;
                        }
                    }
                    else if ((bool)view.Row["No"] == false && (bool)view.Row["Yes"] == true)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
                        e.Appearance.BackColor = Color.White;
                    }
                    else if ((bool)view.Row["Yes"] == false && (bool)view.Row["No"] == false)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "Comments", "");
                        e.Appearance.BackColor = Color.White;
                    }
                }
            }
        }

        private async void SaveTabData()
        {
            IsButton = true;
            try
            {
                if (Validate() == true)
                {
                    if (grd_CheckList.DataSource != null)
                    {
                        DataTable dtinsert = new DataTable();
                        dtinsert.Columns.AddRange(new DataColumn[19]
                             {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Question",typeof(string)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("Work_Type",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Client_Id",typeof(int)),
                     new DataColumn("Sub_Client_Id",typeof(int))
                             });
                        for (int i = 0; i < gridView1.DataRowCount; i++)
                        {
                            bool chk_yes = (bool)(gridView1.GetRowCellValue(i, "Yes"));
                            bool chk_no = (bool)(gridView1.GetRowCellValue(i, "No"));
                            Comments = gridView1.GetRowCellValue(i, "Comments").ToString();
                            Ref_Checklist_Master_Type_Id = int.Parse(gridView1.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                            Checklist_Id = int.Parse(gridView1.GetRowCellValue(i, "Checklist_Id").ToString());
                            Question = (gridView1.GetRowCellValue(i, "Question").ToString());

                            dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, Question, chk_yes, chk_no, OrderId,
                                              OrderTask, OrderTypeAbs_Id, WorkType_Id, Comments, UserId, UserId,
                                              DateTime.Now, UserId, DateTime.Now, ProjectType_Id, "True", ClientId, SubClientId);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/Insert", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                                    tabPane1.SelectedPageIndex += 1;
                                    string tabname = tabPane1.SelectedPage.Caption;
                                    if (tabname == "Report Preview")
                                    {
                                       
                                        Load_Report();
                                    }
                                    else
                                    {
                                        GetTabId(tabname);
                                    }
                                    if (tabPane1.SelectedPageIndex > 0)
                                    {
                                        btn_Previous.Visible = true;
                                    }
                                    else
                                    {
                                        btn_Previous.Visible = false;
                                    }
                                    count = tabPane1.Pages.Count;
                                    if (tabPane1.SelectedPageIndex == count - 1)
                                    {
                                        btn_Save.Visible = true;
                                        btn_Next.Visible = false;
                                    }
                                    else
                                    {
                                        btn_Next.Visible = true;
                                        btn_Save.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void btn_Previous_Click_1(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                stiViewerControl1.Visible = false;
                if (tabPane1.SelectedPage.Caption != "Report Preview")
                { SavePreviousData(); }
                IsButton = true;
                btn_Next.Visible = true;
                btn_Save.Visible = false;
                tabPane1.SelectedPageIndex -= 1;
                index = tabPane1.SelectedPageIndex;
                if (index == 0)
                {
                    btn_Previous.Visible = false;
                }
                BindTabs();
            }
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Error in loading this content" + "\n" + "Please try again","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void btn_Next_Click_1(object sender, EventArgs e)
        {
            SaveTabData();

        }

        private void btn_Save_Click_1(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                //  SaveTabData();
                // if (Validate() == true)
                // {
                // XtraMessageBox.Show("Submitted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                Copy_Check_List_To_Server();
               
               // }
            }
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";
            File_size = size;
            return size;
        }
        private void CreateDirectory(string directoryPath)
        {
            try
            {
                Upload_Directory = directoryPath;
                string Ftp_Host_Name = Ftp_Domain_Name;
                Ftp_Path = Ftp_Host_Name + "/FTP_Application_Files/OMS/Oms_reports";
                string[] folderArray = Upload_Directory.Split('/');
                string folderName = "";
                for (int i = 0; i < folderArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(folderArray[i]))
                    {
                        try
                        {
                            folderName = string.IsNullOrEmpty(folderName) ? folderArray[i] : folderName + "/" + folderArray[i];
                            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://" + Ftp_Path + "/" + folderName);
                            ftp.Credentials = new NetworkCredential(@"" + Ftp_User_Name + "", Ftp_Password);
                            ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                            FtpWebResponse CreateForderResponse = (FtpWebResponse)ftp.GetResponse();
                            if (CreateForderResponse.StatusCode == FtpStatusCode.PathnameCreated)
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        //Copying Source File Into Destional Folder
        private async void Copy_Check_List_To_Server()
        {
            try
            {
                //form_loader.Start_progres();FTP_Application_Files/OMS/Oms_reports
                // string Source = @"\\192.168.12.33\OMS-REPORTS\Order Check List Report  New.pdf";
                if (WorkType_Id == 1)
                {
                    File_Name = "" + OrderId + "-" + OrderTask.ToString() + "-" + "CheckList Report" + ".pdf";
                }
                else if (WorkType_Id == 2)
                {

                    File_Name = "" + OrderId + " - " + " REWORK " + OrderTask.ToString() + "-" + "CheckList" + ".pdf";
                }
                else if (WorkType_Id == 3)
                {
                    File_Name = "" + OrderId + " - " + " SUPER QC " + OrderTask.ToString() + "-" + "CheckList" + ".pdf";
                }
                directoryPath = +ClientId + "/" + SubClientId + "/" + OrderId + "/" + File_Name;
                CreateDirectory(directoryPath);
                string file = report_pdf_Path;
                FileInfo f = new FileInfo(file);
                File_size = GetFileSize(f.Length);
                // @"\\192.168.12.33\oms\" + ClientId + @"\" + SubClientId + @"\" + OrderId.ToString())
                ftpfullpath = "ftp://" + Ftp_Domain_Name + "/FTP_Application_Files/OMS/Oms_reports/" + directoryPath;
                string ftpUploadFullPath = ftpfullpath + "/" + "Checklist_Report_Preview.pdf";
                // Checking File Exit or not
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpfullpath); // FTP Address  
                ftpRequest.Credentials = new NetworkCredential(@"" + Ftp_User_Name + "", Ftp_Password); // Credentials  
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                StreamReader streamReader = new StreamReader(response.GetResponseStream());

                HashSet<string> files = new HashSet<string>(); // create list to store directories.   
                string line = streamReader.ReadLine();
                while (!string.IsNullOrEmpty(line))
                {
                    files.Add(line); // Add Each file to the List.  
                    line = streamReader.ReadLine();
                }
                //if (!files.Contains(f.Name))
                //{
                   FtpWebRequest ftpUpLoadFile = (FtpWebRequest)WebRequest.Create(ftpUploadFullPath);
                    ftpUpLoadFile.Credentials = new NetworkCredential(@"" + Ftp_User_Name + "", Ftp_Password);
                    ftpUpLoadFile.KeepAlive = true;
                    ftpUpLoadFile.UseBinary = true;
                    ftpUpLoadFile.Method = WebRequestMethods.Ftp.UploadFile;
                    FileStream fs = File.OpenRead(file);
                    Stream ftpstream = ftpUpLoadFile.GetRequestStream();
                    fs.CopyTo(ftpstream);
                    fs.Close();
                    ftpstream.Close();
               // }

                var dict = new Dictionary<string, object>();
                {
                    dict.Add("@Trans", "INSERT_DOCUMENT");
                    if (WorkType_Id == 1)
                    {
                        dict.Add("@Instuction", "" + OrderTask.ToString() + "Check List Report");
                    }
                    else if (WorkType_Id == 2)
                    {
                        dict.Add("@Instuction", "REWORK -" + OrderTask.ToString() + "Check List Report");

                    }
                    else if (WorkType_Id == 2)
                    {
                        dict.Add("@Instuction", "SUPER QC -" + OrderTask.ToString() + "Check List Report");

                    }
                    dict.Add("@Order_ID", OrderId);
                    dict.Add("@Document_Name", File_Name);
                    dict.Add("@Document_Path", ftpUploadFullPath);
                    dict.Add("@Inserted_By", UserId);
                    dict.Add("@Inserted_date", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));
                    dict.Add("@Project_Type_Id", ProjectType_Id);
                    dict.Add("@Work_Type", WorkType_Id);
                    dict.Add("@Status", "True");
                    dict.Add("@Chk_UploadPackage", "False");
                    dict.Add("@Chk_Typing_Package", "False");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response1 = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/InsertDocument", data);
                    if (response1.IsSuccessStatusCode)
                    {
                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response1.Content.ReadAsStringAsync();
                        }
                    }
                }
                //StiReport Report = new StiReport();
                //Report.Reset();
                //Report.Dictionary.DataSources.Clear();
                //Report.Load(@"C:\\Temp\\Checklist_Report_Preview.mrt");
                //Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Id"].Value = Convert.ToString(OrderId);
                //Report.DataSources["Usp_CheckList_Report"].Parameters["@Order_Task"].Value = Convert.ToString(OrderTask);
                //Report.DataSources["Usp_CheckList_Report"].Parameters["@Work_Type"].Value = Convert.ToString(WorkType_Id);
                //Report.DataSources["Usp_CheckList_Report"].Parameters["@Project_Type_Id"].Value = Convert.ToString(ProjectType_Id);
                //Report.DataSources["Usp_CheckList_Report"].Parameters["@User_Id"].Value = Convert.ToString(UserId);
                //Report.Compile();
                //Report.Render();
                //Report.ExportDocument(StiExportFormat.Pdf, Path);                    
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("CheckList Submitted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                System.Diagnostics.Process.Start(report_pdf_Path);
                this.Close();
            }
            catch(Exception ex)
            { }        
        }
        private bool IsAvailable(string file)
        {
            using (File.OpenRead(file))
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("The Action can't be completed because the file is open." + "\n\t\t\t" + "Close the file and try again.", "File In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }         
            return true;
        }
          
        private bool Validate()
        {

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                bool val_Yes = (bool)gridView1.GetRowCellValue(i, "Yes");
                bool val_No = (bool)gridView1.GetRowCellValue(i, "No");
                string Comments = gridView1.GetRowCellValue(i, "Comments").ToString();
                if (val_Yes == true || val_No == true)
                {
                    if (val_Yes == true && val_No == true)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Select Either 'Yes' or 'No' ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else if (val_Yes == false && val_No == true && string.IsNullOrWhiteSpace(Comments))
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Enter Comment in Red ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                else 
                {
                    SplashScreenManager.CloseForm(false);                  
                    XtraMessageBox.Show("Please Select Either 'Yes' or 'No' For All Rows ", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
       
            return true;
        }

        //private async void  BindPreviousData(int _tabid, int _OrderTask,int _OrderTypeAbs_Id,int _ProjectType_Id,int _ClientId,int _SubClientId)
        //  {
        //      try
        //      {
        //          SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //          var dictionary = new Dictionary<string, object>()
        //              {
        //                  { "@Trans", "BIND_PREVIOUS_DATA"},
        //                  { "@Ref_Checklist_Master_Type_Id", _tabid },
        //                  { "@Order_Task", _OrderTask},
        //                  { "@OrderType_ABS_Id", _OrderTypeAbs_Id },
        //                  { "@Project_Type_Id", _ProjectType_Id },
        //                  { "@Client_Id", _ClientId },
        //                  { "@Sub_Client_Id", _SubClientId }
        //              };
        //          var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
        //          using (var httpClient = new HttpClient())
        //          {
        //              var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/BindPreviousData", data);

        //              if (response.IsSuccessStatusCode)
        //              {
        //                  if (response.StatusCode == HttpStatusCode.OK)
        //                  {
        //                      var result = await response.Content.ReadAsStringAsync();
        //                      DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);                         
        //                      if (dt != null && dt.Rows.Count > 0)
        //                      {                               
        //                          grd_CheckList.Visible = true;
        //                          grd_CheckList.DataSource = dt; 
        //                          grd_CheckList.Dock = DockStyle.Fill;
        //                          tabPane1.SelectedPage.Controls.Add(grd_CheckList);
        //                      }
        //                  }
        //              }
        //          }
        //      }
        //      catch (Exception ex)
        //      {
        //          SplashScreenManager.CloseForm(false);
        //          XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //      }
        //      finally
        //      {
        //          SplashScreenManager.CloseForm(false);
        //      }
        //  }

        private async void SavePreviousData()
        {
            IsButton = true;
            try
            {
                if (grd_CheckList.DataSource != null)
                {
                    DataTable dtinsert = new DataTable();
                    dtinsert.Columns.AddRange(new DataColumn[19]
                         {
                     new DataColumn("Checklist_Id",typeof(int)),
                     new DataColumn("Ref_Checklist_Master_Type_Id",typeof(int)),
                     new DataColumn("Question",typeof(string)),
                     new DataColumn("Yes",typeof(bool)),
                     new DataColumn("No",typeof(bool)),
                     new DataColumn("Order_Id",typeof(int)),
                     new DataColumn("Order_Task",typeof(int)),
                     new DataColumn("Order_Type_Abs_Id",typeof(int)),
                     new DataColumn("Work_Type",typeof(int)),
                     new DataColumn("Comments",typeof(string)),
                     new DataColumn("User_id",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Client_Id",typeof(int)),
                     new DataColumn("Sub_Client_Id",typeof(int))
                         });
                    for (int i = 0; i < gridView1.DataRowCount; i++)
                    {
                        bool chk_yes = (bool)(gridView1.GetRowCellValue(i, "Yes"));
                        bool chk_no = (bool)(gridView1.GetRowCellValue(i, "No"));
                        Comments = gridView1.GetRowCellValue(i, "Comments").ToString();
                        Ref_Checklist_Master_Type_Id = int.Parse(gridView1.GetRowCellValue(i, "Ref_Checklist_Master_Type_Id").ToString());
                        Checklist_Id = int.Parse(gridView1.GetRowCellValue(i, "Checklist_Id").ToString());
                        Question = (gridView1.GetRowCellValue(i, "Question").ToString());

                        dtinsert.Rows.Add(Checklist_Id, Ref_Checklist_Master_Type_Id, Question, chk_yes, chk_no, OrderId,
                                          OrderTask, OrderTypeAbs_Id, WorkType_Id, Comments, UserId, UserId,
                                          DateTime.Now, UserId, DateTime.Now, ProjectType_Id, "True", ClientId, SubClientId);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/CheckLists/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

    }
}