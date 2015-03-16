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

namespace PowerSweeper.Classes
{
    public class MineButton : Button
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Border border = GetTemplateChild("Background") as Border;
            Rectangle rect = GetTemplateChild("BackgroundGradient") as Rectangle;

            if (border != null)
            {
                border.Background = this.Background;
                border.Opacity = .6;
            }
            if (rect != null)
            {
                LinearGradientBrush lbrush = rect.Fill as LinearGradientBrush;
                if (lbrush != null)
                {
                    lbrush.Opacity = 0;
                }
            }
        }
    }
}
