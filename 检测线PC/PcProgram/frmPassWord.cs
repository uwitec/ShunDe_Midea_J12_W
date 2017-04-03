using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace PcProgram
{
    public partial class frmPassWord : Form
    {
        Font font;
        public frmPassWord()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string sqlStr = string.Format("select * from Users where password='{0}'", txtPW1.Text);
            DataSet ds = cData.readData(sqlStr, cData.ConnMain);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (txtPW2.Text == txtPW3.Text)
                {
                    sqlStr = string.Format("delete from users where password='{0}'", txtPW1.Text);
                    cData.upData(sqlStr, cData.ConnMain);
                    sqlStr = string.Format("insert into users values('{0}')", txtPW2.Text);
                    int changeRow = cData.upData(sqlStr, cData.ConnMain);
                    if (changeRow > 0)
                    {
                        MessageBox.Show("密码修改成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("密码修改失败,请重新修改密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("两次密码输入不一致,请重新输入新密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPW2.Text = "";
                    txtPW3.Text = "";
                }
            }
            else
            {
                MessageBox.Show("原密码输入错误,请重新输入原密码", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPW1.Text = "";
                txtPW2.Text = "";
                txtPW3.Text = "";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPassWord_Load(object sender, EventArgs e)
        {
            font = new Font(cMain.privateFontsShaoNv.Families[0], 15, FontStyle.Bold);
        }

        private void frmPassWord_Paint(object sender, PaintEventArgs e)
        {

            Rectangle r = new Rectangle(0, 0, this.Width, this.Height);
            Graphics g = Graphics.FromHwnd(this.Handle);
            DrawBack(g, r, Color.LightBlue, Color.LightPink);
            SolidBrush b = new SolidBrush(Color.Red);
            g.DrawString("原密码", font, b, new PointF(40, txtPW1.Top));
            g.DrawString("新密码", font, b, new PointF(40, txtPW2.Top));
            g.DrawString("新密码", font, b, new PointF(40, txtPW3.Top));
        }
        static public void DrawBack(Graphics g, System.Drawing.Rectangle rect, Color startColor, Color endColor)
        {
            LinearGradientBrush brush = new LinearGradientBrush(rect,
            startColor,
            endColor,
            90.0f);

            float[] relativeIntensities =   { 0.0f, 0.3f, 1.0f };
            float[] relativePositions =   { 0.0f, 0.7f, 1.0f };

            Blend blend = new Blend();
            blend.Factors = relativeIntensities;
            blend.Positions = relativePositions;
            brush.Blend = blend;

            g.FillRectangle(brush, rect);
        }
    }
}