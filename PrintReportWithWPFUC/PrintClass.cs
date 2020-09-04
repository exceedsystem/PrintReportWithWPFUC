// PrintClass.cs
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace PrintReportWithWPFUC
{
    internal class PrintClass
    {
        // Default paper size = A4(8.27inch * 96dpi, 11.69inch * 96dpi)
        private const double DEFAULT_PAPER_SIZE_A4_S = 8.27 * 96;

        private const double DEFAULT_PAPER_SIZE_A4_L = 11.69 * 96;

        // Print method
        public void Print(ReportData data)
        {
            // Select the printer for printing a report from the print dialog
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                // Obtain capabilities of the selected printer
                var pcaps = printDialog.PrintQueue.GetPrintCapabilities();

                var pticket = printDialog.PrintTicket;

                // Supported page orientation is portrait and landscape only in this sample
                double reportWidth = 0, reportHeight = 0;
                switch (pticket.PageOrientation)
                {
                    case System.Printing.PageOrientation.Portrait:
                        reportWidth = pticket.PageMediaSize.Width ?? DEFAULT_PAPER_SIZE_A4_S;
                        reportHeight = pticket.PageMediaSize.Height ?? DEFAULT_PAPER_SIZE_A4_L;
                        break;

                    case System.Printing.PageOrientation.Landscape:
                        reportWidth = pticket.PageMediaSize.Height ?? DEFAULT_PAPER_SIZE_A4_L;
                        reportHeight = pticket.PageMediaSize.Width ?? DEFAULT_PAPER_SIZE_A4_S;
                        break;
                }

                // Create a new single page that the same size as the size of the selected paper
                var page = new FixedPage()
                {
                    Width = reportWidth,
                    Height = reportHeight,
                    // Apply margins of the selected printer to the page
                    Margin = new Thickness(
                        pcaps.PageImageableArea.OriginWidth,
                        pcaps.PageImageableArea.OriginHeight,
                        0,
                        0),
                };

                // Create the report image with the ReportControl and align it to the page size
                var vb = new Viewbox
                {
                    Width = reportWidth,
                    Height = reportHeight,
                    StretchDirection = StretchDirection.Both,
                    Stretch = Stretch.Uniform,
                    Child = new ReportControl { Width = DEFAULT_PAPER_SIZE_A4_S, Height = DEFAULT_PAPER_SIZE_A4_L, DataContext = data }
                };

                // Create a new fixed document and add the page
                page.Children.Add(vb);
                var content = new PageContent();
                ((IAddChild)content).AddChild(page);
                var fixedDoc = new FixedDocument();
                fixedDoc.Pages.Add(content);

                // Print the document on the selected printer
                printDialog.PrintDocument(fixedDoc.DocumentPaginator, "Report print sample");
            }
        }
    }
}