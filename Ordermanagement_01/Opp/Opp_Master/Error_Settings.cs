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
using System.Net;
using Ordermanagement_01.Models;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;


namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Settings : DevExpress.XtraEditors.XtraForm
    {
        string OperationType;
        int User_Id;
        string _btnname;
        int _Projectid;
        int _productid;
        string errortext;
        int checkederror;
        Classes.Load_Progres form_loader = new Classes.Load_Progres();
    
        public Error_Settings()
        {
            InitializeComponent();
        }




        private void Error_Settings_Load(object sender, EventArgs e)
        {

            btn_delete_multiple.Visible = false;
            Tile_Item_ErrorType.Checked = true;
            navigationFrame1.SelectedPage = navigationPage1;
            BindErrorDetails();
            BindErrorGrid();
            Bind_Error_Tab_Grid();
        }
        private void Tile_Item_ErrorType_ItemClick(object sender, TileItemEventArgs e)
        {

            Tile_Item_ErrorType.Checked = true;
            Tile_Item_ErrorTab.Checked = false;
            Tile_Item_ErrorField.Checked = false;
            navigationFrame1.SelectedPage = navigationPage1;
            BindErrorDetails();

        }

        private void Tile_Item_ErrorTab_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorType.Checked = false;
            Tile_Item_ErrorTab.Checked = true;
            Tile_Item_ErrorField.Checked = false;
            navigationFrame1.SelectedPage = navigationPage2;
            Bind_Error_Tab_Grid();

        }

        private void Tile_Item_ErrorField_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorType.Checked = false;
            Tile_Item_ErrorTab.Checked = false;
            Tile_Item_ErrorField.Checked = true;
            navigationFrame1.SelectedPage = navigationPage3;
            BindErrorGrid();

        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_AddError_Click(object sender, EventArgs e)
        {
            if (Tile_Item_ErrorType.Checked == true)
            {
                OperationType = "Error Type";
                string Boxname = "Error Type";
                _btnname = "Submit";
                Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels ErrorTypes = new Error_Settingspanels(OperationType, Boxname, _Projectid, _productid, errortext, _btnname);
                ErrorTypes.Show();

            }
            else if (Tile_Item_ErrorTab.Checked == true)
            {
                OperationType = "Error Tab";
                string _boxname = "Error Tab";
                _btnname = "Submit";
                Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels ErrorTypes = new Error_Settingspanels(OperationType, _boxname, _Projectid, _productid, errortext, _btnname);
                ErrorTypes.Show();
            }
            else if (Tile_Item_ErrorField.Checked == true)
            {
                OperationType = "Error Field";
                Ordermanagement_01.Opp.Opp_Master.Error_Field ErrorField = new Error_Field(OperationType, _btnname, _Projectid, _productid, errortext, checkederror);
                ErrorField.Show();
            }
        }

        private async void BindErrorDetails()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/BindErrorDetails", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dtError = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dtError.Rows.Count > 0)
                            {
                                grd_Error_Type.DataSource = _dtError;
                                // _Inserted_By = Convert.ToInt32(_dtError.Rows[0]["Inserted_By"].ToString());
                                // _Inserted_Date = Convert.ToDateTime(_dtError.Rows[0]["Inserted_Date"].ToString());

                            }
                            else
                            {
                                grd_Error_Type.DataSource = null;
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

        private async void BindErrorGrid()
        {
            try
            {

                //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_Error_description_grd" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/BindErrors", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {

                                Grd_ErrorDes.DataSource = _dt;

                            }
                            else
                            {
                                Grd_ErrorDes.DataSource = null;

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

        private async void Bind_Error_Tab_Grid()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BindErrorDetails" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTabSettings/GridErrorTabDetails", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {
                                grdErrorTab.DataSource = _dt.DefaultView.ToTable(true, _dt.Columns[3].ColumnName, _dt.Columns[1].ColumnName, _dt.Columns[0].ColumnName, _dt.Columns[2].ColumnName, _dt.Columns[5].ColumnName, _dt.Columns[6].ColumnName);
                                gridView1.BestFitColumns();
                            }
                            else
                            {
                                grdErrorTab.DataSource = null;
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

        private async void gridView5_RowCellClick(object sender, RowCellClickEventArgs e)
        {

            if (e.Column.Caption == "Delete")
            {
                try
                {
                    GridView view = Grd_ErrorDes.MainView as GridView;
                    var index = view.GetDataRow(view.GetSelectedRows()[0]);
                    //_pid = Convert.ToInt32(index.ItemArray[5]);
                    int _error_D_id = Convert.ToInt32(index.ItemArray[1]);

                    var dictonary = new Dictionary<string, object>()
                     {
                        {"@Trans","DELETE_Error_description" },

                        {"@Error_description_Id",_error_D_id }

                    };
                    var op = XtraMessageBox.Show("Do You Want to Delete the Error Description", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (op == DialogResult.Yes)
                    {
                        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                        using (var httpclient = new HttpClient())
                        {
                            var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                    XtraMessageBox.Show("Deleted Successfully");
                                    BindErrorGrid();

                                }
                            }
                        }
                    }
                    else
                    {
                        BindErrorGrid();
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            if (e.Column.Caption == "View")
            {
                GridView view = Grd_ErrorDes.MainView as GridView;
                var index = view.GetDataRow(view.GetSelectedRows()[0]);
                //e.Column.ColumnEdit.NullText = "Edit";
                _btnname = "Update";
                OperationType = "";
                _Projectid = Convert.ToInt32(index.ItemArray[7]);
                //int Pro = Convert.ToInt32(ddl_ProjectType.EditValue);
                //BindProdctType(Pro);
                _productid = Convert.ToInt32(index.ItemArray[8]);
                errortext = index.ItemArray[0].ToString();
                checkederror = Convert.ToInt32(index.ItemArray[3]);
                Ordermanagement_01.Opp.Opp_Master.Error_Field _Efield = new Error_Field(OperationType, _btnname, _Projectid, _productid, errortext, checkederror);
                _Efield.Show();

            }
        }


        private void Tile_Item_ErrorType_CheckedChanged(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorField.AppearanceItem.Selected.BackColor = Color.Blue;
        }

        private void repositoryItemHyperLinkEdit9_Click(object sender, EventArgs e)
        {
            GridView view = grd_Error_Type.MainView as GridView;
            var index = view.GetDataRow(view.GetSelectedRows()[0]);
            OperationType = "Error Type";
            _btnname = "Edit";
            _Projectid = Convert.ToInt32(index.ItemArray[1]);
            errortext = index.ItemArray[3].ToString();
            checkederror = Convert.ToInt32(index.ItemArray[2]);
            Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels _Espanels = new Error_Settingspanels(OperationType, "Error Type", _Projectid, checkederror, errortext, _btnname);
            _Espanels.Show();
        }

        private async void repositoryItemHyperLinkEdit10_Click(object sender, EventArgs e)
        {

            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {

                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    int Error_Id = int.Parse(row["New_Error_Type_Id"].ToString());
                    var dictionary = new Dictionary<string, object>()
                {
                   { "@Trans", "DELETE" },
                    { "@New_Error_Type_Id", Error_Id }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                BindErrorDetails();

                            }
                        }
                        else
                        {
                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("Please Select Client To Delete");
                        }
                    }

                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    throw ex;
                }
            }
            else if (show == DialogResult.No)
            {
                this.Close();
            }
        }

        private async void repositoryItemHyperLinkEdit8_Click(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {


                try
                {
                    System.Data.DataRow row = gridView3.GetDataRow(gridView1.FocusedRowHandle);
                    int ID = int.Parse(row["Error_Type_Id"].ToString());
                    var dictionarydelete = new Dictionary<string, object>();
                    {
                        dictionarydelete.Add("@Trans", "DELETE_Error_Type");
                        dictionarydelete.Add("@Error_Type_Id", ID);
                        dictionarydelete.Add("@Modified_By", User_Id);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictionarydelete), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTabSettings/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                Bind_Error_Tab_Grid();


                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else if (show == DialogResult.No)
            {
                this.Close();
            }
        }

        private void repositoryItemHyperLinkEdit7_Click(object sender, EventArgs e)
        {

            GridView view = grdErrorTab.MainView as GridView;
            var index = view.GetDataRow(view.GetSelectedRows()[0]);
            _Projectid = Convert.ToInt32(index.ItemArray[4]);
            errortext = index.ItemArray[3].ToString();
            checkederror = Convert.ToInt32(index.ItemArray[5]);
            _btnname = "Edit";
            OperationType = "Error Tab";

            Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels _etab = new Error_Settingspanels(OperationType, "Error Tab", _Projectid, checkederror, errortext, _btnname);
            _etab.Show();
            //chkProductType.SelectedValue = ProductChk;
            //int _task = chkProductType.SelectedIndex;
            //chkProductType.SetItemChecked(_task, true);
        }



        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView5_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();

        }

        private void gridView3_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private async void btn_delete_multiple_Click(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {

                try
                {

                    if (Tile_Item_ErrorType.Checked == true)
                    {

                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView1.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView1.GetDataRow(gridViewSelectedRows[i]);
                            int Error_Id = int.Parse(row["New_Error_Type_Id"].ToString());
                            var dictionary = new Dictionary<string, object>()
                        {
                            { "@Trans", "DELETE" },
                            { "@New_Error_Type_Id", Error_Id }
                        };
                            var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTypeSettings/Delete", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        SplashScreenManager.CloseForm(false);
                                        XtraMessageBox.Show("Record Deleted Successfully");
                                        BindErrorDetails();

                                    }
                                }
                                else
                                {
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Please Select Client To Delete");
                                }
                            }

                        }
                    }
                    else if (Tile_Item_ErrorTab.Checked == true)
                    {

                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView3.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView3.GetDataRow(gridViewSelectedRows[i]);
                            int ID = int.Parse(row["Error_Type_Id"].ToString());
                            var dictionarydelete = new Dictionary<string, object>();
                            {
                                dictionarydelete.Add("@Trans", "DELETE_Error_Type");
                                dictionarydelete.Add("@Error_Type_Id", ID);
                                dictionarydelete.Add("@Modified_By", User_Id);
                            }
                            var data = new StringContent(JsonConvert.SerializeObject(dictionarydelete), Encoding.UTF8, "application/json");
                            using (var httpClient = new HttpClient())
                            {
                                var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorTabSettings/Delete", data);
                                if (response.IsSuccessStatusCode)
                                {
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result = await response.Content.ReadAsStringAsync();
                                        SplashScreenManager.CloseForm(false);
                                        XtraMessageBox.Show("Record Deleted Successfully");
                                        Bind_Error_Tab_Grid();


                                    }
                                }
                            }
                        }
                    }
                    if (Tile_Item_ErrorField.Checked == true)
                    {

                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        List<int> gridViewSelectedRows = gridView5.GetSelectedRows().ToList();
                        for (int i = 0; i < gridViewSelectedRows.Count; i++)
                        {
                            DataRow row = gridView5.GetDataRow(gridViewSelectedRows[i]);
                            GridView view = Grd_ErrorDes.MainView as GridView;
                            var index = view.GetDataRow(view.GetSelectedRows()[0]);
                            int _error_D_id = Convert.ToInt32(index.ItemArray[1]);

                            var dictonary = new Dictionary<string, object>()
                     {
                        {"@Trans","DELETE_Error_description" },

                        {"@Error_description_Id",_error_D_id }

                    };
                            var op = XtraMessageBox.Show("Do You Want to Delete the Error Description", "Delete Confirmation", MessageBoxButtons.YesNo);
                            if (op == DialogResult.Yes)
                            {
                                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                                using (var httpclient = new HttpClient())
                                {
                                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorTab/Delete", data);
                                    if (response.IsSuccessStatusCode)
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            var result = await response.Content.ReadAsStringAsync();

                                            XtraMessageBox.Show("Deleted Successfully");
                                            BindErrorGrid();

                                        }
                                    }
                                }
                            }
                            else
                            {
                                BindErrorGrid();
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
            else if (show == DialogResult.No)
            {
                this.Close();
            }


        }

        private void gridView3_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
           if(gridView3.SelectedRowsCount !=0 && Tile_Item_ErrorTab.Checked==true)
            {
                btn_delete_multiple.Visible = true;
            }
           else
            {
                btn_delete_multiple.Visible = false;
            }
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridView1.SelectedRowsCount !=0 && Tile_Item_ErrorType.Checked==true)
            {
                btn_delete_multiple.Visible = true;
            }
            else
            {
                btn_delete_multiple.Visible = false;
            }
        }

        private void gridView5_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridView5.SelectedRowsCount !=0 && Tile_Item_ErrorField.Checked==true)
            {
                btn_delete_multiple.Visible = true;
            }
            else
            {
                btn_delete_multiple.Visible = false;
            }
        }

        private void btn_Export_Click_1(object sender, EventArgs e)
        {
            if (Tile_Item_ErrorType.Checked == true)
            {
                string filePath = @"C:\Error Type\";
                string fileName = filePath + "Error Type-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                gridView1.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else if (Tile_Item_ErrorTab.Checked == true)
            {
                string filePath = @"C:\Error Tab\";
                string fileName = filePath + "Error Tab-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                gridView3.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else if (Tile_Item_ErrorField.Checked == true)
            {
                string filePath = @"C:\Error Field\";
                string fileName = filePath + "Error Field-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                gridView3.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }


        }
    }
}