using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Wpf.DataForm.Library.DataForm.Builder.Factory
{
    class TableControlFactory : IControlFactory
    {
        #region Constants

        private const string TableTagName = "table";
        private const string ColumnSpanTagName = "span";
        private const string TableRowTagName = "tr";
        private const string TableColumnTagName = "td";
        private const string HeaderAttributeName = "header";
        private const string ShowLabelAttributeName = "td-show-label";

        private const int NoLabelElementColumnSpanOffset = 1;

        #endregion

        #region Methods

        private static void CreateLabel(Grid grid, int row, int col, string text)
        {
            if (text == null)
            {
                return;
            }

            TextBlock label = new TextBlock();
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Text = text + ":";
            label.FontWeight = FontWeights.Bold;

            label.SetValue(FrameworkElement.MarginProperty, new Thickness(2d));
            label.SetValue(Grid.ColumnProperty, (col * 2));
            label.SetValue(Grid.RowProperty, row);

            grid.Children.Add(label);
        }

        #endregion

        #region IControlFactory Members

        IEnumerable<string> IControlFactory.GetSupportedNames()
        {
            yield return TableTagName;
        }

        UIElement IControlFactory.Build(ConstructionParameters parameters, IControlBuildService buildService)
        {
            XElement element = parameters.Node;
            IList<XElement> rows = element.Elements(TableRowTagName).ToList();

            int rowCount = rows.Count();
            int columnCount = CalculateMaxColumnCount(rows);

            ContentBuildResult tableresult = new ContentBuildResult();

            Grid table = new Grid();

            XAttribute headerA = element.Attribute(HeaderAttributeName);
            if (headerA != null)
            {
                GroupBox groupBox = new GroupBox();
                groupBox.Header = headerA.Value;
                groupBox.Content = table;

                tableresult.Element = groupBox;
            }
            else
            {
                tableresult.Element = table;
            }

            for (int ir = 0; ir < rowCount; ir++)
            {
                table.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
            for (int ic = 0; ic < columnCount * 2; ic++)
            {
                GridLength gl = new GridLength(1.0d, GridUnitType.Star);
                if (ic % 2 == 0)
                {
                    gl = GridLength.Auto;
                }

                table.ColumnDefinitions.Add(new ColumnDefinition() { Width = gl });
            }

            for (int row = 0; row < rows.Count; row++)
            {
                XElement rowitem = rows[row];

                IList<XElement> columns = rowitem.Elements(TableColumnTagName).ToList();
                for (int col = 0; col < columns.Count; col++)
                {
                    XElement td = columns[col];

                    XElement cell = td.Elements().FirstOrDefault();
                    if (cell == null)
                    {
                        continue;
                    }

                    ContentBuildResult result = buildService.BuildNode(cell, parameters.Parent);

                    int colspan = 1;
                    XAttribute span = td.Attribute(ColumnSpanTagName);
                    if (span != null)
                    {
                        colspan = int.Parse(span.Value);
                        if (colspan > 1)
                        {
                            colspan = (colspan * 2) - 1;
                        }
                    }

                    int colActual = (col * 2);
                    if (result.ShowLabel && ShallShowElementLabel(cell))
                    {
                        CreateLabel(table, row, col, result.DisplayName);
                        colActual = colActual + 1;
                    }
                    else
                    {
                        colspan = colspan + NoLabelElementColumnSpanOffset;
                    }

                    result.Element.SetValue(FrameworkElement.MarginProperty, new Thickness(2d));
                    result.Element.SetValue(Grid.ColumnProperty, colActual);
                    result.Element.SetValue(Grid.ColumnSpanProperty, colspan);
                    result.Element.SetValue(Grid.RowProperty, row);

                    table.Children.Add(result.Element);
                }
            }

            if (buildService.DataFormControlService.IsDebugMode)
            {
                table.SetValue(Grid.ShowGridLinesProperty, true);
                table.Background = SystemColors.InfoBrush;
            }

            return tableresult.Element;
        }

        private static bool ShallShowElementLabel(XElement cell)
        {
            XAttribute showLabelA = cell.Attribute(ShowLabelAttributeName);
            if (showLabelA != null && showLabelA.Value.Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        private static int CalculateMaxColumnCount(IEnumerable<XElement> rows)
        {
            int columnCount = 0;
            foreach (XElement tr in rows)
            {
                int l = tr.Elements(TableColumnTagName).Count();
                columnCount = Math.Max(columnCount, l);
            }
            return columnCount;
        }

        #endregion
    }
}
