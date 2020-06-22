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
using WaitForm_SetDescription;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using System.IO;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Sub_Product_Type_View : DevExpress.XtraEditors.XtraForm
    {
        string Operation_Type;
        int Project_Id,ID;
        int _UserId;
        int User_Id;
        string operation, _subproductType, _btnName;
        int _projectId, _productId, _subproductTypeAbsId;

        public Sub_Product_Type_View(int User_Id)
        {
            InitializeComponent();
            _UserId = User_Id;
        }

        private void Sub_Product_Type_View_Load(object sender, EventArgs e)
        {
            btn_Delete_Multiple.Visible = false;
            BindGridType();
            BindGridAbs();
            Tile_Item_ProductType.Checked = true;
            navigationFrame1.SelectedPage = navigationPage1;
        }
       
        public async void BindGridType()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT_SUB_PRODUCT_TYPE"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/BindGridType", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Type.DataSource = dt;
                            }
                            else
                            {
                                grd_Type.DataSource = null;
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
        public async void BindGridAbs()
        {         
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT_SUB_PRODUCT_TYPE_ABS"},                       
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/BindGridAbs", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                grd_Abs.DataSource = dt;
                            }
                            else
                            {
                                grd_Abs.DataSource = null;
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
      
        private async void btn_Delete_MultipleType_Click_1(object sender, EventArgs e)
        {
            if(Tile_Item_ProductType.Checked==true)
            {

                if (gridView_Type.SelectedRowsCount != 0)
                {
                    DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (show == DialogResult.Yes)
                    {
                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            List<int> gridViewSelectedRows = gridView_Type.GetSelectedRows().ToList();
                            for (int i = 0; i < gridViewSelectedRows.Count; i++)
                            {
                                DataRow row = gridView_Type.GetDataRow(gridViewSelectedRows[i]);
                                int OrderTypeId = int.Parse(row["Order_Type_ID"].ToString());
                                var dictionary = new Dictionary<string, object>()
                                {
                                      { "@Trans", "DELETE_TYPE" },
                                      { "@Order_Type_ID", OrderTypeId }
                                };
                                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/Delete", data);
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
                            BindGridType();
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
                else
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Please Select Any Record To Delete");
                }
            }
            else if(Tile_Item_ProductAbs.Checked==true)
            {

                if (gridView_Abs.SelectedRowsCount != 0)
                {
                    DialogResult show = XtraMessageBox.Show("Do you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (show == DialogResult.Yes)
                    {
                        try
                        {
                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                            List<int> gridViewSelectedRows = gridView_Abs.GetSelectedRows().ToList();
                            for (int i = 0; i < gridViewSelectedRows.Count; i++)
                            {
                                DataRow row = gridView_Abs.GetDataRow(gridViewSelectedRows[i]);
                                int OrderTypeAbsId = int.Parse(row["OrderType_ABS_Id"].ToString());
                                var dictionary = new Dictionary<string, object>()
                                {
                                      { "@Trans", "DELETE_ABS" },
                                      { "@OrderType_ABS_Id", OrderTypeAbsId }
                                };
                                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response = await httpClient.PostAsync(Base_Url.Url + "/SubProductType/Delete", data);
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
                            BindGridType();
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
                else
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Please Select Any Record To Delete");
                }
            }
        }

        public void btn_Add_New_Type_Click_1(object sender, EventArgs e)
        {
            if(Tile_Item_ProductType.Checked==true)
            {
                Operation_Type = "Sub Product Type";
                Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SPT = new Sub_Product_Type_Entry(Operation_Type, operation, _projectId, _productId, _subproductType, _subproductTypeAbsId, _btnName, _UserId,ID, this);
                SPT.Show();
            }
            else if(Tile_Item_ProductAbs.Checked==true)
            {
                Operation_Type = "Sub Product Type Abbreviation";
                Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SPT = new Sub_Product_Type_Entry(Operation_Type, operation, _projectId, _productId, _subproductType, _subproductTypeAbsId, _btnName, _UserId,ID, this);
                SPT.Show();
            }
        }

        private void btn_Export_Type_Click_1(object sender, EventArgs e)
        {
            if(Tile_Item_ProductType.Checked==true)
            {
                string filePath = @"C:\Sub Product Type\";
                string fileName = filePath + "Sub Product Type-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                grd_Type.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else if(Tile_Item_ProductAbs.Checked==true)
            {
                string filePath = @"C:\Sub Product Type Abbreviation\";
                string fileName = filePath + "Sub Product Type Abbreviation-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                grd_Abs.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }

        }

        private void Tile_Item_ProductType_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ProductType.Checked = true;           
            Tile_Item_ProductAbs.Checked = false;
            navigationFrame1.SelectedPage = navigationPage1;
            BindGridType();
            Operation_Type = "Sub Product Type";
        }

        private void Tile_Item_ProductAbs_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ProductAbs.Checked = true;      
            Tile_Item_ProductType.Checked = false;
            navigationFrame1.SelectedPage = navigationPage2;
            BindGridAbs();
            Operation_Type = "Sub Product Type Abbreviation";
        }

        private void gridView_Type_CustomDrawRowIndicator_1(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView_Abs_CustomDrawRowIndicator_1(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView_Type_SelectionChanged_1(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {

            if (gridView_Type.SelectedRowsCount != 0)
            {
                btn_Delete_Multiple.Visible = true;
            }
            else if (gridView_Type.SelectedRowsCount == 0)
            {
                btn_Delete_Multiple.Visible = false;
            }
        }

        private void gridView_Abs_SelectionChanged_1(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView_Abs.SelectedRowsCount != 0)
            {
                btn_Delete_Multiple.Visible = true;
            }
            else if (gridView_Abs.SelectedRowsCount == 0)
            {
                btn_Delete_Multiple.Visible = false;
            }
        }

        private void repositoryItemHyperLinkEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridView_Type.GetDataRow(gridView_Type.FocusedRowHandle);
            string _btnName = "Edit";
            int _projectId = int.Parse(row["Project_Type_Id"].ToString());
            int _productId = int.Parse(row["ProductType_Id"].ToString());
            string _subproductType = row["Order_Type"].ToString();
            int _subproductTypeAbsId = int.Parse(row["OrderType_ABS_Id"].ToString());
            int user_Id = _UserId;
            string operation = "View";
            string Operation_Type = "Sub Product Type";
            int ID = int.Parse(row["Order_Type_ID"].ToString());
            Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SourceEntry = new Sub_Product_Type_Entry(Operation_Type,operation, _projectId, _productId, _subproductType, _subproductTypeAbsId,_btnName, user_Id,ID, this);
            SourceEntry.Show();
        }

        private void repositoryItemHyperLinkEdit_Abs_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridView_Abs.GetDataRow(gridView_Abs.FocusedRowHandle);
            string _btnName = "Edit";
            int _projectId = int.Parse(row["Project_Type_Id"].ToString());
            int _productId = int.Parse(row["ProductType_Id"].ToString());
            string _subproductType = row["Order_Type_Abbreviation"].ToString();            
            int user_Id = _UserId;
            string operation = "View";
            int _subproductTypeid=0;
            string Operation_Type = "Sub Product Type Abbreviation";
            int ID = int.Parse(row["OrderType_ABS_Id"].ToString());
            Ordermanagement_01.Opp.Opp_Master.Sub_Product_Type_Entry SourceEntry = new Sub_Product_Type_Entry(Operation_Type, operation, _projectId, _productId, _subproductType, _subproductTypeid, _btnName, user_Id,ID, this);
            SourceEntry.Show();
        }
    }
}