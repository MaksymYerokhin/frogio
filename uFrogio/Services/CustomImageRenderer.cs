using CustomRenderer.iOS;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using uFrogio.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomImage), typeof(CustomImageRenderer))]
namespace CustomRenderer.iOS
{
    public class CustomImageRenderer : Xamarin.Forms.Platform.iOS.ImageRenderer
    {
        public new CustomImage Element;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            this.Element = (CustomImage)base.Element;
        }

        //public override void TouchesBegan(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesBegan(touches, evt);
        //    var touch = touches.AnyObject as UITouch;

            //    if (touch != null)
            //    {
            //        //var target = GetTouchTarget(touch);
            //        //if (target != null)
            //        //    target.RaiseEntered();
            //        //_last = target;
            //    }
            //}

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var header = this.Element.Relation;

                //var header = ((RelativeLayout)this.Element).FindByName<Xamarin.Forms.RelativeLayout>("header");
                float offsetY = (float)(touch.PreviousLocationInView(null).Y - touch.LocationInView(null).Y);
                var newTransY = header.TranslationY - offsetY;
                if (newTransY > this.Element.MaxY)
                {
                    header.TranslationY = this.Element.MaxY;
                }
                else if (newTransY < this.Element.MinY)
                {
                    header.TranslationY = this.Element.MinY;
                }
                else
                {
                    header.TranslationY -= offsetY;
                }

                this.Element.CallRelatedRenders();

                //image.Frame = new CoreGraphics.CGRect(image.Frame.X, image.Frame.Y - offsetY, image.Frame.Width, image.Frame.Height);
                //var target = GetTouchTarget(touch);

                //if (_last != target)
                //{
                //    if (target != null)
                //        target.RaiseEntered();
                //    if (_last != null)
                //        _last.RaiseExited();
                //    _last = target;
                //}
            }
        }

        //public override void TouchesCancelled(NSSet touches, UIEvent evt)
        //{
        //    base.TouchesCancelled(touches, evt);
        //    var touch = touches.AnyObject as UITouch;

        //    if (touch != null)
        //    {
        //        //var target = GetTouchTarget(touch);
        //        //if (target != null)
        //        //    target.RaiseExited();
        //        //_last = null;
        //    }
        //}

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            var touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                var header = this.Element.Relation;
                float offsetY = (float)(touch.PreviousLocationInView(null).Y - touch.LocationInView(null).Y);
                var newTransY = header.TranslationY - offsetY;
                if (newTransY > this.Element.MaxY)
                {
                    header.TranslationY = this.Element.MaxY;
                    this.Element.CallRelatedRenders();
                }
                else if (newTransY < this.Element.MinY)
                {
                    header.TranslationY = this.Element.MinY;
                    this.Element.CallRelatedRenders();
                }
                else
                {
                    var t = (this.Element.UpsideDown 
                            ? (this.Element.MaxY + this.Element.MinY) 
                            : (this.Element.MaxY - this.Element.MinY)) / 2;
                    
                    var task = header.TranslateTo(header.TranslationX, newTransY < t ? this.Element.MinY : this.Element.MaxY, 100, Easing.Linear);
                    task.ContinueWith((Task<bool> t1) => {
                        if(!t1.Result)
                            this.Element.CallRelatedRenders();
                    });
                }                
                
                //var target = GetTouchTarget(touch);
                //if (target != null)
                //    target.RaiseExited();
                //_last = null;
            }
        }

        //private CustomImage GetTouchTarget(UITouch touch)
        //{
        //    CustomImage target = null;
        //    var grid = ((MainPage)this.Element).FindByName<Xamarin.Forms.Grid>("SmartGrid");

        //    var point = touch.LocationInView(this.View);
        //    var x = point.X - grid.X;
        //    var y = point.Y - grid.Y;

        //    // Find which SmartBoxView, if any, the pointer is over
        //    foreach (var child in grid.Children)
        //    {
        //        if (child.Bounds.Contains(x, y))
        //        {
        //            target = (child is CustomImage) ? (CustomImage)child : null;
        //            break;
        //        }
        //    }

        //    // At this point, target == null means the pointer isn't
        //    // over a SmartBoxView; target != null means it is
        //    return target;
        //}
    }
}