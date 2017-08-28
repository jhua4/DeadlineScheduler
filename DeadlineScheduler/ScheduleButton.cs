﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	class ScheduleButton : Button
	{
		public int rowid { get; set; }
		public string name { get; set; }

		public ScheduleButton(int _rowid, string _name)
		{
			rowid = _rowid;
			name = _name;
		}
	}
}
