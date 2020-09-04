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

namespace Ordermanagement_01.Opp
{
    public partial class ButtonReorder : DevExpress.XtraEditors.XtraForm
    {
        bool isDragged = false;
        Point ptOffset, ptStartPosition;
        public ButtonReorder()
        {
            InitializeComponent();
        }

        private void btn_Save_DragOver(object sender, DragEventArgs e)
        {
            FlowLayoutPanel p = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = p.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next picturebox
            SimpleButton q = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            p.Controls.SetChildIndex(q, myIndex);
        }

        private void btn__Edit_DragOver(object sender, DragEventArgs e)
        {
            FlowLayoutPanel p = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = p.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next picturebox
            SimpleButton q = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            p.Controls.SetChildIndex(q, myIndex);
        }

        private void btn_Delete_DragOver(object sender, DragEventArgs e)
        {
            FlowLayoutPanel p = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = p.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next picturebox
            SimpleButton q = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            p.Controls.SetChildIndex(q, myIndex);
        }

        private void btn_Save_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
        }

        private void btn__Edit_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
        }

        private void btn_Delete_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
        }

        private void simpleButton1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = simpleButton1.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = simpleButton1.Location.X - ptStartPosition.X;
                ptOffset.Y = simpleButton1.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }

        private void simpleButton2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = simpleButton1.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = simpleButton2.Location.X - ptStartPosition.X;
                ptOffset.Y = simpleButton2.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }

        private void simpleButton3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = simpleButton3.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = simpleButton3.Location.X - ptStartPosition.X;
                ptOffset.Y = simpleButton3.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }

        private void simpleButton1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = simpleButton1.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                simpleButton1.Location = newPoint;
                // simpleButton3.Location = ptStartPosition1;
            }
        }

        private void simpleButton2_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = simpleButton2.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                simpleButton2.Location = newPoint;
                // simpleButton3.Location = ptStartPosition1;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Save");
        }

        private void btn__Edit_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Edit");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Delete");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("1");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("2");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("3");
        }

        private void simpleButton3_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = simpleButton3.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                simpleButton3.Location = newPoint;
                // simpleButton3.Location = ptStartPosition1;
            }
        }
    }
}