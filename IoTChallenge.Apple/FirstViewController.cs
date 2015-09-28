using System;
using System.Drawing;
using System.Collections.Generic;
using Foundation;
using UIKit;
using IoTChallenge.Universal.Core.Classes;
using IoTChallenge.Universal.Core;

namespace IoTChallenge.Apple
{
    public partial class FirstViewController : UIViewController
    {
        public FirstViewController(IntPtr handle) : base(handle)
        {
            this.Title = NSBundle.MainBundle.LocalizedString("First", "First");
            this.TabBarItem.Image = UIImage.FromBundle("Images/first");
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            // Clear any references to subviews of the main view in order to
            // allow the Garbage Collector to collect them sooner.
            //
            // e.g. myOutlet.Dispose (); myOutlet = null;

            ReleaseDesignerOutlets();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }

        #endregion

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return true;
        }

        async partial void UIButton37_TouchUpInside(UIButton sender)
        {
            CameraSensorDocument document = await documentDBUtilities.getCameraDocument("bBNBAPFVBgAMAAAAAAAAAA==");

            (new UIAlertView("Straight from C# PCL","Consuming data from DocumentDB", null, "Cancel", "Ok")).Show();
            lblDescription.Text = document.description;
            lblRemainingItems.Text = String.Format("Quedan {0} {1} en este kiosco",document.unitsRemaining, document.product);
        }
    }
}