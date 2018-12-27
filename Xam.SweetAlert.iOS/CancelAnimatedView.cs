using CoreAnimation;
using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Xam.SweetAlert.iOS
{
    public class CancelAnimatedView : AnimatableView
    {
        CAShapeLayer circleLayer = new CAShapeLayer();

        CAShapeLayer crossPathLayer = new CAShapeLayer();

        public CancelAnimatedView():base()
        {
            this.SetupLayers();
            var t = CATransform3D.Identity;
            t.m34 = 1.0f / -500.0f;
            t = t.Rotate(Convert.ToSingle(90.0f * Math.PI) / 180.0f, 1f, 0f, 0f);
            circleLayer.Transform = t;
            crossPathLayer.Opacity = 0.0f;
        }

        public override void LayoutSubviews()
        {
            this.SetupLayers();
        }





        private CGPath outlineCircle
        {
            get
            {
                var path = new UIBezierPath();
                var startAngle =Convert.ToSingle( ((0) / 180.0 * Math.PI)); //0
                var endAngle = Convert.ToSingle(((360) / 180.0 * Math.PI));   //360
                path.AddArc(center: new CGPoint(x: this.Frame.Size.Width / 2.0f, y: this.Frame.Size.Width / 2.0f), radius: this.Frame.Size.Width / 2.0f, startAngle: startAngle, endAngle: endAngle, clockWise: false);
                return path.CGPath;
            }
        }



    



        private CGPath crossPath
        {
            get
            {
                var path = new UIBezierPath();
                var factor = this.Frame.Size.Width / 5.0f;
                path.MoveTo(new CGPoint(x: this.Frame.Size.Height / 2.0 - factor, y: this.Frame.Size.Height / 2.0 - factor));
                path.AddLineTo(new CGPoint(x: this.Frame.Size.Height / 2.0 + factor, y: this.Frame.Size.Height / 2.0 + factor));
                path.MoveTo(new CGPoint(x: this.Frame.Size.Height / 2.0 + factor, y: this.Frame.Size.Height / 2.0 - factor));
                path.AddLineTo(new CGPoint(x: this.Frame.Size.Height / 2.0 - factor, y: this.Frame.Size.Height / 2.0 + factor));
                return path.CGPath;
            }
        }



        public void SetupLayers()
        {
            circleLayer.Path = outlineCircle;
            circleLayer.FillColor = UIColor.Clear.CGColor;
            circleLayer.StrokeColor = Extensions.ToUIColor(0xF27474).CGColor;
            circleLayer.LineCap = CAShapeLayer.CapRound;
            circleLayer.LineWidth = 4;
            circleLayer.Frame = new CGRect(x: 0, y: 0, width: this.Frame.Size.Width, height: this.Frame.Size.Height);
            circleLayer.Position = new CGPoint(x: this.Frame.Size.Width / 2.0f, y: this.Frame.Size.Height / 2.0f);
            this.Layer.AddSublayer(circleLayer);
            crossPathLayer.Path = crossPath;
            crossPathLayer.FillColor = UIColor.Clear.CGColor;
            crossPathLayer.StrokeColor = Extensions.ToUIColor(0xF27474).CGColor;
            crossPathLayer.LineCap = CAShapeLayer.CapRound;
            crossPathLayer.LineWidth = 4;
            crossPathLayer.Frame = new CGRect(x: 0, y: 0, width: this.Frame.Size.Width, height: this.Frame.Size.Height);
            crossPathLayer.Position = new CGPoint(x: this.Frame.Size.Width / 2.0, y: this.Frame.Size.Height / 2.0);
            this.Layer.AddSublayer(crossPathLayer);
        }

        public override void Animate()
        {
            var t = CATransform3D.Identity;

            t.m34 = 1.0f / -500.0f;

            t =t.Rotate( (Convert.ToSingle( 90.0 * Math.PI )/ 180.0f), 1f, 0f, 0f);



            var t2 = CATransform3D.Identity;

            t2.m34 = 1.0f / -500.0f;

            t2 = t2.Rotate(Convert.ToSingle(-Math.PI), 1f, 0f, 0f);



            var animation =  CABasicAnimation.FromKeyPath("transform" );

            var time = 0.3f;

        animation.Duration = time;

            animation.From = NSValue.FromCATransform3D(t);

            animation.To = NSValue.FromCATransform3D(t2);

            animation.RemovedOnCompletion = false;

            animation.FillMode = CAFillMode.Forwards;

            this.circleLayer.AddAnimation(animation, "transform");






        var scale = CATransform3D.Identity;

            scale = scale.Scale(0.3f, 0.3f, 0f);






            var crossAnimation = CABasicAnimation.FromKeyPath("transform"); 

        crossAnimation.Duration = 0.3;

            crossAnimation.BeginTime = CAAnimation.CurrentMediaTime() + time;

            crossAnimation.From = NSValue.FromCATransform3D(scale);

            crossAnimation.TimingFunction = new CAMediaTimingFunction(0.25f, 0.8f, 0.7f, 2.0f);

            crossAnimation.To = NSValue.FromCATransform3D(CATransform3D.Identity);

            this.crossPathLayer.AddAnimation(crossAnimation, "scale");




            var fadeInAnimation = CABasicAnimation.FromKeyPath("opacity"); 

        fadeInAnimation.Duration = 0.3;

            fadeInAnimation.BeginTime = CAAnimation.CurrentMediaTime() + time;

            fadeInAnimation.From =NSValue.FromObject(0.3);

            fadeInAnimation.To = NSValue.FromObject(1.0);

            fadeInAnimation.RemovedOnCompletion = false;

        fadeInAnimation.FillMode = CAFillMode.Forwards;

            this.crossPathLayer.AddAnimation(fadeInAnimation, "opacity");
        }
    }
}