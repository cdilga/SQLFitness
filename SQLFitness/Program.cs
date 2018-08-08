using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ShellProgressBar;

namespace SQLFitness
{
    static class Program
    {


        static void Main(string[] args) {
            if(!(args.Length >= 1 && File.Exists(args[0]))) {
                Console.WriteLine("Please provide numberbatch file as either .txt or .nmb");
                return;
            }

            var dict = NumberBatch.ReadNumberbatchFileOrCache(args[0]);

            Console.WriteLine($"Finished loading numberbatch. Memory usage: {System.Diagnostics.Process.GetCurrentProcess().VirtualMemorySize64 / 1024 / 1024 }Mb");
            Console.ReadKey();



            return;
            //Create a dbaccess
#if !DEBUG
            var basicGA = new TreeTestAlgorithm(new PerformanceTestFitness());
#else
            var db = new DBAccess();
            var basicGA = new TreeSelectionAlgorithm(db, new ClientFitness());
#endif
            for(var i = 0; i < Utility.MaxIterations; i++) {
                basicGA.Evolve();
                Console.WriteLine($"{i}");
            }
            //db.Close();
            Console.WriteLine("Done");
            Console.ReadLine();
        }

        public static MemoryStream SerializeToStream(object o) {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object DeserializeFromStream(MemoryStream stream) {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            return formatter.Deserialize(stream);
        }
    }
}