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
        public double[] Evaluate(StubIndividual individual)
        {
            double[] output = new double[2];
            var tcpClient = new TcpClient();
            while (!tcpClient.Connected)
            {
                try
                {
                    tcpClient.Connect(Utility.FitnessServerAddress, Utility.FitnessServerPort);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    Console.WriteLine("Connecting failed. Trying again");
                    System.Threading.Thread.Sleep(500);
                }
            }
            
            var interpreter = new Interpreter();
            string sql = interpreter.Parse(individual);

            NetworkStream serverStream = tcpClient.GetStream();
            byte[] bytesToSend = Encoding.UTF8.GetBytes(sql);

            Debug.WriteLine("Sending : " + sql);
            serverStream.Write(bytesToSend, 0, bytesToSend.Length);
            var bytesToRead = new byte[tcpClient.ReceiveBufferSize];

            int bytesRead = serverStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            var result = Encoding.UTF8.GetString(bytesToRead, 0, bytesRead).Substring(2);

            tcpClient.Close();
            Debug.WriteLine("Received : " + result);
            //Console.WriteLine(result.Split('\n')[0]);
            var splitResult = result.Split('\n');
            output[0] = Convert.ToDouble(splitResult[0]);
            output[1] = Convert.ToDouble(splitResult[1]);
            return output;
        }
    }
}