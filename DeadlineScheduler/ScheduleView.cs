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
using System.Reflection;

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

		CalendarNode[] cNodes;
		Dictionary<DateTime, CalendarNode> days;
		Dictionary<int, Tag> tags;
		Color[] colors = new Color[] { Color.Purple, Color.Orange, Color.DeepSkyBlue, Color.ForestGreen, Color.Fuchsia, Color.Navy };
		ColorUsed[] colorsUsed;

		private int colorIter = 0;

		private int classCount = 0;

		private int width = 160;
		private int height = 150;
		private int x_offset = 20;
		private int y_offset = 100;
		
		RoundPanel calendar = new RoundPanel();

		private void Form1_Load(object sender, EventArgs e)
		{
			this.Text = sched_name;

			colorsUsed = new ColorUsed[6];

			for (int i = 0; i < 6; i++)
			{
				colorsUsed[i] = new ColorUsed(colors[i]);
			}

			days = new Dictionary<DateTime, CalendarNode>();
			tags = new Dictionary<int, Tag>();

			calendar = new RoundPanel();
			calendar.Size = new Size(1150, 750);
			calendar.Location = new Point(140, 0);
			calendar.Text = DateTime.Now.ToString("MMMM");
			this.Controls.Add(calendar);

			CreateDayOfWeekLabels();

			LoadTags();

			firstDay = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
			LoadMonth(DateTime.Now.StartOfWeek(DayOfWeek.Sunday));
		}

		private void CreateDayOfWeekLabels()
		{
			for (int i = 0; i < 7; i++)
			{
				Label l = new Label();
				l.Text = Enum.GetName(typeof(DayOfWeek), i).Substring(0, 2) + "                                          ";
				l.Location = new Point(i * width + x_offset, 40);
				l.Visible = true;
				l.AutoSize = false;
				l.Width = 160;
				l.Font = new Font(l.Font.FontFamily, 14, FontStyle.Underline);
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

					Label l = new Label();
					l.Text = firstDay.ToString("MMM") + " " + firstDay.AddDays(i).Day.ToString();
					l.Visible = true;
					l.Size = new Size(90, 18);
					l.Location = new Point(10, 0);

					cNode.date = firstDay.AddDays(i);

					if (cNode.date == new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
					{
						Label ll = new Label();
						ll.BackColor = Color.Green;
						ll.AutoSize = false;
						ll.Size = new Size(160, 5);
						ll.Location = new Point(0, 115);
						cNode.Controls.Add(ll);
					}

					cNode.Controls.Add(l);
					cNode.Click += CalendarNode_Click;
					calendar.Controls.Add(cNode);

					days[cNode.date] = cNode;
				}

				if (firstDay == DateTime.Now.StartOfWeek(DayOfWeek.Sunday))
				{
					Label l = new Label();
					l.BackColor = Color.Green;
					l.AutoSize = false;
					l.Size = new Size(5, 120);
					l.Location = new Point(0, 0);
					days[firstDay].Controls.Add(l);
					//MessageBox.Show("?" + firstDay.ToString());
				}

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
							l.Location = new Point(5, 20 * ++days[date].numDates);

							int tagid = (int)reader[1];

							string dueDateName = (string)reader[0];

							int rowid = Convert.ToInt32((Int64)reader[3]);

							if (tagid != -1)
							{
								l.Text = tags[(int)reader[1]].tagNameBox.Text;
								l.ForeColor = tags[(int)reader[1]].color;
								l.Size = TextRenderer.MeasureText(l.Text, l.Font);

								DueDateTextBox t = new DueDateTextBox();
								t.Text = dueDateName;
								t.Location = new Point(l.Width + 5, 20 * days[date].numDates);
								t.Size = new Size(110, 18);
								t.BorderStyle = BorderStyle.None;
								t.LostFocus += DueDateBox_Leave;
								t.rowid = rowid;

								days[date].Controls.Add(l);
								days[date].Controls.Add(t);
							}
							else
							{
								DueDateTextBox t = new DueDateTextBox();
								t.Text = dueDateName;
								t.Location = new Point(10, 20 * days[date].numDates);
								t.Size = new Size(110, 18);
								t.BorderStyle = BorderStyle.None;
								t.LostFocus += DueDateBox_Leave;
								t.rowid = rowid;

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
						t.rowid = Convert.ToInt32((Int64)reader[2]);
						tags[Convert.ToInt32((Int64)reader[2])] = t;
					}
				}
			}
		}

		private void LoadMonth(DateTime firstDay)
		{
			calendar.Controls.Clear();

			LoadWeek(firstDay, 0);
			LoadWeek(firstDay.AddDays(7), 1);
			LoadWeek(firstDay.AddDays(14), 2);
			LoadWeek(firstDay.AddDays(21), 3);

			calendar.Text = firstDay.ToString("MMM") + " " + firstDay.Day.ToString() + " - " + firstDay.AddDays(27).ToString("MMM") + " " + firstDay.AddDays(27).Day.ToString();
		}

		private Tuple<Label, int> GetSelectedTagInfo()
		{
			Label l = new Label();
			l.Text = "Class ?";
			l.Visible = true;
			//l.AutoSize = true;

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
			MessageBox.Show("tag id " + tag.Item2.ToString());
			Label l = tag.Item1;
			l.Location = new Point(5, 20 * ++nodeClicked.numDates);

			DueDateTextBox t = new DueDateTextBox();
			t.Text = "HW #1";
			t.Location = new Point(l.Width + 5, 20 * nodeClicked.numDates);
			t.Size = new Size(110, 18);
			t.BorderStyle = BorderStyle.None;
			t.Leave += DueDateBox_Leave;

			//MessageBox.Show(nodeClicked.numDates.ToString());
			nodeClicked.Controls.Add(l);
			nodeClicked.Controls.Add(t);
			nodeClicked.numDates++;

			using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + sched_table + " (name, tag_id, date) VALUES (@name, @tag_id, @date)", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", "HW1");
				cmd.Parameters.AddWithValue("@tag_id", tag.Item2);
				cmd.Parameters.AddWithValue("@date", dueDate);
				cmd.ExecuteNonQuery();
			}
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
