using ShellProgressBar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public static class NumberBatch
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

            if(String.Equals(Path.GetExtension(path), ".nmb", StringComparison.OrdinalIgnoreCase)) {
                return ReadFromNmbFile(path);
            }
            else if(String.Equals(Path.GetExtension(path), ".txt", StringComparison.OrdinalIgnoreCase)) {
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
    }
}
