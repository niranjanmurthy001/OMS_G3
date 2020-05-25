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

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Error_Settings : DevExpress.XtraEditors.XtraForm
    {
        public Error_Settings()
        {
            InitializeComponent();
        }



       
        private void Error_Settings_Load(object sender, EventArgs e)
        {

            grd_Error_Type.Visible = true;
            Grd_ErrorDes.Visible = false;
            grdErrorTab.Visible = false;

        }
        private void Tile_Item_ErrorType_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorType.Checked = true;
            Tile_Item_ErrorTab.Checked = false;
            Tile_Item_ErrorField.Checked = false;

            grd_Error_Type.Visible = true;
            Grd_ErrorDes.Visible = false;
            grdErrorTab.Visible = false;
        }

        private void Tile_Item_ErrorTab_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorType.Checked = false;
            Tile_Item_ErrorTab.Checked = true;
            Tile_Item_ErrorField.Checked = false;
            grdErrorTab.Visible = true;
            grd_Error_Type.Visible = false;
            Grd_ErrorDes.Visible = false;

        }

        private void Tile_Item_ErrorField_ItemClick(object sender, TileItemEventArgs e)
        {
            Tile_Item_ErrorType.Checked = false;
            Tile_Item_ErrorTab.Checked = false;
            Tile_Item_ErrorField.Checked = true;
            Grd_ErrorDes.Visible = true;
            grdErrorTab.Visible = false;
            grd_Error_Type.Visible = false;
         
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Grd_ErrorDes_Click(object sender, EventArgs e)
        {

        }
    }
}