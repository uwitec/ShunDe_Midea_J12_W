using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace System.Drawing
{
    public abstract class Axis
    {
        protected internal Graphics _hDc;
        protected internal Rectangle _DrawArea;
        int _AxisLength = 100;
        /// <summary>
        /// 坐标长度
        /// </summary>
        public int AxisLength
        {
            get { return _AxisLength; }
            set { _AxisLength = value; }
        }

        double _MinValue = 0;
        /// <summary>
        /// 坐标最小值
        /// </summary>
        public double MinValue
        {
            get { return _MinValue; }
            set { _MinValue = value; }
        }

        double _MaxValue = 10;
        /// <summary>
        /// 坐标轴最大值
        /// </summary>
        public double MaxValue
        {
            get { return _MaxValue; }
            set { _MaxValue = value; }
        }
        int _Part = 10;
        /// <summary>
        /// 坐标轴分段
        /// </summary>
        public int Part
        {
            get { return _Part; }
            set { _Part = value; }
        }
        string _Title = "Name";
        /// <summary>
        /// 坐标轴名字
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        Font _TitleFont = new Font("宋体", 12, FontStyle.Bold);
        /// <summary>
        /// 标题字体
        /// </summary>
        public Font TitleFont
        {
            get { return _TitleFont; }
            set { _TitleFont = value; }
        }
        Font _PartTitleFont = new Font("宋体", 8, FontStyle.Regular);
        Color _TitleColor = Color.Black;
        /// <summary>
        /// 标题字体颜色
        /// </summary>
        public Color TitleColor
        {
            get { return _TitleColor; }
            set { _TitleColor = value; }
        }
        /// <summary>
        /// 刻度字体
        /// </summary>
        public Font PartTitleFont
        {
            get { return _PartTitleFont; }
            set { _PartTitleFont = value; }
        }
        Point _Origin = new Point(0, 0);
        /// <summary>
        /// 坐标原点
        /// </summary>
        public Point Origin
        {
            get { return _Origin; }
            set { _Origin = value; }
        }
        Color _AxisColor = Color.Black;
        /// <summary>
        /// 坐标轴画笔
        /// </summary>
        public Color AxisColor
        {
            get { return _AxisColor; }
            set { _AxisColor = value; }
        }
        int _ArrowLen = 5;
        /// <summary>
        /// 箭头尖大小
        /// </summary>
        public int ArrowLen
        {
            get { return _ArrowLen; }
            set { _ArrowLen = value; }
        }
        bool _IsMainNet = true;
        /// <summary>
        /// 是否画主网格
        /// </summary>
        public bool IsMainNet
        {
            get { return _IsMainNet; }
            set { _IsMainNet = value; }
        }
        bool _IsMinorNet = true;
        /// <summary>
        /// 是否画次网格
        /// </summary>
        public bool IsMinorNet
        {
            get { return _IsMinorNet; }
            set { _IsMinorNet = value; }
        }
        DashStyle _MainDashStyle = DashStyle.Solid;
        /// <summary>
        /// 主网格线型
        /// </summary>
        public DashStyle MainDashStyle
        {
            get { return _MainDashStyle; }
            set { _MainDashStyle = value; }
        }
        DashStyle _MinorDashStyle = DashStyle.Dash;
        /// <summary>
        /// 次网格线型
        /// </summary>
        public DashStyle MinorDashStyle
        {
            get { return _MinorDashStyle; }
            set { _MinorDashStyle = value; }
        }
        public Axis(Graphics g, Rectangle DrawArea, Color axisColor)
        {
            _hDc = g;
            _DrawArea = DrawArea;
            _AxisColor = axisColor;
        }
        public Axis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part)
            : this(g, DrawArea, axisColor)
        {
            _Origin = origin;
            _AxisLength = axisLength;
            _MinValue = minValue;
            _MaxValue = maxValue;
            _Part = part;
        }
        public Axis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title)
            : this(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part)
        {
            _Title = title;
        }
        public Axis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, Font titleFont, Font partTitleFont,Color titleColor)
            : this(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title)
        {
            _TitleFont = titleFont;
            _PartTitleFont = partTitleFont;
            _TitleColor = titleColor;
        }
        public Axis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue,
            int part, string title, Font titleFont, Font partTitleFont,Color titleColor, bool isMainNet, bool isMinorNet, DashStyle mainDashStyle, DashStyle minorDashStyle)
            : this(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title, titleFont, partTitleFont,titleColor)
        {
            _IsMainNet = isMainNet;
            _IsMinorNet = isMinorNet;
            _MainDashStyle = mainDashStyle;
            _MinorDashStyle = minorDashStyle;
        }
        public void Draw()
        {
            float Step = (float)(_AxisLength) / (float)(_Part);
            int length = 0;
            DrawBaseLine();
            for (int i = 0; i <= _Part; i++)
            {
                if (i % 2 == 1)
                {
                    length = 4;//小刻度线长
                    if (_IsMinorNet)
                    {
                        DrawNet(_MinorDashStyle, (int)(Step * i));
                    }
                }
                else
                {
                    length = 6;//大刻度线长
                    if (_IsMainNet)
                    {
                        DrawNet(_MainDashStyle, (int)(Step * i));
                    }
                    double tic = ((_MaxValue - _MinValue) / _Part) * i + _MinValue;
                    DrawTicTitle((int)(Step * i), string.Format("{0:0.###}", tic));
                }
                DrawTic((int)(Step * i), length);

            }
            DrawTitle();
            DrawArrow();
        }
        /// <summary>
        /// 画坐标标题
        /// </summary>
        public abstract void DrawTitle();
        /// <summary>
        /// 画刻度
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="length"></param>
        public abstract void DrawTic(int pos, int length);
        /// <summary>
        /// 画刻度标题
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="text"></param>
        public abstract void DrawTicTitle(int pos, string text);
        /// <summary>
        /// 画坐标直线
        /// </summary>
        public abstract void DrawBaseLine();
        /// <summary>
        /// 画箭头
        /// </summary>
        public abstract void DrawArrow();
        /// <summary>
        /// 画网格线
        /// </summary>
        public abstract void DrawNet(DashStyle dashStyle, int pos);
    }
}
