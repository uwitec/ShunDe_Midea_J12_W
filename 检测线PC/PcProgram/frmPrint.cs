using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace PcProgram
{
    public partial class frmPrint : Form
    {
        DataTable reportDataSet;
        public frmPrint(DataTable crystalReportDataSet)
        {
            reportDataSet = crystalReportDataSet;
            InitializeComponent();
            //ReportDocument doc = new ReportDocument();
            //doc.Load(cMain.path + "\\Report\\ErrorReport.rpt");
            //doc.SetDataSource(reportDataSet);
            //this.crystalReportViewer1.ReportSource = doc;
        }

        private void Print_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            dsPrint ds = new dsPrint();
            DataRow dataRow = reportDataSet.Rows[0];
            DataRow dr = ds.DTPrint.NewRow();
            dr["BarCode"] = dataRow["BarCode"];
            dr["Mode"] = dataRow["Mode"];
            dr["TestNo"] = dataRow["TestNo"];
            for (int i = 0; i < cMain.DataShow; i++)
            {
                dr[string.Format("Data{0}", i + 1)] = dataRow[string.Format("Data{0}", i + 1)];
                dr[string.Format("DataUp{0}", i + 1)] = dataRow[string.Format("DataUp{0}", i + 1)];
                dr[string.Format("DataDown{0}", i + 1)] = dataRow[string.Format("DataDown{0}", i + 1)];
            }
            ds.DTPrint.Rows.Add(dr);
            CrystalReport1 cr1 = new CrystalReport1();
            cr1.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr1;
            
            //crystalReportViewer1.PrintReport();

            cr1.PrintToPrinter(1, true, 0, 0);
            this.Close();
        }
    }
}