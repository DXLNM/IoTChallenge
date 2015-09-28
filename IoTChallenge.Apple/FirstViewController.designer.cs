// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace IoTChallenge.Apple
{
	[Register ("FirstViewController")]
	partial class FirstViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnGetDocument { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblDescription { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblRemainingItems { get; set; }

		[Action ("UIButton37_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton37_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (btnGetDocument != null) {
				btnGetDocument.Dispose ();
				btnGetDocument = null;
			}
			if (lblDescription != null) {
				lblDescription.Dispose ();
				lblDescription = null;
			}
			if (lblRemainingItems != null) {
				lblRemainingItems.Dispose ();
				lblRemainingItems = null;
			}
		}
	}
}
