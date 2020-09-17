namespace Ordermanagement_01.Opp
{
    partial class ButtonReorder
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Delete = new DevExpress.XtraEditors.SimpleButton();
            this.btn__Edit = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.37716F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.62284F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 346);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AllowDrop = true;
            this.flowLayoutPanel1.Controls.Add(this.btn_Delete);
            this.flowLayoutPanel1.Controls.Add(this.btn__Edit);
            this.flowLayoutPanel1.Controls.Add(this.btn_Save);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(448, 50);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // btn_Delete
            // 
            this.btn_Delete.AllowDrop = true;
            this.btn_Delete.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Delete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btn_Delete.Appearance.Options.UseFont = true;
            this.btn_Delete.Appearance.Options.UseForeColor = true;
            this.btn_Delete.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.btn_Delete.Location = new System.Drawing.Point(361, 3);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(84, 42);
            this.btn_Delete.TabIndex = 10;
            this.btn_Delete.Text = "Delete";
            this.btn_Delete.ToolTip = "Delete";
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            this.btn_Delete.DragOver += new System.Windows.Forms.DragEventHandler(this.btn_Delete_DragOver);
            this.btn_Delete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Delete_MouseDown);
            // 
            // btn__Edit
            // 
            this.btn__Edit.AllowDrop = true;
            this.btn__Edit.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn__Edit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btn__Edit.Appearance.Options.UseFont = true;
            this.btn__Edit.Appearance.Options.UseForeColor = true;
            this.btn__Edit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.btn__Edit.Location = new System.Drawing.Point(271, 3);
            this.btn__Edit.Name = "btn__Edit";
            this.btn__Edit.Size = new System.Drawing.Size(84, 42);
            this.btn__Edit.TabIndex = 0;
            this.btn__Edit.Text = "Edit";
            this.btn__Edit.ToolTip = "Edit";
            this.btn__Edit.Click += new System.EventHandler(this.btn__Edit_Click);
            this.btn__Edit.DragOver += new System.Windows.Forms.DragEventHandler(this.btn__Edit_DragOver);
            this.btn__Edit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn__Edit_MouseDown);
            // 
            // btn_Save
            // 
            this.btn_Save.AllowDrop = true;
            this.btn_Save.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Save.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btn_Save.Appearance.Options.UseFont = true;
            this.btn_Save.Appearance.Options.UseForeColor = true;
            this.btn_Save.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.btn_Save.Location = new System.Drawing.Point(181, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(84, 42);
            this.btn_Save.TabIndex = 9;
            this.btn_Save.Text = "Save";
            this.btn_Save.ToolTip = "Save";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            this.btn_Save.DragOver += new System.Windows.Forms.DragEventHandler(this.btn_Save_DragOver);
            this.btn_Save.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_Save_MouseDown);
            // 
            // panelControl1
            // 
            this.panelControl1.AllowDrop = true;
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 59);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(448, 227);
            this.panelControl1.TabIndex = 10;
            // 
            // simpleButton1
            // 
            this.simpleButton1.AllowDrop = true;
            this.simpleButton1.Location = new System.Drawing.Point(64, 23);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 23);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            this.simpleButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleButton1_MouseDown);
            this.simpleButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleButton1_MouseUp);
            // 
            // simpleButton2
            // 
            this.simpleButton2.AllowDrop = true;
            this.simpleButton2.Location = new System.Drawing.Point(64, 74);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "2";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            this.simpleButton2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleButton2_MouseDown);
            this.simpleButton2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleButton2_MouseUp);
            // 
            // simpleButton3
            // 
            this.simpleButton3.AllowDrop = true;
            this.simpleButton3.Location = new System.Drawing.Point(64, 134);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(75, 23);
            this.simpleButton3.TabIndex = 2;
            this.simpleButton3.Text = "3";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            this.simpleButton3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.simpleButton3_MouseDown);
            this.simpleButton3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.simpleButton3_MouseUp);
            // 
            // ButtonReorder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 346);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ButtonReorder";
            this.Text = "ButtonReorder";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btn_Delete;
        private DevExpress.XtraEditors.SimpleButton btn__Edit;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}