using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Test
{
    public partial class ArrangeButtons : XtraForm
    {
        bool isDragged = false;
        Point ptOffset;
        public ArrangeButtons()
        {
            InitializeComponent();
        }

        private void ArrangeButtons_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton2_MouseDown(object sender, MouseEventArgs e)
        {
            
            //if (e.Button == MouseButtons.Right)
            //{
            //    isDragged = true;
            //    Point ptStartPosition = simpleButton2.PointToScreen(new Point(e.X, e.Y));

            //    ptOffset = new Point();
            //    ptOffset.X = simpleButton2.Location.X - ptStartPosition.X;
            //    ptOffset.Y = simpleButton2.Location.Y - ptStartPosition.Y;
            //}
            //else
            //{
            //    isDragged = false;
            //}
        }

        private void simpleButton2_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged==true)
            {
                //Point newPoint = simpleButton2.PointToScreen(new Point(e.X, e.Y));
                //newPoint.Offset(ptOffset);
                //simpleButton2.Location = newPoint;
                simpleButton2.Location = e.Location;
            }
           
        }

        private void simpleButton2_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void simpleButton2_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (isDragged == true)
            {
               
                simpleButton2.Location = e.Location;
            }
        }

        private void simpleButton2_MouseDown_1(object sender, MouseEventArgs e)
        {
            isDragged = true;
        }

        private void simpleButton2_MouseUp_1(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void simpleButton1_MouseDown(object sender, MouseEventArgs e)
        {
            isDragged = true;
        }

        private void simpleButton1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragged == true)
            {

                simpleButton1.Location = e.Location;
            }
        }

        private void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
           // HarrProgressBar data = (HarrProgressBar)e.Data.GetData(typeof(HarrProgressBar));
        }
    }
}
