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
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;
using System.Net.Mail;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace Ordermanagement_01.New_Dashboard.Settings
{
    public partial class Clarification_Setting : DevExpress.XtraEditors.XtraForm
    {
        int ID = 0;
        int UID = 0;
        int _Client;
        DataAccess dataccess = new DataAccess();
        Hashtable ht = new Hashtable();
        DropDownistBindClass dbc = new DropDownistBindClass();
        private int emailId;
        int Email_Type;
        int userid;
        string User_Role;
        string ClientName;
        string ClientId;


        public Clarification_Setting(int User_id, string USER_ROLE)
        {
            InitializeComponent();
            userid = User_id;
            User_Role = USER_ROLE;
            splashScreenManager1.ShowWaitForm();
        }



        private async void BindCategory()
        {
            try
            {
                var dictionarybind = new Dictionary<string, object>();
                {
                    dictionarybind.Add("@Trans", "BIND");
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictionarybind), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindType", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count >= 0)
                            {
                                grd_Clarification_Category.DataSource = dt;
                                grd_Clarification_Category.ForceInitialize();

                            }
                            else
                            {
                                grd_Clarification_Category.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.Message.ToString());
            }

        }


        public void ClearCategory()
        {
            txt_ClarificationCatType.Text = "";
        }


        private bool ValidateCategory()
        {
            if (txt_ClarificationCatType.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Clarification Category");
                txt_ClarificationCatType.Focus();
                return false;
            }
            return true;
        }
        public bool ClassificationCategoryType()
        {
            bool bStatus = true;
            if (txt_ClarificationCatType.Text == "")
            {
                dxErrorProvider1.SetError(txt_Outgoing_server, "Please Enter Category Type");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_ClarificationCatType, "");
            return bStatus;
        }
        private void txt_ClarificationCategory_Validating(object sender, CancelEventArgs e)
        {
            ClassificationCategoryType();
        }

        private async void btn_Submit_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (ValidateCategory() != false)
                {
                    if (btn_ClarificationSubmit.Text == "Save")
                    {
                        // splashScreenManager1.ShowWaitForm();
                        var dictionaryinsert = new Dictionary<string, object>();
                        {
                            dictionaryinsert.Add("@Trans", "INSERT");
                            dictionaryinsert.Add("@Clarification_Category_Type", txt_ClarificationCatType.Text);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryinsert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/InsertType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(txt_ClarificationCatType.Text + " Inserted Successfully ");
                                    BindCategory();
                                    ClearCategory();
                                }
                            }
                        }


                    }
                    if (btn_ClarificationSubmit.Text == "Edit")
                    {

                        var dictionaryedit = new Dictionary<string, object>();
                        {
                            dictionaryedit.Add("@Trans", "UPDATE");
                            dictionaryedit.Add("@Clarification_Category_Type_Id", ID);
                            dictionaryedit.Add("@Clarification_Category_Type", txt_ClarificationCatType.Text);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryedit), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PutAsync(Base_Url.Url + "/ClarificationSetting/UpdateType", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(" Edited Successfully ");
                                    btn_ClarificationSubmit.Text = "Save";
                                    BindCategory();
                                    ClearCategory();

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {

            }
        }



        private void btn_Clear_Click_1(object sender, EventArgs e)
        {
            ClearCategory();
            btn_ClarificationSubmit.Text = "Save";
        }

        private async void repositoryItemHyperLinkEdit1_Click_1(object sender, EventArgs e)
        {
            string message = "Do you want to delete?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult show = XtraMessageBox.Show(message, title, buttons);
            if (show == DialogResult.Yes)
            {


                try
                {
                    System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                    ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
                    var dictionarydelete = new Dictionary<string, object>();
                    {
                        dictionarydelete.Add("@Trans", "DELETE");
                        dictionarydelete.Add("@Clarification_Category_Type_Id", ID);


                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictionarydelete), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/DeleteType", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                BindCategory();
                                ClearCategory();

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
           
        }

        private void repositoryItemHyperLinkEdit2_Click_1(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
            txt_ClarificationCatType.Text = row["Clarification_Category_Type"].ToString();
            btn_ClarificationSubmit.Text = "Edit";

            //DataRow row = gridView1.GetDataRow(e.RowHandle);
            //emailId = Convert.ToInt32(row["ID"]);
        }



        private void Clarification_Setting_Load(object sender, EventArgs e)

        {

            BindCategory();
            grid_Email_Address_list();
            ToEmailBindData();
            Bind_From_Email();
            Bind_To_Email();
            BindClient();
            BindClientGrid();
            if (User_Role == "1")
            {
                gridView4.Columns[1].Visible = true;
                gridView4.Columns[3].Visible = true;
                gridView4.Columns[2].Visible = false;


            }
            if (User_Role == "2")
            {
                gridView4.Columns[1].Visible = true;
                gridView4.Columns[2].Visible = true;
                gridView4.Columns[3].Visible = false;


            }


            txt_IS.Enabled = true;
            txt_OS.Enabled = true;
            this.ActiveControl = txt_Display_Name;
        }

        private void gridView1_RowCellClick_1(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        { //    try
            //    {
            //        if (e.Column.FieldName == "Edit")
            //        {
            //            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            //            ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
            //            txt_ClarificationCategoryyType.Text = row["Clarification_Category_Type"].ToString();
            //            BindData();
            //            btn_Submit.Text = "Edit";
            //        }

        }






        // Clarification From Email Setting


        public void ClearFromEmail()
        {
            txt_Display_Name.Text = "";
            txt_FromEmailId.Text = "";
            txt_Incoming_server.Text = "";
            txt_Outgoing_server.Text = "";
            txt_User_Name.Text = "";
            txt_password.Text = "";
            txt_IS.Text = "";
            txt_OS.Text = "";
            check_ShowPassword.Checked = false;
            check_connection_SSL.Checked = false;
        }

        private void testEmailadrdress()
        {
            using (MailMessage mm = new MailMessage())
            {
                try
                {
                    splashScreenManager1.ShowWaitForm();
                    SendHtmlFormattedEmail();

                }
                catch (Exception error)
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show(error.Message.ToString());
                    return;

                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }

        }

        private void SendHtmlFormattedEmail()
        {
            try
            {
                splashScreenManager1.ShowWaitForm();
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(txt_FromEmailId.Text.ToString());

                    if (txt_FromEmailId.Text != "")
                    {

                        mailMessage.To.Add("niranjanmurthy@drnds.com");

                        SplashScreenManager.CloseForm(false);
                        string Subject = " " + txt_User_Name.Text + " " + "Test Email - OMS";
                        mailMessage.Subject = Subject.ToString();

                        StringBuilder sb = new StringBuilder();
                        sb.Append("Subject: " + Subject.ToString() + "" + Environment.NewLine);

                        SmtpClient smtp = new SmtpClient();

                        smtp.Host = txt_Outgoing_server.Text;

                        //NetworkCredential NetworkCred = new NetworkCredential("netco@drnds.com", "P2DGo5fi-c");
                        NetworkCredential NetworkCred = new NetworkCredential(txt_FromEmailId.Text, txt_password.Text);
                        smtp.UseDefaultCredentials = true;
                        // smtp.Timeout = Math.Max(attachments.Sum(Function(Item) (DirectCast(Item, MailAttachment).Size / 1024)), 100) * 1000
                        smtp.Timeout = (60 * 5 * 1000);
                        smtp.Credentials = NetworkCred;
                        //smtp.EnableSsl = true;
                        smtp.Port = Convert.ToInt32(txt_OS.Text);
                        smtp.Send(mailMessage);
                        smtp.Dispose();
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Email Account Tested Succesfully");
                    }
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Email is Not Added Kindly Check It");
                    }
                }
            }
            catch (Exception e)
            {

                XtraMessageBox.Show(e.Message.ToString());
                return;
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }

        }

        private async void grid_Email_Address_list()
        {

            try
            {
                var dictionary = new Dictionary<string, object>()
            {
                    { "@Trans", "FROM_EMAIL_SELECT"}
            };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindFromEmail", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                gridControl1_From_Email.DataSource = dt;
                                gridControl1_From_Email.ForceInitialize();
                            }
                            else
                            {
                                gridControl1_From_Email.DataSource = null;
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

        public async Task<bool> Usercheck()
        {
            DataTable dt = new DataTable();
            try
            {

                if (txt_FromEmailId.Text != "")
                {
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "FROM_EmailCheck" },
                    { "@From_Email_Id", txt_FromEmailId.Text.Trim()}
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/CheckEmail", data);
                        if (response.IsSuccessStatusCode)
                        {                    
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                int count = Convert.ToInt32(dt1.Rows[0]["count"].ToString());
                                if (count > 0)
                                {
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("E-mail Already Exists");
                                    return false;
                                }

                            }


                        }

                    }

                }
                return true;
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


        private bool ValidationFromEmailId()
        {
            if (txt_Display_Name.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Your_Name");
                txt_Display_Name.Focus();
                return false;
            }
            else if (txt_FromEmailId.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Email_Address");
                txt_FromEmailId.Focus();
                return false;
            }
            else if (txt_Incoming_server.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Incoming Server details");
                txt_Incoming_server.Focus();
                return false;
            }
            else if (txt_Outgoing_server.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Outgoing server details");
                txt_Outgoing_server.Focus();
                return false;
            }
            else if (txt_User_Name.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter User Name");
                txt_User_Name.Focus();
                return false;
            }
            else if (txt_password.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Password");
                txt_password.Focus();
                return false;
            }

            else if (((txt_IS.Text != "" && (Convert.ToInt32(txt_IS.Text.Length) > 4)) || txt_IS.Text == ""))
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("incoming port must be less than 4....And please enter Incoming port");
                txt_IS.Focus();
                return false;
            }

            else if (((txt_OS.Text != "" && (Convert.ToInt32(txt_OS.Text.Length) > 4) || txt_OS.Text == "")))
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Outgoing port must be less than 4.....And please enter Outgoing port");
                txt_OS.Focus();
                return false;
            }
            Regex myRegularExpression = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
            if (myRegularExpression.IsMatch(txt_FromEmailId.Text))
            {

            }
            else
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Email Address Not Valid");
                txt_FromEmailId.Focus();
                return false;
            }

            return true;
        }

        

        private void btn_TestConnectionSetting_Click(object sender, EventArgs e)
        {

            if (ValidationFromEmailId() != false)
            {
                try
                {
                    splashScreenManager1.ShowWaitForm();
                    testEmailadrdress();

                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseForm(false);
                    MessageBox.Show(ex.Message.ToString());
                }
                finally
                {
                    SplashScreenManager.CloseForm(false);
                }
            }
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "From_Email_Id")
                {

                    btn_FromEmailSave.Text = "Edit";
                    DataRow row = gridView2.GetDataRow(e.RowHandle);
                    emailId = Convert.ToInt32(row["Clarification_Email_From_Id"]);
                    txt_Display_Name.Text = row["Your_Name"].ToString();
                    Txt_Name();
                    txt_FromEmailId.Text = row["From_Email_Id"].ToString();
                    Txt_EmailAddress();
                    txt_Incoming_server.Text = row["Incoming_Mail_Server"].ToString();
                    Txt_incoming_server();
                    txt_Outgoing_server.Text = row["Outgoing_Mail_Server"].ToString();
                    Txt_Outgoingserver();
                    bool con = Convert.ToBoolean(Convert.ToInt32(row["Connection_SSL"]));
                    check_connection_SSL.Checked = con;
                    txt_User_Name.Text = row["User_Name"].ToString();
                    // Txt_username();
                    txt_password.Text = row["Password"].ToString();
                    Txt_Password();
                    txt_IS.Text = row["Incoming_Server_Port"].ToString();
                    Txt_Incomingserverport();
                    txt_OS.Text = row["Outgoing_Server_Port"].ToString();
                    Txt_Outgoingport();

                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show(ex.ToString());
            }
        }


        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }




        private void txt_User_Name_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txt_FromEmailId_Leave(object sender, EventArgs e)
        {
            txt_User_Name.Text = txt_FromEmailId.Text.ToString();
        }

        private void txt_User_Name_TextChanged(object sender, EventArgs e)
        {
            txt_User_Name.Text = txt_FromEmailId.Text.ToString();
        }

        private void txt_FromEmailId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_IS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_Display_Name_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_Incoming_server_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_Outgoing_server_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_OS_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void txt_User_Name_Enter(object sender, EventArgs e)
        {
            txt_User_Name.Text = txt_FromEmailId.Text;
        }

        private void txt_FromEmailId_TextChanged(object sender, EventArgs e)
        {
            txt_FromEmailId.Text = txt_FromEmailId.Text.Replace(" ", string.Empty);
        }

        private void txt_IS_TextChanged(object sender, EventArgs e)
        {
            txt_Incoming_server.Text = txt_Incoming_server.Text.Replace(" ", string.Empty);
        }

        private void txt_OS_TextChanged(object sender, EventArgs e)
        {
            txt_Outgoing_server.Text = txt_Outgoing_server.Text.Replace(" ", string.Empty);
        }

        private void txt_password_TextChanged(object sender, EventArgs e)
        {
            txt_password.Text = txt_password.Text.Replace(" ", string.Empty);
        }

        private void txt_password_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }





        public bool Txt_Name()
        {
            bool bStatus = true;
            if (txt_Display_Name.Text == "")
            {
                dxErrorProvider1.SetError(txt_Display_Name, "Please Enter Name");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_Display_Name, "");
            return bStatus;
        }
        private void txt_Display_Name_Validating(object sender, CancelEventArgs e)
        {
            Txt_Name();
        }


        public bool Txt_EmailAddress()
        {
            bool bStatus = true;
            Regex myRegularExpression = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
            if (myRegularExpression.IsMatch(txt_FromEmailId.Text))
            {
                dxErrorProvider1.SetError(txt_FromEmailId, "");
            }
            else
            {
                dxErrorProvider1.SetError(txt_FromEmailId, "Please Enter Valid Email-Id");
                bStatus = false;
            }
            return bStatus;
        }

        private void txt_FromEmailId_Validating(object sender, CancelEventArgs e)
        {
            Txt_EmailAddress();
        }

        public bool Txt_Incomingserverport()
        {
            bool bStatus = true;
            if (txt_IS.Text == "")
            {
                dxErrorProvider1.SetError(txt_IS, "Please enter the incoming server port number");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_IS, "");
            return bStatus;
        }
        private void txt_IS_Validating(object sender, CancelEventArgs e)
        {
            Txt_Incomingserverport();
        }


        public bool Txt_incoming_server()
        {
            bool bStatus = true;
            if (txt_Incoming_server.Text == "")
            {
                dxErrorProvider1.SetError(txt_Incoming_server, "Please Enter incoming mail server");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_Incoming_server, "");
            return bStatus;
        }
        private void txt_Incoming_server_Validating(object sender, CancelEventArgs e)
        {
            Txt_incoming_server();
        }

        public bool Txt_Outgoingserver()
        {
            bool bStatus = true;
            if (txt_Outgoing_server.Text == "")
            {
                dxErrorProvider1.SetError(txt_Outgoing_server, "Please Enter Outgoing mail server");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_Outgoing_server, "");
            return bStatus;
        }
        private void txt_Outgoing_server_Validating(object sender, CancelEventArgs e)
        {
            Txt_Outgoingserver();
        }

        public bool Txt_Outgoingport()
        {
            bool bStatus = true;
            if (txt_OS.Text == "")
            {
                dxErrorProvider1.SetError(txt_OS, "Please nter outgoing server port number");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_OS, "");
            return bStatus;
        }

        private void txt_OS_Validating(object sender, CancelEventArgs e)
        {
            Txt_Outgoingport();
        }

        public bool Txt_Password()
        {
            bool bstatus = true;
            if (txt_password.Text == "")
            {
                dxErrorProvider1.SetError(txt_password, "Please enter the password");
                bstatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_password, "");
            return bstatus;
        }
        private void txt_password_Validating(object sender, CancelEventArgs e)
        {
            Txt_Password();
        }


        private async void btn_FromEmailSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidationFromEmailId() != false)
                {
                    if (btn_FromEmailSave.Text == "Save" && (await Usercheck()) != false)
                    {

                        txt_IS.Enabled = true;
                        txt_OS.Enabled = true;
                        var dictionary = new Dictionary<string, object>();
                        {
                            dictionary.Add("@Trans", "FROM_EMAIL_INSERT");
                            dictionary.Add("@Your_Name", txt_Display_Name.Text);
                            dictionary.Add("@From_Email_Id", txt_FromEmailId.Text);
                            dictionary.Add("@Incoming_Mail_Server", txt_Incoming_server.Text);
                            dictionary.Add("@Outgoing_Mail_Server", txt_Outgoing_server.Text);
                            dictionary.Add("@Incoming_Server_Port", txt_IS.Text);
                            dictionary.Add("@Outgoing_Server_Port", txt_OS.Text);
                            dictionary.Add("@User_Name", txt_User_Name.Text);
                            dictionary.Add("@Password", txt_password.Text);
                            dictionary.Add("@Connection_SSL", check_connection_SSL.Checked);
                        };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/InsertFromEmail", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(txt_User_Name.Text + " Created Successfully ");
                                    grid_Email_Address_list();
                                    ClearFromEmail();
                                }
                            }
                        }


                    }
                    if (btn_FromEmailSave.Text == "Edit")
                    {

                        var dictionary1 = new Dictionary<string, object>();
                        {

                            dictionary1.Add("@Trans", "FROM_EMAIL_UPDATE");
                            dictionary1.Add("@Clarification_Email_From_Id", emailId);
                            dictionary1.Add("@Your_Name", txt_Display_Name.Text);
                            dictionary1.Add("@From_Email_Id", txt_FromEmailId.Text);
                            dictionary1.Add("@Incoming_Mail_Server", txt_Incoming_server.Text);
                            dictionary1.Add("@Outgoing_Mail_Server", txt_Outgoing_server.Text);
                            dictionary1.Add("@Connection_SSL", check_connection_SSL.Checked);
                            dictionary1.Add("@Incoming_Server_Port", txt_IS.Text);
                            dictionary1.Add("@Outgoing_Server_Port", txt_OS.Text);
                            dictionary1.Add("@User_Name", txt_User_Name.Text);
                            dictionary1.Add("@Password", txt_password.Text);
                        };

                        var data = new StringContent(JsonConvert.SerializeObject(dictionary1), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PutAsync(Base_Url.Url + "/ClarificationSetting/UpdateFromEmail", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(txt_User_Name.Text + " Updated Successfully ");
                                    grid_Email_Address_list();
                                    ClearFromEmail();
                                    btn_FromEmailSave.Text = "Save";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {

            }
        }

        private void btn_FromEmailCLear_Click(object sender, EventArgs e)
        {

            btn_FromEmailSave.Text = "Submit";
            ClearFromEmail();
        }

        private async void btn_FromEmailDelete_Click(object sender, EventArgs e)
        {
            string Email_ID = txt_FromEmailId.Text;
            try
            {
                if (txt_FromEmailId.Text != "")
                {
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "FROM_EMAIL_DELETE" },
                  { "@From_Email_Id", Email_ID }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/DeleteEmail", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                gridControl1_From_Email.DataSource = dt;
                                int count = dt.Rows.Count;
                                grid_Email_Address_list();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Record Deleted Successfully");
                                ClearFromEmail();
                            }
                        }
                    }
                }
                else
                {

                    XtraMessageBox.Show("Please Select the Email Address");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void check_ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (check_ShowPassword.Checked)
            {
                txt_password.Properties.PasswordChar = default(char);
            }
            else
            {
                txt_password.Properties.PasswordChar = '*';
            }
        }




        //TO EMail ID Settings

        //To Email Settings    
        public bool Txt_ToEmailAddress()
        {
            bool bStatus = true;
            Regex myRegularExpression = new Regex("^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$");
            if (myRegularExpression.IsMatch(txt_ToEmailId.Text))
            {
                dxErrorProvider1.SetError(txt_ToEmailId, "");
            }
            else
            {
                dxErrorProvider1.SetError(txt_ToEmailId, "Please Enter Valid Email-Id");
                bStatus = false;
            }
            return bStatus;
        }
        private void txt_ToEmailId_Validating(object sender, CancelEventArgs e)
        {
            Txt_ToEmailAddress();
        }

        public async Task<bool> CheckEmail()
        {
            DataTable dt = new DataTable();
            try
            {

                if (txt_ToEmailId.Text != "")
                {
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "To_EmailCheck" },
                    { "@To_Email_Id", txt_ToEmailId.Text.Trim()}
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/CheckEmail", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                int count = Convert.ToInt32(dt1.Rows[0]["count"].ToString());
                                if (count > 0)
                                {
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("E-mail Already Exists");
                                    return false;
                                }

                            }


                        }

                    }

                }
                return true;
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
   

        private async void ToEmailBindData()
        {
            try
            {
                var dictionarybind = new Dictionary<string, object>();
                {
                    dictionarybind.Add("@Trans", "To_Email_Select");
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictionarybind), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindToEmail", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count >= 0)
                            {
                                gridControl1_To_Email.DataSource = dt;
                                gridControl1_To_Email.ForceInitialize();

                            }
                            else
                            {
                                gridControl1_To_Email.DataSource = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString());
            }

        }

        private void txt_ToEmailId_Validating_1(object sender, CancelEventArgs e)
        {
            Txt_EmailAddress();
        }

        private bool validateToEmail()
        {
             if (txt_ToEmailId.Text == "")
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Email_Address");
                txt_ToEmailId.Focus();
                return false;
            }
            return true;
        }
        private async void btn_ToEmailSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (validateToEmail()!=false && (await CheckEmail()) != false)
                {


                    var dictionaryinsert = new Dictionary<string, object>();
                    {
                        dictionaryinsert.Add("@Trans", "To_Email_Save");
                        dictionaryinsert.Add("@To_Email_Id", txt_ToEmailId.Text);

                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictionaryinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/InsertToEmail", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show(txt_ToEmailId.Text + " Inserted Successfully ");
                                ToEmailBindData();
                                txt_ToEmailId.Text = "";
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {

            }

        }

        private void btn_ToEmailClear_Click_1(object sender, EventArgs e)
        {
            txt_ToEmailId.Text = "";
        }

        private void splitContainer3_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        // Client Email Settings Tab

        private async void Bind_From_Email()
        {
            try
            {
                
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_select = new Dictionary<string, object>();
                {
                    dict_select.Add("@Trans", "Bind_From_Email");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict_select), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindFromEmail", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            //DataRow dr = dt.NewRow();
                            //dr[0] = "SELECT";
                            // dt.Rows.InsertAt(dr, 0);
                            lookupedit_Client_FromId.Properties.DataSource = dt;
                            lookupedit_Client_FromId.Properties.ValueMember = "From_Email_Id";
                            lookupedit_Client_FromId.Properties.DisplayMember = "From_Email_Id";

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


        private async void Bind_To_Email()
        {
            try
            {
                
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                IDictionary<string, object> dict_select = new Dictionary<string, object>();
                {
                    dict_select.Add("@Trans", "Bind_To_Email");
                }
                var data = new StringContent(JsonConvert.SerializeObject(dict_select), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindToEmail", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            //DataRow dr = dt.NewRow();
                            //dr[0] = "SELECT";
                            //dt.Rows.InsertAt(dr, 0);
                            lookupedit_Client_ToEmail.Properties.DataSource = dt;
                            lookupedit_Client_ToEmail.Properties.ValueMember = "To_Email_Id";
                            lookupedit_Client_ToEmail.Properties.DisplayMember = "To_Email_Id";

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



        private async void BindClient()
        {
            try
            {
                
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (User_Role == "2")
                {
                    var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_CLIENT" },
                    //{"@To_Email_Id",lookupedit_Client_ToEmail.EditValue },
                    //{"@From_Email_Id",lookupedit_Client_FromId.EditValue }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        checkedboxlist_Client.DataSource = dt;
                                        checkedboxlist_Client.DisplayMember = "Client_Number";
                                        checkedboxlist_Client.ValueMember = "Client_Id";
                                    }
                                }
                            }
                        }
                    }
                }
                else if (User_Role == "1")
                {
                    var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_CLIENT" }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        checkedboxlist_Client.DataSource = dt;
                                        checkedboxlist_Client.DisplayMember = "Client_Name";
                                        checkedboxlist_Client.ValueMember = "Client_Id";
                                    }
                                }
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
        private async void BindAllClient(int ClientID)
        {
            try
            {

                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (User_Role == "2")
                {
                    var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ALL_CLIENT" },                  
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindAllClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        checkedboxlist_Client.DataSource = dt;
                                        checkedboxlist_Client.DisplayMember = "Client_Number";
                                        checkedboxlist_Client.ValueMember = "Client_Id";
                                        checkedboxlist_Client.SelectedValue = ClientID;
                                        _Client = checkedboxlist_Client.SelectedIndex;
                                        checkedboxlist_Client.SetItemChecked(_Client, true);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (User_Role == "1")
                {
                    var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_ALL_CLIENT" }
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindAllClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        checkedboxlist_Client.DataSource = dt;
                                        checkedboxlist_Client.DisplayMember = "Client_Name";
                                        checkedboxlist_Client.ValueMember = "Client_Id";
                                        checkedboxlist_Client.SelectedValue = ClientID;
                                        _Client = checkedboxlist_Client.SelectedIndex;
                                        checkedboxlist_Client.SetItemChecked(_Client, true);
                                    }
                                }
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
        public async Task<bool> CheckClient()
        {
            DataTable dt = new DataTable();
            try
            {


                if (checkedboxlist_Client.SelectedValue != null)
                {
                    DataRowView r1 = checkedboxlist_Client.GetItem(checkedboxlist_Client.SelectedIndex) as DataRowView;
                    int Client = Convert.ToInt32(r1["Client_Id"]);
                    var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "Check_Client" },
                    { "@Client_Id", Client}
                };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/CheckClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                int count = Convert.ToInt32(dt1.Rows[0]["count"].ToString());
                                if (count > 0)
                                {
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Client Already Exists");
                                    return false;
                                }

                            }


                        }

                    }

                }
                return true;
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


        private bool validateClient()
        {
            if (checkedboxlist_Client.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select Client");
                return false;
            }
            if (lookupedit_Client_FromId.EditValue == null)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select From Email Id");
                return false;
            }
            if (lookupedit_Client_ToEmail.EditValue == null)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select To Email Id");
                return false;
            }

            return true;
        }

        private void ClearClient()
        {

            checkedboxlist_Client.UnCheckAll();
            lookupedit_Client_FromId.EditValue = null;
            lookupedit_Client_ToEmail.EditValue = null;
            btn_ClientEmailSave.Text = "Save";
            BindClient();


        }

        private async void BindClientGrid()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);

                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "SELECT_CLIENT_DETAILS"}
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindClientGrid", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt.Rows.Count > 0)
                            {
                                gridControl_Client.DataSource = dt;
                                gridView4.BestFitColumns();

                            }
                            else
                            {
                                gridControl_Client.DataSource = null;
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

        private async void btn_ClientEmailSave_Click(object sender, EventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                int Client = 0;
                if (btn_ClientEmailSave.Text == "Save" && validateClient() != false && (await CheckClient()) != false)
                {
                    DataRowView r1 = checkedboxlist_Client.GetItem(checkedboxlist_Client.SelectedIndex) as DataRowView;
                    Client = Convert.ToInt32(r1["Client_Id"]);

                    var dictionary = new Dictionary<string, object>
                    {
                        {"@Trans","Insert_Client" },
                        {"@Client_Id",Client},
                        {"@From_Email_Id", lookupedit_Client_FromId.EditValue.ToString()},
                        {"@To_Email_Id" ,lookupedit_Client_ToEmail.EditValue.ToString()},
                        {"@Inserted_By",User_Role }

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/InsertClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Client Inserted Successfully");
                                BindClientGrid();
                                ClearClient();
                            }
                        }
                    }
                }
                else if (btn_ClientEmailSave.Text == "Edit" && validateClient() != false)
                {
                    DataRowView r1 = checkedboxlist_Client.GetItem(checkedboxlist_Client.SelectedIndex) as DataRowView;
                    Client = Convert.ToInt32(r1["Client_Id"]);

                    var dictionary = new Dictionary<string, object>
                    {
                        {"@Trans","Update_Client" },
                        {"@U_Id",UID },
                        {"@Client_Id",Client},
                        {"@From_Email_Id", lookupedit_Client_FromId.EditValue.ToString()},
                        {"@To_Email_Id" ,lookupedit_Client_ToEmail.EditValue.ToString()},
                        {"@Inserted_By",User_Role }

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PutAsync(Base_Url.Url + "/ClarificationSetting/UpdateClient", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Client Updated Successfully");
                                btn_ClientEmailSave.Text = "Save";
                                BindClientGrid();
                                ClearClient();

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

        private async void gridView4_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                if (e.Column.FieldName == "Client_Id")
                {
                    checkedboxlist_Client.UnCheckAll();
                    btn_ClientEmailSave.Text = "Edit";
                    DataRow row = gridView4.GetDataRow(e.RowHandle);
                    UID = int.Parse(row["U_Id"].ToString());             
                    int ClientID = int.Parse(row["Client_Id"].ToString());
                    var dictionary = new Dictionary<string, object>
                    {
                        {"@Trans","CLIENT_DETAILS" },
                        {"@Client_Id",ClientID}

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/BindClientDetails", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    lookupedit_Client_FromId.EditValue = dt.Rows[0]["From_Email_Id"];
                                    lookupedit_Client_ToEmail.EditValue = dt.Rows[0]["To_Email_Id"];
                                    BindAllClient(ClientID);                               
                                    //checkedboxlist_Client.SelectedValue = ClientID;
                                    //    _Client = checkedboxlist_Client.SelectedIndex;
                                    //    checkedboxlist_Client.SetItemChecked(_Client, true);

                                }
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

        private async void btn_ClientEmailClear_Click_1(object sender, EventArgs e)
        {
            BindClientGrid();
            ClearClient();
        }

        private bool _ValidateClient()
        {
            if (checkedboxlist_Client.CheckedItems.Count == 0||lookupedit_Client_FromId.EditValue==null||lookupedit_Client_ToEmail.EditValue==null)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Select Client details to delete");
                return false;
            }
            return true;
        }

        private async void btn_ClientEmailDelete_Click_1(object sender, EventArgs e)
        {
            if (_ValidateClient() != false)
            {

                string message = "Do you want to delete?";
                string title = "Delete Record";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult show = XtraMessageBox.Show(message, title, buttons);
                if (show == DialogResult.Yes)
                {

                    try
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        var dictionary = new Dictionary<string, object>()
                {
                   { "@Trans", "CLIENT_DELETE" },
                    { "@U_Id", UID }
                };
                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/ClarificationSetting/DeleteClient", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                                    gridControl1_From_Email.DataSource = dt;
                                    int count = dt.Rows.Count;
                                    grid_Email_Address_list();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show("Record Deleted Successfully");
                                    BindClientGrid();
                                    ClearClient();
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
             
            }
        }

        private void checkedboxlist_Client_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State != CheckState.Checked) return;
            CheckedListBoxControl lb = sender as CheckedListBoxControl;
            for (int i = 0; i < lb.ItemCount; i++)
            {
                if (i != e.Index)
                    lb.SetItemChecked(i, false);
            }
        }
    }
}
