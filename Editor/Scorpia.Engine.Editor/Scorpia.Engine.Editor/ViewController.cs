using System;
using AppKit;
using Foundation;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.Editor
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var engine = new EditorEngine();
            engine.Run(false, renderView.Handle);
        }

        public override NSObject RepresentedObject
        {
            get { return base.RepresentedObject; }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}