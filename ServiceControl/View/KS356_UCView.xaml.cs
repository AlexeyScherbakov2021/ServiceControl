using ServiceControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceControl.View
{
    /// <summary>
    /// Логика взаимодействия для KS356_UCView.xaml
    /// </summary>
    public partial class KS356_UCView : UserControl
    {
        //List<int> word;
        int resPass = 0;

        public KS356_UCView()
        {
            InitializeComponent();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int flag = 0;
            switch(e.Key)
            {
                case Key.O:
                    resPass = 0;
                    flag = 1;
                    break;
                case Key.P:
                    if(resPass == 1)
                        flag = 1;
                    break;
                case Key.E:
                    if (resPass == 2)
                        flag = 1;
                    break;
                case Key.N:
                    if (resPass == 3)
                        flag = 1;
                    break;
                default:
                    resPass = 0;
                    break;
            }

            resPass += flag;
            if (resPass == 4)
            {
                Debug.WriteLine("Pass Enter.");
                ((KS356_UCViewModel)DataContext).IsTimesVisible = Visibility.Visible;
            }
        }
    }
}
