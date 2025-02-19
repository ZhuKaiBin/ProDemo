using System;
using System.IO.Ports;
using System.Management;


namespace SimpleReadSerialPortSln
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string[] portNames = System.IO.Ports.SerialPort.GetPortNames();
            foreach (var port in portNames)
            {
                Console.WriteLine("正在查询端口：" + port);
                string query = $"SELECT * FROM Win32_SerialPort WHERE DeviceID = '{port}'";
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    // 打印设备描述
                    Console.WriteLine("设备名称: " + queryObj["Name"]);
                    Console.WriteLine("设备ID: " + queryObj["DeviceID"]);
                    Console.WriteLine("设备描述: " + queryObj["Description"]);
                    Console.WriteLine("硬件ID: " + queryObj["PNPDeviceID"]);
                    Console.WriteLine();
                }
            }
        }
    }
}
