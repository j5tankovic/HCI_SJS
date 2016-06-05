using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HCI_Project.Komande
{
    public static class RoutedCommands
    {
        public static readonly RoutedUICommand UnosLokala = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control)
            }    
        );

        public static readonly RoutedUICommand UnosTipa = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand UnosEtikete = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand TabelaLokala = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.L, ModifierKeys.Control | ModifierKeys.Shift)
            }
        );

        public static readonly RoutedUICommand TabelaTipova = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift)
            }
        );

        public static readonly RoutedUICommand TabelaEtiketa = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift)
            }
        );

        public static readonly RoutedUICommand Mapa1 = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad1, ModifierKeys.Control),
                new KeyGesture(Key.D1, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Mapa2 = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad2, ModifierKeys.Control),
                new KeyGesture(Key.D2, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Mapa3 = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad3, ModifierKeys.Control),
                new KeyGesture(Key.D3, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Mapa4 = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad4, ModifierKeys.Control),
                new KeyGesture(Key.D4, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Sve4Mape = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.NumPad0, ModifierKeys.Control),
                new KeyGesture(Key.D0, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand EnterClicked = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Enter)
            }
        );

        public static readonly RoutedUICommand DeleteSomething = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Delete, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Escape = new RoutedUICommand(
            " ",
            " ",
            typeof(RoutedCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.Escape)
            }
        );
    }
}
