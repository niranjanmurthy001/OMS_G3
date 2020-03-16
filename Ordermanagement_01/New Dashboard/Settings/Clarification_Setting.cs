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

namespace Ordermanagement_01.New_Dashboard.Settings
{
    public partial class Clarification_Setting : DevExpress.XtraEditors.XtraForm
    {
        int ID = 0;
        public Clarification_Setting()
        {
            InitializeComponent();
        }



        private async void BindData()
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
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Clarification_Setting/BindData", data);
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
                XtraMessageBox.Show(ex.Message.ToString());
            }

        }


        public void Clear()
        {
            txt_ClarificationCategory.Text = "";
        }

        public bool Txt_ClarificationCategory()
        {
            bool bStatus = true;
            if (txt_ClarificationCategory.Text == "")
            {
                dxErrorProvider1.SetError(txt_ClarificationCategory, "Please Enter Name");
                bStatus = false;
            }
            else
                dxErrorProvider1.SetError(txt_ClarificationCategory, "");
            return bStatus;
        }

        private bool Validation()
        {
            if (txt_ClarificationCategory.Text == "")
            {
               // SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Enter Your_Name");
                txt_ClarificationCategory.Focus();
                return false;
            }
            return true;
        }
        private async void btn_Submit_Click(object sender, EventArgs e)
        {

            try
            {
                if (Validation() !=false)
                {
                    if (ID == 0 && btn_Submit.Text == "Submit")
                    {
                        SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                        var dictionaryinsert = new Dictionary<string, object>();
                        {
                            dictionaryinsert.Add("@Trans", "INSERT");
                            dictionaryinsert.Add("@Clarification_Category_Type", txt_ClarificationCategory.Text);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryinsert), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/Clarification_Setting/Insert", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();
                                    SplashScreenManager.CloseForm(false);
                                    XtraMessageBox.Show(txt_ClarificationCategory.Text + " Updated Successfully ");
                                    BindData();
                                    Clear();
                                }
                            }
                        }


                    }
                    if (btn_Submit.Text == "Edit")
                    {
                        var dictionaryedit = new Dictionary<string, object>();
                        {
                            dictionaryedit.Add("@Trans", "UPDATE");
                            dictionaryedit.Add("@Clarification_Category_Type_Id", ID);
                            dictionaryedit.Add("@Clarification_Category_Type", txt_ClarificationCategory.Text);

                        }
                        var data = new StringContent(JsonConvert.SerializeObject(dictionaryedit), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PutAsync(Base_Url.Url + "/Clarification_Setting/Edit", data);
                            if (response.IsSuccessStatusCode)
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    var result = await response.Content.ReadAsStringAsync();

                                    XtraMessageBox.Show(" Edited Successfully ");
                                    BindData();
                                    Clear();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // SplashScreenManager.CloseForm(false);
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                // SplashScreenManager.CloseForm(false);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }


        private void Clarification_Setting_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private async void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
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
                        var response = await httpClient.PostAsync(Base_Url.Url + "/Clarification_Setting/Delete", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();

                                XtraMessageBox.Show("Record Deleted Successfully");
                                BindData();
                                Clear();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                this.Close();
            }
        }


        private void grd_Clarifacation_Category_Click(object sender, EventArgs e)
        {
            //int RowIndex = gridView1.FocusedColumn.VisibleIndex;

            //System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
            //txt_ClarificationCategory.Text = row["Clarification_Category_Type"].ToString();
            //BindData();

        }
        private async void repositoryItemHyperLinkEdit2_Click(object sender, EventArgs e)
        {
            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
            txt_ClarificationCategory.Text = row["Clarification_Category_Type"].ToString();
            btn_Submit.Text = "Edit";

        }

        private async void gridView1_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //    try
            //    {
            //        if (e.Column.FieldName == "Edit")
            //        {
            //            System.Data.DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            //            ID = int.Parse(row["Clarification_Category_Type_Id"].ToString());
            //            txt_ClarificationCategory.Text = row["Clarification_Category_Type"].ToString();
            //            BindData();
            //            btn_Submit.Text = "Edit";
            //        }
          
        }

        private void txt_name_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_OS_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_IS_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_password_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_Outgoing_server_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_Incoming_server_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void txt_Email_address_Properties_Leave(object sender, EventArgs e)
        {

        }

        private void btn_save_Click(object sender, EventArgs e)
        {

        }

     
    }
}