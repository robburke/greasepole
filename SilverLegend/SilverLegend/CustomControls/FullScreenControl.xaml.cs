using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Interop;

namespace SilverLegend.CustomControls
{
    public partial class FullScreenControl : UserControl
    {
        public FullScreenControl()
        {
            InitializeComponent();
        }
        void FullScreenControl_Loaded(object sender, EventArgs e)
        {
            this.MouseLeftButtonDown += delegate
            {
                App.Current.Host.Content.IsFullScreen = !App.Current.Host.Content.IsFullScreen;
                //this.Visibility = GoingFullScreen ? Visibility.Collapsed : Visibility.Visible;
                GoingFullScreen = !GoingFullScreen;
                //MyToolTip.Visibility = Visibility.Collapsed;
            };
        }

        bool GoingFullScreen = true;


        private void FullScreenControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //MyToolTip.Visibility = Visibility.Visible;
        }

        private void FullScreenControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //MyToolTip.Visibility = Visibility.Collapsed;
        }

    }



}
