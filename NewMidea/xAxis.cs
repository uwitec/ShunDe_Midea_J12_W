using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Drawing
{
    public class xAxis : Axis
    {
        #region
        /// <summary>
        /// 建立一条X轴
        /// </summary>
        /// <param name="g">Graphics,X轴的画图对象</param>
        /// <param name="DrawArea">Rectangle,绘画X轴的区域</param>
        /// <param name="axisColor">Color,X轴颜色</param>
        public xAxis(Graphics g, Rectangle DrawArea, Color axisColor)
            : base(g, DrawArea, axisColor)
        { }
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
        public xAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part)
        { }
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
        public xAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title)
        { }
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
        public xAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, Font titleFont, Font partTitleFont,Color titleColor)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title, titleFont, partTitleFont,titleColor)
        { }
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
        public xAxis(Graphics g, Rectangle DrawArea, Color axisColor, Point origin, int axisLength, double minValue, double maxValue, int part, string title, Font titleFont, Font partTitleFont,Color titleColor,
            bool isMainNet, bool isMinorNet, DashStyle mainDashStyle, DashStyle minorDashStyle)
            : base(g, DrawArea, axisColor, origin, axisLength, minValue, maxValue, part, title, titleFont, partTitleFont,titleColor, isMainNet, isMinorNet, mainDashStyle, minorDashStyle)
        { }
        #endregion
        /// <summary>
        /// 绘画X轴刻度
        /// </summary>
        /// <param name="pos">int,刻度位置</param>
        /// <param name="length">int,刻度长度</param>
        public override void DrawTic(int pos, int length)
        {
            int x1 = Origin.X + pos;
            int y1 = _DrawArea.Height - (Origin.Y + length);
            int x2 = x1;
            int y2 = _DrawArea.Height - Origin.Y;
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
        }
        /// <summary>
        /// 绘画X轴标题
        /// </summary>
        public override void DrawTitle()
        {
            _hDc.DrawString(Title, TitleFont, new SolidBrush(TitleColor),
                _DrawArea.Width - (int)(Title.Length * TitleFont.Size) - (int)(0.9 * Origin.X),
                _DrawArea.Height - (int)(Origin.Y * 0.9));
        }
        /// <summary>
        /// 绘画X轴刻度值
        /// </summary>
        /// <param name="pos">int,刻度位置</param>
        /// <param name="text">string,刻度值</param>
        public override void DrawTicTitle(int pos, string text)
        {
            _hDc.DrawString(text, PartTitleFont, new SolidBrush(Color.Red),
                Origin.X + pos - (text.Length * PartTitleFont.Size) / 2, _DrawArea.Height - (int)(0.9 * Origin.Y));
        }
        /// <summary>
        /// 绘画X轴主坐标轴
        /// </summary>
        public override void DrawBaseLine()
        {
            int x1 = Origin.X;
            int y1 = _DrawArea.Height - Origin.Y;
            int x2 = Origin.X + AxisLength;
            int y2 = y1;
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
        }
        /// <summary>
        /// 绘画X轴末尾箭头
        /// </summary>
        public override void DrawArrow()
        {
            int x1 = Origin.X + AxisLength;
            int y1 = _DrawArea.Height - Origin.Y;
            int x2 = x1 - ArrowLen;
            int y2 = y1 - ArrowLen;
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
            y2 = y1 + ArrowLen;
            _hDc.DrawLine(new Pen(AxisColor), x1, y1, x2, y2);
        }
        /// <summary>
        /// 绘画X轴方向辅助网格
        /// </summary>
        /// <param name="dashSyle">DashStyle,网格线型</param>
        /// <param name="pos">int,网格位置</param>
        public override void DrawNet(DashStyle dashSyle, int pos)
        {
            Pen p = new Pen(AxisColor);
            p.DashStyle = dashSyle;
            p.Width = 1;

            int x1 = Origin.X + pos;
            int y1 = _DrawArea.Height - Origin.Y;
            int x2 = x1;
            int y2 = Origin.Y;
            _hDc.DrawLine(p, x1, y1, x2, y2);
        }
    }
}
