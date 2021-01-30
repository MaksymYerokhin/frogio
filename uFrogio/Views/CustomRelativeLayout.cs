using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace uFrogio.Views
{
    public class CustomRelativeLayout : Xamarin.Forms.RelativeLayout
    {
        public double Control
        {
            get { return (double)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        public static readonly BindableProperty ControlProperty = BindableProperty.Create(
    propertyName: "Control",
    returnType: typeof(RelativeLayout),
    declaringType: typeof(RelativeLayout),
    defaultValue: null);

        

        public CustomRelativeLayout()
        {
            
        }
    }
}
