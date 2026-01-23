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
using System.ComponentModel;

namespace SilverLegend.CustomControls
{
    public partial class MuteControl : UserControl
    {
        public MuteControl()
        {
            InitializeComponent();
        }
        void MuteControl_Loaded(object sender, EventArgs e)
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                Page.Instance.IsMutedChanged += delegate
                {
                    bool isMuted = Page.Instance.IsMuted;
                    this.NoEllipse.Visibility = isMuted ? Visibility.Visible : Visibility.Collapsed;
                    this.NoPath.Visibility = isMuted ? Visibility.Visible : Visibility.Collapsed;
                };
                this.MouseLeftButtonDown += delegate
                {
                    Page.Instance.IsMuted = !Page.Instance.IsMuted;
                };
            }
        }


        private void MuteControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //MyToolTip.Visibility = Visibility.Visible;

        }

        private void MuteControl_MouseLeave(object sender, MouseEventArgs e)
        {
            //MyToolTip.Visibility = Visibility.Collapsed;
        }

    }



}
