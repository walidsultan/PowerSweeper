using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PowerSweeper.DataTypes;

namespace PowerSweeper.Classes
{
    public class DifficultySelectedEventArgs:EventArgs 
    {
        private DifficultyLevel _SelectedDifficultyLevel;

        public DifficultyLevel SelectedDifficultyLevel
        {
            get { return _SelectedDifficultyLevel; }
            set { _SelectedDifficultyLevel = value; }
        }
    }
}
