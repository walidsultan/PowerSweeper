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

namespace PowerSweeper
{
    public partial class Instructions : UserControl
    {
        private double _BackGroundAspectRatio = (double)1920 / 1080;
        private double _ZoomFactor = 1;
        private double _FontFactor = .032;

        public Instructions()
        {
            InitializeComponent();

            txtInstructions1.Text = "•Right click to place a flag" + Environment.NewLine +
                                                          "for a mine.";
            txtInstructions2.Text = "•There are 3 types of mines" + Environment.NewLine +
                                                                                                                              "each differ in its power.";

            txtInstructions3.Text = "•You can change the type" + Environment.NewLine +
                                     "of mine by right clicking"+ Environment.NewLine +
                                     "the one you just placed" + Environment.NewLine +
                                     "small=1 medium=2 big=3";

        //    txtInstructions3.Text = "•You finish the puzzle when" + Environment.NewLine +
        //                                                                                                                    "you have placed all the " + Environment.NewLine +
        //                                                                                                                    "flags in their correct place.";
        //
        }


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new Menu());
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetLevelDimensions();
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
            txtInstructions1.FontSize = _ZoomFactor * _FontFactor;
            txtInstructions2.FontSize = _ZoomFactor * _FontFactor;
            txtInstructions3.FontSize = _ZoomFactor * _FontFactor;
        }
    }
}
