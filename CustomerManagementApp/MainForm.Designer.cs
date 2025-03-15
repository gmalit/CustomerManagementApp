
using System;

namespace CustomerManagementApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView customerGridView;
        private System.Windows.Forms.Button btnUppercaseLastNames;
        private System.Windows.Forms.Button btnCommitChanges;
        private System.Windows.Forms.Button btnExportJson;
        private System.Windows.Forms.Button btnSearchByAge;  
        

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
            this.customerGridView = new System.Windows.Forms.DataGridView();
            this.btnUppercaseLastNames = new System.Windows.Forms.Button();
            this.btnCommitChanges = new System.Windows.Forms.Button();
            this.btnExportJson = new System.Windows.Forms.Button();
            this.btnSearchByAge = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.customerGridView)).BeginInit();
            this.SuspendLayout();

            // DataGridView
            this.customerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customerGridView.Location = new System.Drawing.Point(12, 12);
            this.customerGridView.Name = "customerGridView";
            this.customerGridView.Size = new System.Drawing.Size(600, 250);
            this.customerGridView.TabIndex = 0;

            // Uppercase Last Names Button
            this.btnUppercaseLastNames.Location = new System.Drawing.Point(12, 270);
            this.btnUppercaseLastNames.Name = "btnUppercaseLastNames";
            this.btnUppercaseLastNames.Size = new System.Drawing.Size(140, 30);
            this.btnUppercaseLastNames.Text = "Uppercase Last Names";
            this.btnUppercaseLastNames.UseVisualStyleBackColor = true;
            this.btnUppercaseLastNames.Click += new System.EventHandler(this.btnUppercaseLastNames_Click);

            // Commit Changes Button
            this.btnCommitChanges.Location = new System.Drawing.Point(160, 270);
            this.btnCommitChanges.Name = "btnCommitChanges";
            this.btnCommitChanges.Size = new System.Drawing.Size(140, 30);
            this.btnCommitChanges.Text = "Commit Changes";
            this.btnCommitChanges.UseVisualStyleBackColor = true;
            this.btnCommitChanges.Click += new System.EventHandler(this.btnCommitChanges_Click);

            // Export JSON Button
            this.btnExportJson.Location = new System.Drawing.Point(310, 270);
            this.btnExportJson.Name = "btnExportJson";
            this.btnExportJson.Size = new System.Drawing.Size(140, 30);
            this.btnExportJson.Text = "Export JSON";
            this.btnExportJson.UseVisualStyleBackColor = true;
            this.btnExportJson.Click += new System.EventHandler(this.btnExportJson_Click);

            // Search by Age Button
            this.btnSearchByAge.Location = new System.Drawing.Point(460, 270);
            this.btnSearchByAge.Name = "btnSearchByAge";
            this.btnSearchByAge.Size = new System.Drawing.Size(140, 30);
            this.btnSearchByAge.Text = "Search by Age";
            this.btnSearchByAge.UseVisualStyleBackColor = true;
            this.btnSearchByAge.Click += new System.EventHandler(this.btnSearchByAge_Click);

            // Form Settings
            this.ClientSize = new System.Drawing.Size(624, 321);
            this.Controls.Add(this.customerGridView);
            this.Controls.Add(this.btnUppercaseLastNames);
            this.Controls.Add(this.btnCommitChanges);
            this.Controls.Add(this.btnExportJson);
            this.Controls.Add(this.btnSearchByAge);
            this.Name = "MainForm";
            this.Text = "Customer Management App";
            ((System.ComponentModel.ISupportInitialize)(this.customerGridView)).EndInit();
            this.ResumeLayout(false);
        }

       

        #endregion
    }
}

