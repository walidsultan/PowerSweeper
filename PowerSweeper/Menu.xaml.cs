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
using PowerSweeper.Controls;
using PowerSweeper.DataTypes;

namespace PowerSweeper
{
    public partial class Menu : UserControl
    {
        private double _ZoomFactor = 1;
        private double _BackGroundAspectRatio = (double)1920 / 1080;

        private double _FontFactor = .05;
        public Menu()
        {
            InitializeComponent();

           SetLevelDimensions();

           if (Application.Current.IsRunningOutOfBrowser)
           {
               tbInstall.Visibility = Visibility.Collapsed;
           }
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
            foreach (TextBlock tbMenu in stackMenu.Children )
            {
                tbMenu.FontSize = _ZoomFactor*_FontFactor ;
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
                SetLevelDimensions();
        }

        private void tb_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Colors.Green );
        }

        private void tb_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)sender).Foreground = new SolidColorBrush(Color.FromArgb(255,29,26,249));
        }

        private void tbNewGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LevelDifficulty levelDifficulty = new LevelDifficulty();
            levelDifficulty.DifficultySelected += new LevelDifficulty.DifficultySelectedHandler(levelDifficulty_DifficultySelected);
            levelDifficulty.Show();
        }

        void levelDifficulty_DifficultySelected(object source, DifficultySelectedEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            switch (e.SelectedDifficultyLevel)
            {
                case DifficultyLevel.Easy:
                    MainPage._LevelWidth = 7;
                    MainPage._LevelHeight = 7;
                    MainPage._SmallMinesCount = 1;
                    MainPage._MediumMinesCount = 2;
                    MainPage._BigMinesCount = 3;
                    MainPage._CurrentLevelDifficulty = DifficultyLevel.Easy; 
                    pageSwticher.Navigate(new MainPage());
                    break;
                case DifficultyLevel.Medium:
                    MainPage._LevelWidth = 10;
                    MainPage._LevelHeight = 10;
                    MainPage._SmallMinesCount = 3;
                    MainPage._MediumMinesCount = 5;
                    MainPage._BigMinesCount = 7;
                    MainPage._CurrentLevelDifficulty = DifficultyLevel.Medium; 
                    pageSwticher.Navigate(new MainPage());
                    break;
                case DifficultyLevel.Hard:
                    MainPage._LevelWidth = 15;
                    MainPage._LevelHeight = 15;
                    MainPage._SmallMinesCount = 6;
                    MainPage._MediumMinesCount = 10;
                    MainPage._BigMinesCount = 14;
                    MainPage._CurrentLevelDifficulty = DifficultyLevel.Hard; 
                    pageSwticher.Navigate(new MainPage());
                    break;
            }
        }

        private void tbHighScores_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new HighScores ());
        }

        private void tbInstall_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Application.Current.InstallState == InstallState.Installed)
            {
                GenericPopUp applicationInstalled=new GenericPopUp();
                applicationInstalled.Title = "Power Sweeper";
                applicationInstalled.tbMessage.Text = "Application is already installed";
                applicationInstalled.OKButton.Visibility = Visibility.Collapsed;
                applicationInstalled.CancelButton.Content  = "OK"; 
                applicationInstalled.Show();
            }
            else
            {
                Application.Current.Install();
            }
        }

        private void tbInstructions_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new Instructions());
        }

        private void tbCredits_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new Credits ());
        }
    }
}
