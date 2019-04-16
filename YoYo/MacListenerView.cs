using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace YoYo
{
    public partial class MacListenerView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public MacListenerView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MacListenerView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion
    }
}
