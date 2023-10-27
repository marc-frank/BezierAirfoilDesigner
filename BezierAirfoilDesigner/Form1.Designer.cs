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
            txtAirfoilParam = new RichTextBox();
            lblAirfoilParam = new Label();
            chkShowControlTop = new CheckBox();
            chkShowThickness = new CheckBox();
            chkShowRadius = new CheckBox();
            chkShowCamber = new CheckBox();
            btnAxisAuto = new Button();
            chkShowReferenceTop = new CheckBox();
            chkShowReferenceBottom = new CheckBox();
            chkShowTop = new CheckBox();
            chkShowBottom = new CheckBox();
            chkShowControlBottom = new CheckBox();
            txtCamberPosition = new TextBox();
            txtThicknessStepSize = new TextBox();
            txtCamberStepSize = new TextBox();
            lblThicknessStepSize = new Label();
            lblCamberStepSize = new Label();
            lblCamberPosition = new Label();
            progressBar1 = new ProgressBar();
            cmbLanguage = new ComboBox();
            btnCheckForUpdates = new Button();
            tabControl1 = new TabControl();
            tabPageGridView = new TabPage();
            lblTop = new Label();
            dataGridViewTop = new DataGridView();
            panel2 = new Panel();
            lblOrderBottom = new Label();
            btnIncreaseOrderBottom = new Button();
            txtNumOfPointBottom = new TextBox();
            lblNumOfPointBottom = new Label();
            btnDecreaseOrderBottom = new Button();
            dataGridViewBottom = new DataGridView();
            panel1 = new Panel();
            lblOrderTop = new Label();
            btnIncreaseOrderTop = new Button();
            txtNumOfPointsTop = new TextBox();
            lblNumOfPointTop = new Label();
            btnDecreaseOrderTop = new Button();
            lblBottom = new Label();
            tabPageLoad = new TabPage();
            btnLoadBez = new Button();
            btnLoadBezDat = new Button();
            btnLoadDat = new Button();
            btnDefault = new Button();
            chkMatchTEGap = new CheckBox();
            tabPageCurveFitting = new TabPage();
            lblErrorThresholdTop = new Label();
            lblNumberOfParticles = new Label();
            txtNumberOfParticles = new TextBox();
            btnStopSearch = new Button();
            lblErrorNumberOfSteps = new Label();
            lblElapsedTime = new Label();
            txtErrorNumberOfSteps = new TextBox();
            btnAutoSearch = new Button();
            btnStartPSOBottom = new Button();
            btnSearchBottom = new Button();
            lblErrorCalculationDistribution = new Label();
            btnSearchTop = new Button();
            cmbErrorCalculationDistribution = new ComboBox();
            btnStartPSOTop = new Button();
            chkUpdateUI = new CheckBox();
            txtErrorThresholdTop = new TextBox();
            lblErrorThresholdBottom = new Label();
            txtErrorThresholdBottom = new TextBox();
            lblErrorThreshold = new Label();
            tabPageSave = new TabPage();
            btnSaveDXF = new Button();
            btnSaveDat = new Button();
            btnSaveBezDat = new Button();
            btnSaveBez = new Button();
            label1 = new Label();
            cmbCoordinateStyle = new ComboBox();
            lblChord = new Label();
            txtChord = new TextBox();
            tabPagePlotVisibility = new TabPage();
            tabControl1.SuspendLayout();
            tabPageGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).BeginInit();
            panel1.SuspendLayout();
            tabPageLoad.SuspendLayout();
            tabPageCurveFitting.SuspendLayout();
            tabPageSave.SuspendLayout();
            tabPagePlotVisibility.SuspendLayout();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.BackColor = SystemColors.Control;
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Margin = new Padding(3, 0, 3, 0);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(875, 660);
            formsPlot1.TabIndex = 0;
            formsPlot1.AxesChanged += formsPlot1_AxesChanged;
            formsPlot1.PlottableDragged += formsPlot1_PlottableDragged;
            formsPlot1.MouseMove += formsPlot1_MouseMove;
            // 
            // txtAirfoilParam
            // 
            txtAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtAirfoilParam.Location = new Point(875, 472);
            txtAirfoilParam.Name = "txtAirfoilParam";
            txtAirfoilParam.Size = new Size(361, 144);
            txtAirfoilParam.TabIndex = 18;
            txtAirfoilParam.Text = "";
            // 
            // lblAirfoilParam
            // 
            lblAirfoilParam.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblAirfoilParam.AutoSize = true;
            lblAirfoilParam.Location = new Point(875, 452);
            lblAirfoilParam.Margin = new Padding(3);
            lblAirfoilParam.Name = "lblAirfoilParam";
            lblAirfoilParam.Size = new Size(101, 15);
            lblAirfoilParam.TabIndex = 19;
            lblAirfoilParam.Text = "Airfoil Parameters";
            // 
            // chkShowControlTop
            // 
            chkShowControlTop.AutoSize = true;
            chkShowControlTop.Location = new Point(6, 6);
            chkShowControlTop.Name = "chkShowControlTop";
            chkShowControlTop.Size = new Size(64, 19);
            chkShowControlTop.TabIndex = 25;
            chkShowControlTop.Text = "ctrl top";
            chkShowControlTop.UseVisualStyleBackColor = true;
            chkShowControlTop.CheckedChanged += chkShowControlTop_CheckedChanged;
            // 
            // chkShowThickness
            // 
            chkShowThickness.AutoSize = true;
            chkShowThickness.Location = new Point(6, 106);
            chkShowThickness.Name = "chkShowThickness";
            chkShowThickness.Size = new Size(75, 19);
            chkShowThickness.TabIndex = 26;
            chkShowThickness.Text = "thickness";
            chkShowThickness.UseVisualStyleBackColor = true;
            chkShowThickness.CheckedChanged += chkShowThickness_CheckedChanged;
            // 
            // chkShowRadius
            // 
            chkShowRadius.AutoSize = true;
            chkShowRadius.Location = new Point(6, 293);
            chkShowRadius.Name = "chkShowRadius";
            chkShowRadius.Size = new Size(58, 19);
            chkShowRadius.TabIndex = 27;
            chkShowRadius.Text = "radius";
            chkShowRadius.UseVisualStyleBackColor = true;
            chkShowRadius.CheckedChanged += chkShowRadius_CheckedChanged;
            // 
            // chkShowCamber
            // 
            chkShowCamber.AutoSize = true;
            chkShowCamber.Location = new Point(6, 185);
            chkShowCamber.Name = "chkShowCamber";
            chkShowCamber.Size = new Size(66, 19);
            chkShowCamber.TabIndex = 29;
            chkShowCamber.Text = "camber";
            chkShowCamber.UseVisualStyleBackColor = true;
            chkShowCamber.CheckedChanged += chkShowCamber_CheckedChanged;
            // 
            // btnAxisAuto
            // 
            btnAxisAuto.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAxisAuto.AutoSize = true;
            btnAxisAuto.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnAxisAuto.Location = new Point(1049, 631);
            btnAxisAuto.Margin = new Padding(3, 0, 3, 0);
            btnAxisAuto.Name = "btnAxisAuto";
            btnAxisAuto.Size = new Size(49, 25);
            btnAxisAuto.TabIndex = 30;
            btnAxisAuto.Text = "Zoom";
            btnAxisAuto.UseVisualStyleBackColor = true;
            btnAxisAuto.Click += btnAxisAuto_Click;
            // 
            // chkShowReferenceTop
            // 
            chkShowReferenceTop.AutoSize = true;
            chkShowReferenceTop.Location = new Point(6, 343);
            chkShowReferenceTop.Name = "chkShowReferenceTop";
            chkShowReferenceTop.Size = new Size(61, 19);
            chkShowReferenceTop.TabIndex = 35;
            chkShowReferenceTop.Text = "ref top";
            chkShowReferenceTop.UseVisualStyleBackColor = true;
            chkShowReferenceTop.CheckedChanged += chkShowReferenceTop_CheckedChanged;
            // 
            // chkShowReferenceBottom
            // 
            chkShowReferenceBottom.Location = new Point(73, 343);
            chkShowReferenceBottom.Name = "chkShowReferenceBottom";
            chkShowReferenceBottom.Size = new Size(69, 19);
            chkShowReferenceBottom.TabIndex = 36;
            chkShowReferenceBottom.Text = "ref btm";
            chkShowReferenceBottom.UseVisualStyleBackColor = true;
            chkShowReferenceBottom.CheckedChanged += chkShowReferenceBottom_CheckedChanged;
            // 
            // chkShowTop
            // 
            chkShowTop.AutoSize = true;
            chkShowTop.Location = new Point(6, 56);
            chkShowTop.Name = "chkShowTop";
            chkShowTop.Size = new Size(44, 19);
            chkShowTop.TabIndex = 40;
            chkShowTop.Text = "top";
            chkShowTop.UseVisualStyleBackColor = true;
            chkShowTop.CheckedChanged += chkShowTop_CheckedChanged;
            // 
            // chkShowBottom
            // 
            chkShowBottom.AutoSize = true;
            chkShowBottom.Location = new Point(56, 56);
            chkShowBottom.Name = "chkShowBottom";
            chkShowBottom.Size = new Size(66, 19);
            chkShowBottom.TabIndex = 41;
            chkShowBottom.Text = "bottom";
            chkShowBottom.UseVisualStyleBackColor = true;
            chkShowBottom.CheckedChanged += chkShowBottom_CheckedChanged;
            // 
            // chkShowControlBottom
            // 
            chkShowControlBottom.AutoSize = true;
            chkShowControlBottom.Location = new Point(76, 6);
            chkShowControlBottom.Name = "chkShowControlBottom";
            chkShowControlBottom.Size = new Size(68, 19);
            chkShowControlBottom.TabIndex = 43;
            chkShowControlBottom.Text = "ctrl btm";
            chkShowControlBottom.UseVisualStyleBackColor = true;
            chkShowControlBottom.CheckedChanged += chkShowControlBottom_CheckedChanged;
            // 
            // txtCamberPosition
            // 
            txtCamberPosition.Location = new Point(76, 239);
            txtCamberPosition.Name = "txtCamberPosition";
            txtCamberPosition.Size = new Size(57, 23);
            txtCamberPosition.TabIndex = 44;
            txtCamberPosition.Text = "0,5";
            txtCamberPosition.TextChanged += txtCamberPosition_TextChanged;
            // 
            // txtThicknessStepSize
            // 
            txtThicknessStepSize.Location = new Point(76, 131);
            txtThicknessStepSize.Name = "txtThicknessStepSize";
            txtThicknessStepSize.Size = new Size(57, 23);
            txtThicknessStepSize.TabIndex = 45;
            txtThicknessStepSize.Text = "1000";
            txtThicknessStepSize.TextChanged += txtThicknessStepSize_TextChanged;
            // 
            // txtCamberStepSize
            // 
            txtCamberStepSize.Location = new Point(76, 210);
            txtCamberStepSize.Name = "txtCamberStepSize";
            txtCamberStepSize.Size = new Size(57, 23);
            txtCamberStepSize.TabIndex = 46;
            txtCamberStepSize.Text = "1000";
            txtCamberStepSize.TextChanged += txtCamberStepSize_TextChanged;
            // 
            // lblThicknessStepSize
            // 
            lblThicknessStepSize.AutoSize = true;
            lblThicknessStepSize.Location = new Point(6, 134);
            lblThicknessStepSize.Margin = new Padding(3);
            lblThicknessStepSize.Name = "lblThicknessStepSize";
            lblThicknessStepSize.Size = new Size(61, 15);
            lblThicknessStepSize.TabIndex = 47;
            lblThicknessStepSize.Text = "# of steps:";
            // 
            // lblCamberStepSize
            // 
            lblCamberStepSize.AutoSize = true;
            lblCamberStepSize.Location = new Point(6, 213);
            lblCamberStepSize.Margin = new Padding(3);
            lblCamberStepSize.Name = "lblCamberStepSize";
            lblCamberStepSize.Size = new Size(61, 15);
            lblCamberStepSize.TabIndex = 48;
            lblCamberStepSize.Text = "# of steps:";
            // 
            // lblCamberPosition
            // 
            lblCamberPosition.AutoSize = true;
            lblCamberPosition.Location = new Point(6, 242);
            lblCamberPosition.Margin = new Padding(3);
            lblCamberPosition.Name = "lblCamberPosition";
            lblCamberPosition.Size = new Size(53, 15);
            lblCamberPosition.TabIndex = 49;
            lblCamberPosition.Text = "position:";
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar1.Location = new Point(0, 660);
            progressBar1.Margin = new Padding(3, 2, 3, 2);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(1243, 4);
            progressBar1.Style = ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 52;
            // 
            // cmbLanguage
            // 
            cmbLanguage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cmbLanguage.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
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
            btnCheckForUpdates.AutoSize = true;
            btnCheckForUpdates.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnCheckForUpdates.Location = new Point(1104, 631);
            btnCheckForUpdates.Margin = new Padding(3, 2, 3, 2);
            btnCheckForUpdates.Name = "btnCheckForUpdates";
            btnCheckForUpdates.Size = new Size(91, 25);
            btnCheckForUpdates.TabIndex = 57;
            btnCheckForUpdates.Text = "check version";
            btnCheckForUpdates.UseVisualStyleBackColor = true;
            btnCheckForUpdates.Click += btnCheckForUpdates_Click;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPageGridView);
            tabControl1.Controls.Add(tabPageLoad);
            tabControl1.Controls.Add(tabPageCurveFitting);
            tabControl1.Controls.Add(tabPageSave);
            tabControl1.Controls.Add(tabPagePlotVisibility);
            tabControl1.Location = new Point(875, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(361, 450);
            tabControl1.TabIndex = 62;
            tabControl1.MouseMove += tabControl1_MouseMove;
            // 
            // tabPageGridView
            // 
            tabPageGridView.Controls.Add(lblTop);
            tabPageGridView.Controls.Add(dataGridViewTop);
            tabPageGridView.Controls.Add(panel2);
            tabPageGridView.Controls.Add(dataGridViewBottom);
            tabPageGridView.Controls.Add(panel1);
            tabPageGridView.Controls.Add(lblBottom);
            tabPageGridView.Location = new Point(4, 24);
            tabPageGridView.Name = "tabPageGridView";
            tabPageGridView.Size = new Size(353, 422);
            tabPageGridView.TabIndex = 3;
            tabPageGridView.Text = "grid view";
            tabPageGridView.UseVisualStyleBackColor = true;
            tabPageGridView.Click += tabPageGridView_Click;
            // 
            // lblTop
            // 
            lblTop.AutoSize = true;
            lblTop.Location = new Point(3, 3);
            lblTop.Margin = new Padding(3);
            lblTop.Name = "lblTop";
            lblTop.Size = new Size(26, 15);
            lblTop.TabIndex = 3;
            lblTop.Text = "Top";
            // 
            // dataGridViewTop
            // 
            dataGridViewTop.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTop.Location = new Point(3, 20);
            dataGridViewTop.Margin = new Padding(3, 2, 3, 2);
            dataGridViewTop.Name = "dataGridViewTop";
            dataGridViewTop.RowHeadersWidth = 47;
            dataGridViewTop.RowTemplate.Height = 28;
            dataGridViewTop.Size = new Size(266, 190);
            dataGridViewTop.TabIndex = 1;
            dataGridViewTop.CellValueChanged += dataGridView1_CellValueChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblOrderBottom);
            panel2.Controls.Add(btnIncreaseOrderBottom);
            panel2.Controls.Add(txtNumOfPointBottom);
            panel2.Controls.Add(lblNumOfPointBottom);
            panel2.Controls.Add(btnDecreaseOrderBottom);
            panel2.Location = new Point(275, 229);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(75, 190);
            panel2.TabIndex = 59;
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
            // txtNumOfPointBottom
            // 
            txtNumOfPointBottom.Location = new Point(0, 60);
            txtNumOfPointBottom.Margin = new Padding(3, 2, 3, 2);
            txtNumOfPointBottom.Name = "txtNumOfPointBottom";
            txtNumOfPointBottom.Size = new Size(76, 23);
            txtNumOfPointBottom.TabIndex = 11;
            txtNumOfPointBottom.Text = "150";
            txtNumOfPointBottom.TextChanged += txtNumOfPointsBottom_TextChanged;
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
            // dataGridViewBottom
            // 
            dataGridViewBottom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewBottom.Location = new Point(3, 229);
            dataGridViewBottom.Margin = new Padding(3, 2, 3, 2);
            dataGridViewBottom.Name = "dataGridViewBottom";
            dataGridViewBottom.RowHeadersWidth = 47;
            dataGridViewBottom.RowTemplate.Height = 28;
            dataGridViewBottom.Size = new Size(266, 190);
            dataGridViewBottom.TabIndex = 2;
            dataGridViewBottom.CellValueChanged += dataGridView2_CellValueChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(lblOrderTop);
            panel1.Controls.Add(btnIncreaseOrderTop);
            panel1.Controls.Add(txtNumOfPointsTop);
            panel1.Controls.Add(lblNumOfPointTop);
            panel1.Controls.Add(btnDecreaseOrderTop);
            panel1.Location = new Point(275, 20);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(75, 190);
            panel1.TabIndex = 58;
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
            // txtNumOfPointsTop
            // 
            txtNumOfPointsTop.Location = new Point(0, 60);
            txtNumOfPointsTop.Margin = new Padding(3, 2, 3, 2);
            txtNumOfPointsTop.Name = "txtNumOfPointsTop";
            txtNumOfPointsTop.Size = new Size(76, 23);
            txtNumOfPointsTop.TabIndex = 10;
            txtNumOfPointsTop.Text = "150";
            txtNumOfPointsTop.TextChanged += txtNumOfPointsTop_TextChanged;
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
            // lblBottom
            // 
            lblBottom.AutoSize = true;
            lblBottom.Location = new Point(3, 212);
            lblBottom.Margin = new Padding(3);
            lblBottom.Name = "lblBottom";
            lblBottom.Size = new Size(47, 15);
            lblBottom.TabIndex = 4;
            lblBottom.Text = "Bottom";
            // 
            // tabPageLoad
            // 
            tabPageLoad.Controls.Add(btnLoadBez);
            tabPageLoad.Controls.Add(btnLoadBezDat);
            tabPageLoad.Controls.Add(btnLoadDat);
            tabPageLoad.Controls.Add(btnDefault);
            tabPageLoad.Controls.Add(chkMatchTEGap);
            tabPageLoad.Location = new Point(4, 24);
            tabPageLoad.Name = "tabPageLoad";
            tabPageLoad.Padding = new Padding(3);
            tabPageLoad.Size = new Size(353, 422);
            tabPageLoad.TabIndex = 0;
            tabPageLoad.Text = "load";
            tabPageLoad.UseVisualStyleBackColor = true;
            // 
            // btnLoadBez
            // 
            btnLoadBez.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadBez.Location = new Point(6, 147);
            btnLoadBez.Name = "btnLoadBez";
            btnLoadBez.Size = new Size(75, 23);
            btnLoadBez.TabIndex = 53;
            btnLoadBez.Text = ".bez";
            btnLoadBez.UseVisualStyleBackColor = true;
            btnLoadBez.Click += btnLoadBez_Click;
            // 
            // btnLoadBezDat
            // 
            btnLoadBezDat.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadBezDat.Location = new Point(6, 176);
            btnLoadBezDat.Name = "btnLoadBezDat";
            btnLoadBezDat.Size = new Size(75, 23);
            btnLoadBezDat.TabIndex = 32;
            btnLoadBezDat.Text = ".bez.dat";
            btnLoadBezDat.UseVisualStyleBackColor = true;
            btnLoadBezDat.Click += btnLoadBezDat_Click;
            // 
            // btnLoadDat
            // 
            btnLoadDat.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnLoadDat.Location = new Point(6, 64);
            btnLoadDat.Name = "btnLoadDat";
            btnLoadDat.Size = new Size(75, 23);
            btnLoadDat.TabIndex = 31;
            btnLoadDat.Text = ".dat";
            btnLoadDat.UseVisualStyleBackColor = true;
            btnLoadDat.Click += btnLoadDat_Click;
            btnLoadDat.MouseDown += btnLoadDat_MouseDown;
            // 
            // btnDefault
            // 
            btnDefault.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnDefault.Location = new Point(6, 6);
            btnDefault.Name = "btnDefault";
            btnDefault.Size = new Size(75, 23);
            btnDefault.TabIndex = 20;
            btnDefault.Text = "default";
            btnDefault.UseVisualStyleBackColor = true;
            btnDefault.Click += btnDefault_Click;
            // 
            // chkMatchTEGap
            // 
            chkMatchTEGap.AutoSize = true;
            chkMatchTEGap.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            chkMatchTEGap.Location = new Point(6, 93);
            chkMatchTEGap.Name = "chkMatchTEGap";
            chkMatchTEGap.Size = new Size(75, 19);
            chkMatchTEGap.TabIndex = 71;
            chkMatchTEGap.Text = "match TE";
            chkMatchTEGap.UseVisualStyleBackColor = true;
            // 
            // tabPageCurveFitting
            // 
            tabPageCurveFitting.Controls.Add(lblErrorThresholdTop);
            tabPageCurveFitting.Controls.Add(lblNumberOfParticles);
            tabPageCurveFitting.Controls.Add(txtNumberOfParticles);
            tabPageCurveFitting.Controls.Add(btnStopSearch);
            tabPageCurveFitting.Controls.Add(lblErrorNumberOfSteps);
            tabPageCurveFitting.Controls.Add(lblElapsedTime);
            tabPageCurveFitting.Controls.Add(txtErrorNumberOfSteps);
            tabPageCurveFitting.Controls.Add(btnAutoSearch);
            tabPageCurveFitting.Controls.Add(btnStartPSOBottom);
            tabPageCurveFitting.Controls.Add(btnSearchBottom);
            tabPageCurveFitting.Controls.Add(lblErrorCalculationDistribution);
            tabPageCurveFitting.Controls.Add(btnSearchTop);
            tabPageCurveFitting.Controls.Add(cmbErrorCalculationDistribution);
            tabPageCurveFitting.Controls.Add(btnStartPSOTop);
            tabPageCurveFitting.Controls.Add(chkUpdateUI);
            tabPageCurveFitting.Controls.Add(txtErrorThresholdTop);
            tabPageCurveFitting.Controls.Add(lblErrorThresholdBottom);
            tabPageCurveFitting.Controls.Add(txtErrorThresholdBottom);
            tabPageCurveFitting.Controls.Add(lblErrorThreshold);
            tabPageCurveFitting.Location = new Point(4, 24);
            tabPageCurveFitting.Name = "tabPageCurveFitting";
            tabPageCurveFitting.Padding = new Padding(3);
            tabPageCurveFitting.Size = new Size(353, 422);
            tabPageCurveFitting.TabIndex = 1;
            tabPageCurveFitting.Text = "curve fitting";
            tabPageCurveFitting.UseVisualStyleBackColor = true;
            // 
            // lblErrorThresholdTop
            // 
            lblErrorThresholdTop.AutoSize = true;
            lblErrorThresholdTop.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorThresholdTop.Location = new Point(6, 88);
            lblErrorThresholdTop.Margin = new Padding(3);
            lblErrorThresholdTop.Name = "lblErrorThresholdTop";
            lblErrorThresholdTop.Size = new Size(26, 15);
            lblErrorThresholdTop.TabIndex = 77;
            lblErrorThresholdTop.Text = "Top";
            // 
            // lblNumberOfParticles
            // 
            lblNumberOfParticles.AutoSize = true;
            lblNumberOfParticles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblNumberOfParticles.Location = new Point(6, 209);
            lblNumberOfParticles.Margin = new Padding(3);
            lblNumberOfParticles.Name = "lblNumberOfParticles";
            lblNumberOfParticles.Size = new Size(75, 15);
            lblNumberOfParticles.TabIndex = 76;
            lblNumberOfParticles.Text = "# of particles";
            // 
            // txtNumberOfParticles
            // 
            txtNumberOfParticles.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtNumberOfParticles.Location = new Point(87, 201);
            txtNumberOfParticles.Name = "txtNumberOfParticles";
            txtNumberOfParticles.Size = new Size(75, 23);
            txtNumberOfParticles.TabIndex = 75;
            txtNumberOfParticles.Text = "100";
            // 
            // btnStopSearch
            // 
            btnStopSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnStopSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnStopSearch.Location = new Point(6, 393);
            btnStopSearch.Name = "btnStopSearch";
            btnStopSearch.Size = new Size(75, 23);
            btnStopSearch.TabIndex = 51;
            btnStopSearch.Text = "stop";
            btnStopSearch.UseVisualStyleBackColor = true;
            btnStopSearch.Click += btnStopSearch_Click;
            // 
            // lblErrorNumberOfSteps
            // 
            lblErrorNumberOfSteps.AutoSize = true;
            lblErrorNumberOfSteps.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorNumberOfSteps.Location = new Point(168, 117);
            lblErrorNumberOfSteps.Margin = new Padding(3);
            lblErrorNumberOfSteps.Name = "lblErrorNumberOfSteps";
            lblErrorNumberOfSteps.Size = new Size(58, 15);
            lblErrorNumberOfSteps.TabIndex = 74;
            lblErrorNumberOfSteps.Text = "# of steps";
            // 
            // lblElapsedTime
            // 
            lblElapsedTime.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblElapsedTime.AutoSize = true;
            lblElapsedTime.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblElapsedTime.Location = new Point(170, 399);
            lblElapsedTime.Margin = new Padding(3);
            lblElapsedTime.Name = "lblElapsedTime";
            lblElapsedTime.Size = new Size(49, 15);
            lblElapsedTime.TabIndex = 50;
            lblElapsedTime.Text = "00:00:00";
            // 
            // txtErrorNumberOfSteps
            // 
            txtErrorNumberOfSteps.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorNumberOfSteps.Location = new Point(249, 114);
            txtErrorNumberOfSteps.Name = "txtErrorNumberOfSteps";
            txtErrorNumberOfSteps.Size = new Size(75, 23);
            txtErrorNumberOfSteps.TabIndex = 73;
            txtErrorNumberOfSteps.Text = "1000";
            // 
            // btnAutoSearch
            // 
            btnAutoSearch.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnAutoSearch.Location = new Point(168, 6);
            btnAutoSearch.Name = "btnAutoSearch";
            btnAutoSearch.Size = new Size(75, 23);
            btnAutoSearch.TabIndex = 39;
            btnAutoSearch.Text = "auto";
            btnAutoSearch.UseVisualStyleBackColor = true;
            btnAutoSearch.Click += btnAutoSearch_Click;
            // 
            // btnStartPSOBottom
            // 
            btnStartPSOBottom.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnStartPSOBottom.Location = new Point(87, 172);
            btnStartPSOBottom.Name = "btnStartPSOBottom";
            btnStartPSOBottom.Size = new Size(75, 23);
            btnStartPSOBottom.TabIndex = 72;
            btnStartPSOBottom.Text = "PSO btm";
            btnStartPSOBottom.UseVisualStyleBackColor = true;
            btnStartPSOBottom.Click += btnStartPSOBottom_Click;
            // 
            // btnSearchBottom
            // 
            btnSearchBottom.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnSearchBottom.Location = new Point(87, 6);
            btnSearchBottom.Name = "btnSearchBottom";
            btnSearchBottom.Size = new Size(75, 23);
            btnSearchBottom.TabIndex = 38;
            btnSearchBottom.Text = "bottom";
            btnSearchBottom.UseVisualStyleBackColor = true;
            btnSearchBottom.Click += btnSearchBottom_Click;
            btnSearchBottom.MouseDown += btnSearchBottom_MouseDown;
            // 
            // lblErrorCalculationDistribution
            // 
            lblErrorCalculationDistribution.AutoSize = true;
            lblErrorCalculationDistribution.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorCalculationDistribution.Location = new Point(168, 88);
            lblErrorCalculationDistribution.Margin = new Padding(3);
            lblErrorCalculationDistribution.Name = "lblErrorCalculationDistribution";
            lblErrorCalculationDistribution.Size = new Size(68, 15);
            lblErrorCalculationDistribution.TabIndex = 70;
            lblErrorCalculationDistribution.Text = "ErrCalcDistr";
            // 
            // btnSearchTop
            // 
            btnSearchTop.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnSearchTop.Location = new Point(6, 6);
            btnSearchTop.Name = "btnSearchTop";
            btnSearchTop.Size = new Size(75, 23);
            btnSearchTop.TabIndex = 37;
            btnSearchTop.Text = "top";
            btnSearchTop.UseVisualStyleBackColor = true;
            btnSearchTop.Click += btnSearchTop_Click;
            btnSearchTop.MouseDown += btnSearchTop_MouseDown;
            // 
            // cmbErrorCalculationDistribution
            // 
            cmbErrorCalculationDistribution.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            cmbErrorCalculationDistribution.FormattingEnabled = true;
            cmbErrorCalculationDistribution.Location = new Point(249, 85);
            cmbErrorCalculationDistribution.Name = "cmbErrorCalculationDistribution";
            cmbErrorCalculationDistribution.Size = new Size(75, 23);
            cmbErrorCalculationDistribution.TabIndex = 61;
            cmbErrorCalculationDistribution.SelectedIndexChanged += cmbErrorCalculationDistribution_SelectedIndexChanged;
            // 
            // btnStartPSOTop
            // 
            btnStartPSOTop.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btnStartPSOTop.Location = new Point(6, 172);
            btnStartPSOTop.Name = "btnStartPSOTop";
            btnStartPSOTop.Size = new Size(75, 23);
            btnStartPSOTop.TabIndex = 61;
            btnStartPSOTop.Text = "PSO top";
            btnStartPSOTop.UseVisualStyleBackColor = true;
            btnStartPSOTop.Click += btnStartPSOTop_Click;
            // 
            // chkUpdateUI
            // 
            chkUpdateUI.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkUpdateUI.AutoSize = true;
            chkUpdateUI.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            chkUpdateUI.Location = new Point(87, 398);
            chkUpdateUI.Name = "chkUpdateUI";
            chkUpdateUI.Size = new Size(77, 19);
            chkUpdateUI.TabIndex = 61;
            chkUpdateUI.Text = "update UI";
            chkUpdateUI.UseVisualStyleBackColor = true;
            chkUpdateUI.CheckedChanged += chkUpdateUI_CheckedChanged;
            // 
            // txtErrorThresholdTop
            // 
            txtErrorThresholdTop.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorThresholdTop.Location = new Point(56, 85);
            txtErrorThresholdTop.Name = "txtErrorThresholdTop";
            txtErrorThresholdTop.Size = new Size(106, 23);
            txtErrorThresholdTop.TabIndex = 65;
            txtErrorThresholdTop.Text = "7,5e-5";
            txtErrorThresholdTop.TextChanged += txtErrorThresholdTop_TextChanged;
            // 
            // lblErrorThresholdBottom
            // 
            lblErrorThresholdBottom.AutoSize = true;
            lblErrorThresholdBottom.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorThresholdBottom.Location = new Point(6, 117);
            lblErrorThresholdBottom.Margin = new Padding(3);
            lblErrorThresholdBottom.Name = "lblErrorThresholdBottom";
            lblErrorThresholdBottom.Size = new Size(47, 15);
            lblErrorThresholdBottom.TabIndex = 69;
            lblErrorThresholdBottom.Text = "Bottom";
            // 
            // txtErrorThresholdBottom
            // 
            txtErrorThresholdBottom.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtErrorThresholdBottom.Location = new Point(56, 114);
            txtErrorThresholdBottom.Name = "txtErrorThresholdBottom";
            txtErrorThresholdBottom.Size = new Size(106, 23);
            txtErrorThresholdBottom.TabIndex = 66;
            txtErrorThresholdBottom.Text = "7,5e-5";
            txtErrorThresholdBottom.TextChanged += txtErrorThresholdBottom_TextChanged;
            // 
            // lblErrorThreshold
            // 
            lblErrorThreshold.AutoSize = true;
            lblErrorThreshold.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lblErrorThreshold.Location = new Point(6, 64);
            lblErrorThreshold.Margin = new Padding(3);
            lblErrorThreshold.Name = "lblErrorThreshold";
            lblErrorThreshold.Size = new Size(87, 15);
            lblErrorThreshold.TabIndex = 68;
            lblErrorThreshold.Text = "Error Threshold";
            // 
            // tabPageSave
            // 
            tabPageSave.Controls.Add(btnSaveDXF);
            tabPageSave.Controls.Add(btnSaveDat);
            tabPageSave.Controls.Add(btnSaveBezDat);
            tabPageSave.Controls.Add(btnSaveBez);
            tabPageSave.Controls.Add(label1);
            tabPageSave.Controls.Add(cmbCoordinateStyle);
            tabPageSave.Controls.Add(lblChord);
            tabPageSave.Controls.Add(txtChord);
            tabPageSave.Location = new Point(4, 24);
            tabPageSave.Name = "tabPageSave";
            tabPageSave.Padding = new Padding(3);
            tabPageSave.Size = new Size(353, 422);
            tabPageSave.TabIndex = 2;
            tabPageSave.Text = "save";
            tabPageSave.UseVisualStyleBackColor = true;
            // 
            // btnSaveDXF
            // 
            btnSaveDXF.AutoSize = true;
            btnSaveDXF.Location = new Point(6, 99);
            btnSaveDXF.Name = "btnSaveDXF";
            btnSaveDXF.Size = new Size(75, 25);
            btnSaveDXF.TabIndex = 77;
            btnSaveDXF.Text = ".dxf";
            btnSaveDXF.UseVisualStyleBackColor = true;
            btnSaveDXF.Click += btnSaveDXF_Click;
            // 
            // btnSaveDat
            // 
            btnSaveDat.AutoSize = true;
            btnSaveDat.Location = new Point(6, 6);
            btnSaveDat.Name = "btnSaveDat";
            btnSaveDat.Size = new Size(75, 25);
            btnSaveDat.TabIndex = 70;
            btnSaveDat.Text = ".dat";
            btnSaveDat.UseVisualStyleBackColor = true;
            btnSaveDat.Click += btnSaveDat_Click;
            // 
            // btnSaveBezDat
            // 
            btnSaveBezDat.AutoSize = true;
            btnSaveBezDat.Location = new Point(6, 37);
            btnSaveBezDat.Name = "btnSaveBezDat";
            btnSaveBezDat.Size = new Size(75, 25);
            btnSaveBezDat.TabIndex = 71;
            btnSaveBezDat.Text = ".bez.dat";
            btnSaveBezDat.UseVisualStyleBackColor = true;
            btnSaveBezDat.Click += btnSaveBezDat_Click;
            // 
            // btnSaveBez
            // 
            btnSaveBez.AutoSize = true;
            btnSaveBez.Location = new Point(6, 68);
            btnSaveBez.Name = "btnSaveBez";
            btnSaveBez.Size = new Size(75, 25);
            btnSaveBez.TabIndex = 72;
            btnSaveBez.Text = ".bez";
            btnSaveBez.UseVisualStyleBackColor = true;
            btnSaveBez.Click += btnSaveBez_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(87, 73);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 76;
            label1.Text = "export style";
            // 
            // cmbCoordinateStyle
            // 
            cmbCoordinateStyle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbCoordinateStyle.FormattingEnabled = true;
            cmbCoordinateStyle.Location = new Point(87, 101);
            cmbCoordinateStyle.Name = "cmbCoordinateStyle";
            cmbCoordinateStyle.Size = new Size(75, 23);
            cmbCoordinateStyle.TabIndex = 74;
            // 
            // lblChord
            // 
            lblChord.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblChord.AutoSize = true;
            lblChord.Location = new Point(87, 11);
            lblChord.Margin = new Padding(3);
            lblChord.Name = "lblChord";
            lblChord.Size = new Size(38, 15);
            lblChord.TabIndex = 75;
            lblChord.Text = "chord";
            // 
            // txtChord
            // 
            txtChord.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtChord.Location = new Point(87, 39);
            txtChord.Name = "txtChord";
            txtChord.Size = new Size(75, 23);
            txtChord.TabIndex = 73;
            txtChord.Text = "1";
            // 
            // tabPagePlotVisibility
            // 
            tabPagePlotVisibility.Controls.Add(chkShowControlTop);
            tabPagePlotVisibility.Controls.Add(chkShowControlBottom);
            tabPagePlotVisibility.Controls.Add(chkShowTop);
            tabPagePlotVisibility.Controls.Add(chkShowBottom);
            tabPagePlotVisibility.Controls.Add(chkShowReferenceBottom);
            tabPagePlotVisibility.Controls.Add(txtCamberPosition);
            tabPagePlotVisibility.Controls.Add(chkShowReferenceTop);
            tabPagePlotVisibility.Controls.Add(lblCamberPosition);
            tabPagePlotVisibility.Controls.Add(chkShowRadius);
            tabPagePlotVisibility.Controls.Add(chkShowThickness);
            tabPagePlotVisibility.Controls.Add(txtCamberStepSize);
            tabPagePlotVisibility.Controls.Add(lblCamberStepSize);
            tabPagePlotVisibility.Controls.Add(lblThicknessStepSize);
            tabPagePlotVisibility.Controls.Add(txtThicknessStepSize);
            tabPagePlotVisibility.Controls.Add(chkShowCamber);
            tabPagePlotVisibility.Location = new Point(4, 24);
            tabPagePlotVisibility.Name = "tabPagePlotVisibility";
            tabPagePlotVisibility.Padding = new Padding(3);
            tabPagePlotVisibility.Size = new Size(353, 422);
            tabPagePlotVisibility.TabIndex = 4;
            tabPagePlotVisibility.Text = "plot visibility";
            tabPagePlotVisibility.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1243, 664);
            Controls.Add(btnAxisAuto);
            Controls.Add(tabControl1);
            Controls.Add(btnCheckForUpdates);
            Controls.Add(cmbLanguage);
            Controls.Add(progressBar1);
            Controls.Add(lblAirfoilParam);
            Controls.Add(txtAirfoilParam);
            Controls.Add(formsPlot1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BezierAirfoilDesigner";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            MouseClick += Form1_MouseClick;
            MouseMove += Form1_MouseMove;
            Resize += Form1_Resize;
            tabControl1.ResumeLayout(false);
            tabPageGridView.ResumeLayout(false);
            tabPageGridView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTop).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewBottom).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPageLoad.ResumeLayout(false);
            tabPageLoad.PerformLayout();
            tabPageCurveFitting.ResumeLayout(false);
            tabPageCurveFitting.PerformLayout();
            tabPageSave.ResumeLayout(false);
            tabPageSave.PerformLayout();
            tabPagePlotVisibility.ResumeLayout(false);
            tabPagePlotVisibility.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private RichTextBox txtAirfoilParam;
        private Label lblAirfoilParam;
        private CheckBox chkShowControlTop;
        private CheckBox chkShowThickness;
        private CheckBox chkShowRadius;
        private CheckBox chkShowCamber;
        private Button btnAxisAuto;
        private CheckBox chkShowReferenceTop;
        private CheckBox chkShowReferenceBottom;
        private CheckBox chkShowTop;
        private CheckBox chkShowBottom;
        private CheckBox chkShowControlBottom;
        private TextBox txtCamberPosition;
        private TextBox txtThicknessStepSize;
        private TextBox txtCamberStepSize;
        private Label lblThicknessStepSize;
        private Label lblCamberStepSize;
        private Label lblCamberPosition;
        private ProgressBar progressBar1;
        private ComboBox cmbLanguage;
        private Button btnCheckForUpdates;
        private TabControl tabControl1;
        private TabPage tabPageGridView;
        private Label lblTop;
        private DataGridView dataGridViewTop;
        private Panel panel2;
        private Label lblOrderBottom;
        private Button btnIncreaseOrderBottom;
        private TextBox txtNumOfPointBottom;
        private Label lblNumOfPointBottom;
        private Button btnDecreaseOrderBottom;
        private DataGridView dataGridViewBottom;
        private Panel panel1;
        private Label lblOrderTop;
        private Button btnIncreaseOrderTop;
        private TextBox txtNumOfPointsTop;
        private Label lblNumOfPointTop;
        private Button btnDecreaseOrderTop;
        private Label lblBottom;
        private TabPage tabPageLoad;
        private Button btnLoadBez;
        private Button btnLoadBezDat;
        private Button btnLoadDat;
        private Button btnDefault;
        private CheckBox chkMatchTEGap;
        private TabPage tabPageCurveFitting;
        private Label lblNumberOfParticles;
        private TextBox txtNumberOfParticles;
        private Button btnStopSearch;
        private Label lblErrorNumberOfSteps;
        private Label lblElapsedTime;
        private TextBox txtErrorNumberOfSteps;
        private Button btnAutoSearch;
        private Button btnStartPSOBottom;
        private Button btnSearchBottom;
        private Label lblErrorCalculationDistribution;
        private Button btnSearchTop;
        private ComboBox cmbErrorCalculationDistribution;
        private Button btnStartPSOTop;
        private CheckBox chkUpdateUI;
        private TextBox txtErrorThresholdTop;
        private Label lblErrorThresholdBottom;
        private TextBox txtErrorThresholdBottom;
        private Label lblErrorThreshold;
        private TabPage tabPageSave;
        private TabPage tabPagePlotVisibility;
        private Label lblErrorThresholdTop;
        private Button btnSaveDXF;
        private Button btnSaveDat;
        private Button btnSaveBezDat;
        private Button btnSaveBez;
        private Label label1;
        private ComboBox cmbCoordinateStyle;
        private Label lblChord;
        private TextBox txtChord;
    }
}