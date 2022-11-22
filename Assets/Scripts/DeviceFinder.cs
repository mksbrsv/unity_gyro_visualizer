using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using UnityEngine;

namespace GyroscopeControl
{
    public class DeviceFinder
    {
        public List<string> TargetDeviceList { get; private set; }
        public string[] DeviceList { get; private set; }
        private SerialPort _deviceChecker;
        private readonly string _contolMessage = "CustomController";
        public DeviceFinder()
        {
            DeviceList = SerialPort.GetPortNames();
            TargetDeviceList = new List<string>();
            _deviceChecker = new SerialPort()
            {
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                ReadTimeout = 1000,
                WriteTimeout = 500
            };
        }

        public void DeviceChecker()
        {
            foreach (string portName in DeviceList)
            {
                _deviceChecker.PortName = portName;
                try
                {
                    _deviceChecker.Open();
                    if (_deviceChecker.IsOpen)
                    {
                        _deviceChecker.Write("get");
                        try
                        {
                            string response = _deviceChecker.ReadLine();
                            if (response.Contains(_contolMessage))
                            {
                                TargetDeviceList.Add(portName);
                            }

                        }
                        catch (TimeoutException ex)
                        {
                            string message = ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                }
                _deviceChecker.Close();
            }
        }
    }
}