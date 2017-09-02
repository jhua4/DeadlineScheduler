using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace DeadlineScheduler
{
	class ColorUsed
	{
		public Color color { get; set; }
		public int count { get; set; }

		public ColorUsed(Color _color)
		{
			color = _color;
			count = 0;
		}
	}
}
