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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
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
        string ErrorTab;
        int ProjectType;
        int ProductType;
        public int ErrorType_Id;
        int User_Id;
        bool IsRowColorRed = true;
        DataTable dtErrorData = new DataTable();
        DataTable dtImportData = new DataTable();
        int temperrors = 0;
        int ExistingCount = 0;
        int duplicateCount = 0;
        int total = 0;
        string OperationType;
        private DataAccess da;
        private SqlConnection con;

        public object Errorvalue { get; private set; }
        public object Error_descriptionId { get; private set; }
        public object ErrorTypeDescription { get; private set; }
        public int Noofrecords { get; private set; }

        public ImportErrorInfo(string _operationType)
        {
            InitializeComponent();
            this.OperationType = _operationType;
        }

        private void ImportErrorInfo_Load(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (OperationType == "Error Type")
                {

                    gridColErrorTab.Visible = false;
                    errorDescription.Visible = false;
                    groupContError.Text = "Import Error Type";
                }
                else if (OperationType == "Error Tab")
                {
                    gridColErrorType.Visible = false;
                    errorDescription.Visible = false;
                    groupContError.Text = "Import Error Tab";
                }
                else if (OperationType == "Error Field")
                {

                    gridColErrorType.Visible = false;
                    gridColErrorTab.Visible = true;
                    groupContError.Text = "Import Error Field";
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Somethig Went Wrong Check With Administrator ");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private void bthChooseFile_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                deleteExistingTableData();
                OpenFileDialog fileup = new OpenFileDialog();
                fileup.Title = "Select Error Import File";
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
                        errorDescription.Visible = false;
                        ImportErrorTypeData(txtFilename);
                    }

                    else if (OperationType == "Error Tab")
                    {

                        gridColErrorType.Visible = false;
                        errorDescription.Visible = false;
                        ImportErrorTab(txtFilename);
                    }
                    else if (OperationType == "Error Field")
                    {
                        gridColErrorType.Visible = false;
                        gridColErrorTab.Visible = true;
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
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    dtImportData.Columns.Clear();
                    dtImportData.Rows.Clear();


                    using (XLWorkbook workBook = new XLWorkbook(Filename))
                    {
                        IXLWorksheet worksheet = workBook.Worksheet(1);

                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in worksheet.Rows())
                        {

                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dtImportData.Columns.Add(cell.Value.ToString(), typeof(string));
                                }
                                firstRow = false;
                            }
                            else
                            {
                                if (row.IsEmpty())
                                    continue;
                                dtImportData.Rows.Add();

                                int i = 0;
                                try
                                {
                                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                    {
                                        dtImportData.Rows[dtImportData.Rows.Count - 1][i] = cell.Value.ToString();
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    dtImportData.Rows.RemoveAt(dtImportData.Rows.Count - 1);
                                    break;
                                }
                            }
                        }

                    }


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
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Uploaded Successfully");
                            }
                            BindErrorTypeData();


                        }
                    }
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");
                    //throw ex;
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectErrorTypeValues", data);
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
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong");
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
                    dtImportData.Columns.Clear();
                    dtImportData.Rows.Clear();


                    using (XLWorkbook workBook = new XLWorkbook(Filename))
                    {
                        IXLWorksheet worksheet = workBook.Worksheet(1);

                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in worksheet.Rows())
                        {

                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dtImportData.Columns.Add(cell.Value.ToString(), typeof(string));
                                }
                                firstRow = false;
                            }
                            else
                            {
                                if (row.IsEmpty())
                                    continue;
                                dtImportData.Rows.Add();

                                int i = 0;
                                try
                                {
                                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                    {
                                        dtImportData.Rows[dtImportData.Rows.Count - 1][i] = cell.Value.ToString();
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    dtImportData.Rows.RemoveAt(dtImportData.Rows.Count - 1);
                                    break;
                                }
                            }
                        }

                    }

                    deleteExistingTableData();
                    dtErrorFieldData.Columns.AddRange(new DataColumn[]
                          {
                          new DataColumn("Project_Type",typeof(string)),
                          new DataColumn("Product_Type",typeof(string)),
                          new DataColumn("Error_Tab",typeof(string)),
                          new DataColumn("Error_Description",typeof(string))

                          });

                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {


                        string prjvalue = dtImportData.Rows[i][0].ToString();
                        string prdvalue = dtImportData.Rows[i][1].ToString();
                        string errortab = dtImportData.Rows[i][2].ToString();
                        string errDes = dtImportData.Rows[i][3].ToString();
                        dtErrorFieldData.Rows.Add(prjvalue, prdvalue, errortab, errDes);

                        var data = new StringContent(JsonConvert.SerializeObject(dtErrorFieldData), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorFieldData", data);
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
                    XtraMessageBox.Show("Uploaded Successfully");
                    BindErrorFieldData();


                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");

                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        private async void CheckkErrorTypeId(string ErrorType)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","Check_ERROR_TYPE_Id" },
                    {"@Error_Type",ErrorType }

                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/CheckId", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                ErrorType_Id = Convert.ToInt32(dt.Rows[0]["Error_Type_Id"]);
                            }
                            else
                            {
                                ErrorType_Id = 0;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }

        private async void BindErrorFieldData()
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                {
                    {"@Trans","BindErrorfieldData"}
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
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


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
                //throw ex;
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went wrong");
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }


        private async void ImportErrorTab(string FileName)
        {
            DataTable dtimport = new DataTable();
            if (FileName != string.Empty)
            {
                try
                {
                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                    dtImportData.Columns.Clear();
                    dtImportData.Rows.Clear();


                    using (XLWorkbook workBook = new XLWorkbook(FileName))
                    {
                        IXLWorksheet worksheet = workBook.Worksheet(1);

                        DataTable dt = new DataTable();

                        //Loop through the Worksheet rows.
                        bool firstRow = true;
                        foreach (IXLRow row in worksheet.Rows())
                        {

                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dtImportData.Columns.Add(cell.Value.ToString(), typeof(string));
                                }
                                firstRow = false;
                            }
                            else
                            {
                                if (row.IsEmpty())
                                    continue;
                                dtImportData.Rows.Add();

                                int i = 0;
                                try
                                {
                                    foreach (IXLCell cell in row.Cells(row.FirstCellUsed().Address.ColumnNumber, row.LastCellUsed().Address.ColumnNumber))
                                    {
                                        dtImportData.Rows[dtImportData.Rows.Count - 1][i] = cell.Value.ToString();
                                        i++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    dtImportData.Rows.RemoveAt(dtImportData.Rows.Count - 1);
                                    break;
                                }
                            }
                        }

                    }



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
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Uploaded Successfully");
                            }
                            bindErrorData();
                            // ValidateErrors();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }




        }


        private async void bbtnSampleFormat_Click(object sender, EventArgs e)
        {
            if (OperationType == "Error Type")
            {
                try
                {

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","ColumnsToExcelOf_Error_Type" }

                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectError_TypeColumns", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dtCol = JsonConvert.DeserializeObject<DataTable>(result);
                                string filePath = @"C:\Temp\";
                                string fileName = filePath + "Import_Error_Type_Data-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                                StreamWriter wr = new StreamWriter(fileName);


                                for (int i = 0; i < dtCol.Columns.Count; i++)
                                {
                                    wr.Write(dtCol.Columns[i].ToString() + "\t");
                                }

                                wr.WriteLine();
                                wr.Close();

                                if (Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                    //File.Copy(fileName, temppath, true);
                                    Process.Start(fileName);

                                }
                                else
                                {
                                    Process.Start(filePath);
                                }
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("File DownLoaded SucessFully");
                            }
                        }
                    }
                }

                catch (Exception ex)

                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
            else if (OperationType == "Error Tab")
            {
                try
                {

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","ColumnsToExcelOf_Error_Tab" }

                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectError_TabColumns", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dtCol = JsonConvert.DeserializeObject<DataTable>(result);
                                string filePath = @"C:\Temp\";
                                string fileName = filePath + "Import_Error_Tab_Data-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                                StreamWriter wr = new StreamWriter(fileName);

                                for (int i = 0; i < dtCol.Columns.Count; i++)
                                {
                                    wr.Write(dtCol.Columns[i].ToString() + "\t");
                                }

                                wr.WriteLine();
                                wr.Close();
                                if (Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                    //File.Copy(fileName, temppath, true);
                                    Process.Start(fileName);

                                }
                                else
                                {
                                    Process.Start(filePath);
                                }
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("File DownLoaded SucessFully");
                            }
                        }
                    }
                }

                catch (Exception ex)

                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }

            }
            else if (OperationType == "Error Field")
            {
                try
                {

                    SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                    var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","ColumnsToExcelOf_Error_Field" }

                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ErrorImport/SelectError_FieldColumns", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dtCol = JsonConvert.DeserializeObject<DataTable>(result);
                                string filePath = @"C:\Temp\";
                                string fileName = filePath + "Import_Error_Field_Data-" + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".xls";
                                StreamWriter wr = new StreamWriter(fileName);

                                for (int i = 0; i < dtCol.Columns.Count; i++)
                                {
                                    wr.Write(dtCol.Columns[i].ToString() + "\t");
                                }

                                wr.WriteLine();
                                wr.Close();
                                if (Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                    //File.Copy(fileName, temppath, true);
                                    Process.Start(fileName);

                                }
                                else
                                {
                                    Process.Start(filePath);
                                }
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("File DownLoaded SucessFully");
                            }
                        }
                    }
                }

                catch (Exception ex)

                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Something went wrong");
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

        }


        private async void btnSubmit_Click(object sender, EventArgs e)
        {

            if (lblTotalErrors.Text != "0" && gridView1.DataRowCount == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Invalid!,Upload Proper New Error Types", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (OperationType == "Error Tab")
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

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
                            ProjectType = Convert.ToInt32(dtErrorData.Rows[i][0]);
                            ProductType = Convert.ToInt32(dtErrorData.Rows[i][2]);
                            ErrorTab = dtErrorData.Rows[i][4].ToString();

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
                                        SplashScreenManager.CloseForm(false);

                                        XtraMessageBox.Show("Inserted Successfully ");
                                    }
                                    Clear();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // throw ex;
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something went wrong");
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
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
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
                            int projid = Convert.ToInt32(dtErrorData.Rows[i][0]);
                            int prodid = Convert.ToInt32(dtErrorData.Rows[i][2]);
                            string errorType = dtErrorData.Rows[i][4].ToString();

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
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Inserted  Sucessfully");

                                }
                                Clear();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something went wrong");
                    }
                    finally
                    {
                        SplashScreenManager.CloseForm(false);

                    }
                }
                else if (OperationType == "Error Field")
                {
                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        DataTable dtmulti = new DataTable();


                        dtmulti.Columns.AddRange(new DataColumn[]
                        {
                        new DataColumn("Project_Type_Id",typeof(int)),
                        new DataColumn("Product_Type_Id",typeof(int)),
                        new  DataColumn("Error_Type_Id",typeof(int)),
                        new DataColumn("Error_description",typeof(string)),
                        new DataColumn("Status",typeof(bool)),
                        new DataColumn("Inserted_By",typeof(int)),
                        new DataColumn("Instered_Date",typeof(DateTime))
                        });
                        for (int i = 0; i < dtErrorData.Rows.Count; i++)
                        {
                            int errortype_Id = Convert.ToInt32(dtErrorData.Rows[i][4]);

                            int projid = Convert.ToInt32(dtErrorData.Rows[i][0]);
                            int prodid = Convert.ToInt32(dtErrorData.Rows[i][2]);

                            string errordes = dtErrorData.Rows[i][5].ToString();
                            dtmulti.Rows.Add(projid, prodid, errortype_Id, errordes, "True", User_Id, DateTime.Now);
                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ErrorImport/InsertErrorField", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                }

                            }
                        }


                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Inserted  Sucessfully");
                        Clear();
                    }
                    catch (Exception ex)
                    {

                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Something went wrong");
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
                    XtraMessageBox.Show("An Error Occured Please Check With Adminstrator");
                }
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
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
                    string ReportName = "ErrorImport_Data";
                    string FolderPath = "C:\\Temp\\";
                    string Path = FolderPath + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + "-" + ReportName + ".xlsx";
                    compositeLink.CreatePageForEachLink();
                    compositeLink.PrintingSystem.ExportToXlsx(Path, new XlsxExportOptions() { ExportMode = XlsxExportMode.SingleFilePageByPage, ExportHyperlinks = false, TextExportMode = TextExportMode.Value, IgnoreErrors = XlIgnoreErrors.NumberStoredAsText });
                    System.Diagnostics.Process.Start(Path);
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong");
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
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something went Wrong");
                // throw ex;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);

            }
        }



        /*  private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
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
          }*/

        private void ValidateErrors()
        {



            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
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
                    if (Convert.ToInt32(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Duplicate_Count"))) > 0)
                    {
                        duplicateCount += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Duplicate");
                    }

                    if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Project_Type")).ToString()))
                    {
                        temperrors += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Project Type Not Found");
                    }
                    if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Product_Type")).ToString()))
                    {
                        temperrors += 1;
                        gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Product Type Not Found");
                    }

                    if (OperationType == "Error Field")
                    {
                        if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Tab")).ToString()))
                        {
                            temperrors += 1;
                            gridView1.SetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Status"), "Error Tab Not Found");
                        }
                        if (string.IsNullOrWhiteSpace(gridView1.GetRowCellValue(i, gridView1.Columns.ColumnByFieldName("Error_Description")).ToString()))
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
                //for (int i=0;i< dtErrorData.Rows.Count;i++)
                //{
                //    Noofrecords = int.Parse(dtErrorData.Rows[0][8].ToString());
                //}
                /// UserCount = int.Parse(Results.Rows[0][0].ToString());

                total = ExistingCount + duplicateCount + temperrors;
                // total = Noofrecords;
                lblDuplicateRecordCount.Text = duplicateCount.ToString();
                lblExistingRecordCount.Text = ExistingCount.ToString();
                lblTotalErrors.Text = total.ToString();
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Errors : " + total);


            }
            catch (Exception ex)
            {
                // throw ex;
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong");

            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
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
            try
            {
                GridView view = sender as GridView;
                if (view == null) return;


                // var index = view.GetDataRow(view.RowCount["Error_Status"]);
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
            catch (Exception ex)
            {
                //throw ex;
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Something Went Wrong");
            }

        }
    }
}


