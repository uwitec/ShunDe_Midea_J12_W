using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace System
{
    //[ToolboxBitmap(@"D:\old\工具\icon\mf.ico")]
    [ToolboxBitmap(typeof(loopCon), "loopCon.ico")]
    public partial class loopCon : UserControl
    {
        public loopCon()
        {
            InitializeComponent();
        }
        //属性
        bool arrowVisiable = true;//箭头是否可见
        [Browsable(true), Category("台车设置"), Description("旋转方向箭头是否可见")]
        public bool ArrowVisiable
        {
            get { return arrowVisiable; }
            set { arrowVisiable = value; LabelPaint(); }
        }
        int stationCount = 33;
        [Browsable(true), Category("台车设置"), Description("环线上台车的总数量")]
        public int StationCount//总的台车数量 
        {
            get { return stationCount; }
            set { stationCount = value; LabelPaint(); }
        }
        int lengthCount = 14;//环形线边长上台车上的数量
        [Browsable(true), Category("台车设置"), Description("环线边长上的台车数量")]
        public int LengthCount
        {
            get { return lengthCount; }
            set { lengthCount = value; LabelPaint(); }
        }
        Color picBackColor = Color.Sienna;//图片的背景颜色
        [Browsable(true), Category("台车设置"), Description("台车控件的背景色")]
        public Color PicBackColor
        {
            get { return picBackColor; }
            set { picBackColor = value; LabelPaint(); }
        }
        Color labBackColor = Color.LightBlue;//标签背景颜色
        [Browsable(true), Category("台车设置"), Description("台车号标签的背景色")]
        public Color LabBackColor
        {
            get { return labBackColor; }
            set { labBackColor = value; LabelPaint(); }
        }
        int boardIndex = 1;//行程开关的初始台车号
        [Browsable(true), Category("台车设置"), Description("行程开关初始台车号(绝对位置)")]
        public int BoardIndex
        {
            get { return boardIndex; }
            set { if (value <= stationCount)boardIndex = value; barSearch = boardIndex; LabelPaint(); }
        }
        int machineIn = 1;
        [Browsable(true), Category("台车设置"), Description("台车进线位置(绝对位置)")]
        public int MachineIn
        {
            get { return machineIn; }
            set { machineIn = value; LabelPaint(); }
        }
        int machineOut=2;
        [Browsable(true), Category("台车设置"), Description("台车出线位置(绝对位置)")]
        public int MachineOut
        {
          get { return machineOut; }
            set { machineOut = value; LabelPaint(); }
        }
        int barSearch = 3;
        turnMethod _TurnMethod = turnMethod.byTime;//环形线旋转方法
        [Browsable(true), Category("台车设置"), Description("环形线旋转方法")]
        public turnMethod TurnMethod
        {
            get { return _TurnMethod; }
            set { _TurnMethod = value; LabelPaint(); }
        }
        turnMethod _LabelTurnMethod = turnMethod.byTime;//台车标签放置方法
        [Browsable(true), Category("台车设置"), Description("台车标签旋转方法")]
        public turnMethod LabelTurnMethod
        {
            get { return _LabelTurnMethod; }
            set { _LabelTurnMethod = value; LabelPaint(); }
        }
        //变量
        public enum turnMethod : int//环形线旋转方式
        { byTime = 0, notByTime = 1 }
        public struct labelLocation//标签位置
        { public int X, Y;}
        public string ErrStr = "";//错误信息
        int Rround, Llength;//Rround转弯处圆半径,Llength边长
        int firstPoint, secondPoint, thirdPoint;//定义三个分隔点,分别为椭圆轨道上边长最右边,下边长最右边,下边长最左边
        int labelHeight, labelWidth, labelSpace;//定义标签的长,高,两个标签的间隔,三个长度的比例为16:9:2
        double intBase;//定义比例的基数
        bool isOnDraw = true;//定义是否须要重绘台车标签
        int BoardLabelIndex = 0;//记录0号台车所在的台车位 0～stationCount-1
        labelLocation[] labelXY = new labelLocation[100];//存放台车号标签的位置
        Label[] taiChe = new Label[100];//初始化定义100个台车
        //控件
        //事件
        public event EventHandler LabelClick;//台车标签单击事件
        /// <summary>
        /// 台车标签单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Onclick(object sender, EventArgs e)
        {
            if (LabelClick != null)
            {
                LabelClick(sender, e);
            }
        }
        //方法
        private void labelOnDraw()//重绘台车标签位置
        {
            BoardLabelIndex = boardIndex-1;//初始位置
            int index;
            for (index = 0; index < stationCount; index++)
            {
                taiChe[index] = new Label();
                taiChe[index].Click += new EventHandler(Onclick);
                taiChe[index].Size = new Size(labelWidth, labelHeight);
                taiChe[index].BackColor = labBackColor;
                taiChe[index].Name = "Che" + index.ToString();
                taiChe[index].Tag = index.ToString();
                taiChe[index].Text = (index + 1).ToString();
                taiChe[index].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                taiChe[index].TabIndex = index;
                taiChe[index].Font = new Font(new FontFamily("黑体"), 18);
                pictureBox1.Controls.Add(taiChe[index]);
                if (_LabelTurnMethod == turnMethod.byTime)
                {
                    taiChe[index].Location = new Point(labelXY[(index + BoardLabelIndex) % stationCount].X, labelXY[(index + BoardLabelIndex) % stationCount].Y);
                }
                else
                {
                    taiChe[index].Location = new Point(labelXY[(stationCount - index + BoardLabelIndex) % stationCount].X, labelXY[(stationCount - index + BoardLabelIndex) % stationCount].Y);
                }

            }
        }
        private void timer1_Tick(object sender, EventArgs e)//时间控制旋转小标签移动
        {
            lblRightOne.Left = lblRightOne.Left + 1;
            if (lblRightOne.Left > pictureBox1.Right - 40 - Rround)
                lblRightOne.Left = 40 + Rround;
            lblRightTwo.Left = lblRightTwo.Left + 1;
            if (lblRightTwo.Left > pictureBox1.Right - Rround - 40)
                lblRightTwo.Left = 40 + Rround;
            lblLeftOne.Left = lblLeftOne.Left - 1;
            if (lblLeftOne.Left < 40 + Rround)
                lblLeftOne.Left = pictureBox1.Right - 40 - Rround;
            lblLeftTwo.Left = lblLeftTwo.Left - 1;
            if (lblLeftTwo.Left < 40 + Rround)
                lblLeftTwo.Left = pictureBox1.Right - 40 - Rround;
        }
        public void labelChangeColor(Color[] AllColor)//改变台车背景色
        {
            if (AllColor.GetLength(0) != stationCount)
            { ErrStr = "颜色数组 元素个数不符;" + ErrStr; }
            int index;
            for (index = 0; index < AllColor.GetLength(0); index++)
            {
                taiChe[index].BackColor = AllColor[index];
            }
        }
        /// <summary>
        /// 重载 改变台车背景色
        /// </summary>
        /// <param name="labelIndex">从0~staticCount的台车号</param>
        /// <param name="ColorIndex">颜色</param>
        public void labelChangeColor(int labelIndex, Color ColorIndex)//重载 改变台车背景色
        {
            taiChe[labelIndex].BackColor = ColorIndex;
        }
        public void labelTurn()//1号台车按环线旋转方向移动一个位置
        {
            int index;
            if (_TurnMethod == turnMethod.byTime)
            {
                BoardLabelIndex = (BoardLabelIndex + 1) % stationCount;
            }
            else
            {
                BoardLabelIndex = (BoardLabelIndex - 1 + stationCount) % stationCount;
            }
            if (_LabelTurnMethod == turnMethod.byTime)
            {
                for (index = 0; index < stationCount; index++)
                {
                    taiChe[index].Location = new Point(labelXY[(BoardLabelIndex + index) % stationCount].X, labelXY[(BoardLabelIndex + index) % stationCount].Y);
                }
            }
            else
            {
                for (index = 0; index < stationCount; index++)
                {
                    taiChe[index].Location = new Point(labelXY[(BoardLabelIndex + stationCount - index) % stationCount].X, labelXY[(BoardLabelIndex + stationCount - index) % stationCount].Y);
                }
            }
        }
        /// <summary>
        /// 初始化第一次在行程开关挡板的台车,LabelIndex从0到stationCount-1
        /// </summary>
        /// <param name="LabelIndex"></param>
        public void labelInit(int LabelIndex)//初始化第一次在行程开关挡板的台车,LabelIndex从1到stationCount
        {
            LabelIndex = LabelIndex % stationCount;
            //isOnDraw = false;//将标签重绘关闭
            int index;
            for (index = 0; index < stationCount; index++)
            {
                if (_LabelTurnMethod == turnMethod.byTime)//标签旋转方向
                {
                    BoardLabelIndex = (stationCount + boardIndex - LabelIndex - 1) % stationCount;
                    taiChe[index].Location = new Point(labelXY[(stationCount + index - LabelIndex + boardIndex - 1) % stationCount].X, labelXY[(stationCount + index - LabelIndex + boardIndex - 1) % stationCount].Y);
                    //taiChe[index].Location = new Point(labelXY[(stationCount + index - LabelIndex + 1) % stationCount].X, labelXY[(stationCount + index - LabelIndex + 1) % stationCount].Y);
                }
                else
                {
                    BoardLabelIndex = boardIndex + LabelIndex - 1;
                    taiChe[index].Location = new Point(labelXY[(stationCount - index + LabelIndex + boardIndex - 1) % stationCount].X, labelXY[(stationCount - index + LabelIndex + boardIndex - 1) % stationCount].Y);
                    //taiChe[index].Location = new Point(labelXY[(stationCount - index + LabelIndex - 1) % stationCount].X, labelXY[(stationCount - index + LabelIndex - 1) % stationCount].Y);
                }
            }
        }

        private void LabelPaint()
        {
            taiChe = null;
            labelXY = null;
            labelXY = new labelLocation[stationCount];//重定义,台车标签位置坐标 数组大小
            taiChe = new Label[stationCount]; //重定义台车数量
            pictureBox1.Controls.Clear();
            pictureBox1.BackColor = picBackColor;
            firstPoint = lengthCount;//用三个点来将椭圆轨道的两第边长和两个弧长上的台车数量计算出来,第一个点为椭圆轨道上边长数量
            if (stationCount % 2 == 0)//台车数量为偶数时 椭圆轨道两条边长上台车数量相等
            {
                secondPoint = (stationCount - 2 * lengthCount) / 2 + lengthCount + 1;//第二个点为上边长加上右弧长的数量+1,即为下边长最右边
                thirdPoint = secondPoint + lengthCount - 1;//第三个为下边长最左边
            }
            else//台车数量为奇数时,椭圆轨道下边长比上连长数量多一台.
            {
                secondPoint = (stationCount - 2 * lengthCount - 1) / 2 + lengthCount + 1;
                thirdPoint = secondPoint + lengthCount;
            }
            Rround = (pictureBox1.Height - 40) / 2 - 10;//椭圆轨道的半圆半径 为 总高度减去 上下各40的空余位置,减去10的标签高度
            Llength = (pictureBox1.Width - 40 - 2 * Rround - 50);//椭圆轨道的边长为 总长度送去左右40的空余位置,送去2个半径,送去30的标签长度
            intBase = ((double)(Llength) / (double)(lengthCount)) / 25;//放大比例,即标签的长宽和间隔成比例放大 缩小
            labelWidth = (int)((double)16 * intBase);
            labelHeight = (int)((double)12 * intBase);
            labelSpace = (int)((double)9 * intBase);
            for (int index = 1; index <= stationCount; index++)
            {
                if (index <= firstPoint)//第一段直线
                {
                    //台车位置
                    labelXY[index - 1].X = 20 + 8 * (int)intBase + Rround + (int)((double)25 * intBase * (double)(index - 1));
                    labelXY[index - 1].Y = 25;
                    //旋转方向箭头
                    if (_TurnMethod == turnMethod.byTime)
                    {
                        lblRightOne.Top = labelXY[0].Y + labelWidth + lblRightOne.Height - 15;
                        lblRightOne.Left = 20 + 8 * (int)intBase + Rround;
                        lblRightTwo.Top = lblRightOne.Top;
                        lblRightTwo.Left = pictureBox1.Width / 2;
                    }
                    else
                    {
                        lblLeftOne.Top = labelXY[0].Y + labelWidth + lblRightOne.Height - 15;
                        lblLeftOne.Left = 20 + 8 * (int)intBase + Rround;
                        lblLeftTwo.Top = lblLeftOne.Top;
                        lblLeftTwo.Left = pictureBox1.Width / 2;
                    }
                    if (index == machineIn)
                    {
                        lblIn.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblIn.Height * 1.4));
                    }
                    if (index == machineOut)
                    {
                        lblOut.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblOut.Height * 1.4));
                    }
                    if (index == barSearch)
                    {
                        lblStaticBar.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y + labelHeight * 1.2)); ;
                    }

                }
                else
                {
                    if (index < secondPoint)//第一个圆弧
                    {
                        double jiaoDu;
                        jiaoDu = 3.14 * (index - firstPoint) / (secondPoint - firstPoint);
                        labelXY[index - 1].X = 20 + Rround + Llength + (int)(System.Math.Sin(jiaoDu) * Rround);
                        labelXY[index - 1].Y = 25 + Rround - (int)(System.Math.Cos(jiaoDu) * Rround);
                        if (index == machineIn)
                        {
                            lblIn.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblIn.Height * 1.4));
                        }
                        if (index == machineOut)
                        {
                            lblOut.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblOut.Height * 1.4));
                        }
                        if (index == barSearch)
                        {
                            lblStaticBar.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y + labelHeight * 1.2)); ;
                        }
                    }
                    else
                    {
                        if (index <= thirdPoint)//第二段直线
                        {
                            if ((thirdPoint - secondPoint) == (firstPoint - 1))
                            {
                                labelXY[index - 1].X = 20 + Rround + 8 * (int)intBase + (int)((double)25 * intBase * (double)(thirdPoint - index));
                                labelXY[index - 1].Y = this.Height - 15 - labelHeight;
                            }
                            else
                            {
                                labelXY[index - 1].X = 20 + Rround + (int)((double)18 * intBase * (double)(thirdPoint - index));
                                labelXY[index - 1].Y = this.Height - 15 - labelHeight;
                            }

                            if (_TurnMethod == turnMethod.byTime)
                            {
                                lblLeftOne.Top = 2 * Rround;
                                lblLeftOne.Left = 20 + 8 * (int)intBase + Rround;
                                lblLeftTwo.Top = 2 * Rround;
                                lblLeftTwo.Left = pictureBox1.Width / 2;
                            }
                            else
                            {
                                lblRightOne.Top = 2 * Rround;
                                lblRightOne.Left = 20 + 8 * (int)intBase + Rround;
                                lblRightTwo.Top = 2 * Rround;
                                lblRightTwo.Left = pictureBox1.Width / 2;
                            }
                            if (index == machineIn)
                            {
                                lblIn.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y + labelHeight * 1.4));
                            }
                            if (index == machineOut)
                            {
                                lblOut.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y + labelHeight * 1.4));
                            }
                            if (index == barSearch)
                            {
                                lblStaticBar.Location = new Point((int)(labelXY[index - 1].X + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblIn.Height * 1.2));
                            }
                        }
                        else//第二个圆弧
                        {
                            double jiaoDu;
                            jiaoDu = 3.14 * (index - thirdPoint) / (stationCount - thirdPoint + 1);
                            labelXY[index - 1].X = 20 + Rround - (int)(System.Math.Sin(jiaoDu) * Rround);
                            labelXY[index - 1].Y = 15 + Rround + (int)(System.Math.Cos(jiaoDu) * Rround);
                            if (index == machineIn)
                            {
                                lblIn.Location = new Point((int)(labelXY[index - 1].X  + (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblIn.Height * 1.2));
                            }
                            if (index == machineOut)
                            {
                                lblOut.Location = new Point((int)(labelXY[index - 1].X+ (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y - lblOut.Height * 1.2));
                            }
                            if (index == barSearch)
                            {
                                lblStaticBar.Location = new Point((int)(labelXY[index - 1].X+ (labelWidth - lblIn.Width) / 2), (int)(labelXY[index - 1].Y + labelHeight * 1.4)); ;
                            }
                        }
                    }
                }
            }
            if (isOnDraw)//初始化时重绘
            {
                labelOnDraw();
            }
            if (arrowVisiable)//台车旋转小标签可见时,背景色和总背景色一致
            {
                lblLeftOne.Visible = true; lblRightOne.Visible = true; lblLeftTwo.Visible = true; lblRightTwo.Visible = true;
                lblLeftOne.BackColor = pictureBox1.BackColor;
                lblLeftTwo.BackColor = pictureBox1.BackColor;
                lblRightOne.BackColor = pictureBox1.BackColor;
                lblRightTwo.BackColor = pictureBox1.BackColor;
                lblIn.BackColor = pictureBox1.BackColor;
                lblOut.BackColor = pictureBox1.BackColor;
                lblStaticBar.BackColor = pictureBox1.BackColor;
            }
            else//台车旋转小标签不可见
            {
                lblLeftOne.Visible = false;
                lblLeftOne.Visible = false;
                lblLeftTwo.Visible = false;
                lblRightOne.Visible = false;
                lblRightTwo.Visible = false;
                lblStaticBar.Visible = false;
                lblIn.Visible = false;
                lblOut.Visible = false;
            }
        }

        private void loopCon_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            LabelPaint();
        }

    }
}