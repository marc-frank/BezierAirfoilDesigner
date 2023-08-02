namespace BezierAirfoilDesigner
{
    partial class frmSave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSave));
            lblChord = new Label();
            txtChord = new TextBox();
            cmbCoordinateStyle = new ComboBox();
            label1 = new Label();
            btnSaveDat = new Button();
            btnSaveBezDat = new Button();
            btnSaveBez = new Button();
            SuspendLayout();
            // 
            // lblChord
            // 
            lblChord.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblChord.AutoSize = true;
            lblChord.Location = new Point(110, 9);
            lblChord.Name = "lblChord";
            lblChord.Size = new Size(38, 15);
            lblChord.TabIndex = 66;
            lblChord.Text = "chord";
            // 
            // txtChord
            // 
            txtChord.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtChord.Location = new Point(110, 27);
            txtChord.Name = "txtChord";
            txtChord.Size = new Size(75, 23);
            txtChord.TabIndex = 4;
            txtChord.Text = "1";
            // 
            // cmbCoordinateStyle
            // 
            cmbCoordinateStyle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cmbCoordinateStyle.FormattingEnabled = true;
            cmbCoordinateStyle.Location = new Point(110, 71);
            cmbCoordinateStyle.Name = "cmbCoordinateStyle";
            cmbCoordinateStyle.Size = new Size(75, 23);
            cmbCoordinateStyle.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(110, 53);
            label1.Name = "label1";
            label1.Size = new Size(68, 15);
            label1.TabIndex = 68;
            label1.Text = "export style";
            // 
            // btnSaveDat
            // 
            btnSaveDat.AutoSize = true;
            btnSaveDat.Location = new Point(12, 11);
            btnSaveDat.Margin = new Padding(3, 2, 3, 2);
            btnSaveDat.Name = "btnSaveDat";
            btnSaveDat.Size = new Size(75, 25);
            btnSaveDat.TabIndex = 1;
            btnSaveDat.Text = ".dat";
            btnSaveDat.UseVisualStyleBackColor = true;
            btnSaveDat.Click += btnSaveDat_Click;
            // 
            // btnSaveBezDat
            // 
            btnSaveBezDat.AutoSize = true;
            btnSaveBezDat.Location = new Point(12, 40);
            btnSaveBezDat.Margin = new Padding(3, 2, 3, 2);
            btnSaveBezDat.Name = "btnSaveBezDat";
            btnSaveBezDat.Size = new Size(75, 25);
            btnSaveBezDat.TabIndex = 2;
            btnSaveBezDat.Text = ".bez.dat";
            btnSaveBezDat.UseVisualStyleBackColor = true;
            btnSaveBezDat.Click += btnSaveBezDat_Click;
            // 
            // btnSaveBez
            // 
            btnSaveBez.AutoSize = true;
            btnSaveBez.Location = new Point(12, 69);
            btnSaveBez.Margin = new Padding(3, 2, 3, 2);
            btnSaveBez.Name = "btnSaveBez";
            btnSaveBez.Size = new Size(75, 25);
            btnSaveBez.TabIndex = 3;
            btnSaveBez.Text = ".bez";
            btnSaveBez.UseVisualStyleBackColor = true;
            btnSaveBez.Click += btnSaveBez_Click;
            // 
            // frmSave
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(197, 106);
            Controls.Add(btnSaveDat);
            Controls.Add(btnSaveBezDat);
            Controls.Add(btnSaveBez);
            Controls.Add(label1);
            Controls.Add(cmbCoordinateStyle);
            Controls.Add(lblChord);
            Controls.Add(txtChord);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmSave";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Save";
            Load += frmSave_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblChord;
        private TextBox txtChord;
        private ComboBox cmbCoordinateStyle;
        private Label label1;
        private Button btnSaveDat;
        private Button btnSaveBezDat;
        private Button btnSaveBez;
    }
}