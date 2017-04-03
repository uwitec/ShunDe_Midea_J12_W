using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NewMideaProgram
{
    public partial class frmBarSet : Form
    {
        public frmBarSet()
        {
            InitializeComponent();
        }

        private void frmBarSet_Load(object sender, EventArgs e)
        {
            initFrm();
            DataClsToFrm(cMain.mBarSet);
        }
        private void initFrm()
        {
            if (!cMain.isComPuter)
            {
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
            }
            //lblTitle.Left = (this.Width - lblTitle.Width) / 2;
            //cMain.initFrom(this.Controls);
            cMain.initFrom(panel3.Controls);
            //cMain.initFrom(panel1.Controls);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static bool DataClsToTxt(cBarSet barSet)
        {
            bool returnValue = false;
            try
            {
                cXml.saveXml(cMain.AppPath + "\\BarCodeSet.xml", typeof(cBarSet), barSet);
                returnValue = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("FrmBarSet DataClsToTxt is Error " + exc.Message);
                returnValue = false;
            }
            return returnValue;
        }
        public static bool DataTxtToCls(string filePath,out cBarSet barSet)
        {
            bool returnValue = false;
            cBarSet mBarSet = new cBarSet();
            try
            {
                mBarSet = (cBarSet)cXml.readXml(cMain.AppPath + "\\BarCodeSet.xml",typeof(cBarSet), mBarSet);
                returnValue = true;
            }
            catch (Exception exc)
            {
                returnValue = false;
                cMain.WriteErrorToLog("frmBarSet DataTxtToCls is Error " + exc.Message);
            }
            barSet = mBarSet;
            return returnValue;
        }
        private bool DataFrmToCls(out cBarSet barSet)
        {
            bool returnValue = false;
            cBarSet mBarSet = new cBarSet();
            try
            {
                mBarSet.mIsWinCeBar = rbtWince.Checked ? true : false;
                mBarSet.mIsAutoStart = chkAutoStart.Checked ? true : false;

                for (int i = 0; i < 10; i++)
                {
                    mBarSet.mIsUse[i] = Num.BoolParse(BarCodeSet.Rows[i].Cells[0].Value);
                    mBarSet.mIntBarLength[i] = Num.IntParse(BarCodeSet.Rows[i].Cells[1].Value);
                    mBarSet.mIntBarStart[i] = Num.IntParse(BarCodeSet.Rows[i].Cells[2].Value);
                    mBarSet.mIntBarCount[i] = Num.IntParse(BarCodeSet.Rows[i].Cells[3].Value);
                }
                returnValue = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmBarSet DataFrmToCls is Error " + exc.Message);
            }
            barSet = mBarSet;
            return returnValue;
        }
        private bool DataClsToFrm(cBarSet barSet)
        {
            bool returnValue = false;
            try
            {
                if (barSet.mIsWinCeBar)
                {
                    rbtWince.Checked = true;
                }
                else
                {
                    rbtComputer.Checked = true;
                }

                if (barSet.mIsAutoStart)
                {
                    chkAutoStart.Checked = true;
                }
                else
                {
                    chkAutoStart.Checked = false;
                }

                for (int i = 0; i < 10; i++)
                {
                    BarCodeSet.Rows[i].Cells[0].Value = barSet.mIsUse[i];
                    BarCodeSet.Rows[i].Cells[1].Value = barSet.mIntBarLength[i];
                    BarCodeSet.Rows[i].Cells[2].Value = barSet.mIntBarStart[i];
                    BarCodeSet.Rows[i].Cells[3].Value = barSet.mIntBarCount[i];
                }
                returnValue = true;
            }
            catch (Exception exc)
            {
                cMain.WriteErrorToLog("frmBarSet DataClsToFrm is Error " + exc.Message);
            }
            return returnValue;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BarCodeSet.EndEdit();
            if (DataFrmToCls(out cMain.mBarSet))
            {
                if (DataClsToTxt(cMain.mBarSet))
                {
                    MessageBox.Show("条码设置保存成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            MessageBox.Show("条码设置保存失败", "失败", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1); 
        }
    }
}