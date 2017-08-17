﻿namespace VisioPowerTools
{
    partial class FormSelectionTool : System.Windows.Forms.Form
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
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonSelectNone = new System.Windows.Forms.Button();
            this.buttonInvertSelection = new System.Windows.Forms.Button();
            this.buttonUnselectConnectors = new System.Windows.Forms.Button();
            this.buttonSelectConnectors = new System.Windows.Forms.Button();
            this.labelConnectors = new System.Windows.Forms.Label();
            this.labelSelect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectAll.Location = new System.Drawing.Point(11, 30);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(50, 23);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Text = "All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonSelectNone
            // 
            this.buttonSelectNone.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonSelectNone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectNone.Location = new System.Drawing.Point(67, 30);
            this.buttonSelectNone.Name = "buttonSelectNone";
            this.buttonSelectNone.Size = new System.Drawing.Size(50, 23);
            this.buttonSelectNone.TabIndex = 1;
            this.buttonSelectNone.Text = "None";
            this.buttonSelectNone.UseVisualStyleBackColor = true;
            this.buttonSelectNone.Click += new System.EventHandler(this.buttonSelectNone_Click);
            // 
            // buttonInvertSelection
            // 
            this.buttonInvertSelection.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonInvertSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonInvertSelection.Location = new System.Drawing.Point(123, 30);
            this.buttonInvertSelection.Name = "buttonInvertSelection";
            this.buttonInvertSelection.Size = new System.Drawing.Size(50, 23);
            this.buttonInvertSelection.TabIndex = 2;
            this.buttonInvertSelection.Text = "Invert";
            this.buttonInvertSelection.UseVisualStyleBackColor = true;
            this.buttonInvertSelection.Click += new System.EventHandler(this.buttonInvertSelection_Click);
            // 
            // buttonUnselectConnectors
            // 
            this.buttonUnselectConnectors.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonUnselectConnectors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUnselectConnectors.Location = new System.Drawing.Point(77, 96);
            this.buttonUnselectConnectors.Name = "buttonUnselectConnectors";
            this.buttonUnselectConnectors.Size = new System.Drawing.Size(60, 23);
            this.buttonUnselectConnectors.TabIndex = 4;
            this.buttonUnselectConnectors.Text = "Unselect";
            this.buttonUnselectConnectors.UseVisualStyleBackColor = true;
            // 
            // buttonSelectConnectors
            // 
            this.buttonSelectConnectors.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.buttonSelectConnectors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectConnectors.Location = new System.Drawing.Point(11, 96);
            this.buttonSelectConnectors.Name = "buttonSelectConnectors";
            this.buttonSelectConnectors.Size = new System.Drawing.Size(60, 23);
            this.buttonSelectConnectors.TabIndex = 5;
            this.buttonSelectConnectors.Text = "Select";
            this.buttonSelectConnectors.UseVisualStyleBackColor = true;
            // 
            // labelConnectors
            // 
            this.labelConnectors.AutoSize = true;
            this.labelConnectors.Location = new System.Drawing.Point(11, 79);
            this.labelConnectors.Name = "labelConnectors";
            this.labelConnectors.Size = new System.Drawing.Size(61, 13);
            this.labelConnectors.TabIndex = 6;
            this.labelConnectors.Text = "Connectors";
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Location = new System.Drawing.Point(11, 13);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(43, 13);
            this.labelSelect.TabIndex = 8;
            this.labelSelect.Text = "Shapes";
            // 
            // FormSelectionTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 137);
            this.Controls.Add(this.labelSelect);
            this.Controls.Add(this.labelConnectors);
            this.Controls.Add(this.buttonSelectConnectors);
            this.Controls.Add(this.buttonUnselectConnectors);
            this.Controls.Add(this.buttonInvertSelection);
            this.Controls.Add(this.buttonSelectNone);
            this.Controls.Add(this.buttonSelectAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSelectionTool";
            this.Text = "Selection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonSelectNone;
        private System.Windows.Forms.Button buttonInvertSelection;
        private System.Windows.Forms.Button buttonUnselectConnectors;
        private System.Windows.Forms.Button buttonSelectConnectors;
        private System.Windows.Forms.Label labelConnectors;
        private System.Windows.Forms.Label labelSelect;
    }
}