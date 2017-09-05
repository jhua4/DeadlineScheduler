using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	class DueDateTextBox : TextBox
	{
		public int rowid { get; set; }

		public Label tagLabel;

		public int spaceBetween;

		private CalendarNode parent;

		public Label dotLabel;

		public DueDateTextBox(Label l, int _spaceBetween, CalendarNode _parent)
		{
			spaceBetween = _spaceBetween;

			if (_spaceBetween != -1)
			{
				tagLabel = l;
			} else
			{
				tagLabel = null;
			}

			parent = _parent;
		}

		public Label InitDotLabel(int x, int y, Color c)
		{
			Label dLabel = new Label();
			dLabel.Location = new Point(x, y);
			dLabel.Size = new Size(7, 7);
			dLabel.Visible = true;
			dLabel.BackColor = c;
			var path = new System.Drawing.Drawing2D.GraphicsPath();
			path.AddEllipse(0, 0, dLabel.Width, dLabel.Height);

			dLabel.Region = new Region(path);
			dLabel.Cursor = Cursors.Hand;

			dotLabel = dLabel;
			dotLabel.MouseDown += DotLabel_MouseDown;
			
			return dLabel;
		}

		private void DotLabel_MouseDown(object sender, MouseEventArgs e)
		{
			parent.removeLabel.Visible = true;
			
			DragDropEffects d = DoDragDrop(rowid, DragDropEffects.Copy);

			if (d == DragDropEffects.None)
			{
				parent.removeLabel.Visible = false;
			}
		}
	}
}
