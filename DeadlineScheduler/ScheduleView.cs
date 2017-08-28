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
	public partial class ScheduleView : Form
	{
		public ScheduleView(SQLiteConnection _SQLCon, int rowid)
		{
			InitializeComponent();

			SQLCon = _SQLCon;
			sched_table = "sched_" + rowid.ToString();
			tag_ids_table = "tag_ids_" + rowid.ToString();
		}

		private SQLiteConnection SQLCon;
		private string sched_table = "";
		private string tag_ids_table = "";

		//private int current_tag_id = -1;

		CalendarNode[] cNodes;

		Dictionary<DateTime, CalendarNode> days;

		Dictionary<int, Tag> tags;

		//Tag[] classes;

		Color[] colors = new Color[] { Color.Purple, Color.Orange, Color.DeepSkyBlue, Color.ForestGreen, Color.Fuchsia, Color.Navy };

		private int colorIter = 0;

		private int classCount = 0;

		private int width = 160;
		private int height = 130;
		private int x_offset = 15;
		private int y_offset = 100;
		
		RoundPanel calendar = new RoundPanel();

		private SQLiteConnection CreateSQLiteConnection()
		{
			string path = AppDomain.CurrentDomain.BaseDirectory;
			path = path.Replace(@"bin\Debug\", "");

			SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
			builder.FailIfMissing = true;
			builder.DataSource = Path.Combine(path, "ProjectDB.db"); ;

			return new SQLiteConnection(builder.ConnectionString);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			days = new Dictionary<DateTime, CalendarNode>();
			tags = new Dictionary<int, Tag>();

			calendar = new RoundPanel();
			calendar.Size = new Size(1150, 750);
			calendar.Location = new Point(140, 0);
			calendar.Text = DateTime.Now.ToString("MMMM");
			this.Controls.Add(calendar);

			//classes = new Tag[100];

			CreateDayOfWeekLabels();

			LoadMonth(DateTime.Now.Month, DateTime.Now.Year);
		}

		private void CreateDayOfWeekLabels()
		{
			for (int i = 0; i < 7; i++)
			{
				Label l = new Label();
				l.Text = Enum.GetName(typeof(DayOfWeek), i).Substring(0, 2) + "                                          ";
				//MessageBox.Show(l.Text);
				l.Location = new Point(i * width + x_offset, 40);
				//MessageBox.Show(l.Location.ToString());
				//l.Visible = true;
				//l.Name = "Label" + i.ToString();
				//l.AutoSize = true;
				l.Visible = true;
				l.AutoSize = false;
				l.Width = 160;
				l.Font = new Font(l.Font.FontFamily, 14, FontStyle.Underline);
				calendar.Controls.Add(l);
				//this.Controls.Add(l);
			}
		}

		private void LoadMonth(int monthIndex, int yearIndex)
		{
			int numOfDays = DateTime.DaysInMonth(yearIndex, monthIndex);
			int numOfDaysLeft = numOfDays - 1;

			cNodes = new CalendarNode[numOfDays];

			DateTime firstDay = new DateTime(yearIndex, monthIndex, 1);

			int firstDayIndex = (int)firstDay.DayOfWeek;

			//MessageBox.Show(firstDayIndex.ToString());

			int rowCount = 0;

			for (int i = firstDayIndex; i < 7; i++)
			{
				CalendarNode cNode = new CalendarNode(i * width + x_offset, rowCount * height + y_offset);

				Label l = new Label();
				l.Text = (numOfDays - numOfDaysLeft).ToString();
				l.Visible = true;
				l.Size = new Size(30, 15);

				cNode.date = new DateTime(DateTime.Now.Year, monthIndex, numOfDays - numOfDaysLeft);

				cNode.Controls.Add(l);
				cNode.Click += CalendarNode_Click;
				calendar.Controls.Add(cNode);
				cNodes[i] = cNode;

				days[cNode.date] = cNode;

				numOfDaysLeft--;
			}

			rowCount++;

			while (numOfDaysLeft > 0)
			{
				for (int j = 0; j < 7; j++)
				{
					if (numOfDaysLeft >= 0)
					{
						CalendarNode cNode = new CalendarNode(j * width + x_offset, rowCount * height + y_offset);

						Label l = new Label();
						l.Text = (numOfDays - numOfDaysLeft).ToString();
						//MessageBox.Show((numOfDays - numOfDaysLeft).ToString());
						l.Visible = true;
						l.Size = new Size(30, 15);

						cNode.date = new DateTime(DateTime.Now.Year, monthIndex, numOfDays - numOfDaysLeft);

						cNode.Controls.Add(l);
						cNode.Click += CalendarNode_Click;
						calendar.Controls.Add(cNode);
						//cNodes[i] = cNode;
						numOfDaysLeft--;

						days[cNode.date] = cNode;
					}
				}
				rowCount++;
			}

			using (SQLiteCommand cmd = new SQLiteCommand("SELECT *, rowid FROM " + tag_ids_table, SQLCon))
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						Tag t = new Tag(colors[(int)reader[1]], SQLCon, tag_ids_table);

						TagPanel.Controls.Add(t.InitRB(++classCount));
						TagPanel.Controls.Add(t.InitNamebox(classCount));
						t.tagNameBox.Text = (string)reader[0];
						t.rowid = Convert.ToInt32((Int64)reader[2]);
						tags[Convert.ToInt32((Int64)reader[2])] = t;

						//MessageBox.Show("Added tag id " + (Convert.ToInt32((Int64)reader[2])).ToString());
					}
				}
			}

			using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + sched_table, SQLCon))
			{
				using (SQLiteDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						DateTime date = (DateTime)reader[2];

						Label l = new Label();
						l.Location = new Point(5, 20 * ++days[date].numDates);

						int tagid = (int)reader[1];

						//MessageBox.Show("$searching for tag id " + tagid.ToString());

						if (tagid != -1)
						{
							l.Text = tags[(int)reader[1]].tagNameBox.Text;
							l.ForeColor = tags[(int)reader[1]].color;
							l.Size = TextRenderer.MeasureText(l.Text, l.Font);

							DueDateTextBox t = new DueDateTextBox();
							t.Text = "HW #1";
							t.Location = new Point(l.Width + 5, 20 * days[date].numDates);
							t.Size = new Size(110, 18);
							t.BorderStyle = BorderStyle.None;
							t.LostFocus += DueDateBox_Leave;

							days[date].Controls.Add(l);
							days[date].Controls.Add(t);
						} else
						{
							//l.Text = tags[(int)reader[1]].tagNameBox.Text;
							//l.ForeColor = tags[(int)reader[1]].color;
							//l.Size = TextRenderer.MeasureText(l.Text, l.Font);

							DueDateTextBox t = new DueDateTextBox();
							t.Text = "HW #1";
							t.Location = new Point(10, 20 * days[date].numDates);
							t.Size = new Size(110, 18);
							t.BorderStyle = BorderStyle.None;
							t.LostFocus += DueDateBox_Leave;

							//days[date].Controls.Add(l);
							days[date].Controls.Add(t);
						}
					}
				}
			}
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
			int rowid = t.rowid;

			//MessageBox.Show(rowid.ToString());

			using (SQLiteCommand cmd = new SQLiteCommand("UPDATE " + sched_table + " SET name = @name where rowid = @rowid", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", t.Text);
				cmd.Parameters.AddWithValue("@rowid", rowid);
				cmd.ExecuteNonQuery();
			}
		}

		private void AddBtn_Click(object sender, EventArgs e)
		{
			if (colorIter == colors.Length)
				colorIter = 0;

			Tag t = new Tag(colors[colorIter++], SQLCon, tag_ids_table);

			TagPanel.Controls.Add(t.InitRB(classCount));
			TagPanel.Controls.Add(t.InitNamebox(classCount));

			//classes[classCount++] = t;

			

			using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO " + tag_ids_table + " (name, color_id) VALUES (@name, @color_id)", SQLCon))
			{
				cmd.Parameters.AddWithValue("@name", "NewClass");
				cmd.Parameters.AddWithValue("@color_id", colorIter - 1);
				cmd.ExecuteNonQuery();
			}

			t.rowid = Convert.ToInt32(SQLCon.LastInsertRowId);
			MessageBox.Show("&&" + t.rowid.ToString());

			tags[t.rowid] = t;

		}
	}
}
