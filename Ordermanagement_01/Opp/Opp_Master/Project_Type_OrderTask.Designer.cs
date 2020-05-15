namespace Ordermanagement_01.Opp.Opp_Master
{
    partial class Project_Type_OrderTask
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
            this.components = new System.ComponentModel.Container();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grd_projectType = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Project_Type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Order_Status = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.ddl_Project_Type = new DevExpress.XtraEditors.LookUpEdit();
            this.checkedListBoxControl_Task = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.lbl_task = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Projecttype = new DevExpress.XtraEditors.LabelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Clear = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Save = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grd_projectType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Project_Type.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl_Task)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.tableLayoutPanel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(421, 274);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Project_Type_OrderTask";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.29256F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.70744F));
            this.tableLayoutPanel1.Controls.Add(this.grd_projectType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(417, 251);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grd_projectType
            // 
            this.grd_projectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grd_projectType.Location = new System.Drawing.Point(3, 3);
            this.grd_projectType.MainView = this.gridView1;
            this.grd_projectType.Name = "grd_projectType";
            this.grd_projectType.Size = new System.Drawing.Size(136, 245);
            this.grd_projectType.TabIndex = 0;
            this.grd_projectType.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Project_Type,
            this.Order_Status});
            this.gridView1.GridControl = this.grd_projectType;
            this.gridView1.Name = "gridView1";
            this.gridView1.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView1_RowCellClick);
            // 
            // Project_Type
            // 
            this.Project_Type.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Project_Type.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.Project_Type.AppearanceCell.Options.UseFont = true;
            this.Project_Type.AppearanceCell.Options.UseForeColor = true;
            this.Project_Type.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Project_Type.AppearanceHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.Project_Type.AppearanceHeader.Options.UseFont = true;
            this.Project_Type.AppearanceHeader.Options.UseForeColor = true;
            this.Project_Type.Caption = "Project Type";
            this.Project_Type.FieldName = "Project_Type";
            this.Project_Type.Name = "Project_Type";
            this.Project_Type.OptionsColumn.AllowEdit = false;
            this.Project_Type.Visible = true;
            this.Project_Type.VisibleIndex = 0;
            // 
            // Order_Status
            // 
            this.Order_Status.Caption = "Order Status";
            this.Order_Status.FieldName = "Order_Status";
            this.Order_Status.Name = "Order_Status";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tableLayoutPanel2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(145, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(269, 245);
            this.panelControl1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panelControl2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 81.3278F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.6722F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(265, 241);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.ddl_Project_Type);
            this.panelControl2.Controls.Add(this.checkedListBoxControl_Task);
            this.panelControl2.Controls.Add(this.lbl_task);
            this.panelControl2.Controls.Add(this.lbl_Projecttype);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(259, 189);
            this.panelControl2.TabIndex = 0;
            // 
            // ddl_Project_Type
            // 
            this.ddl_Project_Type.Location = new System.Drawing.Point(103, 31);
            this.ddl_Project_Type.Name = "ddl_Project_Type";
            this.ddl_Project_Type.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddl_Project_Type.Properties.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.ddl_Project_Type.Properties.Appearance.Options.UseFont = true;
            this.ddl_Project_Type.Properties.Appearance.Options.UseForeColor = true;
            this.ddl_Project_Type.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddl_Project_Type.Properties.NullText = "Select";
            this.ddl_Project_Type.Size = new System.Drawing.Size(149, 20);
            this.ddl_Project_Type.TabIndex = 9;
            this.ddl_Project_Type.EditValueChanged += new System.EventHandler(this.ddl_Project_Type_EditValueChanged);
            // 
            // checkedListBoxControl_Task
            // 
            this.checkedListBoxControl_Task.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBoxControl_Task.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.checkedListBoxControl_Task.Appearance.Options.UseFont = true;
            this.checkedListBoxControl_Task.Appearance.Options.UseForeColor = true;
            this.checkedListBoxControl_Task.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkedListBoxControl_Task.Location = new System.Drawing.Point(103, 77);
            this.checkedListBoxControl_Task.Name = "checkedListBoxControl_Task";
            this.checkedListBoxControl_Task.Size = new System.Drawing.Size(149, 95);
            this.checkedListBoxControl_Task.TabIndex = 8;
            // 
            // lbl_task
            // 
            this.lbl_task.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_task.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.lbl_task.Appearance.Options.UseFont = true;
            this.lbl_task.Appearance.Options.UseForeColor = true;
            this.lbl_task.Location = new System.Drawing.Point(6, 108);
            this.lbl_task.Name = "lbl_task";
            this.lbl_task.Size = new System.Drawing.Size(36, 17);
            this.lbl_task.TabIndex = 6;
            this.lbl_task.Text = "Task :";
            // 
            // lbl_Projecttype
            // 
            this.lbl_Projecttype.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Projecttype.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.lbl_Projecttype.Appearance.Options.UseFont = true;
            this.lbl_Projecttype.Appearance.Options.UseForeColor = true;
            this.lbl_Projecttype.Location = new System.Drawing.Point(4, 31);
            this.lbl_Projecttype.Name = "lbl_Projecttype";
            this.lbl_Projecttype.Size = new System.Drawing.Size(86, 17);
            this.lbl_Projecttype.TabIndex = 5;
            this.lbl_Projecttype.Text = "Project_Type :";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btn_Clear);
            this.flowLayoutPanel1.Controls.Add(this.btn_Save);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 198);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(259, 40);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btn_Clear
            // 
            this.btn_Clear.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Clear.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.btn_Clear.Appearance.Options.UseFont = true;
            this.btn_Clear.Appearance.Options.UseForeColor = true;
            this.btn_Clear.Location = new System.Drawing.Point(181, 3);
            this.btn_Clear.Name = "btn_Clear";
            this.btn_Clear.Size = new System.Drawing.Size(75, 32);
            this.btn_Clear.TabIndex = 0;
            this.btn_Clear.Text = "Clear";
            this.btn_Clear.Click += new System.EventHandler(this.btn_Clear_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.btn_Save.Appearance.Options.UseFont = true;
            this.btn_Save.Appearance.Options.UseForeColor = true;
            this.btn_Save.Location = new System.Drawing.Point(100, 3);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(75, 32);
            this.btn_Save.TabIndex = 1;
            this.btn_Save.Text = "Submit";
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2013";
            // 
            // Project_Type_OrderTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 274);
            this.Controls.Add(this.groupControl1);
            this.Name = "Project_Type_OrderTask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Project_Type_OrderTask";
            this.Load += new System.EventHandler(this.Order_Task_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grd_projectType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ddl_Project_Type.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkedListBoxControl_Task)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraGrid.GridControl grd_projectType;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControl_Task;
        private DevExpress.XtraEditors.LabelControl lbl_task;
        private DevExpress.XtraEditors.LabelControl lbl_Projecttype;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btn_Clear;
        private DevExpress.XtraEditors.SimpleButton btn_Save;
        private DevExpress.XtraGrid.Columns.GridColumn Project_Type;
        private DevExpress.XtraEditors.LookUpEdit ddl_Project_Type;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private DevExpress.XtraGrid.Columns.GridColumn Order_Status;
    }
}