using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PowerSweeper.Classes;
using System.Windows.Browser;

namespace PowerSweeper
{
    public partial class Credits : UserControl
    {
        private double _BackGroundAspectRatio = (double)1920 / 1080;
        private double _ZoomFactor = 1;
        private double _FontFactor = .04;

        public Credits()
        {
            InitializeComponent();

            SetLevelDimensions();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new Menu());
        }

        private void SetLevelDimensions()
        {
            //Adjust level BackGround
            if (BrowserScreenInformation.ClientHeight <= BrowserScreenInformation.ClientWidth / _BackGroundAspectRatio)
            {
                LayoutRoot.Width = BrowserScreenInformation.ClientHeight * _BackGroundAspectRatio;
                LayoutRoot.Height = BrowserScreenInformation.ClientHeight;
            }
            else
            {
                LayoutRoot.Height = BrowserScreenInformation.ClientWidth / _BackGroundAspectRatio;
                LayoutRoot.Width = BrowserScreenInformation.ClientWidth;
            }

            //Get Level width and height
            double zoomFactorHeight = LayoutRoot.Height;
            double zoomFactorWidth = LayoutRoot.Width;
            double clientHeight = LayoutRoot.Height;
            double clientWidth = LayoutRoot.Width;

            if (zoomFactorWidth <= zoomFactorHeight)
            {
                _ZoomFactor = zoomFactorWidth;
            }
            else
            {
                _ZoomFactor = zoomFactorHeight;
            }

            //Adjust Menu Fonts
            foreach (TextBlock tbMenu in stackMenu.Children.Where(t=>t.GetType()==typeof(TextBlock)))
            {
                tbMenu.FontSize = _ZoomFactor * _FontFactor;
            }

            tbWalidAly.FontSize = _ZoomFactor * _FontFactor;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetLevelDimensions();
        }

    }
}
