using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace DeadlineScheduler
{
	public partial class StartMenu : Form
	{
		private const int WM_NCHITTEST = 0x84;
		private const int HTCLIENT = 0x1;
		private const int HTCAPTION = 0x2;

		///
		/// Handling the window messages
		///
		protected override void WndProc(ref Message message)
		{
			base.WndProc(ref message);

			if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
				message.Result = (IntPtr)HTCAPTION;
		}

		private bool firstClickOnScheduleNameBox = true;
		private int scheduleCount = 0;
		private SQLiteConnection SQLCon;

		public StartMenu()
		{
			InitializeComponent();
		}

		private void StartMenu_Load(object sender, EventArgs e)
		{
			this.CenterToScreen();

			if (!File.Exists("schedules.db"))
			{
				SQLiteConnection.CreateFile("schedules.db");
			}

			SQLCon = CreateSQLiteConnection();

			SQLCon.Open();

			using (SQLiteCommand cmd = new SQLiteCommand("SELECT 1 FROM sqlite_master WHERE type='table' AND name='schedule_names'", SQLCon))
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						using (SQLiteCommand loadSchedules = new SQLiteCommand("SELECT rowid, name, last_update FROM schedule_names", SQLCon))
						{
							reader.Close();

							using (SQLiteDataReader reader1 = loadSchedules.ExecuteReader())
							{
								while (reader1.Read())
								{
									AddScheduleToPanel(Convert.ToInt32((Int64)reader1[0]), (string)reader1[1], (DateTime)reader1[2]);
								}
							}
						}						
					}
					else
					{
						reader.Close();
						
						using (SQLiteCommand createTable = new SQLiteCommand("CREATE TABLE schedule_names (name nvarchar(45), last_update datetime)", SQLCon))
						{
							createTable.ExecuteNonQuery();
						}
					}
				}
			}
			
		}

		private void CloseBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void NewScheduleBtn_Click(object sender, EventArgs e)
		{
			ScheduleNameBox.Visible = true;
			ScheduleNameBox.Text = "Schedule Name";
			ScheduleNameBox.Font = new Font(ScheduleNameBox.Font, FontStyle.Italic);
			ScheduleNameBox.ForeColor = Color.Gray;

			SaveOptionBox.Visible = true;
			CreateScheduleBtn.Visible = true;
			CancelBtn.Visible = true;
		}

		private void ScheduleNameBox_Click(object sender, EventArgs e)
		{
			if (firstClickOnScheduleNameBox)
			{
				ScheduleNameBox.Text = "";
				ScheduleNameBox.ForeColor = Color.Black;
				ScheduleNameBox.Font = new Font(ScheduleNameBox.Font, FontStyle.Regular);
				firstClickOnScheduleNameBox = false;
			}
			
		}

		private void CreateScheduleBtn_Click(object sender, EventArgs e)
		{
			if (ScheduleNameBox.Text != "")
			{
				DateTime createDate = DateTime.Now;

				using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO schedule_names (name, last_update) VALUES (@name, @last_update)", SQLCon))
				{
					cmd.Parameters.AddWithValue("@name", ScheduleNameBox.Text);
					cmd.Parameters.AddWithValue("@last_update", createDate);
					cmd.ExecuteNonQuery();
				}

				Int64 rowid = SQLCon.LastInsertRowId;
				//MessageBox.Show(rowid.ToString() + " $");
				using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE sched_" + rowid.ToString() + " (name nvarchar(45), tag_id int, date datetime); CREATE TABLE tag_ids_" + rowid.ToString() + " (name nvarchar(20), color_id int)", SQLCon))
				{
					cmd.ExecuteNonQuery();
				}


				AddScheduleToPanel(Convert.ToInt32(rowid), ScheduleNameBox.Text, createDate);

				ScheduleNameBox.Visible = false;
				CancelBtn.Visible = false;
				CreateScheduleBtn.Visible = false;
				SaveOptionBox.Visible = false;
				firstClickOnScheduleNameBox = true;
			}
		}

		private SQLiteConnection CreateSQLiteConnection()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory;
			//path = path.Replace(@"bin\Debug\", "");

			SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
			builder.FailIfMissing = true;
			builder.DataSource = Path.Combine(path, "schedules.db");

			return new SQLiteConnection(builder.ConnectionString);
		}

		private void ScheduleList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if (ScheduleList.SelectedItem != null)
			//{
			//	MessageBox.Show(ScheduleList.SelectedItem.ToString());
			//}
		}

		private void AddScheduleToPanel(int rowid, string scheduleName, DateTime lastUpdate)
		{
			ScheduleButton b = new ScheduleButton(rowid, scheduleName);
			b.Text = scheduleName;
			b.AutoSize = true;
			b.Visible = true;
			b.FlatStyle = FlatStyle.Flat;
			b.FlatAppearance.BorderSize = 0;
			b.Cursor = Cursors.Hand;
			b.Font = new Font(b.Font.FontFamily, 9, FontStyle.Regular);
			b.Click += ShowSchedule;
			b.Location = new Point(0, 60 * scheduleCount);
			b.FlatAppearance.MouseOverBackColor = Color.Transparent;
			b.FlatAppearance.MouseDownBackColor = Color.Transparent;

			Label l = new Label();
			l.Text = "Last Updated: " + lastUpdate.ToString("MM/dd/yyyy hh:mm tt");
			l.AutoSize = true;
			l.Font = new Font(l.Font, FontStyle.Italic);
			l.Location = new Point(5, 30 + 60 * scheduleCount++);
			l.ForeColor = Color.DarkGray;

			SchedulesPanel.Controls.Add(b);
			SchedulesPanel.Controls.Add(l);
		}

		private void ShowSchedule(object sender, EventArgs e)
		{
			ScheduleButton b = (ScheduleButton)sender;

			ScheduleView s = new ScheduleView(SQLCon, b.rowid, b.Text);
			s.Show();
		}

		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
