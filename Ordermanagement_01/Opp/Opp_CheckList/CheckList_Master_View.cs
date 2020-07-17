using DevExpress.Utils.DragDrop;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections;
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
        int ProductTypeValueForQs;
        string TabNameValue;
        private string chkListTypeValue;
        int CheckListValueForQs;
        int ProjectID;
        const string OrderFieldName = "ChecklistType_Id";
        GridHitInfo downHitInfo = null;

        public string Btn_Name { get; private set; }
        public string OperType { get; private set; }
        public string QuestionValue { get; private set; }
        Classes.Load_Progres form_loader = new Classes.Load_Progres();
        //private GridHitInfo downHitInfo;
        public CheckList_Master_View()
        {
            InitializeComponent();
            HandleBehaviorDragDropEvents();
        }      
        private void CheckList_Master_View_Load(object sender, EventArgs e)
        {
            tile_CheckList_Master.Checked = true;
            BindCheckListTypeMaster();            
            navigationFrame1.SelectedPage = navigationPage1;
            btn_multiselect.Visible = false;
            panel1.Visible = false;
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
            tile_TabSettings.Checked = false;
            navigationFrame1.SelectedPage = navigationPage1;
            BindCheckListTypeMaster();
            flowLayoutPanel1.Visible = true;
            panel1.Visible = false;
            Clear();         
        }
        private void tile_Question_SetUp_ItemClick(object sender, TileItemEventArgs e)
        {
            tile_Question_SetUp.Checked = true;
            tile_CheckList_Master.Checked = false;
            tile_TabSettings.Checked = false;
            BindCheckListQuesSetUpTab();
            navigationFrame1.SelectedPage = navigationPage2;
            flowLayoutPanel1.Visible = true;
            panel1.Visible = false;
            Clear();
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
                    ProductTypeAbbrValue = int.Parse(row["Product_Type_Abbr_Id"].ToString());
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
        private void gridChkQuestionSetup_Click(object sender, EventArgs e)
        {

        }
        private void gridViewCheckListMaster_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridViewCheckListMaster.SelectedRowsCount != 0)
            {
                btn_multiselect.Visible = true;
            }
            else
            {
                btn_multiselect.Visible = false;
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
                    ProductTypeAbbrValue = int.Parse(row["Product_Type_Abbr_Id"].ToString());
                    ProductTypeValueForQs = int.Parse(row["Product_Type_Abbr_Id"].ToString());
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
            if (gridviewChkQuestionSetup.SelectedRowsCount != 0)
            {
                btn_multiselect.Visible = true;

            }
            else
            {
                btn_multiselect.Visible = false;
            }
        }
           
        private async void btn_multiselect_Click_1(object sender, EventArgs e)
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
                else if (tile_Question_SetUp.Checked == true)
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
        private void btn_Add_Click_1(object sender, EventArgs e)
        {
            if (tile_CheckList_Master.Checked == true)
            {
                OperType = "CheckListMaster";
                Btn_Name = "Save";
                this.Enabled = false;
                Ordermanagement_01.Opp.Opp_CheckList.ChekList_Master_Entry me = new ChekList_Master_Entry(OperType, Btn_Name, ProjectIdValue, chklIstMasterTypeIdValue, ProductTypeAbbrValue, chkListTypeValue, this);
                me.Show();
            }
            else if (tile_Question_SetUp.Checked == true)
            {
                OperType = "QuestionSetUp";
                Btn_Name = "Save";
                this.Enabled = false;
                Ordermanagement_01.Opp.Opp_CheckList.CheckList_Question_Setup_Entry Qs = new CheckList_Question_Setup_Entry(OperType, Btn_Name, ProjectIdValue, chklIstMasterTypeIdValue, ProductTypeAbbrValue, ProductTypeValueForQs, CheckListValueForQs, QuestionValue, this);
                Qs.Show();
            }
        }
        private void btn_Export_Click_1(object sender, EventArgs e)
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
        private void tile_ItemClick(object sender, TileItemEventArgs e)
        {
            tile_Question_SetUp.Checked = false;
            tile_CheckList_Master.Checked = false;
            tile_TabSettings.Checked = true;
            navigationFrame1.SelectedPage = navigationPage3;
            ddl_ProductType.Enabled = false;
            flowLayoutPanel1.Visible = false;
            BindProjectType();
            panel1.Visible = true;         
        }
        public async void BindProjectType()
        {
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                ddl_ProjectType.Properties.Columns.Clear();
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","Get_Project_Type" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindProject", data);
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
        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
               // ddl_ProjectType.EditValue = null;
                ddl_ProductType.EditValue = null;
                int ProjectID = Convert.ToInt32(ddl_ProjectType.EditValue);
                ddl_ProductType.Enabled = true;
                ddl_ProductType.EditValue = null;
                grd_TabSetting.DataSource = null;
                BindProductType(ProjectID);
            }
            else
            {
                grd_TabSetting.DataSource = null;
                ddl_ProjectType.EditValue = 0;
            }
        }
        private async void BindProductType(int _ProjectID)
        {
            try
            {
                ddl_ProductType.Properties.Columns.Clear();
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict = new Dictionary<string, object>()
                {
                    {"@Trans" ,"SELECT_ORDER_ABS"},
                    {"@Project_Type_Id",_ProjectID }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindProductTypeAbs", data);
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
                                ddl_ProductType.Properties.DataSource = dt;
                                ddl_ProductType.Properties.DisplayMember = "Order_Type_Abbreviation";
                                ddl_ProductType.Properties.ValueMember = "OrderType_ABS_Id";
                                DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                                col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Order_Type_Abbreviation");
                                ddl_ProductType.Properties.Columns.Add(col);
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
        private void ddl_ProductType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
                ddl_ProductType.EditValue = 0;
                int ProducttID = Convert.ToInt32(ddl_ProductType.EditValue);
                int ProjectID = Convert.ToInt32(ddl_ProjectType.EditValue);
                BindGridTabSetting(ProjectID, ProducttID);
            }
            else
            {
                grd_TabSetting.DataSource = null;
                ddl_ProductType.EditValue = 0;
            }
        }
        public async void BindGridTabSetting(int Proj_ID, int Prod_ID)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_GRID_TAB_SETTING" },
                    {"@Project_Type_Id ",Proj_ID  },
                    {"@Product_Type_Abbr_Id",Prod_ID }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindGridTabSetting", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                grd_TabSetting.DataSource = dt;
                                gridView_TabSetting.BestFitColumns();
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
        public void HandleBehaviorDragDropEvents()
        {
            DragDropBehavior gridControlBehavior = behaviorManager1.GetBehavior<DragDropBehavior>(this.gridView_TabSetting);
            gridControlBehavior.DragDrop += Behavior_DragDrop;
            gridControlBehavior.DragOver += Behavior_DragOver;
        }
        private void Behavior_DragOver(object sender, DragOverEventArgs e)
        {
            DragOverGridEventArgs args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
            e.InsertType = args.InsertType;
            e.InsertIndicatorLocation = args.InsertIndicatorLocation;
            e.Action = args.Action;
            Cursor.Current = args.Cursor;
            args.Handled = true;           
        }
        private void Behavior_DragDrop(object sender, DevExpress.Utils.DragDrop.DragDropEventArgs e)
        {
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                GridView targetGrid = e.Target as GridView;
                GridView sourceGrid = e.Source as GridView;
                if (e.Action == DragDropActions.None || targetGrid != sourceGrid)
                    return;
                DataTable sourceTable = sourceGrid.GridControl.DataSource as DataTable;

                Point hitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
                GridHitInfo hitInfo = targetGrid.CalcHitInfo(hitPoint);

                int[] sourceHandles = e.GetData<int[]>();

                int targetRowHandle = hitInfo.RowHandle;
                int targetRowIndex = targetGrid.GetDataSourceRowIndex(targetRowHandle);

                List<DataRow> draggedRows = new List<DataRow>();
                foreach (int sourceHandle in sourceHandles)
                {
                    int oldRowIndex = sourceGrid.GetDataSourceRowIndex(sourceHandle);
                    DataRow oldRow = sourceTable.Rows[oldRowIndex];
                    draggedRows.Add(oldRow);
                }

                int newRowIndex;

                switch (e.InsertType)
                {
                    case InsertType.Before:
                        newRowIndex = targetRowIndex > sourceHandles[sourceHandles.Length - 1] ? targetRowIndex - 1 : targetRowIndex;
                        for (int i = draggedRows.Count - 1; i >= 0; i--)
                        {
                            DataRow oldRow = draggedRows[i];
                            DataRow newRow = sourceTable.NewRow();
                            newRow.ItemArray = oldRow.ItemArray;
                            sourceTable.Rows.Remove(oldRow);
                            sourceTable.Rows.InsertAt(newRow, newRowIndex);
                        }
                        break;
                    case InsertType.After:
                        newRowIndex = targetRowIndex < sourceHandles[0] ? targetRowIndex + 1 : targetRowIndex;
                        for (int i = 0; i < draggedRows.Count; i++)
                        {
                            DataRow oldRow = draggedRows[i];
                            DataRow newRow = sourceTable.NewRow();
                            newRow.ItemArray = oldRow.ItemArray;
                            sourceTable.Rows.Remove(oldRow);
                            sourceTable.Rows.InsertAt(newRow, newRowIndex);
                        }
                        break;
                    default:
                        newRowIndex = -1;
                        break;
                }
                int insertedIndex = targetGrid.GetRowHandle(newRowIndex);
                targetGrid.FocusedRowHandle = insertedIndex;
                targetGrid.SelectRow(targetGrid.FocusedRowHandle);
                Update();
            }
            catch(Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Arrange Rows Properly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void gridView_TabSetting_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }
        private void Clear()
        {
            ddl_ProductType.EditValue = null;
            ddl_ProjectType.EditValue = null;
            grd_TabSetting.DataSource = null;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                DevExpress.XtraGrid.Columns.GridColumn col = gridView_TabSetting.Columns.ColumnByFieldName("ChecklistType_Id");
                ArrayList aL = new ArrayList();
                for (int i = 0; i < gridView_TabSetting.DataRowCount; i++)
                {
                    aL.Add(gridView_TabSetting.GetRowCellValue(i, col));
                }
                int preference = 1;
                foreach (var chk_Id in aL)
                {
                    UpdatePreference(Convert.ToInt32(chk_Id), preference);
                    preference += 1;
                }
                int _ProjID = Convert.ToInt32(ddl_ProjectType.EditValue);
                int _ProdID = Convert.ToInt32(ddl_ProductType.EditValue);
                this.BindGridTabSetting(_ProjID, _ProdID);
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Rows Updated Sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);              
                Clear();
             //   btnUpdate.Visible = false;
                ddl_ProductType.Enabled = false;
            }
            catch
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }          
        }
        private void Update()
        {
            DevExpress.XtraGrid.Columns.GridColumn col = gridView_TabSetting.Columns.ColumnByFieldName("ChecklistType_Id");
            ArrayList aL = new ArrayList();
            for (int i = 0; i < gridView_TabSetting.DataRowCount; i++)
            {
                aL.Add(gridView_TabSetting.GetRowCellValue(i, col));
            }
            int preference = 1;
            foreach (var chk_Id in aL)
            {
                UpdatePreference(Convert.ToInt32(chk_Id), preference);
                preference += 1;
            }
            int _ProjID = Convert.ToInt32(ddl_ProjectType.EditValue);
            int _ProdID = Convert.ToInt32(ddl_ProductType.EditValue);
            this.BindGridTabSetting(_ProjID, _ProdID);
        }
        private async void UpdatePreference(int locationId, int preference)
        {
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);               
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","UPDATE_REORDER_ROWS" },
                    {"@ID ",locationId },
                    {"@Preference ",preference }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CheckListMaster/BindReorderedRows", data);
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
      
    }
}



