using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace System.Drawing
{
    [Serializable]
    public partial class QuXianControl : UserControl
    {
        private int _rightChanelNameWidth = 70;
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int RightChanelNameWidth
        {
            get { return _rightChanelNameWidth; }
            set { _rightChanelNameWidth = value; reSet(); }
        }
        private bool _isTwoYAxis = false;//是否两条Y轴
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsTwoYAxis
        {
            get { return _isTwoYAxis; }
            set { _isTwoYAxis = value; reSet(); }
        }
        private double _yMinRight = 0;//Y轴最小值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double YAxisMinRight
        {
            get { return _yMinRight; }
            set
            {
                if (_isTwoYAxis)
                { _yMinRight = value; reSet(); }
            }
        }
        private double _yMaxRight = 100;//Y轴最大值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double YAxisMaxRight
        {
            get { return _yMaxRight; }
            set
            {
                if (_isTwoYAxis)
                { _yMaxRight = value; reSet(); }
            }
        }
        private string _yTitleRight = "Y";//Y轴标题
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string YAxisTitleRight
        {
            get { return _yTitleRight; }
            set
            {
                if (_isTwoYAxis)
                { _yTitleRight = value; reSet(); }
            }
        }

        private double _xMin = 0;//X轴最小值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double XAxisMin
        {
            get { return _xMin; }
            set { _xMin = value; reSet(); }
        }
        private double _yMin = 0;//Y轴最小值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double YAxisMin
        {
            get { return _yMin; }
            set { _yMin = value; reSet(); }
        }

        private double _xMax = 200;//X轴最大值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double XAxisMax
        {
            get { return _xMax; }
            set { _xMax = value; reSet(); }
        }
        private double _yMax = 100;//Y轴最大值
        [EditorBrowsable(EditorBrowsableState.Always)]
        public double YAxisMax
        {
            get { return _yMax; }
            set { _yMax = value; reSet(); }
        }
        private string _xTitle = "X";//X轴标题
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string XAxisTitle
        {
            get { return _xTitle; }
            set { _xTitle = value; reSet(); }
        }
        private string _yTitle = "Y";//Y轴标题
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string YAxisTitle
        {
            get { return _yTitle; }
            set { _yTitle = value; reSet(); }
        }
        private int _xPart = 20;//X轴等分
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int XPart
        {
            get { return _xPart; }
            set { _xPart = value; reSet(); }
        }
        private int _yPart = 20;//Y轴等分
        [EditorBrowsable(EditorBrowsableState.Always)]
        public int YPart
        {
            get { return _yPart; }
            set { _yPart = value; reSet(); }
        }
        private Color _linePen = Color.Green;//轴线颜色
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color LinePen
        {
            get { return _linePen; }
            set { _linePen = value; reSet(); }
        }
        private Font _lineFont = new Font("宋体", 12, FontStyle.Bold);//轴标题字体
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Font LineFont
        {
            get { return _lineFont; }
            set { _lineFont = value; reSet(); }
        }
        private Color _lineColor = Color.Black;//轴标题颜色
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; reSet(); }
        }

        private Font _PartLineFont = new Font("宋体", 8, FontStyle.Bold);//刻度标记颜色
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Font PartLineFont
        {
            get { return _PartLineFont; }
            set { _PartLineFont = value; reSet(); }
        }
        private Color _DrawBackColor = Color.White;//背景色
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Color DrawBackColor
        {
            get { return _DrawBackColor; }
            set { _DrawBackColor = value; reSet(); }
        }
        private Point origin = new Point(30, 20);//原点坐标
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Point Origin
        {
            get { return origin; }
            set { origin = value; reSet(); }
        }
        private bool _IsMainNet = true;//是否显示主网格
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsMainNet
        {
            get { return _IsMainNet; }
            set { _IsMainNet = value; reSet(); }
        }
        private bool _IsMinorNet = true;//是否显示辅助网格
        [EditorBrowsable(EditorBrowsableState.Always)]
        public bool IsMinorNet
        {
            get { return _IsMinorNet; }
            set { _IsMinorNet = value; reSet(); }
        }
        private DashStyle _MainDashStyle = DashStyle.Solid;//主网格线型
        [EditorBrowsable(EditorBrowsableState.Always)]
        public DashStyle MainDashStyle
        {
            get { return _MainDashStyle; }
            set { _MainDashStyle = value; reSet(); }
        }
        private DashStyle _MinorDashStyle = DashStyle.Dash;//辅助网格线形
        [EditorBrowsable(EditorBrowsableState.Always)]
        public DashStyle MinorDashStyle
        {
            get { return _MinorDashStyle; }
            set { _MinorDashStyle = value; reSet(); }
        }
        private yAxis YAxisRight;
        private yAxis YAxis;
        private xAxis XAxis;

        private List<Chanel> chanel = new List<Chanel>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Chanel> Chanel
        {
            get
            {   return chanel; 
            }
            set { chanel = value; reSet(); }
        }

        Graphics g;
        Bitmap b;

        public Bitmap Image
        {
            get { return b; }
            set { b = value; }
        }
        public QuXianControl()
        {
            InitializeComponent();
            reSet();
        }
        public void reSet()
        {
            initAxis();
            initChanel();
        }
        private void initAxis()
        {
            b = new Bitmap(this.Width, this.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            g = Graphics.FromImage(b);
            g.Clear(_DrawBackColor);
            YAxis = new yAxis(g, new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - _rightChanelNameWidth, this.ClientRectangle.Height), _linePen, origin, Height - 2 * origin.Y, _yMin, _yMax,
                _yPart, _yTitle, _lineFont, _PartLineFont, _lineColor, _IsMainNet, _IsMinorNet, _MainDashStyle, _MinorDashStyle, true);
            XAxis = new xAxis(g, new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - _rightChanelNameWidth, this.ClientRectangle.Height), _linePen, origin, Width-RightChanelNameWidth - 2 * origin.X, _xMin, _xMax,
                _xPart, _xTitle, _lineFont, _PartLineFont,_lineColor, _IsMainNet, _IsMinorNet, _MainDashStyle, _MinorDashStyle);
            if (_isTwoYAxis)
            {
                YAxisRight = new yAxis(g, new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - _rightChanelNameWidth, this.ClientRectangle.Height), _linePen, origin, Height - 2 * origin.Y, _yMinRight, _yMaxRight,
                    _yPart, _yTitleRight, _lineFont, _PartLineFont, _lineColor, _IsMainNet, _IsMinorNet, _MainDashStyle, _MinorDashStyle, false);
                YAxisRight.Draw();
            }
            YAxis.Draw();
            XAxis.Draw();
        }
        private void initChanel()
        {
            for (int i = 0; i < chanel.Count; i++)
            {
                if (!chanel[i].IsYAxisRight)
                {
                    chanel[i].initChanel(g, new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - _rightChanelNameWidth, this.ClientRectangle.Height), YAxis, XAxis, origin);
                }
                else
                {
                    if (_isTwoYAxis)
                    {
                        chanel[i].initChanel(g, new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width - _rightChanelNameWidth, this.ClientRectangle.Height), YAxisRight, XAxis, origin);
                    }
                    else
                    {
                        throw new Exception("当前只有一条Y轴,指定Y轴错误");
                    }
                }
                g.DrawString(chanel[i].ChanelName, new Font("宋体", 10), new SolidBrush(chanel[i].DrawColor), new PointF(this.Width - RightChanelNameWidth, 20 + 25 * i));
            }
            
            this.Invalidate();
        }
        public void AddPoint(Chanel c, double x, double y)
        {
            c.AddPoint(x, y);
            this.Invalidate();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }
        private void QuXianControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(b, 0, 0);
        }

        private void QuXianControl_SizeChanged(object sender, EventArgs e)
        {
            reSet();
        }
    }
}