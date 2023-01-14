using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XMLpad
{
    /// <summary>
    /// Interaction logic for ManualWindow.xaml
    /// </summary>
    public partial class ManualWindow : Window
    {
        public ManualWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/MaykalDroshev-Hull/XMLpad",
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();


        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> routed event is raised on this element. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. The event data reports that the left mouse button was pressed.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedItem = (TreeViewItem)manualTreeView.SelectedItem;
            string header = selectedItem.Header as string;
            switch (header)
            {
                case "New":
                    explanationTextBlock.Text = "The 'New' command creates a new file.";
                    break;
                case "Clear":
                    explanationTextBlock.Text = "The 'Clear' command clears the contents of the current file.";
                    break;
                case "Open":
                    explanationTextBlock.Text = "The 'Open' command opens an existing file.";
                    break;
                case "Save":
                    explanationTextBlock.Text = "The 'Save' command saves the current file.";
                    break;
                case "Save As":
                    explanationTextBlock.Text = "The 'Save As' command saves the current file with a different name or location.";
                    break;
                case "Save A Copy As":
                    explanationTextBlock.Text = "The 'Save A Copy As' command saves a copy of the current file with a different name or location.";
                    break;
                case "Print":
                    explanationTextBlock.Text = "The 'Print' command prints the current file.";
                    break;
                case "Exit":
                    explanationTextBlock.Text = "The 'Exit' command closes the application.";
                    break;
                case "Undo":
                    explanationTextBlock.Text = "The 'Undo' command undoes the last action.";
                    break;
                case "Redo":
                    explanationTextBlock.Text = "The 'Redo' command redoes the last undone action.";
                    break;
                case "Cut":
                    explanationTextBlock.Text = "The 'Cut' command cuts the selected text.";
                    break;
                case "Copy":
                    explanationTextBlock.Text = "The 'Copy' command copies the selected text.";
                    break;
                case "Paste":
                    explanationTextBlock.Text = "The 'Paste' command pastes the copied or cut text.";
                    break;
                case "Delete":
                    explanationTextBlock.Text = "The 'Delete' command deletes the selected text.";
                    break;
                case "Select All":
                    explanationTextBlock.Text = "The 'Select All' command selects all the text in the current file.";
                    break;
                case "Date Time (Short)":
                    explanationTextBlock.Text = "The 'Date Time (Short)' command inserts the current date and time in a short format.";
                    break;
                case "Date Time (Long)":
                    explanationTextBlock.Text = "The 'Date Time (Long)' command inserts the current date and time in a long format.";
                    break;
                case "Date Time (Customised)":
                    explanationTextBlock.Text = "The 'Date Time (Customised)' command inserts the current date and time in a customised format.";
                    break;
                case "Customise Date Time":
                    explanationTextBlock.Text = "The 'Customise Date Time' command allows you to customize the format of the date and time that is inserted when using the 'Date Time (Customised)' command.";
                    break;
                case "Copy Current File Path":
                    explanationTextBlock.Text = "The 'Copy Current File Path' command copies the file path of the current file to the clipboard.";
                    break;
                case "Copy Current File Name":
                    explanationTextBlock.Text = "The 'Copy Current File Name' command copies the file name of the current file to the clipboard.";
                    break;
                case "Increase Indent (+)":
                    explanationTextBlock.Text = "The 'Increase Indent (+)' command increases the indentation level of the selected text.";
                    break;
                case "Decrease Indent (-)":
                    explanationTextBlock.Text = "The 'Decrease Indent (-)' command decreases the indentation level of the selected text.";
                    break;
                case "All Uppercase":
                    explanationTextBlock.Text = "The 'All Uppercase' command converts the selected text to uppercase.";
                    break;
                case "All Lowercase":
                    explanationTextBlock.Text = "The 'All Lowercase' command converts the selected text to lowercase.";
                    break;
                case "Invert Case":
                    explanationTextBlock.Text = "The 'Invert Case' command inverts the case of the selected text.";
                    break;
                case "Random Case":
                    explanationTextBlock.Text = "The 'Random Case' command applies a random case to the selected text.";
                    break;
                case "Duplicate Line":
                    explanationTextBlock.Text = "The 'Duplicate Line' command duplicates the current line.";
                    break;
                case "Remove Duplicate Lines":
                    explanationTextBlock.Text = "The 'Remove Duplicate Lines' command removes duplicate lines from the current file.";
                    break;
                case "Remove Consecutive Duplicate Lines":
                    explanationTextBlock.Text = "The 'Remove Consecutive Duplicate Lines' command removes consecutive duplicate lines from the current file.";
                    break;
                case "Move Up The Current Line":
                    explanationTextBlock.Text = "The 'Move Up The Current Line' command moves the current line up one position.";
                    break;
                case "Move Down The Current Line":
                    explanationTextBlock.Text = "The 'Move Down The Current Line' command moves the current line down one position.";
                    break;
                case "Remove Empty Lines":
                    explanationTextBlock.Text = "The 'Remove Empty Lines' command removes empty lines from the current file.";
                    break;
                case "Insert Blank Line Above Current":
                    explanationTextBlock.Text = "The 'Insert Blank Line Above Current' command inserts a blank line above the current line.";
                    break;
                case "Insert Blank Line Below Current":
                    explanationTextBlock.Text = "The 'Insert Blank Line Below Current' command inserts a blank line below the current line.";
                    break;
                case "Comment/Uncomment":
                    explanationTextBlock.Text = "The 'Comment/Uncomment' command allows you to comment or uncomment the selected text.";
                    break;
                case "Comment":
                    explanationTextBlock.Text = "The 'Comment' command allows you to add a comment symbol at the beginning of each line in the selected text.";
                    break;
                case "Uncomment":
                    explanationTextBlock.Text = "The 'Uncomment' command allows you to remove the comment symbol at the beginning of each line in the selected text.";
                    break;
                default:
                    explanationTextBlock.Text = "This command is not yet implemented.";
                    break;
            }
        }
    }
}

