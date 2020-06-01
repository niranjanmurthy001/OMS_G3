using ClosedXML.Excel;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using Ordermanagement_01.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
    

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class ImportErrorInfo : XtraForm
    {
        string txtFilename;
        string ErrorType;
        string ErrorTab;
        int ProjectType;
        int ProductType;
        int User_Id;
        bool IsRowColorRed = true;
        DataTable dtErrorData = new DataTable();
        DataTable dtImportData = new DataTable();
        DataTable dataTableErrors = new DataTable();
        DataTable dataTableExportToDb = new DataTable();
        DataTable dataTableImportExcel = new DataTable();
        string OperationType;
        private DataAccess da;
        private SqlConnection con;

        public object Errorvalue { get; private set; }
        public object Error_descriptionId { get; private set; }
        public object ErrorTypeDescription { get; private set; }

        public ImportErrorInfo(string _operationType)
        {
            InitializeComponent();
            this.OperationType = _operationType;
        }

        private void ImportErrorInfo_Load(object sender, EventArgs e)
        {

        }

        private void bthChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog fileup = new OpenFileDialog();
                fileup.Title = "Select Error Tab File";
                fileup.InitialDirectory = @"c:\";

                fileup.Filter = "Excel Sheet(*.xlsx)|*.xlsx|Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fileup.FilterIndex = 1;
                fileup.RestoreDirectory = true;
                // var txtFileName = fileup.FileName;

                if (fileup.ShowDialog() == DialogResult.OK)
                {
                    txtFilename = fileup.FileName;
                    if (OperationType == "Error Type")
                    {
                        gridColErrorTab.Visible = false;
                        gridColErrorDescrip.Visible = false;
                        ImportErrorTypeData(txtFilename);
                    }

                    else if (OperationType == "Error Tab")
                    {

                        gridColErrorType.Visible = false;
                        gridColErrorDescrip.Visible = false;
                        ImportErrorTab(txtFilename);
                    }
                    else if (OperationType == "Error Field")
                    {
                        gridColErrorType.Visible = false;
                        gridColErrorTab.Visible = false;
                        ImportErrorFieldData(txtFilename);
                    }
                    lbl_Uploadfilename.Text = txtFilename;
                    System.Windows.Forms.Application.DoEvents();
                    //ValidateErrors();
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

        private async void ImportErrorTypeData(string Filename)
        {
            DataTable dtImportErrorType = new DataTable();
            if (txtFilename != string.Empty)
            {
                try
                {
                    String name = "Sheet1";
                    String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilename + ";Extended Properties='Excel 12.0 XML;HDR=YES;';";
                    System.Data.OleDb.OleDbConnection conn = new OleDbConnection(constr);
                    OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", conn);
                    conn.Open();

                    OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                    sda.Fill(dtImportData);


                    deleteExistingTableData();
                    dtImportErrorType.Columns.AddRange(new DataColumn[]
                    {     new DataColumn("Project_Type",typeof(string)),
                          new DataColumn("Product_Type",typeof(string)),
                          new DataColumn("New_Error_Type",typeof(string))

                    });
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        string prjvalue = dtImportData.Rows[i][0].ToString();
                        string prdValue = dtImportData.Rows[i][1].ToString();
                        string ErrorType = dtImportData.Rows[i][2].ToString();
                        dtImportErrorType.Rows.Add(prjvalue, prdValue, ErrorType);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtImportErrorType), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorTypeData", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                XtraMessageBox.Show("Inserted Succesfully");
                            }
                            BindErrorTypeData();


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
        }

        private async void BindErrorTypeData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","SelectErrorTypeData"}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectErrorTabValues", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dtErrorData = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dtErrorData.Rows.Count > 0)
                            {
                                gridErrorImport.DataSource = dtErrorData;


                            }
                            ValidateErrors();
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

        private async void ImportErrorFieldData(string Filename)
        {
            DataTable dtErrorFieldData = new DataTable();

            if (Filename != null)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    String name = "Sheet1";
                    String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilename + ";Extended Properties='Excel 12.0 XML;HDR=YES;';";
                    System.Data.OleDb.OleDbConnection conn = new OleDbConnection(constr);
                    OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", conn);
                    conn.Open();

                    OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                    sda.Fill(dtImportData);
                    dtErrorFieldData.Columns.AddRange(new DataColumn[]
                    {
                       new DataColumn("Project_Type",typeof(string)),
                          new DataColumn("Product_Type",typeof(string)),
                          new DataColumn("Error_Description",typeof(string))

                    });
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        string prjvalue = dtImportData.Rows[i][0].ToString();
                        string prdvalue = dtImportData.Rows[i][1].ToString();
                        string errDes = dtImportData.Rows[i][2].ToString();
                        dtErrorFieldData.Rows.Add(prjvalue, prdvalue, errDes);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtErrorFieldData), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorFieldData", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                XtraMessageBox.Show("Inserted Succesfully");
                            }
                            BindErrorFieldData();
                           
                           
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
        }

        private async void BindErrorFieldData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","SelectErrorFieldData"}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectErrorFieldValues", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dtErrorData = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dtErrorData.Rows.Count > 0)
                            {
                                gridErrorImport.DataSource = dtErrorData;


                            }
                            ValidateErrors();
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

        private void ProcessErrorImport(string Filename)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(Masters.WaitForm1), true, true, false);
                dataTableImportExcel.Columns.Clear();
                dataTableImportExcel.Rows.Clear();

                using (XLWorkbook wb = new XLWorkbook(Filename))
                {
                    IXLWorksheet ws = wb.Worksheet(1);
                    DataTable dt = new DataTable();

                    bool firstRow = true;
                    foreach (IXLRow rows in ws.Rows())
                    {
                        if (firstRow)
                        {
                            foreach (IXLCell cell in rows.Cells())
                            {
                                dataTableImportExcel.Columns.Add(cell.Value.ToString(), typeof(string));

                            }
                            firstRow = false;
                        }
                        else
                        {
                            if (rows.IsEmpty())
                                continue;
                            dataTableImportExcel.Columns.Add();
                            int i = 0;
                            try
                            {
                                foreach (IXLCell cell in rows.Cells(rows.FirstCellUsed().Address.ColumnNumber, rows.LastCellUsed().Address.ColumnNumber))
                                {
                                    dataTableImportExcel.Rows[dataTableImportExcel.Rows.Count][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                            catch (Exception ex)
                            {
                                // dataTableImportExcel.Rows.RemoveAt(dataTableImportExcel.Rows.Count - 1);
                                break;
                            }
                        }
                    }
                }
                dataTableErrors.Columns.Clear();
                dataTableExportToDb.Columns.Clear();
                //  dataTableErrors = dataTableImportExcel.Clone();
                // dataTableImportExcel.Columns.AddRange(new DataColumn[]
                //{
                //        new DataColumn("Project_Type",typeof(string)),
                //         new DataColumn("Product_Type",typeof(string)),
                //         new DataColumn("Error_Type",typeof(string))
                //});
                // dataTableExportToDb = dataTableErrors.Clone();
                var ht1 = new Hashtable();
                ht1.Add("@Trans", "TRUNCATE");
                da = new DataAccess();
                var d = da.ExecuteSP("Usp_Error_Import", ht1);
                using (con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Con"].ConnectionString))
                {
                    con.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                    {
                        bulkCopy.ColumnMappings.Add("Project_Type", "Project_Type");
                        bulkCopy.ColumnMappings.Add("Product_Type", "Product_Type");
                        bulkCopy.ColumnMappings.Add("Error_Type", "Error_Type");
                        bulkCopy.BulkCopyTimeout = 3000;
                        bulkCopy.BatchSize = 10000;
                        bulkCopy.DestinationTableName = "Tbl_Error_Import_Data";
                        bulkCopy.WriteToServer(dataTableImportExcel.CreateDataReader());
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Inserted Temperory data succesfully");
                    }
                    con.Close();
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
            // bindErrorData();
            // BindErrorTypeData();
        }
        private async void bindErrorData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","SelectErrorTabData"}
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectErrorTabValues", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dtErrorData = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dtErrorData.Rows.Count > 0)
                            {
                                dtErrorData.Columns.Add("Error_Status", typeof(string));
                                gridErrorImport.DataSource = dtErrorData;


                            }
                            ValidateErrors();
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


        private async void ImportErrorTab(string txtFileName)
        {
            DataTable dtimport = new DataTable();
            if (txtFileName != string.Empty)
            {
                try
                {


                    String name = "Sheet1";
                    String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilename + ";Extended Properties='Excel 12.0 XML;HDR=YES;';";
                    System.Data.OleDb.OleDbConnection conn = new OleDbConnection(constr);
                    OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", conn);
                    conn.Open();

                    OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
                    sda.Fill(dtImportData);


                    deleteExistingTableData();
                    //Insertion to temptable
                    dtimport.Columns.AddRange(new DataColumn[]
                    {  new DataColumn("Project_Type",typeof(string)),
                          new DataColumn("Product_Type",typeof(string)),
                          new DataColumn("Error_Tab",typeof(string))

                    });

                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {

                        string prjvalue = dtImportData.Rows[i][0].ToString();
                        string prdvalue = dtImportData.Rows[i][1].ToString();
                        string errvalue = dtImportData.Rows[i][2].ToString();
                        dtimport.Rows.Add(prjvalue, prdvalue, errvalue);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtimport), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorTabData", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                XtraMessageBox.Show("Inserted Succesfully");
                            }
                            bindErrorData();
                            // ValidateErrors();
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




        }


        private void bbtnSampleFormat_Click(object sender, EventArgs e)
        {

            try
            {
                if (OperationType == "Error Tab")
                {


                    Directory.CreateDirectory(@"c:\OMS_Importing\");
                    //string temppath = @"c:\OMS_Import\Error_Type_Master.xlsx";
                    string temppath = @"c:\OMS_Importing\Error_Tab_Master_Settings.xlsx";
                    if (!Directory.Exists(temppath))
                    {
                        File.Copy(@"\\192.168.12.20\Queen\OmsImports\Error_Tab_Master_Settings.xlsx", temppath, true);
                        Process.Start(temppath);
                    }
                    else
                    {
                        Process.Start(temppath);
                    }
                }
                else if (OperationType == "Error Type")
                {

                }
                else if (OperationType == "Error Field")
                {

                }
                else
                {
                    XtraMessageBox.Show("Data Not Found");
                }
            }



            catch
            {

            }
            finally
            {

            }
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < gridView1.DataRowCount; i++)
            //{


            //}
            if (OperationType == "Error Tab")
            {
                try
                {

                    DataTable dtins_ErrorTab = new DataTable();
                    dtins_ErrorTab.Columns.AddRange(new DataColumn[]
                    {
                     new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type_Id",typeof(int)),
                     new DataColumn("Error_Type",typeof(string)),
                     new DataColumn("Status",typeof(bool)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Instered_Date",typeof(DateTime)),

                    });
                    for (int i = 0; i < dtErrorData.Rows.Count; i++)
                    {
                        ProjectType = Convert.ToInt32(dtErrorData.Rows[i][1]);
                        ProductType = Convert.ToInt32(dtErrorData.Rows[i][3]);
                        ErrorTab = dtErrorData.Rows[i][6].ToString();

                        dtins_ErrorTab.Rows.Add(ProjectType, ProductType, ErrorTab, "true", User_Id, DateTime.Now);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dtins_ErrorTab), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorTab", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt.Rows.Count > 0)
                                {
                                    XtraMessageBox.Show("Inserted Destination sucess");
                                }
                                Clear();
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
            else if (OperationType == "Error Type")
            {

                try
                {
                    DataTable dtInsError_Type = new DataTable();
                    dtInsError_Type.Columns.AddRange(new DataColumn[]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type_Id",typeof(int)),
                     new DataColumn("New_Error_Type",typeof(string)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Instered_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                    });
                    for (int i = 0; i < dtErrorData.Rows.Count; i++)
                    {
                        int projid = Convert.ToInt32(dtErrorData.Rows[i][1]);
                        int prodid = Convert.ToInt32(dtErrorData.Rows[i][4]);
                        string errorType = dtErrorData.Rows[i][6].ToString();

                        dtInsError_Type.Rows.Add(projid, prodid, errorType, User_Id, DateTime.Now, "True");
                    }

                    var data = new StringContent(JsonConvert.SerializeObject(dtInsError_Type), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorType", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                            }
                            Clear();
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
            else if (OperationType == "ErrorField")
            {
                try
                {

                    DataTable dtmulti = new DataTable();
                    dtmulti.Columns.AddRange(new DataColumn[7]
                    {
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new  DataColumn("Error_Type_Id",typeof(string)),
                        new DataColumn("Error_description",typeof(string)),
                        new DataColumn("Status",typeof(int)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("Instered_Date",typeof(DateTime))
                    });
                    for (int i = 0; i < dtErrorData.Rows.Count; i++)
                    {
                        int projid = Convert.ToInt32(dtErrorData.Rows[i][0]);
                        int prodid = Convert.ToInt32(dtErrorData.Rows[i][1]);
                        int errorTypeId = Convert.ToInt32(dtErrorData.Rows[i][2]);
                        string errordes = dtErrorData.Rows[i][3].ToString();
                        dtmulti.Rows.Add(projid, prodid, errorTypeId, errordes, "True", User_Id, DateTime.Now);
                    }
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorField", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Error details are Submitted");

                            }
                            Clear();
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
            else
            {
                XtraMessageBox.Show("An Error Occured Please Check With Adminstrator");
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView1.RowCount > 0)
                {
                    SplashScreenManager.ShowForm(this, typeof(Masters.WaitForm1), true, true, false);
                    PrintingSystem ps = new PrintingSystem();
                    CompositeLink compositeLink = new CompositeLink(ps);
                    PrintableComponentLink LinkErrorData = new PrintableComponentLink();
                    LinkErrorData.Component = gridErrorImport;
                    ps.SetCommandVisibility(PrintingSystemCommand.Open, CommandVisibility.All);
                    ps.SetCommandVisibility(new PrintingSystemCommand[] { PrintingSystemCommand.ExportCsv, PrintingSystemCommand.ExportTxt,
                PrintingSystemCommand.ExportXlsx, PrintingSystemCommand.ExportDocx, PrintingSystemCommand.ExportXls }, CommandVisibility.All);
                    compositeLink.Links.AddRange(new object[]
                   {
                    LinkErrorData
                   });
                    string ReportName = "Error Data";
                    string FolderPath = "C:\\Temp\\";
                    string Path = FolderPath + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + "-" + ReportName + ".xlsx";
                    compositeLink.CreatePageForEachLink();
                    compositeLink.PrintingSystem.ExportToXlsx(Path, new XlsxExportOptions() { ExportMode = XlsxExportMode.SingleFilePageByPage, ExportHyperlinks = false, TextExportMode = TextExportMode.Value, IgnoreErrors = XlIgnoreErrors.NumberStoredAsText });
                    System.Diagnostics.Process.Start(Path);
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
        private async void deleteExistingTableData()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","TRUNCATE" }


                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/TruncateTbl", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }



        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            DataRowView view = gridView1.GetRow(e.RowHandle) as DataRowView;

            if (view.Row["Error_Message"] != null)
            {
                string errormessage = view.Row["Error_Message"].ToString();
                if (errormessage != "")
                {
                    e.Appearance.BackColor = System.Drawing.Color.Red;
                    IsRowColorRed = true;
                }
                else
                {
                    IsRowColorRed = false;
                    e.Appearance.BackColor = System.Drawing.Color.White;
                }
            }
        }

        private void ValidateErrors()
        {

            int temperrors = 0;
            int ExistingCount = 0;
            int duplicateCount = 0;
            int total = 0;

            try
            {
                lblTotalErrors.Text = "";
                lblExistingRecordCount.Text = "";
                lblDuplicateRecordCount.Text = "";
                for (int i = 0; i < gridView1.DataRowCount; i++)
                {
                    if (Convert.ToInt32(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Exist_count"))) > 0)
                    {
                        ExistingCount += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Existing");
                        //  gridView1.SetRowCellValue(i, "Error_Message", "Existing");

                     }
                    else if (Convert.ToInt32(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Duplicate_Count"))) > 0)
                    {
                        duplicateCount += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Duplicate");
                    }

                    else if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Project_Type")).ToString()))
                    {
                        temperrors += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Project Type Not Found");
                    }
                    else if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Product_Type")).ToString()))
                    {
                        temperrors += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Product Type Not Found");
                    }

                    if (OperationType == "Error Field")
                    {
                        if (string.IsNullOrWhiteSpace(gridView1.Columns.ColumnByFieldName("Error_Description").ToString()))
                        {
                            temperrors += 1;
                            gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Error Description Not Found");
                            
                        }
                    }
                    else if (OperationType == "Error Type")
                    {
                        if (string.IsNullOrWhiteSpace(gridView1.Columns.ColumnByFieldName("Error_Type").ToString()))
                        {
                            temperrors += 1;
                            gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Error Type Not Found");
                        }
                    }
                    else if (OperationType == "Error Tab")
                    {
                        if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Tab")).ToString()))
                        {
                            temperrors += 1;
                            gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Error Tab Not Found");
                        }
                    }



                }
                total = ExistingCount + duplicateCount + temperrors;
                lblDuplicateRecordCount.Text = duplicateCount.ToString();
                lblExistingRecordCount.Text = ExistingCount.ToString();
                lblTotalErrors.Text = total.ToString();
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Errors : " + total);


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

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {

          
        }




        private void btnClear_Click(object sender, EventArgs e)
        {

            Clear();
        }
        private void Clear()
        {
            gridErrorImport.DataSource = null;
            lblTotalErrors.Text = "00";
            lblExistingRecordCount.Text = "00";
            lblDuplicateRecordCount.Text = "00";
            lbl_Uploadfilename.Text = "No File Choosen";
            deleteExistingTableData();
        }

        private void gridView1_RowStyle_1(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;
            if (e.RowHandle >= 0)
            {
                string error = view.GetRowCellValue(e.RowHandle, view.Columns["Error_Status"]).ToString();

                if (error == "Existing")
                {
                    e.Appearance.BackColor = System.Drawing.Color.Yellow;
                }

                else if (error == "Duplicate")
                {
                    e.Appearance.BackColor = Color.Aqua;
                    e.Appearance.ForeColor = Color.White;
                }
                else if (error != "")
                {
                    e.Appearance.BackColor = Color.Red;
                    e.Appearance.ForeColor = Color.Black;
                }
                else if (error == "")
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.ForeColor = Color.Black;


                }

            }
        }
    }
}


