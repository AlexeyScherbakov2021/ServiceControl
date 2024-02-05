using Modbus.Utility;
using ServiceControl.Commands;
using ServiceControl.Based;
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ServiceControl.ViewModel
{
    internal class TermViewModel : Observable //, IDataErrorInfo
    {
        public struct TypeFunction
        {
            public string name { get;set; }
            public ModbusFunc type { get; set; }
        }

        public List<TypeFunction> listFunction { get; set; } = new List<TypeFunction>()
        {
            new TypeFunction() { name = "Coils (0x01)", type = ModbusFunc.Coil },
            new TypeFunction() { name = "InputDiscrete (0x02)", type = ModbusFunc.InputDiscrete },
            new TypeFunction() { name = "HoldingRegister (0x03)", type = ModbusFunc.HoldingRegister },
            new TypeFunction() { name = "InputRegister (0x04)", type = ModbusFunc.InputRegister },
            new TypeFunction() { name = "WriteCoil (0x05)", type = ModbusFunc.WriteCoil },
            new TypeFunction() { name = "WriteRegister (0x06)", type = ModbusFunc.WriteRegister },
            //new TypeFunction() { name = "WriteMultiCoil (0x0F)", type = ModbusFunc.WriteMultiCoils },
            //new TypeFunction() { name = "WriteMultiRegister (0x10)", type = ModbusFunc.WriteMultiple },
        };


        private readonly Dictionary<int, string> listError = new Dictionary<int, string>()
        {
            { 1, "1 - Принятый код функции не может быть обработан." },
            { 2, "2 - Адрес данных, указанный в запросе, недоступен." },
            { 3, "3 - Значение, содержащееся в поле данных запроса, является недопустимой величиной." },
            { 4, "4 - Невосстанавливаемая ошибка имела место, пока ведомое устройство пыталось выполнить затребованное действие." },
            { 5, "5 - Ведомое устройство приняло запрос и обрабатывает его, но это требует много времени. Этот ответ предохраняет ведущее устройство от генерации ошибки тайм-аута." },
            { 6, "6 - Ведомое устройство занято обработкой команды. Ведущее устройство должно повторить сообщение позже, когда ведомое освободится." },
            { 7, "7 - Ведомое устройство не может выполнить программную функцию, заданную в запросе. Этот код возвращается для неуспешного программного запроса, использующего функции с номерами 13 или 14. Ведущее устройство должно запросить диагностическую информацию или информацию об ошибках от ведомого." },
            { 8, "8 - Ведомое устройство при чтении расширенной памяти обнаружило ошибку паритета. Ведущее устройство может повторить запрос, но обычно в таких случаях требуется ремонт." },
            { 10, "10 - Шлюз неправильно настроен или перегружен запросами." },
            { 11, "11 - Slave устройства нет в сети или от него нет ответа." },
        };

        private readonly MbWork work;

        //----------------------------------------------------------------------------------------------
        #region Экранные переменные для отправки

        private byte _SlaveAddress  = 1;
        public byte SlaveAddress { get => _SlaveAddress; set { Set(ref _SlaveAddress, value); packetToString(); } }

        private ModbusFunc _function = ModbusFunc.HoldingRegister;
        public ModbusFunc function 
        { 
            get => _function; 
            set 
            { 
                Set(ref _function, value); packetToString();
                if(_function == ModbusFunc.WriteCoil || _function == ModbusFunc.WriteRegister)
                {
                    nameParam = "Значение";
                }
                else
                {
                    nameParam = "Число регистров";
                }
            } 
        }

        private ushort _AddressReg = 1;
        public ushort AddressReg { get => _AddressReg; set { Set(ref _AddressReg, value); packetToString(); } }

        private short _countRegister = 1;
        public short countRegister { get => _countRegister; set { Set(ref _countRegister, value); packetToString(); } } 

        private string _checkSumma;
        public string checkSumma { get => _checkSumma; set { Set(ref _checkSumma, value); } }

        private string _packetSend;
        public string packetSend { get => _packetSend; set { Set(ref _packetSend, value); } }


        private string _nameParam = "Число регистров";
        public string nameParam { get => _nameParam; set { Set(ref _nameParam, value); } }


        #endregion

        //----------------------------------------------------------------------------------------------
        #region Экранные переменные для получения
        private byte _SlaveAddressR = 1;
        public byte SlaveAddressR { get => _SlaveAddressR; set { Set(ref _SlaveAddressR, value); } }

        private ModbusFunc _functionR = ModbusFunc.HoldingRegister;
        public ModbusFunc functionR { get => _functionR; set { Set(ref _functionR, value); } }

        private ushort _countByte;
        public ushort countByte { get => _countByte; set { Set(ref _countByte, value); } }

        private string _dataString;
        public string dataString { get => _dataString; set { Set(ref _dataString, value); } }

        private string _dataIntString;
        public string dataIntString { get => _dataIntString; set { Set(ref _dataIntString, value); } }

        private string _checkSummaR;
        public string checkSummaR { get => _checkSummaR; set { Set(ref _checkSummaR, value); } }

        private string _packetSendR;
        public string packetSendR { get => _packetSendR; set { Set(ref _packetSendR, value); } }

        private string _packetError;
        public string packetError { get => _packetError; set { Set(ref _packetError, value); } }

        #endregion

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public TermViewModel()
        {

        }

        //--------------------------------------------------------------------------------------------
        // конструктор
        //--------------------------------------------------------------------------------------------
        public TermViewModel(MainWindowViewModel mainViewModel, MbWork wrk, int Slave)
        {
            work = wrk;
            SlaveAddress = (byte)Slave;
            work.master.Transport.EventLogEvent += Transport_EventLogEvent;
        }

        private void Transport_EventLogEvent(string header, byte[] message)
        {
            if(header == "RX")
            {
                packetSendR = ByteArrayToString(message);
                parsingAnswer(message);
            }
        }

        //--------------------------------------------------------------------------
        // Расчет контрольной суммы
        //--------------------------------------------------------------------------
        //private void calculateCRC()
        //{
        //    byte[] values = new byte[6];

        //    values[0] = SlaveAddress;
        //    values[1] = (byte)function;
        //    values[2] = (byte)(AddressReg >> 8);
        //    values[3] = (byte)(AddressReg & 0xFF);
        //    values[4] = (byte)(countRegister >> 8);
        //    values[5] = (byte)(countRegister & 0xFF);

        //    var crc = ModbusUtility.CalculateCrc(values);
        //    checkSumma = ByteArrayToString(crc);
        //}

        //--------------------------------------------------------------------------
        // Преобразование всего пакета в строку
        //--------------------------------------------------------------------------
        private void packetToString()
        {
            byte[] values = new byte[6];

            values[0] = SlaveAddress;
            values[1] = (byte)function;
            values[2] = (byte)(AddressReg >> 8);
            values[3] = (byte)(AddressReg & 0xFF);
            values[4] = (byte)(countRegister >> 8);
            values[5] = (byte)(countRegister & 0xFF);

            var crc = ModbusUtility.CalculateCrc(values);
            checkSumma = ByteArrayToString(crc);

            byte[] values2 = new byte[8];
            values.CopyTo(values2, 0);
            values2[6] = crc[0];
            values2[7] = crc[1];

            packetSend = ByteArrayToString(values2);
        }


        //--------------------------------------------------------------------------
        // Преобразование полученных данных в строку
        //--------------------------------------------------------------------------
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2} ", b);
            return hex.ToString();
        }


        //--------------------------------------------------------------------------
        // Преобразование полученных данных в форму
        //--------------------------------------------------------------------------
        private void parsingAnswer(byte[] data2)
        {
            int indexData = 0;
            byte[] data = data2;

            if (work.isTCP)
            {
                data = new byte[data2.Count() - 6];
                for(int i = 0; i < data.Count(); ++i)
                {
                    data[i] = data2[i + 6];
                }    
            }

            SlaveAddressR = data[0];
            functionR = (ModbusFunc)(data[1] & 0x7F);
            dataIntString = "";

            packetError = "";

            if ((data[1] & 0x80) > 0)
            {
                dataString = "";
                countByte = 0;
                // был установлен код ошибки
                packetError = listError[data[2]];

                byte[] crc = new byte[2];
                crc[0] = data[3];
                crc[1] = data[4];
                checkSummaR = ByteArrayToString(crc);

            }
            else
            {
                countByte = data[2];

                byte[] dataByte = new byte[countByte];
                for (int i = 0; i < countByte; ++i)
                {
                    dataByte[i] = data[i + 3];
                }

                dataString = ByteArrayToString(dataByte);

                int indexCRC = data.Count() - 2;
                byte[] crc = new byte[2];
                crc[0] = data[indexCRC];
                crc[1] = data[++indexCRC];
                checkSummaR = ByteArrayToString(crc);

                List<string> listInt = new List<string>();
                for (int i = 0; i < dataByte.Count(); i += 2)
                {
                    short n = (short)((dataByte[i] << 8) | dataByte[i + 1]);
                    listInt.Add(n.ToString());
                }

                dataIntString = string.Join(", ", listInt);
            }
        }

#region Команды =================================

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand SendCommand => new LambdaCommand(OnSendCommandExecuted, CanSendCommand);
        private bool CanSendCommand(object p) => true;
        private void OnSendCommandExecuted(object p)
        {
            ushort[] res;

            try
            {
                switch (function)
                {
                    case ModbusFunc.Coil:
                        work.ReadRegisterCoil(AddressReg, (ushort)countRegister, SlaveAddress);
                        break;

                    case ModbusFunc.InputDiscrete:
                        work.ReadRegisterDiscret(AddressReg, (ushort)countRegister, SlaveAddress);
                        break;

                    case ModbusFunc.HoldingRegister:
                        work.ReadRegisterHolding(AddressReg, (ushort)countRegister, SlaveAddress);
                        break;

                    case ModbusFunc.InputRegister:
                        work.ReadRegisterInput(AddressReg, (ushort)countRegister, SlaveAddress);
                        break;

                    case ModbusFunc.WriteRegister:
                        work.WriteRegister(AddressReg, (ushort)countRegister, SlaveAddress);
                        break;

                    case ModbusFunc.WriteCoil:
                        bool val = countRegister != 0;
                        work.WriteRegister(AddressReg, val, SlaveAddress);
                        break;
                }
            }
            catch
            {

            }
        }

        #endregion

    }
}
