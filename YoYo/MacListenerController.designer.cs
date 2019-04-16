// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace YoYo
{
	[Register ("ListenerController")]
	partial class MacListenerController
	{
		[Outlet]
		AppKit.NSScrollView MacListener { get; set; }

		[Outlet]
		AppKit.NSTextView MacListenerTextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (MacListener != null) {
				MacListener.Dispose ();
				MacListener = null;
			}

			if (MacListenerTextView != null) {
				MacListenerTextView.Dispose ();
				MacListenerTextView = null;
			}
		}
	}
}
