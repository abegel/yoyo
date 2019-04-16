using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace YoYo
{
    public partial class MacListenerViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public MacListenerViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MacListenerViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MacListenerViewController() : base("MacListenerView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new MacListenerView View
        {
            get
            {
                return (MacListenerView)base.View;
            }
        }
    }
}
