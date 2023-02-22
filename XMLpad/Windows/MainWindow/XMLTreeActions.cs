namespace XMLpad
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows;
    using System.Collections.Generic;
    using System.Collections;
    using System.Xml.Linq;

    /// <summary>
    /// The XML tree generation logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {

        /// <summary>
        /// This method attempts to load an XML file and update a tree view
        /// </summary>
        private void TryLoadXML()
        {
            try
            {
                // Call the LoadXml method to update the tree view with the new XML data
                LoadXml();
            }
            catch
            {
                // If an error occurs while loading the XML, display an error message in the tree view
                ElementTree.ItemsSource = new List<TreeViewItem> {
            new TreeViewItem {
                Header = "Invalid XML",
                Foreground = currentTheme == theme.Light ? Brushes.Black : Brushes.White
            }
        };
            }
        }

        /// <summary>
        /// A flag, indicating if the root has already been created.
        /// </summary>
        bool rootCreated;

        /// <summary>
        /// Loads the XML tree view control.
        /// </summary>
        private void LoadXml()
        {
            XDocument xDoc = XDocument.Parse(textEditor.Text);

            rootCreated = false;

            ElementTree.ItemsSource = LoadNodes(xDoc.Root);
        }

        /// <summary>
        /// Loads the nodes into the node tree.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private List<TreeViewItem> LoadNodes(XElement node)
        {
            List<TreeViewItem> items = new List<TreeViewItem>();

            if (!rootCreated)
            {
                rootCreated = true;
                TreeViewItem rootItem = new TreeViewItem
                {
                    Header = node.Name,
                    ItemsSource = LoadNodes(node),
                    IsExpanded = true,
                    Foreground = currentTheme == theme.Light ? Brushes.Black : Brushes.White
                };
                items.Add(rootItem);
                return items;
            }



            foreach (XElement childNode in node.Elements())
            {
                XElement currentChildNode = childNode;

                TreeViewItem item = new TreeViewItem
                {
                    Header = currentChildNode.Name,
                    ItemsSource = LoadNodes(currentChildNode),
                    IsExpanded = true,
                    Foreground = currentTheme == theme.Light ? Brushes.Black : Brushes.White

                };

                if (item.ItemsSource == null || item.Items.Count == 0)
                {
                    item.ItemsSource = new List<TreeViewItem>
                {
                    new TreeViewItem
                    {
                        Header = "\""+currentChildNode.Value+"\"",
                        IsExpanded = true,
                        Foreground = Brushes.Purple
                    }
                };
                }

                items.Add(item);
            }
            return items;
        }

        private void TreeSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the search text from the TextBox
            string searchText = TreeSearchTextBox.Text;

            // Clear any previous highlights
            ClearHighlights();

            // If the search text is not empty, search the TreeView and highlight the results
            if (!string.IsNullOrEmpty(searchText))
            {
                SearchAndHighlight(ElementTree.Items, searchText);
            }
        }

        private void ClearHighlights()
        {
            foreach (TreeViewItem item in ElementTree.Items)
            {
                ClearHighlightsRecursive(item);
            }
        }

        private void ClearHighlightsRecursive(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }

            item.Background = App.Current.Resources["grayBrush"] as SolidColorBrush;

            foreach (TreeViewItem childItem in item.Items)
            {
                ClearHighlightsRecursive(childItem);
            }
        }

        private void SearchAndHighlight(IEnumerable items, string searchText)
        {
            XDocument xDoc = XDocument.Parse(textEditor.Text);

            if (xDoc == null) { return; }

            foreach (TreeViewItem item in items)
            {

                    if (item.Header.ToString().ToLower().Contains(searchText.ToLower()))
                    {
                        HighlightItem(item);
                    }

                    SearchAndHighlight(item.Items, searchText);
                }

        }

        private void HighlightItem(TreeViewItem element)
        {
            // Set the background color of the XElement to yellow to highlight it
            element.Background = Brushes.Azure;
        }


        /// <summary>
        /// Handles the Click event of the refreshXmlTreeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void refreshXmlTreeButton_Click(object sender, RoutedEventArgs e) => TryLoadXML();
    }
}
