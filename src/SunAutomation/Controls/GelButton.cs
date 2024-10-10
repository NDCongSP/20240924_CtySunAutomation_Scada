using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SunAutomation.Controls
{
    public class GelButton : Button
    {

        #region Fields

        private Color _gradientTop = Color.FromArgb(255, 44, 85, 177);
        private Color _gradientBottom = Color.FromArgb(255, 153, 198, 241);
        private Color _paintGradientTop;
        private Color _paintGradientBottom;
        private Color _paintForeColor;
        private Rectangle _buttonRect;
        private Rectangle _highlightRect;
        private int _rectCornerRadius;
        private float _rectOutlineWidth;
        private int _highlightRectOffset;
        private int _defaultHighlightOffset;
        private int _highlightAlphaTop = 255;
        private int _highlightAlphaBottom;
        private Timer _animateButtonHighlightedTimer = new Timer();
        private Timer _animateResumeNormalTimer = new Timer();
        private bool _increasingAlpha;

        #endregion

        #region Properties

        [Category("Appearance")]
        [Description("The color to use for the top portion of the gradient fill of the component.")]
        [DefaultValue(typeof(Color), "0x2C55B1")]
        public Color GradientTop
        {
            get
            {
                return _gradientTop;
            }
            set
            {
                _gradientTop = value;
                SetPaintColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("The color to use for the bottom portion of the gradient fill of the component.")]
        [DefaultValue(typeof(Color), "0x99C6F1")]
        public Color GradientBottom
        {
            get
            {
                return _gradientBottom;
            }
            set
            {
                _gradientBottom = value;
                SetPaintColors();
                Invalidate();
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                SetPaintColors();
                Invalidate();
            }
        }

        #endregion

        #region Initialization and Modification

        protected override void OnCreateControl()
        {
            SuspendLayout();
            SetControlSizes();
            SetPaintColors();
            InitializeTimers();
            base.OnCreateControl();
            ResumeLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            SetControlSizes();
            this.Invalidate();
            base.OnResize(e);
        }

        private void SetControlSizes()
        {
            int scalingDividend = Math.Min(ClientRectangle.Width, ClientRectangle.Height);
            _buttonRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y,
      ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            _rectCornerRadius = Math.Max(1, scalingDividend / 10);
            _rectOutlineWidth = Math.Max(1, scalingDividend / 50);
            _highlightRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y,
      ClientRectangle.Width - 1, (ClientRectangle.Height - 1) / 2);
            _highlightRectOffset = Math.Max(1, scalingDividend / 35);
            _defaultHighlightOffset = Math.Max(1, scalingDividend / 35);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if (!Enabled)
            {
                _animateButtonHighlightedTimer.Stop();
                _animateResumeNormalTimer.Stop();
            }
            SetPaintColors();
            Invalidate();
            base.OnEnabledChanged(e);
        }

        private void SetPaintColors()
        {
            if (Enabled)
            {
                if (SystemInformation.HighContrast)
                {
                    _paintGradientTop = Color.Black;
                    _paintGradientBottom = Color.Black;
                    _paintForeColor = Color.White;
                }
                else
                {
                    _paintGradientTop = _gradientTop;
                    _paintGradientBottom = _gradientBottom;
                    _paintForeColor = ForeColor;
                }
            }
            else
            {
                if (SystemInformation.HighContrast)
                {
                    _paintGradientTop = Color.Gray;
                    _paintGradientBottom = Color.White;
                    _paintForeColor = Color.Black;
                }
                else
                {
                    int grayscaleColorTop = (int)(_gradientTop.GetBrightness() * 255);
                    _paintGradientTop = Color.FromArgb(grayscaleColorTop,
          grayscaleColorTop, grayscaleColorTop);
                    int grayscaleGradientBottom = (int)(_gradientBottom.GetBrightness() * 255);
                    _paintGradientBottom = Color.FromArgb(grayscaleGradientBottom,
          grayscaleGradientBottom, grayscaleGradientBottom);
                    int grayscaleForeColor = (int)(ForeColor.GetBrightness() * 255);
                    if (grayscaleForeColor > 255 / 2)
                    {
                        grayscaleForeColor -= 60;
                    }
                    else
                    {
                        grayscaleForeColor += 60;
                    }
                    _paintForeColor = Color.FromArgb(grayscaleForeColor, grayscaleForeColor, grayscaleForeColor);
                }
            }
        }

        private void InitializeTimers()
        {
            _animateButtonHighlightedTimer.Interval = 20;
            _animateButtonHighlightedTimer.Tick += new EventHandler(animateButtonHighlightedTimer_Tick);
            _animateResumeNormalTimer.Interval = 5;
            _animateResumeNormalTimer.Tick += new EventHandler(animateResumeNormalTimer_Tick);
        }

        #endregion

        #region Custom Painting

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            ButtonRenderer.DrawParentBackground(g, ClientRectangle, this);
            // Paint the outer rounded rectangle
            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath outerPath = RoundedRectangle(_buttonRect, _rectCornerRadius, 0))
            {
                using (LinearGradientBrush outerBrush = new LinearGradientBrush(_buttonRect,
        _paintGradientTop, _paintGradientBottom, LinearGradientMode.Vertical))
                {
                    g.FillPath(outerBrush, outerPath);
                }
                using (Pen outlinePen = new Pen(_paintGradientTop, _rectOutlineWidth))
                {
                    outlinePen.Alignment = PenAlignment.Inset;
                    g.DrawPath(outlinePen, outerPath);
                }
            }
            // If this is the default button, paint an additional highlight
            if (IsDefault)
            {
                using (GraphicsPath defaultPath = new GraphicsPath())
                {
                    defaultPath.AddPath(RoundedRectangle(_buttonRect, _rectCornerRadius, 0), false);
                    defaultPath.AddPath(RoundedRectangle(_buttonRect, _rectCornerRadius, _defaultHighlightOffset), false);
                    using (PathGradientBrush defaultBrush = new PathGradientBrush(defaultPath))
                    {
                        defaultBrush.CenterColor = Color.FromArgb(50, Color.White);
                        defaultBrush.SurroundColors = new Color[] { Color.FromArgb(100, Color.White) };
                        g.FillPath(defaultBrush, defaultPath);
                    }
                }
            }
            // Paint the gel highlight
            using (GraphicsPath innerPath = RoundedRectangle(_highlightRect, _rectCornerRadius, _highlightRectOffset))
            {
                using (LinearGradientBrush innerBrush = new LinearGradientBrush(_highlightRect,
        Color.FromArgb(_highlightAlphaTop, Color.White),
        Color.FromArgb(_highlightAlphaBottom, Color.White), LinearGradientMode.Vertical))
                {
                    g.FillPath(innerBrush, innerPath);
                }
            }
            // Paint the text
            TextRenderer.DrawText(g, Text, Font, _buttonRect, _paintForeColor, Color.Transparent,
      TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static GraphicsPath RoundedRectangle(Rectangle boundingRect, int cornerRadius, int margin)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(boundingRect.X + margin, boundingRect.Y + margin, cornerRadius * 2,
      cornerRadius * 2, 180, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2,
      boundingRect.Y + margin, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddArc(boundingRect.X + boundingRect.Width - margin - cornerRadius * 2,
      boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddArc(boundingRect.X + margin,
      boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2,
      cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(boundingRect.X + margin,
      boundingRect.Y + boundingRect.Height - margin - cornerRadius * 2,
      boundingRect.X + margin, boundingRect.Y + margin + cornerRadius);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        #endregion

        #region Mouse and Keyboard Interaction

        protected override void OnMouseEnter(EventArgs e)
        {
            HighlightButton();
            base.OnMouseEnter(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            HighlightButton();
            base.OnGotFocus(e);
        }

        private void HighlightButton()
        {
            if (Enabled)
            {
                _animateResumeNormalTimer.Stop();
                _animateButtonHighlightedTimer.Start();
            }
        }

        private void animateButtonHighlightedTimer_Tick(object sender, EventArgs e)
        {
            if (_increasingAlpha)
            {
                if (100 <= _highlightAlphaBottom)
                {
                    _increasingAlpha = false;
                }
                else
                {
                    _highlightAlphaBottom += 5;
                }
            }
            else
            {
                if (0 >= _highlightAlphaBottom)
                {
                    _increasingAlpha = true;
                }
                else
                {
                    _highlightAlphaBottom -= 5;
                }
            }
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            ResumeNormalButton();
            base.OnMouseLeave(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            ResumeNormalButton();
            base.OnLostFocus(e);
        }

        private void ResumeNormalButton()
        {
            if (Enabled)
            {
                _animateButtonHighlightedTimer.Stop();
                _animateResumeNormalTimer.Start();
            }
        }

        private void animateResumeNormalTimer_Tick(object sender, EventArgs e)
        {
            bool modified = false;
            if (_highlightAlphaBottom > 0)
            {
                _highlightAlphaBottom -= 5;
                modified = true;
            }
            if (_highlightAlphaTop < 255)
            {
                _highlightAlphaTop += 5;
                modified = true;
            }
            if (!modified)
            {
                _animateResumeNormalTimer.Stop();
            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            PressButton();
            base.OnMouseDown(mevent);
        }

        protected override void OnKeyDown(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space || kevent.KeyCode == Keys.Return)
            {
                PressButton();
            }
            base.OnKeyDown(kevent);
        }

        private void PressButton()
        {
            if (Enabled)
            {
                _animateButtonHighlightedTimer.Stop();
                _animateResumeNormalTimer.Stop();
                _highlightRect.Location = new Point(0, ClientRectangle.Height / 2);
                _highlightAlphaTop = 0;
                _highlightAlphaBottom = 200;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            ReleaseButton();
            if (DisplayRectangle.Contains(mevent.Location))
            {
                HighlightButton();
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnKeyUp(KeyEventArgs kevent)
        {
            if (kevent.KeyCode == Keys.Space || kevent.KeyCode == Keys.Return)
            {
                ReleaseButton();
                if (IsDefault)
                {
                    HighlightButton();
                }
            }
            base.OnKeyUp(kevent);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (Enabled && (mevent.Button & MouseButtons.Left) == MouseButtons.Left &&
      !ClientRectangle.Contains(mevent.Location))
            {
                ReleaseButton();
            }
            base.OnMouseMove(mevent);
        }

        private void ReleaseButton()
        {
            if (Enabled)
            {
                _highlightRect.Location = new Point(0, 0);
                _highlightAlphaTop = 255;
                _highlightAlphaBottom = 0;
            }
        }

        #endregion

    }
}