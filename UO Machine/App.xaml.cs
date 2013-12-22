using System;
using System.Windows;

namespace UOMachine
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            UOMachine.UOM.Dispose();
            base.OnExit(e);
        }
    }
}
