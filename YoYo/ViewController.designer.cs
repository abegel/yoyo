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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        AppKit.NSTextView MacListener { get; set; }

        [Outlet]
        AppKit.NSScrollView MacListenerScrollView { get; set; }
        
        void ReleaseDesignerOutlets ()
        {
            if (MacListenerScrollView != null) {
                MacListenerScrollView.Dispose ();
                MacListenerScrollView = null;
            }

            if (MacListener != null) {
                MacListener.Dispose ();
                MacListener = null;
            }
        }
    }
}
