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
    using System.Windows.Input;
    using System.Diagnostics;
    using System.Windows.Media.Animation;
    using ICSharpCode.AvalonEdit;
    using ICSharpCode.AvalonEdit.Rendering;
    using ICSharpCode.AvalonEdit.Document;
    using System.Media;

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
                        Foreground = Application.Current.Resources["blueTagBrush"] as SolidColorBrush
                    }
                };
                }

                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Handles the TextChanged event of the TreeSearchTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Clears the highlights.
        /// </summary>
        private void ClearHighlights()
        {
            foreach (TreeViewItem item in ElementTree.Items)
            {
                ClearHighlightsRecursive(item);
            }
        }

        /// <summary>
        /// Clears the highlights recursive.
        /// </summary>
        /// <param name="item">The item.</param>
        private void ClearHighlightsRecursive(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }

            item.Background = currentTheme == theme.Dark? App.Current.Resources["grayBrush"] as SolidColorBrush:Brushes.White;

            foreach (TreeViewItem childItem in item.Items)
            {
                ClearHighlightsRecursive(childItem);
            }
        }

        /// <summary>
        /// Searches the and highlight.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="searchText">The search text.</param>
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

        /// <summary>
        /// Highlights the item.
        /// </summary>
        /// <param name="element">The element.</param>
        private void HighlightItem(TreeViewItem element)
        {
            // Set the background color of the XElement to yellow to highlight it
            element.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3FAAA5"));
        }


        /// <summary>
        /// Handles the Click event of the refreshXmlTreeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void refreshXmlTreeButton_Click(object sender, RoutedEventArgs e) => TryLoadXML();

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item from the TreeView
            TreeViewItem selectedItem = (TreeViewItem)ElementTree.SelectedItem;

            if (selectedItem == null)
            {
                SystemSounds.Exclamation.Play();
                return;
            }
            // Get the header of the selected item
            string? header = selectedItem.Header.ToString().Trim('"');

            // Set the focus on the textEditor
            textEditor.Focus();

            int index = textEditor.Text.IndexOf(header);

            if (index >= 0)
            {
                // Select the matching text in the text editor
                textEditor.Select(index, header.Length);
                int line = textEditor.Document.GetLineByOffset(index).LineNumber;
                textEditor.ScrollToLine(line);
            }
        }
    }
}
