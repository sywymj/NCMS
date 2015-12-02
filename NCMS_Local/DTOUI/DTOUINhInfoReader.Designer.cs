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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DTOUINhInfoReader));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButtonKh = new System.Windows.Forms.RadioButton();
            this.radioButtonNhh = new System.Windows.Forms.RadioButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStripButtonQuery = new System.Windows.Forms.ToolStripButton();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStripButtonOK = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCancel = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(7, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(137, 20);
            this.comboBox1.TabIndex = 0;
            // 
            // radioButtonKh
            // 
            this.radioButtonKh.AutoSize = true;
            this.radioButtonKh.Checked = true;
            this.radioButtonKh.Location = new System.Drawing.Point(155, 5);
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
            this.radioButtonNhh.Location = new System.Drawing.Point(208, 5);
            this.radioButtonNhh.Name = "radioButtonNhh";
            this.radioButtonNhh.Size = new System.Drawing.Size(71, 16);
            this.radioButtonNhh.TabIndex = 2;
            this.radioButtonNhh.Text = "农合证号";
            this.radioButtonNhh.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(279, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(95, 21);
            this.textBox1.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonQuery,
            this.toolStripButtonOK,
            this.toolStripButtonCancel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(382, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.radioButtonKh);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.radioButtonNhh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(382, 30);
            this.panel1.TabIndex = 8;
            // 
            // toolStripButtonQuery
            // 
            this.toolStripButtonQuery.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonQuery.Image")));
            this.toolStripButtonQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonQuery.Name = "toolStripButtonQuery";
            this.toolStripButtonQuery.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonQuery.Text = "查询";
            this.toolStripButtonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 55);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(382, 331);
            this.propertyGrid1.TabIndex = 9;
            // 
            // toolStripButtonOK
            // 
            this.toolStripButtonOK.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOK.Image")));
            this.toolStripButtonOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOK.Name = "toolStripButtonOK";
            this.toolStripButtonOK.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonOK.Text = "确定";
            this.toolStripButtonOK.Click += new System.EventHandler(this.toolStripButtonOK_Click);
            // 
            // toolStripButtonCancel
            // 
            this.toolStripButtonCancel.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCancel.Image")));
            this.toolStripButtonCancel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCancel.Name = "toolStripButtonCancel";
            this.toolStripButtonCancel.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonCancel.Text = "取消";
            this.toolStripButtonCancel.Click += new System.EventHandler(this.toolStripButtonCancel_Click);
            // 
            // DTOUINhInfoReader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 386);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DTOUINhInfoReader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "农合患者信息查询";
            this.Load += new System.EventHandler(this.DTOUINhInfoReader_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButtonKh;
        private System.Windows.Forms.RadioButton radioButtonNhh;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonQuery;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripButton toolStripButtonOK;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancel;
    }
}