using CrystalDecisions.CrystalReports.Engine;
using Entity;
using System;
using System.IO;
using System.Windows.Forms;

namespace ReportViewer
{
    public partial class Viewer : Form
    {
        private Product _product;
        public Viewer()
        {
            InitializeComponent();
            
        }

        public void SetProduct(Product product)
        {
            this._product = product;
        }

        private void Viewer_Load(object sender, EventArgs e)
        {
            if(_product == null)
            {
                MessageBox.Show("No se ha especificado el producto");
            }
            else
            {
                string rptPath = Path.Combine(AppContext.BaseDirectory, "report.rpt");
                ReportDocument rpt = new ReportDocument();
                rpt.Load(rptPath);
                rpt.SetParameterValue("paramName", (string.IsNullOrEmpty(_product.Name)) ? "<SIN NOMBRE>" : _product.Name);
                rpt.SetParameterValue("paramDescription", (string.IsNullOrEmpty(_product.Description)) ? "<SIN DESCRIPCION>" : _product.Description);
                rpt.SetParameterValue("paramPrice", _product.Price);
                rpt.SetParameterValue("paramStock", _product.Stock);
                crystalReportViewer.ReportSource = rpt;
            }
        }
    }
}
