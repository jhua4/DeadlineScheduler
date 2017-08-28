﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	class CalendarNode : Panel
	{
		public int numDates = 0;

		public CalendarNode(int x, int y)
		{
			this.Size = new System.Drawing.Size(150, 120);
			this.Visible = true;
			this.Location = new System.Drawing.Point(x, y);
			//this.BackColor = System.Drawing.Color.Black;
			this.BorderStyle = BorderStyle.FixedSingle;
			this.BackColor = System.Drawing.Color.White;
		}
	}
}
