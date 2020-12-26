using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace NUC_Raw_Tools
{
    public partial class NRTB : UserControl
    {
        private readonly LineNumberStrip _strip;
        public NRTB()
        {
            InitializeComponent();
            _strip = new LineNumberStrip(richTextBox);
            this.Controls.Add(_strip);
            this.BorderStyle = BorderStyle.Fixed3D;
            base.BackColor = richTextBox.BackColor;
        }
        #region Botões e Especiais
        Rectangle cloneRect;
        Bitmap bmpCrop;
        Bitmap buttons = new Bitmap(Properties.Resources.buttons);
        string[] simbols = { "%l1", "%r1", "%r2", "%l2", "%2", "%4", "%6", "%8", "%u", "%d", "%le", "%ri","%pd","%pl" };
        string[] colors = {"$R","$O","$Y","$G","$B","$P" };
        void DrawButtons(int finded,string s)
        {

            switch (s)
            {
                #region Shoulders
                case "%l1":
                    cloneRect = new Rectangle(0, 0, 24, 16);
                    break;
                case "%r1":
                    cloneRect = new Rectangle(24, 0, 24, 16);
                    break;
                case "%r2":
                    cloneRect = new Rectangle(72, 0, 24, 16);
                    break;
                case "%l2":
                    cloneRect = new Rectangle(48, 0, 24, 16);
                    break;
                #endregion
                #region Botões
                case "%2":
                    cloneRect = new Rectangle(48, 16, 16, 16);
                    break;
                case "%4":
                    cloneRect = new Rectangle(32, 16, 16, 16);
                    break;
                case "%6":
                    cloneRect = new Rectangle(16, 16, 16, 16);
                    break;
                case "%8":
                    cloneRect = new Rectangle(0, 16, 16, 16);
                    break;
                    #endregion
            }
            #region Direcionais
            if (s == "%u")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(88,16,8,9), PixelFormat.DontCare);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(5,0,8,8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            else if(s=="%d")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(88, 16, 8, 9), PixelFormat.DontCare);
                bmpCrop1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(5, 10, 8, 8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            else if (s == "%le")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(87, 28, 9, 8), PixelFormat.DontCare);
                bmpCrop1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(0, 5, 8, 8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            else if (s == "%ri")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(87, 28, 9, 8), PixelFormat.DontCare);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(10, 5, 8, 8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            else if (s == "%pd")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(88, 16, 8, 9), PixelFormat.DontCare);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(5, 0, 8, 8));
                bmpCrop1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(5, 10, 8, 8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            else if (s == "%pl")
            {
                cloneRect = new Rectangle(77, 45, 18, 18);
                bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
                bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
                Bitmap bmpCrop1 = buttons.Clone(new Rectangle(87, 28, 9, 8), PixelFormat.DontCare);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(10, 5, 8, 8));
                bmpCrop1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                CopyRegionIntoImage(bmpCrop1, new Rectangle(0, 0, 8, 9), ref bmpCrop, new Rectangle(0, 5, 8, 8));
                Clipboard.SetImage(bmpCrop);
                this.richTextBox.Text.Remove(finded, s.Length);
                this.richTextBox.Paste();
                return;
            }
            #endregion
            bmpCrop = new Bitmap(cloneRect.Width, cloneRect.Height);
            bmpCrop = buttons.Clone(cloneRect, PixelFormat.DontCare);
            Clipboard.SetImage(bmpCrop);
            this.richTextBox.Text.Remove(finded,s.Length);
            this.richTextBox.Paste();
        }
        void DrawColors(int finded, string c)
        {
            switch(c)
            {
                case "$R":

                    break;
                case "$O":

                    break;
                case "$Y":

                    break;
                case "$G":

                    break;
                case "$B":

                    break;
                case "$P":

                    break;

            }
        }
        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            for (int start = 0; start < this.richTextBox.TextLength; start++)
            {
                #region Botões(Verifier)
                foreach (var s in simbols)
                {
                    int finded = this.richTextBox.Find(s, start, RichTextBoxFinds.None);
                    if (finded != -1)
                    {
                        DrawButtons(finded, s);
                    }
                }
                #endregion
                #region Cores (Verifier)
                foreach (var c in colors)
                {
                    int finded = this.richTextBox.Find(c, start, RichTextBoxFinds.None);
                    if (finded != -1)
                    {
                        DrawColors(finded, c);
                    }
                }
                #endregion
            }
        }

        public static void CopyRegionIntoImage(Bitmap srcBitmap, Rectangle srcRegion, ref Bitmap destBitmap, Rectangle destRegion)
        {
            using (Graphics grD = Graphics.FromImage(destBitmap))
            {
                grD.DrawImage(srcBitmap, destRegion, srcRegion, GraphicsUnit.Pixel);
            }
        }
        #endregion
        public RichTextBox RichTextBox
        {
            get { return richTextBox; }
        }

        public LineNumberStrip Strip
        {
            get { return _strip; }
        }

       
    }

    public enum LineNumberStyle { None, OffsetColors, Boxed };

    public class LineNumberStrip : Control
    {
        private BufferedGraphics _bufferedGraphics;
        private readonly BufferedGraphicsContext _bufferContext = BufferedGraphicsManager.Current;
        private readonly RichTextBox _richTextBox;
        private Brush _fontBrush;
        private Brush _offsetBrush = new SolidBrush(Color.DarkSlateGray);
        private LineNumberStyle _style;
        private Pen _penBoxedLine = Pens.LightGray;
        private float _fontHeight;
        private const float _FONT_MODIFIER = 0.09f;
        private bool _hideWhenNoLines, _speedBump;
        private const int _DRAWING_OFFSET = 1;
        private int _lastYPos = -1, _dragDistance, _lastLineCount;
        private int _scrollingLineIncrement = 5, _numPadding = 3;

        /// <summary>
        /// We need to pass in the MainForm so we can check the form state, Do not
        /// use mainForm.ActiveForm, this is just too dangerous
        /// </summary>
        public LineNumberStrip(RichTextBox plainTextBox)
        {
            _richTextBox = plainTextBox;
            plainTextBox.TextChanged += _richTextBox_TextChanged;
            plainTextBox.FontChanged += _richTextBox_FontChanged;
            plainTextBox.VScroll += _richTextBox_VScroll;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            this.Size = new Size(10, 10);
            base.BackColor = Color.White;
            base.Dock = DockStyle.Left;
            this.OffsetColor = Color.PapayaWhip;
            this.Style = LineNumberStyle.OffsetColors;

            _fontBrush = new SolidBrush(base.ForeColor);

            SetFontHeight();
            UpdateBackBuffer();
            this.SendToBack();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button.Equals(MouseButtons.Left) && _scrollingLineIncrement != 0)
            {
                _lastYPos = Cursor.Position.Y;
                this.Cursor = Cursors.NoMoveVert;
            }
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            SetControlWidth();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            this.Cursor = Cursors.Default;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left) && _scrollingLineIncrement != 0)
            {
                _dragDistance += Cursor.Position.Y - _lastYPos;

                if (_dragDistance > _fontHeight)
                {
                    int selectionStart = _richTextBox.GetFirstCharIndexFromLine(NextLineDown);
                    _richTextBox.Select(selectionStart, 0);
                    _dragDistance = 0;
                }
                else if (_dragDistance < _fontHeight * -1)
                {
                    int selectionStart = _richTextBox.GetFirstCharIndexFromLine(NextLineUp);
                    _richTextBox.Select(selectionStart, 0);
                    _dragDistance = 0;
                }

                _lastYPos = Cursor.Position.Y;
            }
        }

        #region Functions
        private void UpdateBackBuffer()
        {
            if (this.Width > 0)
            {
                _bufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
                _bufferedGraphics = _bufferContext.Allocate(this.CreateGraphics(), this.ClientRectangle);
            }
        }

        /// <summary>
        /// This method keeps the painted text aligned with the text in the corisponding 
        /// textbox perfectly. GetFirstCharIndexFromLine will yeild -1 if line not
        /// present. GetPositionFromCharIndex will yeild an empty point to char index -1.
        /// To explicitly say that line is not present return -1.
        /// </summary>
        private int GetPositionOfRtbLine(int lineNumber)
        {
            int index = _richTextBox.GetFirstCharIndexFromLine(lineNumber);
            Point pos = _richTextBox.GetPositionFromCharIndex(index);
            return index.Equals(-1) ? -1 : pos.Y;
        }

        private void SetFontHeight()
        {
            // Shrink the font for minor compensation
            this.Font = new Font(_richTextBox.Font.FontFamily, _richTextBox.Font.Size -
                _FONT_MODIFIER, _richTextBox.Font.Style);

            _fontHeight = _bufferedGraphics.Graphics.MeasureString("123ABC", this.Font).Height;
        }

        private void SetControlWidth()
        {
            // Make the line numbers virtually invisble when no lines present
            if (_richTextBox.Lines.Length.Equals(0) && _hideWhenNoLines)
            {
                this.Width = 0;
            }
            else
            {
                this.Width = WidthOfWidestLineNumber + _numPadding * 2;
            }

            this.Invalidate(false);
        }
        #endregion

        #region Event Handlers
        private void _richTextBox_FontChanged(object sender, EventArgs e)
        {
            SetFontHeight();
            SetControlWidth();
        }

        /// <summary>
        /// Use this event to look for changes in the line count
        /// </summary>
        private void _richTextBox_TextChanged(object sender, EventArgs e)
        {
            // If word wrap is enabled do not check for line changes as new lines
            // from word wrapping will not raise the line changed event

            // Last line count is always equal to current when words are wrapped
            if (_richTextBox.WordWrap || !_lastLineCount.Equals(_richTextBox.Lines.Length))
            {
                SetControlWidth();
            }

            _lastLineCount = _richTextBox.Lines.Length;
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            _fontBrush = new SolidBrush(this.ForeColor);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateBackBuffer();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            _bufferedGraphics.Graphics.Clear(this.BackColor);

            int firstIndex = _richTextBox.GetCharIndexFromPosition(Point.Empty);
            int firstLine = _richTextBox.GetLineFromCharIndex(firstIndex);
            Point bottomLeft = new Point(0, this.ClientRectangle.Height);
            int lastIndex = _richTextBox.GetCharIndexFromPosition(bottomLeft);
            int lastLine = _richTextBox.GetLineFromCharIndex(lastIndex);

            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                int charYPos = GetPositionOfRtbLine(i);
                if (charYPos.Equals(-1)) continue;
                float yPos = GetPositionOfRtbLine(i) + _DRAWING_OFFSET;

                if (_style.Equals(LineNumberStyle.OffsetColors))
                {
                    if (i % 2 == 0)
                    {
                        _bufferedGraphics.Graphics.FillRectangle(_offsetBrush, 0, yPos, this.Width,
                            _fontHeight * _FONT_MODIFIER * 10);
                    }
                }
                else if (_style.Equals(LineNumberStyle.Boxed))
                {
                    PointF endPos = new PointF(this.Width, yPos + _fontHeight - _DRAWING_OFFSET * 3);
                    PointF startPos = new PointF(0, yPos + _fontHeight - _DRAWING_OFFSET * 3);
                    _bufferedGraphics.Graphics.DrawLine(_penBoxedLine, startPos, endPos);
                }

                PointF stringPos = new PointF(_numPadding, yPos);
                string line = (i + 1).ToString(CultureInfo.InvariantCulture);
                // i + 1 to start the line numbers at 1 instead of 0
                _bufferedGraphics.Graphics.DrawString(line, this.Font, _fontBrush, stringPos);

            }

            _bufferedGraphics.Render(pevent.Graphics);
        }

        private void _richTextBox_VScroll(object sender, EventArgs e)
        {
            // Decrease the paint calls by one half when there is more than 3000 lines
            if (_richTextBox.Lines.Length > 3000 && _speedBump)
            {
                _speedBump = !_speedBump;
                return;
            }

            this.Invalidate(false);
        }
        #endregion

        #region Properties
        private int NextLineDown
        {
            get
            {
                int yPos = _richTextBox.ClientSize.Height + (int)(_fontHeight * ScrollSpeed + 0.5f);
                Point topPos = new Point(0, yPos);
                int index = _richTextBox.GetCharIndexFromPosition(topPos);
                return _richTextBox.GetLineFromCharIndex(index);
            }
        }

        private int NextLineUp
        {
            get
            {
                Point topPos = new Point(0, (int)(_fontHeight * (ScrollSpeed * -1) + -0.5f));
                int index = _richTextBox.GetCharIndexFromPosition(topPos);
                return _richTextBox.GetLineFromCharIndex(index);
            }
        }

        /// <summary>
        /// Gets the width of the widest number on the strip
        /// </summary>
        private int WidthOfWidestLineNumber
        {
            get
            {
                if (_bufferedGraphics.Graphics != null)
                {
                    string strNumber = (_richTextBox.Lines.Length).ToString(CultureInfo.InvariantCulture);
                    SizeF size = _bufferedGraphics.Graphics.MeasureString(strNumber, _richTextBox.Font);
                    return (int)(size.Width + 0.5);
                }

                return 1;
            }
        }

        /// <summary>
        /// Make sure this is set according to the users left to right layout
        /// </summary>
        public bool DockToRight
        {
            get { return (this.Dock == DockStyle.Right); }
            set { this.Dock = (value) ? DockStyle.Right : DockStyle.Left; }
        }

        [Category("Layout")]
        [Description("Gets or sets the spacing from the left and right of the numbers to the let and right of the control")]
        public int NumberPadding
        {
            get { return _numPadding; }
            set
            {
                _numPadding = value;

                if (_richTextBox != null)
                {
                    SetControlWidth();
                }
            }
        }

        [Category("Appearance")]
        public LineNumberStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                this.Invalidate(false);
            }
        }

        [Category("Appearance")]
        public Color BoxedLineColor
        {
            get { return _penBoxedLine.Color; }
            set
            {
                _penBoxedLine = new Pen(value);
                this.Invalidate(false);
            }
        }

        [Category("Appearance")]
        public Color OffsetColor
        {
            get { return new Pen(_offsetBrush).Color; }
            set
            {
                _offsetBrush = new SolidBrush(value);
                this.Invalidate(false);
            }
        }

        [Category("Behavior")]
        public bool HideWhenNoLines
        {
            get { return _hideWhenNoLines; }
            set { _hideWhenNoLines = value; }
        }

        /// <summary>
        /// Hide this, The right to left layout property will determine 
        /// the dock style
        /// </summary>
        [Browsable(false)]
        public override DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        /// <summary>
        /// Gets or sets the scrolling speed in the number of lines
        /// to increment or decrement
        /// </summary>
        [Category("Behavior")]
        public int ScrollSpeed
        {
            get { return _scrollingLineIncrement; }
            set { _scrollingLineIncrement = value; }
        }
        #endregion
    }
}