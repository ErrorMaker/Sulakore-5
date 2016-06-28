using System.Timers;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Sulakore.Components
{
    [DesignerCategory("Code")]
    public class SKoreLabel : Control
    {
        private int _dotCount;
        private string _frame;
        private readonly System.Timers.Timer _timer;

        private int _borderWidth = 1;
        [DefaultValue(1)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; Invalidate(); }
        }

        private bool _isBorderVisible = true;
        [DefaultValue(true)]
        public bool IsBorderVisible
        {
            get { return _isBorderVisible; }
            set { _isBorderVisible = value; Invalidate(); }
        }

        private Color _skin = Color.SteelBlue;
        [DefaultValue(typeof(Color), "SteelBlue")]
        public Color Skin
        {
            get { return _skin; }
            set { _skin = value; Invalidate(); }
        }

        [DefaultValue(typeof(Color), "White")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [DefaultValue((double)250)]
        public double Interval
        {
            get { return _timer.Interval; }
            set { _timer.Interval = value; }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public bool IsAnimating
        {
            get { return _timer.Enabled; }
            set
            {
                if (_timer.Enabled == value)
                    return;

                if (value)
                {
                    _dotCount = 1;
                    _frame = (BaseAnimation + ".");
                }
                else _frame = string.Empty;

                _timer.Enabled = value;
                Invalidate();
            }
        }

        [DefaultValue(3)]
        public int MaximumDots { get; set; } = 3;

        private string _baseAnimation = "Working";
        [DefaultValue("Working")]
        public string BaseAnimation
        {
            get { return _baseAnimation; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = "Working";

                if (_baseAnimation == value)
                    return;

                _baseAnimation = value;
                if (IsAnimating)
                {
                    _timer.Enabled = false;
                    IsAnimating = true;
                }
            }
        }

        public SKoreLabel()
        {
            SetStyle((ControlStyles)2050, true);
            BackColor = Color.White;
            DoubleBuffered = true;

            Height = 20;

            _timer = new System.Timers.Timer(250);
            _timer.SynchronizingObject = this;
            _timer.Elapsed += Elapsed;
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_dotCount >= MaximumDots)
            {
                _dotCount = 0;
                _frame = BaseAnimation;
            }
            _frame += ".";
            _dotCount++;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (IsBorderVisible)
            {
                using (var brush = new SolidBrush(Skin))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, BorderWidth, Height);
                    e.Graphics.FillRectangle(brush, Width - BorderWidth, 0, BorderWidth, Height);

                    TextRenderer.DrawText(e.Graphics,
                        (IsAnimating ? _frame : Text), Font, ClientRectangle, ForeColor);
                }
            }
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _timer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}