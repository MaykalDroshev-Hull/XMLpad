﻿namespace XMLpad
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for ManualWindow.xaml
    /// </summary>
    public partial class ManualWindow : Window
    {
        public ManualWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Hyperlink_RequestNavigate event when the Github hyperlink is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the information on the navigation request.</param>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // Start a new process to open the Github link
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/MaykalDroshev-Hull/XMLpad",
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Handles the CloseButton_Click event when the close button is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();


        /// <summary>
        /// Handles the OnMouseLeftButtonDown event when the mouse left button is pressed down on the window.
        /// </summary>
        /// <param name="e">The mouse button event arguments containing information about the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            this.DragMove();
        }

        /// <summary>
        /// Handles the TreeView_SelectedItemChanged event when the selected item in the TreeView changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing information about the change in the selected item.</param>
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Get the selected item from the TreeView
            TreeViewItem selectedItem = (TreeViewItem)manualTreeView.SelectedItem;

            // Get the header of the selected item
            string? header = selectedItem.Header as string;

            // Set the explanation text based on the header of the selected item
            explanationTextBlock.Text = header switch
            {
                "New" => "The 'New' command creates a new file in the current directory. This file is typically blank, but can be pre-populated with default content depending on the application or operating system being used. The new file will have a unique file name that can be specified by the user or automatically generated by the system.",
                "Clear" => "The 'Clear' command clears the contents of the current file, effectively deleting all the data stored within it.",
                "Open" => "The 'Open' command opens an existing file.",
                "Save" => "The 'Save' command saves the current file.",
                "Save As" => "The 'Save As' command saves the current file with a different name or location.",
                "Save A Copy As" => "The 'Save A Copy As' command saves a copy of the current file with a different name or location.",
                "Print" => "The 'Print' command prints the current file.",
                "Compare File With Another" => "The 'Compare File With Another' saves and compares the current file with another file chosen immediately after pressing the button.",
                "Exit" => "The 'Exit' command closes the application.",
                "Undo" => "The 'Undo' command undoes the last action.",
                "Redo" => "The 'Redo' command redoes the last undone action.",
                "Cut" => "The 'Cut' command cuts the selected text.",
                "Copy" => "The 'Copy' command copies the selected text.",
                "Paste" => "The 'Paste' command pastes the copied or cut text.",
                "Delete" => "The 'Delete' command deletes the selected text.",
                "Select All" => "The 'Select All' command selects all the text in the current file.",
                "Date Time (Short)" => "The 'Date Time (Short)' command inserts the current date and time in a short format.",
                "Date Time (Long)" => "The 'Date Time (Long)' command inserts the current date and time in a long format.",
                "Date Time (Customised)" => "The 'Date Time (Customised)' command inserts the current date and time in a customised format.",
                "Customise Date Time" => "The 'Customise Date Time' command allows you to customize the format of the date and time that is inserted when using the 'Date Time (Customised)' command.",
                "Copy Current File Path" => "The 'Copy Current File Path' command copies the file path of the current file to the clipboard.",
                "Copy Current File Name" => "The 'Copy Current File Name' command copies the file name of the current file to the clipboard.",
                "Increase Indent (+)" => "The 'Increase Indent (+)' command increases the indentation level of the selected text.",
                "Decrease Indent (-)" => "The 'Decrease Indent (-)' command decreases the indentation level of the selected text.",
                "All Uppercase" => "The 'All Uppercase' command converts the selected text to uppercase.",
                "All Lowercase" => "The 'All Lowercase' command converts the selected text to lowercase.",
                "Invert Case" => "The 'Invert Case' command inverts the case of the selected text.",
                "Random Case" => "The 'Random Case' command applies a random case to the selected text.",
                "Duplicate Line" => "The 'Duplicate Line' command duplicates the current line.",
                "Remove Duplicate Lines" => "The 'Remove Duplicate Lines' command removes duplicate lines from the current file.",
                "Remove Consecutive Duplicate Lines" => "The 'Remove Consecutive Duplicate Lines' command removes consecutive duplicate lines from the current file.",
                "Move Up The Current Line" => "The 'Move Up The Current Line' command moves the current line up one position.",
                "Move Down The Current Line" => "The 'Move Down The Current Line' command moves the current line down one position.",
                "Remove Empty Lines" => "The 'Remove Empty Lines' command removes empty lines from the current file.",
                "Insert Blank Line Above Current" => "The 'Insert Blank Line Above Current' command inserts a blank line above the current line.",
                "Insert Blank Line Below Current" => "The 'Insert Blank Line Below Current' command inserts a blank line below the current line.",
                "Comment/Uncomment" => "The 'Comment/Uncomment' command allows you to comment or uncomment the selected text.",
                "Comment" => "The 'Comment' command allows you to add a comment symbol at the beginning of each line in the selected text.",
                "Uncomment" => "The 'Uncomment' command allows you to remove the comment symbol at the beginning of each line in the selected text.",
                "Trim Trailing Space" => "The 'Trim Trailing Spaces' command is used to remove any unwanted whitespace characters, such as spaces or tabs, from the end of lines in the selected text. This is useful for cleaning up code and making sure that there are no unnecessary characters present. By trimming the whitespace, the code will be more readable and easier to understand.",
                "Trim Leading Space" => "The 'Trim Leading Space' command is used to remove any unwanted whitespace characters, such as spaces or tabs, from the beginning of lines in the selected text. This is useful for cleaning up code and making sure that there are no unnecessary characters present. By trimming the whitespace, the code will be more readable and easier to understand.",
                "Trim Trailing and Leading Space" => "The 'Trim Trailing and Leading Space' command is used to remove any unwanted whitespace characters, such as spaces or tabs, from both the beginning and end of lines in the selected text. This is useful for cleaning up code and making sure that there are no unnecessary characters present. By trimming the whitespace, the code will be more readable and easier to understand.",
                "TAB To Space" => "The 'TAB To Space' command is used to convert any tab characters into spaces in the selected text. This is useful for making sure that the code is properly formatted and easier to read. By converting all of the tabs to spaces, the code will be more consistent and easier to understand.",
                "Space to TAB(Leading)" => "The 'Space to TAB(Leading)' command is used to convert any leading spaces into tab characters in the selected text. This is useful for making sure that the code is properly formatted and easier to read. By converting all of the spaces to tabs, the code will be more consistent and easier to understand.",
                "Space To TAB(Trailing)" => "The 'Space to TAB(Trailing)' command is used to convert any trailing spaces into tab characters in the selected text. This is useful for making sure that the code is properly formatted and easier to read. By converting all of the spaces to tabs, the code will be more consistent and easier to understand.",
                "Space to TAB(All)" => "The 'Space to TAB(All)' command is used to convert any spaces into tab characters in the selected text, regardless of whether they are leading or trailing. This is useful for making sure that the code is properly formatted and easier to read. By converting all of the spaces to tabs, the code will be more consistent and easier to understand.",
                "Set As Read-Only" => "The 'Set As Read-Only' command is used to make the file read-only, meaning that it cannot be modified. This is useful for protecting certain parts of a document from being accidentally changed.",
                "Toggle Full Screen Mode" => "The 'Toggle Full Screen Mode' command is used to toggle between full screen and windowed mode, allowing you to make the most of your screen space. To toggle full screen mode, simply click the 'Toggle Full Screen Mode' command from the menu. The program will then switch between full screen and windowed mode.",
                "Syntax Highlighting" => "The syntax highlighting command usually has multiple options for different programming languages, and the highlighting rules for each language are defined in a syntax definition file. This file specifies which elements of the code should be highlighted and how they should be displayed. In addition to coloring, syntax highlighting can also provide code folding, which allows you to collapse blocks of code to make it easier to navigate large files. For example, you can fold all of the functions in a file so that only the function names are visible. When you need to access the code in a function, you can simply unfold it.",
                "Show spaces and Tabs" => "The 'Show Spaces and Tabs' command is used to display any whitespace characters in a document, including spaces and tabs. This is useful for debugging and for visualizing how the document is structured. To display the whitespace characters, simply click the 'Show Spaces and Tabs' command from the menu. The whitespace characters will then be highlighted and visible in the document",
                "Show new lines" => "The 'Show New Lines' command is used to display any new line characters in a document, allowing you to see where lines begin and end. To display the new line characters, simply click the 'Show New Lines' command from the menu. The new line characters will then be highlighted and visible in the document.",
                "Show all characters" => "The 'Show All Characters' command is used to display all of the characters, including whitespace, new line, and other hidden characters, in a document. To display all of the characters, simply click the 'Show All Characters' command from the menu. The characters will then be highlighted and visible in the document.",
                "Zoom in" => "The 'Zoom In' command is used to magnify the current view of a document, allowing you to see more detail. To zoom in, simply click the 'Zoom In' command from the menu or press the corresponding keyboard shortcut ('Ctrl' + '+'). The document will then be magnified, allowing you to see more detail.",
                "Zoom out" => "The 'Zoom Out' command is used to reduce the current view of a document, allowing you to see a larger area. To zoom out, simply click the 'Zoom Out' command from the menu or press the corresponding keyboard shortcut ('Ctrl' + '-'). The document will then be reduced in size, allowing you to see more of the document at once.",
                "Restore default zoom" => "The 'Restore Default Zoom' command is used to reset the view of a document to the default size. To restore the default zoom, simply click the 'Restore Default Zoom' command from the menu or press the corresponding keyboard shortcut ('Ctrl' + '0'). The document will then be reset to its default zoom level.",
                "Change Theme" => "The 'Change Theme' Command will change between the dark and the light mode of the program.",
                "View Manual" => "The View Manual commands leads to this window",
                "Menu Bar Commands" or "File Commands" or "Edit Commands" or "View" or "Settings" or "Help" or "Insert" or "Convert Case" or "Indent" or "Line Operations" or "Blank Operations" or "Show Symbol" or "Zoom" => string.Empty,
                _ => "This command is not yet implemented.",
            };
        }
    }
}
