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
using Ordermanagement_01.Masters;
using DevExpress.XtraSplashScreen;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraEditors.Repository;

namespace Ordermanagement_01.Opp.Opp_Efficiency
{
    public partial class Client_TAT_Entry : DevExpress.XtraEditors.XtraForm
    {
        private Client_TAT_View Mainform = null;
        int user_id;
        DataTable _dtcol,dt;
        int _ProjectId, _Client, _TypeABSid, ProjectID;
        string Col_Name ,_clodata, _UserRole;
        int _categoryid, _ClientID;
        DataTable dtmulti = new DataTable();

        public Client_TAT_Entry(Form callingform, int userid, string User_Role)
        {
            InitializeComponent();           
           Mainform = callingform as Client_TAT_View;
           user_id = userid;
            _UserRole = User_Role;
        }

        private void Client_TAT_Entry_Load(object sender, EventArgs e)
        {
            BindProjectType();          
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
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ClientTAT/BindProject", data);
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
                                dr[1] = "SELECT";
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
                XtraMessageBox.Show( "Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private void ddl_Project_Type_EditValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) != 0)
            {
                chk_Client.DataSource = null;
                int ProjectID = Convert.ToInt32(ddl_Project_Type.EditValue);
                BindClient(ProjectID);
                BindColumnToGrid();
            }
            else
            {
                grd_ClientTAT.DataSource = null;
                chk_Client.DataSource = null;
            }
        }
        private async void BindClient(int projectid)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                // _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_CLIENT" },
                    {"@Project_Type_Id" ,projectid},
                      {"@User_Role",_UserRole }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClientTAT/BindClient", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (_UserRole == "1")
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        chk_Client.DataSource = dt;
                                        chk_Client.DisplayMember = "Client_Name";
                                        chk_Client.ValueMember = "Client_Id";
                                    }
                                }
                                else
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        chk_Client.DataSource = dt;
                                        chk_Client.DisplayMember = "Client_Number";
                                        chk_Client.ValueMember = "Client_Id";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SplashScreenManager.CloseForm(false);
                        XtraMessageBox.Show("Client does not exist for " + ddl_Project_Type.Text, "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private async void BindColumnToGrid()
        {
            try
            {
                _dtcol = new DataTable();
                grd_ClientTAT.DataSource = null;
                gridView_ClientTAT.Columns.Clear();
                _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","SELECT_HEADERS" },
                    {"@Project_Type_Id" ,_ProjectId}

                };
                this.grd_ClientTAT.DataSource = new DataTable();
                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ClientTAT/BindHeaders", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    Col_Name = Convert.ToString((dt.Rows[i]["Order_Type_Abbreviation"]));
                                    _dtcol.Columns.Add(Col_Name.ToString());
                                }
                                grd_ClientTAT.DataSource = _dtcol;
                                _dtcol.Rows.Add().ItemArray[0] = null;
                                for (int i = 0; i < gridView_ClientTAT.Columns.Count; i++)
                                {
                                    RepositoryItemTextEdit textEdit = new RepositoryItemTextEdit();
                                    textEdit.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
                                    textEdit.Mask.EditMask = "([1-9][0-9]{0,2})";
                                    textEdit.Mask.UseMaskAsDisplayFormat = true;
                                    grd_ClientTAT.RepositoryItems.Add(textEdit);
                                    gridView_ClientTAT.Columns[i].ColumnEdit = textEdit;
                                    gridView_ClientTAT.Columns[i].AppearanceHeader.Font = new Font(gridView_ClientTAT.Columns[i].AppearanceHeader.Font, FontStyle.Bold);
                                    gridView_ClientTAT.Columns[i].AppearanceHeader.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView_ClientTAT.Columns[i].AppearanceCell.ForeColor = Color.FromArgb(30, 57, 81);
                                    gridView_ClientTAT.Columns[i].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                    gridView_ClientTAT.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                }
                            }
                        }
                        else
                        {
                            SplashScreenManager.CloseForm(false);
                            XtraMessageBox.Show("Abbreviation does not exist for" + ddl_Project_Type.Text, "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private async void btn_Submit_Click(object sender, EventArgs e)
        {
            dtmulti.Rows.Clear();
            if (validate() == true)
            {
                try
                {
                    var dictonary = new Dictionary<string, object>()
                    {
                        {"@Trans","GET_ABS_DETAILS" }

                    };
                    var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                    using (var httpclient = new HttpClient())
                    {
                        var response = await httpclient.PostAsync(Base_Url.Url + "/ClientTAT/BindHeaders", data);
                        if (response.IsSuccessStatusCode)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                var result = await response.Content.ReadAsStringAsync();
                                DataTable dt1 = JsonConvert.DeserializeObject<DataTable>(result);
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    Col_Name = Convert.ToString(dt1.Rows[i]["Order_Type_Abbreviation"]);
                                    _TypeABSid = Convert.ToInt32(dt1.Rows[i]["OrderType_ABS_Id"]);
                                    if (IsMatch(Col_Name.ToString()))
                                    {

                                        int[] SelectedRowHandles = gridView_ClientTAT.GetSelectedRows();
                                        _ProjectId = Convert.ToInt32(ddl_Project_Type.EditValue);                                     
                                        DataRowView r1 = chk_Client.GetItem(chk_Client.SelectedIndex) as DataRowView;
                                        _Client = Convert.ToInt32(r1["Client_Id"]);                                      
                                        if (dtmulti.Columns.Count <= 0)
                                        {
                                            dtmulti.Columns.AddRange(new DataColumn[8]                                                                                  {
                                         new DataColumn("Project_Type_Id",typeof(int)),
                                         new DataColumn("Client_Id",typeof(int)),
                                          new DataColumn("Sub_Process_Id",typeof(int)),
                                         new DataColumn("Order_Type_ABS",typeof(string)),                                      
                                         new DataColumn("Allocated_Time",typeof(double)),
                                         new DataColumn("Status",typeof(bool)),
                                         new DataColumn("Inserted_By",typeof(int)),
                                         new DataColumn("Instered_Date",typeof(DateTime))
                                                                                   });
                                        }
                                        foreach (object itemsChecked in chk_Client.CheckedItems)
                                        {
                                            DataRowView castedItem = itemsChecked as DataRowView;
                                            if (_UserRole == "1")
                                            {
                                                string _Client_Name = castedItem["Client_Name"].ToString();
                                            }
                                            else
                                            {
                                                string _Client_Name = castedItem["Client_Number"].ToString();
                                            }
                                            int _Client_Id = Convert.ToInt32(castedItem["Client_Id"]);
                                            foreach (object itemChecked in chk_SubClient.CheckedItems)
                                            {
                                                DataRowView castedItems = itemChecked as DataRowView;
                                                if (_UserRole == "1")
                                                {
                                                    string _SubClient = castedItems["Sub_ProcessName"].ToString();
                                                }
                                                else
                                                {
                                                    string _SubClient = castedItems["Subprocess_Number"].ToString();
                                                }

                                                int _SubClientID = Convert.ToInt32(castedItems["Subprocess_Id"]);
                                                int projecttype = _ProjectId;
                                                string _ABS = Col_Name;
                                                string _allocatedtime = _clodata;
                                                int status = 1;
                                                int insertedby = user_id;
                                                DateTime inserteddate = DateTime.Now;
                                                dtmulti.Rows.Add(projecttype, _Client_Id, _SubClientID, _ABS, _allocatedtime, status, insertedby, inserteddate);
                                            }
                                        }                                       
                                    }
                                }
                                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                                var _data = new StringContent(JsonConvert.SerializeObject(dtmulti), Encoding.UTF8, "application/json");
                                using (var httpClient = new HttpClient())
                                {
                                    var response1 = await httpClient.PostAsync(Base_Url.Url + "/ClientTAT/Insert", _data);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        if (response1.StatusCode == HttpStatusCode.OK)
                                        {
                                            var _result = await response.Content.ReadAsStringAsync();
                                            SplashScreenManager.CloseForm(false);
                                            XtraMessageBox.Show("Submitted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                                            Clear();
                                            this.Mainform.BindClientTAT();
                                            this.Close();
                                            this.Mainform.Enabled = true;
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
        }

        private void Client_TAT_Entry_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Mainform.Enabled = true;
        }

        private void chk_Client_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void chk_Client_SelectedValueChanged(object sender, EventArgs e)
        {
          
        }

        private void chk_Client_CheckMemberChanged(object sender, EventArgs e)
        {
           
        }

        private void chk_Client_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            if (chk_Client.CheckedItemsCount == 1)
            {
                foreach (object itemchecked in chk_Client.CheckedItems)
                {
                    DataRowView CastedItems = itemchecked as DataRowView;
                    _ClientID = Convert.ToInt32(CastedItems["Client_Id"]);                  
                    BindSubClient(_ClientID);
                    chk_SubClient.Enabled = true;
                }
            }
            else if(chk_Client.CheckedItemsCount>1)
            {
                string checkedItems = "0";
                foreach (object Item in chk_Client.CheckedItems)
                {
                    var row = (Item as DataRowView).Row;
                    checkedItems= checkedItems +","+ (row["Client_Id"]);
                }
                BindMultipleSubClient(checkedItems);
              
               // chk_SubClient.Enabled = false;        
            }
            else if (chk_Client.CheckedItemsCount == 0)
            {
                chk_SubClient.UnCheckAll();
                chk_SubClient.DataSource = null;
                chk_SubClient.Enabled = true;
            }
            SplashScreenManager.CloseForm(false);
        }

        private async void BindSubClient(int ClientID)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_SUBCLIENT" },
                    {"@Client_Id" ,ClientID} ,
                      {"@User_Role",_UserRole }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClientTAT/BindSubClient", data);
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
                                    if (_UserRole == "1")
                                        {
                                        chk_SubClient.DataSource = dt;
                                        chk_SubClient.DisplayMember = "Sub_ProcessName";
                                        chk_SubClient.ValueMember = "Subprocess_Id";
                                        }                                
                                    else
                                      {
                                        chk_SubClient.DataSource = dt;
                                        chk_SubClient.DisplayMember = "Subprocess_Number";
                                        chk_SubClient.ValueMember = "Subprocess_Id";
                                      }
                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Sub Client does not exist for " + chk_Client.Text, "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }                   
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private async void BindMultipleSubClient(string ClientID)
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                chk_SubClient.DataSource = null;
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans","SELECT_MULTIPLE_SUBCLIENT" },
                    {"@Multiple_Client" ,ClientID},
                    {"@User_Role",_UserRole }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ClientTAT/BindMultipleSubClient", data);
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
                                    if (_UserRole == "1")
                                    {
                                        chk_SubClient.DataSource = dt;
                                        chk_SubClient.DisplayMember = "Sub_ProcessName";
                                        chk_SubClient.ValueMember = "Subprocess_Id";
                                        chk_SubClient.CheckAll();
                                    }
                                    else
                                    {
                                        chk_SubClient.DataSource = dt;
                                        chk_SubClient.DisplayMember = "Subprocess_Number";
                                        chk_SubClient.ValueMember = "Subprocess_Id";
                                        chk_SubClient.CheckAll();
                                    }
                                }
                            }
                            else
                            {
                                SplashScreenManager.CloseForm(false);
                                XtraMessageBox.Show("Sub Client does not exist for " + chk_Client.Text, "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please contact with Admin", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }

        private void gridView_ClientTAT_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
        }

        private bool IsMatch(string colName)
        {
            for (int i = 0; i < gridView_ClientTAT.Columns.Count; i++)
            {
                if (gridView_ClientTAT.Columns[i].ToString() == colName)
                {
                    _clodata = gridView_ClientTAT.GetRowCellValue(0, gridView_ClientTAT.Columns[i]).ToString();
                    return true;
                }
            }
            return false;
        }
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private bool validate()
        {
            if (Convert.ToInt32(ddl_Project_Type.EditValue) == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Select Project Type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddl_Project_Type.Focus();
                return false;
            }
            if (chk_Client.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chk_Client.Focus();
                return false;
            }
            if (chk_SubClient.CheckedItems.Count == 0)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Please Check Sub Client", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                chk_SubClient.Focus();
                return false;
            }
            for (int i = 0; i < gridView_ClientTAT.Columns.Count; i++)
            {
                string data = gridView_ClientTAT.GetRowCellValue(0, gridView_ClientTAT.Columns[i]).ToString();
                if (data == "")
                {
                    SplashScreenManager.CloseForm(false);
                    XtraMessageBox.Show("Please Fill All The Columns", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gridView_ClientTAT.Columns[i].AppearanceCell.BackColor = Color.Red;
                    return false;
                }
                else
                {
                    gridView_ClientTAT.Columns[i].AppearanceCell.BackColor = Color.White;
                }
            }
            return true;
        }
        private void Clear()
        {
            ddl_Project_Type.ItemIndex = 0;
            chk_Client.UnCheckAll();
            chk_Client.DataSource = null;
            _dtcol = new DataTable();
            gridView_ClientTAT.Columns.Clear();
            chk_SubClient.UnCheckAll();
            chk_SubClient.DataSource = null;
        }


    }
}