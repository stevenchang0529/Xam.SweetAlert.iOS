using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Xam.SweetAlert.iOS
{
    public class InfoAnimatedView : AnimatableView
    {
        CAShapeLayer circleLayer = new CAShapeLayer();
        CAShapeLayer crossPathLayer = new CAShapeLayer();

        public InfoAnimatedView()
        {
            SetupLayers();
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
                var factor = this.Frame.Width / 1.5f;
                path.MoveTo(new CGPoint(x: this.Frame.Size.Width / 2.0, y: 15.0));
                path.AddLineTo(new CGPoint(x: this.Frame.Size.Width / 2.0, y: factor));
                path.MoveTo(new CGPoint(x: this.Frame.Size.Width / 2.0, y: factor + 10.0));
                path.AddArc(center: new CGPoint(x: this.Frame.Size.Width / 2.0, y: factor + 10.0), radius: 1.0f, startAngle: startAngle, endAngle: endAngle, clockWise: true);
                return path.CGPath;
            }
        }

        public void SetupLayers()
        {
            circleLayer.Path = outlineCircle;
            circleLayer.FillColor = UIColor.Clear.CGColor;
            circleLayer.StrokeColor = Extensions.ToUIColor(0xF8D486).CGColor;
            circleLayer.LineCap = CAShapeLayer.CapRound;
            circleLayer.LineWidth = 4;
            circleLayer.Frame = new CGRect(x: 0, y: 0, width: this.Frame.Size.Width, height: this.Frame.Size.Height);
            circleLayer.Position = new CGPoint(x: this.Frame.Size.Width / 2.0, y: this.Frame.Size.Height / 2.0);
            this.Layer.AddSublayer(circleLayer);
        }

        public override void Animate()
        {
            var colorAnimation =  CABasicAnimation.FromKeyPath("strokeColor" );
            colorAnimation.Duration = 1.0;
            colorAnimation.RepeatCount = float.PositiveInfinity;
            colorAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            colorAnimation.AutoReverses = true;
            colorAnimation.From = NSValue.FromObject(Extensions.ToUIColor(0xF7D58B).CGColor);
            colorAnimation.To = NSValue.FromObject(Extensions.ToUIColor(0xF2A665).CGColor);
            circleLayer.AddAnimation(colorAnimation, "strokeColor");
        }
    }
}