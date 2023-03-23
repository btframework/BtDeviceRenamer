namespace BtDeviceRenamer
{
    partial class fmMain
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
            this.btEnumDevices = new System.Windows.Forms.Button();
            this.lvDevices = new System.Windows.Forms.ListView();
            this.chMac = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.laNewName = new System.Windows.Forms.Label();
            this.edNewName = new System.Windows.Forms.TextBox();
            this.btSetName = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btEnumDevices
            // 
            this.btEnumDevices.Location = new System.Drawing.Point(12, 12);
            this.btEnumDevices.Name = "btEnumDevices";
            this.btEnumDevices.Size = new System.Drawing.Size(91, 23);
            this.btEnumDevices.TabIndex = 0;
            this.btEnumDevices.Text = "Enum devices";
            this.btEnumDevices.UseVisualStyleBackColor = true;
            this.btEnumDevices.Click += new System.EventHandler(this.btEnumDevices_Click);
            // 
            // lvDevices
            // 
            this.lvDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chMac,
            this.chName});
            this.lvDevices.FullRowSelect = true;
            this.lvDevices.HideSelection = false;
            this.lvDevices.Location = new System.Drawing.Point(12, 41);
            this.lvDevices.MultiSelect = false;
            this.lvDevices.Name = "lvDevices";
            this.lvDevices.ShowGroups = false;
            this.lvDevices.Size = new System.Drawing.Size(331, 97);
            this.lvDevices.TabIndex = 1;
            this.lvDevices.UseCompatibleStateImageBehavior = false;
            this.lvDevices.View = System.Windows.Forms.View.Details;
            // 
            // chMac
            // 
            this.chMac.Text = "Device address";
            this.chMac.Width = 130;
            // 
            // chName
            // 
            this.chName.Text = "Device name";
            this.chName.Width = 155;
            // 
            // laNewName
            // 
            this.laNewName.AutoSize = true;
            this.laNewName.Location = new System.Drawing.Point(12, 156);
            this.laNewName.Name = "laNewName";
            this.laNewName.Size = new System.Drawing.Size(61, 13);
            this.laNewName.TabIndex = 2;
            this.laNewName.Text = "New name:";
            // 
            // edNewName
            // 
            this.edNewName.Location = new System.Drawing.Point(79, 153);
            this.edNewName.Name = "edNewName";
            this.edNewName.Size = new System.Drawing.Size(183, 20);
            this.edNewName.TabIndex = 3;
            // 
            // btSetName
            // 
            this.btSetName.Location = new System.Drawing.Point(268, 151);
            this.btSetName.Name = "btSetName";
            this.btSetName.Size = new System.Drawing.Size(75, 23);
            this.btSetName.TabIndex = 4;
            this.btSetName.Text = "Set name";
            this.btSetName.UseVisualStyleBackColor = true;
            this.btSetName.Click += new System.EventHandler(this.btSetName_Click);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 187);
            this.Controls.Add(this.btSetName);
            this.Controls.Add(this.edNewName);
            this.Controls.Add(this.laNewName);
            this.Controls.Add(this.lvDevices);
            this.Controls.Add(this.btEnumDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bluetooth Device Renamer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmMain_FormClosed);
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btEnumDevices;
        private System.Windows.Forms.ListView lvDevices;
        private System.Windows.Forms.ColumnHeader chMac;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Label laNewName;
        private System.Windows.Forms.TextBox edNewName;
        private System.Windows.Forms.Button btSetName;
    }
}

