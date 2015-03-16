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
using PowerSweeper.DataTypes;

namespace PowerSweeper.Controls
{
    public partial class LevelDifficulty : ChildWindow
    {
        public delegate void DifficultySelectedHandler(object source, DifficultySelectedEventArgs e);
        public event DifficultySelectedHandler DifficultySelected;

        private DifficultyLevel _SelectedDifficultyLevel;

        public DifficultyLevel SelectedDifficultyLevel
        {
            get { return _SelectedDifficultyLevel; }
            set { _SelectedDifficultyLevel = value; }
        }

        public LevelDifficulty()
        {
            InitializeComponent();
        }

        private void OnDifficultySelected()
        {
            if (DifficultySelected != null)
            {
                DifficultySelectedEventArgs e = new DifficultySelectedEventArgs();
                e.SelectedDifficultyLevel = _SelectedDifficultyLevel;
                DifficultySelected(this, e);
            }
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            _SelectedDifficultyLevel = DifficultyLevel.Easy;
            OnDifficultySelected();
            this.DialogResult = true;
        }

        private void btnMedium_Click(object sender, RoutedEventArgs e)
        {
            _SelectedDifficultyLevel = DifficultyLevel.Medium;
            OnDifficultySelected();
            this.DialogResult = true;
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            _SelectedDifficultyLevel = DifficultyLevel.Hard;
            OnDifficultySelected();
            this.DialogResult = true;
        }
    }
}

