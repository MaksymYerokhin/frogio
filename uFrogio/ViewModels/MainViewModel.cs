using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace uFrogio.ViewModels
{
    [Serializable]
    public class Task
    {
        public Task() { }
        public Task(JToken jTask)
        {
            UserID = (int)jTask["UserID"];
            Text = (string)jTask["Text"];
            LastUpdateTime = (DateTime)jTask["LastUpdateTime"];
            IsDone = (bool)jTask["IsDone"];
        }
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public string Text { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public DateTime TaskTime { get; set; }
        public bool IsDone { get; set; }
    }

    public class MainViewModel : BaseViewModel
    {
        public string currentDate = "November, 2020";
        public string CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                OnPropertyChanged();
                //PreviousCalendarCommand.ChangeCanExecute();
                //NextCalendarCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<Task> TodayTasks { get; set; }
        public ObservableCollection<Task> TomorrowTasks { get; set; }
        public ObservableCollection<Task> WeekTasks { get; set; }
        public Command LoadItemsCommand { get; set; }

        public Command LoadTomorrowItemsCommand { get; set; }
        public Command LoadWeekItemsCommand { get; set; }

        public MainViewModel()
        {
            Title = "Browse";
            TodayTasks = new ObservableCollection<Task>();
            TomorrowTasks = new ObservableCollection<Task>();
            WeekTasks = new ObservableCollection<Task>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadTomorrowItemsCommand = new Command(async () => await ExecuteLoadTomorrowItemsCommand());
            LoadWeekItemsCommand = new Command(async () => await ExecuteLoadWeekItemsCommand());
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //var okAlertController = UIAlertController.Create("My1Title", "The message", UIAlertControllerStyle.Alert);
            //object p = ShowViewController(okAlertController, null);
            //PresentViewController(okAlertController, true, null);

            //MessagingCenter.Subscribe<MainPage, Task>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Task;
            //    Tasks.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        async Task<bool> ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                TodayTasks.Clear();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mayer1995-001-site2.itempurl.com/Task/GetAllUserTasks?userId=1");
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.KeepAlive = false;
                var response = (HttpWebResponse)request.GetResponse();
                string j;
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    j = reader.ReadToEnd();
                }

                var arr = JArray.Parse(j);
                foreach (var jtoken in arr)
                {
                    TodayTasks.Add(new Task(jtoken));
                    //WeekTasks.Add(new Task(jtoken));
                }

                //TodayTasks.Add(new Task()
                //{
                //    UserId = Guid.NewGuid().ToString(),
                //    IsDone = false,
                //    Description = "Task 1"
                //});

                //TodayTasks.Add(new Task()
                //{
                //    UserId = Guid.NewGuid().ToString(),
                //    IsDone = false,
                //    Description = "Task 2"
                //});

                //var todayTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.Date).ToList();
                //var tomorrowTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.AddDays(1).Date).ToList();

                //await DisplayAlert("Alert", "You have been alerted", "OK");


                //var items = await DataStore.GetItemsAsync(true);
                foreach (var item in TodayTasks)
                {
                    await DataStore.AddItemAsync(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        async Task<bool> ExecuteLoadTomorrowItemsCommand()
        {
            IsBusy = true;

            try
            {
                TomorrowTasks.Clear();

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mayer1995-001-site1.itempurl.com/api/task");
                //request.Method = "GET";
                //request.ContentType = "application/json";
                //request.Accept = "application/json";
                //request.KeepAlive = false;
                //var response = (HttpWebResponse)request.GetResponse();
                //string j;
                //using (Stream responseStream = response.GetResponseStream())
                //{
                //    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                //    j = reader.ReadToEnd();
                //}

                //var arr = JArray.Parse(j);
                //foreach (var jtoken in arr)
                //{
                //    TomorrowTasks.Add(new Task(jtoken));
                //}

                TomorrowTasks.Add(new Task()
                {
                    UserID = 1,//Guid.NewGuid().ToString(),
                    IsDone = false,
                    Text = "Task 3"
                });

                TomorrowTasks.Add(new Task()
                {
                    UserID = 1,//Guid.NewGuid().ToString(),
                    IsDone = false,
                    Text = "Task 4"
                });

                //var todayTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.Date).ToList();
                //var tomorrowTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.AddDays(1).Date).ToList();

                foreach (var item in TomorrowTasks)
                {
                    await DataStore.AddItemAsync(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }

        async Task<bool> ExecuteLoadWeekItemsCommand()
        {
            IsBusy = true;

            try
            {
                //WeekTasks.Clear();

                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://mayer1995-001-site1.itempurl.com/api/task");
                //request.Method = "GET";
                //request.ContentType = "application/json";
                //request.Accept = "application/json";
                //request.KeepAlive = false;
                //var response = (HttpWebResponse)request.GetResponse();
                //string j;
                //using (Stream responseStream = response.GetResponseStream())
                //{
                //    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                //    j = reader.ReadToEnd();
                //}

                //var arr = JArray.Parse(j);
                //foreach (var jtoken in arr)
                //{
                //    WeekTasks.Add(new Task(jtoken));
                //}

                WeekTasks.Add(new Task()
                {
                    UserID = 1,//Guid.NewGuid().ToString(),
                    IsDone = false,
                    Text = "Task 5"
                });

                WeekTasks.Add(new Task()
                {
                    UserID = 1,//Guid.NewGuid().ToString(),
                    IsDone = false,
                    Text = "Task 6"
                });

                //var todayTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.Date).ToList();
                //var tomorrowTasks = Tasks.Where(x => x.DueDate.Date == DateTime.UtcNow.AddDays(1).Date).ToList();

                foreach (var item in WeekTasks)
                {
                    await DataStore.AddItemAsync(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return true;
        }
    }
}
