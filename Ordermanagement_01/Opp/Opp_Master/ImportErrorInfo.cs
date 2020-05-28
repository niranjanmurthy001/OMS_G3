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
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class ImportErrorInfo : XtraForm
    {
          string txtFilename;
        string ErrorType;
        string ProjectType;
        string ProductType;
        int User_Id;
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


                ProcessErrorImport(txtFilename);
                //ImportErrorType(txtFilename);
                System.Windows.Forms.Application.DoEvents();
                lbl_Uploadfilename.Text = txtFilename;
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
                dataTableErrors = dataTableImportExcel.Clone();
                dataTableErrors.Columns.AddRange(new DataColumn[3]
               {
                     new DataColumn("Project_Type",typeof(string)),
                       new DataColumn("Product_Type",typeof(string)),
                       new DataColumn("Error_Type",typeof(string))
               });
                dataTableExportToDb = dataTableErrors.Clone();
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
            bindErrorData();
        }
        private async void bindErrorData()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","Select"}
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
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                gridErrorImport.DataSource = dt;
                                gridView1.BestFitColumns();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //private async void ImportErrorType(string txtFileName)
        //{
        //    if (txtFileName != string.Empty)
        //    {
        //        try
        //        {


        //            String name = "Sheet1";
        //            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtFilename + ";Extended Properties='Excel 12.0 XML;HDR=YES;';";
        //            System.Data.OleDb.OleDbConnection conn = new OleDbConnection(constr);
        //            OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", conn);
        //            conn.Open();

        //            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);



        //            sda.Fill(dtImportData);



        //            for (int i = 0; i < dtImportData.Rows.Count; i++)
        //            {
        //                ProjectType = dtImportData.Rows[i][0].ToString();
        //                ProductType = dtImportData.Rows[i][1].ToString();
        //                ErrorType = dtImportData.Rows[i][2].ToString();
        //                //GridView view = gridErrorImport.MainView as GridView;
        //                //var index = view.GetDataRow(view.RowCount);



        //                gridErrorImport.DataSource = dtImportData.DefaultView.ToTable(true, dtImportData.Columns[0].ColumnName, dtImportData.Columns[1].ColumnName, dtImportData.Columns[2].ColumnName);
        //                gridView1.BestFitColumns(
        //                    gridErrorImport.DataSource = dtImportData;




        //             // GetData(dtImportData);

        //                var dict = new Dictionary<string, object>()
        //                {
        //                   {"@Trans" ,"CheckErrorType"},
        //                   {"@Error_Type",ErrorType }
        //                };
        //                var data1 = new System.Net.Http.StringContent(JsonConvert.SerializeObject(dict), Encoding.UTF8, "application/Json");
        //                using (var httpclient = new HttpClient())

        //                {
        //                    var response = await httpclient.PostAsync(Models.Base_Url.Url + "/ErrorImport/CheckError", data1);
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        if (response.StatusCode == HttpStatusCode.OK)
        //                        {
        //                            var result = await response.Content.ReadAsStringAsync();
        //                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);

        //                            if (dt != null && dt.Rows.Count > 0)
        //                            {
        //                              cas
        //                            }

        //                        }
        //                    }
        //                }
        //            }



        //            }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //    }

        //}


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
               

                
               

        //public static IEnumerable GetData(this DataTable dataTable)
        //{
        //    foreach (DataRow data in dataTable.AsEnumerable())
        //    {
        //        yield return GetElements(data, dataTable.Columns);
        //    }

        //}

        //private static object GetElements(DataRow data, DataColumnCollection columns)
        //{
        //    var element = (IDictionary<string, object>)new ExpandoObject();
        //    foreach (DataColumn column in columns)
        //    {
        //        element.Add(column.ColumnName, data[column.ColumnName]);
        //    }
        //    return element;
        //}
        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (OperationType == "Error Tab")
            {
                try
                {
                    DataTable dtins_ErrorTab = new DataTable();
                    dtins_ErrorTab.Columns.AddRange(new DataColumn[3]
                    {
                    new DataColumn("Project_Type",typeof(string)),
                     new DataColumn("Product_Type",typeof(string)),
                     new DataColumn("Error_Type",typeof(string))
                     //new DataColumn("Inserted_By",typeof(int)),
                     //new DataColumn("Instered_Date",typeof(DateTime)),
                     //new DataColumn("Status",typeof(bool)),
                    });
                    for (int i = 0;i< dtImportData.Rows.Count ; i++)
                    {
                        string prjvalue = dtImportData.Rows[i][0].ToString();
                        string prdvalue = dtImportData.Rows[i][1].ToString();
                        string errvalue = dtImportData.Rows[i][2].ToString();

                        dtins_ErrorTab.Rows.Add(prjvalue, prdvalue, errvalue);
                    }/* User_Id, DateTime.Now, "True");*/
                    var data = new StringContent(JsonConvert.SerializeObject(dtins_ErrorTab), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorTab", data);
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
                    dtInsError_Type.Columns.AddRange(new DataColumn[6]
                    {
                    new DataColumn("Project_Type_Id",typeof(int)),
                     new DataColumn("Product_Type_Id",typeof(int)),
                     new DataColumn("Error_Type",typeof(string)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Instered_Date",typeof(DateTime)),
                     new DataColumn("Status",typeof(bool)),
                    });

                    dtInsError_Type.Rows.Add(ProjectType, ProductType, ErrorType, User_Id, DateTime.Now, "True");
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

                    dtmulti.Rows.Add(ProjectType, ProductType, Error_descriptionId, ErrorTypeDescription, "True", User_Id, DateTime.Now);


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
                                // BindErrorGrid();

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
            else
            {
                XtraMessageBox.Show("An Error Occured Please Check With Adminstrator");
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {   try
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
    }
}

