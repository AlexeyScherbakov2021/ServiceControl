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
    /// Логика взаимодействия для SKZ12_UCView.xaml
    /// </summary>
    public partial class KS216_UCView : UserControl
    {
        public KS216_UCView()
        {
            InitializeComponent();
        }

        //private void TextBox_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if(e.Key == Key.Enter)
        //    {
        //        var res = BindingOperations.GetBinding((DependencyObject)sender, TextBox.TextProperty);
        //    }
        //}

        //private void validationError(object sender, ValidationErrorEventArgs e)
        //{
        //    if (e.Action == ValidationErrorEventAction.Added)
        //    {

        //        if(e.Source is TextBox tb)
        //        {
        //            tb.Text = "1,2";
        //            var bind = tb.GetBindingExpression(TextBox.TextProperty);
        //            Validation.ClearInvalid(bind);
        //        }

        //        //MessageBox.Show(e.Error.ErrorContent.ToString());
        //    }
        //    else
        //    {
        //        if (e.Source is TextBox tb)
        //        {
        //            tb.Text = "2,2";
        //            var bind = tb.GetBindingExpression(TextBox.TextProperty);
        //            bind.UpdateTarget();

        //        }
        //    }
        //}
    }
}
