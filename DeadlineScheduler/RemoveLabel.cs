using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlineScheduler
{
	class RemoveLabel : Label
	{
		public RemoveLabel()
		{
			this.BackColor = System.Drawing.Color.Transparent;
			this.Image = new Bitmap(DeadlineScheduler.Properties.Resources.trash_small_2);
			this.Text = "REMOVE";
			this.Font = new Font(this.Font.FontFamily, 6, FontStyle.Regular);
			this.ImageAlign = ContentAlignment.MiddleLeft;
			this.TextAlign = ContentAlignment.MiddleRight;
			this.AutoSize = false;
			this.Size = new Size(73, 20);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}
	}
}
