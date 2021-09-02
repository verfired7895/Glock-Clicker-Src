using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000004 RID: 4
	public class checkBoxes : ContainerControl
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0029C0F7 File Offset: 0x002918F7
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0029C102 File Offset: 0x00291902
		[Category("Colors")]
		public Color BaseColor
		{
			get
			{
				return this._BaseColor;
			}
			set
			{
				this._BaseColor = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0029C111 File Offset: 0x00291911
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0029C11C File Offset: 0x0029191C
		public bool ShowText
		{
			get
			{
				return this._ShowText;
			}
			set
			{
				this._ShowText = value;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0029CCA4 File Offset: 0x002924A4
		public checkBoxes()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.Transparent;
			base.Size = new Size(240, 180);
			this.Font = new Font("Segoe ui", 10f);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0029CD54 File Offset: 0x00292554
		protected override void OnPaint(PaintEventArgs e)
		{
			this.UpdateColors();
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width - 1;
			this.H = base.Height - 1;
			GraphicsPath path = new GraphicsPath();
			new GraphicsPath();
			new GraphicsPath();
			Rectangle rectangle = new Rectangle(2, 2, this.W - 4, this.H - 4);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.Clear(this.BackColor);
			path = Helpers.RoundRec(rectangle, 3);
			graphics.FillPath(new SolidBrush(this._BaseColor), path);
			bool showText = this.ShowText;
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0029CEB4 File Offset: 0x002926B4
		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);
			this._TextColor = colors.Flat;
		}

		// Token: 0x04000007 RID: 7
		private int W;

		// Token: 0x04000008 RID: 8
		private int H;

		// Token: 0x04000009 RID: 9
		private bool _ShowText = true;

		// Token: 0x0400000A RID: 10
		private Color _BaseColor = Color.FromArgb(60, 70, 73);

		// Token: 0x0400000B RID: 11
		private Color _TextColor = Helpers.FlatColor;
	}
}
