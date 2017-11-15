namespace BizWTF.Testing.VSDevTools.Messaging.Steps
{
    partial class frmMPSeachParam
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtMgmtDBName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDTATest = new System.Windows.Forms.Button();
            this.txtDTADBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDTAServerName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMaxRecord = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.dgProps = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dgMessages = new System.Windows.Forms.DataGridView();
            this.PropValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PropDelete = new System.Windows.Forms.DataGridViewImageColumn();
            this.EventType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SchemaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstanceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProps)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.txtMgmtDBName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnDTATest);
            this.groupBox1.Controls.Add(this.txtDTADBName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDTAServerName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1132, 107);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DTA Db connection properties";
            // 
            // txtMgmtDBName
            // 
            this.txtMgmtDBName.Location = new System.Drawing.Point(817, 44);
            this.txtMgmtDBName.Name = "txtMgmtDBName";
            this.txtMgmtDBName.Size = new System.Drawing.Size(130, 31);
            this.txtMgmtDBName.TabIndex = 7;
            this.txtMgmtDBName.Text = "BizTalkMgmtDb";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(652, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 25);
            this.label6.TabIndex = 6;
            this.label6.Text = "Mgmt DB name";
            // 
            // btnDTATest
            // 
            this.btnDTATest.Location = new System.Drawing.Point(980, 44);
            this.btnDTATest.Name = "btnDTATest";
            this.btnDTATest.Size = new System.Drawing.Size(137, 33);
            this.btnDTATest.TabIndex = 5;
            this.btnDTATest.Text = "Test connection";
            this.btnDTATest.UseVisualStyleBackColor = true;
            this.btnDTATest.Click += new System.EventHandler(this.btnDTATest_Click);
            // 
            // txtDTADBName
            // 
            this.txtDTADBName.Location = new System.Drawing.Point(491, 44);
            this.txtDTADBName.Name = "txtDTADBName";
            this.txtDTADBName.Size = new System.Drawing.Size(130, 31);
            this.txtDTADBName.TabIndex = 4;
            this.txtDTADBName.Text = "BizTalkDTADb";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(337, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "DTA DB name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server name";
            // 
            // txtDTAServerName
            // 
            this.txtDTAServerName.Location = new System.Drawing.Point(146, 44);
            this.txtDTAServerName.Name = "txtDTAServerName";
            this.txtDTAServerName.Size = new System.Drawing.Size(185, 31);
            this.txtDTAServerName.TabIndex = 1;
            this.txtDTAServerName.Text = ".";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMaxRecord);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dtTo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.dtFrom);
            this.groupBox2.Controls.Add(this.dgProps);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 110);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1127, 344);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Search params";
            // 
            // txtMaxRecord
            // 
            this.txtMaxRecord.Location = new System.Drawing.Point(979, 45);
            this.txtMaxRecord.Name = "txtMaxRecord";
            this.txtMaxRecord.Size = new System.Drawing.Size(97, 31);
            this.txtMaxRecord.TabIndex = 7;
            this.txtMaxRecord.Text = "100";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(842, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 25);
            this.label5.TabIndex = 6;
            this.label5.Text = "Max records";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(902, 297);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(219, 41);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Get messages";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(434, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "To";
            // 
            // dtTo
            // 
            this.dtTo.Location = new System.Drawing.Point(477, 46);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(359, 31);
            this.dtTo.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "From";
            // 
            // dtFrom
            // 
            this.dtFrom.Location = new System.Drawing.Point(73, 46);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(355, 31);
            this.dtFrom.TabIndex = 1;
            // 
            // dgProps
            // 
            this.dgProps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PropValue,
            this.PropDelete});
            this.dgProps.Location = new System.Drawing.Point(6, 83);
            this.dgProps.Name = "dgProps";
            this.dgProps.Size = new System.Drawing.Size(1111, 190);
            this.dgProps.TabIndex = 0;
            this.dgProps.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgMessages);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(12, 481);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1127, 270);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Results";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(915, 757);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(230, 43);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate mock messages";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // dgMessages
            // 
            this.dgMessages.AllowUserToAddRows = false;
            this.dgMessages.AllowUserToDeleteRows = false;
            this.dgMessages.AllowUserToOrderColumns = true;
            this.dgMessages.AllowUserToResizeRows = false;
            this.dgMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventType,
            this.ServiceName,
            this.PortName,
            this.SchemaName,
            this.Timestamp,
            this.InstanceID});
            this.dgMessages.Location = new System.Drawing.Point(7, 40);
            this.dgMessages.Name = "dgMessages";
            this.dgMessages.ReadOnly = true;
            this.dgMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgMessages.ShowEditingIcon = false;
            this.dgMessages.Size = new System.Drawing.Size(1114, 207);
            this.dgMessages.TabIndex = 0;
            // 
            // PropValue
            // 
            this.PropValue.HeaderText = "Instance name";
            this.PropValue.Name = "PropValue";
            this.PropValue.Width = 1000;
            // 
            // PropDelete
            // 
            this.PropDelete.HeaderText = "";
            this.PropDelete.Image = global::BizWTF.Testing.VSDevTools.Properties.Resources.no;
            this.PropDelete.MinimumWidth = 30;
            this.PropDelete.Name = "PropDelete";
            this.PropDelete.Width = 30;
            // 
            // EventType
            // 
            this.EventType.HeaderText = "Type";
            this.EventType.Name = "EventType";
            this.EventType.ReadOnly = true;
            // 
            // ServiceName
            // 
            this.ServiceName.HeaderText = "Service Name";
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.ReadOnly = true;
            this.ServiceName.Width = 400;
            // 
            // PortName
            // 
            this.PortName.HeaderText = "Port Name";
            this.PortName.Name = "PortName";
            this.PortName.ReadOnly = true;
            this.PortName.Width = 200;
            // 
            // SchemaName
            // 
            this.SchemaName.HeaderText = "Schema Name";
            this.SchemaName.Name = "SchemaName";
            this.SchemaName.ReadOnly = true;
            this.SchemaName.Width = 210;
            // 
            // Timestamp
            // 
            this.Timestamp.HeaderText = "Timestamp";
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.ReadOnly = true;
            this.Timestamp.Width = 140;
            // 
            // InstanceID
            // 
            this.InstanceID.HeaderText = "Instance ID";
            this.InstanceID.Name = "InstanceID";
            this.InstanceID.ReadOnly = true;
            this.InstanceID.Visible = false;
            // 
            // frmMPSeachParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 812);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMPSeachParam";
            this.Text = "Search for a message in DTA Db";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProps)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDTATest;
        private System.Windows.Forms.TextBox txtDTADBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDTAServerName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgProps;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgMessages;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtMaxRecord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMgmtDBName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn PropValue;
        private System.Windows.Forms.DataGridViewImageColumn PropDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SchemaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstanceID;
    }
}