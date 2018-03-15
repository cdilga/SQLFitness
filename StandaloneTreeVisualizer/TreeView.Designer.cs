namespace TreeDebugVisualizer
{
    partial class TreeView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.diagramView = new MindFusion.Diagramming.WinForms.DiagramView();
            this.treeDiagram = new MindFusion.Diagramming.Diagram();
            this.SuspendLayout();
            // 
            // diagramView
            // 
            this.diagramView.Diagram = this.treeDiagram;
            this.diagramView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramView.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.diagramView.LicenseKey = null;
            this.diagramView.Location = new System.Drawing.Point(0, 0);
            this.diagramView.Name = "diagramView";
            this.diagramView.Size = new System.Drawing.Size(507, 388);
            this.diagramView.TabIndex = 0;
            this.diagramView.Text = "diagramView1";
            this.diagramView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.diagramView_KeyDown);
            // 
            // treeDiagram
            // 
            this.treeDiagram.DefaultShape = MindFusion.Diagramming.Shape.FromId("Rectangle");
            this.treeDiagram.DiagramStyle.ShadowBrush = new MindFusion.Drawing.SolidBrush("#00FFFFFF");
            this.treeDiagram.TouchThreshold = 0F;
            // 
            // TreeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 388);
            this.Controls.Add(this.diagramView);
            this.Name = "TreeView";
            this.Text = "TreeView";
            this.ResumeLayout(false);

        }

        #endregion

        private MindFusion.Diagramming.WinForms.DiagramView diagramView;
        private MindFusion.Diagramming.Diagram treeDiagram;
    }
}