using ServiceControl.Based;
using ServiceControl.Commands;
using ServiceControl.Modbus;
using ServiceControl.Modbus.Devices;
using ServiceControl.Modbus.Registers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ServiceControl.ViewModel
{
    internal class BIT_UCViewModel : Observable
    {

        public DeviceBIT device { get; set; }
        public List<RegisterFloat> ListInput { get; set; }
        public List<Register> ListCommand { get; set; }
        public List<TwoRegister> ListWriteControl { get; set; }

        public RegisterFloat regRadius;
        public RegisterFloat regCorrect;

        public BIT_UCViewModel()
        {

        }

        public BIT_UCViewModel(MainWindowViewModel mainViewModel, MbWork work, int Slave)
        {
            regRadius = new RegisterFloat()
            {
                Name = "Диаметр трубы",
                CodeFunc = ModbusFunc.HoldingRegister,
                Measure = "мм.",
                Value = 110
            };

            regCorrect = new RegisterFloat()
            {
                Name = "Заданный ток",
                CodeFunc = ModbusFunc.HoldingRegister,
                Value = 1,
                Measure = "А"

            };

            device = new DeviceBIT(mainViewModel, work, Slave);
            device.Start();

            ListInput = new List<RegisterFloat>()
            {
                device.Amper,
                device.Celsius
            };

            ListCommand = new List<Register>()
            {
                device.Address, device.K1, device.K2, device.K3//,  device.K4
            };

            // добавление в список регистров управления
            ListWriteControl = new List<TwoRegister>()
            {
                new TwoRegister() { Register1 = device.Gauss,
                    Register2 = device.K1, },
                new TwoRegister() { Register1 = regRadius,
                    Register2 = device.K2,},
                new TwoRegister() { Register1 = regCorrect,
                    Register2 = device.K3,},
                new TwoRegister() { Register1 = device.Address,
                    Register2 = device.Address, },
            };
        }

        #region Команды

        //--------------------------------------------------------------------------------
        // Команда Отправить значение
        //--------------------------------------------------------------------------------
        public ICommand WriteValueCommand => new LambdaCommand(OnWriteValueCommandExecuted, CanWriteValueCommand);
        private bool CanWriteValueCommand(object p) => device != null;
        private void OnWriteValueCommandExecuted(object p)
        {
            if (p is TwoRegister reg)
            {
                try
                {
                    RegisterFloat regFloat = reg.Register2 as RegisterFloat;

                    if (regFloat?.Address == 0x05)
                    {
                        Task.Run(() =>
                        {
                            SendWaitMessage(regFloat);
                        });
                        return;
                    }
                    else
                    {
                        if (regFloat != null)
                        {
                            regFloat.Value = (reg.Register1 as RegisterFloat).Value;
                            device.WriteRegister(regFloat);
                        }
                        else
                        {
                            RegisterInt regInt = reg.Register2 as RegisterInt;
                            regInt.Value = (reg.Register1 as RegisterInt).Value;
                            device.WriteRegister(regInt);
                        }
                    }

                    Debug.WriteLine("Отправка команды.");
                }
                catch (TimeoutException)
                {
                }
            }
        }

        //--------------------------------------------------------------------------------
        // Команда Сброс
        //--------------------------------------------------------------------------------
        public ICommand ResetCommand => new LambdaCommand(OnResetCommandExecuted, CanResetCommand);
        private bool CanResetCommand(object p) => device != null;
        private void OnResetCommandExecuted(object p)
        {
            Mouse.SetCursor(Cursors.Wait);

            device.K1.Value = DeviceBIT.defK1;
            device.WriteRegister(device.K1);
            Thread.Sleep(2000);

            device.K2.Value = DeviceBIT.defK2;
            device.WriteRegister(device.K2);
            Thread.Sleep(2000);

            device.K3.Value = DeviceBIT.defK3;
            device.WriteRegister(device.K3);

            Mouse.SetCursor(Cursors.None);

        }


        #endregion

        void SendWaitMessage(RegisterFloat regFloat)
        {
            Debug.WriteLine($"Текущий ток: {device.Amper.Value}");

            regFloat.Value = 1;
            Debug.WriteLine("Отправка первой команды с 1.");
            device.WriteRegister(regFloat);

            Thread.Sleep(2000);

            Debug.WriteLine("Чтение результата.");
            device.ReadRegister(device.Amper);

            Debug.WriteLine($"Измеренный ток для 1А: {device.Amper.Value}");

            regFloat.Value = device.Amper.Value == 0 ? 0 : regCorrect.Value / device.Amper.Value;
            
            Debug.WriteLine($"Коэффициент: {regFloat.Value}");

            Debug.WriteLine("Отправка второй команды.");
            device.WriteRegister(regFloat);
        }
    }
}
