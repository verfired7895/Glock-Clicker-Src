using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000006 RID: 6
	public class FlatGroupBox : ContainerControl
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0029C254 File Offset: 0x00291A54
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0029C25F File Offset: 0x00291A5F
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0029C26E File Offset: 0x00291A6E
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0029C279 File Offset: 0x00291A79
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

		// Token: 0x06000027 RID: 39 RVA: 0x0029D4EC File Offset: 0x00292CEC
		public FlatGroupBox()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.Transparent;
			base.Size = new Size(240, 180);
			this.Font = new Font("Segoe ui", 10f);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0029D59C File Offset: 0x00292D9C
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
			path = Helpers.RoundRec(rectangle, 7);
			graphics.FillPath(new SolidBrush(this._BaseColor), path);
			bool showText = this.ShowText;
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0029D6FC File Offset: 0x00292EFC
		private void UpdateColors()
		{
			FlatColors colors = Helpers.GetColors(this);
			this._TextColor = colors.Flat;
		}

		// Token: 0x04000015 RID: 21
		private int W;

		// Token: 0x04000016 RID: 22
		private int H;

		// Token: 0x04000017 RID: 23
		private bool _ShowText = true;

		// Token: 0x04000018 RID: 24
		private Color _BaseColor = Color.FromArgb(60, 70, 73);

		// Token: 0x04000019 RID: 25
		private Color _TextColor = Helpers.FlatColor;
	}
}
