using System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UwpTimerTest
{
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer dispatcherTimer;
        private int countDT;

        private ThreadPoolTimer threadPoolTimer;
        private int countTPT;

        private DateTime startTime;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            countDT = 0;
            countTPT = 0;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1.0);
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
            threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(ThreadPoolTimer_Tick, TimeSpan.FromSeconds(1.0)); ;
            startTime = DateTime.Now;
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            countDT++;
            UpdateLabel();
        }

        private async void ThreadPoolTimer_Tick(ThreadPoolTimer timer)
        {
            countTPT++;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateLabel();
            });
        }

        private void UpdateLabel()
        {
            tbStatus.Text = $"DT: {countDT} TPT: {countTPT} RealTime: {(DateTime.Now - startTime).TotalSeconds}";
        }
    }
}
