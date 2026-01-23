using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SilverLegend.Util
{
    public class KeyHandler
    {
        List<Key> KeysDown = new List<Key>();

        public KeyHandler(FrameworkElement fe)
        {
            fe.KeyDown += new KeyEventHandler(fe_KeyDown);
            fe.KeyUp += new KeyEventHandler(fe_KeyUp);
            fe.LostFocus += new RoutedEventHandler(fe_LostFocus);
        }
        public void Detatch(FrameworkElement fe)
        {
            fe.KeyDown -= new KeyEventHandler(fe_KeyDown);
            fe.KeyUp -= new KeyEventHandler(fe_KeyUp);
            fe.LostFocus -= new RoutedEventHandler(fe_LostFocus);
        }

        void fe_KeyDown(object sender, KeyEventArgs e)
        {
            KeysDown.Add(e.Key);
        }

        void fe_KeyUp(object sender, KeyEventArgs e)
        {
            KeysDown.Remove(e.Key);
        }

        void fe_LostFocus(object sender, RoutedEventArgs e)
        {
            KeysDown.Clear();
        }

        public bool IsKeyPressed(Key k)
        {
            return KeysDown.Contains(k);
        }
    }
}
