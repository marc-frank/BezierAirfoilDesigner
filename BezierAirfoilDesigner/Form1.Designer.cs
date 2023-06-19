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
            formsPlot1.Size = new Size(1260, 857);
            formsPlot1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(1268, 31);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 47;
            dataGridView1.RowTemplate.Height = 28;
            dataGridView1.Size = new Size(279, 385);
            dataGridView1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(1268, 441);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.RowHeadersWidth = 47;
            dataGridView2.RowTemplate.Height = 28;
            dataGridView2.Size = new Size(279, 385);
            dataGridView2.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1268, 9);
            label1.Name = "label1";
            label1.Size = new Size(31, 19);
            label1.TabIndex = 3;
            label1.Text = "Top";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(1268, 419);
            label2.Name = "label2";
            label2.Size = new Size(55, 19);
            label2.TabIndex = 4;
            label2.Text = "Bottom";
            // 
            // button1
            // 
            button1.Location = new Point(1674, 832);
            button1.Name = "button1";
            button1.Size = new Size(86, 26);
            button1.TabIndex = 5;
            button1.Text = "set";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(1553, 31);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(207, 795);
            richTextBox1.TabIndex = 6;
            richTextBox1.Text = "";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(1553, 9);
            label3.Name = "label3";
            label3.Size = new Size(53, 19);
            label3.TabIndex = 7;
            label3.Text = ".dat file";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1772, 870);
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
    }
}