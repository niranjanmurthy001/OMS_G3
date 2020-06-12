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
using System.Net.Http;
using Ordermanagement_01.Models;
using System.Net;
using Ordermanagement_01.Masters;
using System.Web.UI.WebControls;
using System.Collections;
using System.IO;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Category_Salary_Bracket_ProjectWise : DevExpress.XtraEditors.XtraForm
    {
        DataTable _dtUpdate = new DataTable();
        DataTable _dtUpdatecell = new DataTable();
         
        public Category_Salary_Bracket_ProjectWise()
        {
            InitializeComponent();
        }

        private void btn_addnew_Click(object sender, EventArgs e)
        {
            Ordermanagement_01.Opp.Opp_Efficiency.Category_Salary_Bracket_EntryForm EF = new Category_Salary_Bracket_EntryForm(this);
            EF.Show();
        }

        public async void BindCategorySalaryBracket()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT" }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/BindGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable _dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (_dt.Rows.Count > 0)
                            {

                                Grd_Category_Salary.DataSource = _dt;

                            }
                            else
                            {
                                Grd_Category_Salary.DataSource = null;

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

        private void Category_Salary_Bracket_ProjectWise_Load(object sender, EventArgs e)
        {
            BindCategorySalaryBracket();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if (gridView1.SelectedRowsCount != 0)
            {
                btn_Delete.Visible = true;
               // bnt_Submit.Visible = true;
                // var row = gridView1.GetFocusedRow();

                //DataRow row = gridView1.GetDataRow(gridViewSelectedRows[i]);
                gridView1.Columns["Category_Name"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["Salary_From"].OptionsColumn.AllowEdit = true;
                gridView1.Columns["Salary_To"].OptionsColumn.AllowEdit = true;




            }
            else
            {
                //bnt_Submit.Visible = false;
                btn_Delete.Visible = false;
            }
        }

        private async void btn_Delete_Click(object sender, EventArgs e)
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
                    List<int> gridViewSelectedRows = gridView1.GetSelectedRows().ToList();
                    for (int i = 0; i < gridViewSelectedRows.Count; i++)
                    {
                        DataRow row = gridView1.GetDataRow(gridViewSelectedRows[i]);
                        int Category_Id = int.Parse(row["Category_ID"].ToString());
                        var dictionary = new Dictionary<string, object>()
                        {
                            { "@Trans", "DELETE" },
                            { "@Category_ID", Category_Id }
                        };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);

                                    BindCategorySalaryBracket();
                                    btn_Delete.Visible = false;

                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Please Select Type To Delete");
                            }
                        }

                    }
                    XtraMessageBox.Show("Record Deleted Successfully");
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

        private async void bnt_Submit_Click(object sender, EventArgs e)
        {
            try
            {

                //_dtUpdatecell.Columns.Add("Status");
                //_dtUpdatecell.Columns.Add("Modified_By");
                //_dtUpdate.Columns.Add("Modified_Date");
                //_dtUpdatecell.Rows.Add(1, 1, DateTime.Now);
                _dtUpdatecell.Columns.RemoveAt(5);
                _dtUpdatecell.Columns.Add("Status");
                _dtUpdatecell.Columns.Add("Modified_By");
                _dtUpdatecell.Columns.Add("Modified_Date");

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var data = new StringContent(JsonConvert.SerializeObject(_dtUpdatecell), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/CategorySalaryBracket/Update", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            SplashScreenManager.CloseForm(false);

                            BindCategorySalaryBracket();
                            //_dtUpdate.Columns.Clear();
                            _dtUpdate.Columns.Clear();
                            XtraMessageBox.Show("Category Salary Bracket is Updated Succesfully");
                            bnt_Submit.Visible = false;
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


        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {

        }

        private void gridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
             bnt_Submit.Visible = true;
            //DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //int category = int.Parse(row["Salary_From"].ToString());
            //SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            // List<int> gridViewSelectedRows = gridView1.GetSelectedRows().ToList();
            //var _gridViewSelectedRows = gridView1.Columns.ToList();
            // for (int i = 0; i < _gridViewSelectedRows.Count; i++)
            // {
            //DataRow row1 = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //int Category_Id = int.Parse(row["Category_ID"].ToString());
            // _dtUpdatecell = gridView1.DataSource as DataTable;

            //BindingSource bs = (BindingSource)gridView1.DataSource; // Se convierte el DataSource 
            _dtUpdatecell = ((DataView)gridView1.DataSource).Table;
           
            // _dtUpdatecell.Columns.RemoveAt(5);
            


        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\OMS\";
            string fileName = filePath + "Category Salary Bracket" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xlsx";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Grd_Category_Salary.ExportToXlsx(fileName);
            System.Diagnostics.Process.Start(fileName);
            XtraMessageBox.Show("Exported Successfully");
        }
    }
}