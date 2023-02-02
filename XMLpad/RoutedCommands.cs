using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XMLpad
{

    public class IncreaseIndentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainWindow window = parameter as MainWindow;
            window.MainMenu_IncreaseIndent(null, null);
        }
    }
    public class DecreaseIndentCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            MainWindow window = parameter as MainWindow;
            window.MainMenu_IncreaseIndent(null, null);
        }
    }
}

