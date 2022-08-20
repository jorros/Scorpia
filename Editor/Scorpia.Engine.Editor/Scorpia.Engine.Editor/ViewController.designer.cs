// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Scorpia.Engine.Editor
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSView renderView { get; set; }

		[Action ("testButton:")]
		partial void testButton (Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (renderView != null) {
				renderView.Dispose ();
				renderView = null;
			}

		}
	}
}
