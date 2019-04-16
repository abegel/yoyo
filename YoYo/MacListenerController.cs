using System;

using Foundation;
using AppKit;

namespace YoYo
{
    public partial class MacListenerController : NSWindowController
    {
        public MacListenerController(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public MacListenerController(NSCoder coder) : base(coder)
        {
        }

        public MacListenerController() : base("Listener")
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            MacListenerTextView.TextStorage.Append(new NSAttributedString("hello"));
        }

        public new MacListenerWindow Window
        {
            get { return (MacListenerWindow)base.Window; }
        }
    }
}
