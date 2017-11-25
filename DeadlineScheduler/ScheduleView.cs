using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;

namespace DeadlineScheduler
{
	public partial class ScheduleView : Form
	{
		public ScheduleView(SQLiteConnection _SQLCon, int rowid, string _sched_name)
		{
			InitializeComponent();

			SQLCon = _SQLCon;
			sched_table = "sched_" + rowid.ToString();
			tag_ids_table = "tag_ids_" + rowid.ToString();
			sched_row_id = rowid;
		}

		private SQLiteConnection SQLCon;
		private string sched_table = "";
		private string tag_ids_table = "";
		private int sched_row_id;
		private string sched_name = "";

		private DateTime firstDay;

		static public Color c1 = Color.FromArgb(0x18, 0x20, 0x6F);
		//public Color c2 = Color.FromArgb(0x17, 0x25, 0x5A);
		static public Color c2 = Color.FromArgb(230, 219, 116);
		public Color c3 = Color.FromArgb(0xF5, 0xE2, 0xC8);
		public Color c4 = Color.FromArgb(0xD8, 0x83, 0x73);
		static public Color c5 = Color.FromArgb(0xBD, 0x1E, 0x1E);

		public Color c6 = Color.FromArgb(27, 28, 22); //grey color from Monokai theme

		CalendarNode[] cNodes;
		Dictionary<DateTime, CalendarNode> days;
		Dictionary<int, Tag> tags;
		Color[] colors = new Color[] { Color.Magenta, c2, Color.DeepSkyBlue, Color.ForestGreen, c5, Color.Navy };
		ColorUsed[] colorsUsed;

		private int colorIter = 0;

		private int classCount = 0;

		private int width = 160;
		private int height = 150;
		private int x_offset = 20;
		private int y_offset = 100;

		Button UpButton;
		Button DownButton;

		RoundPanel calendar = new RoundPanel();

		private void Form1_Load(object sender, EventArgs e)
		{
			this.Text = sched_name;
			this.BringToFront();
			colorsUsed = new ColorUsed[6];

			//this.BackColor = c3;
			this.BackColor = c6;
			//calendar.BackColor = c3;
			calendar.ForeColor = c2;
			UpButton = new Button();
			DownButton = new Button();

			UpButton.Text = "<-";
			DownButton.Text = "->";

			//set colors
			AddBtn.BackColor = c1;
			AddBtn.ForeColor = Color.White;

			MenuBtn.BackColor = c1;
			MenuBtn.ForeColor = Color.White;

			UpButton.BackColor = c1;
			UpButton.ForeColor = Color.White;

			DownButton.BackColor = c1;
			DownButton.ForeColor = Color.White;

			for (int i = 0; i < 6; i++)
			{
				colorsUsed[i] = new ColorUsed(colors[i]);
			}

			days = new Dictionary<DateTime, CalendarNode>();
			tags = new Dictionary<int, Tag>();

			calendar = new RoundPanel();
			calendar.Size = new Size(1150, 750);
			calendar.Location = new Point(10, 10);
			calendar.Text = DateTime.Now.ToString("MMMM");
			this.Controls.Add(calendar);
			calendar.TitleBackColor = c6;
			calendar.TitleForeColor = c2;

			//l.Location = new Point(i * width + x_offset, 40);
			UpButton.Location = new Point(x_offset, 70);
			DownButton.Location = new Point(7 * width - DownButton.Width, 70);

			UpButton.FlatStyle = FlatStyle.Flat;
			DownButton.FlatStyle = FlatStyle.Flat;

			UpButton.Click += UpButton_Click;
			DownButton.Click += DownButton_Click;

			//UpButton.AutoSize = true;

			calendar.Controls.Add(UpButton);
			calendar.Controls.Add(DownButton);

			UpButton.Visible = true;
			DownButton.Visible = true;

			LoadTags();

			firstDay = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
			LoadMonth(DateTime.Now.StartOfWeek(DayOfWeek.Sunday));
		}

		private void CreateDayOfWeekLabels()
		{
			for (int i = 0; i < 7; i++)
			{
				Label l = new Label();
				l.Text = Enum.GetName(typeof(DayOfWeek), i).Substring(0, 2); //+ "                                          ";
				l.Location = new Point(i * width + x_offset, 40);
				l.Visible = true;
				l.AutoSize = false;
				l.Width = 160;
				l.Font = new Font(l.Font.FontFamily, 14, FontStyle.Underline);
				l.ForeColor = c2;
				calendar.Controls.Add(l);
			}
		}

		private Color GetLeastUsedColor()
		{
			int min = 1000;
			int index = -1;

			for (int i = 0; i < colorsUsed.Length; i++)
			{
				if (colorsUsed[i].count < min)
				{
					min = colorsUsed[i].count;
					index = i;
				}
			}

			colorsUsed[index].count++;

			return colorsUsed[index].color;
		}

		private void LoadWeek(DateTime firstDay, int rowIndex)
		{
			//this week has already been loaded
			if (days.ContainsKey(firstDay))
			{
				for (int i = 0; i < 7; i++)
				{
					days[firstDay.AddDays(i)].SetLocation(i * width + x_offset, rowIndex * height + y_offset);
					calendar.Controls.Add(days[firstDay.AddDays(i)]);
				}
			} else
			{
				//this week has not been loaded yet
				for (int i = 0; i < 7; i++)
				{
					CalendarNode cNode = new CalendarNode(i * width + x_offset, rowIndex * height + y_offset);
					cNode.BorderStyle = BorderStyle.FixedSingle;

					Label l = new Label();
					l.Text = firstDay.AddDays(i).ToString("MMM") + " " + firstDay.AddDays(i).Day.ToString();
					l.Visible = true;
					l.Size = new Size(90, 18);
					l.Location = new Point(10, 0);
					l.BackColor = Color.Transparent;
					//l.ForeColor = c2;
					l.ForeColor = Color.FromArgb(249, 38, 89);
					l.Font = new Font(l.Font, FontStyle.Bold);
					cNode.date = firstDay.AddDays(i);

					cNode.BackColor = c6;

					if (cNode.date == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
					{
						Label ll = new Label();
						//ll.BackColor = c2;
						ll.BackColor = Color.FromArgb(166, 226, 46);
						ll.AutoSize = false;
						ll.Size = new Size(160, 6);
						ll.Location = new Point(0, 114);
						cNode.Controls.Add(ll);
					}

					RemoveLabel r = new RemoveLabel();
					r.Location = new Point(32, 95);
					r.Visible = false;

					r.DragDrop += (o, e) => {
						e.Effect = DragDropEffects.Copy;
						r.SendToBack();
						r.Font = new Font(r.Font, FontStyle.Regular);
						r.Text = r.Text.Substring(3);
						r.Width -= 5;

						int rowid = (int)e.Data.GetData(typeof(int));

						CalendarNode parent = (CalendarNode)r.Parent;

						DueDateTextBox d = null;

						foreach (Control c in parent.Controls)
						{
							if (c is DueDateTextBox)
							{
								if (((DueDateTextBox)c).rowid == rowid)
								{
									d = (DueDateTextBox)c;
								}
							}
						}

						if (MessageBox.Show("Are you sure you want to delete \"" + d.Text + "\"?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							using (SQLiteCommand cmd = new SQLiteCommand("DELETE FROM " + sched_table + " WHERE rowid = @rowid", SQLCon))
							{
								cmd.Parameters.AddWithValue("@rowid", rowid);
								cmd.ExecuteNonQuery();
							}

							parent.Controls.Remove(d.dotLabel);

							if (d.tagLabel != null)
								parent.Controls.Remove(d.tagLabel);

							parent.Controls.Remove(d);

							if (d.rowIndex < parent.numDates)
							{
								foreach (Control c in parent.Controls)
								{
									if (c is DueDateTextBox)
									{
										DueDateTextBox d1 = (DueDateTextBox)c;

										if (d1.rowIndex > d.rowIndex)
										{
											d1.Location = new Point(d1.Location.X, d1.Location.Y - 20);
											d1.dotLabel.Location = new Point(d1.dotLabel.Location.X, d1.dotLabel.Location.Y - 20);
											if (d1.tagLabel != null)
											{
												d1.tagLabel.Location = new Point(d1.tagLabel.Location.X, d1.tagLabel.Location.Y - 20);
												d1.rowIndex--;
											}
										}
									}
								}
							}

							parent.numDates--;
						}

						r.Visible = false;
					};

					r.DragEnter+= (o, e) => {
						e.Effect = DragDropEffects.Copy;
						r.SendToBack();
						r.Font = new Font(r.Font, FontStyle.Bold);
						r.Text = "   " + r.Text;
						r.Width += 5;
					};

					r.DragLeave += (o, e) => {
						r.Font = new Font(r.Font, FontStyle.Regular);
						r.Text = r.Text.Substring(3);
						r.Width -= 5;
					};

					r.AllowDrop = true;

					cNode.removeLabel = r;

					cNode.Controls.Add(r);
					cNode.Controls.Add(l);

					cNode.Click += CalendarNode_Click;

					calendar.Controls.Add(cNode);

					days[cNode.date] = cNode;
				}

				//if (firstDay == DateTime.Now.StartOfWeek(DayOfWeek.Sunday))
				//{
				//	Label l = new Label();
				//	l.BackColor = Color.Green;
				//	l.AutoSize = false;
				//	l.Size = new Size(5, 120);
				//	l.Location = new Point(0, 0);
				//	days[firstDay].Controls.Add(l);
				//	//MessageBox.Show("?" + firstDay.ToString());
				//}

				using (SQLiteCommand cmd = new SQLiteCommand("SELECT *, rowid FROM " + sched_table + " WHERE date >= @fd AND date <= @ld", SQLCon))
				{
					cmd.Parameters.AddWithValue("@fd", firstDay);
					cmd.Parameters.AddWithValue("@ld", firstDay.AddDays(6));

					using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							DateTime date = (DateTime)reader[2];

							Label l = new Label();
							l.Location = new Point(10, 20 * ++days[date].numDates);

							int tagid = (int)reader[1];

							string dueDateName = (string)reader[0];

							int rowid = Convert.ToInt32((Int64)reader[3]);

							if (tagid != -1)
							{
								l.Text = tags[(int)reader[1]].tagNameBox.Text;
								l.ForeColor = tags[(int)reader[1]].color;
								l.Size = TextRenderer.MeasureText(l.Text, l.Font);
								l.BackColor = Color.Transparent;

								DueDateTextBox t = new DueDateTextBox(l, l.Width, days[date], days[date].numDates); //???
								t.Text = dueDateName;
								t.Location = new Point(l.Width + 10, 20 * days[date].numDates);
								t.Size = new Size(100, 18);
								t.BorderStyle = BorderStyle.None;
								t.LostFocus += DueDateBox_Leave;
								t.rowid = rowid;
								t.BackColor = c6;
								t.ForeColor = Color.White;

								days[date].Controls.Add(t.InitDotLabel(5, 20 * days[date].numDates + 5, l.ForeColor));
								days[date].Controls.Add(l);
								days[date].Controls.Add(t);
							}
							else
							{
								DueDateTextBox t = new DueDateTextBox(l, -1, days[date], days[date].numDates);
								t.Text = dueDateName;
								t.Location = new Point(13, 20 * days[date].numDates);
								t.Size = new Size(100, 18);
								t.BorderStyle = BorderStyle.None;
								t.LostFocus += DueDateBox_Leave;
								t.rowid = rowid;
								t.BackColor = c6;

								days[date].Controls.Add(t.InitDotLabel(5, 20 * days[date].numDates + 5, Color.Black));
								days[date].Controls.Add(t);
							}
						}
					}
				}
			}
		}

		private void LoadTags()
		{
			using (SQLiteCommand cmd = new SQLiteCommand("SELECT *, rowid FROM " + tag_ids_table, SQLCon))
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Tag t = new Tag(colorsUsed[(int)reader[1]].color, SQLCon, tag_ids_table);

						TagPanel.Controls.Add(t.InitRB(++classCount));
						TagPanel.Controls.Add(t.InitNamebox(classCount));
						t.tagNameBox.Text = (string)reader[0];
						t.tagNameBox.BackColor = c6;
						
						t.rowid = Convert.ToInt32((Int64)reader[2]);
						tags[Convert.ToInt32((Int64)reader[2])] = t;
					}
				}
			}
		}

		private void LoadMonth(DateTime firstDay)
		{
			calendar.Controls.Clear();

			CreateDayOfWeekLabels();

			calendar.Controls.Add(UpButton);
			calendar.Controls.Add(DownButton);

			LoadWeek(firstDay, 0);
			LoadWeek(firstDay.AddDays(7), 1);
			LoadWeek(firstDay.AddDays(14), 2);
			LoadWeek(firstDay.AddDays(21), 3);

			calendar.Text = firstDay.ToString("MMM") + " " + firstDay.Day.ToString() + " - " + firstDay.AddDays(27).ToString("MMM") + " " + firstDay.AddDays(27).Day.ToString();
		}

		private Tuple<Label, int> GetSelectedTagInfo()
		{
			Label l = new Label();
			l.Text = "";
			l.Visible = true;

			for (int i = 0; i < classCount; i++)
			{
				foreach (int key in tags.Keys)
				{
					if (tags[key].rb.Checked)
					{
						l.Text = tags[key].tagNameBox.Text;
						l.ForeColor = tags[key].color;
						l.Size = TextRenderer.MeasureText(l.Text, l.Font);
						return Tuple.Create(l, tags[key].rowid);
					}
				}
			}

			l.Size = TextRenderer.MeasureText(l.Text, l.Font);

			return Tuple.Create(l, -1);
		}

		private void CalendarNode_Click(object sender, EventArgs e)
		{
			CalendarNode nodeClicked = (CalendarNode)sender;

			if (nodeClicked.numDates == 4)
				return;
			
			DateTime dueDate = nodeClicked.date;

			Tuple<Label, int> tag = GetSelectedTagInfo();

			Label l = tag.Item1;
			l.Location = new Point(10, 20 * ++nodeClicked.numDates);

			DueDateTextBox t = new DueDateTextBox(l, l.Width, nodeClicked, nodeClicked.numDates);
			t.BackColor = c6;
			t.ForeColor = Color.White;

			if (tag.Item2 != -1)
			{
				t.Text = "HW #1";
				t.Location = new Point(l.Width + 10, 20 * nodeClicked.numDates);
				t.Size = new Size(110, 18);
				t.BorderStyle = BorderStyle.None;
				t.LostFocus += DueDateBox_Leave;

				nodeClicked.Controls.Add(t.InitDotLabel(5, 20 * nodeClicked.numDates + 5, l.ForeColor));
			}
			else
			{
				t.Text = "HW #1";
				t.Location = new Point(13, 20 * nodeClicked.numDates);
				t.Size = new Size(110, 18);
				t.BorderStyle = BorderStyle.None;
				t.LostFocus += DueDateBox_Leave;

				nodeClicked.Controls.Add(t.InitDotLabel(5, 20 * nodeClicked.numDates + 5, Color.Black));
			}

			nodeClicked.Controls.Add(l);
			nodeClicked.Controls.Add(t);

			using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + sched_table + " (name, tag_id, date) VALUES (@name, @tag_id, @date)", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", "HW #1");
				cmd.Parameters.AddWithValue("@tag_id", tag.Item2);
				cmd.Parameters.AddWithValue("@date", dueDate);
				cmd.ExecuteNonQuery();
			}

			t.rowid = Convert.ToInt32(SQLCon.LastInsertRowId);
		}

		private void DueDateBox_Leave(object sender, EventArgs e)
		{
			DueDateTextBox t = (DueDateTextBox)sender;

			//MessageBox.Show(rowid.ToString());

			using (SQLiteCommand cmd = new SQLiteCommand("UPDATE " + sched_table + " SET name = @name where rowid = @rowid", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", t.Text);
				cmd.Parameters.AddWithValue("@rowid", t.rowid);
				cmd.ExecuteNonQuery();
			}
		}

		private void AddBtn_Click(object sender, EventArgs e)
		{
			if (colorIter == colorsUsed.Length)
				colorIter = 0;

			Tag t = new Tag(colorsUsed[colorIter].color, SQLCon, tag_ids_table);
			colorsUsed[colorIter++].count++;

			TagPanel.Controls.Add(t.InitRB(++classCount));
			TagPanel.Controls.Add(t.InitNamebox(classCount));

			t.tagNameBox.BackColor = c3;

			using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + tag_ids_table + " (name, color_id) VALUES (@name, @color_id)", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", "NewClass");
				cmd.Parameters.AddWithValue("@color_id", colorIter - 1);
				cmd.ExecuteNonQuery();
			}

			t.rowid = Convert.ToInt32(SQLCon.LastInsertRowId);

			tags[t.rowid] = t;

		}

		private void UpButton_Click(object sender, EventArgs e)
		{
			firstDay = firstDay.AddDays(-7);
			LoadMonth(firstDay);
		}

		private void DownButton_Click(object sender, EventArgs e)
		{
			firstDay = firstDay.AddDays(7);
			LoadMonth(firstDay);
		}

		private void ScheduleView_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Focus();

			using (SQLiteCommand cmd = new SQLiteCommand("UPDATE schedule_names SET last_update = @now WHERE rowid = @rowid", SQLCon))
			{
				cmd.Parameters.AddWithValue("@now", DateTime.Now);
				cmd.Parameters.AddWithValue("@rowid", sched_row_id);
				cmd.ExecuteNonQuery();
			}
		}

		private void ScheduleView_Click(object sender, EventArgs e)
		{
			
		}

		private void MenuBtn_Click(object sender, EventArgs e)
		{
			FormCollection openForms = Application.OpenForms;

			bool menuOpen = false;

			foreach (Form f in openForms)
			{
				if (f.Name == "StartMenu")
				{
					menuOpen = true;
					f.BringToFront();
				}
			}

			if (!menuOpen)
			{
				StartMenu menu = new StartMenu();
				menu.openedFromScheduleView = true;
				menu.Show();
			}
		}
	}

	public static class DateTimeExtensions
	{
		public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
		{
			int diff = dt.DayOfWeek - startOfWeek;
			if (diff < 0)
			{
				diff += 7;
			}
			return dt.AddDays(-1 * diff).Date;
		}
	}
}
