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
            chkShowControlTop = new CheckBox();
            chkShowThickness = new CheckBox();
            chkShowRadius = new CheckBox();
            lblShow = new Label();
            chkShowCamber = new CheckBox();
            btnAxisAuto = new Button();
            btnLoadBezDat = new Button();
            btnLoadDat = new Button();
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
            cmbLanguage = new ComboBox();
            btnCheckForUpdates = new Button();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            lblNumberOfParticles = new Label();
            txtNumberOfParticles = new TextBox();
            lblErrorNumberOfSteps = new Label();
            txtErrorNumberOfSteps = new TextBox();
            btnStartPSOBottom = new Button();
            chkMatchTEGap = new CheckBox();
            lblErrorCalculationDistribution = new Label();
            cmbErrorCalculationDistribution = new ComboBox();
            chkUpdateUI = new CheckBox();
            lblErrorThresholdBottom = new Label();
            lblErrorThresholdTop = new Label();
            btnSave = new Button();
            txtErrorThresholdBottom = new TextBox();
            txtErrorThresholdTop = new TextBox();
            btnStartPSOTop = new Button();
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
            formsPlot1.Margin = new Padding(6, 0, 6, 0);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1515, 1357);
            formsPlot1.TabIndex = 0;
            formsPlot1.AxesChanged += formsPlot1_AxesChanged;
            formsPlot1.PlottableDragged += formsPlot1_PlottableDragged;
            // 
            // dataGridViewTop
            // 
            dataGridViewTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewTop.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTop.Location = new Point(1653, 53);
            dataGridViewTop.Margin = new Padding(6, 4, 6, 4);
            dataGridViewTop.Name = "dataGridViewTop";
            dataGridViewTop.RowHeadersWidth = 47;
            dataGridViewTop.RowTemplate.Height = 28;
            dataGridViewTop.Size = new Size(494, 405);
            dataGridViewTop.TabIndex = 1;
            dataGridViewTop.CellValueChanged += dataGridView1_CellValueChanged;
            // 
            // dataGridViewBottom
            // 
            dataGridViewBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridViewBottom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBottom.Location = new Point(1653, 499);
            dataGridViewBottom.Margin = new Padding(6, 4, 6, 4);
            dataGridViewBottom.Name = "dataGridViewBottom";
            dataGridViewBottom.RowHeadersWidth = 47;
            dataGridViewBottom.RowTemplate.Height = 28;
            dataGridViewBottom.Size = new Size(494, 405);
            dataGridViewBottom.TabIndex = 2;
            dataGridViewBottom.CellValueChanged += dataGridView2_CellValueChanged;
            // 
            // lblTop
            // 
            lblTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTop.AutoSize = true;
            lblTop.Location = new Point(1653, 17);
            lblTop.Margin = new Padding(6, 0, 6, 0);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(53, 32);
            lblTop.TabIndex = 3;
            lblTop.Text = "Top";
            // 
            // lblBottom
            // 
            lblBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblBottom.AutoSize = true;
            lblBottom.Location = new Point(1653, 463);
            lblBottom.Margin = new Padding(6, 0, 6, 0);
            lblBottom.Name = "lblBottom";
            lblBottom.Size = new Size(93, 32);
            lblBottom.TabIndex = 4;
            lblBottom.Text = "Bottom";
            // 
            // btnIncreaseOrderTop
            // 
            btnIncreaseOrderTop.AutoSize = true;
            btnIncreaseOrderTop.Location = new Point(0, 36);
            btnIncreaseOrderTop.Margin = new Padding(6, 4, 6, 4);
            btnIncreaseOrderTop.Name = "btnIncreaseOrderTop";
            btnIncreaseOrderTop.Size = new Size(74, 90);
            btnIncreaseOrderTop.TabIndex = 8;
            btnIncreaseOrderTop.Text = "+";
            btnIncreaseOrderTop.UseVisualStyleBackColor = true;
            btnIncreaseOrderTop.Click += btnIncreaseOrderTop_Click;
            // 
            // btnIncreaseOrderBottom
            // 
            btnIncreaseOrderBottom.AutoSize = true;
            btnIncreaseOrderBottom.Location = new Point(0, 36);
            btnIncreaseOrderBottom.Margin = new Padding(6, 4, 6, 4);
            btnIncreaseOrderBottom.Name = "btnIncreaseOrderBottom";
            btnIncreaseOrderBottom.Size = new Size(74, 90);
            btnIncreaseOrderBottom.TabIndex = 9;
            btnIncreaseOrderBottom.Text = "+";
            btnIncreaseOrderBottom.UseVisualStyleBackColor = true;
            btnIncreaseOrderBottom.Click += btnIncreaseOrderBottom_Click;
            // 
            // txtNumOfPointsTop
            // 
            txtNumOfPointsTop.Location = new Point(0, 128);
            txtNumOfPointsTop.Margin = new Padding(6, 4, 6, 4);
            txtNumOfPointsTop.Name = "txtNumOfPointsTop";
            txtNumOfPointsTop.Size = new Size(138, 39);
            txtNumOfPointsTop.TabIndex = 10;
            txtNumOfPointsTop.Text = "150";
            txtNumOfPointsTop.TextChanged += txtNumOfPointsTop_TextChanged;
            // 
            // txtNumOfPointBottom
            // 
            txtNumOfPointBottom.Location = new Point(0, 128);
            txtNumOfPointBottom.Margin = new Padding(6, 4, 6, 4);
            txtNumOfPointBottom.Name = "txtNumOfPointBottom";
            txtNumOfPointBottom.Size = new Size(138, 39);
            txtNumOfPointBottom.TabIndex = 11;
            txtNumOfPointBottom.Text = "150";
            txtNumOfPointBottom.TextChanged += txtNumOfPointsBottom_TextChanged;
            // 
            // lblNumOfPointTop
            // 
            lblNumOfPointTop.AutoSize = true;
            lblNumOfPointTop.Location = new Point(0, 92);
            lblNumOfPointTop.Margin = new Padding(6, 0, 6, 0);
            lblNumOfPointTop.Name = "lblNumOfPointTop";
            lblNumOfPointTop.Size = new Size(130, 32);
            lblNumOfPointTop.TabIndex = 12;
            lblNumOfPointTop.Text = "# of points";
            // 
            // lblNumOfPointBottom
            // 
            lblNumOfPointBottom.AutoSize = true;
            lblNumOfPointBottom.Location = new Point(0, 92);
            lblNumOfPointBottom.Margin = new Padding(6, 0, 6, 0);
            lblNumOfPointBottom.Name = "lblNumOfPointBottom";
            lblNumOfPointBottom.Size = new Size(130, 32);
            lblNumOfPointBottom.TabIndex = 13;
            lblNumOfPointBottom.Text = "# of points";
            // 
            // lblOrderTop
            // 
            lblOrderTop.AutoSize = true;
            lblOrderTop.Location = new Point(0, 0);
            lblOrderTop.Margin = new Padding(6, 0, 6, 0);
            lblOrderTop.Name = "lblOrderTop";
            lblOrderTop.Size = new Size(76, 32);
            lblOrderTop.TabIndex = 14;
            lblOrderTop.Text = "order:";
            // 
            // lblOrderBottom
            // 
            lblOrderBottom.AutoSize = true;
            lblOrderBottom.Location = new Point(0, 0);
            lblOrderBottom.Margin = new Padding(6, 0, 6, 0);
            lblOrderBottom.Name = "lblOrderBottom";
            lblOrderBottom.Size = new Size(76, 32);
            lblOrderBottom.TabIndex = 15;
            lblOrderBottom.Text = "order:";
            // 
            // btnDecreaseOrderTop
            // 
            btnDecreaseOrderTop.AutoSize = true;
            btnDecreaseOrderTop.Location = new Point(58, 36);
            btnDecreaseOrderTop.Margin = new Padding(6, 4, 6, 4);
            btnDecreaseOrderTop.Name = "btnDecreaseOrderTop";
            btnDecreaseOrderTop.Size = new Size(63, 90);
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
            btnDecreaseOrderBottom.Location = new Point(58, 36);
            btnDecreaseOrderBottom.Margin = new Padding(6, 4, 6, 4);
            btnDecreaseOrderBottom.Name = "btnDecreaseOrderBottom";
            btnDecreaseOrderBottom.Size = new Size(63, 90);
            btnDecreaseOrderBottom.TabIndex = 17;
            btnDecreaseOrderBottom.Text = "-";
            btnDecreaseOrderBottom.UseVisualStyleBackColor = true;
            btnDecreaseOrderBottom.Click += btnDecreaseOrderBottom_Click;
            // 
            // txtAirfoilParam
            // 
            txtAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtAirfoilParam.Location = new Point(1653, 945);
            txtAirfoilParam.Margin = new Padding(6, 4, 6, 4);
            txtAirfoilParam.Name = "txtAirfoilParam";
            txtAirfoilParam.Size = new Size(641, 365);
            txtAirfoilParam.TabIndex = 18;
            txtAirfoilParam.Text = "";
            // 
            // lblAirfoilParam
            // 
            lblAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblAirfoilParam.AutoSize = true;
            lblAirfoilParam.Location = new Point(1655, 909);
            lblAirfoilParam.Margin = new Padding(6, 0, 6, 0);
            lblAirfoilParam.Name = "lblAirfoilParam";
            lblAirfoilParam.Size = new Size(201, 32);
            lblAirfoilParam.TabIndex = 19;
            lblAirfoilParam.Text = "Airfoil Parameters";
            // 
            // btnDefault
            // 
            btnDefault.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnDefault.Location = new Point(0, 30);
            btnDefault.Margin = new Padding(6, 2, 6, 2);
            btnDefault.Name = "btnDefault";
            btnDefault.Size = new Size(139, 49);
            btnDefault.TabIndex = 20;
            btnDefault.Text = "default";
            btnDefault.UseVisualStyleBackColor = true;
            btnDefault.Click += btnDefault_Click;
            // 
            // chkShowControlTop
            // 
            chkShowControlTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowControlTop.AutoSize = true;
            chkShowControlTop.Location = new Point(74, 1368);
            chkShowControlTop.Margin = new Padding(2, 0, 2, 0);
            chkShowControlTop.Name = "chkShowControlTop";
            chkShowControlTop.Size = new Size(122, 36);
            chkShowControlTop.TabIndex = 25;
            chkShowControlTop.Text = "ctrl top";
            chkShowControlTop.UseVisualStyleBackColor = true;
            chkShowControlTop.CheckedChanged += chkShowControlTop_CheckedChanged;
            // 
            // chkShowThickness
            // 
            chkShowThickness.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowThickness.AutoSize = true;
            chkShowThickness.Location = new Point(539, 1368);
            chkShowThickness.Margin = new Padding(2, 0, 2, 0);
            chkShowThickness.Name = "chkShowThickness";
            chkShowThickness.Size = new Size(144, 36);
            chkShowThickness.TabIndex = 26;
            chkShowThickness.Text = "thickness";
            chkShowThickness.UseVisualStyleBackColor = true;
            chkShowThickness.CheckedChanged += chkShowThickness_CheckedChanged;
            // 
            // chkShowRadius
            // 
            chkShowRadius.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowRadius.AutoSize = true;
            chkShowRadius.Location = new Point(1137, 1368);
            chkShowRadius.Margin = new Padding(2, 0, 2, 0);
            chkShowRadius.Name = "chkShowRadius";
            chkShowRadius.Size = new Size(110, 36);
            chkShowRadius.TabIndex = 27;
            chkShowRadius.Text = "radius";
            chkShowRadius.UseVisualStyleBackColor = true;
            chkShowRadius.CheckedChanged += chkShowRadius_CheckedChanged;
            // 
            // lblShow
            // 
            lblShow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblShow.AutoSize = true;
            lblShow.Location = new Point(0, 1365);
            lblShow.Margin = new Padding(2, 0, 2, 0);
            lblShow.Name = "lblShow";
            lblShow.Size = new Size(74, 32);
            lblShow.TabIndex = 28;
            lblShow.Text = "show:";
            // 
            // chkShowCamber
            // 
            chkShowCamber.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowCamber.AutoSize = true;
            chkShowCamber.Location = new Point(791, 1368);
            chkShowCamber.Margin = new Padding(2, 0, 2, 0);
            chkShowCamber.Name = "chkShowCamber";
            chkShowCamber.Size = new Size(125, 36);
            chkShowCamber.TabIndex = 29;
            chkShowCamber.Text = "camber";
            chkShowCamber.UseVisualStyleBackColor = true;
            chkShowCamber.CheckedChanged += chkShowCamber_CheckedChanged;
            // 
            // btnAxisAuto
            // 
            btnAxisAuto.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAxisAuto.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAxisAuto.Location = new Point(0, 1248);
            btnAxisAuto.Margin = new Padding(6, 0, 6, 0);
            btnAxisAuto.Name = "btnAxisAuto";
            btnAxisAuto.Size = new Size(139, 49);
            btnAxisAuto.TabIndex = 30;
            btnAxisAuto.Text = "Zoom";
            btnAxisAuto.UseVisualStyleBackColor = true;
            btnAxisAuto.Click += btnAxisAuto_Click;
            // 
            // btnLoadBezDat
            // 
            btnLoadBezDat.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadBezDat.Location = new Point(0, 173);
            btnLoadBezDat.Margin = new Padding(6, 2, 6, 2);
            btnLoadBezDat.Name = "btnLoadBezDat";
            btnLoadBezDat.Size = new Size(139, 49);
            btnLoadBezDat.TabIndex = 32;
            btnLoadBezDat.Text = ".bez.dat";
            btnLoadBezDat.UseVisualStyleBackColor = true;
            btnLoadBezDat.Click += btnLoadBezDat_Click;
            // 
            // btnLoadDat
            // 
            btnLoadDat.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadDat.Location = new Point(0, 83);
            btnLoadDat.Margin = new Padding(6, 2, 6, 2);
            btnLoadDat.Name = "btnLoadDat";
            btnLoadDat.Size = new Size(139, 49);
            btnLoadDat.TabIndex = 31;
            btnLoadDat.Text = ".dat";
            btnLoadDat.UseVisualStyleBackColor = true;
            btnLoadDat.Click += btnLoadDat_Click;
            btnLoadDat.MouseDown += btnLoadDat_MouseDown;
            // 
            // lblLoad
            // 
            lblLoad.AutoSize = true;
            lblLoad.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblLoad.Location = new Point(0, 0);
            lblLoad.Margin = new Padding(6, 0, 6, 0);
            lblLoad.Name = "lblLoad";
            lblLoad.Size = new Size(60, 30);
            lblLoad.TabIndex = 34;
            lblLoad.Text = "load:";
            // 
            // chkShowReferenceTop
            // 
            chkShowReferenceTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowReferenceTop.AutoSize = true;
            chkShowReferenceTop.Location = new Point(1248, 1368);
            chkShowReferenceTop.Margin = new Padding(2, 0, 2, 0);
            chkShowReferenceTop.Name = "chkShowReferenceTop";
            chkShowReferenceTop.Size = new Size(118, 36);
            chkShowReferenceTop.TabIndex = 35;
            chkShowReferenceTop.Text = "ref top";
            chkShowReferenceTop.UseVisualStyleBackColor = true;
            chkShowReferenceTop.CheckedChanged += chkShowReferenceTop_CheckedChanged;
            // 
            // chkShowReferenceBottom
            // 
            chkShowReferenceBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowReferenceBottom.Location = new Point(1365, 1363);
            chkShowReferenceBottom.Margin = new Padding(2, 0, 2, 0);
            chkShowReferenceBottom.Name = "chkShowReferenceBottom";
            chkShowReferenceBottom.Size = new Size(128, 41);
            chkShowReferenceBottom.TabIndex = 36;
            chkShowReferenceBottom.Text = "ref btm";
            chkShowReferenceBottom.UseVisualStyleBackColor = true;
            chkShowReferenceBottom.CheckedChanged += chkShowReferenceBottom_CheckedChanged;
            // 
            // btnSearchTop
            // 
            btnSearchTop.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSearchTop.Location = new Point(0, 343);
            btnSearchTop.Margin = new Padding(6, 2, 6, 2);
            btnSearchTop.Name = "btnSearchTop";
            btnSearchTop.Size = new Size(139, 49);
            btnSearchTop.TabIndex = 37;
            btnSearchTop.Text = "top";
            btnSearchTop.UseVisualStyleBackColor = true;
            btnSearchTop.Click += btnSearchTop_Click;
            btnSearchTop.MouseDown += btnSearchTop_MouseDown;
            // 
            // btnSearchBottom
            // 
            btnSearchBottom.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSearchBottom.Location = new Point(0, 397);
            btnSearchBottom.Margin = new Padding(6, 2, 6, 2);
            btnSearchBottom.Name = "btnSearchBottom";
            btnSearchBottom.Size = new Size(139, 49);
            btnSearchBottom.TabIndex = 38;
            btnSearchBottom.Text = "bottom";
            btnSearchBottom.UseVisualStyleBackColor = true;
            btnSearchBottom.Click += btnSearchBottom_Click;
            btnSearchBottom.MouseDown += btnSearchBottom_MouseDown;
            // 
            // btnAutoSearch
            // 
            btnAutoSearch.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnAutoSearch.Location = new Point(0, 450);
            btnAutoSearch.Margin = new Padding(6, 2, 6, 2);
            btnAutoSearch.Name = "btnAutoSearch";
            btnAutoSearch.Size = new Size(139, 49);
            btnAutoSearch.TabIndex = 39;
            btnAutoSearch.Text = "auto";
            btnAutoSearch.UseVisualStyleBackColor = true;
            btnAutoSearch.Click += btnAutoSearch_Click;
            // 
            // chkShowTop
            // 
            chkShowTop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowTop.AutoSize = true;
            chkShowTop.Location = new Point(327, 1368);
            chkShowTop.Margin = new Padding(2, 0, 2, 0);
            chkShowTop.Name = "chkShowTop";
            chkShowTop.Size = new Size(82, 36);
            chkShowTop.TabIndex = 40;
            chkShowTop.Text = "top";
            chkShowTop.UseVisualStyleBackColor = true;
            chkShowTop.CheckedChanged += chkShowTop_CheckedChanged;
            // 
            // chkShowBottom
            // 
            chkShowBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowBottom.AutoSize = true;
            chkShowBottom.Location = new Point(412, 1368);
            chkShowBottom.Margin = new Padding(2, 0, 2, 0);
            chkShowBottom.Name = "chkShowBottom";
            chkShowBottom.Size = new Size(125, 36);
            chkShowBottom.TabIndex = 41;
            chkShowBottom.Text = "bottom";
            chkShowBottom.UseVisualStyleBackColor = true;
            chkShowBottom.CheckedChanged += chkShowBottom_CheckedChanged;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblSearch.Location = new Point(0, 277);
            lblSearch.Margin = new Padding(6, 0, 6, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(75, 30);
            lblSearch.TabIndex = 42;
            lblSearch.Text = "search";
            // 
            // chkShowControlBottom
            // 
            chkShowControlBottom.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkShowControlBottom.AutoSize = true;
            chkShowControlBottom.Location = new Point(197, 1368);
            chkShowControlBottom.Margin = new Padding(2, 0, 2, 0);
            chkShowControlBottom.Name = "chkShowControlBottom";
            chkShowControlBottom.Size = new Size(129, 36);
            chkShowControlBottom.TabIndex = 43;
            chkShowControlBottom.Text = "ctrl btm";
            chkShowControlBottom.UseVisualStyleBackColor = true;
            chkShowControlBottom.CheckedChanged += chkShowControlBottom_CheckedChanged;
            // 
            // txtCamberPosition
            // 
            txtCamberPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtCamberPosition.Location = new Point(1027, 1359);
            txtCamberPosition.Margin = new Padding(2, 0, 2, 0);
            txtCamberPosition.Name = "txtCamberPosition";
            txtCamberPosition.Size = new Size(102, 39);
            txtCamberPosition.TabIndex = 44;
            txtCamberPosition.Text = "0,5";
            txtCamberPosition.TextChanged += txtCamberPosition_TextChanged;
            // 
            // txtThicknessStepSize
            // 
            txtThicknessStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtThicknessStepSize.Location = new Point(682, 1359);
            txtThicknessStepSize.Margin = new Padding(2, 0, 2, 0);
            txtThicknessStepSize.Name = "txtThicknessStepSize";
            txtThicknessStepSize.Size = new Size(102, 39);
            txtThicknessStepSize.TabIndex = 45;
            txtThicknessStepSize.Text = "1000";
            txtThicknessStepSize.TextChanged += txtThicknessStepSize_TextChanged;
            // 
            // txtCamberStepSize
            // 
            txtCamberStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            txtCamberStepSize.Location = new Point(917, 1359);
            txtCamberStepSize.Margin = new Padding(2, 0, 2, 0);
            txtCamberStepSize.Name = "txtCamberStepSize";
            txtCamberStepSize.Size = new Size(102, 39);
            txtCamberStepSize.TabIndex = 46;
            txtCamberStepSize.Text = "1000";
            txtCamberStepSize.TextChanged += txtCamberStepSize_TextChanged;
            // 
            // lblThicknessStepSize
            // 
            lblThicknessStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblThicknessStepSize.AutoSize = true;
            lblThicknessStepSize.Location = new Point(682, 1327);
            lblThicknessStepSize.Margin = new Padding(2, 0, 2, 0);
            lblThicknessStepSize.Name = "lblThicknessStepSize";
            lblThicknessStepSize.Size = new Size(124, 32);
            lblThicknessStepSize.TabIndex = 47;
            lblThicknessStepSize.Text = "# of steps:";
            // 
            // lblCamberStepSize
            // 
            lblCamberStepSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCamberStepSize.AutoSize = true;
            lblCamberStepSize.Location = new Point(917, 1327);
            lblCamberStepSize.Margin = new Padding(2, 0, 2, 0);
            lblCamberStepSize.Name = "lblCamberStepSize";
            lblCamberStepSize.Size = new Size(124, 32);
            lblCamberStepSize.TabIndex = 48;
            lblCamberStepSize.Text = "# of steps:";
            // 
            // lblCamberPosition
            // 
            lblCamberPosition.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblCamberPosition.AutoSize = true;
            lblCamberPosition.Location = new Point(1027, 1325);
            lblCamberPosition.Margin = new Padding(2, 0, 2, 0);
            lblCamberPosition.Name = "lblCamberPosition";
            lblCamberPosition.Size = new Size(105, 32);
            lblCamberPosition.TabIndex = 49;
            lblCamberPosition.Text = "position:";
            // 
            // lblElapsedTime
            // 
            lblElapsedTime.AutoSize = true;
            lblElapsedTime.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblElapsedTime.Location = new Point(24, 868);
            lblElapsedTime.Margin = new Padding(6, 0, 6, 0);
            lblElapsedTime.Name = "lblElapsedTime";
            lblElapsedTime.Size = new Size(95, 30);
            lblElapsedTime.TabIndex = 50;
            lblElapsedTime.Text = "00:00:00";
            // 
            // btnStopSearch
            // 
            btnStopSearch.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnStopSearch.Location = new Point(0, 817);
            btnStopSearch.Margin = new Padding(6, 2, 6, 2);
            btnStopSearch.Name = "btnStopSearch";
            btnStopSearch.Size = new Size(139, 49);
            btnStopSearch.TabIndex = 51;
            btnStopSearch.Text = "stop";
            btnStopSearch.UseVisualStyleBackColor = true;
            btnStopSearch.Click += btnStopSearch_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar1.Location = new Point(0, 1408);
            progressBar1.Margin = new Padding(6, 4, 6, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(2308, 9);
            progressBar1.TabIndex = 52;
            // 
            // btnLoadBez
            // 
            btnLoadBez.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadBez.Location = new Point(0, 226);
            btnLoadBez.Margin = new Padding(6, 2, 6, 2);
            btnLoadBez.Name = "btnLoadBez";
            btnLoadBez.Size = new Size(139, 49);
            btnLoadBez.TabIndex = 53;
            btnLoadBez.Text = ".bez";
            btnLoadBez.UseVisualStyleBackColor = true;
            btnLoadBez.Click += btnLoadBez_Click;
            // 
            // cmbLanguage
            // 
            cmbLanguage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.Location = new Point(2230, 1354);
            cmbLanguage.Margin = new Padding(6, 4, 6, 4);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(63, 40);
            cmbLanguage.TabIndex = 56;
            cmbLanguage.SelectedIndexChanged += cmbLanguage_SelectedIndexChanged;
            // 
            // btnCheckForUpdates
            // 
            btnCheckForUpdates.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCheckForUpdates.Location = new Point(2056, 1354);
            btnCheckForUpdates.Margin = new Padding(6, 4, 6, 4);
            btnCheckForUpdates.Name = "btnCheckForUpdates";
            btnCheckForUpdates.Size = new Size(162, 49);
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
            panel1.Location = new Point(2158, 53);
            panel1.Margin = new Padding(6, 4, 6, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(139, 405);
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
            panel2.Location = new Point(2158, 499);
            panel2.Margin = new Padding(6, 4, 6, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(139, 405);
            panel2.TabIndex = 59;
            // 
            // panel3
            // 
            panel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panel3.Controls.Add(lblNumberOfParticles);
            panel3.Controls.Add(txtNumberOfParticles);
            panel3.Controls.Add(lblErrorNumberOfSteps);
            panel3.Controls.Add(txtErrorNumberOfSteps);
            panel3.Controls.Add(btnStartPSOBottom);
            panel3.Controls.Add(chkMatchTEGap);
            panel3.Controls.Add(lblErrorCalculationDistribution);
            panel3.Controls.Add(cmbErrorCalculationDistribution);
            panel3.Controls.Add(chkUpdateUI);
            panel3.Controls.Add(lblErrorThresholdBottom);
            panel3.Controls.Add(lblErrorThresholdTop);
            panel3.Controls.Add(btnSave);
            panel3.Controls.Add(txtErrorThresholdBottom);
            panel3.Controls.Add(txtErrorThresholdTop);
            panel3.Controls.Add(btnStartPSOTop);
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
            panel3.Controls.Add(btnAxisAuto);
            panel3.Location = new Point(1502, 17);
            panel3.Margin = new Padding(6, 4, 6, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(139, 1297);
            panel3.TabIndex = 60;
            // 
            // lblNumberOfParticles
            // 
            lblNumberOfParticles.AutoSize = true;
            lblNumberOfParticles.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblNumberOfParticles.Location = new Point(0, 1003);
            lblNumberOfParticles.Margin = new Padding(6, 0, 6, 0);
            lblNumberOfParticles.Name = "lblNumberOfParticles";
            lblNumberOfParticles.Size = new Size(138, 30);
            lblNumberOfParticles.TabIndex = 76;
            lblNumberOfParticles.Text = "# of particles";
            // 
            // txtNumberOfParticles
            // 
            txtNumberOfParticles.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtNumberOfParticles.Location = new Point(0, 1033);
            txtNumberOfParticles.Margin = new Padding(6, 2, 6, 2);
            txtNumberOfParticles.Name = "txtNumberOfParticles";
            txtNumberOfParticles.Size = new Size(136, 37);
            txtNumberOfParticles.TabIndex = 75;
            txtNumberOfParticles.Text = "100";
            // 
            // lblErrorNumberOfSteps
            // 
            lblErrorNumberOfSteps.AutoSize = true;
            lblErrorNumberOfSteps.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorNumberOfSteps.Location = new Point(0, 578);
            lblErrorNumberOfSteps.Margin = new Padding(6, 0, 6, 0);
            lblErrorNumberOfSteps.Name = "lblErrorNumberOfSteps";
            lblErrorNumberOfSteps.Size = new Size(108, 30);
            lblErrorNumberOfSteps.TabIndex = 74;
            lblErrorNumberOfSteps.Text = "# of steps";
            // 
            // txtErrorNumberOfSteps
            // 
            txtErrorNumberOfSteps.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorNumberOfSteps.Location = new Point(0, 608);
            txtErrorNumberOfSteps.Margin = new Padding(6, 2, 6, 2);
            txtErrorNumberOfSteps.Name = "txtErrorNumberOfSteps";
            txtErrorNumberOfSteps.Size = new Size(136, 37);
            txtErrorNumberOfSteps.TabIndex = 73;
            txtErrorNumberOfSteps.Text = "1000";
            // 
            // btnStartPSOBottom
            // 
            btnStartPSOBottom.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnStartPSOBottom.Location = new Point(0, 951);
            btnStartPSOBottom.Margin = new Padding(6, 2, 6, 2);
            btnStartPSOBottom.Name = "btnStartPSOBottom";
            btnStartPSOBottom.Size = new Size(139, 49);
            btnStartPSOBottom.TabIndex = 72;
            btnStartPSOBottom.Text = "PSO btm";
            btnStartPSOBottom.UseVisualStyleBackColor = true;
            btnStartPSOBottom.Click += btnStartPSOBottom_Click;
            // 
            // chkMatchTEGap
            // 
            chkMatchTEGap.AutoSize = true;
            chkMatchTEGap.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            chkMatchTEGap.Location = new Point(0, 134);
            chkMatchTEGap.Margin = new Padding(6, 0, 6, 0);
            chkMatchTEGap.Name = "chkMatchTEGap";
            chkMatchTEGap.Size = new Size(133, 34);
            chkMatchTEGap.TabIndex = 71;
            chkMatchTEGap.Text = "match TE";
            chkMatchTEGap.UseVisualStyleBackColor = true;
            // 
            // lblErrorCalculationDistribution
            // 
            lblErrorCalculationDistribution.AutoSize = true;
            lblErrorCalculationDistribution.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorCalculationDistribution.Location = new Point(0, 501);
            lblErrorCalculationDistribution.Margin = new Padding(6, 0, 6, 0);
            lblErrorCalculationDistribution.Name = "lblErrorCalculationDistribution";
            lblErrorCalculationDistribution.Size = new Size(124, 30);
            lblErrorCalculationDistribution.TabIndex = 70;
            lblErrorCalculationDistribution.Text = "ErrCalcDistr";
            // 
            // cmbErrorCalculationDistribution
            // 
            cmbErrorCalculationDistribution.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            cmbErrorCalculationDistribution.FormattingEnabled = true;
            cmbErrorCalculationDistribution.Location = new Point(0, 531);
            cmbErrorCalculationDistribution.Margin = new Padding(6, 2, 6, 2);
            cmbErrorCalculationDistribution.Name = "cmbErrorCalculationDistribution";
            cmbErrorCalculationDistribution.Size = new Size(136, 38);
            cmbErrorCalculationDistribution.TabIndex = 61;
            cmbErrorCalculationDistribution.SelectedIndexChanged += cmbErrorCalculationDistribution_SelectedIndexChanged;
            // 
            // chkUpdateUI
            // 
            chkUpdateUI.AutoSize = true;
            chkUpdateUI.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            chkUpdateUI.Location = new Point(0, 305);
            chkUpdateUI.Margin = new Padding(6, 0, 6, 0);
            chkUpdateUI.Name = "chkUpdateUI";
            chkUpdateUI.Size = new Size(140, 34);
            chkUpdateUI.TabIndex = 61;
            chkUpdateUI.Text = "update UI";
            chkUpdateUI.UseVisualStyleBackColor = true;
            chkUpdateUI.CheckedChanged += chkUpdateUI_CheckedChanged;
            // 
            // lblErrorThresholdBottom
            // 
            lblErrorThresholdBottom.AutoSize = true;
            lblErrorThresholdBottom.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorThresholdBottom.Location = new Point(0, 736);
            lblErrorThresholdBottom.Margin = new Padding(6, 0, 6, 0);
            lblErrorThresholdBottom.Name = "lblErrorThresholdBottom";
            lblErrorThresholdBottom.Size = new Size(144, 30);
            lblErrorThresholdBottom.TabIndex = 69;
            lblErrorThresholdBottom.Text = "ErrThreshBtm";
            // 
            // lblErrorThresholdTop
            // 
            lblErrorThresholdTop.AutoSize = true;
            lblErrorThresholdTop.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorThresholdTop.Location = new Point(0, 657);
            lblErrorThresholdTop.Margin = new Padding(6, 0, 6, 0);
            lblErrorThresholdTop.Name = "lblErrorThresholdTop";
            lblErrorThresholdTop.Size = new Size(141, 30);
            lblErrorThresholdTop.TabIndex = 68;
            lblErrorThresholdTop.Text = "ErrThreshTop";
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnSave.Location = new Point(0, 1199);
            btnSave.Margin = new Padding(6, 0, 6, 0);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(139, 49);
            btnSave.TabIndex = 67;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // txtErrorThresholdBottom
            // 
            txtErrorThresholdBottom.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorThresholdBottom.Location = new Point(0, 766);
            txtErrorThresholdBottom.Margin = new Padding(6, 2, 6, 2);
            txtErrorThresholdBottom.Name = "txtErrorThresholdBottom";
            txtErrorThresholdBottom.Size = new Size(136, 37);
            txtErrorThresholdBottom.TabIndex = 66;
            txtErrorThresholdBottom.Text = "7,5e-5";
            txtErrorThresholdBottom.TextChanged += txtErrorThresholdBottom_TextChanged;
            // 
            // txtErrorThresholdTop
            // 
            txtErrorThresholdTop.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorThresholdTop.Location = new Point(0, 687);
            txtErrorThresholdTop.Margin = new Padding(6, 2, 6, 2);
            txtErrorThresholdTop.Name = "txtErrorThresholdTop";
            txtErrorThresholdTop.Size = new Size(136, 37);
            txtErrorThresholdTop.TabIndex = 65;
            txtErrorThresholdTop.Text = "7,5e-5";
            txtErrorThresholdTop.TextChanged += txtErrorThresholdTop_TextChanged;
            // 
            // btnStartPSOTop
            // 
            btnStartPSOTop.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            btnStartPSOTop.Location = new Point(0, 898);
            btnStartPSOTop.Margin = new Padding(6, 2, 6, 2);
            btnStartPSOTop.Name = "btnStartPSOTop";
            btnStartPSOTop.Size = new Size(139, 49);
            btnStartPSOTop.TabIndex = 61;
            btnStartPSOTop.Text = "PSO top";
            btnStartPSOTop.UseVisualStyleBackColor = true;
            btnStartPSOTop.Click += btnStartPSOTop_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2308, 1417);
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
            Margin = new Padding(6, 4, 6, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
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
        private CheckBox chkShowControlTop;
        private CheckBox chkShowThickness;
        private CheckBox chkShowRadius;
        private Label lblShow;
        private CheckBox chkShowCamber;
        private Button btnAxisAuto;
        private Button btnLoadBezDat;
        private Button btnLoadDat;
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
        private ComboBox cmbLanguage;
        private Button btnCheckForUpdates;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox txtErrorThresholdBottom;
        private TextBox txtErrorThresholdTop;
        private Button btnStartPSOTop;
        private Button btnSave;
        private Label lblErrorThresholdBottom;
        private Label lblErrorThresholdTop;
        private CheckBox chkUpdateUI;
        private Label lblErrorCalculationDistribution;
        private ComboBox cmbErrorCalculationDistribution;
        private CheckBox chkMatchTEGap;
        private Button btnStartPSOBottom;
        private Label lblNumberOfParticles;
        private TextBox txtNumberOfParticles;
        private Label lblErrorNumberOfSteps;
        private TextBox txtErrorNumberOfSteps;
    }
}