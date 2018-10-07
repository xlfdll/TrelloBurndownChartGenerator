using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Wpf;
using System.IO;
using System.Windows;

namespace TrelloBurndownChartGenerator
{
    /// <summary>
    /// Interaction logic for PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : Window
    {
        public PlotWindow()
        {
            InitializeComponent();
        }

        public PlotWindow(PlotModel plotModel)
            : this()
        {
            MainPlotView.Model = plotModel;
        }

        private void SaveGraphButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                Filter = "Portable Network Graphics (*.png)|*.png|Portable Document Format (*.pdf)|*.pdf|Scalable Vector Graphics (*.svg)|*.svg",
                RestoreDirectory = true
            };

            if (dlg.ShowDialog() == true)
            {
                if (File.Exists(dlg.FileName))
                {
                    try
                    {
                        File.Delete(dlg.FileName);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Cannot access the file " + dlg.FileName + " because it is being used by another process.",
                            Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                switch (dlg.FileName.Substring(dlg.FileName.LastIndexOf('.')))
                {
                    case ".png":
                        PngExporter pngExporter = new PngExporter();

                        using (FileStream fileStream = File.OpenWrite(dlg.FileName))
                        {
                            pngExporter.Export(MainPlotView.Model, fileStream);
                        }

                        break;
                    case ".pdf":
                        PdfExporter pdfExporter = new PdfExporter();

                        using (FileStream fileStream = File.OpenWrite(dlg.FileName))
                        {
                            pdfExporter.Export(MainPlotView.Model, fileStream);
                        }

                        break;
                    case ".svg":
                        OxyPlot.SvgExporter svgExporter = new OxyPlot.SvgExporter();

                        using (FileStream fileStream = File.OpenWrite(dlg.FileName))
                        {
                            svgExporter.Export(MainPlotView.Model, fileStream);
                        }

                        break;
                    default:
                        break;
                }
            }
        }
    }
}
