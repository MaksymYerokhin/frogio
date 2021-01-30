using uFrogio.ViewModels;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;
using System.Reflection;
using System.Drawing;
using Xamarin.Essentials;
using DependencyServiceDemos.iOS;
using Xamarin.Forms.Markup.LeftToRight;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace uFrogio.Views
{
    //[Serializable]
    //public class Task
    //{
    //    public Task() { }
    //    public Task(JToken jTask)
    //    {

    //        UserId = (string)jTask["UserId"];
    //        Description = (string)jTask["Text"];
    //        DueDate = (DateTime)jTask["LastUpdateTime"];
    //        IsDone = (bool)jTask["IsDone"];
    //    }
    //    public string UserId { get; set; }
    //    //string Name { get; set; }
    //    public string Description { get; set; }
    //    public DateTime DueDate { get; set; }
    //    //bool IsRoutine { get; set; }
    //    public bool IsDone { get; set; }
    //}

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        DateTime dateForSaveResult;
        DateTime dateForKeyboardHeight;
        MainViewModel viewModel;
        private HashSet<int> hs = new HashSet<int>();
        private bool _isAddPanelsShown;
        private bool _isAddPanelOpen;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MainViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            KeyboardHeightService service = DependencyService.Get<KeyboardHeightService>();
            service.KeyboardHeightChanged.Subscribe((float keyboardHeight) =>
            {
                lock (viewModel)
                {
                    if (dateForKeyboardHeight == default)
                    {
                        dateForKeyboardHeight = DateTime.Now;
                        AddLayout.TranslationY -= keyboardHeight;
                    }
                    else
                    {
                        TimeSpan? span = DateTime.Now - dateForKeyboardHeight;
                        dateForKeyboardHeight = DateTime.Now;
                        if (span.Value.TotalSeconds > 1)
                        {
                            AddLayout.TranslationY -= keyboardHeight;
                        }
                    }
                }
            });
            //todayCollectionView.ChildAdded += ResizeCollection;
            viewModel.TodayTasks.CollectionChanged += ResizeTodayCollection;
            viewModel.TomorrowTasks.CollectionChanged += ResizeTomorrowCollection;
            viewModel.WeekTasks.CollectionChanged += ResizeWeekCollection;
            //taskEdit.Completed += Entry1_TextChanged;
            //weekCollectionView.BindingContextChanged += ResizeCollection;
            //weekCollectionView.ChildAdded += ResizeCollection;
            //Device.StartTimer(new TimeSpan(0, 0, 2), () => {
            //    this.NewTaskPanel.AnchorY = 300;
            //    return false;
            //});

            //TaskPanelControl.OnTranslationChanged += TaskPanelControlHandler;
            //EventPanelControl.OnTranslationChanged += EventPanelControlHandler;
            CalendarControl.OnTranslationChanged += CalendarControlHandler;

            if (viewModel.TodayTasks.Count == 0)
                viewModel.IsBusy = true;



            //calendar_holder.Children.Add();
            //month_layout = new Grid
            //{
            //    ColumnSpacing = 2,
            //    RowSpacing = 2,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //};
        }

        void TaskPanelControlHandler(CustomImage sender, CustomImageEventArgs e) {
            var rel = 1 - (sender.MaxY - e.TranslationY) / sender.Diff;
            NewEventPanel.FadeTo(rel, 0, Easing.Linear);
            NewTaskPanel.FadeTo(1, 0, Easing.Linear);
        }

        void CalendarControlHandler(CustomImage sender, CustomImageEventArgs e)
        {
            //var rel = 1 - (sender.MaxY - e.TranslationY) / sender.Diff;
            //NewTaskPanel.FadeTo(rel, 0, Easing.Linear);
            //NewEventPanel.FadeTo(1, 0, Easing.Linear);
        }

        void EventPanelControlHandler(CustomImage sender, CustomImageEventArgs e)
        {
            var rel = 1 - (sender.MaxY - e.TranslationY) / sender.Diff;
            NewTaskPanel.FadeTo(rel, 0, Easing.Linear);
            NewEventPanel.FadeTo(1, 0, Easing.Linear);
        }

        void OnTaskPanelImageTapped(object sender, MR.Gestures.DownUpEventArgs args)
        {
            if (_isAddPanelOpen)
            {
                _isAddPanelsShown = false;
                _isAddPanelOpen = false;
                NewEventPanel.FadeTo(1, 200, Easing.Linear);
                var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, 0, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                AddLayout.LayoutTo(rect, 200, Easing.Linear);
                AddButtonImage.RotateTo(0, 200, Easing.Linear);
                AddButtonImage.ScaleTo(1, 200, Easing.Linear);
                MenuButtonImage.ScaleTo(1, 200, Easing.Linear);
            }
            else
            {
                _isAddPanelOpen = true;
                NewEventPanel.FadeTo(0, 200, Easing.Linear);
                var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, -400, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                AddLayout.LayoutTo(rect, 200, Easing.Linear);
            }
        }

        void OnEventPanelImageTapped(object sender, MR.Gestures.DownUpEventArgs args)
        {
            if (_isAddPanelOpen)
            {
                _isAddPanelsShown = false;
                _isAddPanelOpen = false;
                NewTaskPanel.FadeTo(1, 200, Easing.Linear);
                var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, 0, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                AddLayout.LayoutTo(rect, 200, Easing.Linear);
                AddButtonImage.RotateTo(0, 200, Easing.Linear);
                AddButtonImage.ScaleTo(1, 200, Easing.Linear);
                MenuButtonImage.ScaleTo(1, 200, Easing.Linear);
            }
            else
            {
                _isAddPanelOpen = true;
                NewTaskPanel.FadeTo(0, 200, Easing.Linear);
                var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, -400, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                AddLayout.LayoutTo(rect, 200, Easing.Linear);
            }
        }

        void NewTaskPanning(object sender, MR.Gestures.PanEventArgs e)
        {
            //AddLayout.TranslationY += e.DeltaDistance.Y;
            Console.WriteLine(e.DeltaDistance.Y);
            Console.WriteLine(e.TotalDistance.Y);
            //Console.WriteLine(e.ViewPosition.X);
            Console.WriteLine(e.Velocity.Y);

            //Console.WriteLine(e.Sender.);
            //e.ViewPosition;
            //Console.WriteLine("BoxViewXaml.Red_LongPressed method called");
        }

        //public override void ViewDidLoad()
        //{
        //    base.ViewDidLoad();

        //    // Keyboard popup
        //    NSNotificationCenter.DefaultCenter.AddObserver
        //    (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

        //    // Keyboard Down
        //    //NSNotificationCenter.DefaultCenter.AddObserver
        //    //(UIKeyboard.WillHideNotification, KeyBoardDownNotification);

        //}

        //private void KeyBoardUpNotification(NSNotification notification)
        //{
        //    if (!InputTextView.IsFirstResponder) return;

        //    // get the keyboard size
        //    CGRect r = UIKeyboard.BoundsFromNotification(notification);

        //    ContentInputTextViewBottomConstraint.Constant = r.Height;
        //}

        //private void KeyboardDidShowNotification(NSNotification notification)
        //{
        //    UIView activeView = View.FindFirstResponder();
        //    if (activeView == null)
        //        return;

        //    ((UITextField)activeView).ShowDoneButtonOnKeyboard();

        //    UIScrollView scrollView = activeView.FindSuperviewOfType(this.View, typeof(UIScrollView)) as UIScrollView;
        //    if (scrollView == null)
        //        return;

        //    RectangleF keyboardBounds = UIKeyboard.BoundsFromNotification(notification);

        //    UIEdgeInsets contentInsets = new UIEdgeInsets(0.0f, 0.0f, keyboardBounds.Size.Height, 0.0f);
        //    scrollView.ContentInset = contentInsets;
        //    scrollView.ScrollIndicatorInsets = contentInsets;

        //    // If activeField is hidden by keyboard, scroll it so it's visible
        //    RectangleF viewRectAboveKeyboard = new RectangleF(this.View.Frame.Location, new SizeF(this.View.Frame.Width, this.View.Frame.Size.Height - keyboardBounds.Size.Height));

        //    RectangleF activeFieldAbsoluteFrame = activeView.Superview.ConvertRectToView(activeView.Frame, this.View);
        //    // activeFieldAbsoluteFrame is relative to this.View so does not include any scrollView.ContentOffset

        //    // Check if the activeField will be partially or entirely covered by the keyboard
        //    if (!viewRectAboveKeyboard.Contains(activeFieldAbsoluteFrame))
        //    {
        //        // Scroll to the activeField Y position + activeField.Height + current scrollView.ContentOffset.Y - the keyboard Height
        //        PointF scrollPoint = new PointF(0.0f, activeFieldAbsoluteFrame.Location.Y + activeFieldAbsoluteFrame.Height + scrollView.ContentOffset.Y - viewRectAboveKeyboard.Height);
        //        scrollView.SetContentOffset(scrollPoint, true);
        //    }
        //}

        void Entry1_TextChanged(object sender, EventArgs e)
        {
            //var qwe1 = Application.Current.MainPage.Width;
            //var qwe2 = Application.Current.MainPage.Height;
            //var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            AddLayout.TranslationY = 500;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mayer1995-001-site3.itempurl.com/Task/AddTask");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            request.KeepAlive = false;
            string s;
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var newTask = new ViewModels.Task() {
                    UserID = 1,
                    TypeID = 1,
                    TaskTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    Text = ((Editor)sender).Text,
                    IsDone = false
                };
                s = JsonConvert.SerializeObject(newTask);

                streamWriter.Write(s);
            }
            
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var qwe = new ViewModels.Task()
                {
                    TaskTime = DateTime.Now,
                    Text = ((Editor)sender).Text,
                    IsDone = false
                };

                var str = qwe.GetHashCode();
                lock (viewModel)
                {
                    if (dateForSaveResult == default)
                    {
                        dateForSaveResult = DateTime.Now;
                        viewModel.TodayTasks.Add(qwe);
                    }
                    else
                    {
                        TimeSpan? span = DateTime.Now - dateForSaveResult;
                        dateForSaveResult = DateTime.Now;
                        if (span.Value.TotalSeconds > 1)
                        {
                            viewModel.TodayTasks.Add(qwe);
                        }
                    }
                }
            }
        }

        public void ResizeTodayCollection(object sender, EventArgs e)
        {
            var count = viewModel.TodayTasks.Count;// todayCollectionView.GetChildElements(new Point(todayCollectionView.X, todayCollectionView.Y));
            todayCollectionView.HeightRequest = 10 + 40 * count;

            //var count2 = weekCollectionView.GetChildElements(new Point(weekCollectionView.X, weekCollectionView.Y)).Count;
            //weekCollectionView.HeightRequest = 30 * count2;
        }

        public void ResizeTomorrowCollection(object sender, EventArgs e)
        {
            //switch (sender) { 
            //    case 
            //}
            var count = viewModel.TomorrowTasks.Count;// todayCollectionView.GetChildElements(new Point(todayCollectionView.X, todayCollectionView.Y));
            //var count = chs != null ? chs.Count : 0;
            tomorrowCollectionView.HeightRequest = 10 + 40 * count;

            //var count2 = weekCollectionView.GetChildElements(new Point(weekCollectionView.X, weekCollectionView.Y)).Count;
            //weekCollectionView.HeightRequest = 30 * count2;
        }

        public void ResizeWeekCollection(object sender, EventArgs e)
        {
            //switch (sender) { 
            //    case 
            //}
            var count = viewModel.WeekTasks.Count;// todayCollectionView.GetChildElements(new Point(todayCollectionView.X, todayCollectionView.Y));
            //var count = chs != null ? chs.Count : 0;
            weekCollectionView.HeightRequest = 10 + 40 * count;

            //var count2 = weekCollectionView.GetChildElements(new Point(weekCollectionView.X, weekCollectionView.Y)).Count;
            //weekCollectionView.HeightRequest = 30 * count2;
        }

        //void OnScrollViewScrolled(object sender, ScrolledEventArgs e)
        //{
        //    header.TranslationY = 500;
        //    Console.WriteLine($"ScrollX: {e.ScrollX}, ScrollY: {e.ScrollY}");
        //}

        //TouchesBegan

        void OnAddTaskImageTapped(object sender, EventArgs args)
        {
            //var renderer = Platform.GetRenderer(page);
            //if (renderer == null)
            //{
            //    renderer = RendererFactory.GetRenderer(page);
            //    Platform.SetRenderer(page, renderer);
            //}
            //var viewController = renderer.ViewController;

            try
            {
                if (_isAddPanelsShown)
                {
                    _isAddPanelsShown = false;
                    _isAddPanelOpen = false;
                    var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, 0, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                    AddLayout.LayoutTo(rect, 200, Easing.Linear);
                    //AddLayout.TranslationY = 620;

                    NewEventPanel.FadeTo(1, 200, Easing.Linear);
                    NewTaskPanel.FadeTo(1, 200, Easing.Linear);

                    AddButtonImage.RotateTo(0, 200, Easing.Linear);
                    AddButtonImage.ScaleTo(1, 200, Easing.Linear);
                    MenuButtonImage.ScaleTo(1, 200, Easing.Linear);
                }
                else
                {
                    _isAddPanelsShown = true;
                    var rect = new Xamarin.Forms.Rectangle(AddLayout.Bounds.X, -150, AddLayout.Bounds.Width, AddLayout.Bounds.Height);
                    AddLayout.LayoutTo(rect, 200, Easing.Linear);
                    //AddLayout.TranslationY = 500;

                    AddButtonImage.RotateTo(180, 200, Easing.Linear);
                    AddButtonImage.ScaleTo(1.5, 200, Easing.Linear);
                    MenuButtonImage.ScaleTo(0, 200, Easing.Linear);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void taskEdit_Focused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
            {
                //header.TranslationY = 200;
                //stacks.TranslationY = 370;
                //AddLayout.TranslationY = -100;
            }
        }
    }
}