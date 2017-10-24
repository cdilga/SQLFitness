using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SQLFitness
{
    public class TerminalFitness : IFitness
    {
        public double Evaluate(Individual individual)
        {
            var tcpClient = new TcpClient();
            tcpClient.Connect(Utility.FitnessServerAddress, Utility.FitnessServerPort);

            NetworkStream serverStream = tcpClient.GetStream();
            byte[] bytesToSend = UnicodeEncoding.UTF8.GetBytes(new Interpreter().Parse(individual));

            Console.WriteLine("Sending : " + bytesToSend);
            serverStream.Write(bytesToSend, 0, bytesToSend.Length);
            byte[] bytesToRead = new byte[tcpClient.ReceiveBufferSize];
            int bytesRead = serverStream.Read(bytesToRead, 0, tcpClient.ReceiveBufferSize);
            var result = UnicodeEncoding.UTF8.GetString(bytesToRead, 0, bytesRead);
            Console.WriteLine("Received : " + result);
            tcpClient.Close();
            //Console.WriteLine(individual.Genome.Count);
            //Process p = new Process();
            //p.StartInfo.UseShellExecute = false;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.Arguments = "/C help";
            //p.Start();



            //// To avoid deadlocks, always read the output stream first and then wait.
            //string output = p.StandardOutput.ReadToEnd();
            //p.WaitForExit();
            return Convert.ToDouble(result);
        }

        public TerminalFitness()
        {

        }
    }
}
