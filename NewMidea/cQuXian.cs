using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace NewMideaProgram
{
    public class cQuXian
    {
        static int xMax = 600;//X轴最大值
        /// <summary>
        /// X轴坐标最大值
        /// </summary>
        public static  int XMax
        {
            get { return xMax; }
            set { xMax = value; }
        }
        static int arrowLength = 4;

        public static int ArrowLength
        {
            get { return arrowLength; }
            set { arrowLength = value; }
        }

        static int yMaxLeft = 30;//Y轴左边最大值
        /// <summary>
        /// 左Y轴坐标最大值
        /// </summary>
        public static int YMaxLeft
        {
            get { return yMaxLeft; }
            set { yMaxLeft = value; }
        }
        static int yMaxRight = 5;//Y轴右边最大值
        /// <summary>
        /// 右Y轴坐标最大值
        /// </summary>
        public static int YMaxRight
        {
            get { return yMaxRight; }
            set { yMaxRight = value; }
        }
        static int xPart = 10;//X轴分成部分

        /// <summary>
        /// X轴分成多少段
        /// </summary>
        public static int XPart
        {
            get { return xPart; }
            set { xPart = value; }
        }
        static int yPart = 6;//Y轴分成部分

        /// <summary>
        /// Y轴分成多少段
        /// </summary>
        public static int YPart
        {
            get { return yPart; }
            set { yPart = value; }
        }
        /// <summary>
        /// 画曲线的颜色
        /// </summary>
        public Color[] CurveColor = new Color[]{Color.Red, Color.Blue, Color.DarkGreen,Color.LightCyan,Color.SeaGreen,
								Color.Yellow, Color.Pink, Color.Red, Color.Brown, Color.Cyan,Color.DarkViolet,
                                Color.Red, Color.Blue, Color.DarkGreen,Color.Yellow,Color.SeaGreen,
								Color.Yellow, Color.Pink, Color.White, Color.Brown, Color.Cyan,Color.SlateGray};//画曲线的颜色
        static int xSpace = 50;//图像左右边距
        /// <summary>
        /// 图像到边框的左,右边距
        /// </summary>
        public static int XSpace
        {
            get { return xSpace; }
            set { xSpace = value; }
        }
        static int ySapce = 50;//图像上下边距
        /// <summary>
        /// 图像到边框的上,下边距
        /// </summary>
        public static int YSapce
        {
            get { return ySapce; }
            set { ySapce = value; }
        }
        static Color titleFontColor = Color.HotPink;//总标题字体颜色
        /// <summary>
        /// 总标题字体颜色
        /// </summary>
        public static Color TitleFontColor
        {
            get { return titleFontColor; }
            set { titleFontColor = value; }
        }
        static int titleFontSize = 20;//总标题字体大小
        /// <summary>
        /// 总标题字体大小
        /// </summary>
        public static int TitleFontSize
        {
            get { return titleFontSize; }
            set { titleFontSize = value; }
        }
        static Color axisFontColor = Color.SeaGreen;//小标题字体颜色
        /// <summary>
        /// 小标题字体颜色
        /// </summary>
        public static Color AxisFontColor
        {
            get { return axisFontColor; }
            set { axisFontColor = value; }
        }
        static int axisFontSize = 10;//小标题,坐标字体大小
        /// <summary>
        /// 坐标轴字体大小
        /// </summary>
        public static int AxisFontSize
        {
            get { return axisFontSize; }
            set { axisFontSize = value; }
        }
        static string Title = "实时曲线图";//总标题
        /// <summary>
        /// 曲线总标题
        /// </summary>
        public static string Title1
        {
            get { return Title; }
            set { Title = value; }
        }
        static string xTitle = "时间(s)";//X轴标题
        /// <summary>
        /// X轴标题
        /// </summary>
        public static string XTitle
        {
            get { return xTitle; }
            set { xTitle = value; }
        }
        static string yTitleLeft = "温差,电流(℃,A)";//Y轴左标题
        /// <summary>
        /// 左Y轴标题
        /// </summary>
        public static string YTitleLeft
        {
            get { return yTitleLeft; }
            set { yTitleLeft = value; }
        }
        static string yTitleRight = "压力(Mpa)";//Y轴右标题
        /// <summary>
        /// 右Y轴标题(如果有右Y轴)
        /// </summary>
        public static string YTitleRight
        {
            get { return yTitleRight; }
            set { yTitleRight = value; }
        }
        static bool isTwoYAxis = true;//是否Y轴为两轴;
        /// <summary>
        /// 是否有两个Y轴
        /// </summary>
        public static bool IsTwoYAxis
        {
            get { return isTwoYAxis; }
            set { isTwoYAxis = value; }
        }
        static Color axisColor = Color.Green;//坐标轴颜色
        /// <summary>
        /// 主坐标轴颜色
        /// </summary>
        public static Color AxisColor
        {
            get { return axisColor; }
            set { axisColor = value; }
        }
        static Color backColor = Color.Black;//曲线图背景色
        /// <summary>
        /// 曲线图背景色
        /// </summary>
        public static Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        static Color partAxisColor = Color.LightPink;//辅助网格颜色
        /// <summary>
        /// 辅助网格颜色
        /// </summary>
        public static Color PartAxisColor
        {
            get { return partAxisColor; }
            set { partAxisColor = value; }
        }
        static DashStyle mainAxisStyle = DashStyle.Solid;//主网格线形,默认实线
        /// <summary>
        /// 主网格线形,默认实线
        /// </summary>
        public static DashStyle MainAxisStyle
        {
            get { return mainAxisStyle; }
            set { mainAxisStyle = value; }
        }
        static DashStyle partAxisStyle = DashStyle.Dash;//辅风格线形,默认虚线
        /// <summary>
        /// 辅助网格线形,默认虚线
        /// </summary>
        public static DashStyle PartAxisStyle
        {
            get { return partAxisStyle; }
            set { partAxisStyle = value; }
        }
        static bool isMainAxis = true;//是否画主网格
        /// <summary>
        /// 是否画主网格
        /// </summary>
        public static bool IsMainAxis
        {
            get { return isMainAxis; }
            set { isMainAxis = value; }
        }
        static bool isPartAxis = true;//是否画辅网格
        /// <summary>
        /// 是否画辅助网格
        /// </summary>
        public static bool IsPartAxis
        {
            get { return isPartAxis; }
            set { isPartAxis = value; }
        }
        bool[] isShowTitle = new bool[20];//是否显示曲线
        /// <summary>
        /// 是否显示曲线
        /// </summary>
        public bool[] IsShowTitle
        {
            get { return isShowTitle; }
            set { isShowTitle = value; }
        }

        public static Bitmap img;//保存的图片
        Graphics objGraphics;
        PictureBox pic;
        int CurveCount = cMain.DataShow;//多少条曲线
        int xCurPart;//变量,X轴实际分成部分
        Point[] oldPoint;
        bool[] isFirstPoint;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mPic">picture,要在上面画图的控件</param>
        /// <param name="mCurveCount">int,曲线数量</param>
        /// <param name="isShow">bool,数组,表示所有曲线是否显示</param>
        public cQuXian(PictureBox mPic, int mCurveCount,bool[] isShow)
        {
            int i = 0;
            for (i = 0; i < isShow.Length; i++)
            {
                isShowTitle[i] = isShow[i];
            }
            for (i = isShow.Length; i < 60; i++)
            {
                isShowTitle[i] = false;
            }
            Bitmap tempImg;//临时位图
            pic = mPic;
            CurveCount = mCurveCount;
            oldPoint = new Point[CurveCount];
            isFirstPoint = new bool[CurveCount];
            tempImg = new Bitmap(pic.Width, pic.Height);
            pic.Image = tempImg;
            pic.BackColor = backColor;
            img = new Bitmap(pic.Image);
            objGraphics = Graphics.FromImage(img);
            fClear();
        }
        /// <summary>
        /// 曲线清除
        /// </summary>
        public void fClear()
        {
            isFirstPoint = new bool[CurveCount];
            int i;
            for (i = 0; i < CurveCount; i++)
            {
                isFirstPoint[i] = true;
            }
            oldPoint = new Point[CurveCount];
            objGraphics.Clear(pic.BackColor);
            CurveInit();
        }
        /// <summary>
        /// 曲线初始化,画横纵坐标轴,网格线等
        /// </summary>
        private void CurveInit()
        {
            int i;
            Pen p = new Pen(axisColor, 3);
            p.DashStyle = DashStyle.Solid;
            objGraphics.DrawLine(p, xSpace, pic.Height - ySapce, pic.Width - 10, pic.Height - ySapce);//画X轴
            //画两个像素的箭头 其中那个加一是为了让箭头长一点,好看
            objGraphics.DrawLine(p, pic.Width - 10 - arrowLength, pic.Height - ySapce - arrowLength, pic.Width - 10 + 1, pic.Height - ySapce + 1);
            objGraphics.DrawLine(p, pic.Width - 10 - arrowLength, pic.Height - ySapce + arrowLength, pic.Width - 10 + 1, pic.Height - ySapce - 1);

            objGraphics.DrawLine(p, xSpace, pic.Height - ySapce, xSpace, 10);//画Y轴
            //画两个像素的箭头
            objGraphics.DrawLine(p, xSpace - arrowLength, 10 + arrowLength, xSpace + 1, 10 - 1);
            objGraphics.DrawLine(p, xSpace + arrowLength, 10 + arrowLength, xSpace - 1, 10 + 1);
            if (isTwoYAxis)//是否两条Y轴
            {
                objGraphics.DrawLine(p, pic.Width - xSpace, pic.Height - ySapce, pic.Width - xSpace, 10);
                //画两个像素的箭头
                objGraphics.DrawLine(p, pic.Width - xSpace - arrowLength, 10 + arrowLength, pic.Width - xSpace + 1, 10 - 1);
                objGraphics.DrawLine(p, pic.Width - xSpace + arrowLength, 10 + arrowLength, pic.Width - xSpace - 1, 10 + 1);
                xCurPart = xPart;
            }
            else
            {
                xCurPart = xPart + 1;
            }
            if (isMainAxis)//是否画主网格
            {
                p.Width = 2;//风格线宽
                p.DashStyle = mainAxisStyle;
                p.Color = partAxisColor;
                for (i = 1; i < xCurPart; i++)
                {//画主辅助网格
                    objGraphics.DrawLine(p, xSpace + i * (pic.Width - 2 * xSpace) / xPart, pic.Height - ySapce, xSpace + i * (pic.Width - 2 * xSpace) / xPart, ySapce);
                }
                for (i = 0; i < yPart; i++)
                {
                    objGraphics.DrawLine(p, xSpace, ySapce + i * (pic.Height - 2 * ySapce) / yPart, pic.Width - xSpace, ySapce + i * (pic.Height - 2 * ySapce) / yPart);
                }
            }
            if (isPartAxis)//是否画辅助风格
            {
                p.Width = 1;//风格线宽
                p.DashStyle = partAxisStyle;
                p.Color = partAxisColor;
                for (i = 1; i < xPart + 1; i++)
                {//画辅助网格
                    objGraphics.DrawLine(p, xSpace + (int)((i - 0.5) * (pic.Width - 2 * xSpace) / xPart), pic.Height - ySapce, xSpace + (int)((i - 0.5) * (pic.Width - 2 * xSpace) / xPart), ySapce);
                }
                for (i = 0; i < yPart; i++)
                {
                    objGraphics.DrawLine(p, xSpace, ySapce + (int)((i + 0.5) * (pic.Height - 2 * ySapce) / yPart), pic.Width - xSpace, ySapce + (int)((i + 0.5) * (pic.Height - 2 * ySapce) / yPart));
                }
            }
            //画X轴文字
            for (i = 0; i < xPart + 1; i++)
            {
                objGraphics.DrawString((xMax * i / xPart).ToString(), new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(Color.White), xSpace - 10 + i * (pic.Width - 2 * xSpace) / xPart, pic.Height - ySapce + 10);
            }
            //画Y轴文字
            for (i = 1; i < yPart + 1; i++)
            {
                objGraphics.DrawString((yMaxLeft * i / yPart).ToString(), new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(Color.White), xSpace - 20, pic.Height - ySapce - i * (pic.Height - ySapce * 2) / yPart);
            }
            //双Y轴时,画第二条Y轴文字
            if (isTwoYAxis)
            {
                for (i = 1; i < yPart + 1; i++)
                {
                    objGraphics.DrawString(((double)((double)yMaxRight *(double) i /(double) yPart)).ToString(), new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(Color.White), pic.Width - xSpace + 10, pic.Height - ySapce - i * (pic.Height - ySapce * 2) / yPart);
                }

            }
            //画主标题
            objGraphics.DrawString(Title, new Font("黑体", titleFontSize, FontStyle.Regular), new SolidBrush(titleFontColor), (int)((pic.Width - titleFontSize * Title.Length) / 2), 10);
            //new Point((int)((pic.Width - titleFontSize * Title.Length) / 2), 10));
            //画X轴标题
            objGraphics.DrawString(xTitle, new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(axisFontColor), (pic.Width - xSpace - (int)(axisFontSize * xTitle.Length / 2)), (pic.Height - ySapce - axisFontSize - 5));
            //画Y轴标题
            objGraphics.DrawString(yTitleLeft, new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(axisFontColor), xSpace - (int)(axisFontSize * yTitleLeft.Length / 2), xSpace - axisFontSize - 5);
            //两条轴时,画第二个标题
            if (isTwoYAxis)
            {
                objGraphics.DrawString(yTitleRight, new Font("宋体", axisFontSize, FontStyle.Regular), new SolidBrush(axisFontColor), pic.Width - xSpace - (int)(axisFontSize * yTitleRight.Length / 2), xSpace - axisFontSize - 5);
            }
            p.Dispose();
        }
        /// <summary>
        /// 向曲线图中添加一个点
        /// </summary>
        /// <param name="mXValue">添加点的X坐标</param>
        /// <param name="mYValue">添加点的Y坐标</param>
        /// <param name="mIndex">当曲线数量大于1时,添加点的序号,从0开始</param>
        /// <param name="YAxis">当曲线有两条Y轴时,添加点的Y轴序号,左轴为1,右轴为2</param>
        public void AddPoint(int mXValue, double mYValue, int mIndex, int YAxis)
        {
            if (mXValue > xMax)//X值超横坐标最大值
                return;
            if ((YAxis == 1) && ((mYValue > yMaxLeft) || (mYValue < 0)))
            {
                if (mYValue > yMaxLeft)
                { mYValue = yMaxLeft; }
                else
                { mYValue = 0; }

            }
            if ((YAxis == 2) && ((mYValue > YMaxRight) || (mYValue < 0)))
            {
                if (mYValue > yMaxRight)
                { mYValue = yMaxRight; }
                else
                { mYValue = 0; }
            }
            Point realPoint = new Point(0, 0);
            realPoint.X = xSpace + mXValue * (pic.Width - 2 * xSpace) / xMax;
            if (YAxis == 1)
            {
                realPoint.Y = pic.Height - ySapce - (int)(mYValue * (pic.Height - 2 * ySapce) / yMaxLeft);
            }
            else if (YAxis == 2)
            {
                realPoint.Y = pic.Height - ySapce - (int)(mYValue * (pic.Height - 2 * ySapce) / yMaxRight);
            }
            else
            {
                return;
            }
            if (isFirstPoint[mIndex])
            {
                isFirstPoint[mIndex] = false;
                oldPoint[mIndex] = realPoint;
            }
            else
            {
                if (isShowTitle[mIndex])
                {
                    objGraphics.DrawLine(new Pen(CurveColor[mIndex], 2), oldPoint[mIndex].X, oldPoint[mIndex].Y, realPoint.X, realPoint.Y);
                }
            }
            oldPoint[mIndex] = realPoint;
            pic.Refresh();
        }
        /// <summary>
        /// 向曲线图中添加一组点
        /// </summary>
        /// <param name="mXValue">添加点的X轴坐标</param>
        /// <param name="mYValue">添加点的Y轴坐标数组</param>
        /// <param name="YAxis">当曲线有两条Y轴时,添加点的Y轴序号数组,左轴为1,右轴为2</param>
        public void AddPoint(int mXValue, double[] mYValue, int[] YAxis)
        {
            if ((mYValue.Length != CurveCount) || (YAxis.Length != CurveCount))
            {
                throw new Exception("传入数组维数不对");
            }
            int i;
            for (i = 0; i < CurveCount; i++)
            {
                AddPoint(mXValue, mYValue[i], i, YAxis[i]);
            }
        }

    }

}
