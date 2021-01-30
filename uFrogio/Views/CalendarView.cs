﻿//using System;
//using uFrogio.Models;
//using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

//[XamlCompilation(XamlCompilationOptions.Compile)]
//public partial class CalendarView : ContentPage
//{
//    #region for common settings
//    Color selectedDateColor;
//    Color disabledColor;
//    Color oldMonthDatesColor;
//    Color activatedColor;
//    Color TextColor;
//    string borderType;
//    string weekEndingDay;
//    string selectedFillType;
//    public DateTime minDateRange;
//    public string DateFormat;
//    public DateTime calendarUI_DT;
//    public DateTime maxDateRange;
//    #endregion
//    CustomCalendarStackLayout selectedLayout = null;
//    int wed = -1;
//    DateTime selectedDate;
//    CalendarGlobalData calendarGlobalData;
//    CalendarViewModel cvm;
//    bool DateSet = false;
//    //for yearview,we will show months in 4x3 grid
//    int max_row_year = 4;
//    int max_col_year = 3;
//    Grid month_layout = null;
//    StackLayout year_layout = null;
//    CalendarGlobalData calendarGlobalData;
//    List<CustomCalendarStackLayout> month_ccsl;
//    List<CustomCalendarStackLayout> year_ccsl;
//    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
//    DateTime lastSelectedMonth;
//    public CalendarView(CalendarGlobalData cgd)
//    {
//        InitializeComponent();
//        calendarGlobalData = cgd;
//        InitiateCommonVariables();
//        cvm = new CalendarViewModel(this);
//        BindingContext = cvm;
//        cvm.CurrentDate = string.Format("{0}, {1}", calendarUI_DT.ToString("MMMM"), calendarUI_DT.Year);
//        if (weekEndingDay != null) wed = Array.IndexOf(GlobalVar.WeekDays, weekEndingDay.ToLower());
//        tapGestureRecognizer.Tapped += DateSelected;
//        //the first view will be of Current month
//        MonthLayout(calendarUI_DT);
//    }
//    public void InitiateCommonVariables()
//    {
//        parentObj = calendarGlobalData.Parent as NewTimesheet;
//        selectedDateColor = calendarGlobalData.SelectedDateColor;
//        disabledColor = calendarGlobalData.DisabledColor;
//        oldMonthDatesColor = calendarGlobalData.OldMonthDatesColor;
//        selectedFillType = calendarGlobalData.SelectedFillType;
//        borderType = calendarGlobalData.BorderType;
//        TextColor = calendarGlobalData.TextColor;
//        minDateRange = calendarGlobalData.MinDateRange;
//        maxDateRange = calendarGlobalData.MaxDateRange;
//        DateFormat = calendarGlobalData.DateFormat;
//        calendarUI_DT = calendarGlobalData.CalendarUI_DT;
//        weekEndingDay = calendarGlobalData.WeekEndingDay;
//        activatedColor = calendarGlobalData.ActivatedColor;
//    }
//    public void MonthLayout(DateTime dt)
//    {
//        //Only layout is created,which will just show stacklayout.Values are filled in MonthView method
//        cvm.CurrentCalendarView = GlobalVar.MonthView;
//        if (year_layout != null) year_layout.IsVisible = false;
//        cvm.CurrentDate = string.Format("{0}, {1}", dt.ToString("MMMM"), dt.Year);
//        if (month_layout == null)
//        {
//            month_ccsl = new List<CustomCalendarStackLayout>();
//            month_layout = new Grid
//            {
//                ColumnSpacing = 2,
//                RowSpacing = 2,
//                HorizontalOptions = LayoutOptions.FillAndExpand,
//                VerticalOptions = LayoutOptions.FillAndExpand,
//            };
//            calendar_holder.Children.Add(month_layout);
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            month_layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
//            for (int row = 0; row < 7; row++)
//            {
//                for (int col = 0; col < 7; col++)
//                {
//                    var sl = new CustomCalendarStackLayout
//                    {
//                        VerticalOptions = LayoutOptions.FillAndExpand,
//                        HorizontalOptions = LayoutOptions.FillAndExpand,
//                    };
//                    var label = new Label
//                    {
//                        VerticalOptions = LayoutOptions.FillAndExpand,
//                        HorizontalOptions = LayoutOptions.FillAndExpand,
//                        VerticalTextAlignment = TextAlignment.Center,
//                        HorizontalTextAlignment = TextAlignment.Center,
//                        TextColor = TextColor
//                    };
//                    sl.Children.Add(label);
//                    month_ccsl.Add(sl);
//                    month_layout.Children.Add(sl, col, row);
//                    if (row == 0)
//                    {
//                        label.TextColor = Color.Gray;
//                        label.Text = GlobalVar.WeekDaysLabel[col];
//                    }
//                }
//            }
//        }
//        else
//        {
//            month_layout.IsVisible = true;
//        }
//        MonthView(dt);
//    }
//    public void YearLayout(DateTime dt)
//    {
//        if (month_layout != null) month_layout.IsVisible = false;
//        if (year_layout == null)
//        {
//            year_ccsl = new List<CustomCalendarStackLayout>();
//            year_layout = new StackLayout()
//            {
//                Spacing = 5,
//                HorizontalOptions = LayoutOptions.FillAndExpand,
//                VerticalOptions = LayoutOptions.FillAndExpand
//            };
//            calendar_holder.Children.Add(year_layout);
//            for (var row = 0; row < max_row_year; row++)
//            {
//                var sl_row = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 5, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
//                year_layout.Children.Add(sl_row);
//                for (var col = 0; col < max_col_year; col++)
//                {
//                    var sl_month = new CustomCalendarStackLayout()
//                    {
//                        HorizontalOptions = LayoutOptions.FillAndExpand,
//                        VerticalOptions = LayoutOptions.FillAndExpand,
//                        BackgroundColor = activatedColor
//                    };
//                    var label = new Label()
//                    {
//                        VerticalOptions = LayoutOptions.CenterAndExpand,
//                        HorizontalOptions = LayoutOptions.CenterAndExpand
//                    };
//                    year_ccsl.Add(sl_month);
//                    sl_month.Children.Add(label);
//                    sl_row.Children.Add(sl_month);
//                }
//            }
//        }
//        else
//        {
//            year_layout.IsVisible = true;
//        }
//        YearView(dt);
//    }
//    public void YearView(DateTime dt)
//    {
//        int month_counter = 0;
//        calendarUI_DT = dt;
//        cvm.CurrentCalendarView = GlobalVar.YearView;
//        cvm.CurrentDate = string.Format("{0}", dt.Year);
//        for (int row = 0; row < max_row_year; row++)
//        {
//            for (int col = 0; col < max_col_year; col++)
//            {
//                CustomCalendarStackLayout sl = year_ccsl[month_counter];
//                Label label = sl.Children[0] as Label;
//                label.Text = GlobalVar.MonthsLabel[month_counter];
//                sl.DateTimeInfo = new DateTime(calendarUI_DT.Year, month_counter + 1, 1);
//                sl.dateType = GlobalVar.YearView;
//                label.TextColor = TextColor;
//                sl.BackgroundColor = disabledColor;
//                CheckIfValid(sl);
//                if (lastSelectedMonth != null && (month_counter + 1) == lastSelectedMonth.Month && dt.Year == lastSelectedMonth.Year)
//                {
//                    sl.BackgroundColor = selectedDateColor;
//                    label.TextColor = Color.White;
//                }
//                month_counter++;
//            }
//        }
//    }
//    public void CheckIfValid(CustomCalendarStackLayout sl)
//    {
//        // only those days/month will be active which satisfy following conditions:
//        //1. with in max & min date range.
//        sl.GestureRecognizers.Clear();
//        switch (sl.dateType)
//        {
//            case GlobalVar.YearView:
//                int maxDate = DateTime.DaysInMonth(sl.DateTimeInfo.Year, sl.DateTimeInfo.Month) > maxDateRange.Day ? maxDateRange.Day : DateTime.DaysInMonth(sl.DateTimeInfo.Year, sl.DateTimeInfo.Month);
//                DateTime newDTToCompareMin = new DateTime(sl.DateTimeInfo.Year, sl.DateTimeInfo.Month, minDateRange.Day);
//                DateTime newDTToCompareMax = new DateTime(sl.DateTimeInfo.Year, sl.DateTimeInfo.Month, maxDate);
//                if (newDTToCompareMin >= minDateRange && newDTToCompareMax <= maxDateRange)
//                {
//                    sl.GestureRecognizers.Add(tapGestureRecognizer);
//                    sl.BackgroundColor = activatedColor;
//                }
//                break;
//            case GlobalVar.MonthView:
//                if (sl.DateTimeInfo.Date >= minDateRange.Date && sl.DateTimeInfo.Date <= maxDateRange.Date)
//                {
//                    if (wed == -1 || (wed != -1 && (int)sl.DateTimeInfo.DayOfWeek == wed))
//                    {
//                        if (sl.DateTimeInfo.Month == calendarUI_DT.Month)
//                        {
//                            sl.GestureRecognizers.Add(tapGestureRecognizer);
//                            sl.isWeekEndingDate = true;
//                            sl.BackgroundColor = activatedColor;
//                        }

//                    }
//                }
//                break;
//        }
//    }
//    public void MonthView(DateTime dt)
//    {
//        int currentDay = dt.Day;
//        lastSelectedMonth = dt;
//        calendarUI_DT = dt;
//        cvm.CurrentDate = string.Format("{0}, {1}", dt.ToString("MMMM"), dt.Year);
//        int noOfDays = DateTime.DaysInMonth(dt.Year, dt.Month);
//        int startingDay = (int)new DateTime(dt.Year, dt.Month, 1).DayOfWeek;
//        DateTime previousMonth_DT = calendarUI_DT.AddMonths(-1);
//        int previousMonth_noOfDays = DateTime.DaysInMonth(previousMonth_DT.Year, previousMonth_DT.Month);
//        int counter = 0;
//        int nextMonth_counter = 1;
//        for (int row = 0; row < 7; row++)
//        {
//            for (int col = 0; col < 7; col++)
//            {
//                CustomCalendarStackLayout sl = month_ccsl[counter + 7];
//                // +7 added as counter start after weekday labels are filled
//                Label label = sl.Children[0] as Label;
//                sl.dateType = GlobalVar.MonthView;
//                #region show week labels 
//                if (row == 0)
//                {
//                    //update if weekdays need to be updated
//                }
//                #endregion
//                else
//                {
//                    #region previous month
//                    if (counter < startingDay)
//                    {
//                        int date = previousMonth_noOfDays - (startingDay - counter - 1);
//                        label.Text = date.ToString();
//                        sl.BackgroundColor = oldMonthDatesColor;
//                        sl.DateTimeInfo = new DateTime(calendarUI_DT.AddMonths(-1).Year, calendarUI_DT.AddMonths(-1).Month, date);
//                    }
//                    #endregion
//                    #region current Month
//                    else if (counter >= startingDay && (counter - startingDay) < noOfDays)
//                    {
//                        sl.BackgroundColor = disabledColor;
//                        int date = (counter + 1 - startingDay);
//                        label.Text = date.ToString();
//                        sl.IsCurrentMonthsDate = true;
//                        sl.DateTimeInfo = new DateTime(calendarUI_DT.Year, calendarUI_DT.Month, date);
//                    }
//                    #endregion
//                    #region next month
//                    else if (counter >= (noOfDays + startingDay))
//                    {
//                        int date = nextMonth_counter++;
//                        label.Text = date.ToString();
//                        sl.BackgroundColor = oldMonthDatesColor;
//                        sl.DateTimeInfo = new DateTime(calendarUI_DT.AddMonths(+1).Year, calendarUI_DT.AddMonths(+1).Month, date);
//                    }
//                    #region week ending date column
//                    CheckIfValid(sl);
//                    #endregion
//                    #endregion
//                    counter++;
//                }
//            }
//        }
//    }
//    public void ClearPreviousSelected(CustomCalendarStackLayout sl)
//    {
//        switch (selectedFillType)
//        {
//            case CalendarGlobalData.FillType_Circle:
//                break;
//            case CalendarGlobalData.FillType_Fill:
//                if (sl.dateType == GlobalVar.YearView)
//                {
//                    sl.BackgroundColor = activatedColor;
//                }
//                else if (sl.dateType == GlobalVar.MonthView)
//                {
//                    if (sl.isWeekEndingDate)
//                    {
//                        sl.BackgroundColor = activatedColor;
//                    }
//                    else if (sl.IsCurrentMonthsDate)
//                    {
//                        sl.BackgroundColor = disabledColor;
//                    }
//                    else
//                    {
//                        sl.BackgroundColor = oldMonthDatesColor;
//                    }
//                }
//                break;
//        }
//    }
//    public void DateSelected(object s, EventArgs e)
//    {
//        var sl = s as CustomCalendarStackLayout;
//        if (selectedLayout != null) ClearPreviousSelected(selectedLayout);
//        selectedLayout = sl;
//        sl.BackgroundColor = selectedDateColor;
//        switch (sl.dateType)
//        {
//            case GlobalVar.MonthView:
//                selectedDate = sl.dateTime;
//                //Do your work
//                break;
//            case GlobalVar.YearView:
//                MonthLayout(sl.DateTimeInfo);
//                break;
//        }
//    }
//}