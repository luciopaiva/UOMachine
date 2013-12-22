using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UOMachine
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    internal partial class About : Window
    {
        private static DropShadowEffect myDropShadow;
        private bool myCancelEnabled = true;
        public bool CancelEnabled
        {
            get { return Utility.ThreadHelper.VolatileRead<bool>(ref myCancelEnabled); }
            internal set {  myCancelEnabled = value;}
        }

        public About()
        {
            InitializeComponent();
            myDropShadow = new DropShadowEffect();
            myDropShadow.Opacity = 1;
            myDropShadow.ShadowDepth = 0;
            myDropShadow.BlurRadius = 8;
            myDropShadow.Color = Colors.Red;
            this.Closing += new System.ComponentModel.CancelEventHandler(About_Closing);
        }

        private void About_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.CancelEnabled)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void textBlock5_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://uomachine.pbwiki.com/");
        }

        private void textBlock5_MouseEnter(object sender, MouseEventArgs e)
        {
            textBlock5.Effect = myDropShadow;

        }

        private void textBlock5_MouseLeave(object sender, MouseEventArgs e)
        {
            textBlock5.Effect = null;
        }
    }
}
