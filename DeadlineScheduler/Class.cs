using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	class Class
	{
		public RadioButton rb;
		public TextBox nameBox;
		public Button deleteBtn;
		public Color color;

		public Class(Color c)
		{
			rb = new RadioButton();
			nameBox = new TextBox();
			nameBox.ForeColor = c;
			color = c;
		}

		public RadioButton InitRB(int classCount)
		{
			rb.Location = new Point(15, 8 + 30 * classCount);
			rb.Text = "";
			rb.Size = new Size(20, 13);

			return rb;
		}

		public TextBox InitNamebox(int classCount)
		{
			nameBox.Location = new Point(40, 5 + 30 * classCount);
			nameBox.Size = new Size(100, 13);
			nameBox.Font = new Font(nameBox.Font.FontFamily, 11);
			nameBox.BorderStyle = BorderStyle.None;
			nameBox.Text = "Class " + (classCount + 1).ToString();
			nameBox.Visible = true;

			return nameBox;
		}

		//public Button InitDeleteBtn(int classCount)
		//{

		//}
	}
}
