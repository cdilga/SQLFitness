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
        private static readonly ProgressBarOptions standardProgressOptions = new ProgressBarOptions() {
            ForegroundColor = ConsoleColor.DarkMagenta,
            ForegroundColorDone = ConsoleColor.Green,
            DisplayTimeInRealTime = true,
            CollapseWhenFinished = true,
            ProgressCharacter = '─'
        };

        public static IDictionary<string, float[]> ReadFromNumberbatchText(string path) {
            using(var bar = new ProgressBar(3, "Loading numberbatch text file", standardProgressOptions)) {
                if(!File.Exists(path)) {
                    throw new FileNotFoundException("Please provide location of numberbatch txt file");
                }
                bar.Tick();
                var lines = File.ReadAllLines(path);
                bar.Tick();
                var matrixSize = lines[0].Split(' ').Select(Int32.Parse).ToArray();
                var numWords = matrixSize[0];
                ConcurrentDictionary<string, float[]> numberbatch;
                using(var parsingBar = bar.Spawn(numWords, "Parsing numberbatch file", standardProgressOptions)) {
                    var ticker = parsingBar.ThrottleTicks();
                    numberbatch = new ConcurrentDictionary<string, float[]>(Environment.ProcessorCount, numWords);
                    Parallel.For(1, numWords + 1, i => {
                        var line = lines[i].Split(' ');
                        if(line.Length != matrixSize[1] + 1) {
                            Console.WriteLine($"Incorrect vector - dimension mismatch should be word + {matrixSize[1]} numbers. Line is: \"\n   {line}\"");
                            return;
                        }
                        var vector = line.Skip(1).Select(Single.Parse).ToArray();
                        numberbatch.TryAdd(line[0], vector);
                        ticker.OnNext(Unit.Default);
                    });
                }
                bar.Tick();
                return numberbatch;
            }
        }

        public static void WriteToNmbFile(string path, IDictionary<string, float[]> dict) {
            using(var bar = new ProgressBar(dict.Count, "Writing to cache nmb file", standardProgressOptions))
            using(var stream = File.OpenWrite(path))
            using(var writer = new BinaryWriter(stream)) {
                var ticker = bar.ThrottleTicks();
                writer.Write(dict.Count);
                var c = 0;
                foreach(var key in dict.Keys) {
                    writer.Write(key);
                    var val = dict[key];
                    writer.Write(val.Length);
                    for(int i = 0; i < val.Length; ++i) {
                        writer.Write(val[i]);
                    }
                    ticker.OnNext(Unit.Default);
                }
            }
        }

        public static IDictionary<string, float[]> ReadFromNmbFile(string path) {
            using(var stream = File.OpenRead(path))
            using(var reader = new BinaryReader(stream)) {
                int numWords = reader.ReadInt32();

                using(var bar = new ProgressBar(numWords, "Read from nmb file", standardProgressOptions)) {
                    var ticker = bar.ThrottleTicks();
                    var dict = new Dictionary<string, float[]>(numWords);
                    for(int i = 0; i < numWords; i++) {
                        var key = reader.ReadString();
                        var len = reader.ReadInt32();
                        float[] vals = new float[len];
                        for(int j = 0; j < len; j++) {
                            vals[j] = reader.ReadSingle();
                        }
                        dict.Add(key, vals);
                        ticker.OnNext(Unit.Default);
                    }
                    return dict;
                }
            }
        }

        public static IDictionary<string, float[]> ReadNumberbatchFileOrCache(string path) {
            if(!File.Exists(path)) {
                throw new FileNotFoundException("Could not find numberbatch file");
            }

            if(string.Equals(Path.GetExtension(path), ".nmb", StringComparison.OrdinalIgnoreCase)) {
                return ReadFromNmbFile(path);
            }
            else if(string.Equals(Path.GetExtension(path), ".txt", StringComparison.OrdinalIgnoreCase)) {
                var nmbCache = Path.ChangeExtension(path, ".nmb");
                if(File.Exists(nmbCache)) {
                    return ReadFromNmbFile(nmbCache);
                }
                else {
                    var dict = ReadFromNumberbatchText(path);
                    WriteToNmbFile(nmbCache, dict);
                    return dict;
                }
            }
            else {
                throw new FileLoadException("Invalid numberbatch file extension: please use .txt or .nmb file");
            }
        }

        static void Main(string[] args) {
            if(!(args.Length >= 1 && File.Exists(args[0]))) {
                Console.WriteLine("Please provide numberbatch file as either .txt or .nmb");
                return;
            }

            var dict = ReadNumberbatchFileOrCache(args[0]);

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