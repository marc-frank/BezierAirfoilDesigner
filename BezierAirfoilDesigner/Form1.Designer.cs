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
            dataGridView1.Location = new Point(1196, 31);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 47;
            dataGridView1.RowTemplate.Height = 28;
            dataGridView1.Size = new Size(279, 318);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(1196, 374);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 47;
            dataGridView2.RowTemplate.Height = 28;
            dataGridView2.Size = new Size(279, 318);
            dataGridView2.TabIndex = 2;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1196, 9);
            label1.Name = "label1";
            label1.Size = new Size(31, 19);
            label1.TabIndex = 3;
            label1.Text = "Top";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1196, 352);
            label2.Name = "label2";
            label2.Size = new Size(55, 19);
            label2.TabIndex = 4;
            label2.Text = "Bottom";
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button1.Location = new Point(1389, 702);
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
            richTextBox1.Location = new Point(1573, 31);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(187, 661);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(1573, 9);
            label3.Name = "label3";
            label3.Size = new Size(53, 19);
            label3.TabIndex = 7;
            label3.Text = ".dat file";
            // 
            // btnIncreaseOrderTop
            // 
            btnIncreaseOrderTop.Location = new Point(1481, 31);
            btnIncreaseOrderTop.Name = "btnIncreaseOrderTop";
            btnIncreaseOrderTop.Size = new Size(86, 52);
            btnIncreaseOrderTop.TabIndex = 8;
            btnIncreaseOrderTop.Text = "increase order";
            btnIncreaseOrderTop.UseVisualStyleBackColor = true;
            btnIncreaseOrderTop.Click += btnIncreaseOrderTop_Click;
            // 
            // btnIncreaseOrderBottom
            // 
            btnIncreaseOrderBottom.Location = new Point(1481, 374);
            btnIncreaseOrderBottom.Name = "btnIncreaseOrderBottom";
            btnIncreaseOrderBottom.Size = new Size(86, 52);
            btnIncreaseOrderBottom.TabIndex = 9;
            btnIncreaseOrderBottom.Text = "increase oder";
            btnIncreaseOrderBottom.UseVisualStyleBackColor = true;
            btnIncreaseOrderBottom.Click += btnIncreaseOrderBottom_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(1481, 127);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(86, 26);
            textBox1.TabIndex = 10;
            textBox1.Text = "100";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(1481, 470);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(86, 26);
            textBox2.TabIndex = 11;
            textBox2.Text = "100";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(1481, 105);
            label4.Name = "label4";
            label4.Size = new Size(75, 19);
            label4.TabIndex = 12;
            label4.Text = "# of points";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(1481, 448);
            label5.Name = "label5";
            label5.Size = new Size(75, 19);
            label5.TabIndex = 13;
            label5.Text = "# of points";
            // 
            // lblOrderTop
            // 
            lblOrderTop.AutoSize = true;
            lblOrderTop.Location = new Point(1481, 86);
            lblOrderTop.Name = "lblOrderTop";
            lblOrderTop.Size = new Size(57, 19);
            lblOrderTop.TabIndex = 14;
            lblOrderTop.Text = "order: 4";
            // 
            // lblOrderBottom
            // 
            lblOrderBottom.AutoSize = true;
            lblOrderBottom.Location = new Point(1481, 429);
            lblOrderBottom.Name = "lblOrderBottom";
            lblOrderBottom.Size = new Size(57, 19);
            lblOrderBottom.TabIndex = 15;
            lblOrderBottom.Text = "order: 4";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1772, 740);
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
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
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
    }
}