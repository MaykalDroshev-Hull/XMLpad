using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XMLpad
{
    public static class CustomApplicationCommands
    {
        public static RoutedUICommand ZoomIn { get; private set; } = new RoutedUICommand("Zoom In", "ZoomIn", typeof(CustomApplicationCommands), new InputGestureCollection { new KeyGesture(Key.Add, ModifierKeys.Control) });
        public static RoutedUICommand ZoomOut { get; private set; } = new RoutedUICommand("Zoom Out", "ZoomOut", typeof(CustomApplicationCommands), new InputGestureCollection { new KeyGesture(Key.Subtract, ModifierKeys.Control) });
        public static RoutedUICommand ZoomDefault { get; private set; } = new RoutedUICommand("Restore Default Zoom", "ZoomDefault", typeof(CustomApplicationCommands), new InputGestureCollection { new KeyGesture(Key.Divide, ModifierKeys.Control) });
    }
}
