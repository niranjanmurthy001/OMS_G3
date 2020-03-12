namespace Ordermanagement_01.New_Dashboard.Orders
{
    partial class ImportOrders
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
            this.groupControlImport = new DevExpress.XtraEditors.GroupControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControlProjectType = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditSubClient = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lookUpEditClient = new DevExpress.XtraEditors.LookUpEdit();
            this.buttonExportTemplate = new DevExpress.XtraEditors.SimpleButton();
            this.gridControlOrders = new DevExpress.XtraGrid.GridControl();
            this.gridViewOrders = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnErrors = new DevExpress.XtraEditors.SimpleButton();
            this.btnImport = new DevExpress.XtraEditors.SimpleButton();
            //this.xtraFileDialog = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlImport)).BeginInit();
            this.groupControlImport.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSubClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditClient.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControlImport
            // 
            this.groupControlImport.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControlImport.AppearanceCaption.Options.UseFont = true;
            this.groupControlImport.Controls.Add(this.tableLayoutPanel1);
            this.groupControlImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlImport.Location = new System.Drawing.Point(0, 0);
            this.groupControlImport.Name = "groupControlImport";
            this.groupControlImport.Size = new System.Drawing.Size(984, 482);
            this.groupControlImport.TabIndex = 0;
            this.groupControlImport.Text = "Import Orders";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridControlOrders, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelControl2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 21);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(980, 459);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.labelControlProjectType);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.lookUpEditSubClient);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.lookUpEditClient);
            this.panelControl1.Controls.Add(this.buttonExportTemplate);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(974, 36);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControlProjectType
            // 
            this.labelControlProjectType.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelControlProjectType.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControlProjectType.Appearance.Options.UseFont = true;
            this.labelControlProjectType.Location = new System.Drawing.Point(587, 9);
            this.labelControlProjectType.Name = "labelControlProjectType";
            this.labelControlProjectType.Size = new System.Drawing.Size(77, 17);
            this.labelControlProjectType.TabIndex = 27;
            this.labelControlProjectType.Text = "Project Type";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(246, 10);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(66, 15);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "SUB CLIENT";
            // 
            // lookUpEditSubClient
            // 
            this.lookUpEditSubClient.EditValue = "0";
            this.lookUpEditSubClient.Location = new System.Drawing.Point(318, 5);
            this.lookUpEditSubClient.MinimumSize = new System.Drawing.Size(170, 25);
            this.lookUpEditSubClient.Name = "lookUpEditSubClient";
            this.lookUpEditSubClient.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditSubClient.Properties.Appearance.Options.UseFont = true;
            this.lookUpEditSubClient.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditSubClient.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lookUpEditSubClient.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditSubClient.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lookUpEditSubClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditSubClient.Properties.NullText = "SELECT";
            this.lookUpEditSubClient.Size = new System.Drawing.Size(188, 22);
            this.lookUpEditSubClient.TabIndex = 25;
            this.lookUpEditSubClient.EditValueChanged += new System.EventHandler(this.lookUpEditSubClient_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(7, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 15);
            this.labelControl1.TabIndex = 24;
            this.labelControl1.Text = "CLIENT";
            // 
            // lookUpEditClient
            // 
            this.lookUpEditClient.EditValue = "0";
            this.lookUpEditClient.Location = new System.Drawing.Point(52, 5);
            this.lookUpEditClient.MinimumSize = new System.Drawing.Size(170, 25);
            this.lookUpEditClient.Name = "lookUpEditClient";
            this.lookUpEditClient.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditClient.Properties.Appearance.Options.UseFont = true;
            this.lookUpEditClient.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditClient.Properties.AppearanceDropDown.Options.UseFont = true;
            this.lookUpEditClient.Properties.AppearanceDropDownHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUpEditClient.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            this.lookUpEditClient.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditClient.Properties.NullText = "SELECT";
            this.lookUpEditClient.Size = new System.Drawing.Size(188, 22);
            this.lookUpEditClient.TabIndex = 23;
            this.lookUpEditClient.EditValueChanged += new System.EventHandler(this.lookUpEditClient_EditValueChanged);
            // 
            // buttonExportTemplate
            // 
            this.buttonExportTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExportTemplate.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonExportTemplate.Appearance.Options.UseFont = true;
            this.buttonExportTemplate.Location = new System.Drawing.Point(889, 4);
            this.buttonExportTemplate.MinimumSize = new System.Drawing.Size(0, 25);
            this.buttonExportTemplate.Name = "buttonExportTemplate";
            this.buttonExportTemplate.Size = new System.Drawing.Size(80, 25);
            this.buttonExportTemplate.TabIndex = 22;
            this.buttonExportTemplate.Text = "Template";
            this.buttonExportTemplate.Click += new System.EventHandler(this.buttonExportTemplate_Click);
            // 
            // gridControlOrders
            // 
            this.gridControlOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlOrders.Location = new System.Drawing.Point(3, 45);
            this.gridControlOrders.MainView = this.gridViewOrders;
            this.gridControlOrders.Name = "gridControlOrders";
            this.gridControlOrders.Size = new System.Drawing.Size(974, 369);
            this.gridControlOrders.TabIndex = 1;
            this.gridControlOrders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewOrders});
            // 
            // gridViewOrders
            // 
            this.gridViewOrders.GridControl = this.gridControlOrders;
            this.gridViewOrders.Name = "gridViewOrders";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnErrors);
            this.panelControl2.Controls.Add(this.btnImport);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 420);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(974, 36);
            this.panelControl2.TabIndex = 2;
            // 
            // btnErrors
            // 
            this.btnErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnErrors.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnErrors.Appearance.Options.UseFont = true;
            this.btnErrors.Location = new System.Drawing.Point(801, 5);
            this.btnErrors.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnErrors.Name = "btnErrors";
            this.btnErrors.Size = new System.Drawing.Size(80, 25);
            this.btnErrors.TabIndex = 24;
            this.btnErrors.Text = "Errors";
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Appearance.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Appearance.Options.UseFont = true;
            this.btnImport.Location = new System.Drawing.Point(887, 5);
            this.btnImport.MinimumSize = new System.Drawing.Size(0, 25);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 25);
            this.btnImport.TabIndex = 23;
            this.btnImport.Text = "Import";
            // 
            // xtraFileDialog
            // 
            //this.xtraFileDialog.FileName = null;
            //this.xtraFileDialog.InitialDirectory = "C:";
            //this.xtraFileDialog.RestoreDirectory = true;
            // 
            // ImportOrders
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 482);
            this.Controls.Add(this.groupControlImport);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.MinimumSize = new System.Drawing.Size(1000, 520);
            this.Name = "ImportOrders";
            this.Text = "Import Orders";
            this.Load += new System.EventHandler(this.ImportOrders_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControlImport)).EndInit();
            this.groupControlImport.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSubClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditClient.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControlImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton buttonExportTemplate;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditClient;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditSubClient;
        private DevExpress.XtraEditors.LabelControl labelControlProjectType;
        private DevExpress.XtraGrid.GridControl gridControlOrders;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewOrders;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnErrors;
        private DevExpress.XtraEditors.SimpleButton btnImport;
        //private DevExpress.XtraEditors.XtraOpenFileDialog xtraFileDialog;
    }
}