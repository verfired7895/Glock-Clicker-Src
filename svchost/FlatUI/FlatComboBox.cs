using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FlatUI
{
	// Token: 0x02000005 RID: 5
	public class FlatComboBox : ComboBox
	{
		// Token: 0x06000014 RID: 20 RVA: 0x0029C12B File Offset: 0x0029192B
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.State = MouseState.Down;
			base.Invalidate();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0029C151 File Offset: 0x00291951
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.State = MouseState.Over;
			base.Invalidate();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0029C177 File Offset: 0x00291977
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.State = MouseState.Over;
			base.Invalidate();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0029C19D File Offset: 0x0029199D
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.State = MouseState.None;
			base.Invalidate();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0029CEE0 File Offset: 0x002926E0
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.x = e.Location.X;
			this.y = e.Location.Y;
			base.Invalidate();
			if (e.X < base.Width - 41)
			{
				this.Cursor = Cursors.IBeam;
				return;
			}
			this.Cursor = Cursors.Hand;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0029C1C3 File Offset: 0x002919C3
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);
			base.Invalidate();
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				base.Invalidate();
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0029C1FA File Offset: 0x002919FA
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			base.Invalidate();
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001B RID: 27 RVA: 0x0029C212 File Offset: 0x00291A12
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0029C21D File Offset: 0x00291A1D
		public Color HoverColor
		{
			get
			{
				return this._HoverColor;
			}
			set
			{
				this._HoverColor = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0029C22C File Offset: 0x00291A2C
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0029CF78 File Offset: 0x00292778
		private int StartIndex
		{
			get
			{
				return this._StartIndex;
			}
			set
			{
				this._StartIndex = value;
				try
				{
					base.SelectedIndex = value;
				}
				catch
				{
				}
				base.Invalidate();
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0029CFC0 File Offset: 0x002927C0
		public void DrawItem_(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}
			e.DrawBackground();
			e.DrawFocusRectangle();
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(new SolidBrush(this._HoverColor), e.Bounds);
			}
			else
			{
				e.Graphics.FillRectangle(new SolidBrush(this._BaseColor), e.Bounds);
			}
			e.Graphics.DrawString(base.GetItemText(base.Items[e.Index]), new Font("Segoe UI", 8f), Brushes.White, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height));
			e.Graphics.Dispose();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0029C237 File Offset: 0x00291A37
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			base.Height = 18;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0029D15C File Offset: 0x0029295C
		public FlatComboBox()
		{
			base.DrawItem += this.DrawItem_;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			this.DoubleBuffered = true;
			base.DrawMode = DrawMode.OwnerDrawFixed;
			this.BackColor = Color.FromArgb(45, 45, 48);
			this.ForeColor = Color.White;
			base.DropDownStyle = ComboBoxStyle.DropDownList;
			this.Cursor = Cursors.Hand;
			this.StartIndex = 0;
			base.ItemHeight = 18;
			this.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0029D290 File Offset: 0x00292A90
		protected override void OnPaint(PaintEventArgs e)
		{
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			this.W = base.Width;
			this.H = base.Height;
			Rectangle rect = new Rectangle(0, 0, this.W, this.H);
			Rectangle rect2 = new Rectangle(Convert.ToInt32(this.W - 40), 0, this.W, this.H);
			GraphicsPath graphicsPath = new GraphicsPath();
			new GraphicsPath();
			graphics.Clear(Color.FromArgb(45, 45, 48));
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			graphics.FillRectangle(new SolidBrush(this._BGColor), rect);
			graphicsPath.Reset();
			graphicsPath.AddRectangle(rect2);
			graphics.SetClip(graphicsPath);
			graphics.FillRectangle(new SolidBrush(this._BaseColor), rect2);
			graphics.ResetClip();
			graphics.DrawLine(Pens.White, this.W - 10, 6, this.W - 30, 6);
			graphics.DrawLine(Pens.White, this.W - 10, 12, this.W - 30, 12);
			graphics.DrawLine(Pens.White, this.W - 10, 18, this.W - 30, 18);
			graphics.DrawString(this.Text, this.Font, Brushes.White, new Point(4, 4), Helpers.NearSF);
			graphics.Dispose();
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			e.Graphics.DrawImageUnscaled(bitmap, 0, 0);
			bitmap.Dispose();
		}

		// Token: 0x0400000C RID: 12
		private int W;

		// Token: 0x0400000D RID: 13
		private int H;

		// Token: 0x0400000E RID: 14
		private int _StartIndex;

		// Token: 0x0400000F RID: 15
		private int x;

		// Token: 0x04000010 RID: 16
		private int y;

		// Token: 0x04000011 RID: 17
		private MouseState State;

		// Token: 0x04000012 RID: 18
		private Color _BaseColor = Color.FromArgb(25, 27, 29);

		// Token: 0x04000013 RID: 19
		private Color _BGColor = Color.FromArgb(60, 60, 60);

		// Token: 0x04000014 RID: 20
		private Color _HoverColor = Color.FromArgb(35, 168, 109);
	}
}
