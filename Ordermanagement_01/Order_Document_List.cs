using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using WaitForm_SetDescription;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;

namespace Ordermanagement_01
{
    public partial class Order_Document_List : Form
    {
        Commonclass Comclass = new Commonclass();
        DataAccess dataaccess = new DataAccess();
        DropDownistBindClass dbc = new DropDownistBindClass();
        DataTable dt = new DataTable();
        int userid = 0, Task, Task_Confirm_Id, Order_Status, Order_ID, Check, No_Of_Documents, Valid_Doc;
        int row; string List_Name;
        int Work_Type;
        public Order_Document_List(int user_id, int ORDER_ID, int ORDER_STTAUS, int WORK_TYPE)
        {
            InitializeComponent();
            Work_Type = WORK_TYPE;
            Order_ID = ORDER_ID;
            Order_Status = ORDER_STTAUS;
            userid = user_id;
        }
        public async void Load_Document_Details_Before()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dict_comments = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_BEFORE" }
                };
                var data_comments = new StringContent(JsonConvert.SerializeObject(dict_comments), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/OrderDocumentList/OrderDocuments", data_comments);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt_comments = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt_comments.Rows.Count > 0)
                            {
                                grd_Error.Rows.Clear();
                                for (int i = 0; i < dt_comments.Rows.Count; i++)
                                {
                                    grd_Error.AutoGenerateColumns = false;
                                    grd_Error.Rows.Add();
                                    grd_Error.Rows[i].Cells[0].Value = i + 1;
                                    grd_Error.Rows[i].Cells[1].Value = dt_comments.Rows[i]["Document_List_Name"].ToString();
                                    grd_Error.Rows[i].Cells[3].Value = dt_comments.Rows[i]["Document_List_Id"].ToString();
                                }
                            }
                            else
                            {
                                grd_Error.Rows.Clear();
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
            //Hashtable htComments = new Hashtable();
            //DataTable dtComments = new System.Data.DataTable();

            //htComments.Add("@Trans", "SELECT_BEFORE");
            //dtComments = dataaccess.ExecuteSP("Sp_Order_Document_List", htComments);

            //if (dtComments.Rows.Count > 0)
            //{
            //    grd_Error.Rows.Clear();
            //    for (int i = 0; i < dtComments.Rows.Count; i++)
            //    {
            //        grd_Error.AutoGenerateColumns = false;
            //        grd_Error.Rows.Add();

            //        grd_Error.Rows[i].Cells[0].Value = i + 1;
            //        grd_Error.Rows[i].Cells[1].Value = dtComments.Rows[i]["Document_List_Name"].ToString();

            //        grd_Error.Rows[i].Cells[3].Value = dtComments.Rows[i]["Document_List_Id"].ToString();                                  
            //    }
            //}
            //else
            //{
            //    grd_Error.Rows.Clear();
            //}
        }
        private void Order_Document_List_Load(object sender, EventArgs e)
        {
            Load_Document_Details_Before();
        }

        private async System.Threading.Tasks.Task<bool> Document_Validation()
        {
            try
            {
                DataTable dtComments;
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictComments = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_BEFORE" }
                };
                var dataComments = new StringContent(JsonConvert.SerializeObject(dictComments), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/OrderDocumentList/OrderDocuments", dataComments);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dtComments = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dtComments.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtComments.Rows.Count; i++)
                                {
                                    if (grd_Error.Rows[i].Cells[2].Value.ToString() == "" || grd_Error.Rows[i].Cells[2].Value == null)
                                    {
                                        Valid_Doc = 1;
                                        row = i;
                                        break;
                                    }
                                    else
                                    {
                                        Valid_Doc = 0;
                                    }
                                }
                                List_Name = dtComments.Rows[row]["Document_List_Name"].ToString();
                                if (Valid_Doc == 1)
                                {
                                    MessageBox.Show(List_Name + " Document Pages not entered");
                                    //MessageBox.Show("Check No of Documents field not entered");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
            //Hashtable htComments = new Hashtable();
            //DataTable dtComments = new System.Data.DataTable();

            //htComments.Add("@Trans", "SELECT_BEFORE");
            //dtComments = dataaccess.ExecuteSP("Sp_Order_Document_List", htComments);

            //if (dtComments.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dtComments.Rows.Count; i++)
            //    {
            //        if (grd_Error.Rows[i].Cells[2].Value.ToString() == "" || grd_Error.Rows[i].Cells[2].Value == null)
            //        {
            //            Valid_Doc = 1;
            //            row = i;
            //            break;
            //        }
            //        else
            //        {

            //            Valid_Doc = 0;
            //        }
            //    }
            //}
            //List_Name = dtComments.Rows[row]["Document_List_Name"].ToString();
            //if (Valid_Doc == 1)
            //{
            //    MessageBox.Show(List_Name + " Document Pages not entered");
            //    //MessageBox.Show("Check No of Documents field not entered");
            //    return false;
            //}
            return true;
        }

        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (await Document_Validation() != false)
                {
                    for (int i = 0; i < grd_Error.Rows.Count; i++)
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        var dict_check = new Dictionary<string, object>
                        {
                        {"@Trans", "CHECK" },
                        {"@Order_Id", Order_ID },
                        {"@Order_Status", Order_Status },
                        {"@Document_List_Id", int.Parse(grd_Error.Rows[i].Cells[3].Value.ToString()) },
                        {"@Work_Type_Id", Work_Type }
                        };
                        var data_check = new StringContent(JsonConvert.SerializeObject(dict_check), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/OrderDocumentList/OrderDocuments", data_check);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    DataTable dtCheck = JsonConvert.DeserializeObject<DataTable>(result);
                                    if (grd_Error.Rows[i].Cells[2].Value.ToString() != "" && grd_Error.Rows[i].Cells[2].Value != null)
                                    {
                                        if (dtCheck.Rows.Count > 0)
                                        {
                                            Check = int.Parse(dtCheck.Rows[0]["count"].ToString());
                                        }
                                        if (Check == 0)
                                        {
                                            if (grd_Error.Rows[i].Cells[2].Value.ToString() != "" && grd_Error.Rows[i].Cells[2].Value != null)
                                            {
                                                No_Of_Documents = Convert.ToInt32(grd_Error.Rows[i].Cells[2].Value.ToString());
                                            }
                                            else
                                            {
                                                No_Of_Documents = 0;
                                            }
                                            DataTable dt_multi = new DataTable();
                                            dt_multi.Columns.AddRange(new DataColumn[10]
                                            {
                                                new DataColumn("Order_Id",typeof(int)),
                                                new DataColumn("Order_Status",typeof(int)),
                                                new DataColumn("No_Of_Documents",typeof(int)),
                                                new DataColumn("Document_List_Id",typeof(int)),
                                                new DataColumn("User_Id",typeof(int)),
                                                new DataColumn("EnteredDate",typeof(DateTime)),
                                                new DataColumn("Status",typeof(bool)),
                                                new DataColumn("Inserted_By",typeof(int)),
                                                new DataColumn("Instered_Date",typeof(DateTime)),
                                                new DataColumn("Work_Type_Id",typeof(int))
                                            });
                                            int Document_List_Id = int.Parse(grd_Error.Rows[i].Cells[3].Value.ToString());
                                            dt_multi.Rows.Add(Order_ID, Order_Status, No_Of_Documents, Document_List_Id,
                                                userid, DateTime.Now, "True", userid, DateTime.Now, Work_Type);
                                            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                            var data_multi = new StringContent(JsonConvert.SerializeObject(dt_multi), Encoding.UTF8, "application/json");
                                            using (var httpClient1 = new HttpClient())
                                            {
                                                var response1 = await httpClient1.PostAsync(Base_Url.Url + "/OrderDocumentList/DocumentInsert", data_multi);
                                                if (response1.IsSuccessStatusCode)
                                                {
                                                    if (response1.StatusCode == HttpStatusCode.OK)
                                                    {
                                                        var result1 = await response1.Content.ReadAsStringAsync();
                                                        SplashScreenManager.CloseForm(false);                                                       
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Order Document List is Submitted");
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                throw ex;
            }
            finally
            {                
                SplashScreenManager.CloseForm(false);
                this.Close();
            }

            //    Hashtable htcheck = new Hashtable();
            //    DataTable dtcheck = new DataTable();
            //    htcheck.Add("@Trans", "CHECK");
            //    htcheck.Add("@Order_Id", Order_ID);
            //    htcheck.Add("@Order_Status", Order_Status);
            //    htcheck.Add("@Document_List_Id", int.Parse(grd_Error.Rows[i].Cells[3].Value.ToString()));
            //    htcheck.Add("@Work_Type_Id", Work_Type);
            //    dtcheck = dataaccess.ExecuteSP("Sp_Order_Document_List", htcheck);
            //    if (grd_Error.Rows[i].Cells[2].Value.ToString() != "" && grd_Error.Rows[i].Cells[2].Value != null)
            //    {
            //        if (dtcheck.Rows.Count > 0)
            //        {
            //            Check = int.Parse(dtcheck.Rows[0]["count"].ToString());
            //        }
            //        if (Check == 0)
            //        {
            //            Hashtable hsforSP = new Hashtable();
            //            DataTable dt = new System.Data.DataTable();
            //            if (grd_Error.Rows[i].Cells[2].Value.ToString() != "" && grd_Error.Rows[i].Cells[2].Value != null)
            //            {
            //                No_Of_Documents = Convert.ToInt32(grd_Error.Rows[i].Cells[2].Value.ToString());
            //            }
            //            else
            //            {
            //                No_Of_Documents = 0;
            //            }
            //            hsforSP.Add("@Trans", "INSERT");
            //            hsforSP.Add("@Order_Id", Order_ID);
            //            hsforSP.Add("@Order_Status", Order_Status);
            //            hsforSP.Add("@No_Of_Documents", No_Of_Documents);
            //            hsforSP.Add("@Document_List_Id", int.Parse(grd_Error.Rows[i].Cells[3].Value.ToString()));
            //            hsforSP.Add("@User_Id", userid);
            //            hsforSP.Add("@EnteredDate", DateTime.Now);
            //            hsforSP.Add("@status", "True");
            //            hsforSP.Add("@Inserted_By", userid);
            //            hsforSP.Add("@Instered_Date", DateTime.Now);
            //            hsforSP.Add("@Work_Type_Id", Work_Type);
            //            dt = dataaccess.ExecuteSP("Sp_Order_Document_List", hsforSP);
            //        }
            //    }
        }
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grd_Error_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if ((int)(((DataGridView)(sender)).CurrentCell.ColumnIndex) == 2)
            {
                e.Control.KeyPress += new KeyPressEventHandler(this.TextboxNumeric_KeyPress);
            }
        }
        private void TextboxNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            Boolean nonNumberEntered;

            nonNumberEntered = true;

            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8)
            {
                nonNumberEntered = false;
            }

            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }
    }
}
