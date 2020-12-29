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
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Models;
using System.Net;
using DevExpress.XtraSplashScreen;
using Ordermanagement_01.Masters;
using DevExpress.LookAndFeel;

namespace Ordermanagement_01.Opp
{
    public partial class ButtonReorder : DevExpress.XtraEditors.XtraForm
    {
        bool isDragged = false;
        Point ptOffset, ptStartPosition;
        int user_id, ProjectType_Id;
        DataTable dt_dynamic_Buttons;
        public ButtonReorder(int User_Id, int Project_Type_Id)
        {
            InitializeComponent();
            user_id = User_Id;
            ProjectType_Id = Project_Type_Id;
        }
        private void ButtonReorder_Load(object sender, EventArgs e)
        {
            CheckUserExist();
        }

        private async void CheckUserExist()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictonary = new Dictionary<string, object>()
                {
                    {"@Trans","CHECK_USER" },
                    {"@User_Id" ,user_id},
                    {"@Project_Type_Id", ProjectType_Id }
                };

                var data = new StringContent(JsonConvert.SerializeObject(dictonary), Encoding.UTF8, "Application/Json");
                using (var httpclient = new HttpClient())
                {
                    var response = await httpclient.PostAsync(Base_Url.Url + "/ButtonReorder/CheckUser", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            int count = Convert.ToInt32(dt.Rows[0]["Count"].ToString());
                            if (count != 0)
                            {
                                ReorderDynamicButtons();
                            }
                            else
                            {
                                LoadButtons();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Error", "Please contact with Admin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }
        }
        private async void LoadButtons()
        {
            try
            {
                var dictionary1 = new Dictionary<string, object>()
                             {
                                { "@Trans", "LOAD_DYNAMIC_BUTTONS"},
                                {"@Project_Type_Id", ProjectType_Id },
                                {"@User_Id", user_id }
                             };
                var data1 = new StringContent(JsonConvert.SerializeObject(dictionary1), Encoding.UTF8, "application/json");
                using (var httpClient1 = new HttpClient())
                {
                    var response1 = await httpClient1.PostAsync(Base_Url.Url + "/ButtonReorder/BindButtons", data1);

                    if (response1.IsSuccessStatusCode)
                    {
                        if (response1.StatusCode == HttpStatusCode.OK)
                        {
                            var result1 = await response1.Content.ReadAsStringAsync();
                            DataTable dt_btns = JsonConvert.DeserializeObject<DataTable>(result1);
                            if (dt_btns.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt_btns.Rows.Count; j++)
                                {
                                    SimpleButton newButton = new SimpleButton();
                                    {
                                        newButton.Size = new System.Drawing.Size(75, 35);
                                        newButton.AllowDrop = true;
                                        flowLayoutPanel1.AllowDrop = true;
                                        newButton.DragOver += btn_dragover;
                                        newButton.MouseDown += btn_mousedown;
                                        newButton.Text = dt_btns.Rows[j]["Order_Status"].ToString();
                                        newButton.Name = "btn_" + dt_btns.Rows[j]["Order_Status"].ToString();
                                        flowLayoutPanel1.Controls.Add(newButton);
                                    }
                                }
                               // SaveDynamicButtonIndex();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private async void SaveDynamicButtonIndex()
        {
            try
            {
                DataTable dtinsert = new DataTable();
                dtinsert.Columns.AddRange(new DataColumn[10]
                     {
                     new DataColumn("Button_Name",typeof(string)),
                     new DataColumn("Button_Text",typeof(string)),
                     new DataColumn("Button_Location",typeof(string)),
                     new DataColumn("Button_Index",typeof(int)),
                     new DataColumn("Inserted_By",typeof(int)),
                     new DataColumn("Inserted_Date",typeof(DateTime)),
                     new DataColumn("Modified_By",typeof(int)),
                     new DataColumn("Modified_Date",typeof(DateTime)),
                     new DataColumn("User_Id",typeof(int)),
                     new DataColumn("Project_Type_Id",typeof(int))
                     });
                foreach (Control ctrl in flowLayoutPanel1.Controls)
                {
                    //foreach (object items in flowLayoutPanel1.Container.Components)
                    //{
                    //   int btn_Id = ;
                    string btn_Name = ctrl.Name;
                    string btn_Text = ctrl.Text;
                    int btn_Loc_X = (ctrl.Location).X;
                    int btn_Loc_Y = (ctrl.Location).Y;
                    string btn_Location = (btn_Loc_X + "," + btn_Loc_Y).ToString();
                    int index = flowLayoutPanel1.Controls.GetChildIndex((ctrl));
                    dtinsert.Rows.Add(btn_Name, btn_Text, btn_Location, index, user_id, DateTime.Now, user_id, DateTime.Now, user_id, ProjectType_Id);
                }
                var data = new StringContent(JsonConvert.SerializeObject(dtinsert), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ButtonReorder/InsertButtonsDynamically", data);
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
                XtraMessageBox.Show("Problem In Reordering\n\t" + " Please try again");
            }
        }

        public async void ReorderDynamicButtons()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>()
                    {
                        { "@Trans", "LOAD_BUTTONS_BY_INDEX"},
                        { "@User_Id", user_id },
                        {"@Project_Type_Id", ProjectType_Id }
                    };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/ButtonReorder/BindButtonsByLocation", data);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            dt_dynamic_Buttons = dt.Copy();
                            if (dt.Rows.Count != 0)
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    SimpleButton newButton = new SimpleButton();
                                    {
                                        newButton.Size = new System.Drawing.Size(75, 35);
                                        newButton.AllowDrop = true;
                                        flowLayoutPanel1.AllowDrop = true;
                                        newButton.DragOver += btn_dragover;
                                        newButton.MouseDown += btn_mousedown;
                                        flowLayoutPanel1.Controls.Add(newButton);
                                    }
                                }
                                int j = 0;
                                foreach (Control ctrl in flowLayoutPanel1.Controls)
                                {
                                    ctrl.Name = dt.Rows[j]["Button_Name"].ToString();
                                    ctrl.Text = dt.Rows[j]["Button_Text"].ToString();
                                    string loc_X_Y = dt.Rows[j]["Button_Location"].ToString();
                                    string[] loc = loc_X_Y.Split(',');
                                    int X = Convert.ToInt32(loc[0]);
                                    int Y = Convert.ToInt32(loc[1]);
                                    ctrl.Location = new Point(X, Y);
                                    //  flowLayoutPanel1.Controls.SetChildIndex(ctrl, Convert.ToInt32(dt.Rows[i]["Button_Index"]));                                        
                                    j++;
                                }
                            }
                        }
                    }
                }
                            var dictionary1 = new Dictionary<string, object>()
                             {
                                { "@Trans", "GET_BUTTONS"},
                                { "@User_Id", user_id },
                                {"@Project_Type_Id", ProjectType_Id }
                             };
                            var data1 = new StringContent(JsonConvert.SerializeObject(dictionary1), Encoding.UTF8, "application/json");
                            using (var httpClient1 = new HttpClient())
                            {
                                var response1 = await httpClient1.PostAsync(Base_Url.Url + "/ButtonReorder/BindButtons", data1);

                                if (response1.IsSuccessStatusCode)
                                {
                                    if (response1.StatusCode == HttpStatusCode.OK)
                                    {
                                        var result1 = await response1.Content.ReadAsStringAsync();
                                        DataTable dt_btns = JsonConvert.DeserializeObject<DataTable>(result1);

                                        if (dt_btns.Rows.Count > 0)
                                        {
                                            for (int j = 0; j < dt_btns.Rows.Count; j++)
                                            {
                                                SimpleButton newButton = new SimpleButton();
                                                        {
                                                            newButton.Size = new System.Drawing.Size(75, 35);
                                                            newButton.AllowDrop = true;
                                                            flowLayoutPanel1.AllowDrop = true;
                                                            newButton.DragOver += btn_dragover;
                                                            newButton.MouseDown += btn_mousedown;                                                         
                                                            newButton.Text = dt_btns.Rows[j]["Order_Status"].ToString();
                                                            newButton.Name = "btn_" + dt_btns.Rows[j]["Order_Status"].ToString();
                                                        }
                                                        flowLayoutPanel1.Controls.Add(newButton);
                                                    }
                                                }
                                        // SaveDynamicButtonIndex();
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

        //private void btn_Submit_Click(object sender, EventArgs e)
        //{
        //    int tot_buttons = Convert.ToInt16(txt_Btns.Text);

        //    DataTable dt = dt_dynamic_Buttons;
        //    int k = 0 + dt.Rows.Count;
        //    // int loc = 5 + dt.Rows[]["Button_Location"];
        //    //DataRow[] dr = dt.Select("[To (m)] = MAX([To (m)])");

        //    int max = 0;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        string Location = dr.Field<string>("Button_Location");
        //        string[] loc = Location.Split(',');
        //        int X = Convert.ToInt32(loc[0]);
        //        max = Math.Max(max, X);
        //    }
        //        //
        //        int xlocation =5+ max;
        //    for (int i = 1; i <= tot_buttons; i++)
        //    {
        //        SimpleButton newButton = new SimpleButton();
        //        {
        //            newButton.Name = string.Format("Button{0}", i + k);
        //            newButton.Text = string.Format("Button {0}", i + k);
        //            newButton.Location = new System.Drawing.Point(xlocation, 10);
        //            newButton.Size = new System.Drawing.Size(75, 35);
        //            newButton.AllowDrop = true;
        //            flowLayoutPanel1.AllowDrop = true;
        //            newButton.DragOver += btn_dragover;
        //            newButton.MouseDown += btn_mousedown;
        //            flowLayoutPanel1.Controls.Add(newButton);
        //        }
        //        xlocation = xlocation + 85;
        //    }
        //}

        private void btn_dragover(object sender, DragEventArgs e)
        {
            FlowLayoutPanel dynamic_flp = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = dynamic_flp.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next button
            SimpleButton btn = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            dynamic_flp.Controls.SetChildIndex(btn, myIndex);
            SaveDynamicButtonIndex();
        }

        private void btn_mousedown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
        }


    }
}