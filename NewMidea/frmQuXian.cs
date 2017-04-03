using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace NewMideaProgram
{
    public partial class frmQuXian : Form
    {
        //bool isShangShuoQuXian = false;
        //int indexQuXianSelect = 0;
        //int countQuXianShangShuo = 15;
        uint[] QuXianColor = new uint[] { 0xFF, 0xFFFF, 0xFF0000, 0xFF00 };
        string FileName = "";
        public frmQuXian(string fileName,string openName)
        {
            FileName = fileName;
            this.Text = openName;
            InitializeComponent();
        }

        private void frmQuXian_Load(object sender, EventArgs e)
        {
            //axiPlotX1.get_Channel(0).TitleText = "电流";
            //axiPlotX1.get_Channel(1).TitleText = "功率";
            //axiPlotX1.get_Channel(2).TitleText = "压力";
            //axiPlotX1.get_Channel(3).TitleText = "频率";
            //axiPlotX1.get_XAxis(0).Span = 0.1 / 24;
            //if (File.Exists(FileName))
            //{
            //    axiPlotX1.LoadDataFromFile(FileName);
            //}
        }
        //private void axiPlotX1_OnGotFocusChannel(object sender, AxiPlotLibrary.IiPlotXEvents_OnGotFocusChannelEvent e)
        //{
        //    uint tmpColor = axiPlotX1.get_Channel(e.index).Color;
        //    for (int i = 0; i < axiPlotX1.YAxisCount; i++)
        //    {
        //        axiPlotX1.get_YAxis(i).Visible = false;
        //        axiPlotX1.get_YAxis(i).GridLinesVisible = false;
        //    }
        //    axiPlotX1.get_YAxis(e.index).Visible = true;
        //    axiPlotX1.get_YAxis(e.index).GridLinesVisible = true;
        //    axiPlotX1.get_Channel(e.index).Color = 0;
        //    axiPlotX1.get_Channel(e.index).Color = tmpColor;
        //    axiPlotX1.get_Channel(e.index).Color = 0;
        //    axiPlotX1.get_Channel(e.index).Color = tmpColor;

        //    if (axiPlotX1.DataCursorCount > 0)
        //    {
        //        axiPlotX1.get_DataCursor(0).Style = iPlotLibrary.TxiPlotDataCursorStyle.ipcsValueY;
        //        axiPlotX1.get_DataCursor(0).ChannelName = axiPlotX1.get_Channel(e.index).Name;
        //    }
        //    QuXianShangShuo.Enabled = true;
        //    indexQuXianSelect = e.index;
        //    isShangShuoQuXian = true;
        //    countQuXianShangShuo = 0;
        //}

        private void QuXianShangShuo_Tick(object sender, EventArgs e)
        {
            //int i = indexQuXianSelect;
            //if (isShangShuoQuXian)
            //{
            //    if (countQuXianShangShuo <= 15)
            //    {
            //        if (countQuXianShangShuo == 0)
            //        {
            //            for (int j = 0; j < axiPlotX1.ChannelCount; j++)
            //            {
            //                axiPlotX1.get_Channel(j).Color = QuXianColor[j];
            //            }
            //        }
            //        if ((countQuXianShangShuo % 2) == 0)
            //        {
            //            axiPlotX1.get_Channel(i).Color = 0xFFFFFF;
            //        }
            //        else
            //        {
            //            axiPlotX1.get_Channel(i).Color = QuXianColor[i];
            //        }
            //        countQuXianShangShuo++;
            //    }
            //    else
            //    {
            //        QuXianShangShuo.Enabled = false;
            //        isShangShuoQuXian = false;
            //        for (int j = 0; j < axiPlotX1.ChannelCount; j++)
            //        {
            //            axiPlotX1.get_Channel(j).Color = QuXianColor[j];
            //        }
            //    }
            //}
        }
    }
}