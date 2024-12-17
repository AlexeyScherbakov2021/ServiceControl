using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ServiceControl.View
{
    /// <summary>
    /// Логика взаимодействия для DumpRegWindow.xaml
    /// </summary>
    public partial class DumpRegWindow : Window
    {
        public List<RegisterBase> listRegister { get; set; } = new List<RegisterBase>();

        public DumpRegWindow(List<RegisterBase> _listReg)
        {
            DataContext = this;
            listRegister = _listReg;
            InitializeComponent();
        }
    }
}
