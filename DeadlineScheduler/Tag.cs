using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace DeadlineScheduler
{
	class Tag
	{
		public RadioButton rb;
		public TextBox tagNameBox;
		public Color color;
		public int rowid { get; set; }
		private SQLiteConnection SQLCon;
		private string tag_id_table;
		private string initialTagName = "";

		public Tag(Color c, SQLiteConnection _SQLCon, string _tag_id_table)
		{
			rb = new RadioButton();
			tagNameBox = new TextBox();
			tagNameBox.ForeColor = c;
			color = c;
			rowid = -1;
			SQLCon = _SQLCon;
			tag_id_table = _tag_id_table;

			tagNameBox.Enter += TagNameBox_Enter;
			tagNameBox.Leave += TagNameBox_Leave;
			tagNameBox.LostFocus += TagNameBox_LostFocus;
		}

		public RadioButton InitRB(int classCount)
		{
			rb.Location = new Point(15, 8 + 30 * classCount);
			rb.Text = "";
			rb.Size = new Size(20, 18);

			return rb;
		}

		public TextBox InitNamebox(int classCount)
		{
			tagNameBox.Location = new Point(40, 5 + 30 * classCount);
			tagNameBox.Size = new Size(100, 18);
			tagNameBox.Font = new Font(tagNameBox.Font.FontFamily, 11);
			tagNameBox.BorderStyle = BorderStyle.None;
			tagNameBox.Text = "Class " + (classCount + 1).ToString();
			tagNameBox.Visible = true;

			return tagNameBox;
		}
		
		public void TagNameBox_Enter(object sender, EventArgs e)
		{
			initialTagName = tagNameBox.Text;
		}

		public void TagNameBox_Leave(object sender, EventArgs e)
		{
			MessageBox.Show("$leave");
			if (tagNameBox.Text != initialTagName)
			{
				MessageBox.Show("$changed");
				using (SQLiteCommand cmd = new SQLiteCommand("UPDATE " + tag_id_table + " SET name = @name where rowid = @rowid", SQLCon))
				{
					cmd.Parameters.AddWithValue("@name", tagNameBox.Text);
					cmd.Parameters.AddWithValue("@rowid", rowid);
					cmd.ExecuteNonQuery();
				}
			}
			else
				MessageBox.Show("$no change");
		}

		public void TagNameBox_LostFocus(object sender, EventArgs e)
		{
			if (tagNameBox.Text != initialTagName)
			{
				using (SQLiteCommand cmd = new SQLiteCommand("UPDATE " + tag_id_table + " SET name = @name where rowid = @rowid", SQLCon))
				{
					cmd.Parameters.AddWithValue("@name", tagNameBox.Text);
					cmd.Parameters.AddWithValue("@rowid", rowid);
					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
