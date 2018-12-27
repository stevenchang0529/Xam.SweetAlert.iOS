using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using System.Linq;
using CoreAnimation;

namespace Xam.SweetAlert.iOS
{

    public class SweetAlert : UIViewController
    {
        nfloat kBakcgroundTansperancy = 0.7f;
        nfloat kHeightMargin = 10.0f;
        nfloat KTopMargin = 20.0f;
        nfloat kWidthMargin = 10.0f;
        nfloat kAnimatedViewHeight = 70.0f;
        nfloat kMaxHeight = 300.0f;
        nfloat kContentWidth = 300.0f;
        nfloat kButtonHeight = 35.0f;
        nfloat textViewHeight = 90.0f;
        nfloat kTitleHeight = 30.0f;
        UIView contentView = new UIView();
        UILabel titleLabel = new UILabel();
        List<UIButton> buttons = new List<UIButton>();
        AnimatableView animatedView;
        UIImageView imageView;
        UITextView subTitleTextView = new UITextView();
        Action<bool> userAction;
        string kFont = "Helvetica";

        public SweetAlert() 
        {
            this.View.Frame = UIScreen.MainScreen.Bounds;
            this.View.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
            this.View.BackgroundColor = new UIColor(red: 0, green: 0, blue: 0, alpha: kBakcgroundTansperancy);
            this.View.AddSubview(contentView);
        }

        public SweetAlert(NSCoder coder)
        {
            throw new NotImplementedException();
        }


        private void SetupContentView()
        {
            contentView.BackgroundColor = new UIColor(white: 1.0f, alpha: 1.0f);
            contentView.Layer.CornerRadius = 5.0f;
            contentView.Layer.MasksToBounds = true;
            contentView.Layer.BorderWidth = 0.5f;
            contentView.AddSubview(titleLabel);
            contentView.AddSubview(subTitleTextView);
            contentView.BackgroundColor = 0xFFFFFF.ToUIColor();
            contentView.Layer.BorderColor = Extensions.ToUIColor(0xCCCCCC).CGColor;
            this.View.AddSubview(contentView);
        }

        private void SetupTitleLabel()
        {
            titleLabel.Text = "";
            titleLabel.Lines = 1;
            titleLabel.TextAlignment = UITextAlignment.Center;
            titleLabel.Font = UIFont.FromName(name: kFont, size: 25);
            titleLabel.TextColor = Extensions.ToUIColor(0x575757);
        }

        private void SetupSubtitleTextView()
        {
            subTitleTextView.Text = "";
            subTitleTextView.TextAlignment = UITextAlignment.Center;
            subTitleTextView.Font = UIFont.FromName(name: kFont, size: 16);
            subTitleTextView.TextColor = Extensions.ToUIColor(0x797979);
            subTitleTextView.Editable = false;
        }

        private void ResizeAndRelayout()
        {

            var mainScreenBounds = UIScreen.MainScreen.Bounds;
            this.View.Frame = new CoreGraphics.CGRect(this.View.Frame.Location, mainScreenBounds.Size);
            nfloat x = kWidthMargin;
            nfloat y = KTopMargin;
            nfloat width = kContentWidth - (kWidthMargin * 2);

            if (animatedView != null)
            {
                animatedView.Frame = new CGRect(x: (kContentWidth - kAnimatedViewHeight) / 2.0, y: y, width: kAnimatedViewHeight, height: kAnimatedViewHeight);
                contentView.AddSubview(animatedView);
                y += kAnimatedViewHeight + kHeightMargin;
            }

            if (imageView != null)
            {
                imageView.Frame = new CGRect(x: (kContentWidth - kAnimatedViewHeight) / 2.0, y: y, width: kAnimatedViewHeight, height: kAnimatedViewHeight);
                contentView.AddSubview(imageView);
                y += imageView.Frame.Size.Height + kHeightMargin;
            }

            // Title

            if (!string.IsNullOrEmpty(this.titleLabel.Text))
            {
                titleLabel.Frame = new CGRect(x: x, y: y, width: width, height: kTitleHeight);
                contentView.AddSubview(titleLabel);
                y += kTitleHeight + kHeightMargin;
            }

            // Subtitle

            if (!string.IsNullOrEmpty(this.subTitleTextView.Text))
            {
                var subtitleString = new NSString(subTitleTextView.Text);
                var rect = subtitleString.GetBoundingRect(size: new CGSize(width: width, height: 0.0), options: NSStringDrawingOptions.UsesLineFragmentOrigin, attributes: new UIStringAttributes() { Font = subTitleTextView.Font }, context: null);
                textViewHeight = Convert.ToSingle(Math.Ceiling(Convert.ToDouble(rect.Size.Height))) + 10.0f;
                subTitleTextView.Frame = new CGRect(x: x, y: y, width: width, height: textViewHeight);
                contentView.AddSubview(subTitleTextView);
                y += textViewHeight + kHeightMargin;
            }

            var buttonRect = buttons.Select(c => new NSString(c.Title(UIControlState.Normal)).GetBoundingRect(new CGSize(width: width, height: 0.0), NSStringDrawingOptions.UsesLineFragmentOrigin, new UIStringAttributes() { Font = c.TitleLabel.Font }, null)).ToList();

            nfloat totalWidth = 0.0f;

            if (buttons.Count == 2)
            {
                totalWidth = buttonRect[0].Size.Width + buttonRect[1].Size.Width + kWidthMargin + 40.0f;
            }
            else
            {
                totalWidth = buttonRect[0].Size.Width + 20.0f;
            }

            y += kHeightMargin;

            var buttonX = (kContentWidth - totalWidth) / 2.0f;


            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Frame = new CGRect(x: buttonX, y: y, width: buttonRect[i].Size.Width + 20.0f, height: buttonRect[i].Size.Height + 10.0);
                buttonX = buttons[i].Frame.X + kWidthMargin + buttonRect[i].Size.Width + 20.0f;
                buttons[i].Layer.CornerRadius = 5.0f;
                this.contentView.AddSubview(buttons[i]);
                //buttons[i].AddTarget(new EventHandler(Pressed), UIControlEvent.TouchUpInside);
            }



            y += kHeightMargin + buttonRect[0].Size.Height + 10.0f;

            if (y > kMaxHeight)
            {
                var diff = y - kMaxHeight;
                var sFrame = subTitleTextView.Frame;
                subTitleTextView.Frame = new CGRect(x: sFrame.X, y: sFrame.Y, width: sFrame.Width, height: sFrame.Height - diff);

                foreach (var button in buttons)
                {
                    var bFrame = button.Frame;
                    button.Frame = new CGRect(x: bFrame.X, y: bFrame.Y - diff, width: bFrame.Width, height: bFrame.Height);
                }
                y = kMaxHeight;
            }
            contentView.Frame = new CGRect(x: (mainScreenBounds.Size.Width - kContentWidth) / 2.0f, y: (mainScreenBounds.Size.Height - y) / 2.0f, width: kContentWidth, height: y);
            contentView.ClipsToBounds = true;
        }

        public void Pressed(object sender, EventArgs e)
        {

            var btn = sender as UIButton;
            this.CloseAlert(btn.Tag);
        }

        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();
            var sz = UIScreen.MainScreen.Bounds.Size;

            var sver = UIDevice.CurrentDevice.SystemVersion;

            var ver = Convert.ToSingle(sver);

            if (ver < 8.0)
            {

                // iOS versions before 7.0 did not switch the width and height on device roration
                if (UIApplication.SharedApplication.StatusBarOrientation.IsLandscape())
                {
                    var ssz = sz;
                    sz = new CGSize(width: ssz.Height, height: ssz.Width);
                }
            }

            this.ResizeAndRelayout();
        }




        public void CloseAlert(nint buttonIndex)
        {

            UIView.Animate(0.5, 0.0, UIViewAnimationOptions.CurveEaseOut, () =>
             {
                 this.View.Alpha = 0.0f;

             }, () =>
              {
                  this.View.RemoveFromSuperview();
                  this.CleanUpAlert();
                  if (userAction != null)
                  {
                      var isOtherButton = buttonIndex == 0 ? true : false;
                      SweetAlertContext.ShouldNotAnimate = true;
                      userAction(isOtherButton);
                      SweetAlertContext.ShouldNotAnimate = false;
                  }

              });
        }



        public void CleanUpAlert()
        {
            if (this.animatedView != null)
            {
                this.animatedView.RemoveFromSuperview();
                this.animatedView = null;
            }
            this.contentView.RemoveFromSuperview();
            this.contentView = new UIView();
        }

        public SweetAlert ShowAlert(string title)
        {
            this.ShowAlert(title, null, AlertStyle.None);
            return this;
        }

        public SweetAlert ShowAlert(string title, string subTitle, AlertStyle style, string imageFile = null)
        {
            ShowAlert(title, subTitle, style, buttonTitle: "OK");
            return this;
        }

        public SweetAlert ShowAlert(string title, string subTitle, AlertStyle style, string buttonTitle, string imageFile = null, Action<bool> action = null)
        {
            ShowAlert(title, subTitle, style, buttonTitle, buttonColor: Extensions.ToUIColor(0xAEDEF4));
            userAction = action;
            return this;

        }



        public SweetAlert ShowAlert(string title, string subTitle, AlertStyle style, string buttonTitle, UIColor buttonColor, string imageFile = null, Action<bool> action = null)
        {
            ShowAlert(title, subTitle, style, buttonTitle, buttonColor,otherButtonTitle:null, action: null);
            userAction = action;
            return this;
        }



        public SweetAlert ShowAlert(string title, string subTitle, AlertStyle style, string buttonTitle, UIColor buttonColor, string otherButtonTitle, string imageFile = null, Action<bool> action = null)
        {
            ShowAlert(title, subTitle, style, buttonTitle, buttonColor, otherButtonTitle, UIColor.Red);
            userAction = action;
            return this;
        }



        public SweetAlert ShowAlert(string title, string subTitle, AlertStyle style, string buttonTitle, UIColor buttonColor, string otherButtonTitle, UIColor otherButtonColor, string imageFile = null, Action<bool> action = null)
        {

            userAction = action;
            var window = UIApplication.SharedApplication.KeyWindow;
            window.AddSubview(this.View);
            window.BringSubviewToFront(this.View);
            View.Frame = window.Bounds;
            this.SetupContentView();
            this.SetupTitleLabel();
            this.SetupSubtitleTextView();



            switch (style)
            {
                case AlertStyle.Success:
                    this.animatedView = new SuccessAnimatedView();
                    break;
                case AlertStyle.Error:
                    this.animatedView = new CancelAnimatedView();
                    break;
                case AlertStyle.Warning:
                    this.animatedView = new InfoAnimatedView();
                    break;
                case AlertStyle.CustomImage:
                    this.imageView = new UIImageView() { Image = new UIImage(imageFile) };
                    break;
                case AlertStyle.None:
                    this.animatedView = null;
                    break;
            }



            this.titleLabel.Text = title;

            if (!string.IsNullOrEmpty(subTitle))
            {

                this.subTitleTextView.Text = subTitle;

            }

            buttons = new List<UIButton>();

            if (!string.IsNullOrEmpty(buttonTitle))
            {
                var button = new UIButton(type: UIButtonType.Custom);
                button.SetTitle(buttonTitle, UIControlState.Normal);
                button.BackgroundColor = buttonColor;
                button.UserInteractionEnabled = true;
                button.AddTarget(new EventHandler(Pressed), UIControlEvent.TouchUpInside);
                button.Tag = 0;
                buttons.Add(button);
            }

            if (!string.IsNullOrEmpty(otherButtonTitle))
            {
                var button = new UIButton(type: UIButtonType.Custom);
                button.SetTitle(otherButtonTitle, UIControlState.Normal);
                button.BackgroundColor = otherButtonColor;
                button.AddTarget(new EventHandler(Pressed), UIControlEvent.TouchUpInside);
                button.Tag = 1;
                buttons.Add(button);
            }



            ResizeAndRelayout();

            if (SweetAlertContext.ShouldNotAnimate)
            {
                //Do not animate Alert
                if (this.animatedView != null)
                {
                    this.animatedView.Animate();
                }
            }
            else
            {
                AnimateAlert();
            }
            return this;
        }

        public void AnimateAlert()
        {
            View.Alpha = 0;
            UIView.Animate(0.1, () =>
            {
                this.View.Alpha = 1.0f;
            });
            var previousTransform = this.contentView.Transform;

            this.contentView.Layer.Transform = CATransform3D.MakeScale(0.9f, 0.9f, 0.0f);
            UIView.Animate(0.2,
                animation: () =>
            {
                this.contentView.Layer.Transform = CATransform3D.MakeScale(1.1f, 1.1f, 0.0f);
            },
             completion: () =>
             {
                 UIView.Animate(0.1, () =>
                 {
                     this.contentView.Layer.Transform = CATransform3D.MakeScale(0.9f, 0.9f, 0.0f);
                 },
                 () =>
                 {
                     UIView.Animate(0.1, () =>
                     {
                         this.contentView.Layer.Transform = CATransform3D.MakeScale(1.1f, 1.1f, 0.0f);
                         if(animatedView!=null)
                             this.animatedView.Animate();
                     },()=>
                     {
                         this.contentView.Transform = previousTransform;
                     });

                 });
             });
        }

    }
}
