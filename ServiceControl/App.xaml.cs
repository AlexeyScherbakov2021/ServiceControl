using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace ServiceControl
{
    public enum NumLanguage { None = -1, Rus, Eng};


    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private CultureInfo SelectedCilture = new CultureInfo("ru-RU");
        //public static NumLanguage SelectLang;

        public App()
        {

            FrameworkElement.LanguageProperty.OverrideMetadata(
                            typeof(FrameworkElement),
                            new FrameworkPropertyMetadata(
                            XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        }

        //public NumLanguage ChangeReg(CultureInfo cu)
        //{
        //    if (Equals(cu.Name, SelectedCilture.Name)) return NumLanguage.None;

        //    var dict = new ResourceDictionary() { Source = new Uri($"Resources/lang.{cu.Name}.xaml", UriKind.Relative) };
        //    if (dict == null) return NumLanguage.None;

        //    SelectedCilture = cu;

        //    ResourceDictionary oldDict = Application.Current.Resources.MergedDictionaries
        //        .Where(it => it.Source != null && it.Source.OriginalString.Contains("lang"))
        //        .Select(s => s)
        //        .First();

        //    Application.Current.Resources.MergedDictionaries.Remove(oldDict);
        //    Application.Current.Resources.MergedDictionaries.Add(dict);

        //    if (cu.Name == "ru-RU")
        //    {
        //        SelectLang = NumLanguage.Rus;
        //        return NumLanguage.Rus;
        //    }
        //    else if (cu.Name == "en-US")
        //    {
        //        SelectLang = NumLanguage.Eng;
        //        return NumLanguage.Eng;
        //    }
        //    else
        //        return NumLanguage.None;
                    
        //}

    }
}
