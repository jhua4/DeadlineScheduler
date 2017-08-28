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
		public ScheduleView()
		{
			InitializeComponent();
		}

		CalendarNode[] cNodes;

		Class[] classes;

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
			//MessageBox.Show(DateTime.DaysInMonth(2017, 8).ToString());
			//MessageBox.Show(AppDomain.CurrentDomain.BaseDirectory);

			


			

			using (SQLiteConnection SQLCon = CreateSQLiteConnection())
			{
				SQLCon.Open();

				//MessageBox.Show("?");

				//using (SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1", SQLCon))
				//{
				//	using (SQLiteDataReader reader = cmd.ExecuteReader())
				//	{
				//		if (reader.Read())
				//		{
				//			MessageBox.Show((string)reader[0]);
				//		}
				//	}
				//}

				//using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Deadlines (Name, DueDate) VALUES (@Name, @DueDate)", SQLCon))
				//{
				//	cmd.Parameters.AddWithValue("@Name", "HW1");
				//	cmd.Parameters.AddWithValue("@DueDate", DateTime.Now);
				//	cmd.ExecuteNonQuery();
				//}

				//using (SQLiteCommand cmd = new SQLiteCommand("DROP TABLE Deadlines", SQLCon))
				//{
				//	cmd.ExecuteNonQuery();
				//}

				//using (SQLiteCommand cmd = new SQLiteCommand(@"CREATE TABLE [Deadlines] (
				//  [Id] INTEGER NOT NULL
				//, [Name] TEXT NOT NULL
				//, [DueDate] DateTime NOT NULL
				//, [ClassId] INTEGER NULL
				//, CONSTRAINT[PK_Deadlines] PRIMARY KEY([Id])
				//); ", SQLCon))
				//{
				//	cmd.ExecuteNonQuery();
				//}

				using (SQLiteCommand cmd = new SQLiteCommand("SELECT Name, DueDate FROM Deadlines", SQLCon))
				{
					using (SQLiteDataReader reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							MessageBox.Show((string)reader[0] + " : " + ((DateTime)reader[1]).ToString());
						} else
						{
							//MessageBox.Show("??");
						}
					}
				}

				//SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1
			}

			calendar = new RoundPanel();
			calendar.Size = new Size(1150, 750);
			calendar.Location = new Point(40, 0);
			calendar.Text = DateTime.Now.ToString("MMMM");
			this.Controls.Add(calendar);

			classes = new Class[100];

			CreateDayOfWeekLabels();

			LoadMonth(DateTime.Now.Month, DateTime.Now.Year);
		}

		private void CreateDayOfWeekLabels()
		{
			//int x_offset = 140;
			
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
				l.Size = new Size(10, 12);

				cNode.Controls.Add(l);
				cNode.Click += CalendarNode_Click;
				calendar.Controls.Add(cNode);
				cNodes[i] = cNode;

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
						l.Size = new Size(30, 12);

						cNode.Controls.Add(l);
						cNode.Click += CalendarNode_Click;
						calendar.Controls.Add(cNode);
						//cNodes[i] = cNode;
						numOfDaysLeft--;
					}
				}
				rowCount++;
			}
		}

		private Label GetSelectedClassInfo()
		{
			Label l = new Label();
			l.Text = "Class ?";
			l.Visible = true;
			//l.AutoSize = true;

			for (int i = 0; i < classCount; i++)
			{
				if (classes[i].rb.Checked)
				{
					l.Text = classes[i].nameBox.Text;
					l.ForeColor = classes[i].color;
					l.Size = TextRenderer.MeasureText(l.Text, l.Font);
					return l;
				}
			}

			l.Size = TextRenderer.MeasureText(l.Text, l.Font);

			return l;
		}

		private void CalendarNode_Click(object sender, EventArgs e)
		{
			CalendarNode nodeClicked = (CalendarNode)sender;

			//if (nodeClicked.numDates == 4)
			//{
			//	nodeClicked.AutoScroll = false;
			//	nodeClicked.HorizontalScroll.Enabled = false;
			//	nodeClicked.HorizontalScroll.Visible = false;
			//	nodeClicked.HorizontalScroll.Maximum = 0;
			//	nodeClicked.AutoScroll = true;
			//	nodeClicked.VerticalScroll.Maximum = 100;
			//}
			//if (nodeClicked.numDates >= 4)
			//{
			//	//nodeClicked.Size = new Size(nodeClicked.Width, nodeClicked.Height + 20);
			//}

			if (nodeClicked.numDates == 4)
				return;

			Label l = GetSelectedClassInfo();
			l.Location = new Point(5, 20 * ++nodeClicked.numDates);
			//MessageBox.Show(l.Size.ToString());

			TextBox t = new TextBox();
			t.Text = "HW #1";
			t.Location = new Point(l.Width + 5, 20 * nodeClicked.numDates);
			t.Size = new Size(110, 18);
			t.BorderStyle = BorderStyle.None;

			//MessageBox.Show(nodeClicked.numDates.ToString());
			nodeClicked.Controls.Add(l);
			nodeClicked.Controls.Add(t);
		}

		private void AddBtn_Click(object sender, EventArgs e)
		{
			//RadioButton rb = new RadioButton();
			//rb.Location = new Point(15, 8 + 30 * classCount);
			//rb.Text = "";
			//rb.Size = new Size(20, 13);

			//TextBox t = new TextBox();
			//t.Location = new Point(40, 5 + 30 * classCount);
			//t.Size = new Size(100, 13);
			//t.Font = new Font(t.Font.FontFamily, 11);
			//t.BorderStyle = BorderStyle.None;
			//t.Text = "Class " + (classCount + 1).ToString();
			//t.Visible = true;

			if (colorIter == colors.Length)
				colorIter = 0;

			Class c = new Class(colors[colorIter++]);

			ClassPanel.Controls.Add(c.InitRB(classCount));
			ClassPanel.Controls.Add(c.InitNamebox(classCount));

			classes[classCount++] = c;
		}
	}
}
