namespace BizWTF.Testing.VSDevTools.Processing.Steps
{
    partial class frmProcessDebugGen
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
            this.btnGetOrch = new System.Windows.Forms.Button();
            this.txtDTADBName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDTAServerName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgOrchestration = new System.Windows.Forms.DataGridView();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Assembly = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DebugSymbols = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgDebugShapes = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MustComplete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddToScenario = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.RepeatCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShapeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOrchestration)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDebugShapes)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnGetOrch);
            this.groupBox1.Controls.Add(this.txtDTADBName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDTAServerName);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1417, 94);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DTA Db connection properties";
            // 
            // btnGetOrch
            // 
            this.btnGetOrch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetOrch.Location = new System.Drawing.Point(880, 30);
            this.btnGetOrch.Name = "btnGetOrch";
            this.btnGetOrch.Size = new System.Drawing.Size(512, 34);
            this.btnGetOrch.TabIndex = 5;
            this.btnGetOrch.Text = "Get orchestrations";
            this.btnGetOrch.UseVisualStyleBackColor = true;
            this.btnGetOrch.Click += new System.EventHandler(this.btnGetOrch_Click);
            // 
            // txtDTADBName
            // 
            this.txtDTADBName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDTADBName.Location = new System.Drawing.Point(543, 30);
            this.txtDTADBName.Name = "txtDTADBName";
            this.txtDTADBName.Size = new System.Drawing.Size(207, 31);
            this.txtDTADBName.TabIndex = 4;
            this.txtDTADBName.Text = "BizTalkDTADb";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(389, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "DTA DB name";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server name";
            // 
            // txtDTAServerName
            // 
            this.txtDTAServerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDTAServerName.Location = new System.Drawing.Point(164, 30);
            this.txtDTAServerName.Name = "txtDTAServerName";
            this.txtDTAServerName.Size = new System.Drawing.Size(207, 31);
            this.txtDTAServerName.TabIndex = 1;
            this.txtDTAServerName.Text = ".";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dgOrchestration);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(9, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1561, 239);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "1 - Select tracked orchestrations";
            // 
            // dgOrchestration
            // 
            this.dgOrchestration.AllowUserToAddRows = false;
            this.dgOrchestration.AllowUserToDeleteRows = false;
            this.dgOrchestration.AllowUserToOrderColumns = true;
            this.dgOrchestration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgOrchestration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgOrchestration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOrchestration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceName,
            this.Assembly,
            this.DebugSymbols});
            this.dgOrchestration.Location = new System.Drawing.Point(9, 25);
            this.dgOrchestration.MultiSelect = false;
            this.dgOrchestration.Name = "dgOrchestration";
            this.dgOrchestration.ReadOnly = true;
            this.dgOrchestration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgOrchestration.Size = new System.Drawing.Size(1531, 195);
            this.dgOrchestration.TabIndex = 0;
            this.dgOrchestration.SelectionChanged += new System.EventHandler(this.dgOrchestration_SelectionChanged);
            // 
            // ServiceName
            // 
            this.ServiceName.HeaderText = "Name";
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.ReadOnly = true;
            // 
            // Assembly
            // 
            this.Assembly.HeaderText = "Assembly";
            this.Assembly.Name = "Assembly";
            this.Assembly.ReadOnly = true;
            // 
            // DebugSymbols
            // 
            this.DebugSymbols.HeaderText = "Debug Symbols";
            this.DebugSymbols.Name = "DebugSymbols";
            this.DebugSymbols.ReadOnly = true;
            this.DebugSymbols.Visible = false;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(18, 836);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(1552, 33);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "GENERATE SCENARIO";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.dgDebugShapes);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(11, 332);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1559, 492);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "2 - Select relevant shapes";
            // 
            // dgDebugShapes
            // 
            this.dgDebugShapes.AllowUserToAddRows = false;
            this.dgDebugShapes.AllowUserToDeleteRows = false;
            this.dgDebugShapes.AllowUserToOrderColumns = true;
            this.dgDebugShapes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDebugShapes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgDebugShapes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDebugShapes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn1,
            this.MustComplete,
            this.AddToScenario,
            this.RepeatCount,
            this.ShapeID});
            this.dgDebugShapes.Location = new System.Drawing.Point(9, 30);
            this.dgDebugShapes.MultiSelect = false;
            this.dgDebugShapes.Name = "dgDebugShapes";
            this.dgDebugShapes.RowTemplate.Height = 25;
            this.dgDebugShapes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDebugShapes.Size = new System.Drawing.Size(1531, 456);
            this.dgDebugShapes.TabIndex = 0;
            this.dgDebugShapes.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDebugShapes_CellEndEdit);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.HeaderText = "Description";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 124;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 64;
            // 
            // MustComplete
            // 
            this.MustComplete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MustComplete.FalseValue = "false";
            this.MustComplete.HeaderText = "Must complete";
            this.MustComplete.Name = "MustComplete";
            this.MustComplete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.MustComplete.TrueValue = "true";
            this.MustComplete.Width = 140;
            // 
            // AddToScenario
            // 
            this.AddToScenario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AddToScenario.FalseValue = "false";
            this.AddToScenario.HeaderText = "Add to scenario";
            this.AddToScenario.Name = "AddToScenario";
            this.AddToScenario.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.AddToScenario.TrueValue = "true";
            this.AddToScenario.Width = 149;
            // 
            // RepeatCount
            // 
            this.RepeatCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.RepeatCount.HeaderText = "Repeat count";
            this.RepeatCount.Name = "RepeatCount";
            this.RepeatCount.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.RepeatCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.RepeatCount.Width = 129;
            // 
            // ShapeID
            // 
            this.ShapeID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ShapeID.HeaderText = "ID";
            this.ShapeID.Name = "ShapeID";
            this.ShapeID.ReadOnly = true;
            this.ShapeID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ShapeID.Visible = false;
            this.ShapeID.Width = 36;
            // 
            // frmProcessDebugGen
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1586, 881);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(1000, 800);
            this.Name = "frmProcessDebugGen";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate a test scenario";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgOrchestration)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgDebugShapes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGetOrch;
        private System.Windows.Forms.TextBox txtDTADBName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDTAServerName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgOrchestration;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgDebugShapes;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Assembly;
        private System.Windows.Forms.DataGridViewTextBoxColumn DebugSymbols;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MustComplete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AddToScenario;
        private System.Windows.Forms.DataGridViewTextBoxColumn RepeatCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShapeID;
    }
}