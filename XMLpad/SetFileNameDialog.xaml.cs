using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SetFileNameDialog.xaml
    /// </summary>
    public partial class SetFileNameDialog: Window
    {
        private bool mIsCreated;

        public bool IsFileCreated { get => mIsCreated; }

        public SetFileNameDialog()
        {
            mIsCreated = false;
            InitializeComponent();
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!fileNameTextBox.Text.EndsWith(".xml"))
                fileNameTextBox.AppendText(".xml");

            FileStream fileStream = new FileStream(fileNameTextBox.Text, FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fileStream);
            writer.Flush();
            writer.Close();
            // TODO: add the file to the list of recent files;
            mIsCreated = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => Close();

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
    }
}
