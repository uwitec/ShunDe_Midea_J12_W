using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;
using System.Drawing.Imaging;
using System.IO;
namespace NewMideaProgram
{
    public partial class cPrint
    {
        public void SetValue(cAllResult AllResult)
        {
            if (AllResult == null)
            {
                return;
            }
            LocalReport report = new LocalReport();
            //设置需要打印的报表的文件名称。
            report.ReportPath = ".\\rptReport.rdlc";
            //创建要打印的数据源
            #region//填充数据
            List<Microsoft.Reporting.WinForms.ReportParameter> allValue = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            if (AllResult.RunResult.mBar.Length >= 8)
            {
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", AllResult.RunResult.mBar.Substring(AllResult.RunResult.mBar.Length - 8)));
            }
            else
            {
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter("Barcode", AllResult.RunResult.mBar));
            }
            allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter("TestTime", AllResult.RunResult.mTestTime.ToString("yyyy-MM-dd HH:mm:ss")));
            allValue.Add(new ReportParameter("TestNo", AllResult.RunResult.mTestNo.ToString()));
            allValue.Add(new ReportParameter("TestResult", AllResult.RunResult.mIsPass.ToString()));
            float[] tmpHot = new float[cMain.DataShow];
            float[] tmpCol = new float[cMain.DataShow];
            bool[] tmpHotResult=new bool[cMain.DataShow];
            bool[] tmpColResult=new bool[cMain.DataShow];
            int hotIndex = -1, coldIndex = -1;
            for (int i = 0; i < cModeSet.StepCount; i++)
            {
                if (AllResult.ModeSet.mStepId[i] == "制热" && AllResult.ModeSet.mSetTime[i] > 0)
                {
                    hotIndex = i;
                }
                if (AllResult.ModeSet.mStepId[i] == "制冷" && AllResult.ModeSet.mSetTime[i] > 0)
                {
                    coldIndex = i;
                }
            }
            if (hotIndex >= 0 && hotIndex < cModeSet.StepCount)
            {
                for (int i = 0; i < tmpHot.Length; i++)
                {
                    tmpHot[i] = (float)AllResult.StepResult[hotIndex].mData[i];
                    tmpHotResult[i]=(AllResult.StepResult[hotIndex].mIsDataPass[i]!=0);
                }
            }
            else
            {
                for (int i = 0; i < tmpHot.Length; i++)
                {
                    tmpHot[i] = 0;
                    tmpHotResult[i]=true;
                }
            }
            if (coldIndex >= 0 && coldIndex < cModeSet.StepCount)
            {
                for (int i = 0; i < tmpCol.Length; i++)
                {
                    tmpCol[i] = (float)AllResult.StepResult[coldIndex].mData[i];
                    tmpColResult[i] = (AllResult.StepResult[coldIndex].mIsDataPass[i] != 0);
                }
            }
            else
            {
                for (int i = 0; i < tmpCol.Length; i++)
                {
                    tmpCol[i] = 0;
                    tmpColResult[i]=true;
                }
            }
            string hotStr = "HotVol,HotCur,HotPower,HotPressIn,HotPressOut,HotTempIn,HotTempOut,HotTempDiff,HotPiepIn,HotPiepOut,HotPiepDiff,HotHz,HotT1,HotT2,HotT3,HotSpeed,HotWuLiao,HotBanBen";
            string colStr = "ColVol,ColCur,ColPower,ColPressIn,ColPressOut,ColTempIn,ColTempOut,ColTempDiff,ColPiepIn,ColPiepOut,ColPiepDiff,ColHz,ColT1,ColT2,ColT3,ColSpeed,ColWuLiao,ColBanBen";
            string[] tmp = hotStr.Split(',');
            for (int i = 0; i < tmp.Length; i++)
            {
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter(tmp[i], string.Format("{0:F2}", tmpHot[i])));
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter(string.Format("{0}Result", tmp[i]), tmpHotResult[i].ToString()));
            }
            tmp = colStr.Split(',');
            for (int i = 0; i < tmp.Length; i++)
            {
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter(tmp[i], string.Format("{0:F2}", tmpCol[i])));
                allValue.Add(new Microsoft.Reporting.WinForms.ReportParameter(string.Format("{0}Result", tmp[i]), tmpColResult[i].ToString()));
            }
            #endregion
            report.SetParameters(allValue);
            //刷新报表中的需要呈现的数据
            report.Refresh();
            PrintStream(report);
        }
        /// <summary>
        /// 用来记录当前打印到第几页了
        /// </summary>
        //private int m_currentPageIndex;
        /// <summary>
        /// 声明一个Stream对象的列表用来保存报表的输出数据,LocalReport对象的Render方法会将报表按页输出为多个Stream对象。
        /// </summary>
        private IList<Stream> m_streams;
        private bool isLandSapces = false;
        /// <summary>
        /// 用来提供Stream对象的函数，用于LocalReport对象的Render方法的第三个参数。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileNameExtension"></param>
        /// <param name="encoding"></param>
        /// <param name="mimeType"></param>
        /// <param name="willSeek"></param>
        /// <returns></returns>
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            //如果需要将报表输出的数据保存为文件，请使用FileStream对象。
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        /// <summary>
        /// 为Report.rdlc创建本地报告加载数据,输出报告到.emf文件,并打印,同时释放资源
        /// </summary>
        /// <param name="rv">参数:ReportViewer.LocalReport</param>
        public void PrintStream(LocalReport rvDoc)
        {
            //获取LocalReport中的报表页面方向
            isLandSapces = rvDoc.GetDefaultPageSettings().IsLandscape;
            Export(rvDoc);
            PrintSetting();
            Dispose();
        }
        private void Export(LocalReport report)
        {
            string deviceInfo =
            @"<DeviceInfo>
                 <OutputFormat>EMF</OutputFormat>
             </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            //将报表的内容按照deviceInfo指定的格式输出到CreateStream函数提供的Stream中。
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;
        }

        private void PrintSetting()
        {
            if (m_streams == null || m_streams.Count == 0)
                return;
                //throw new Exception("错误:没有检测到打印数据流");
            //声明PrintDocument对象用于数据的打印
            PrintDocument printDoc = new PrintDocument();
            //获取配置文件的清单打印机名称
            System.Configuration.AppSettingsReader appSettings = new System.Configuration.AppSettingsReader();
            //printDoc.PrinterSettings.PrinterName = reportViewer1.PrinterSettings.PrinterName;
            printDoc.PrintController = new System.Drawing.Printing.StandardPrintController();//指定打印机不显示页码 
            //判断指定的打印机是否可用
            if (!printDoc.PrinterSettings.IsValid)
            {
                return;
                //throw new Exception("错误:找不到打印机");
            }
            else
            {
                //设置打印机方向遵从报表方向
                printDoc.DefaultPageSettings.Landscape = isLandSapces;
                //声明PrintDocument对象的PrintPage事件，具体的打印操作需要在这个事件中处理。
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                //m_currentPageIndex = 0;
                //设置打印机打印份数
                printDoc.PrinterSettings.Copies = 1;
                //执行打印操作，Print方法将触发PrintPage事件。
                printDoc.Print();
            }
        }
        /// <summary>
        /// 处理程序PrintPageEvents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ev"></param>
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //Metafile对象用来保存EMF或WMF格式的图形，
            //我们在前面将报表的内容输出为EMF图形格式的数据流。
            Metafile pageImage = new Metafile(m_streams[0]);

            //调整打印机区域的边距
            System.Drawing.Rectangle adjustedRect = new System.Drawing.Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            //绘制一个白色背景的报告
            //ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            //获取报告内容
            //这里的Graphics对象实际指向了打印机
            ev.Graphics.DrawImage(pageImage, adjustedRect);
            //ev.Graphics.DrawImage(pageImage, ev.PageBounds);

            // 准备下一个页,已确定操作尚未结束
            //m_currentPageIndex++;

            //设置是否需要继续打印
            //ev.HasMorePages = false;
        }
        private void Dispose()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
