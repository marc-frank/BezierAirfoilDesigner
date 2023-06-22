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
            dataGridView1 = new DataGridView();
            topBindingSource = new BindingSource(components);
            dataGridView2 = new DataGridView();
            bottomBindingSource = new BindingSource(components);
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            richTextBox1 = new RichTextBox();
            label3 = new Label();
            btnIncreaseOrderTop = new Button();
            btnIncreaseOrderBottom = new Button();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label4 = new Label();
            label5 = new Label();
            lblOrderTop = new Label();
            lblOrderBottom = new Label();
            btnDecreaseOrderTop = new Button();
            btnDecreaseOrderBottom = new Button();
            richTextBox2 = new RichTextBox();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)topBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bottomBindingSource).BeginInit();
            SuspendLayout();
            // 
            // formsPlot1
            // 
            formsPlot1.Location = new Point(0, 0);
            formsPlot1.Margin = new Padding(5, 4, 5, 4);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(1188, 724);
            formsPlot1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(1184, 31);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 47;
            dataGridView1.RowTemplate.Height = 28;
            dataGridView1.Size = new Size(298, 169);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(1184, 225);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 47;
            dataGridView2.RowTemplate.Height = 28;
            dataGridView2.Size = new Size(298, 169);
            dataGridView2.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1193, 9);
            label1.Name = "label1";
            label1.Size = new Size(31, 19);
            label1.TabIndex = 3;
            label1.Text = "Top";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1184, 203);
            label2.Name = "label2";
            label2.Size = new Size(55, 19);
            label2.TabIndex = 4;
            label2.Text = "Bottom";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(1681, 698);
            button1.Name = "button1";
            button1.Size = new Size(86, 26);
            button1.TabIndex = 5;
            button1.Text = "set";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            richTextBox1.Location = new Point(1580, 31);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(187, 661);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(1580, 9);
            label3.Name = "label3";
            label3.Size = new Size(53, 19);
            label3.TabIndex = 7;
            label3.Text = ".dat file";
            // 
            // btnIncreaseOrderTop
            // 
            btnIncreaseOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIncreaseOrderTop.Location = new Point(1488, 53);
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
            btnIncreaseOrderBottom.Location = new Point(1488, 247);
            btnIncreaseOrderBottom.Name = "btnIncreaseOrderBottom";
            btnIncreaseOrderBottom.Size = new Size(26, 26);
            btnIncreaseOrderBottom.TabIndex = 9;
            btnIncreaseOrderBottom.Text = "+";
            btnIncreaseOrderBottom.UseVisualStyleBackColor = true;
            btnIncreaseOrderBottom.Click += btnIncreaseOrderBottom_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox1.Location = new Point(1488, 104);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(86, 26);
            textBox1.TabIndex = 10;
            textBox1.Text = "100";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBox2.Location = new Point(1488, 298);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(86, 26);
            textBox2.TabIndex = 11;
            textBox2.Text = "100";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(1488, 82);
            label4.Name = "label4";
            label4.Size = new Size(75, 19);
            label4.TabIndex = 12;
            label4.Text = "# of points";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(1488, 276);
            label5.Name = "label5";
            label5.Size = new Size(75, 19);
            label5.TabIndex = 13;
            label5.Text = "# of points";
            // 
            // lblOrderTop
            // 
            lblOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrderTop.AutoSize = true;
            lblOrderTop.Location = new Point(1488, 31);
            lblOrderTop.Name = "lblOrderTop";
            lblOrderTop.Size = new Size(45, 19);
            lblOrderTop.TabIndex = 14;
            lblOrderTop.Text = "order:";
            // 
            // lblOrderBottom
            // 
            lblOrderBottom.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOrderBottom.AutoSize = true;
            lblOrderBottom.Location = new Point(1488, 225);
            lblOrderBottom.Name = "lblOrderBottom";
            lblOrderBottom.Size = new Size(45, 19);
            lblOrderBottom.TabIndex = 15;
            lblOrderBottom.Text = "order:";
            // 
            // btnDecreaseOrderTop
            // 
            btnDecreaseOrderTop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDecreaseOrderTop.Location = new Point(1523, 53);
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
            btnDecreaseOrderBottom.Location = new Point(1523, 247);
            btnDecreaseOrderBottom.Name = "btnDecreaseOrderBottom";
            btnDecreaseOrderBottom.Size = new Size(26, 26);
            btnDecreaseOrderBottom.TabIndex = 17;
            btnDecreaseOrderBottom.Text = "-";
            btnDecreaseOrderBottom.UseVisualStyleBackColor = true;
            btnDecreaseOrderBottom.Click += btnDecreaseOrderBottom_Click;
            // 
            // richTextBox2
            // 
            richTextBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            richTextBox2.Location = new Point(1184, 419);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(390, 273);
            richTextBox2.TabIndex = 18;
            richTextBox2.Text = "";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(1184, 397);
            label6.Name = "label6";
            label6.Size = new Size(117, 19);
            label6.TabIndex = 19;
            label6.Text = "Airfoil Parameters";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1781, 740);
            Controls.Add(label6);
            Controls.Add(richTextBox2);
            Controls.Add(btnDecreaseOrderBottom);
            Controls.Add(btnDecreaseOrderTop);
            Controls.Add(lblOrderBottom);
            Controls.Add(lblOrderTop);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(btnIncreaseOrderBottom);
            Controls.Add(btnIncreaseOrderTop);
            Controls.Add(label3);
            Controls.Add(richTextBox1);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView2);
            Controls.Add(dataGridView1);
            Controls.Add(formsPlot1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "BezierAirfoilDesigner";
            Load += Form1_Load;
            Resize += Form1_Resize;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)topBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ((System.ComponentModel.ISupportInitialize)bottomBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ScottPlot.FormsPlot formsPlot1;
        private DataGridView dataGridView1;
        private BindingSource topBindingSource;
        private DataGridView dataGridView2;
        private BindingSource bottomBindingSource;
        private Label label1;
        private Label label2;
        private Button button1;
        private RichTextBox richTextBox1;
        private Label label3;
        private Button btnIncreaseOrderTop;
        private Button btnIncreaseOrderBottom;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label4;
        private Label label5;
        private Label lblOrderTop;
        private Label lblOrderBottom;
        private Button btnDecreaseOrderTop;
        private Button btnDecreaseOrderBottom;
        private RichTextBox richTextBox2;
        private Label label6;
    }
}