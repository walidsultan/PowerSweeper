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
using PowerSweeper.Controls;
using PowerSweeper.Classes;
using PowerSweeper.Web.Services;
using PowerSweeper.DataTypes;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Threading;

namespace PowerSweeper
{
    public partial class MainPage : UserControl
    {
        public static int _LevelWidth = 10;
        public static int _LevelHeight = 10;

        private const double _ContentWidthtRatio = (double)1;
        private const double _ContentHeightRatio = (double)100 / 123;

        private int _BlockSize = 1;

        public static int _BigMinesCount = 3;
        public static int _MediumMinesCount = 5;
        public static int _SmallMinesCount = 7;

        public static List<LevelBlock> _LstLevelBlocks = new List<LevelBlock>();

        private double _ZoomFactor = 1;
        private double _Fontfactor = 0.027;
        private double _LeftHeaderFactor = 0.035;
        private double _RightHeaderMarginFactor = 0.9;

        public const int MaxMinePower = 3;

        public static bool _IsPlaying = false;

        public double _LevelTime = 0;
        public double _UpdateInterval = 0;

        App _Application = (App)Application.Current;

        LogRecordsContext _LogRecordsContext = new LogRecordsContext();

        public static PowerSweeper.DataTypes.DifficultyLevel _CurrentLevelDifficulty;

        public static string _CurrentUserName;
        public static string _IpAddress;

        public MainPage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(MainPage_Loaded);

            Unloaded += new RoutedEventHandler(MainPage_Unloaded);

            mainCanvas.MouseRightButtonDown += new MouseButtonEventHandler(mainCanvas_MouseRightButtonDown);

            SetHeaderLeft();

            _LstLevelBlocks.Clear();

            StartTimer();
        }

        private void StartTimer()
        {
            DispatcherTimer mainTimer = new DispatcherTimer();
            mainTimer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds 
            mainTimer.Tick += new EventHandler(mainTimer_Tick);
            mainTimer.Start();
        }

        void mainTimer_Tick(object sender, EventArgs e)
        {
            if (_IsPlaying)
            {
                _LevelTime += 0.1;
                _UpdateInterval += 0.1;
                if (Math.Round (_UpdateInterval,0) == 1)
                {
                    tbTimer.Text = _LevelTime.ToString("000");
                    _UpdateInterval = 0;
                }
            }
        }

        void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            mainCanvas.MouseRightButtonDown -= new MouseButtonEventHandler(mainCanvas_MouseRightButtonDown);
            mainCanvas.Children.Clear();
            _LstLevelBlocks.Clear();
        }

        private void SetHeaderLeft()
        {
            int bigMinesCount = _BigMinesCount - _LstLevelBlocks.Count(b => b.SelectedMinePower == 3);
            int mediumMinesCount = _MediumMinesCount - _LstLevelBlocks.Count(b => b.SelectedMinePower == 2);
            int smallMinesCount = _SmallMinesCount - _LstLevelBlocks.Count(b => b.SelectedMinePower == 1);

            tbBigMines.Text = bigMinesCount.ToString();
            tbMediumMines.Text = mediumMinesCount.ToString();
            tbSmallMines.Text = smallMinesCount.ToString();
        }

        void mainCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SetLevelDimensions();

            GenerateLevel();

            if (!Application.Current.IsRunningOutOfBrowser)
            {
                System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", this.OnContextMenu);
            }

            this.KeyDown += new KeyEventHandler(MainPage_KeyDown);
        }

        void MainPage_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    GenericPopUp confirmEscape = new GenericPopUp();
                    confirmEscape.Title = " Confirm Escape";
                    confirmEscape.tbMessage.Text = "Are you sure you want to Quit?";
                    confirmEscape.Closed += new EventHandler(confirmEscape_Closed);
                    confirmEscape.Show();
                    break;
            }
        }

        void confirmEscape_Closed(object sender, EventArgs e)
        {
            GenericPopUp confirmEscape = (GenericPopUp)sender;
            if (confirmEscape.DialogResult == true)
            {
                PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
                pageSwticher.Navigate(new Menu());
            }
        }

        public void GenerateLevel()
        {
            //Set Blocks position
            for (double rowIndex = 0; rowIndex < _LevelWidth; rowIndex++)
            {
                for (double columnIndex = 0; columnIndex < _LevelHeight; columnIndex++)
                {
                    LevelBlock blockInstance = AddBlock<LevelBlock>(rowIndex, columnIndex);
                    blockInstance.btnBlock.FontSize *= _ZoomFactor * _Fontfactor * _LevelWidth;
                    blockInstance.MineSelected += new LevelBlock.MineSelectedHandler(blockInstance_MineSelected);
                    blockInstance.LevelSolved += new LevelBlock.LevelSolvedHandler(blockInstance_LevelSolved);
                    blockInstance.LevelLost += new LevelBlock.LevelLostHandler(blockInstance_LevelLost);
                    _LstLevelBlocks.Add(blockInstance);
                }
            }

            //Add mines
            AddMines(_BigMinesCount, 3);
            AddMines(_MediumMinesCount, 2);
            AddMines(_SmallMinesCount, 1);
        }

        void blockInstance_LevelLost(object source, EventArgs e)
        {
            GenericPopUp levelLost = new GenericPopUp();
            levelLost.tbMessage.Text = "Play again?";
            levelLost.Title = "Game Over";
            levelLost.Closed += new EventHandler(gameOver_Closed);
            levelLost.Show();
        }

        void blockInstance_LevelSolved(object source, EventArgs e)
        {
            activitySavingProgress.IsActive = true;

            InvokeOperation<bool> isHighScoreOperation = _LogRecordsContext.IsHighScore(_CurrentLevelDifficulty, _LevelTime);
            isHighScoreOperation.Completed += new EventHandler(isHighScoreOperation_Completed);
        }

        void isHighScoreOperation_Completed(object sender, EventArgs e)
        {
            InvokeOperation<bool> isHighScoreOperation = (InvokeOperation<bool>)sender;
          
                 if (string.IsNullOrEmpty(_CurrentUserName))
                 {
                     if (isHighScoreOperation.Value)
                     {

                         activitySavingProgress.IsActive = false;
                         NewHighScore newHighScore = new NewHighScore();
                         newHighScore.tbName.Text = string.Empty;
                         newHighScore.Show();
                         newHighScore.Closed += new EventHandler(newHighScore_Closed);
                     }
                     else
                     {
                         AddLogRecord("Anonymous");
                     }
                 }
                 else
                 {
                     AddLogRecord(_CurrentUserName);
                 }
        }

        void newHighScore_Closed(object sender, EventArgs e)
        {
            activitySavingProgress.IsActive = true;
            NewHighScore newHighScore = (NewHighScore)sender;
            string username = newHighScore.tbName.Text.Trim() == string.Empty ? "Anonymous" : newHighScore.tbName.Text.Trim();
            AddLogRecord(username);
        }

        private void AddLogRecord(string username)
        {
            LogRecord logRecord = new LogRecord();
            logRecord.DifficultyLevel = _CurrentLevelDifficulty;
            logRecord.Username = username;
            logRecord.IpAddress = Application.Current.IsRunningOutOfBrowser ? "OOB" : _IpAddress;
            logRecord.Time = Math.Round(_LevelTime, 1);
            _LogRecordsContext.LogRecords.Add(logRecord);
            SubmitOperation submitLogRecord = _LogRecordsContext.SubmitChanges();
            submitLogRecord.Completed += new EventHandler(submitLogRecord_Completed);
        }

        void submitLogRecord_Completed(object sender, EventArgs e)
        {
            activitySavingProgress.IsActive = false;

            GenericPopUp levelSolved = new GenericPopUp();
            levelSolved.tbMessage.Text = string.Format("Play again?", Math.Round(_LevelTime, 1));
            levelSolved.Title = "Level Solved";
            levelSolved.Closed += new EventHandler(gameOver_Closed);
            levelSolved.Show();
        }

        void gameOver_Closed(object sender, EventArgs e)
        {
            GenericPopUp levelSolved = (GenericPopUp)sender;
            if (levelSolved.DialogResult == true)
            {
                _LstLevelBlocks.Clear();
                _LevelTime = 0;
                tbTimer.Text = _LevelTime.ToString("000");
                mainCanvas.Children.Clear();
                GenerateLevel();
                SetHeaderLeft();
            }
            else
            {
                PageSwitcher pageSwticher = (PageSwitcher)this.Parent;
                pageSwticher.Navigate(new Menu());
            }
        }

        void blockInstance_MineSelected(object source, EventArgs e)
        {
            SetHeaderLeft();
        }

        private void AddMines(int minesCount, int power)
        {
            int minesLeft = minesCount;
            Random randomGenrator = new Random();
            Random topIndex = new Random();
            while (minesLeft > 0)
            {
                int nextLeft = randomGenrator.Next(_LevelWidth - 1);
                int nextTop = randomGenrator.Next(_LevelHeight - 1);
                LevelBlock randomBlock = _LstLevelBlocks.SingleOrDefault(b => b.position.Left == nextLeft &&
                                                                                                                                    b.position.Top == nextTop &&
                                                                                                                                    b.IsMine == false);
                if (randomBlock != null)
                {
                    randomBlock.IsMine = true;
                    randomBlock.MinePower = power;
                    minesLeft--;
                }
            }
        }

        private T AddBlock<T>(double left, double top) where T : System.Windows.UIElement, IBlockPositionable, new()
        {
            T blockInstance = new T();
            mainCanvas.Children.Add(blockInstance);
            blockInstance.SetValue(Canvas.LeftProperty, left * _ZoomFactor);
            blockInstance.SetValue(Canvas.TopProperty, top * _ZoomFactor);

            blockInstance.position.Left = left;
            blockInstance.position.Top = top;

            blockInstance.SetValue(UserControl.WidthProperty, _BlockSize * _ZoomFactor);
            blockInstance.SetValue(UserControl.HeightProperty, _BlockSize * _ZoomFactor);

            blockInstance.Opacity = LevelBlock.BlockOpacity;

            return blockInstance;
        }

        private void SetLevelDimensions()
        {
            //Adjust level BackGround
            if (BrowserScreenInformation.ClientHeight <= BrowserScreenInformation.ClientWidth)
            {
                LayoutRoot.Width = BrowserScreenInformation.ClientHeight / 1048 * 1000;
                LayoutRoot.Height = BrowserScreenInformation.ClientHeight;
            }
            else
            {
                LayoutRoot.Height = BrowserScreenInformation.ClientWidth / 1000 * 1048;
                LayoutRoot.Width = BrowserScreenInformation.ClientWidth;
            }

            //Get Level width and height
            double levelWidth = _LevelWidth * _BlockSize;
            double levelHeight = _LevelHeight * _BlockSize;
            double clientHeight = LayoutRoot.Height * _ContentHeightRatio;
            double clientWidth = LayoutRoot.Width * _ContentWidthtRatio;

            double zoomFactorHeight = clientHeight / levelHeight;
            double zoomFactorWidth = clientWidth / levelWidth;

            if (zoomFactorWidth <= zoomFactorHeight)
            {
                _ZoomFactor = zoomFactorWidth;

                //Set main canvas left margin
                double topMargin = (clientHeight - levelHeight * _ZoomFactor) / 2;
                mainCanvas.SetValue(Canvas.MarginProperty, new Thickness(0, topMargin, 0, topMargin));
            }
            else
            {
                _ZoomFactor = zoomFactorHeight;

                //Set main canvas left margin
                double leftMargin = (clientWidth - levelWidth * _ZoomFactor) / 2;
                mainCanvas.SetValue(Canvas.MarginProperty, new Thickness(leftMargin, 0, leftMargin, 0));
            }

            foreach (UIElement ctrl in stackLeftHeader.Children)
            {
                ctrl.SetValue(Image.WidthProperty, _ZoomFactor * _LeftHeaderFactor * _LevelWidth);
                ctrl.SetValue(Image.HeightProperty, _ZoomFactor * _LeftHeaderFactor * _LevelWidth);
                if (ctrl.GetType() == typeof(TextBlock))
                {
                    ctrl.SetValue(TextBlock.FontSizeProperty, _ZoomFactor * _Fontfactor * _LevelWidth);
                    ctrl.SetValue(TextBlock.PaddingProperty,  new Thickness(_ZoomFactor * _Fontfactor ,0,0,0 ));
                }
            }
            tbTimer.FontSize = _ZoomFactor * _Fontfactor * _LevelWidth;

            stackRightHeader.Margin = new Thickness(0, 0, _ZoomFactor * _RightHeaderMarginFactor, 0);
        }

        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            e.PreventDefault();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetLevelDimensions();

            foreach (LevelBlock block in _LstLevelBlocks)
            {
                block.SetValue(Canvas.LeftProperty, block.position.Left * _ZoomFactor);
                block.SetValue(Canvas.TopProperty, block.position.Top * _ZoomFactor);

                block.SetValue(UserControl.WidthProperty, _BlockSize * _ZoomFactor);
                block.SetValue(UserControl.HeightProperty, _BlockSize * _ZoomFactor);

                block.btnBlock.FontSize = _ZoomFactor * _Fontfactor * _LevelWidth;
            }
        }
    }
}
