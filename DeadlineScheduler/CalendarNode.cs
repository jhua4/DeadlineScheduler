using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DeadlineScheduler
{
	class CalendarNode : Panel
	{
		public int rowid { get; set; }
		public DateTime date { get; set; }
		public int numDates { get; set; }
		public RemoveLabel removeLabel;

		public CalendarNode(int x, int y)
		{
			this.Size = new System.Drawing.Size(150, 120);
			this.Visible = true;
			this.Location = new System.Drawing.Point(x, y);
			//this.BackColor = System.Drawing.Color.Black;
			this.BorderStyle = BorderStyle.FixedSingle;
			this.BackColor = Color.FromArgb(27, 28, 22); //grey color from Monokai theme

			numDates = 0;
			date = new DateTime();
			rowid = -1;

			//SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
		}

		public void SetLocation(int x, int y)
		{
			this.Location = new System.Drawing.Point(x, y);
		}

		public void s()
		{
			removeLabel.Visible = false;
		}

		//protected override void OnPaint(PaintEventArgs e)
		//{
		//	using (SolidBrush brush = new SolidBrush(Color.White))
		//		e.Graphics.FillRectangle(brush, ClientRectangle);
		//	e.Graphics.DrawRectangle(Pens.Yellow, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
		//}
	}
}
