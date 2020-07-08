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
using DevExpress.XtraGrid.Columns;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_View : DevExpress.XtraEditors.XtraForm
    {
        DataTable _dtcol;
        string Col_Name, Client_Name;
        int _ProjectId;
        DataTable _dt, dt;
        int client_id;
        int user_id;
        int _UserRole;
        public Efficiency_View(int userid,int Userrole)
        {
            InitializeComponent();
            user_id = userid;
            _UserRole = Userrole;
        }

        private void Efficiency_Entry_Load(object sender, EventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            btn_delete.Visible = false;
            BindProjectType();
            ddl_ProjectType.EditValue = 1;
            SplashScreenManager.CloseForm(false);

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
            Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Create Efficiency_Entry = new Efficiency_Create(this,user_id,_UserRole);
            Efficiency_Entry.Show();
        }
        //public async void BindColumnstogrid()
        //{
        //    try
        //    {
        //        _dtcol = new DataTable();
        //        grd_Efficiency_Form.DataSource = null;
        //        gridView1.Columns.Clear();
        //       
        //        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
        //        var dictonary = new Dictionary<string, object>()
        //        {
        //            {"@Trans","Select Headers" },
        //            {"@Project_Type_Id" ,_ProjectId}

        //        };
        //        this.grd_Efficiency_Form.DataSource = new DataTable();
        //        var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
        //        using (var httpclient = new HttpClient())
        //        {
        //            var response = await httpclient.PostAsync(Base_Url.Url + "/ImportCategorySalary/BindHeaders", data);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (response.StatusCode == HttpStatusCode.OK)
        //                {
        //                    var result = await response.Content.ReadAsStringAsync();
        //                     dt = JsonConvert.DeserializeObject<DataTable>(result);
        //                    _dtcol.Columns.Add("Client_Name");
        //                    _dtcol.Columns.Add("Order_Task");
        //                    _dtcol.Columns.Add("Order_Type");
        //                    _dtcol.Columns.Add("Order_Source_Type");
        //                    _dtcol.Columns.Add("Client_Id");
        //                    _dtcol.Columns.Add("Order_Task_Id");
        //                    _dtcol.Columns.Add("Order_Type_Id");
        //                    _dtcol.Columns.Add("Order_Source_Type_Id");
        //                    for (int i = 0; i < dt.Rows.Count; i++)
        //                    {
        //                        Col_Name = Convert.ToDouble(dt.Rows[i]["Category_Name"]).ToString();
        //                        _dtcol.Columns.Add(Col_Name.ToString());
        //                        //count = Convert.ToInt32(_dt.Rows.Count.ToString());

        //                    }
        //                    grd_Efficiency_Form.DataSource = _dtcol;
        //                    gridView1.Columns[4].Visible = false;
        //                    gridView1.Columns[5].Visible = false;
        //                    gridView1.Columns[6].Visible = false;
        //                    gridView1.Columns[7].Visible = false;
        //                    for (int i = 0; i < gridView1.Columns.Count; i++)
        //                    {
        //                        gridView1.Columns[i].AppearanceHeader.Font = new Font(gridView1.Columns[i].AppearanceHeader.Font, FontStyle.Bold);
        //                        gridView1.Columns[i].OptionsColumn.AllowEdit = false;
        //                        if (i>7)
        //                        {
        //                            gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        //                            gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
        //                        }
        //                    }
                           


        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SplashScreenManager.CloseForm(false);
        //        XtraMessageBox.Show("Please contact with Admin");
        //    }
        //    finally
        //    {
        //        SplashScreenManager.CloseForm(false);
        //    }
        //}

        private void ddl_ProjectType_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_ProjectType.EditValue) != 0)
            {
                //BindRows();
               
                BindCategorySalaryBracket();
                //BindColumnstogrid();
            }
            else
            {
                grd_Efficiency_Form.DataSource = null;
            }

        }

        public async void BindCategorySalaryBracket()
        {
            try
            {
                _dt = new DataTable();
                grd_Efficiency_Form.DataSource = null;
                gridView1.Columns.Clear();
                int proid = Convert.ToInt32(ddl_ProjectType.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","BIND_DATA_TO_GRID" },
                    {"@Project_Type_Id",proid}
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
                           
                            if (_dt.Rows.Count >= 0)
                            {
                                grd_Efficiency_Form.DataSource = _dt;
                                gridView1.Columns[4].Visible = false;
                                gridView1.Columns[5].Visible = false;
                                gridView1.Columns[6].Visible = false;
                                gridView1.Columns[7].Visible = false;
                                gridView1.Columns[1].Visible = false;
                                if (_UserRole == 1)
                                {
                                    gridView1.Columns[0].Visible = true;
                                    gridView1.Columns[1].Visible = false;
                                }
                                else
                                {
                                    gridView1.Columns[1].Visible = true;
                                    gridView1.Columns[0].Visible = false;
                                    gridView1.Columns[1].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                                }
                                for (int i = 0; i < gridView1.Columns.Count; i++)
                                {
                                    gridView1.Columns[i].AppearanceHeader.Font = new Font(gridView1.Columns[i].AppearanceHeader.Font, FontStyle.Bold);
                                    gridView1.Columns[i].AppearanceHeader.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView1.Columns[i].AppearanceCell.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                                    if (i > 7)
                                    {
                                        gridView1.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                        gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    }
                                }

                            }
                           
                        }
                    }
                    else
                    {
                        DataTable dtcolun = new DataTable();
                        dtcolun.Columns.Add("Client Name");
                        dtcolun.Columns.Add("Order Task");
                        dtcolun.Columns.Add("Order Type");
                        dtcolun.Columns.Add("Order_Source_Type");
                        grd_Efficiency_Form.DataSource = dtcolun;

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
                for (int i =0; i < gridView1.Columns.Count; i++)
                {
                    GridColumn colModelPrice = gridView1.Columns[i];
                    //gridView_ClientTAT.Columns[i].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    colModelPrice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    colModelPrice.DisplayFormat.FormatString = "D";
                }
                grd_Efficiency_Form.ExportToXlsx(fileName);
                System.Diagnostics.Process.Start(fileName);
            }
            else
            {
                XtraMessageBox.Show("Select Project Type to Export");
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            _ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
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
                        int client_id = int.Parse(row["Client_Id"].ToString());
                        int Ordertaskid = int.Parse(row["Order_Task_Id"].ToString());
                        int Ordertype = int.Parse(row["Order_Type_Id"].ToString());
                        int ordersourcetype = int.Parse(row["Order_Source_Type_Id"].ToString());
                        
                       var dictionary = new Dictionary<string, object>()
                        {
                            { "@Trans", "DELETE" },
                            { "@Project_Type_Id",_ProjectId},
                           {"@Client_Id",client_id },
                           {"@Order_Task_Id",Ordertaskid },
                           {"@Order_Type_Id",Ordertype},
                           {"@Order_Source_Type_Id",ordersourcetype }
                        };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ImportCategorySalary/Delete", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
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
                    XtraMessageBox.Show("Record Deleted Successfully");
                    BindCategorySalaryBracket();
                    btn_delete.Visible = false;
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

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            if(gridView1.SelectedRowsCount>0)
            {
                btn_delete.Visible = true;
            }
            else
            {
                btn_delete.Visible = false;
            }
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            _ProjectId = Convert.ToInt32(ddl_ProjectType.EditValue);
            List<int> gridViewSelectedRows = gridView1.GetSelectedRows().ToList();
            for (int i = 0; i < gridViewSelectedRows.Count; i++)
            {
                DataRow row = gridView1.GetDataRow(gridViewSelectedRows[i]);
                //int Category_Id = int.Parse(row["Category_ID"].ToString());
                Client_Name = row.ItemArray[0].ToString();
                client_id = Convert.ToInt32(row.ItemArray[4]);
            }
            //_dtcopy.Columns.Remove("Client_Name");
            //_dtcopy.Columns.Remove("Order_Task");
            //_dtcopy.Columns.Remove("Order_Type");
            //_dtcopy.Columns.Remove("Order_Source_Type");
            //_dtcopy.Columns.Remove("Client_Id");
            Ordermanagement_01.Opp.Opp_Efficiency.Efficiency_Copy Efficiecnycopy = new Efficiency_Copy(_ProjectId, Client_Name, client_id,this);
            Efficiecnycopy.Show();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.RowHandle >= 0)
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
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