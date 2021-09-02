using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000007 RID: 7
	[DefaultEvent("Scroll")]
	public class FlatTrackBar : Control
	{
		// Token: 0x0600002A RID: 42 RVA: 0x0029D728 File Offset: 0x00292F28
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			base.OnMouseDown(e);
			this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(base.Width - 10));
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
				this.Track = new Rectangle(this.Val, 0, 10, 40);
				this.Bool = this.Track.Contains(e.Location);
			}
			if (this._Value > this._Maximum)
			{
				this._Value = this._Maximum;
			}
			if (this._Value < this.minvalue)
			{
				this._Value = this.minvalue;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0029D8D0 File Offset: 0x002930D0
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			base.OnMouseDown(e);
			if (this.Bool && e.X > -10 && e.X < base.Width + 10)
			{
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
			}
			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(base.Width - 10));
				this.Value = this.minvalue + Convert.ToInt32((float)(this._Maximum - this.minvalue) * ((float)e.X / (float)base.Width));
				this.Track = new Rectangle(this.Val, 0, 10, 40);
			}
			if (this._Value > this._Maximum)
			{
				this._Value = this._Maximum;
			}
			if (this._Value < this.minvalue)
			{
				this._Value = this.minvalue;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0029C288 File Offset: 0x00291A88
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.Bool = false;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0029C2A5 File Offset: 0x00291AA5
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0029C2B0 File Offset: 0x00291AB0
		[Category("Colors")]
		public Color TrackColor
		{
			get
			{
				return this._TrackColor;
			}
			set
			{
				this._TrackColor = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0029C2BF File Offset: 0x00291ABF
		// (set) Token: 0x06000030 RID: 48 RVA: 0x0029C2CA File Offset: 0x00291ACA
		[Category("Colors")]
		public Color HatchColor
		{
			get
			{
				return this._HatchColor;
			}
			set
			{
				this._HatchColor = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0029C2D9 File Offset: 0x00291AD9
		// (set) Token: 0x06000032 RID: 50 RVA: 0x0029C2E0 File Offset: 0x00291AE0
		[Category("Colors")]
		public Color ColorScheme1
		{
			get
			{
				return FlatTrackBar.scheme1;
			}
			set
			{
				FlatTrackBar.scheme1 = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000033 RID: 51 RVA: 0x0029C2EB File Offset: 0x00291AEB
		// (set) Token: 0x06000034 RID: 52 RVA: 0x0029C2F2 File Offset: 0x00291AF2
		[Category("Colors")]
		public Color ColorScheme2
		{
			get
			{
				return FlatTrackBar.scheme2;
			}
			set
			{
				FlatTrackBar.scheme2 = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000035 RID: 53 RVA: 0x0029C2FD File Offset: 0x00291AFD
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0029C308 File Offset: 0x00291B08
		[Category("Misc")]
		public int Minimum
		{
			get
			{
				return this.minvalue;
			}
			set
			{
				this.minvalue = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000037 RID: 55 RVA: 0x0029C317 File Offset: 0x00291B17
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0029C322 File Offset: 0x00291B22
		[Category("Misc")]
		public bool Full
		{
			get
			{
				return this.filled;
			}
			set
			{
				this.filled = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x0029C331 File Offset: 0x00291B31
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0029C33C File Offset: 0x00291B3C
		[Category("Misc")]
		public bool Decimal
		{
			get
			{
				return this.floatText;
			}
			set
			{
				this.floatText = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x0029C34B File Offset: 0x00291B4B
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0029C356 File Offset: 0x00291B56
		[Category("Misc")]
		public double FloatValue
		{
			get
			{
				return this.FloatVal;
			}
			set
			{
				this.FloatVal = value;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003D RID: 61 RVA: 0x0029DA90 File Offset: 0x00293290
		// (remove) Token: 0x0600003E RID: 62 RVA: 0x0029DAEC File Offset: 0x002932EC
		public event FlatTrackBar.ScrollEventHandler Scroll;

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003F RID: 63 RVA: 0x0029C365 File Offset: 0x00291B65
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0029DB48 File Offset: 0x00293348
		public int Maximum
		{
			get
			{
				return this._Maximum;
			}
			set
			{
				this._Maximum = value;
				if (value < this._Value)
				{
					this._Value = value;
				}
				if (value < this.minvalue)
				{
					this.minvalue = value;
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000041 RID: 65 RVA: 0x0029C370 File Offset: 0x00291B70
		// (set) Token: 0x06000042 RID: 66 RVA: 0x0029DBA4 File Offset: 0x002933A4
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if (value == this._Value)
				{
					return;
				}
				if (value <= this.Maximum && value >= this.minvalue)
				{
					this._Value = value;
					base.Invalidate();
					if (this.Scroll != null)
					{
						this.Scroll(this);
					}
					return;
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000043 RID: 67 RVA: 0x0029C37B File Offset: 0x00291B7B
		// (set) Token: 0x06000044 RID: 68 RVA: 0x0029C386 File Offset: 0x00291B86
		public bool ShowValue
		{
			get
			{
				return this._ShowValue;
			}
			set
			{
				this._ShowValue = value;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0029DC14 File Offset: 0x00293414
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.KeyCode != Keys.Subtract)
			{
				if (e.KeyCode == Keys.Add)
				{
					if (this.Value == this._Maximum)
					{
						return;
					}
					this.Value++;
				}
				return;
			}
			if (this.Value == 0)
			{
				return;
			}
			this.Value--;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0029C395 File Offset: 0x00291B95
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			base.Invalidate();
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0029C3AD File Offset: 0x00291BAD
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			base.Height = 25;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0029DCA0 File Offset: 0x002934A0
		public FlatTrackBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(38, 38, 38);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0029DDAC File Offset: 0x002935AC
		protected override void OnPaint(PaintEventArgs e)
		{
			new Pen(Color.FromArgb(FlatTrackBar.scheme2.ToArgb()), 1f);
			Bitmap bitmap = new Bitmap(base.Width + 30, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width;
			this.H = base.Height - 1;
			Rectangle rectangle = new Rectangle(2, 2, this.W - 2, this.H - 3);
			GraphicsPath graphicsPath = new GraphicsPath();
			new GraphicsPath();
			Graphics graphics2 = graphics;
			graphics2.SmoothingMode = SmoothingMode.HighQuality;
			graphics2.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics2.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics2.Clear(this.BackColor);
			graphicsPath = Helpers.RoundRec(rectangle, 3);
			graphics2.FillPath(new SolidBrush(this.BaseColor), graphicsPath);
			this.Val = Convert.ToInt32((float)(this._Value - this.minvalue) / (float)(this._Maximum - this.minvalue) * (float)(this.W - 8));
			if (this._Value == this.minvalue)
			{
				this.Track = new Rectangle(this.Val + 4, 4, 6, this.H - 7);
			}
			else if (this._Value == this.minvalue + 1)
			{
				this.Track = new Rectangle(this.Val + 2, 4, 6, this.H - 7);
			}
			else
			{
				this.Track = new Rectangle(this.Val, 4, 6, this.H - 7);
			}
			graphicsPath.AddRectangle(rectangle);
			graphics2.SetClip(graphicsPath);
			graphics2.FillRectangle(new SolidBrush(this._TrackColor), new Rectangle(0, 1, this.Track.X + this.Track.Width, 9));
			graphics2.ResetClip();
			new HatchBrush(HatchStyle.Plaid, this.HatchColor, this._TrackColor);
			new HatchBrush(HatchStyle.Plaid, this.BaseColor, this.BaseColor);
			GraphicsPath path = new GraphicsPath();
			Rectangle rectangle2 = default(Rectangle);
			if (this.filled)
			{
				rectangle2 = new Rectangle(4, 4, this.Track.X + this.Track.Width - 5, this.H - 7);
			}
			else
			{
				rectangle2 = new Rectangle(4, 2, this.Track.X + this.Track.Width - 5, 0);
			}
			path = Helpers.RoundRec(rectangle2, 2);
			graphics2.FillPath(new SolidBrush(FlatTrackBar.scheme1), path);
			GraphicsPath path2 = new GraphicsPath();
			path2 = Helpers.RoundRec(this.Track, 2);
			graphics2.FillPath(new SolidBrush(FlatTrackBar.scheme1), path2);
			graphics2.FillRectangle(this.HBB2, new Rectangle(0, 0, 3, 3));
			if (this.ShowValue)
			{
				if (this.floatText)
				{
					double floatVal = ((double)this.Minimum + (double)this.Value) / 100.0;
					this.FloatVal = floatVal;
					string str = floatVal.ToString();
					graphics2.DrawString(str + this.Text, new Font("Arial", 10f), Brushes.White, new Rectangle(0, 0, this.W, this.H + 2), new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
				}
				else
				{
					graphics2.DrawString(this.Value.ToString() + this.Text, new Font("Arial", 10f), Brushes.White, new Rectangle(0, 0, this.W, this.H + 2), new StringFormat
					{
						Alignment = StringAlignment.Center,
						LineAlignment = StringAlignment.Center
					});
				}
			}
			base.OnPaint(e);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x0400001A RID: 26
		private int W;

		// Token: 0x0400001B RID: 27
		private int H;

		// Token: 0x0400001C RID: 28
		private int Val;

		// Token: 0x0400001D RID: 29
		private bool Bool;

		// Token: 0x0400001E RID: 30
		private bool filled;

		// Token: 0x0400001F RID: 31
		private bool floatText;

		// Token: 0x04000020 RID: 32
		private double FloatVal;

		// Token: 0x04000021 RID: 33
		private Rectangle Track;

		// Token: 0x04000022 RID: 34
		private int minvalue;

		// Token: 0x04000024 RID: 36
		private int _Maximum = 10;

		// Token: 0x04000025 RID: 37
		private int _Value;

		// Token: 0x04000026 RID: 38
		private bool _ShowValue;

		// Token: 0x04000027 RID: 39
		private HatchBrush HBB2 = new HatchBrush(HatchStyle.Plaid, Color.FromArgb(25, 25, 25), Color.FromArgb(25, 25, 25));

		// Token: 0x04000028 RID: 40
		public static Color scheme1 = Color.FromArgb(130, 96, 189);

		// Token: 0x04000029 RID: 41
		public static Color scheme2 = Color.FromArgb(130, 96, 189);

		// Token: 0x0400002A RID: 42
		public static Pen outline_color = new Pen(Color.FromArgb(FlatTrackBar.scheme1.ToArgb()), 1f);

		// Token: 0x0400002B RID: 43
		private Color SliderColor = Color.FromArgb(FlatTrackBar.scheme1.ToArgb());

		// Token: 0x0400002C RID: 44
		private Color BaseColor = Color.FromArgb(60, 60, 60);

		// Token: 0x0400002D RID: 45
		private Color _TrackColor = Color.FromArgb(100, 100, 100);

		// Token: 0x0400002E RID: 46
		private Color _HatchColor = Color.FromArgb(100, 100, 100);

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x060000DF RID: 223
		public delegate void ScrollEventHandler(object sender);
	}
}
