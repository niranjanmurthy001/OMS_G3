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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Settings : DevExpress.XtraEditors.XtraForm
    {
        string OperationType;
        int User_Id;
        public Error_Settings()
        {
            InitializeComponent();
        }




        private void Error_Settings_Load(object sender, EventArgs e)
        {

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
            BindErrorDetails(); ;

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

        private void Grd_ErrorDes_Click(object sender, EventArgs e)
        {

        }

        private void btn_AddError_Click(object sender, EventArgs e)
        {
            if (Tile_Item_ErrorType.Checked == true)
            {
                OperationType = "Error Type";
                string Boxname = "Error Type";
                Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels ErrorTypes = new Error_Settingspanels(OperationType,Boxname);
                ErrorTypes.Show();

            }
            else if (Tile_Item_ErrorTab.Checked == true)
            {
                OperationType = "Error Tab";
                string _boxname = "Error Tab";
                Ordermanagement_01.Opp.Opp_Master.Error_Settingspanels ErrorTypes = new Error_Settingspanels(OperationType, _boxname);
                ErrorTypes.Show();
            }
            else if (Tile_Item_ErrorField.Checked == true)
            {

                Ordermanagement_01.Opp.Opp_Master.Error_Field ErrorField = new Error_Field();
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
        }

        private async void repositoryItemHyperLinkEdit4_Click(object sender, EventArgs e)
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

        private async void repositoryItemHyperLinkEdit6_Click(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {


                try
                {
                    System.Data.DataRow row = gridView3.GetDataRow(gridView3.FocusedRowHandle);
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

        private void Tile_Item_ErrorType_CheckedChanged(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorField.AppearanceItem.Selected.BackColor = Color.Blue;
        }
    }

}