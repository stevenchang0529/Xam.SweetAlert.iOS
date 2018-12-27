using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Xam.SweetAlert.iOS
{
    public class SuccessAnimatedView : AnimatableView
    {

        CAShapeLayer circleLayer = new CAShapeLayer();
        CAShapeLayer outlineLayer = new CAShapeLayer();

        public SuccessAnimatedView()
        {
            SetupLayers();
            circleLayer.StrokeStart = 0.0f;
            circleLayer.StrokeEnd = 0.0f;
        }

        public override void LayoutSubviews()
        {
            SetupLayers();
        }

        private CGPath outlineCircle
        {
            get
            {
                var path = new UIBezierPath();
                var startAngle = Convert.ToSingle(((0) / 180.0 * Math.PI)); //0
                var endAngle = Convert.ToSingle(((360) / 180.0 * Math.PI));   //360
                path.AddArc(center: new CGPoint(x: this.Frame.Size.Width / 2.0f, y: this.Frame.Size.Width / 2.0f), radius: this.Frame.Size.Width / 2.0f, startAngle: startAngle, endAngle: endAngle, clockWise: false);
                return path.CGPath;
            }
        }





        private CGPath path
        {
            get
            {
                var path = new UIBezierPath();
                var startAngle = Convert.ToSingle(((60) / 180.0 * Math.PI)); //0
                var endAngle = Convert.ToSingle(((200) / 180.0 * Math.PI));   //360
                path.AddArc(center: new CGPoint(x: this.Frame.Size.Width / 2.0f, y: this.Frame.Size.Width / 2.0f), radius: this.Frame.Size.Width / 2.0f, startAngle: startAngle, endAngle: endAngle, clockWise: false);
                path.AddLineTo(new CGPoint(x: 36.0 - 10.0, y: 60.0 - 10.0));
                path.AddLineTo(new CGPoint(x: 85.0 - 20.0, y: 30.0 - 20.0));
                return path.CGPath;
            }
        }




        public void SetupLayers()
        {

            outlineLayer.Position = new CGPoint(x: 0, y: 0);
            outlineLayer.Path = outlineCircle;
            outlineLayer.FillColor = UIColor.Clear.CGColor;
            outlineLayer.StrokeColor = new UIColor(red: 150.0f / 255.0f, green: 216.0f / 255.0f, blue: 115.0f / 255.0f, alpha: 1.0f).CGColor;
            outlineLayer.LineCap = CAShapeLayer.CapRound;
            outlineLayer.LineWidth = 4;
            outlineLayer.Opacity = 0.1f;
            this.Layer.AddSublayer(outlineLayer);

            circleLayer.Position = new CGPoint(x: 0, y: 0);
            circleLayer.Path = path;
            circleLayer.FillColor = UIColor.Clear.CGColor;
            circleLayer.StrokeColor = new UIColor(red: 150.0f / 255.0f, green: 216.0f / 255.0f, blue: 115.0f / 255.0f, alpha: 1.0f).CGColor;
            circleLayer.LineCap = CAShapeLayer.CapRound;
            circleLayer.LineWidth = 4;
            circleLayer.Actions = Foundation.NSDictionary<NSString, NSObject>.FromObjectsAndKeys
                (
                new NSObject[] { new NSNull(), new NSNull(), new NSNull() },
                new NSString[] { new NSString("strokeStart"), new NSString("strokeEnd"), new NSString("transform") }
                );

            this.Layer.AddSublayer(circleLayer);
        }




        public override void Animate()
        {
            var strokeStart = CABasicAnimation.FromKeyPath("strokeStart");
            var strokeEnd = CABasicAnimation.FromKeyPath("strokeEnd");
            var factor = 0.045f;
            strokeEnd.From = NSValue.FromObject(0.00);
            strokeEnd.To = NSValue.FromObject(0.93);
            strokeEnd.Duration = 10.0 * factor;
            var timing = new CAMediaTimingFunction(0.3f, 0.6f, 0.8f, 1.2f);
            strokeEnd.TimingFunction = timing;

            strokeStart.From = NSValue.FromObject(0.00);
            strokeStart.To = NSValue.FromObject(0.68);
            strokeStart.Duration = 7.0 * factor;
            strokeStart.BeginTime = CAAnimation.CurrentMediaTime() + 3.0 * factor;
            strokeStart.FillMode = CAFillMode.Backwards;
            strokeStart.TimingFunction = timing;
            circleLayer.StrokeStart = 0.68f;
            circleLayer.StrokeEnd = 0.93f;
            this.circleLayer.AddAnimation(strokeEnd, "strokeEnd");
            this.circleLayer.AddAnimation(strokeStart, "strokeStart");
        }
    }
}