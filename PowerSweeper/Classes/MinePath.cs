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
using System.Windows.Media.Imaging;

namespace PowerSweeper.Classes
{
    public class MinePath
    {
        public static BitmapImage GetMineImageByPower(int minePower)
        {
            string imagePath = null;
            switch (minePower)
            {
                case 1:
                    imagePath = "../images/smallMine.png";
                    break;
                case 2:
                    imagePath = "../images/MediumMine.png";
                    break;
                case 3:
                    imagePath = "../images/BigMine.png";
                    break;

                default:
                    throw new Exception("Mine power not supported");
            }

            return new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }
    }
}
