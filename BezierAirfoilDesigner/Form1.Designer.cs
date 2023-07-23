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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            formsPlot1 = new ScottPlot.FormsPlot();
            dataGridViewTop = new DataGridView();
            dataGridViewBottom = new DataGridView();
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
            txtCamberPosition = new TextBox();
            txtThicknessStepSize = new TextBox();
            txtCamberStepSize = new TextBox();
            lblThicknessStepSize = new Label();
            lblCamberStepSize = new Label();
            lblCamberPosition = new Label();
            lblElapsedTime = new Label();
            btnStopSearch = new Button();
            progressBar1 = new ProgressBar();
            btnLoadBez = new Button();
            btnSaveBez = new Button();
            cmbLanguage = new ComboBox();
            btnCheckForUpdates = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.BackColor = SystemColors.Control;
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Margin = new Padding(4, 3, 4, 3);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(816, 636);
            formsPlot1.TabIndex = 0;
            formsPlot1.AxesChanged += formsPlot1_AxesChanged;
            formsPlot1.PlottableDragged += formsPlot1_PlottableDragged;
            // 
            // dataGridViewTop
            // 
            dataGridViewTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewTop.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTop.Location = new Point(890, 25);
            dataGridViewTop.Margin = new Padding(3, 2, 3, 2);
            dataGridViewTop.Name = "dataGridViewTop";
            dataGridViewTop.RowHeadersWidth = 47;
            dataGridViewTop.RowTemplate.Height = 28;
            dataGridViewTop.Size = new Size(266, 190);
            dataGridViewTop.TabIndex = 1;
            dataGridViewTop.CellValueChanged += dataGridView1_CellValueChanged;
            // 
            // dataGridViewBottom
            // 
            dataGridViewBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewBottom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBottom.Location = new Point(890, 234);
            dataGridViewBottom.Margin = new Padding(3, 2, 3, 2);
            dataGridViewBottom.Name = "dataGridViewBottom";
            dataGridViewBottom.RowHeadersWidth = 47;
            dataGridViewBottom.RowTemplate.Height = 28;
            dataGridViewBottom.Size = new Size(266, 190);
            dataGridViewBottom.TabIndex = 2;
            dataGridViewBottom.CellValueChanged += dataGridView2_CellValueChanged;
            // 
            // lblTop
            // 
            lblTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTop.AutoSize = true;
            lblTop.Location = new Point(890, 8);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(26, 15);
            lblTop.TabIndex = 3;
            lblTop.Text = "Top";
            // 
            // lblBottom
            // 
            lblBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBottom.AutoSize = true;
            lblBottom.Location = new Point(890, 217);
            lblBottom.Name = "lblBottom";
            lblBottom.Size = new Size(47, 15);
            lblBottom.TabIndex = 4;
            lblBottom.Text = "Bottom";
            // 
            // btnSaveDat
            // 
            btnSaveDat.AutoSize = true;
            btnSaveDat.Location = new Point(0, 283);
            btnSaveDat.Margin = new Padding(3, 2, 3, 2);
            btnSaveDat.Name = "btnSaveDat";
            btnSaveDat.Size = new Size(75, 25);
            btnSaveDat.TabIndex = 5;
            btnSaveDat.Text = ".dat";
            btnSaveDat.UseVisualStyleBackColor = true;
            btnSaveDat.Click += btnSaveDat_Click;
            // 
            // btnIncreaseOrderTop
            // 
            btnIncreaseOrderTop.AutoSize = true;
            btnIncreaseOrderTop.Location = new Point(0, 17);
            btnIncreaseOrderTop.Margin = new Padding(3, 2, 3, 2);
            btnIncreaseOrderTop.Name = "btnIncreaseOrderTop";
            btnIncreaseOrderTop.Size = new Size(25, 25);
            btnIncreaseOrderTop.TabIndex = 8;
            btnIncreaseOrderTop.Text = "+";
            btnIncreaseOrderTop.UseVisualStyleBackColor = true;
            btnIncreaseOrderTop.Click += btnIncreaseOrderTop_Click;
            // 
            // btnIncreaseOrderBottom
            // 
            btnIncreaseOrderBottom.AutoSize = true;
            btnIncreaseOrderBottom.Location = new Point(0, 17);
            btnIncreaseOrderBottom.Margin = new Padding(3, 2, 3, 2);
            btnIncreaseOrderBottom.Name = "btnIncreaseOrderBottom";
            btnIncreaseOrderBottom.Size = new Size(25, 25);
            btnIncreaseOrderBottom.TabIndex = 9;
            btnIncreaseOrderBottom.Text = "+";
            btnIncreaseOrderBottom.UseVisualStyleBackColor = true;
            btnIncreaseOrderBottom.Click += btnIncreaseOrderBottom_Click;
            // 
            // txtNumOfPointsTop
            // 
            txtNumOfPointsTop.Location = new Point(0, 60);
            txtNumOfPointsTop.Margin = new Padding(3, 2, 3, 2);
            txtNumOfPointsTop.Name = "txtNumOfPointsTop";
            txtNumOfPointsTop.Size = new Size(76, 23);
            txtNumOfPointsTop.TabIndex = 10;
            txtNumOfPointsTop.Text = "225";
            txtNumOfPointsTop.TextChanged += txtNumOfPointsTop_TextChanged;
            // 
            // txtNumOfPointBottom
            // 
            txtNumOfPointBottom.Location = new Point(0, 60);
            txtNumOfPointBottom.Margin = new Padding(3, 2, 3, 2);
            txtNumOfPointBottom.Name = "txtNumOfPointBottom";
            txtNumOfPointBottom.Size = new Size(76, 23);
            txtNumOfPointBottom.TabIndex = 11;
            txtNumOfPointBottom.Text = "225";
            txtNumOfPointBottom.TextChanged += txtNumOfPointsBottom_TextChanged;
            // 
            // lblNumOfPointTop
            // 
            lblNumOfPointTop.AutoSize = true;
            lblNumOfPointTop.Location = new Point(0, 43);
            lblNumOfPointTop.Name = "lblNumOfPointTop";
            lblNumOfPointTop.Size = new Size(64, 15);
            lblNumOfPointTop.TabIndex = 12;
            lblNumOfPointTop.Text = "# of points";
            // 
            // lblNumOfPointBottom
            // 
            lblNumOfPointBottom.AutoSize = true;
            lblNumOfPointBottom.Location = new Point(0, 43);
            lblNumOfPointBottom.Name = "lblNumOfPointBottom";
            lblNumOfPointBottom.Size = new Size(64, 15);
            lblNumOfPointBottom.TabIndex = 13;
            lblNumOfPointBottom.Text = "# of points";
            // 
            // lblOrderTop
            // 
            lblOrderTop.AutoSize = true;
            lblOrderTop.Location = new Point(0, 0);
            lblOrderTop.Name = "lblOrderTop";
            lblOrderTop.Size = new Size(38, 15);
            lblOrderTop.TabIndex = 14;
            lblOrderTop.Text = "order:";
            // 
            // lblOrderBottom
            // 
            lblOrderBottom.AutoSize = true;
            lblOrderBottom.Location = new Point(0, 0);
            lblOrderBottom.Name = "lblOrderBottom";
            lblOrderBottom.Size = new Size(38, 15);
            lblOrderBottom.TabIndex = 15;
            lblOrderBottom.Text = "order:";
            // 
            // btnDecreaseOrderTop
            // 
            btnDecreaseOrderTop.AutoSize = true;
            btnDecreaseOrderTop.Location = new Point(31, 17);
            btnDecreaseOrderTop.Margin = new Padding(3, 2, 3, 2);
            btnDecreaseOrderTop.Name = "btnDecreaseOrderTop";
            btnDecreaseOrderTop.Size = new Size(25, 25);
            btnDecreaseOrderTop.TabIndex = 16;
            btnDecreaseOrderTop.Text = "-";
            btnDecreaseOrderTop.UseVisualStyleBackColor = true;
            btnDecreaseOrderTop.Click += btnDecreaseOrderTop_Click;
            // 
            // btnDecreaseOrderBottom
            // 
            btnDecreaseOrderBottom.AutoSize = true;
            btnDecreaseOrderBottom.BackColor = SystemColors.Control;
            btnDecreaseOrderBottom.ForeColor = SystemColors.ControlText;
            btnDecreaseOrderBottom.Location = new Point(31, 17);
            btnDecreaseOrderBottom.Margin = new Padding(3, 2, 3, 2);
            btnDecreaseOrderBottom.Name = "btnDecreaseOrderBottom";
            btnDecreaseOrderBottom.Size = new Size(25, 25);
            btnDecreaseOrderBottom.TabIndex = 17;
            btnDecreaseOrderBottom.Text = "-";
            btnDecreaseOrderBottom.UseVisualStyleBackColor = true;
            btnDecreaseOrderBottom.Click += btnDecreaseOrderBottom_Click;
            // 
            // txtAirfoilParam
            // 
            txtAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtAirfoilParam.Location = new Point(890, 443);
            txtAirfoilParam.Margin = new Padding(3, 2, 3, 2);
            txtAirfoilParam.Name = "txtAirfoilParam";
            txtAirfoilParam.Size = new Size(347, 173);
            txtAirfoilParam.TabIndex = 18;
            txtAirfoilParam.Text = "";
            // 
            // lblAirfoilParam
            // 
            lblAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblAirfoilParam.AutoSize = true;
            lblAirfoilParam.Location = new Point(891, 426);
            lblAirfoilParam.Name = "lblAirfoilParam";
            lblAirfoilParam.Size = new Size(101, 15);
            lblAirfoilParam.TabIndex = 19;
            lblAirfoilParam.Text = "Airfoil Parameters";
            // 
            // btnDefault
            // 
            btnDefault.AutoSize = true;
            btnDefault.Location = new Point(0, 17);
            btnDefault.Margin = new Padding(3, 2, 3, 2);
            btnDefault.Name = "btnDefault";
            btnDefault.Size = new Size(75, 25);
            btnDefault.TabIndex = 20;
            btnDefault.Text = "default";
            btnDefault.UseVisualStyleBackColor = true;
            btnDefault.Click += btnDefault_Click;
            // 
            // btnSaveBezDat
            // 
            btnSaveBezDat.AutoSize = true;
            btnSaveBezDat.Location = new Point(0, 311);
            btnSaveBezDat.Margin = new Padding(3, 2, 3, 2);
            btnSaveBezDat.Name = "btnSaveBezDat";
            btnSaveBezDat.Size = new Size(75, 25);
            btnSaveBezDat.TabIndex = 21;
            btnSaveBezDat.Text = ".bez.dat";
            btnSaveBezDat.UseVisualStyleBackColor = true;
            btnSaveBezDat.Click += btnSaveBezDat_Click;
            // 
            // chkShowControlTop
            // 
            chkShowControlTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowControlTop.AutoSize = true;
            chkShowControlTop.Location = new Point(54, 636);
            chkShowControlTop.Margin = new Padding(3, 2, 3, 2);
            chkShowControlTop.Name = "chkShowControlTop";
            chkShowControlTop.Size = new Size(64, 19);
            chkShowControlTop.TabIndex = 25;
            chkShowControlTop.Text = "ctrl top";
            chkShowControlTop.UseVisualStyleBackColor = true;
            chkShowControlTop.CheckedChanged += chkShowControlTop_CheckedChanged;
            // 
            // chkShowThickness
            // 
            chkShowThickness.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowThickness.AutoSize = true;
            chkShowThickness.Location = new Point(316, 636);
            chkShowThickness.Margin = new Padding(3, 2, 3, 2);
            chkShowThickness.Name = "chkShowThickness";
            chkShowThickness.Size = new Size(75, 19);
            chkShowThickness.TabIndex = 26;
            chkShowThickness.Text = "thickness";
            chkShowThickness.UseVisualStyleBackColor = true;
            chkShowThickness.CheckedChanged += chkShowThickness_CheckedChanged;
            // 
            // chkShowRadius
            // 
            chkShowRadius.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowRadius.AutoSize = true;
            chkShowRadius.Location = new Point(658, 636);
            chkShowRadius.Margin = new Padding(3, 2, 3, 2);
            chkShowRadius.Name = "chkShowRadius";
            chkShowRadius.Size = new Size(58, 19);
            chkShowRadius.TabIndex = 27;
            chkShowRadius.Text = "radius";
            chkShowRadius.UseVisualStyleBackColor = true;
            chkShowRadius.CheckedChanged += chkShowRadius_CheckedChanged;
            // 
            // lblShow
            // 
            lblShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblShow.AutoSize = true;
            lblShow.Location = new Point(10, 637);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(38, 15);
            lblShow.TabIndex = 28;
            lblShow.Text = "show:";
            // 
            // chkShowCamber
            // 
            chkShowCamber.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowCamber.AutoSize = true;
            chkShowCamber.Location = new Point(460, 636);
            chkShowCamber.Margin = new Padding(3, 2, 3, 2);
            chkShowCamber.Name = "chkShowCamber";
            chkShowCamber.Size = new Size(66, 19);
            chkShowCamber.TabIndex = 29;
            chkShowCamber.Text = "camber";
            chkShowCamber.UseVisualStyleBackColor = true;
            chkShowCamber.CheckedChanged += chkShowCamber_CheckedChanged;
            // 
            // btnAxisAuto
            // 
            btnAxisAuto.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAxisAuto.Location = new Point(9, 571);
            btnAxisAuto.Margin = new Padding(3, 2, 3, 2);
            btnAxisAuto.Name = "btnAxisAuto";
            btnAxisAuto.Size = new Size(56, 21);
            btnAxisAuto.TabIndex = 30;
            btnAxisAuto.Text = "Zoom";
            btnAxisAuto.UseVisualStyleBackColor = true;
            btnAxisAuto.Click += btnAxisAuto_Click;
            // 
            // btnLoadBezDat
            // 
            btnLoadBezDat.AutoSize = true;
            btnLoadBezDat.Location = new Point(0, 73);
            btnLoadBezDat.Margin = new Padding(3, 2, 3, 2);
            btnLoadBezDat.Name = "btnLoadBezDat";
            btnLoadBezDat.Size = new Size(75, 25);
            btnLoadBezDat.TabIndex = 32;
            btnLoadBezDat.Text = ".bez.dat";
            btnLoadBezDat.UseVisualStyleBackColor = true;
            btnLoadBezDat.Click += btnLoadBezDat_Click;
            // 
            // btnLoadDat
            // 
            btnLoadDat.AutoSize = true;
            btnLoadDat.Location = new Point(0, 45);
            btnLoadDat.Margin = new Padding(3, 2, 3, 2);
            btnLoadDat.Name = "btnLoadDat";
            btnLoadDat.Size = new Size(75, 25);
            btnLoadDat.TabIndex = 31;
            btnLoadDat.Text = ".dat";
            btnLoadDat.UseVisualStyleBackColor = true;
            btnLoadDat.Click += btnLoadDat_Click;
            btnLoadDat.MouseDown += btnLoadDat_MouseDown;
            // 
            // lblSave
            // 
            lblSave.AutoSize = true;
            lblSave.Location = new Point(-1, 266);
            lblSave.Name = "lblSave";
            lblSave.Size = new Size(33, 15);
            lblSave.TabIndex = 33;
            lblSave.Text = "save:";
            // 
            // lblLoad
            // 
            lblLoad.AutoSize = true;
            lblLoad.Location = new Point(0, 0);
            lblLoad.Name = "lblLoad";
            lblLoad.Size = new Size(33, 15);
            lblLoad.TabIndex = 34;
            lblLoad.Text = "load:";
            // 
            // chkShowReferenceTop
            // 
            chkShowReferenceTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowReferenceTop.AutoSize = true;
            chkShowReferenceTop.Location = new Point(722, 636);
            chkShowReferenceTop.Margin = new Padding(3, 2, 3, 2);
            chkShowReferenceTop.Name = "chkShowReferenceTop";
            chkShowReferenceTop.Size = new Size(61, 19);
            chkShowReferenceTop.TabIndex = 35;
            chkShowReferenceTop.Text = "ref top";
            chkShowReferenceTop.UseVisualStyleBackColor = true;
            chkShowReferenceTop.CheckedChanged += chkShowReferenceTop_CheckedChanged;
            // 
            // chkShowReferenceBottom
            // 
            chkShowReferenceBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowReferenceBottom.Location = new Point(789, 636);
            chkShowReferenceBottom.Margin = new Padding(3, 2, 3, 2);
            chkShowReferenceBottom.Name = "chkShowReferenceBottom";
            chkShowReferenceBottom.Size = new Size(61, 19);
            chkShowReferenceBottom.TabIndex = 36;
            chkShowReferenceBottom.Text = "ref bot";
            chkShowReferenceBottom.UseVisualStyleBackColor = true;
            chkShowReferenceBottom.CheckedChanged += chkShowReferenceBottom_CheckedChanged;
            // 
            // btnSearchTop
            // 
            btnSearchTop.AutoSize = true;
            btnSearchTop.Location = new Point(0, 143);
            btnSearchTop.Margin = new Padding(3, 2, 3, 2);
            btnSearchTop.Name = "btnSearchTop";
            btnSearchTop.Size = new Size(75, 25);
            btnSearchTop.TabIndex = 37;
            btnSearchTop.Text = "top";
            btnSearchTop.UseVisualStyleBackColor = true;
            btnSearchTop.Click += btnSearchTop_Click;
            // 
            // btnSearchBottom
            // 
            btnSearchBottom.AutoSize = true;
            btnSearchBottom.Location = new Point(0, 171);
            btnSearchBottom.Margin = new Padding(3, 2, 3, 2);
            btnSearchBottom.Name = "btnSearchBottom";
            btnSearchBottom.Size = new Size(75, 25);
            btnSearchBottom.TabIndex = 38;
            btnSearchBottom.Text = "bottom";
            btnSearchBottom.UseVisualStyleBackColor = true;
            btnSearchBottom.Click += btnSearchBottom_Click;
            // 
            // btnAutoSearch
            // 
            btnAutoSearch.AutoSize = true;
            btnAutoSearch.Location = new Point(0, 198);
            btnAutoSearch.Margin = new Padding(3, 2, 3, 2);
            btnAutoSearch.Name = "btnAutoSearch";
            btnAutoSearch.Size = new Size(75, 25);
            btnAutoSearch.TabIndex = 39;
            btnAutoSearch.Text = "auto";
            btnAutoSearch.UseVisualStyleBackColor = true;
            btnAutoSearch.Click += btnAutoSearch_Click;
            // 
            // chkShowTop
            // 
            chkShowTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowTop.AutoSize = true;
            chkShowTop.Location = new Point(194, 636);
            chkShowTop.Margin = new Padding(3, 2, 3, 2);
            chkShowTop.Name = "chkShowTop";
            chkShowTop.Size = new Size(44, 19);
            chkShowTop.TabIndex = 40;
            chkShowTop.Text = "top";
            chkShowTop.UseVisualStyleBackColor = true;
            chkShowTop.CheckedChanged += chkShowTop_CheckedChanged;
            // 
            // chkShowBottom
            // 
            chkShowBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowBottom.AutoSize = true;
            chkShowBottom.Location = new Point(244, 636);
            chkShowBottom.Margin = new Padding(3, 2, 3, 2);
            chkShowBottom.Name = "chkShowBottom";
            chkShowBottom.Size = new Size(66, 19);
            chkShowBottom.TabIndex = 41;
            chkShowBottom.Text = "bottom";
            chkShowBottom.UseVisualStyleBackColor = true;
            chkShowBottom.CheckedChanged += chkShowBottom_CheckedChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(0, 126);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(41, 15);
            lblSearch.TabIndex = 42;
            lblSearch.Text = "search";
            // 
            // chkShowControlBottom
            // 
            chkShowControlBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowControlBottom.AutoSize = true;
            chkShowControlBottom.Location = new Point(124, 636);
            chkShowControlBottom.Margin = new Padding(3, 2, 3, 2);
            chkShowControlBottom.Name = "chkShowControlBottom";
            chkShowControlBottom.Size = new Size(64, 19);
            chkShowControlBottom.TabIndex = 43;
            chkShowControlBottom.Text = "ctrl bot";
            chkShowControlBottom.UseVisualStyleBackColor = true;
            chkShowControlBottom.CheckedChanged += chkShowControlBottom_CheckedChanged;
            // 
            // txtCamberPosition
            // 
            txtCamberPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtCamberPosition.Location = new Point(595, 634);
            txtCamberPosition.Margin = new Padding(3, 2, 3, 2);
            txtCamberPosition.Name = "txtCamberPosition";
            txtCamberPosition.Size = new Size(57, 23);
            txtCamberPosition.TabIndex = 44;
            txtCamberPosition.Text = "0,5";
            txtCamberPosition.TextChanged += txtCamberPosition_TextChanged;
            // 
            // txtThicknessStepSize
            // 
            txtThicknessStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtThicknessStepSize.Location = new Point(397, 634);
            txtThicknessStepSize.Margin = new Padding(3, 2, 3, 2);
            txtThicknessStepSize.Name = "txtThicknessStepSize";
            txtThicknessStepSize.Size = new Size(57, 23);
            txtThicknessStepSize.TabIndex = 45;
            txtThicknessStepSize.Text = "0,001";
            txtThicknessStepSize.TextChanged += txtThicknessStepSize_TextChanged;
            // 
            // txtCamberStepSize
            // 
            txtCamberStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtCamberStepSize.Location = new Point(532, 634);
            txtCamberStepSize.Margin = new Padding(3, 2, 3, 2);
            txtCamberStepSize.Name = "txtCamberStepSize";
            txtCamberStepSize.Size = new Size(57, 23);
            txtCamberStepSize.TabIndex = 46;
            txtCamberStepSize.Text = "0,001";
            txtCamberStepSize.TextChanged += txtCamberStepSize_TextChanged;
            // 
            // lblThicknessStepSize
            // 
            lblThicknessStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblThicknessStepSize.AutoSize = true;
            lblThicknessStepSize.Location = new Point(397, 617);
            lblThicknessStepSize.Name = "lblThicknessStepSize";
            lblThicknessStepSize.Size = new Size(54, 15);
            lblThicknessStepSize.TabIndex = 47;
            lblThicknessStepSize.Text = "step size:";
            // 
            // lblCamberStepSize
            // 
            lblCamberStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCamberStepSize.AutoSize = true;
            lblCamberStepSize.Location = new Point(532, 617);
            lblCamberStepSize.Name = "lblCamberStepSize";
            lblCamberStepSize.Size = new Size(54, 15);
            lblCamberStepSize.TabIndex = 48;
            lblCamberStepSize.Text = "step size:";
            // 
            // lblCamberPosition
            // 
            lblCamberPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCamberPosition.AutoSize = true;
            lblCamberPosition.Location = new Point(595, 617);
            lblCamberPosition.Name = "lblCamberPosition";
            lblCamberPosition.Size = new Size(53, 15);
            lblCamberPosition.TabIndex = 49;
            lblCamberPosition.Text = "position:";
            // 
            // lblElapsedTime
            // 
            lblElapsedTime.AutoSize = true;
            lblElapsedTime.Location = new Point(0, 223);
            lblElapsedTime.Name = "lblElapsedTime";
            lblElapsedTime.Size = new Size(49, 15);
            lblElapsedTime.TabIndex = 50;
            lblElapsedTime.Text = "00:00:00";
            // 
            // btnStopSearch
            // 
            btnStopSearch.AutoSize = true;
            btnStopSearch.Location = new Point(0, 241);
            btnStopSearch.Margin = new Padding(3, 2, 3, 2);
            btnStopSearch.Name = "btnStopSearch";
            btnStopSearch.Size = new Size(75, 25);
            btnStopSearch.TabIndex = 51;
            btnStopSearch.Text = "stop";
            btnStopSearch.UseVisualStyleBackColor = true;
            btnStopSearch.Click += btnStopSearch_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar1.Location = new Point(0, 660);
            progressBar1.Margin = new Padding(3, 2, 3, 2);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1243, 4);
            progressBar1.TabIndex = 52;
            // 
            // btnLoadBez
            // 
            btnLoadBez.AutoSize = true;
            btnLoadBez.Location = new Point(0, 100);
            btnLoadBez.Margin = new Padding(3, 2, 3, 2);
            btnLoadBez.Name = "btnLoadBez";
            btnLoadBez.Size = new Size(75, 25);
            btnLoadBez.TabIndex = 53;
            btnLoadBez.Text = ".bez";
            btnLoadBez.UseVisualStyleBackColor = true;
            btnLoadBez.Click += btnLoadBez_Click;
            // 
            // btnSaveBez
            // 
            btnSaveBez.AutoSize = true;
            btnSaveBez.Location = new Point(0, 339);
            btnSaveBez.Margin = new Padding(3, 2, 3, 2);
            btnSaveBez.Name = "btnSaveBez";
            btnSaveBez.Size = new Size(75, 25);
            btnSaveBez.TabIndex = 54;
            btnSaveBez.Text = ".bez";
            btnSaveBez.UseVisualStyleBackColor = true;
            btnSaveBez.Click += btnSaveBez_Click;
            // 
            // cmbLanguage
            // 
            cmbLanguage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(1201, 632);
            cmbLanguage.Margin = new Padding(3, 2, 3, 2);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(36, 23);
            cmbLanguage.TabIndex = 56;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // btnCheckForUpdates
            // 
            btnCheckForUpdates.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCheckForUpdates.Location = new Point(1108, 633);
            btnCheckForUpdates.Margin = new Padding(3, 2, 3, 2);
            btnCheckForUpdates.Name = "btnCheckForUpdates";
            btnCheckForUpdates.Size = new Size(87, 23);
            btnCheckForUpdates.TabIndex = 57;
            btnCheckForUpdates.Text = "check version";
            btnCheckForUpdates.UseVisualStyleBackColor = true;
            btnCheckForUpdates.Click += btnCheckForUpdates_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel1.Controls.Add(lblOrderTop);
            panel1.Controls.Add(btnIncreaseOrderTop);
            panel1.Controls.Add(txtNumOfPointsTop);
            panel1.Controls.Add(lblNumOfPointTop);
            panel1.Controls.Add(btnDecreaseOrderTop);
            panel1.Location = new Point(1162, 25);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(75, 190);
            panel1.TabIndex = 58;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel2.Controls.Add(lblOrderBottom);
            panel2.Controls.Add(btnIncreaseOrderBottom);
            panel2.Controls.Add(txtNumOfPointBottom);
            panel2.Controls.Add(lblNumOfPointBottom);
            panel2.Controls.Add(btnDecreaseOrderBottom);
            panel2.Location = new Point(1162, 234);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(75, 190);
            panel2.TabIndex = 59;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel3.Controls.Add(button1);
            panel3.Controls.Add(lblLoad);
            panel3.Controls.Add(btnDefault);
            panel3.Controls.Add(btnLoadDat);
            panel3.Controls.Add(btnLoadBezDat);
            panel3.Controls.Add(btnLoadBez);
            panel3.Controls.Add(lblSearch);
            panel3.Controls.Add(btnSearchTop);
            panel3.Controls.Add(btnSearchBottom);
            panel3.Controls.Add(btnAutoSearch);
            panel3.Controls.Add(lblElapsedTime);
            panel3.Controls.Add(btnStopSearch);
            panel3.Controls.Add(lblSave);
            panel3.Controls.Add(btnSaveDat);
            panel3.Controls.Add(btnSaveBezDat);
            panel3.Controls.Add(btnSaveBez);
            panel3.Controls.Add(btnAxisAuto);
            panel3.Location = new Point(809, 25);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(75, 591);
            panel3.TabIndex = 60;
            // 
            // button1
            // 
            button1.Location = new Point(0, 366);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(75, 21);
            button1.TabIndex = 61;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1243, 664);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(btnCheckForUpdates);
            Controls.Add(cmbLanguage);
            Controls.Add(progressBar1);
            Controls.Add(lblCamberPosition);
            Controls.Add(lblCamberStepSize);
            Controls.Add(lblThicknessStepSize);
            Controls.Add(txtCamberStepSize);
            Controls.Add(txtThicknessStepSize);
            Controls.Add(txtCamberPosition);
            Controls.Add(chkShowControlBottom);
            Controls.Add(chkShowBottom);
            Controls.Add(chkShowTop);
            Controls.Add(chkShowReferenceBottom);
            Controls.Add(chkShowReferenceTop);
            Controls.Add(chkShowCamber);
            Controls.Add(lblShow);
            Controls.Add(chkShowRadius);
            Controls.Add(chkShowThickness);
            Controls.Add(chkShowControlTop);
            Controls.Add(lblAirfoilParam);
            Controls.Add(txtAirfoilParam);
            Controls.Add(lblBottom);
            Controls.Add(lblTop);
            Controls.Add(dataGridViewBottom);
            Controls.Add(dataGridViewTop);
            Controls.Add(formsPlot1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "BezierAirfoilDesigner";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private DataGridView dataGridViewTop;
        private DataGridView dataGridViewBottom;
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
        private TextBox txtCamberPosition;
        private TextBox txtThicknessStepSize;
        private TextBox txtCamberStepSize;
        private Label lblThicknessStepSize;
        private Label lblCamberStepSize;
        private Label lblCamberPosition;
        private Label lblElapsedTime;
        private Button btnStopSearch;
        private ProgressBar progressBar1;
        private Button btnLoadBez;
        private Button btnSaveBez;
        private ComboBox cmbLanguage;
        private Button btnCheckForUpdates;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button button1;
    }
}