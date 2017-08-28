namespace DeadlineScheduler
{
	partial class StartMenu
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
			this.label1 = new System.Windows.Forms.Label();
			this.CloseBtn = new System.Windows.Forms.Button();
			this.ScheduleNameBox = new System.Windows.Forms.TextBox();
			this.SaveOptionBox = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SchedulesPanel = new System.Windows.Forms.Panel();
			this.CreateScheduleBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.NewScheduleBtn = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(110, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Scheduler";
			// 
			// CloseBtn
			// 
			this.CloseBtn.BackColor = System.Drawing.Color.Gainsboro;
			this.CloseBtn.FlatAppearance.BorderSize = 0;
			this.CloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CloseBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CloseBtn.Location = new System.Drawing.Point(1102, 7);
			this.CloseBtn.Name = "CloseBtn";
			this.CloseBtn.Size = new System.Drawing.Size(44, 33);
			this.CloseBtn.TabIndex = 3;
			this.CloseBtn.Text = "x";
			this.CloseBtn.UseVisualStyleBackColor = false;
			this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
			// 
			// ScheduleNameBox
			// 
			this.ScheduleNameBox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ScheduleNameBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ScheduleNameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ScheduleNameBox.Location = new System.Drawing.Point(804, 31);
			this.ScheduleNameBox.Multiline = true;
			this.ScheduleNameBox.Name = "ScheduleNameBox";
			this.ScheduleNameBox.Size = new System.Drawing.Size(193, 27);
			this.ScheduleNameBox.TabIndex = 4;
			this.ScheduleNameBox.Visible = false;
			this.ScheduleNameBox.Click += new System.EventHandler(this.ScheduleNameBox_Click);
			// 
			// SaveOptionBox
			// 
			this.SaveOptionBox.AutoSize = true;
			this.SaveOptionBox.Checked = true;
			this.SaveOptionBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.SaveOptionBox.Location = new System.Drawing.Point(1003, 36);
			this.SaveOptionBox.Name = "SaveOptionBox";
			this.SaveOptionBox.Size = new System.Drawing.Size(125, 21);
			this.SaveOptionBox.TabIndex = 5;
			this.SaveOptionBox.Text = "Save to SQLite";
			this.SaveOptionBox.UseVisualStyleBackColor = true;
			this.SaveOptionBox.Visible = false;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel1.Controls.Add(this.SchedulesPanel);
			this.panel1.Controls.Add(this.ScheduleNameBox);
			this.panel1.Controls.Add(this.SaveOptionBox);
			this.panel1.Controls.Add(this.CreateScheduleBtn);
			this.panel1.Controls.Add(this.CancelBtn);
			this.panel1.Location = new System.Drawing.Point(0, 100);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1155, 517);
			this.panel1.TabIndex = 6;
			// 
			// SchedulesPanel
			// 
			this.SchedulesPanel.Location = new System.Drawing.Point(115, 31);
			this.SchedulesPanel.Name = "SchedulesPanel";
			this.SchedulesPanel.Size = new System.Drawing.Size(575, 306);
			this.SchedulesPanel.TabIndex = 11;
			// 
			// CreateScheduleBtn
			// 
			this.CreateScheduleBtn.BackColor = System.Drawing.Color.SteelBlue;
			this.CreateScheduleBtn.FlatAppearance.BorderSize = 0;
			this.CreateScheduleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CreateScheduleBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CreateScheduleBtn.ForeColor = System.Drawing.Color.White;
			this.CreateScheduleBtn.Location = new System.Drawing.Point(921, 69);
			this.CreateScheduleBtn.Name = "CreateScheduleBtn";
			this.CreateScheduleBtn.Size = new System.Drawing.Size(76, 27);
			this.CreateScheduleBtn.TabIndex = 10;
			this.CreateScheduleBtn.Text = "Create";
			this.CreateScheduleBtn.UseVisualStyleBackColor = false;
			this.CreateScheduleBtn.Visible = false;
			this.CreateScheduleBtn.Click += new System.EventHandler(this.CreateScheduleBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.BackColor = System.Drawing.Color.Silver;
			this.CancelBtn.FlatAppearance.BorderSize = 0;
			this.CancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.CancelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CancelBtn.Location = new System.Drawing.Point(804, 69);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(76, 27);
			this.CancelBtn.TabIndex = 9;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = false;
			this.CancelBtn.Visible = false;
			// 
			// button1
			// 
			this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button1.FlatAppearance.BorderSize = 0;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(84, 56);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(155, 41);
			this.button1.TabIndex = 7;
			this.button1.Text = "Schedules";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// NewScheduleBtn
			// 
			this.NewScheduleBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			this.NewScheduleBtn.FlatAppearance.BorderSize = 0;
			this.NewScheduleBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.NewScheduleBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NewScheduleBtn.Location = new System.Drawing.Point(854, 56);
			this.NewScheduleBtn.Name = "NewScheduleBtn";
			this.NewScheduleBtn.Size = new System.Drawing.Size(76, 41);
			this.NewScheduleBtn.TabIndex = 8;
			this.NewScheduleBtn.Text = "New";
			this.NewScheduleBtn.UseVisualStyleBackColor = true;
			this.NewScheduleBtn.Click += new System.EventHandler(this.NewScheduleBtn_Click);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.SteelBlue;
			this.label2.Location = new System.Drawing.Point(112, 89);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 4);
			this.label2.TabIndex = 9;
			// 
			// StartMenu
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1155, 617);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.CloseBtn);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.NewScheduleBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "StartMenu";
			this.Text = "StartMenu";
			this.Load += new System.EventHandler(this.StartMenu_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button CloseBtn;
		private System.Windows.Forms.TextBox ScheduleNameBox;
		private System.Windows.Forms.CheckBox SaveOptionBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button NewScheduleBtn;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Button CreateScheduleBtn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel SchedulesPanel;
	}
}