using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000008 RID: 8
	public static class Helpers
	{
		// Token: 0x0600004B RID: 75 RVA: 0x0029E3D0 File Offset: 0x00293BD0
		public static GraphicsPath RoundRec(Rectangle Rectangle, int Curve)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			int num = Curve * 2;
			graphicsPath.AddArc(new Rectangle(Rectangle.X, Rectangle.Y, num, num), -180f, 90f);
			graphicsPath.AddArc(new Rectangle(Rectangle.Width - num + Rectangle.X, Rectangle.Y, num, num), -90f, 90f);
			graphicsPath.AddArc(new Rectangle(Rectangle.Width - num + Rectangle.X, Rectangle.Height - num + Rectangle.Y, num, num), 0f, 90f);
			graphicsPath.AddArc(new Rectangle(Rectangle.X, Rectangle.Height - num + Rectangle.Y, num, num), 90f, 90f);
			graphicsPath.AddLine(new Point(Rectangle.X, Rectangle.Height - num + Rectangle.Y), new Point(Rectangle.X, Curve + Rectangle.Y));
			return graphicsPath;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0029E530 File Offset: 0x00293D30
		public static GraphicsPath RoundRect(float x, float y, float w, float h, double r = 0.3, bool TL = true, bool TR = true, bool BR = true, bool BL = true)
		{
			float num = Math.Min(w, h) * (float)r;
			float num2 = x + w;
			float num3 = y + h;
			GraphicsPath graphicsPath;
			GraphicsPath result = graphicsPath = new GraphicsPath();
			if (TL)
			{
				graphicsPath.AddArc(x, y, num, num, 180f, 90f);
			}
			else
			{
				graphicsPath.AddLine(x, y, x, y);
			}
			if (TR)
			{
				graphicsPath.AddArc(num2 - num, y, num, num, 270f, 90f);
			}
			else
			{
				graphicsPath.AddLine(num2, y, num2, y);
			}
			if (BR)
			{
				graphicsPath.AddArc(num2 - num, num3 - num, num, num, 0f, 90f);
			}
			else
			{
				graphicsPath.AddLine(num2, num3, num2, num3);
			}
			if (BL)
			{
				graphicsPath.AddArc(x, num3 - num, num, num, 90f, 90f);
			}
			else
			{
				graphicsPath.AddLine(x, num3, x, num3);
			}
			graphicsPath.CloseFigure();
			return result;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0029E6A8 File Offset: 0x00293EA8
		public static GraphicsPath DrawArrow(int x, int y, bool flip)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			int num = 12;
			int num2 = 6;
			if (flip)
			{
				graphicsPath.AddLine(x + 1, y, x + num + 1, y);
				graphicsPath.AddLine(x + num, y, x + num2, y + num2 - 1);
			}
			else
			{
				graphicsPath.AddLine(x, y + num2, x + num, y + num2);
				graphicsPath.AddLine(x + num, y + num2, x + num2, y);
			}
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0029C3CA File Offset: 0x00291BCA
		public static FlatColors GetColors(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException();
			}
			return new FlatColors();
		}

		// Token: 0x0400002F RID: 47
		public static Color FlatColor = Color.FromArgb(38, 38, 38);

		// Token: 0x04000030 RID: 48
		public static readonly StringFormat NearSF = new StringFormat
		{
			Alignment = StringAlignment.Near,
			LineAlignment = StringAlignment.Near
		};

		// Token: 0x04000031 RID: 49
		public static readonly StringFormat CenterSF = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};
	}
}
