using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SQLFitness
{
    public class ClientFitness : IFitness
    {
        public double Evaluate(StubIndividual individual)
        {
            var tcpClient = new TcpClient();
            while (!tcpClient.Connected)
            {
                try
                {
                    tcpClient.Connect(Utility.FitnessServerAddress, Utility.FitnessServerPort);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine("Connecting failed. Try again?");
                    Console.ReadLine();
                }
            }
            
            var interpreter = new Interpreter();
            String sql = interpreter.Parse(individual);

            NetworkStream serverStream = tcpClient.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(sql);

            Console.WriteLine("Sending : " + sql);
            serverStream.Write(bytesToSend, 0, bytesToSend.Length);
            var bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = serverStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            var result = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead).Substring(2);

            tcpClient.Close();
            Console.WriteLine("Received : " + result);
            return Convert.ToDouble(result);
        }
    }
}
