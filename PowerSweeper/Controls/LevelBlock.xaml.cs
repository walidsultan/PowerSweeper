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
using System.Windows.Media.Imaging;
using System.Threading;

namespace PowerSweeper.Controls
{
    public partial class LevelBlock : UserControl, IBlockPositionable
    {
        public delegate void MineSelectedHandler(object source, EventArgs e);
        public event MineSelectedHandler MineSelected;

        public delegate void LevelSolvedHandler(object source, EventArgs e);
        public event LevelSolvedHandler LevelSolved;

        public delegate void LevelLostHandler(object source, EventArgs e);
        public event LevelLostHandler LevelLost;

        private bool _IsMine = false;

        public bool IsMine
        {
            get { return _IsMine; }
            set { _IsMine = value; }
        }

        private bool _IsSelected = false;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set { _IsSelected = value; }
        }

        private int _MinePower;

        public int MinePower
        {
            get { return _MinePower; }
            set { _MinePower = value; }
        }

        private int _SelectedMinePower;

        public int SelectedMinePower
        {
            get { return _SelectedMinePower; }
            set { _SelectedMinePower = value; }
        }

        private Position _Position = new Position();

        public Position position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }

        public const double BlockOpacity = 0.7;

        private bool _IsLevelSolved = false;

        public LevelBlock()
        {
            InitializeComponent();
        }

        private void OnMineSelected()
        {
            if (MineSelected != null)
            {
                MineSelected(this.imgMine, new EventArgs());
            }
        }

        private void OnLevelSolved()
        {
            if (LevelSolved  != null)
            {
                LevelSolved(this.imgMine, new EventArgs());
            }
        }

        private void OnLevelLost()
        {
            if (LevelLost  != null)
            {
                LevelLost(this.imgMine, new EventArgs());
            }
        }

        private void btnBlock_Click(object sender, RoutedEventArgs e)
        {
            MainPage._IsPlaying = true;
            if (_IsMine)
            {
                btnBlock.Visibility = Visibility.Collapsed;
                imgMine.Visibility = Visibility.Visible;
                imgMine.Source = MinePath.GetMineImageByPower(this.MinePower);
                this.LayoutRoot.Background = new SolidColorBrush(Colors.Red);
                EndGame();
                OnLevelLost();
            }
            else
            {
                //Set the state of the adjacent blocks recursivley
                SetBlockState(this, true);
                IsLevelSolved();
            }

            OnMineSelected();
        }

        private void EndGame()
        {
            foreach (LevelBlock block in MainPage._LstLevelBlocks)
            {
                if (!block.IsSelected && !block.IsMine)
                {
                    SetBlockState(block, false);
                }
                else if (block.IsMine)
                {
                    block.btnBlock.Visibility = Visibility.Collapsed;
                    block.imgMine.Visibility = Visibility.Visible;
                    block.imgMine.Source = MinePath.GetMineImageByPower(block.MinePower);
                    block.Opacity = 1;
                }
            }
            MainPage._IsPlaying = false;
        }

        private void SetBlockState(LevelBlock levelBlock, bool isRecursive)
        {
            List<LevelBlock> lstAdjacentBlocks = GetAdjacentBlocks(levelBlock);
            List<LevelBlock> lstAdjacentMines = lstAdjacentBlocks.Where(b => b.IsMine).ToList();
            int minesCount = 0;
            foreach (LevelBlock mine in lstAdjacentMines)
            {
                minesCount += mine.MinePower;
            }

            levelBlock.btnBlock.Content = (minesCount == 0) ? string.Empty : minesCount.ToString();
            levelBlock.btnBlock.IsEnabled = false;
            levelBlock.btnBlock.Visibility = Visibility.Visible;
            levelBlock.Opacity = 1;
            levelBlock.IsSelected = true;
            levelBlock.imgMine.Visibility = Visibility.Collapsed;
            levelBlock.SelectedMinePower = 0;
            if (minesCount == 0 && isRecursive)
            {
                foreach (LevelBlock block in lstAdjacentBlocks)
                {
                    if (!block.IsMine && !block.IsSelected)
                    {
                        SetBlockState(block, true);
                    }
                }
            }
        }

        private List<LevelBlock> GetAdjacentBlocks(LevelBlock targetBlock)
        {
            return MainPage._LstLevelBlocks.Where(b => (b.position.Left == targetBlock.position.Left - 1 && b.position.Top == targetBlock.position.Top) ||
                                                                                     (b.position.Left == targetBlock.position.Left + 1 && b.position.Top == targetBlock.position.Top) ||
                                                                                     (b.position.Left == targetBlock.position.Left && b.position.Top == targetBlock.position.Top - 1) ||
                                                                                     (b.position.Left == targetBlock.position.Left && b.position.Top == targetBlock.position.Top + 1) ||

                                                                                     (b.position.Left == targetBlock.position.Left - 1 && b.position.Top == targetBlock.position.Top - 1) ||
                                                                                     (b.position.Left == targetBlock.position.Left + 1 && b.position.Top == targetBlock.position.Top + 1) ||
                                                                                     (b.position.Left == targetBlock.position.Left - 1 && b.position.Top == targetBlock.position.Top + 1) ||
                                                                                     (b.position.Left == targetBlock.position.Left + 1 && b.position.Top == targetBlock.position.Top - 1)
                                                                                     ).ToList();
        }

        private void btnBlock_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MainPage._IsPlaying)
            {
                e.Handled = true;
                SetSelectedMinepower();
                OnMineSelected();
            }
        }

        private void imgMine_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            SetSelectedMinepower();
            OnMineSelected();
        }

        private void SetSelectedMinepower()
        {
            if (this.SelectedMinePower < MainPage.MaxMinePower)
            {
                this.SelectedMinePower++;

                //Get Mines with the selected power
                int selectedMines = MainPage._LstLevelBlocks.Count(b => b.SelectedMinePower == this.SelectedMinePower);
                if ((selectedMines > MainPage._BigMinesCount && this.SelectedMinePower == 3) ||
                    (selectedMines > MainPage._MediumMinesCount && this.SelectedMinePower == 2) ||
                    (selectedMines > MainPage._SmallMinesCount && this.SelectedMinePower == 1))
                {
                    SetSelectedMinepower();
                    return;
                }

                this.imgMine.Source = MinePath.GetMineImageByPower(this.SelectedMinePower);
                this.imgMine.Visibility = Visibility.Visible;
                this.btnBlock.Visibility = Visibility.Collapsed;
                this.Opacity = 1;
            }
            else if (this.SelectedMinePower == MainPage.MaxMinePower)
            {
                this.SelectedMinePower = 0;
                this.imgMine.Visibility = Visibility.Collapsed;
                this.btnBlock.Visibility = Visibility.Visible;
                this.Opacity = BlockOpacity;
            }

            //Check if Level is solved
            if (!_IsLevelSolved)
            {
                IsLevelSolved();
            }
        }

        private void IsLevelSolved()
        {
            int wrongBlocks = MainPage._LstLevelBlocks.Count(b => (b.SelectedMinePower != b.MinePower) || (b.MinePower == 0 && b.IsMine));
            if (wrongBlocks == 0)
            {
                EndGame();
                _IsLevelSolved = true;
                OnLevelSolved();
            }
        }

        private void imgMine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.SelectedMinePower = 0;
            this.imgMine.Visibility = Visibility.Collapsed;
            this.btnBlock.Visibility = Visibility.Visible;
            this.Opacity = BlockOpacity;
            OnMineSelected();
        }

    }
}
