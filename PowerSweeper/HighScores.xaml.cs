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
using PowerSweeper.Web.Services;
using System.ServiceModel.DomainServices.Client;
using PowerSweeper.DataTypes;

namespace PowerSweeper
{
    public partial class HighScores : UserControl
    {
        public HighScores()
        {
            InitializeComponent();

            activityLoadingTopUsersEasy.IsActive = true;

            Loaded += new RoutedEventHandler(HighScores_Loaded);
        }

        void HighScores_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEasyHighScores();
        }

        private void LoadEasyHighScores()
        {
            LogRecordsContext _LogRecordsContext = new LogRecordsContext();
            dgEasy.ItemsSource = _LogRecordsContext.LogRecords;
            LoadOperation loadTopTenUsersEasyOperation = _LogRecordsContext.Load(_LogRecordsContext.GetHighScoresByDifficultyQuery(DifficultyLevel.Easy ));
            loadTopTenUsersEasyOperation.Completed += new EventHandler(loadTopTenUsersEasyOperation_Completed);
        }

        private void LoadMediumHighScores()
        {
            LogRecordsContext _LogRecordsContext = new LogRecordsContext();
            dgMedium.ItemsSource = _LogRecordsContext.LogRecords;
            LoadOperation loadTopTenUsersEasyOperation = _LogRecordsContext.Load(_LogRecordsContext.GetHighScoresByDifficultyQuery(DifficultyLevel.Medium));
            loadTopTenUsersEasyOperation.Completed += new EventHandler(loadTopTenUsersEasyOperation_Completed);
        }

        private void LoadHardHighScores()
        {
            LogRecordsContext _LogRecordsContext = new LogRecordsContext();
            dgHard.ItemsSource = _LogRecordsContext.LogRecords;
            LoadOperation loadTopTenUsersEasyOperation = _LogRecordsContext.Load(_LogRecordsContext.GetHighScoresByDifficultyQuery(DifficultyLevel.Hard));
            loadTopTenUsersEasyOperation.Completed += new EventHandler(loadTopTenUsersEasyOperation_Completed);
        }

        void loadTopTenUsersEasyOperation_Completed(object sender, EventArgs e)
        {
            activityLoadingTopUsersEasy.IsActive = false;
            activityLoadingTopUsersMedium.IsActive = false;
            activityLoadingTopUsersHard.IsActive = false;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
            pageSwticher.Navigate(new Menu());
        }

        private void tabItemEasy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            activityLoadingTopUsersEasy.IsActive = true;
            LoadEasyHighScores();
        }

        private void tabItemMedium_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            activityLoadingTopUsersMedium.IsActive = true;
            LoadMediumHighScores();
        }

        private void tabItemHard_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            activityLoadingTopUsersHard.IsActive = true;
            LoadHardHighScores();
        }
    }
}
