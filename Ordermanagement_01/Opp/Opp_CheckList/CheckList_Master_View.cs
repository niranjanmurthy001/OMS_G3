using DevExpress.XtraBars.Navigation;
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

namespace Ordermanagement_01.Opp.Opp_CheckList
{
    public partial class CheckList_Master_View : XtraForm
    {
        int chklIstMasterTypeIdValue;
        int ProjectIdValue;
       int ProductTypeAbbrValue;
        string ProductTypeValueForQs;
        string TabNameValue;
        private string chkListTypeValue;
        int CheckListValueForQs;

        public string Btn_Name { get; private set; }
        public string OperType { get; private set; }
        public string QuestionValue { get; private set; }

        Classes.Load_Progres form_loader = new Classes.Load_Progres();
        public CheckList_Master_View()
        {
            InitializeComponent();
        }

        private void CheckList_Master_View_Load(object sender, EventArgs e)
        {
            BindCheckListTypeMaster();
            BindCheckListQuesSetUpTab();
            navigationFrame1.SelectedPage = navigationPage1;
            btn_multiselect.Visible = false;
        }

       

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (tile_CheckList_Master.Checked == true)
            {
                OperType = "CheckListMaster";
                Btn_Name = "Save";
                this.Enabled = false;
                Ordermanagement_01.Opp.Opp_CheckList.ChekList_Master_Entry me = new ChekList_Master_Entry(OperType,Btn_Name,ProjectIdValue,chklIstMasterTypeIdValue,ProductTypeAbbrValue,chkListTypeValue,this);
                me.Show();
            }
            else if (tile_Question_SetUp.Checked == true)
            {
                OperType = "QuestionSetUp";
                Btn_Name = "Save";
                this.Enabled = false;
                Ordermanagement_01.Opp.Opp_CheckList.CheckList_Question_Setup_Entry Qs = new CheckList_Question_Setup_Entry( OperType,  Btn_Name,  ProjectIdValue, chklIstMasterTypeIdValue,  ProductTypeAbbrValue, ProductTypeValueForQs, CheckListValueForQs,  QuestionValue,  this);
                Qs.Show();
            }
        }

        public async void BindCheckListTypeMaster()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SelectCheckListMasterDetails" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindMastersGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {

                                gridCheckListMasterDetails.DataSource = dt;
                                gridViewCheckListMaster.BestFitColumns();


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong ! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void tile_CheckList_Master_ItemClick(object sender, TileItemEventArgs e)
        {
            tile_Question_SetUp.Checked = false;
            tile_CheckList_Master.Checked = true;

            navigationFrame1.SelectedPage = navigationPage1;
            BindCheckListTypeMaster();
        }

        private void tile_Question_SetUp_ItemClick(object sender, TileItemEventArgs e)
        {
            tile_Question_SetUp.Checked = true;
            tile_CheckList_Master.Checked = false;
            BindCheckListQuesSetUpTab();
            navigationFrame1.SelectedPage = navigationPage2;

        }
        public async void BindCheckListQuesSetUpTab()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SelectCheckListTypeData"},

                };

                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindGridQuestionSetup", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                gridChkQuestionSetup.DataSource = dt;
                                gridColProjTypeQs.Width = 15;
                                gridColProductTypeAbbrQs.Width = 15;
                                gridColChkTypeQs.Width = 30;
                                gridColQuestionQs.Width = 130;
                                gridColDeleteQs.Width = 5;
                                gridColViewQs.Width = 5;


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong ! Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        

        private void gridCheckListMaster_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridviewChkQuestionSetup_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private async void gridCheckListMaster_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "View")
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    System.Data.DataRow row = gridViewCheckListMaster.GetDataRow(gridViewCheckListMaster.FocusedRowHandle);
                    ProjectIdValue = int.Parse(row["Project_Type_Id"].ToString());
                    chklIstMasterTypeIdValue = int.Parse(row["ChecklistType_Id"].ToString());
                    ProductTypeAbbrValue = int.Parse(row["OrderType_ABS_Id"].ToString());
                    chkListTypeValue = row["Checklist_Master_Type"].ToString();

                    Btn_Name = "Edit";
                    OperType = "Update";
                    this.Enabled = false;
                    Ordermanagement_01.Opp.Opp_CheckList.ChekList_Master_Entry cm = new ChekList_Master_Entry(OperType, Btn_Name, ProjectIdValue, chklIstMasterTypeIdValue, ProductTypeAbbrValue, chkListTypeValue, this);
                    cm.Show();



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
                        System.Data.DataRow row = gridViewCheckListMaster.GetDataRow(gridViewCheckListMaster.FocusedRowHandle);
                        int chklistvalue = int.Parse(row["ChecklistType_Id"].ToString());
                        var dictionary = new Dictionary<string, object>()
                  {
                        { "@Trans", "DELETEChkListMaster"},
                       { "@Checklist_Id", chklistvalue },

                  };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/DeleteChkMasterDetails", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Record Deleted Successfully");
                                    BindCheckListTypeMaster();

                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Please Select Question To Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something Went Wrong","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
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

        private void gridChkQuestionSetup_Click(object sender, EventArgs e)
        {

        }

        private void gridViewCheckListMaster_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridViewCheckListMaster.SelectedRowsCount!=0)
            {
                btn_multiselect.Visible = true;
            }
            else
            {
                btn_multiselect.Visible = false;
            }
        }

        private async void btn_multiselect_Click(object sender, EventArgs e)
        {
           

                DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (show == DialogResult.Yes)
                {
                    if (tile_CheckList_Master.Checked == true)
                    {
                     if (gridViewCheckListMaster.SelectedRowsCount != 0)
                      {
                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            List<int> gridViewSelectedRows = gridViewCheckListMaster.GetSelectedRows().ToList();
                            for (int i = 0; i < gridViewSelectedRows.Count; i++)
                            {
                                DataRow row = gridViewCheckListMaster.GetDataRow(gridViewSelectedRows[i]);
                                int chkvalue = int.Parse(row["ChecklistType_Id"].ToString());
                                var dictionary = new Dictionary<string, object>()
                                 {
                                   { "@Trans", "DELETEChkListMaster"},
                                     { "@Checklist_Id", chkvalue },

                                };
                                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/DeleteChkMasterDetails", data);
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
                            btn_multiselect.Visible = false;
                            BindCheckListTypeMaster();
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
                     else
                      {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Select Any Record To Delete");
                      }
                    }
                    else if(tile_Question_SetUp.Checked==true)
                    {

                    if (gridviewChkQuestionSetup.SelectedRowsCount != 0)
                    {
                        try
                        {

                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                            List<int> gridViewSelectedRows = gridviewChkQuestionSetup.GetSelectedRows().ToList();
                            for (int i = 0; i < gridViewSelectedRows.Count; i++)
                            {
                                DataRow row = gridviewChkQuestionSetup.GetDataRow(gridViewSelectedRows[i]);
                                int chklistvalue = int.Parse(row["Checklist_Id"].ToString());
                                var dictionary = new Dictionary<string, object>()
                               {
                                 { "@Trans", "DELETE_CHECKLIST"},
                                 { "@Checklist_Id", chklistvalue },

                               };
                                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/DeleteChklist", data);
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
                            XtraMessageBox.Show("Record Deleted Successfully");
                            btn_multiselect.Visible = false;
                            BindCheckListQuesSetUpTab();
                        }

                        catch (Exception ex)
                        {
                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //throw ex;
                        }
                        finally
                        {
                            SplashScreenManager.CloseForm(false);
                        }
                    }
                     else
                     {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Please Select Any Record To Delete");
                     }


                  }
                }
                else if (show == DialogResult.No)
                {

                }
            
           
        }

        private async void gridviewChkQuestionSetup_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Column.Caption == "View")
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    System.Data.DataRow row = gridviewChkQuestionSetup.GetDataRow(gridviewChkQuestionSetup.FocusedRowHandle);
                    ProjectIdValue = int.Parse(row["Project_Type_Id"].ToString());
                    chklIstMasterTypeIdValue = int.Parse(row["Checklist_Id"].ToString());
                    ProductTypeAbbrValue = int.Parse(row["OrderType_ABS_Id"].ToString());
                    ProductTypeValueForQs = row["Product_Type_Abbr"].ToString();
                    CheckListValueForQs = int.Parse(row["ChecklistType_Id"].ToString());
                    QuestionValue = row["Question"].ToString();

                    Btn_Name = "Edit";
                    OperType = "Update";
                    this.Enabled = false;
                    Ordermanagement_01.Opp.Opp_CheckList.CheckList_Question_Setup_Entry qse = new CheckList_Question_Setup_Entry(OperType, Btn_Name, ProjectIdValue, chklIstMasterTypeIdValue, ProductTypeAbbrValue, ProductTypeValueForQs, CheckListValueForQs, QuestionValue, this);
                    qse.Show();



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
                        System.Data.DataRow row = gridviewChkQuestionSetup.GetDataRow(gridviewChkQuestionSetup.FocusedRowHandle);
                        int chklistvalue = int.Parse(row["Checklist_Id"].ToString());
                        var dictionary = new Dictionary<string, object>()
                  {
                        { "@Trans", "DELETE_CHECKLIST"},
                       { "@Checklist_Id", chklistvalue },

                  };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CheckListMaster/DeleteChklist", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Record Deleted Successfully");
                                    BindCheckListQuesSetUpTab();

                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Please Select Question To Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void gridviewChkQuestionSetup_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridviewChkQuestionSetup.SelectedRowsCount!=0)
            {
                btn_multiselect.Visible = true;

            }
            else
            {
                btn_multiselect.Visible = false;
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            if (tile_CheckList_Master.Checked == true)
            {
                string filePath = @"C:\Temp\";
                string fileName = filePath + "CheckList Master Type Details-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                gridViewCheckListMaster.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else if (tile_Question_SetUp.Checked == true)
            {
                string filePath = @"C:\Temp\";
                string fileName = filePath + "ChecKList Master Question SetUp Details-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                gridviewChkQuestionSetup.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
        }
    }
}
    

