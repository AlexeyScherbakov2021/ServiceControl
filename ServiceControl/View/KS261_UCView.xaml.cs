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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ServiceControl.View
{
    /// <summary>
    /// Логика взаимодействия для KS261_UCView.xaml
    /// </summary>
    public partial class KS261_UCView : UserControl
    {
        public KS261_UCView()
        {
            InitializeComponent();
        }



        DataGridColumn CurrentColumn = null;
        Style OldStyle = null;

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (CurrentColumn != null)
            {
                CurrentColumn.CellStyle = OldStyle;
            }
            CurrentColumn = dataGrid.CurrentColumn;
            if (CurrentColumn != null)
            {
                OldStyle = CurrentColumn.CellStyle;
                CurrentColumn.CellStyle = (Style)dataGrid.Resources["SelectedColumnStyle"];
            }
        }
    }
}
