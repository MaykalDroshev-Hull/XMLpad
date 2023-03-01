using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoadingPopUp.xaml
    /// </summary>
    public partial class LoadingPopUp : Window
    {
        private readonly TaskCompletionSource<bool> _hiddenTcs = new TaskCompletionSource<bool>();
        private readonly TaskCompletionSource<bool> _shownTcs = new TaskCompletionSource<bool>();

        public LoadingPopUp()
        {
            InitializeComponent();
            Closed += (_, _) => _hiddenTcs.TrySetResult(true);
        }

        public async Task ShowAsync()
        {
            Show();
            await _shownTcs.Task;
        }

        public async Task HideAsync()
        {
            Hide();
            await _hiddenTcs.Task;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            _shownTcs.TrySetResult(true);
        }
    }
}
