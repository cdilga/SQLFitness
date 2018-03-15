using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeDebugVisualizer;

namespace StandaloneTreeVisualizer
{
    public static class Program
    {
        public static string ExeFileName => typeof(Program).Assembly.Location;


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] rootNodeMemoryMapName)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (rootNodeMemoryMapName.Length != 1)
            {
                MessageBox.Show("Must have root node memory map name as only parameter");
                return;
            }
            try
            {
                var view = new TreeDebugVisualizer.TreeView();

                using (var fs = File.OpenRead(rootNodeMemoryMapName[0]))
                {
                    view.RootNode = (IVisualizableNode)DeserializeFromStream(fs);
                }

                Application.Run(view);
            }
            catch(Exception e)
            {
                MessageBox.Show("Something went wrong: " + e.ToString(), "Something went wrong starting standalone tree view", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static object DeserializeFromStream(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }
    }
}
