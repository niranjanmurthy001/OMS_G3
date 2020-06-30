namespace Ordermanagement_01.Opp.Opp_Master
{
    partial class Project_Wise_Notification_View
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
            this.gridNotification = new DevExpress.XtraGrid.GridControl();
            this.layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.MessageCol = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_messageColumn = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.UpdateonCol = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn2 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewColumn3 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_layoutViewColumn3 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNotification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_messageColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.CaptionImageOptions.Image = global::Ordermanagement_01.Properties.Resources.notification_Alert;
            this.groupControl1.Controls.Add(this.tableLayoutPanel1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(683, 392);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Notifications";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gridNotification, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.00813F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.99187F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(679, 357);
            this.tableLayoutPanel1.TabIndex = 1;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // gridNotification
            // 
            this.gridNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNotification.Location = new System.Drawing.Point(3, 49);
            this.gridNotification.MainView = this.layoutView1;
            this.gridNotification.Name = "gridNotification";
            this.gridNotification.Size = new System.Drawing.Size(673, 305);
            this.gridNotification.TabIndex = 0;
            this.gridNotification.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.layoutView1});
            // 
            // layoutView1
            // 
            this.layoutView1.CardHorzInterval = 0;
            this.layoutView1.CardMinSize = new System.Drawing.Size(96, 48);
            this.layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.MessageCol,
            this.UpdateonCol,
            this.layoutViewColumn3});
            this.layoutView1.GridControl = this.gridNotification;
            this.layoutView1.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_layoutViewColumn3});
            this.layoutView1.Name = "layoutView1";
            this.layoutView1.OptionsItemText.AlignMode = DevExpress.XtraGrid.Views.Layout.FieldTextAlignMode.AlignGlobal;
            this.layoutView1.OptionsItemText.TextToControlDistance = 0;
            this.layoutView1.OptionsView.CardArrangeRule = DevExpress.XtraGrid.Views.Layout.LayoutCardArrangeRule.AllowPartialCards;
            this.layoutView1.OptionsView.ShowCardCaption = false;
            this.layoutView1.OptionsView.ViewMode = DevExpress.XtraGrid.Views.Layout.LayoutViewMode.Column;
            this.layoutView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.UpdateonCol, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.layoutView1.TemplateCard = this.layoutViewCard1;
            // 
            // MessageCol
            // 
            this.MessageCol.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.MessageCol.AppearanceCell.Options.UseFont = true;
            this.MessageCol.AppearanceCell.Options.UseForeColor = true;
            this.MessageCol.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.MessageCol.AppearanceHeader.Options.UseFont = true;
            this.MessageCol.AppearanceHeader.Options.UseForeColor = true;
            this.MessageCol.CustomizationCaption = "Message";
            this.MessageCol.FieldName = "Message";
            this.MessageCol.LayoutViewField = this.layoutViewField_messageColumn;
            this.MessageCol.Name = "MessageCol";
            // 
            // layoutViewField_messageColumn
            // 
            this.layoutViewField_messageColumn.EditorPreferredWidth = 612;
            this.layoutViewField_messageColumn.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_messageColumn.Name = "layoutViewField_messageColumn";
            this.layoutViewField_messageColumn.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutViewField_messageColumn.Size = new System.Drawing.Size(616, 24);
            this.layoutViewField_messageColumn.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutViewField_messageColumn.TextSize = new System.Drawing.Size(0, 0);
            this.layoutViewField_messageColumn.TextVisible = false;
            // 
            // UpdateonCol
            // 
            this.UpdateonCol.AppearanceCell.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.UpdateonCol.AppearanceCell.Options.UseFont = true;
            this.UpdateonCol.AppearanceCell.Options.UseForeColor = true;
            this.UpdateonCol.AppearanceCell.Options.UseTextOptions = true;
            this.UpdateonCol.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.UpdateonCol.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.UpdateonCol.AppearanceHeader.Options.UseBackColor = true;
            this.UpdateonCol.AppearanceHeader.Options.UseFont = true;
            this.UpdateonCol.AppearanceHeader.Options.UseForeColor = true;
            this.UpdateonCol.AppearanceHeader.Options.UseTextOptions = true;
            this.UpdateonCol.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.UpdateonCol.Caption = "                                                                                 " +
    "                                             Update On";
            this.UpdateonCol.CustomizationCaption = " Updated On ";
            this.UpdateonCol.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.UpdateonCol.FieldName = "Inserted_Date";
            this.UpdateonCol.LayoutViewField = this.layoutViewField_layoutViewColumn2;
            this.UpdateonCol.Name = "UpdateonCol";
            this.UpdateonCol.OptionsColumn.AllowEdit = false;
            this.UpdateonCol.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            // 
            // layoutViewField_layoutViewColumn2
            // 
            this.layoutViewField_layoutViewColumn2.AppearanceItemCaption.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutViewField_layoutViewColumn2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutViewField_layoutViewColumn2.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutViewField_layoutViewColumn2.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutViewField_layoutViewColumn2.EditorPreferredWidth = 175;
            this.layoutViewField_layoutViewColumn2.Location = new System.Drawing.Point(0, 24);
            this.layoutViewField_layoutViewColumn2.Name = "layoutViewField_layoutViewColumn2";
            this.layoutViewField_layoutViewColumn2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutViewField_layoutViewColumn2.Size = new System.Drawing.Size(616, 24);
            this.layoutViewField_layoutViewColumn2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutViewField_layoutViewColumn2.TextSize = new System.Drawing.Size(437, 13);
            // 
            // layoutViewColumn3
            // 
            this.layoutViewColumn3.Caption = "layoutViewColumn3";
            this.layoutViewColumn3.LayoutViewField = this.layoutViewField_layoutViewColumn3;
            this.layoutViewColumn3.Name = "layoutViewColumn3";
            // 
            // layoutViewField_layoutViewColumn3
            // 
            this.layoutViewField_layoutViewColumn3.EditorPreferredWidth = 10;
            this.layoutViewField_layoutViewColumn3.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_layoutViewColumn3.Name = "layoutViewField_layoutViewColumn3";
            this.layoutViewField_layoutViewColumn3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutViewField_layoutViewColumn3.Size = new System.Drawing.Size(646, 69);
            this.layoutViewField_layoutViewColumn3.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
            this.layoutViewField_layoutViewColumn3.TextSize = new System.Drawing.Size(97, 20);
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.CustomizationFormText = "TemplateCard";
            this.layoutViewCard1.GroupBordersVisible = false;
            this.layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_messageColumn,
            this.layoutViewField_layoutViewColumn2});
            this.layoutViewCard1.Name = "layoutViewTemplateCard";
            this.layoutViewCard1.OptionsItemText.TextToControlDistance = 0;
            this.layoutViewCard1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutViewCard1.Text = "TemplateCard";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnExport);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(594, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(82, 40);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Appearance.Options.UseFont = true;
            this.btnExport.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.btnExport.Location = new System.Drawing.Point(3, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 32);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Export";
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "Office 2013";
            // 
            // Project_Wise_Notification_View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 392);
            this.Controls.Add(this.groupControl1);
            this.MinimumSize = new System.Drawing.Size(699, 430);
            this.Name = "Project_Wise_Notification_View";
            this.Text = "Project_Wise_Notification";
            this.Load += new System.EventHandler(this.Notification_View_Based_on_the_Project_Type_Wise_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNotification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_messageColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_layoutViewColumn3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gridNotification;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn MessageCol;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn UpdateonCol;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn layoutViewColumn3;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_messageColumn;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn2;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_layoutViewColumn3;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
    }
}