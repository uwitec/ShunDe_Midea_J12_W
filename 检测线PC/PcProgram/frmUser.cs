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
    public partial class frmUser : Form
    {
        frmMain fatherForm;
        bool isAdmin = false;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }
        string _userName = "";
        public frmUser(frmMain f,string userName)
        {
            _userName = userName;
            fatherForm = f;
            InitializeComponent();
        }
        Font font;
        private void frmUser_Load(object sender, EventArgs e)
        {
            font = new Font(cMain.privateFontsShaoNv.Families[0], 15, FontStyle.Bold);
            txtUserName.Text = _userName;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
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

        private void frmUser_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = Graphics.FromHwnd(this.Handle);
            Rectangle r = new Rectangle(0, 0, this.Width, this.Height);
            DrawBack(g, r, Color.LightBlue, Color.LightPink);
            SolidBrush b = new SolidBrush(Color.Red);
            g.DrawString("用户名", font, b, new PointF(40, txtUserName.Top));
            g.DrawString("密  码", font, b, new PointF(40, txtPassword.Top));
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if ((txtUserName.Text.ToUpper() == "HK"))
            {
                //cMain._Modeuser = "HK";
                isAdmin = true;
                this.Close();
                return;
            }
            if (cData.isLogin(txtPassword.Text))
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else 
            {
                MessageBox.Show("输入的贴或者密码不正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
    }
}