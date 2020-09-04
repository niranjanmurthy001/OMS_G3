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
using System.Net;
using Ordermanagement_01.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Ordermanagement_01.Masters;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.DragDrop;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraVerticalGrid.Native;

namespace Ordermanagement_01.Opp
{
    public partial class DragandDrop : DevExpress.XtraEditors.XtraForm
    {
        public DragandDrop()
        {
            InitializeComponent();
            HandleBehaviorDragDropEvents();
        }

        private void DragandDrop_Load(object sender, EventArgs e)
        {
            BindProjecttype();
        }
        private async void BindProjecttype()
        {
            try
            {
                SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                var dictionary = new Dictionary<string, object>
                {
                    {"@Trans", "SELECT_PROJECT_TYPE" }
                };
                var data = new StringContent(JsonConvert.SerializeObject(dictionary), Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(Base_Url.Url + "/Master/BindClients", data);
                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            DataTable dt = JsonConvert.DeserializeObject<DataTable>(result);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dt.Columns.Add("Buttons", typeof(string));
                                int i = 0;
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["Buttons"] = "Btn"+i; 
                                    i = i + 1;
                                }
                                gridControl1.DataSource = dt;

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

        public void HandleBehaviorDragDropEvents()
        {
            DragDropBehavior gridControlBehavior = behaviorManager1.GetBehavior<DragDropBehavior>(this.gridView1);
            gridControlBehavior.DragDrop += Behavior_DragDrop;
            gridControlBehavior.DragOver += Behavior_DragOver;
        }
        private void Behavior_DragOver(object sender, DragOverEventArgs e)
        {
            DragOverGridEventArgs args = DragOverGridEventArgs.GetDragOverGridEventArgs(e);
            e.InsertType = args.InsertType;
            e.InsertIndicatorLocation = args.InsertIndicatorLocation;
            e.Action = args.Action;
            Cursor.Current = args.Cursor;
            args.Handled = true;
        }
        private void Behavior_DragDrop(object sender, DevExpress.Utils.DragDrop.DragDropEventArgs e)
        {

            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
                GridView targetGrid = e.Target as GridView;
                GridView sourceGrid = e.Source as GridView;
                if (e.Action == DragDropActions.None || targetGrid != sourceGrid)
                    return;
                DataTable sourceTable = sourceGrid.GridControl.DataSource as DataTable;

                Point hitPoint = targetGrid.GridControl.PointToClient(Cursor.Position);
                GridHitInfo hitInfo = targetGrid.CalcHitInfo(hitPoint);

                int[] sourceHandles = e.GetData<int[]>();

                int targetRowHandle = hitInfo.RowHandle;
                int targetRowIndex = targetGrid.GetDataSourceRowIndex(targetRowHandle);

                List<DataRow> draggedRows = new List<DataRow>();
                foreach (int sourceHandle in sourceHandles)
                {
                    int oldRowIndex = sourceGrid.GetDataSourceRowIndex(sourceHandle);
                    DataRow oldRow = sourceTable.Rows[oldRowIndex];
                    draggedRows.Add(oldRow);
                }

                int newRowIndex;

                switch (e.InsertType)
                {
                    case InsertType.Before:
                        newRowIndex = targetRowIndex > sourceHandles[sourceHandles.Length - 1] ? targetRowIndex - 1 : targetRowIndex;
                        for (int i = draggedRows.Count - 1; i >= 0; i--)
                        {
                            DataRow oldRow = draggedRows[i];
                            DataRow newRow = sourceTable.NewRow();
                            newRow.ItemArray = oldRow.ItemArray;
                            sourceTable.Rows.Remove(oldRow);
                            sourceTable.Rows.InsertAt(newRow, newRowIndex);
                        }
                        break;
                    case InsertType.After:
                        newRowIndex = targetRowIndex < sourceHandles[0] ? targetRowIndex + 1 : targetRowIndex;
                        for (int i = 0; i < draggedRows.Count; i++)
                        {
                            DataRow oldRow = draggedRows[i];
                            DataRow newRow = sourceTable.NewRow();
                            newRow.ItemArray = oldRow.ItemArray;
                            sourceTable.Rows.Remove(oldRow);
                            sourceTable.Rows.InsertAt(newRow, newRowIndex);
                        }
                        break;
                    default:
                        newRowIndex = -1;
                        break;
                }
                int insertedIndex = targetGrid.GetRowHandle(newRowIndex);
                targetGrid.FocusedRowHandle = insertedIndex;
                targetGrid.SelectRow(targetGrid.FocusedRowHandle);
                Update();

            }
            catch (Exception ex)
            {
                SplashScreenManager.CloseForm(false);
                XtraMessageBox.Show("Arrange Rows Properly", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SplashScreenManager.CloseForm(false);
            }


        }
    }
}