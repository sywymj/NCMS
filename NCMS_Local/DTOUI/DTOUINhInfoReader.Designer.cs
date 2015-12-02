namespace NCMS_Local.DTOUI
{
    partial class DTOUINhInfoReader
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButtonKh = new System.Windows.Forms.RadioButton();
            this.radioButtonNhh = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonQuery = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(9, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(312, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // radioButtonKh
            // 
            this.radioButtonKh.AutoSize = true;
            this.radioButtonKh.Checked = true;
            this.radioButtonKh.Location = new System.Drawing.Point(9, 28);
            this.radioButtonKh.Name = "radioButtonKh";
            this.radioButtonKh.Size = new System.Drawing.Size(47, 16);
            this.radioButtonKh.TabIndex = 1;
            this.radioButtonKh.TabStop = true;
            this.radioButtonKh.Text = "卡号";
            this.radioButtonKh.UseVisualStyleBackColor = true;
            // 
            // radioButtonNhh
            // 
            this.radioButtonNhh.AutoSize = true;
            this.radioButtonNhh.Location = new System.Drawing.Point(58, 28);
            this.radioButtonNhh.Name = "radioButtonNhh";
            this.radioButtonNhh.Size = new System.Drawing.Size(71, 16);
            this.radioButtonNhh.TabIndex = 2;
            this.radioButtonNhh.Text = "农合证号";
            this.radioButtonNhh.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(136, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(117, 21);
            this.textBox1.TabIndex = 3;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Location = new System.Drawing.Point(260, 25);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(61, 23);
            this.buttonQuery.TabIndex = 4;
            this.buttonQuery.Text = "查询";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.buttonQuery);
            this.panel1.Controls.Add(this.radioButtonKh);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.radioButtonNhh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 56);
            this.panel1.TabIndex = 5;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 56);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(336, 330);
            this.propertyGrid1.TabIndex = 6;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // DTOUINhInfoReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 386);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DTOUINhInfoReader";
            this.Text = "农合患者信息查询";
            this.Load += new System.EventHandler(this.DTOUINhInfoReader_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButtonKh;
        private System.Windows.Forms.RadioButton radioButtonNhh;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
    }
}