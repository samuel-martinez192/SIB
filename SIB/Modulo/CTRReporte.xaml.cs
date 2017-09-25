using System;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using SIB.EntityFramework;

namespace SIB.Modulo
{
    /// <summary>
    /// Interaction logic for CTRReporte.xaml
    /// </summary>
    public partial class CTRReporte : MetroWindow
    {
        public CTRReporte()
        {
            InitializeComponent();
        }

        public void crearReporte(string dataSourceName, string reportResource, object dataSourceValue)
        {
            var reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            reportDataSource1.Name = dataSourceName;
            reportDataSource1.Value = dataSourceValue;
            this._reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this._reportViewer.LocalReport.ReportEmbeddedResource = reportResource;
            this._reportViewer.RefreshReport();
        }
    }
}
