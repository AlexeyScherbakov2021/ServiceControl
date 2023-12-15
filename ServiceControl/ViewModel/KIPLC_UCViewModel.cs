using Microsoft.Win32;
using ServiceControl.Commands;
using ServiceControl.Infrastructure;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using ServiceControl.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using static ServiceControl.ViewModel.ConfigKIPUDZ_WindowVM;

namespace ServiceControl.ViewModel
{
    internal class KIPLC_UCViewModel : Observable //, IDataErrorInfo
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public class StringLC
        {
            public string name { get; set; }
            //public double? value { get; set; }
            public string valueString { get; set; }

            public string paramName;
        }
        public List<StringLC> listLCParam { get; set; }



        public DeviceKIPLC device { get; set; }

        public List<Register> ListInput { get; set; }
        //public List<Register> ListInput2 { get; set; }

        public List<Register> ListRealTime { get; set; }


        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPLC_UCViewModel()
        {
        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public KIPLC_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {

            listLCParam = new List<StringLC>()
            {
                new StringLC() { name = "Modbus адрес получателя", valueString = "1", paramName = "ModBus_addr"},
                new StringLC() { name = "APN", valueString = " ", paramName = "APN"},
                new StringLC() { name = "IP адрес", valueString = "188.120.226.199", paramName = "MQTT_ip_addr"},
                new StringLC() { name = "Порт", valueString = "1883", paramName = "MQTT_port"},
                new StringLC() { name = "Логин", valueString = "mqtt", paramName = "MQTT_login"},
                new StringLC() { name = "Пароль", valueString = "12345", paramName = "MQTT_pass"},
                new StringLC() { name = "Время сна устройства", valueString = "1", paramName = "sleep"},
                new StringLC() { name = "Тест", valueString = "TEST", paramName = "test"},
            };


            device = new DeviceKIPLC(mainViewModel, work, Slave);
            //device.EndRead += OnReadFinish;
            //device.EndStartRead += OnEndStartRead;
            //device.Start();

            ListRealTime = new List<Register>
            {
                device.RealTime
            };
            // добавление в список входных параметров
            ListInput = new List<Register>()
            {
                device.NumberPacket, device.VoltPower,device.SummPot1,
                device.PolPot1, device.CurrPot1, device.VoltNaveden1,  device.FreqVoltNaveden1,
                //device.KoefSummPot1, device.KoefPolPot1,
                //device.KoCurrPot1, device.AddressBIM,
                //device.AddressBIMChange, device.SetID_BI,
            };

            device.Start();

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();

        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            device.RealTime.RealTimeValue = DateTime.Now;
            device.SetRegister(device.RealTime);
            _timer.Start();
        }


        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров при старте
        //--------------------------------------------------------------------------------
        //private void OnEndStartRead(object sender, EventArgs e)
        //{
        //    CommandManager.InvalidateRequerySuggested();
        //    //LastSetMode = device.SetMode.Value;
        //}

        //--------------------------------------------------------------------------------
        // событие после чтения всех регистров
        //--------------------------------------------------------------------------------
        //private void OnReadFinish(object sender, EventArgs e)
        //{

        //}



        #region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Сохранить файл конфигурации
        //--------------------------------------------------------------------------------
        public ICommand WriteConfigCommand => new LambdaCommand(OnWriteConfigCommandExecuted, CanWriteConfigCommand);
        private bool CanWriteConfigCommand(object p) => true;
        private void OnWriteConfigCommandExecuted(object p)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.FileName = "Config_BI_LC.xml";
            sd.Filter = "XML файлы|*.xml";

            if (sd.ShowDialog() == false) return;


            XmlNode userNode;

            XmlDocument xmlDoc = new XmlDocument();
            var decl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "");
            xmlDoc.AppendChild(decl);

            XmlNode rootNode = xmlDoc.CreateElement("BI_LC");
            xmlDoc.AppendChild(rootNode);

            foreach (var item in listLCParam)
            {
                userNode = xmlDoc.CreateElement(item.paramName);
                userNode.InnerText = item.valueString.Trim();
                rootNode.AppendChild(userNode);
            }

            xmlDoc.Save(sd.FileName);
        }


        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        //public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        //private bool CanWriteValueCommand(object p) => device != null;
        //private void OnWriteValueCommandExecuted(object p)
        //{
        //    if (p is Register reg)
        //    {
        //        try
        //        {
        //            device.WriteRegister(reg);
        //            if (reg.GetType() == typeof(RegisterFloat))
        //            {
        //                //(reg as RegisterFloat).Value = 0;
        //            }
        //        }
        //        catch (TimeoutException)
        //        {

        //        }
        //    }
        //}

        //--------------------------------------------------------------------------------
        // Команда Записать в регистр Bool
        //--------------------------------------------------------------------------------
        //public ICommand OnOffCommand => new LambdaCommand(OnOnOffCommandExecuted);
        //private void WriteValueBoolCommand(object p)
        //{
        //    RegisterBool reg = p as RegisterBool;
        //    device.WriteRegister(reg);
        //}


        #endregion



    }
}
