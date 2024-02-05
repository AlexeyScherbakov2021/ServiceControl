using Microsoft.Win32;
using ServiceControl.Commands;
using ServiceControl.Based;
using ServiceControl.Modbus.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace ServiceControl.ViewModel
{
    internal class ConfigKIPUDZ_WindowVM :Observable
    {
        public class StringUDZ
        {
            public string name { get; set; }
            public double? value { get; set; }
            public double? max_value { get; set; }
            
            public string paramName;
        }

        public List<StringUDZ> listUDZParam { get; set; }
        public List<StringUDZ> listUDZParamAvar { get; set; }
        public List<StringUDZ> listUDZParamUst { get; set; }
        public List<StringUDZ> listUDZParamKoef { get; set; }


        public ConfigKIPUDZ_WindowVM()
        {
            listUDZParam = new List<StringUDZ>()
            {
                new StringUDZ() { name = "адрес УДЗ", value = 1, paramName = "mb_addr"},
                new StringUDZ() { name = "включение BlueTooth", value = 1, paramName = "bt_on"},
                new StringUDZ() { name = "периодичность замеров параметров в секундах", value = 3, paramName = "time_adc"},
                new StringUDZ() { name = "периодичность передачи данных в минутах (в тестовом режиме в секундах)", value = 1, paramName = "time_transmite"},
                new StringUDZ() { name = "выдержка времени для инициализации модема в секундах", value = 1, paramName = "time_modem"},
                new StringUDZ() { name = "номинал шунта тока дренажа", value = 500, paramName = "shunt"},
                new StringUDZ() { name = "адрес правого индикатора тока БИТ", value = 1, paramName = "BIT_R_mb_addr"},
                new StringUDZ() { name = "адрес левого индикатора тока БИТ", value = 2, paramName = "BIT_L_mb_addr"},
                new StringUDZ() { name = "адрес УС ИКП", value = 255, paramName = "USIKP_mb_addr"},
            };

            listUDZParamAvar = new List<StringUDZ>()
            {
                new StringUDZ() { name = "вскрытие/закрывание двери УДЗ", value = 1, paramName = "CASE_control"},
                new StringUDZ() { name = "выход за уставки суммарного потенциала", value = 0, paramName = "SP_control"},
                new StringUDZ() { name = "выход за уставки поляризационного потенциала", value = 0, paramName = "PP_control"},
                new StringUDZ() { name = "выход за уставки тока поляризации", value = 0, paramName = "IP_control"},
                new StringUDZ() { name = "выход за уставки напряжения труба-рельс", value = 0, paramName = "TR_control"},
                new StringUDZ() { name = "выход за уставки тока труба-рельс", value = 0, paramName = "ITR_control"},
                new StringUDZ() { name = "выход за уставки цепи труба-рельс, Ом", value = 0, paramName = "RTR_control"},
                new StringUDZ() { name = "выход за уставки напряжения питания", value = 0, paramName = "PWR_control"},
                new StringUDZ() { name = "выход за уставки температуры", value = 0, paramName = "T_control"},
            };

            listUDZParamUst = new List<StringUDZ>()
            {
                new StringUDZ() { name = "суммарный потенциал", value = -10, max_value = 10, paramName = "SP_"},
                new StringUDZ() { name = "поляризационный потенциал", value = -2, max_value = 2, paramName = "PP_"},
                new StringUDZ() { name = "ток поляризации", value = -50, max_value = 50, paramName = "IP_"},
                new StringUDZ() { name = "напряжения труба-рельс", value = -100, max_value = 100, paramName = "TR_"},
                new StringUDZ() { name = "ток труба-рельс", value = -500, max_value = 500, paramName = "ITR_"},
                new StringUDZ() { name = "сопротивления цепи труба-рельс", value = 0, max_value = 100, paramName = "RTR_"},
                new StringUDZ() { name = "напряжения питания", value = 8, max_value = 50, paramName = "PWR_"},
                new StringUDZ() { name = "температура", value = -40, max_value = 80, paramName = "T_"},
            };

            listUDZParamKoef = new List<StringUDZ>()
            {
                new StringUDZ() { name = "коэффициент коррекции суммарного потенциала", value = 1, paramName = "k_SP"},
                new StringUDZ() { name = "коэффициент коррекции поляризационного потенциала", value = 1, paramName = "k_PP"},
                new StringUDZ() { name = "коэффициент коррекции тока поляризации", value = 1, paramName = "k_IP"},
                new StringUDZ() { name = "коэффициент коррекции напряжения труба-рельс", value = 1, paramName = "k_TR"},
                new StringUDZ() { name = "коэффициент коррекции тока труба-рельс", value = 1, paramName = "k_ITR"},
            };
        }

        //--------------------------------------------------------------------------------
        // Команда Сохранить файл конфигурации
        //--------------------------------------------------------------------------------
        public ICommand WriteConfigCommand => new LambdaCommand(OnWriteConfigCommandExecuted, CanWriteConfigCommand);
        private bool CanWriteConfigCommand(object p) => true;
        private void OnWriteConfigCommandExecuted(object p)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "Config_UDZ.xml";
            sd.Filter = "XML файлы|*.xml";

            if (sd.ShowDialog() == false) return;


            XmlNode userNode;

            XmlDocument xmlDoc = new XmlDocument();
            var decl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(decl);

            XmlNode rootNode = xmlDoc.CreateElement("UDZ");
            xmlDoc.AppendChild(rootNode);

            foreach(var item in listUDZParam)
            {
                userNode = xmlDoc.CreateElement(item.paramName);
                userNode.InnerText = item.value.ToString();
                rootNode.AppendChild(userNode);
            }

            foreach(var item in listUDZParamAvar)
            {
                userNode = xmlDoc.CreateElement(item.paramName);
                userNode.InnerText = item.value.ToString();
                rootNode.AppendChild(userNode);
            }

            foreach(var item in listUDZParamUst)
            {
                userNode = xmlDoc.CreateElement(item.paramName + "max");
                userNode.InnerText = item.max_value.ToString();
                rootNode.AppendChild(userNode);
                userNode = xmlDoc.CreateElement(item.paramName + "min");
                userNode.InnerText = item.value.ToString();
                rootNode.AppendChild(userNode);
            }

            foreach (var item in listUDZParamKoef)
            {
                userNode = xmlDoc.CreateElement(item.paramName);
                userNode.InnerText = item.value.ToString();
                rootNode.AppendChild(userNode);
            }
            xmlDoc.Save(sd.FileName);
        }
    }
}
