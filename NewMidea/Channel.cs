using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace System.Drawing
{
    public class Chanel
    {
        string _ChanelName = "";
        /// <summary>
        /// 曲线名称
        /// </summary>
        public string ChanelName
        {
            get { return _ChanelName; }
            set { _ChanelName = value; }
        }

        private Color _DrawColor = Color.Red;
        /// <summary>
        /// 曲线颜色
        /// </summary>
        public Color DrawColor
        {
            get { return _DrawColor; }
            set { _DrawColor = value; }
        }

        private bool _IsVisble = false;
        /// <summary>
        /// 曲线是否可见
        /// </summary>
        public bool IsVisble
        {
            get { return _IsVisble; }
            set { _IsVisble = value; }
        }
        private bool _IsYAxisRight = false;
        /// <summary>
        /// 是否对应右Y轴
        /// </summary>
        public bool IsYAxisRight
        {
            get { return _IsYAxisRight; }
            set { _IsYAxisRight = value; }
        }

        private yAxis _YAxis;
        private xAxis _XAxis;
        private Point _Origin;
        private Graphics _hDc;
        private Rectangle _DrawArea;

        Point oldPoint;
        bool isFirstPoint = true;
        /// <summary>
        /// 生成一条曲线
        /// </summary>
        /// <param name="isVisble">bool,曲线是否可见</param>
        /// <param name="drawColor">Color,曲线颜色</param>
        /// <param name="isYAxisRight">bool,曲线是否对应右轴</param>
        public Chanel(bool isVisble, Color drawColor, bool isYAxisRight)
        {
            _IsVisble = isVisble;
            _DrawColor = drawColor;
            _IsYAxisRight = isYAxisRight;
        }
        public void initChanel(Graphics g, Rectangle DrawArea, yAxis YAxis, xAxis XAxis, Point Origin)
        {
            _YAxis = YAxis;
            _XAxis = XAxis;
            _Origin = Origin;
            _DrawArea = DrawArea;
            _hDc = g;
            isFirstPoint = true;
        }
        public void AddPoint(double X, double Y)
        {
            X = Math.Max(X, _XAxis.MinValue);
            X = Math.Min(X, _XAxis.MaxValue);
            Y = Math.Max(Y, _YAxis.MinValue);
            Y = Math.Min(Y, _YAxis.MaxValue);

            Point curPoint = GetPointAtImage(X, Y);
            if (_IsVisble)
            {
                if (isFirstPoint)
                {
                    isFirstPoint = false;
                }
                else
                {
                    //System.Windows.Forms.MessageBox.Show(string.Format("{0},{1},{2},{3}",
                    //    oldPoint.X, oldPoint.Y, curPoint.X, curPoint.Y));
                    _hDc.DrawLine(new Pen(_DrawColor, 2), oldPoint.X, oldPoint.Y, curPoint.X, curPoint.Y);
                }
                oldPoint = curPoint;
            }
        }
        private Point GetPointAtImage(double x, double y)
        {
            Point curPoint = new Point(0, 0);
            if ((_XAxis.MaxValue == _XAxis.MinValue)
                || (_YAxis.MaxValue == _YAxis.MinValue))
            {
                return curPoint;
            }
            curPoint.X = (int)((double)x * (double)_XAxis.AxisLength / (double)(_XAxis.MaxValue - _XAxis.MinValue) + _Origin.X);
            curPoint.Y = _DrawArea.Height - _Origin.Y - (int)((double)y * (double)_YAxis.AxisLength / (double)(_YAxis.MaxValue - _YAxis.MinValue));
            return curPoint;
        }
    }
}
