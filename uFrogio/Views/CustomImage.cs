using Xamarin.Forms;

namespace uFrogio.Views
{
    public class CustomImageEventArgs
    {
        public CustomImageEventArgs(double translationY) { TranslationY = translationY; }
        public double TranslationY { get; }
    }

    public class CustomImage : Xamarin.Forms.Image
    {
        public delegate void CustomImageEventHandler(CustomImage sender, CustomImageEventArgs e);

        public event CustomImageEventHandler OnTranslationChanged;

        public void CallRelatedRenders() 
        {
            OnTranslationChanged?.Invoke(this, new CustomImageEventArgs(Relation.TranslationY));
        }

        public double MaxY;

        public RelativeLayout Relation
        {
            get { return (RelativeLayout)GetValue(RelationProperty); }
            set { SetValue(RelationProperty, value); }
        }

        public bool UpsideDown
        {
            get { return (bool)GetValue(UpsideDownProperty); }
            set { SetValue(UpsideDownProperty, value); }
        }

        public double MinY
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public double Diff
        {
            get { return (double)GetValue(DiffProperty); }
            set { SetValue(DiffProperty, value); }
        }

        public static readonly BindableProperty RelationProperty = BindableProperty.Create(
    propertyName: "Relation",
    returnType: typeof(RelativeLayout),
    declaringType: typeof(RelativeLayout),
    defaultValue: null);

        public static readonly BindableProperty UpsideDownProperty = BindableProperty.Create(
    propertyName: "UpsideDown",
    returnType: typeof(bool),
    declaringType: typeof(bool),
    defaultValue: false);

        public static readonly BindableProperty MinProperty = BindableProperty.Create(
    propertyName: "MinY",
    returnType: typeof(double),
    declaringType: typeof(double),
    defaultValue: 100.0d);

        public static readonly BindableProperty DiffProperty = BindableProperty.Create(
    propertyName: "Diff",
    returnType: typeof(double),
    declaringType: typeof(double),
    defaultValue: 100.0d);
        
        protected override void OnParentSet()
        {
            Relation = (RelativeLayout)this.Parent;
            if (!UpsideDown)
            {
                MinY = Relation.TranslationY;
                MaxY = MinY + Diff;
            }
            else
            {
                MaxY = Relation.TranslationY;
                MinY = MaxY - Diff;
            }
        }

        public CustomImage()
        {
        }
    }
}