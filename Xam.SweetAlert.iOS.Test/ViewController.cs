using Foundation;
using System;
using UIKit;

namespace Xam.SweetAlert.iOS.Test
{
    public partial class ViewController : UIViewController
    {
        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }

        partial void UIButton439_TouchUpInside(UIButton sender)
        {
            SweetAlert alert = null;
          alert=   new SweetAlert().ShowAlert("Are you sure?", subTitle: "You file will permanently delete!", style: AlertStyle.Warning, buttonTitle: "Cancel", buttonColor: Extensions.ToUIColor(0xD0D0D0), otherButtonTitle: "Yes, delete it!", otherButtonColor: Extensions.ToUIColor(0xDD6B55), action: isOtherButton =>
             {
                 
                 if (!isOtherButton)
                 {
                     new SweetAlert().ShowAlert("Deleted!", subTitle: "Your imaginary file has been deleted!", style: AlertStyle.Success);
                 }
             }); 
                
        
            
        }
    }
}