﻿namespace DeadlineScheduler
{
	partial class ScheduleView
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
			this.AddBtn = new System.Windows.Forms.Button();
			this.TagPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// AddBtn
			// 
			this.AddBtn.BackColor = System.Drawing.Color.ForestGreen;
			this.AddBtn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.AddBtn.FlatAppearance.BorderSize = 3;
			this.AddBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen;
			this.AddBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.AddBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddBtn.ForeColor = System.Drawing.Color.White;
			this.AddBtn.Location = new System.Drawing.Point(1340, 80);
			this.AddBtn.Name = "AddBtn";
			this.AddBtn.Size = new System.Drawing.Size(160, 43);
			this.AddBtn.TabIndex = 0;
			this.AddBtn.Text = "Add Tag";
			this.AddBtn.UseVisualStyleBackColor = false;
			this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
			// 
			// TagPanel
			// 
			this.TagPanel.Location = new System.Drawing.Point(1320, 140);
			this.TagPanel.Name = "TagPanel";
			this.TagPanel.Size = new System.Drawing.Size(200, 621);
			this.TagPanel.TabIndex = 1;
			// 
			// ScheduleView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1532, 773);
			this.Controls.Add(this.TagPanel);
			this.Controls.Add(this.AddBtn);
			this.Name = "ScheduleView";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button AddBtn;
		private System.Windows.Forms.Panel TagPanel;
	}
}

