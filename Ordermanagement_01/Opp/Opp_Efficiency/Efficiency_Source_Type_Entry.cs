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
using Ordermanagement_01.Models;
using System.Net;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Efficiency_Source_Type_Entry : DevExpress.XtraEditors.XtraForm
    {
        int User_Id, _ProjectId, _ID, userid,ID;
        string _SourceType, _BtnName, _Operaion_Id;
        private Efficiency_Source_Type Mainform = null;
        public Efficiency_Source_Type_Entry(string _Oid, int ID, int ProjId, string SrcType, string btnname, int User_Id, Form CallingForm)
        {
            InitializeComponent();
            _ProjectId = ProjId;
            _ID = ID;
            _SourceType = SrcType;
            _BtnName = btnname;
            _Operaion_Id = _Oid;
            userid = User_Id;
            Mainform = CallingForm as Efficiency_Source_Type;
        }

        private void Efficiency_Source_Type_Entry_Load(object sender, EventArgs e)
        {
            Clear();
            BindProjectType();          
            if (_Operaion_Id == "View")
            {
                btn_Save_EffSource.Text = _BtnName;
                ddl_Project_Type.EditValue = _ProjectId;
                txt_Efficiency_Source.Text = _SourceType;
                userid = User_Id;
                ID = _ID;
            }
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/EfficiencySource/BindProjectType", data);
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
                            ddl_Project_Type.Properties.DataSource = dt;
                            ddl_Project_Type.Properties.DisplayMember = "Project_Type";
                            ddl_Project_Type.Properties.ValueMember = "Project_Type_Id";
                            DevExpress.XtraEditors.Controls.LookUpColumnInfo col;
                            col = new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Project_Type");
                            ddl_Project_Type.Properties.Columns.Add(col);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show( "Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void Clear()
        {
            ddl_Project_Type.EditValue = null;
            ddl_Project_Type.ItemIndex = 0;          
            txt_Efficiency_Source.Text = "";
        }
        private bool Validate()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }        
            if (string.IsNullOrWhiteSpace(txt_Efficiency_Source.Text))
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Enter Efficiency Source Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private async Task<bool> CheckEfficiencyExists()
        {
            DataTable dt = new DataTable();
            try
            {
                if (Validate() != false)
                {
                 var dictionary = new Dictionary<string, object>()
                {
                    { "@Trans", "CHECK_EXISTS" },
                    { "@Project_Type_Id", ddl_Project_Type.EditValue},                   
                    { "@Order_Source_Type_Name",txt_Efficiency_Source.Text.Trim() }
                };

                        var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencySource/Exists", data);
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
                                        XtraMessageBox.Show("Efficiency Source Type Already Exists", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async void btn_Save_EffSource_Click(object sender, EventArgs e)
        {
            try
            {                
                DateTime _inserteddate = DateTime.Now;
                if (btn_Save_EffSource.Text == "Save" && Validate() != false && (await CheckEfficiencyExists()) != false)
                {
                    var dictinsert = new Dictionary<string, object>();
                    {
                        dictinsert.Add("@Trans", "INSERT");
                        dictinsert.Add("@Project_Type_Id", ddl_Project_Type.EditValue);
                        dictinsert.Add("@Order_Source_Type_Name", txt_Efficiency_Source.Text);                       
                        dictinsert.Add("@Status", "True");
                        dictinsert.Add("@Inserted_By", userid);
                        dictinsert.Add("@Inserted_Date", _inserteddate);
                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictinsert), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PostAsync(Base_Url.Url + "/EfficiencySource/Insert", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show(" Inserted Successfully ");
                                this.Mainform.BindEfficiencySource();
                                this.Mainform.Enabled = true;
                                Clear();
                                this.Close();
                            }
                        }
                    }
                }
                if (btn_Save_EffSource.Text == "Edit" && Validate() != false && (await CheckEfficiencyExists()) != false)
                {

                    var dictionaryedit = new Dictionary<string, object>();
                    {
                        dictionaryedit.Add("@Trans", "UPDATE");
                        dictionaryedit.Add("@Order_Source_Type_ID", ID);
                        dictionaryedit.Add("@Project_Type_Id", ddl_Project_Type.EditValue);
                        dictionaryedit.Add("@Order_Source_Type_Name", txt_Efficiency_Source.Text);
                        dictionaryedit.Add("@Status", "True");
                        dictionaryedit.Add("@Modified_By", userid);
                        dictionaryedit.Add("@Modified_Date", _inserteddate);

                    }
                    var data = new StringContent(JsonConvert.SerializeObject(dictionaryedit), Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.PutAsync(Base_Url.Url + "/EfficiencySource/Update", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Edited Successfully ");
                                this.Mainform.BindEfficiencySource();
                                this.Mainform.Enabled = true;
                                Clear();
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Contact Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void btn_Clear_Eff_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Efficiency_Source_Type_Entry_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Mainform.Enabled = true;
        }
    }
}