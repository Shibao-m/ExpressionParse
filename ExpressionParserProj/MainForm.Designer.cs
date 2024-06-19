namespace ExpressionParserProj
{
    partial class MainForm
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            txtExpression = new TextBox();
            txtRPN = new TextBox();
            txtValue = new TextBox();
            btnRun = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 47);
            label1.Name = "label1";
            label1.Size = new Size(19, 15);
            label1.TabIndex = 0;
            label1.Text = "式";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(67, 76);
            label2.Name = "label2";
            label2.Size = new Size(86, 15);
            label2.TabIndex = 1;
            label2.Text = "逆ポーランド記法";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(67, 105);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 2;
            label3.Text = "評価値";
            // 
            // txtExpression
            // 
            txtExpression.Location = new Point(173, 44);
            txtExpression.Name = "txtExpression";
            txtExpression.Size = new Size(339, 23);
            txtExpression.TabIndex = 3;
            // 
            // txtRPN
            // 
            txtRPN.Location = new Point(173, 73);
            txtRPN.Name = "txtRPN";
            txtRPN.Size = new Size(339, 23);
            txtRPN.TabIndex = 4;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(173, 102);
            txtValue.Name = "txtValue";
            txtValue.Size = new Size(339, 23);
            txtValue.TabIndex = 5;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(543, 44);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 81);
            btnRun.TabIndex = 6;
            btnRun.Text = "実行";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnRun);
            Controls.Add(txtValue);
            Controls.Add(txtRPN);
            Controls.Add(txtExpression);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "MainForm";
            Text = "構文解析器";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox txtExpression;
        private TextBox txtRPN;
        private TextBox txtValue;
        private Button btnRun;
    }
}
