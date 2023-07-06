namespace BezierAirfoilDesigner
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            formsPlot1 = new ScottPlot.FormsPlot();
            dataGridViewTop = new DataGridView();
            topBindingSource = new BindingSource(components);
            dataGridViewBottom = new DataGridView();
            bottomBindingSource = new BindingSource(components);
            lblTop = new Label();
            lblBottom = new Label();
            btnSaveDat = new Button();
            btnIncreaseOrderTop = new Button();
            btnIncreaseOrderBottom = new Button();
            txtNumOfPointsTop = new TextBox();
            txtNumOfPointBottom = new TextBox();
            lblNumOfPointTop = new Label();
            lblNumOfPointBottom = new Label();
            lblOrderTop = new Label();
            lblOrderBottom = new Label();
            btnDecreaseOrderTop = new Button();
            btnDecreaseOrderBottom = new Button();
            txtAirfoilParam = new RichTextBox();
            lblAirfoilParam = new Label();
            btnDefault = new Button();
            btnSaveBezDat = new Button();
            chkShowControlTop = new CheckBox();
            chkShowThickness = new CheckBox();
            chkShowRadius = new CheckBox();
            lblShow = new Label();
            chkShowCamber = new CheckBox();
            btnAxisAuto = new Button();
            btnLoadBezDat = new Button();
            btnLoadDat = new Button();
            lblSave = new Label();
            lblLoad = new Label();
            chkShowReferenceTop = new CheckBox();
            chkShowReferenceBottom = new CheckBox();
            btnSearchTop = new Button();
            btnSearchBottom = new Button();
            btnAutoSearch = new Button();
            chkShowTop = new CheckBox();
            chkShowBottom = new CheckBox();
            lblSearch = new Label();
            chkShowControlBottom = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)topBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bottomBindingSource).BeginInit();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.BackColor = SystemColors.Control;
            formsPlot1.Location = new Point(14, 13);
            formsPlot1.Margin = new Padding(5, 4, 5, 4);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1062, 713);
            formsPlot1.TabIndex = 0;
            formsPlot1.AxesChanged += formsPlot1_AxesChanged;
            formsPlot1.PlottableDragged += formsPlot1_PlottableDragged;
            // 
            // dataGridViewTop
            // 
            dataGridViewTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewTop.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTop.Location = new Point(1174, 31);
            dataGridViewTop.Name = "dataGridViewTop";
            dataGridViewTop.RowHeadersWidth = 47;
            dataGridViewTop.RowTemplate.Height = 28;
            dataGridViewTop.Size = new Size(298, 169);
            dataGridViewTop.TabIndex = 1;
            dataGridViewTop.CellValueChanged += dataGridView1_CellValueChanged;
            // 
            // dataGridViewBottom
            // 
            dataGridViewBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewBottom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBottom.Location = new Point(1174, 225);
            dataGridViewBottom.Name = "dataGridViewBottom";
            dataGridViewBottom.RowHeadersWidth = 47;
            dataGridViewBottom.RowTemplate.Height = 28;
            dataGridViewBottom.Size = new Size(298, 169);
            dataGridViewBottom.TabIndex = 2;
            dataGridViewBottom.CellValueChanged += dataGridView2_CellValueChanged;
            // 
            // lblTop
            // 
            lblTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTop.AutoSize = true;
            lblTop.Location = new Point(1183, 9);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(31, 19);
            lblTop.TabIndex = 3;
            lblTop.Text = "Top";
            // 
            // lblBottom
            // 
            lblBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBottom.AutoSize = true;
            lblBottom.Location = new Point(1174, 203);
            lblBottom.Name = "lblBottom";
            lblBottom.Size = new Size(55, 19);
            lblBottom.TabIndex = 4;
            lblBottom.Text = "Bottom";
            // 
            // btnSaveDat
            // 
            btnSaveDat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveDat.Location = new Point(1084, 541);
            btnSaveDat.Name = "btnSaveDat";
            btnSaveDat.Size = new Size(84, 26);
            btnSaveDat.TabIndex = 5;
            btnSaveDat.Text = ".dat";
            btnSaveDat.UseVisualStyleBackColor = true;
            btnSaveDat.Click += btnSaveDat_Click;
            // 
            // btnIncreaseOrderTop
            // 
            btnIncreaseOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIncreaseOrderTop.Location = new Point(1478, 53);
            btnIncreaseOrderTop.Name = "btnIncreaseOrderTop";
            btnIncreaseOrderTop.Size = new Size(26, 26);
            btnIncreaseOrderTop.TabIndex = 8;
            btnIncreaseOrderTop.Text = "+";
            btnIncreaseOrderTop.UseVisualStyleBackColor = true;
            btnIncreaseOrderTop.Click += btnIncreaseOrderTop_Click;
            // 
            // btnIncreaseOrderBottom
            // 
            btnIncreaseOrderBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIncreaseOrderBottom.Location = new Point(1478, 247);
            btnIncreaseOrderBottom.Name = "btnIncreaseOrderBottom";
            btnIncreaseOrderBottom.Size = new Size(26, 26);
            btnIncreaseOrderBottom.TabIndex = 9;
            btnIncreaseOrderBottom.Text = "+";
            btnIncreaseOrderBottom.UseVisualStyleBackColor = true;
            btnIncreaseOrderBottom.Click += btnIncreaseOrderBottom_Click;
            // 
            // txtNumOfPointsTop
            // 
            txtNumOfPointsTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtNumOfPointsTop.Location = new Point(1478, 104);
            txtNumOfPointsTop.Name = "txtNumOfPointsTop";
            txtNumOfPointsTop.Size = new Size(86, 26);
            txtNumOfPointsTop.TabIndex = 10;
            txtNumOfPointsTop.Text = "250";
            // 
            // txtNumOfPointBottom
            // 
            txtNumOfPointBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtNumOfPointBottom.Location = new Point(1478, 298);
            txtNumOfPointBottom.Name = "txtNumOfPointBottom";
            txtNumOfPointBottom.Size = new Size(86, 26);
            txtNumOfPointBottom.TabIndex = 11;
            txtNumOfPointBottom.Text = "250";
            // 
            // lblNumOfPointTop
            // 
            lblNumOfPointTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblNumOfPointTop.AutoSize = true;
            lblNumOfPointTop.Location = new Point(1478, 82);
            lblNumOfPointTop.Name = "lblNumOfPointTop";
            lblNumOfPointTop.Size = new Size(75, 19);
            lblNumOfPointTop.TabIndex = 12;
            lblNumOfPointTop.Text = "# of points";
            // 
            // lblNumOfPointBottom
            // 
            lblNumOfPointBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblNumOfPointBottom.AutoSize = true;
            lblNumOfPointBottom.Location = new Point(1478, 276);
            lblNumOfPointBottom.Name = "lblNumOfPointBottom";
            lblNumOfPointBottom.Size = new Size(75, 19);
            lblNumOfPointBottom.TabIndex = 13;
            lblNumOfPointBottom.Text = "# of points";
            // 
            // lblOrderTop
            // 
            lblOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrderTop.AutoSize = true;
            lblOrderTop.Location = new Point(1478, 31);
            lblOrderTop.Name = "lblOrderTop";
            lblOrderTop.Size = new Size(45, 19);
            lblOrderTop.TabIndex = 14;
            lblOrderTop.Text = "order:";
            // 
            // lblOrderBottom
            // 
            lblOrderBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrderBottom.AutoSize = true;
            lblOrderBottom.Location = new Point(1478, 225);
            lblOrderBottom.Name = "lblOrderBottom";
            lblOrderBottom.Size = new Size(45, 19);
            lblOrderBottom.TabIndex = 15;
            lblOrderBottom.Text = "order:";
            // 
            // btnDecreaseOrderTop
            // 
            btnDecreaseOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDecreaseOrderTop.Location = new Point(1513, 53);
            btnDecreaseOrderTop.Name = "btnDecreaseOrderTop";
            btnDecreaseOrderTop.Size = new Size(26, 26);
            btnDecreaseOrderTop.TabIndex = 16;
            btnDecreaseOrderTop.Text = "-";
            btnDecreaseOrderTop.UseVisualStyleBackColor = true;
            btnDecreaseOrderTop.Click += btnDecreaseOrderTop_Click;
            // 
            // btnDecreaseOrderBottom
            // 
            btnDecreaseOrderBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDecreaseOrderBottom.BackColor = SystemColors.Control;
            btnDecreaseOrderBottom.ForeColor = SystemColors.ControlText;
            btnDecreaseOrderBottom.Location = new Point(1513, 247);
            btnDecreaseOrderBottom.Name = "btnDecreaseOrderBottom";
            btnDecreaseOrderBottom.Size = new Size(26, 26);
            btnDecreaseOrderBottom.TabIndex = 17;
            btnDecreaseOrderBottom.Text = "-";
            btnDecreaseOrderBottom.UseVisualStyleBackColor = true;
            btnDecreaseOrderBottom.Click += btnDecreaseOrderBottom_Click;
            // 
            // txtAirfoilParam
            // 
            txtAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtAirfoilParam.Location = new Point(1174, 419);
            txtAirfoilParam.Name = "txtAirfoilParam";
            txtAirfoilParam.Size = new Size(390, 268);
            txtAirfoilParam.TabIndex = 18;
            txtAirfoilParam.Text = "";
            // 
            // lblAirfoilParam
            // 
            lblAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblAirfoilParam.AutoSize = true;
            lblAirfoilParam.Location = new Point(1174, 397);
            lblAirfoilParam.Name = "lblAirfoilParam";
            lblAirfoilParam.Size = new Size(117, 19);
            lblAirfoilParam.TabIndex = 19;
            lblAirfoilParam.Text = "Airfoil Parameters";
            // 
            // btnDefault
            // 
            btnDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDefault.Location = new Point(1084, 311);
            btnDefault.Name = "btnDefault";
            btnDefault.Size = new Size(84, 26);
            btnDefault.TabIndex = 20;
            btnDefault.Text = "default";
            btnDefault.UseVisualStyleBackColor = true;
            btnDefault.Click += btnDefault_Click;
            // 
            // btnSaveBezDat
            // 
            btnSaveBezDat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSaveBezDat.Location = new Point(1084, 573);
            btnSaveBezDat.Name = "btnSaveBezDat";
            btnSaveBezDat.Size = new Size(84, 26);
            btnSaveBezDat.TabIndex = 21;
            btnSaveBezDat.Text = ".bez.dat";
            btnSaveBezDat.UseVisualStyleBackColor = true;
            btnSaveBezDat.Click += btnSaveBezDat_Click;
            // 
            // chkShowControlTop
            // 
            chkShowControlTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowControlTop.AutoSize = true;
            chkShowControlTop.Location = new Point(1084, 31);
            chkShowControlTop.Name = "chkShowControlTop";
            chkShowControlTop.Size = new Size(72, 23);
            chkShowControlTop.TabIndex = 25;
            chkShowControlTop.Text = "ctrl top";
            chkShowControlTop.UseVisualStyleBackColor = true;
            chkShowControlTop.CheckedChanged += chkShowControlTop_CheckedChanged;
            // 
            // chkShowThickness
            // 
            chkShowThickness.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowThickness.AutoSize = true;
            chkShowThickness.Location = new Point(1084, 147);
            chkShowThickness.Name = "chkShowThickness";
            chkShowThickness.Size = new Size(84, 23);
            chkShowThickness.TabIndex = 26;
            chkShowThickness.Text = "thickness";
            chkShowThickness.UseVisualStyleBackColor = true;
            chkShowThickness.CheckedChanged += chkShowThickness_CheckedChanged;
            // 
            // chkShowRadius
            // 
            chkShowRadius.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowRadius.AutoSize = true;
            chkShowRadius.Location = new Point(1084, 205);
            chkShowRadius.Name = "chkShowRadius";
            chkShowRadius.Size = new Size(65, 23);
            chkShowRadius.TabIndex = 27;
            chkShowRadius.Text = "radius";
            chkShowRadius.UseVisualStyleBackColor = true;
            chkShowRadius.CheckedChanged += chkShowRadius_CheckedChanged;
            // 
            // lblShow
            // 
            lblShow.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblShow.AutoSize = true;
            lblShow.Location = new Point(1084, 9);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(44, 19);
            lblShow.TabIndex = 28;
            lblShow.Text = "show:";
            // 
            // chkShowCamber
            // 
            chkShowCamber.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowCamber.AutoSize = true;
            chkShowCamber.Location = new Point(1084, 176);
            chkShowCamber.Name = "chkShowCamber";
            chkShowCamber.Size = new Size(73, 23);
            chkShowCamber.TabIndex = 29;
            chkShowCamber.Text = "camber";
            chkShowCamber.UseVisualStyleBackColor = true;
            chkShowCamber.CheckedChanged += chkShowCamber_CheckedChanged;
            // 
            // btnAxisAuto
            // 
            btnAxisAuto.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAxisAuto.Location = new Point(1084, 693);
            btnAxisAuto.Name = "btnAxisAuto";
            btnAxisAuto.Size = new Size(84, 26);
            btnAxisAuto.TabIndex = 30;
            btnAxisAuto.Text = "Zoom";
            btnAxisAuto.UseVisualStyleBackColor = true;
            btnAxisAuto.Click += btnAxisAuto_Click;
            // 
            // btnLoadBezDat
            // 
            btnLoadBezDat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadBezDat.Location = new Point(1084, 375);
            btnLoadBezDat.Name = "btnLoadBezDat";
            btnLoadBezDat.Size = new Size(84, 26);
            btnLoadBezDat.TabIndex = 32;
            btnLoadBezDat.Text = ".bez.dat";
            btnLoadBezDat.UseVisualStyleBackColor = true;
            btnLoadBezDat.Click += btnLoadBezDat_Click;
            // 
            // btnLoadDat
            // 
            btnLoadDat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLoadDat.Location = new Point(1084, 343);
            btnLoadDat.Name = "btnLoadDat";
            btnLoadDat.Size = new Size(84, 26);
            btnLoadDat.TabIndex = 31;
            btnLoadDat.Text = ".dat";
            btnLoadDat.UseVisualStyleBackColor = true;
            btnLoadDat.Click += btnLoadDat_Click;
            btnLoadDat.MouseDown += btnLoadDat_MouseDown;
            // 
            // lblSave
            // 
            lblSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSave.AutoSize = true;
            lblSave.Location = new Point(1084, 519);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(39, 19);
            lblSave.TabIndex = 33;
            lblSave.Text = "save:";
            // 
            // lblLoad
            // 
            lblLoad.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLoad.AutoSize = true;
            lblLoad.Location = new Point(1084, 289);
            lblLoad.Name = "lblLoad";
            lblLoad.Size = new Size(38, 19);
            lblLoad.TabIndex = 34;
            lblLoad.Text = "load:";
            // 
            // chkShowReferenceTop
            // 
            chkShowReferenceTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowReferenceTop.AutoSize = true;
            chkShowReferenceTop.Location = new Point(1084, 234);
            chkShowReferenceTop.Name = "chkShowReferenceTop";
            chkShowReferenceTop.Size = new Size(69, 23);
            chkShowReferenceTop.TabIndex = 35;
            chkShowReferenceTop.Text = "ref top";
            chkShowReferenceTop.UseVisualStyleBackColor = true;
            chkShowReferenceTop.CheckedChanged += chkShowReferenceTop_CheckedChanged;
            // 
            // chkShowReferenceBottom
            // 
            chkShowReferenceBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowReferenceBottom.AutoSize = true;
            chkShowReferenceBottom.Location = new Point(1084, 263);
            chkShowReferenceBottom.Name = "chkShowReferenceBottom";
            chkShowReferenceBottom.Size = new Size(69, 23);
            chkShowReferenceBottom.TabIndex = 36;
            chkShowReferenceBottom.Text = "ref bot";
            chkShowReferenceBottom.UseVisualStyleBackColor = true;
            chkShowReferenceBottom.CheckedChanged += chkShowReferenceBottom_CheckedChanged;
            // 
            // btnSearchTop
            // 
            btnSearchTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchTop.Location = new Point(1084, 426);
            btnSearchTop.Name = "btnSearchTop";
            btnSearchTop.Size = new Size(84, 26);
            btnSearchTop.TabIndex = 37;
            btnSearchTop.Text = "top";
            btnSearchTop.UseVisualStyleBackColor = true;
            btnSearchTop.Click += btnSearchTop_Click;
            // 
            // btnSearchBottom
            // 
            btnSearchBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchBottom.Location = new Point(1084, 458);
            btnSearchBottom.Name = "btnSearchBottom";
            btnSearchBottom.Size = new Size(84, 26);
            btnSearchBottom.TabIndex = 38;
            btnSearchBottom.Text = "bottom";
            btnSearchBottom.UseVisualStyleBackColor = true;
            btnSearchBottom.Click += btnSearchBottom_Click;
            // 
            // btnAutoSearch
            // 
            btnAutoSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAutoSearch.Location = new Point(1084, 490);
            btnAutoSearch.Name = "btnAutoSearch";
            btnAutoSearch.Size = new Size(84, 26);
            btnAutoSearch.TabIndex = 39;
            btnAutoSearch.Text = "auto";
            btnAutoSearch.UseVisualStyleBackColor = true;
            btnAutoSearch.Click += btnAutoSearch_Click;
            // 
            // chkShowTop
            // 
            chkShowTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowTop.AutoSize = true;
            chkShowTop.Location = new Point(1084, 89);
            chkShowTop.Name = "chkShowTop";
            chkShowTop.Size = new Size(49, 23);
            chkShowTop.TabIndex = 40;
            chkShowTop.Text = "top";
            chkShowTop.UseVisualStyleBackColor = true;
            chkShowTop.CheckedChanged += chkShowTop_CheckedChanged;
            // 
            // chkShowBottom
            // 
            chkShowBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowBottom.AutoSize = true;
            chkShowBottom.Location = new Point(1084, 118);
            chkShowBottom.Name = "chkShowBottom";
            chkShowBottom.Size = new Size(74, 23);
            chkShowBottom.TabIndex = 41;
            chkShowBottom.Text = "bottom";
            chkShowBottom.UseVisualStyleBackColor = true;
            chkShowBottom.CheckedChanged += chkShowBottom_CheckedChanged;
            // 
            // lblSearch
            // 
            lblSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(1084, 404);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(48, 19);
            lblSearch.TabIndex = 42;
            lblSearch.Text = "search";
            // 
            // chkShowControlBottom
            // 
            chkShowControlBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            chkShowControlBottom.AutoSize = true;
            chkShowControlBottom.Location = new Point(1084, 60);
            chkShowControlBottom.Name = "chkShowControlBottom";
            chkShowControlBottom.Size = new Size(72, 23);
            chkShowControlBottom.TabIndex = 43;
            chkShowControlBottom.Text = "ctrl bot";
            chkShowControlBottom.UseVisualStyleBackColor = true;
            chkShowControlBottom.CheckedChanged += chkShowControlBottom_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1570, 739);
            Controls.Add(chkShowControlBottom);
            Controls.Add(lblSearch);
            Controls.Add(chkShowBottom);
            Controls.Add(chkShowTop);
            Controls.Add(btnAutoSearch);
            Controls.Add(btnSearchBottom);
            Controls.Add(btnSearchTop);
            Controls.Add(chkShowReferenceBottom);
            Controls.Add(chkShowReferenceTop);
            Controls.Add(lblLoad);
            Controls.Add(lblSave);
            Controls.Add(btnLoadBezDat);
            Controls.Add(btnLoadDat);
            Controls.Add(btnAxisAuto);
            Controls.Add(chkShowCamber);
            Controls.Add(lblShow);
            Controls.Add(chkShowRadius);
            Controls.Add(chkShowThickness);
            Controls.Add(chkShowControlTop);
            Controls.Add(btnSaveBezDat);
            Controls.Add(btnDefault);
            Controls.Add(lblAirfoilParam);
            Controls.Add(txtAirfoilParam);
            Controls.Add(btnDecreaseOrderBottom);
            Controls.Add(btnDecreaseOrderTop);
            Controls.Add(lblOrderBottom);
            Controls.Add(lblOrderTop);
            Controls.Add(lblNumOfPointBottom);
            Controls.Add(lblNumOfPointTop);
            Controls.Add(txtNumOfPointBottom);
            Controls.Add(txtNumOfPointsTop);
            Controls.Add(btnIncreaseOrderBottom);
            Controls.Add(btnIncreaseOrderTop);
            Controls.Add(btnSaveDat);
            Controls.Add(lblBottom);
            Controls.Add(lblTop);
            Controls.Add(dataGridViewBottom);
            Controls.Add(dataGridViewTop);
            Controls.Add(formsPlot1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "BezierAirfoilDesigner";
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).EndInit();
            ((System.ComponentModel.ISupportInitialize)topBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize)bottomBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private DataGridView dataGridViewTop;
        private BindingSource topBindingSource;
        private DataGridView dataGridViewBottom;
        private BindingSource bottomBindingSource;
        private Label lblTop;
        private Label lblBottom;
        private Button btnSaveDat;
        private Button btnIncreaseOrderTop;
        private Button btnIncreaseOrderBottom;
        private TextBox txtNumOfPointsTop;
        private TextBox txtNumOfPointBottom;
        private Label lblNumOfPointTop;
        private Label lblNumOfPointBottom;
        private Label lblOrderTop;
        private Label lblOrderBottom;
        private Button btnDecreaseOrderTop;
        private Button btnDecreaseOrderBottom;
        private RichTextBox txtAirfoilParam;
        private Label lblAirfoilParam;
        private Button btnDefault;
        private Button btnSaveBezDat;
        private CheckBox chkShowControlTop;
        private CheckBox chkShowThickness;
        private CheckBox chkShowRadius;
        private Label lblShow;
        private CheckBox chkShowCamber;
        private Button btnAxisAuto;
        private Button btnLoadBezDat;
        private Button btnLoadDat;
        private Label lblSave;
        private Label lblLoad;
        private CheckBox chkShowReferenceTop;
        private CheckBox chkShowReferenceBottom;
        private Button btnSearchTop;
        private Button btnSearchBottom;
        private Button btnAutoSearch;
        private CheckBox chkShowTop;
        private CheckBox chkShowBottom;
        private Label lblSearch;
        private CheckBox chkShowControlBottom;
    }
}