using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wpf.Ui.Controls;
using Wpf.Ui.Markup;

namespace GMSMAG.ViewModels.Pages
{
    class PrintViewModel
    {
        private const double PageWidth = 595; // A4 size width in points
        private const double PageHeight = 842; // A4 size height in points
        private const double Margin = 20;
        private const double HeaderHeight = 40;
        private const double RowHeight = 20;
        private const double ColumnWidth = 100;

        public async Task GenerateAndSavePdfAsync(Wpf.Ui.Controls.DataGrid dataGrid, string filePath)
        {
            // Access DataGrid properties on the UI thread
            var columns = Application.Current.Dispatcher.Invoke(() => dataGrid.Columns.ToList());
            var rowsCount = Application.Current.Dispatcher.Invoke(() => dataGrid.Items.Count);

            await Task.Run(() =>
            {
                // Create a new PDF document
                using (PdfDocument document = new PdfDocument())
                {
                    // Calculate the number of pages required
                    var rowsPerPage = CalculateRowsPerPage();
                    var totalPages = (int)Math.Ceiling(rowsCount / (double)rowsPerPage);

                    for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
                    {
                        // Create a new page
                        PdfPage page = document.AddPage();
                        page.Width = PageWidth;
                        page.Height = PageHeight;

                        // Create graphics object
                        XGraphics gfx = XGraphics.FromPdfPage(page);

                        // Draw header
                        DrawHeader(gfx);

                        // Access DataGrid items on the UI thread within the Task.Run
                        var rows = Application.Current.Dispatcher.Invoke(() =>
                        {
                            var list = new List<object>();
                            for (int i = pageIndex * rowsPerPage; i < Math.Min((pageIndex + 1) * rowsPerPage, rowsCount); i++)
                            {
                                list.Add(dataGrid.Items[i]);
                            }
                            return list;
                        });

                        DrawDataGrid(gfx, columns, rows);

                        // Add page number if not the last page
                        if (pageIndex < totalPages - 1)
                        {
                            gfx.DrawString($"Page {pageIndex + 1} of {totalPages}",
                                new XFont("Arial", 12, XFontStyleEx.Regular),
                                XBrushes.Black,
                                new XRect(0, PageHeight - 30, PageWidth, 30),
                                XStringFormats.Center);
                        }
                    }

                    // Save the document
                    document.Save(filePath);
                }
            });
        }

        private void DrawHeader(XGraphics gfx)
        {
            gfx.DrawString("GMS Report",
                new XFont("Arial", 16, XFontStyleEx.Bold),
                XBrushes.Black,
                new XRect(Margin, Margin, PageWidth - 2 * Margin, HeaderHeight),
                XStringFormats.Center);
        }

        private void DrawDataGrid(XGraphics gfx, List<DataGridColumn> columns, List<object> rows)
        {
            double yPosition = Margin + HeaderHeight;

            // Draw column headers
            foreach (var column in columns)
            {
                if (column is DataGridTextColumn textColumn)
                {
                    // Ensure textColumn.Header is a string before using it
                    var headerText = textColumn.Header?.ToString() ?? string.Empty;
                    gfx.DrawString(headerText,
                        new XFont("Arial", 12, XFontStyleEx.Bold),
                        XBrushes.Black,
                        new XRect(Margin, yPosition, ColumnWidth, RowHeight),
                        XStringFormats.TopLeft);
                }
            }
            yPosition += RowHeight;

            // Draw rows
            foreach (var item in rows)
            {
                foreach (var column in columns)
                {
                    if (column is DataGridTextColumn textColumn)
                    {
                        var cellValue = GetCellValue(item, textColumn);
                        gfx.DrawString(cellValue,
                            new XFont("Arial", 10),
                            XBrushes.Black,
                            new XRect(Margin, yPosition, ColumnWidth, RowHeight),
                            XStringFormats.TopLeft);
                    }
                }
                yPosition += RowHeight;
            }
        }

        private string GetCellValue(object item, DataGridTextColumn column)
        {
            var binding = column.Binding as Binding;
            if (binding != null)
            {
                var property = binding.Path.Path;
                var propertyInfo = item.GetType().GetProperty(property);
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(item);
                    return value != null ? value.ToString() : string.Empty;
                }
            }
            return string.Empty;
        }

        private int CalculateRowsPerPage()
        {
            // Calculate number of rows that fit on a page
            return (int)((PageHeight - Margin - HeaderHeight) / RowHeight);
        }

    }
}
