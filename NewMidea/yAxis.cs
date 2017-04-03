using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace System.Drawing
{
    public class yAxis : Axis
    {
        private bool _IsLeftYAxis = true;
        /// <summary>
        /// 是否是左轴
        /// </summary>
        public bool IsLeftYAxis
        {
            get { return _IsLeftYAxis; }
            set { _IsLeftYAxis = value; }
        }
        #region
        /// <summary>
        ///  建立一条坐标轴
        /// </summary>
        /// <param name="g">Graphics,坐标轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画坐标轴的区域</param>
        /// <param name="axisColor">Color,坐标轴颜色</param>
        /// <param name="isLeftYAxis">bool,是否是左Y轴</param>
        public yAxis(Graphics g, Rectangle DrawArea, Color axisColor, bool isLeftYAxis)
            : base(g, DrawArea, axisColor)
        {
            _IsLeftYAxis = isLeftYAxis;
        }
        /// <summary>
        ///  建立一条坐标轴
        /// </summary>
        /// <param name="g">Graphics,坐标轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画坐标轴的区域</param>
        /// <param name="axisColor">Color,坐标轴颜色</param>
        /// <param name="origin">Point,坐标轴的原点位置</param>
        /// <param name="axisLength">int,坐标轴像素长度</param>
        /// <param name="minValue">double,坐标轴显示最小值</param>
        /// <param name="maxValue">double,坐标轴显示最大值</param>
        /// <param name="part">int,坐标轴分段数</param>
        /// <param name="isLeftYAxis">bool,是否是左Y轴</param>
        public yAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, bool isLeftYAxis)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part)
        {
            _IsLeftYAxis = isLeftYAxis;
        }
        /// <summary>
        ///  建立一条坐标轴
        /// </summary>
        /// <param name="g">Graphics,坐标轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画坐标轴的区域</param>
        /// <param name="axisColor">Color,坐标轴颜色</param>
        /// <param name="origin">Point,坐标轴的原点位置</param>
        /// <param name="axisLength">int,坐标轴像素长度</param>
        /// <param name="minValue">double,坐标轴显示最小值</param>
        /// <param name="maxValue">double,坐标轴显示最大值</param>
        /// <param name="part">int,坐标轴分段数</param>
        /// <param name="title">string,坐标轴标题</param>
        /// <param name="isLeftYAxis">bool,是否是左Y轴</param>
        public yAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, bool isLeftYAxis)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title)
        {
            _IsLeftYAxis = isLeftYAxis;
        }
        /// <summary>
        ///  建立一条坐标轴
        /// </summary>
        /// <param name="g">Graphics,坐标轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画坐标轴的区域</param>
        /// <param name="axisColor">Color,坐标轴颜色</param>
        /// <param name="origin">Point,坐标轴的原点位置</param>
        /// <param name="axisLength">int,坐标轴像素长度</param>
        /// <param name="minValue">double,坐标轴显示最小值</param>
        /// <param name="maxValue">double,坐标轴显示最大值</param>
        /// <param name="part">int,坐标轴分段数</param>
        /// <param name="title">string,坐标轴标题</param>
        /// <param name="titleFont">Font,坐标轴标题字体</param>
        /// <param name="partTitleFont">Font,坐标轴刻度字体</param>
        /// <param name="isLeftYAxis">bool,是否是左Y轴</param>
        public yAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, Font titleFont, Font partTitleFont, Color titleColor, bool isLeftYAxis)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title, titleFont, partTitleFont,titleColor)
        {
            _IsLeftYAxis = isLeftYAxis;
        }
        /// <summary>
        ///  建立一条坐标轴
        /// </summary>
        /// <param name="g">Graphics,坐标轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画坐标轴的区域</param>
        /// <param name="axisColor">Color,坐标轴颜色</param>
        /// <param name="origin">Point,坐标轴的原点位置</param>
        /// <param name="axisLength">int,坐标轴像素长度</param>
        /// <param name="minValue">double,坐标轴显示最小值</param>
        /// <param name="maxValue">double,坐标轴显示最大值</param>
        /// <param name="part">int,坐标轴分段数</param>
        /// <param name="title">string,坐标轴标题</param>
        /// <param name="titleFont">Font,坐标轴标题字体</param>
        /// <param name="partTitleFont">Font,坐标轴刻度字体</param>
        /// <param name="isMainNet">bool,是否画主网格</param>
        /// <param name="isMinorNet">bool,是否画辅助网格</param>
        /// <param name="mainDashStyle">DashStyle,主网格线线形</param>
        /// <param name="minorDashStyle">DashStyle,辅助网格线线形</param>
        /// <param name="isLeftYAxis">bool,是否是左Y轴</param>
        public yAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, Font titleFont, Font partTitleFont,Color titleColor, bool isMainNet, bool isMinorNet, DashStyle mainDashStyle, DashStyle minorDashStyle, bool isLeftYAxis)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title, titleFont, partTitleFont,titleColor, isMainNet, isMinorNet, mainDashStyle, minorDashStyle)
        {
            _IsLeftYAxis = isLeftYAxis;
        }
        #endregion
        public override void DrawTic(int pos, int length)
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (_IsLeftYAxis)
            {
                x1 = Origin.X;
                y1 = _DrawArea.Height - (Origin.Y + pos);
                x2 = x1 - length;
                y2 = y1;
            }
            else
            {
                x1 = _DrawArea.Width - Origin.X;
                y1 = _DrawArea.Height - (Origin.Y + pos);
                x2 = x1 + length;
                y2 = y1;
            }
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
        }
        public override void DrawTitle()
        {
            if (_IsLeftYAxis)
            {
                _hDc.DrawString(Title, TitleFont, new SolidBrush(TitleColor), (float)(Origin.X * 0.1), (float)(Origin.Y * 0.1));
            }
            else
            {
                _hDc.DrawString(Title, TitleFont, new SolidBrush(TitleColor), _DrawArea.Width - (int)(0.9 * Origin.X) - (int)(Title.Length * TitleFont.Size), (float)(Origin.Y * 0.1));
            }
        }
        public override void DrawTicTitle(int pos, string text)
        {
            if (_IsLeftYAxis)
            {
                _hDc.DrawString(text, PartTitleFont, new SolidBrush(Color.Red), (float)(Origin.X * 0.2), _DrawArea.Height - Origin.Y - pos - PartTitleFont.Size / 2);
            }
            else
            {
                _hDc.DrawString(text, PartTitleFont, new SolidBrush(Color.Red), (float)(_DrawArea.Width - Origin.X * 0.8), _DrawArea.Height - Origin.Y - pos - PartTitleFont.Size / 2);
            }
        }
        public override void DrawBaseLine()
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (_IsLeftYAxis)
            {
                x1 = Origin.X;
                y1 = _DrawArea.Height - Origin.Y;
                x2 = x1;
                y2 = y1 - AxisLength;
            }
            else
            {
                x1 = _DrawArea.Width - Origin.X;
                y1 = _DrawArea.Height - Origin.Y;
                x2 = x1;
                y2 = y1 - AxisLength;
            }
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
        }
        public override void DrawArrow()
        {
            int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (_IsLeftYAxis)
            {
                x1 = Origin.X;
                y1 = _DrawArea.Height - Origin.Y - AxisLength;
                x2 = x1 - ArrowLen;
                y2 = y1 + ArrowLen;
                _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
                x2 = x1 + ArrowLen;
                _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
            }
            else
            {
                x1 = _DrawArea.Width - Origin.X;
                y1 = _DrawArea.Height - Origin.Y - AxisLength;
                x2 = x1 - ArrowLen;
                y2 = y1 + ArrowLen;
                _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
                x2 = x1 + ArrowLen;
                _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
            }
        }
        public override void DrawNet(DashStyle dashSyle, int pos)
        {
            Pen p = new Pen(AxisColor);
            p.DashStyle = dashSyle;
            p.Width = 1;
            int x1 = Origin.X;
            int y1 = _DrawArea.Height - (Origin.Y + pos);
            int x2 = _DrawArea.Width - Origin.X;
            int y2 = y1;
            _hDc.DrawLine(p, x1, y1, x2, y2);
        }
    }
}
