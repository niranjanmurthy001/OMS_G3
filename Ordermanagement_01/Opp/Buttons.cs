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
    public partial class Buttons : DevExpress.XtraEditors.XtraForm
    {
        bool isDragged = false;
        Point ptOffset, ptStartPosition1;
        public Buttons()
        {
            InitializeComponent();
        }

        private void Buttons_Load(object sender, EventArgs e)
        {
            //// When adding and removing controls, the order is not kept.
            //var runsOrderedByStartDate = this.flowLayoutPanel1.Controls.Cast<RunNodeControl>().Select(_ => new { StartDate = _.StartDateTime, RunControl = _ }).OrderBy(_ => _.StartDate).ToList();

            //// Sets index of controls according to their index in the ordered collection
            //foreach (var anonKeyValue in runsOrderedByStartDate)
            //{
            //    this.flowLayoutPanel1.Controls.SetChildIndex(anonKeyValue.RunControl, runsOrderedByStartDate.IndexOf(anonKeyValue));
            //}
        }

        private void simpleButton1_MouseDown(object sender, MouseEventArgs e)
        {
            // simpleButton1.DoDragDrop(sender, DragDropEffects.Move);
            //  simpleButton1.DoDragDrop(simpleButton1.Text, DragDropEffects.Copy |
            //DragDropEffects.Move);
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition1 = simpleButton1.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = simpleButton1.Location.X - ptStartPosition1.X;
                ptOffset.Y = simpleButton1.Location.Y - ptStartPosition1.Y;
            }
            else
            {
                isDragged = false;
            }

            //base.OnMouseDown(e);
            //DoDragDrop(sender, DragDropEffects.All);
        }

        private void simpleButton1_DragEnter(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(DataFormats.StringFormat))
            //{
            //    e.Effect = DragDropEffects.Move;
            //}
            //else
            //{
            //    e.Effect = DragDropEffects.None;
            //}
        }

        private void simpleButton2_MouseDown(object sender, MouseEventArgs e)
        {
            //////button1.DoDragDrop(button1, DragDropEffects.Copy | DragDropEffects.Move);
            ////simpleButton2.DoDragDrop(simpleButton2, DragDropEffects.Move);
            //simpleButton2.DoDragDrop(sender, DragDropEffects.Move);
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = simpleButton2.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = simpleButton2.Location.X - ptStartPosition.X;
                ptOffset.Y = simpleButton2.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }

        private void simpleButton1_MouseMove(object sender, MouseEventArgs e)
        {
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

        private void btn_Save_MouseDown(object sender, MouseEventArgs e)
        {
            ////////  simpleButton1.DoDragDrop(simpleButton1.Text, DragDropEffects.Copy |
            ////////DragDropEffects.Move);
            //////if (e.Button == MouseButtons.Left)
            //////{
            //////    isDragged = true;
            //////    Point ptStartPosition = btn_Save.PointToScreen(new Point(e.X, e.Y));

            //////    ptOffset = new Point();
            //////    ptOffset.X = btn_Save.Location.X - ptStartPosition.X;
            //////    ptOffset.Y = btn_Save.Location.Y - ptStartPosition.Y;
            //////}
            //////else
            //////{
            //////    isDragged = false;
            //////}
            ////base.OnMouseDown(e);
            ////DoDragDrop(sender, DragDropEffects.All);

            //if (e.Button == MouseButtons.Left)
            //{
            //    isDragged = true;
            //    Point ptStartPosition = simpleButton1.PointToScreen(new Point(e.X, e.Y));

            //    ptOffset = new Point();
            //    ptOffset.X = simpleButton1.Location.X - ptStartPosition.X;
            //    ptOffset.Y = simpleButton1.Location.Y - ptStartPosition.Y;
            //}
            //else
            //{
            //    isDragged = false;
            //}
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
        }

        private void btn_Save_MouseUp(object sender, MouseEventArgs e)
        {
        //    //if (isDragged)
        //    //{
        //    //    Point newPoint = btn_Save.PointToScreen(new Point(e.X, e.Y));
        //    //    newPoint.Offset(ptOffset);
        //    //    btn_Save.Location = newPoint;
        //    //}

        //    if (isDragged)
        //    {
        //        Point newPoint = simpleButton1.PointToScreen(new Point(e.X, e.Y));
        //        newPoint.Offset(ptOffset);
        //        simpleButton1.Location = newPoint;
        //    }
        }

        private void simpleButton1_DragOver(object sender, DragEventArgs e)
        {
           
        }

        private void btn_Save_DragOver(object sender, DragEventArgs e)
        {
            FlowLayoutPanel p = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = p.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next picturebox
            SimpleButton q = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            p.Controls.SetChildIndex(q, myIndex);

            //base.OnDragOver(e);
            //// is another dragable
            //if (e.Data.GetData(typeof(Button)) != null)
            //{
            //    FlowLayoutPanel p = (FlowLayoutPanel)(sender as Button).Parent;
            //    //Current Position             
            //    int myIndex = p.Controls.GetChildIndex((sender as Button));

            //    //Dragged to control to location of next picturebox
            //    Button q = (Button)e.Data.GetData(typeof(Button));
            //    p.Controls.SetChildIndex(q, myIndex);
            //}
        }

        private void btn_Save_DragEnter(object sender, DragEventArgs e)
        {
           // e.Effect = DragDropEffects.Move;
        }

        private void flowLayoutPanel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel1_DragDrop(object sender, DragEventArgs e)
        {
            //Button data = (Button)e.Data.GetData(typeof(Button));
            //FlowLayoutPanel _destination = (FlowLayoutPanel)sender;
            //FlowLayoutPanel _source = (FlowLayoutPanel)data.Parent;

            //if (_source != _destination)
            //{
            //    // Add control to panel
            //    _destination.Controls.Add(data);
            //    data.Size = new Size(_destination.Width, 50);

            //    // Reorder
            //    Point p = _destination.PointToClient(new Point(e.X, e.Y));
            //    var item = _destination.GetChildAtPoint(p);
            //    int index = _destination.Controls.GetChildIndex(item, false);
            //    _destination.Controls.SetChildIndex(data, index);

            //    // Invalidate to paint!
            //    _destination.Invalidate();
            //    _source.Invalidate();
            //}
            //else
            //{
            //    // Just add the control to the new panel.
            //    // No need to remove from the other panel,
            //    // this changes the Control.Parent property.
            //    Point p = _destination.PointToClient(new Point(e.X, e.Y));
            //    var item = _destination.GetChildAtPoint(p);
            //    int index = _destination.Controls.GetChildIndex(item, false);
            //    _destination.Controls.SetChildIndex(data, index);
            //    _destination.Invalidate();
            //}
        }

        private void simpleButton2_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void simpleButton3_MouseDown(object sender, MouseEventArgs e)
        {
            // simpleButton3.DoDragDrop(sender, DragDropEffects.Move);
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

        private void simpleButton2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void simpleButton3_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void simpleButton1_DragDrop(object sender, DragEventArgs e)
        {
            simpleButton1.Text = e.Data.GetData(DataFormats.StringFormat).ToString(); 
        }

        private void simpleButton2_DragDrop(object sender, DragEventArgs e)
        {
            simpleButton2.Text = e.Data.GetData(DataFormats.StringFormat).ToString();
        }

        private void simpleButton3_DragDrop(object sender, DragEventArgs e)
        {
            simpleButton3.Text = e.Data.GetData(DataFormats.StringFormat).ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Save/btn1");
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Edit/btn2");
           
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Delete/btn3");
            
        }

    

        private void btn_Delete_MouseDown(object sender, MouseEventArgs e)
        {
            // btn_Delete.DoDragDrop(btn_Delete.Text, DragDropEffects.All);
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);

        }

        private void simpleButton2_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = simpleButton2.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                simpleButton2.Location = newPoint;
            }
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

        private void btn_Edit_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
            DoDragDrop(sender, DragDropEffects.All);
           // DoDragDrop((sender as Label).Text, DragDropEffects.Link);
        }

        private void btn_Edit_DragOver(object sender, DragEventArgs e)
        {
            FlowLayoutPanel p = (FlowLayoutPanel)(sender as SimpleButton).Parent;
            //Current Position             
            int myIndex = p.Controls.GetChildIndex((sender as SimpleButton));

            //Dragged to control to location of next picturebox
            SimpleButton q = (SimpleButton)e.Data.GetData(typeof(SimpleButton));
            p.Controls.SetChildIndex(q, myIndex);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Save");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Delete");
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            XtraMessageBox.Show("Edit");
        }

        private void simpleButton3_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragged)
            {
                Point newPoint = simpleButton3.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                simpleButton3.Location = newPoint;
            }
        }
    }
}