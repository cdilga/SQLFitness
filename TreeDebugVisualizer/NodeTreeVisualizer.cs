using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeDebugVisualizer
{
    // TODO: Add the following to SomeType's definition to see this visualizer when debugging instances of SomeType:
    // 
    //  [DebuggerVisualizer(typeof(XMindStringVisualizer))]
    //  [Serializable]
    //  public class SomeType
    //  {
    //   ...
    //  }
    // 
    /// <summary>
    /// A Visualizer for SomeType.  
    /// </summary>
    public class NodeTreeVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null)
                throw new ArgumentNullException("windowService");
            if (objectProvider == null)
                throw new ArgumentNullException("objectProvider");

            // TODO: Get the object to display a visualizer for.
            //       Cast the result of objectProvider.GetObject() 
            //       to the type of the object being visualized.
            object data = (object)objectProvider.GetObject();
            if (data is IVisualizableNode visNode)
            {
                using (var view = new TreeView())
                {
                    view.Text = "Node debug view";
                    view.RootNode = visNode;
                    windowService.ShowDialog(view);
                }
            }
            else
            {
                MessageBox.Show("Not a " + nameof(IVisualizableNode) + ": can't debug this object with this view.",
                    "Can't visualize object as tree", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // TODO: Add the following to your testing code to test the visualizer:
        // 
        //    XMindStringVisualizer.TestShowVisualizer(new SomeType());
        // 
        /// <summary>
        /// Tests the visualizer by hosting it outside of the debugger.
        /// </summary>
        /// <param name="objectToVisualize">The object to display in the visualizer.</param>
        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(NodeTreeVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }
}
