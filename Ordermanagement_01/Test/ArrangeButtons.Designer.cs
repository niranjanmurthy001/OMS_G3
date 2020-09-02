namespace Ordermanagement_01.Test
{
    partial class ArrangeButtons
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(3, 3);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(85, 39);
            this.simpleButton1.TabIndex = 3;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleButton1_MouseDown);
            this.simpleButton1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.simpleButton1_MouseMove);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(94, 3);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 39);
            this.simpleButton2.TabIndex = 4;
            this.simpleButton2.Text = "simpleButton2";
            this.simpleButton2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleButton2_MouseDown_1);
            this.simpleButton2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.simpleButton2_MouseMove_1);
            this.simpleButton2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleButton2_MouseUp_1);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(175, 3);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(75, 39);
            this.simpleButton3.TabIndex = 5;
            this.simpleButton3.Text = "simpleButton3";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.simpleButton1);
            this.flowLayoutPanel1.Controls.Add(this.simpleButton2);
            this.flowLayoutPanel1.Controls.Add(this.simpleButton3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(261, 215);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanel1_DragDrop);
            this.flowLayoutPanel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.flowLayoutPanel1_DragEnter);
            // 
            // ArrangeButtons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 258);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "ArrangeButtons";
            this.Text = "ArrangeButtons";
            this.Load += new System.EventHandler(this.ArrangeButtons_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}