namespace XMLpad
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using System.Windows;

    /// <summary>
    /// The XML tree generation logic for MainWindow.xaml
    /// </summary>
    /// <seealso cref="System.Windows.Window" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class MainWindow : Window
    {
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
                item.Background = currentTheme == theme.Light ? Brushes.White : Application.Current.Resources["grayBrush"] as SolidColorBrush;
                ClearHighlights(item);
            }
        }

        /// <summary>
        /// Clears the highlights.
        /// </summary>
        /// <param name="item">The item.</param>
        private void ClearHighlights(TreeViewItem item)
        {
            foreach (TreeViewItem child in item.Items)
            {
                child.Background = currentTheme == theme.Light ? Brushes.White : Application.Current.Resources["grayBrush"] as SolidColorBrush;
                ClearHighlights(child);
            }
        }

        /// <summary>
        /// Searches and highlights.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="searchText">The search text.</param>
        private void SearchAndHighlight(ItemCollection items, string searchText)
        {
            foreach (object? item in items)
            {
                TreeViewItem? treeViewItem = item as TreeViewItem;
                if (treeViewItem == null)
                {
                    continue;
                }

                // Check if the current item matches the search text
                string text = treeViewItem.Header.ToString();
                if (text != null && text.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                {
                    HighlightText((TextBlock)treeViewItem.Header, searchText);
                }
                // Recursively search the children
                SearchAndHighlight(treeViewItem.Items, searchText);
            }
        }

        /// <summary>
        /// Highlights the text.
        /// </summary>
        /// <param name="textBlock">The text block.</param>
        /// <param name="searchText">The search text.</param>
        private void HighlightText(TextBlock textBlock, string searchText)
        {
            string text = textBlock.Text;
            int index = text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                textBlock.Text = text.Remove(index, searchText.Length).Insert(index, searchText);
                TextRange range = new TextRange(textBlock.ContentStart.GetPositionAtOffset(index, LogicalDirection.Forward),
                                          textBlock.ContentStart.GetPositionAtOffset(index + searchText.Length, LogicalDirection.Backward));
                range.ApplyPropertyValue(TextBlock.BackgroundProperty, Brushes.Yellow);
            }
        }

        /// <summary>
        /// Handles the Click event of the refreshXmlTreeButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void refreshXmlTreeButton_Click(object sender, RoutedEventArgs e) => TryLoadXML();
    }
}
