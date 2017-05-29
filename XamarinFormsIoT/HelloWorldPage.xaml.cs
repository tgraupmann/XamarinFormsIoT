using Plugin.MediaManager;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsIoT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelloWorldPage : ContentPage
    {
        private const int LED_PIN_BLUE = 2;

        private const int LED_PIN_RED = 10;

        public HelloWorldPage()
        {
            InitializeComponent();
        }

        private Portable_IGPIOController _mGpioController = null;

        private Portable_IGpioPin _mPinBlue = null;
        private Portable_IGpioPin _mPinRed = null;

        protected override void OnAppearing()
        {
            /*
            _mStackLayout.Children.Add(new Label { Text = "Scanning..." });

            try
            {
                // discover some devices
                CrossBleAdapter.Current.Scan().Subscribe(scanResult =>
                {
                    _mStackLayout.Children.Add(new Label { Text = scanResult.ToString() });

                });
            }
            catch (Exception e)
            {
                _mStackLayout.Children.Add(new Label { Text = e.Message });
            }
            */

            base.OnAppearing();

            CrossMediaManager.Current.MediaFinished += Current_MediaFinished;

            _mGpioController = DependencyService.Get<Portable_IGPIOController>().GetDefault();
            if (null != _mGpioController)
            {
                _mPinBlue = _mGpioController.OpenPin(LED_PIN_BLUE);
                if (null != _mPinBlue)
                {
                    _mPinBlue.SetDriveMode(Portable_GpioPinDriveMode.Output);
                }

                _mPinRed = _mGpioController.OpenPin(LED_PIN_RED);
                if (null != _mPinRed)
                {
                    _mPinRed.SetDriveMode(Portable_GpioPinDriveMode.Output);
                }
            }
        }

        private async void Current_MediaFinished(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFinishedEventArgs e)
        {
            await CrossMediaManager.Current.Stop();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Play("ms-appx:///Assets/Test.wav");
        }

        private void OnClickBlueOn(object sender, EventArgs e)
        {
            if (null != _mPinBlue)
            {
                var pinValue = Portable_GpioPinValue.Low;
                _mPinBlue.Write(pinValue);
            }
        }

        private void OnClickBlueOff(object sender, EventArgs e)
        {
            if (null != _mPinBlue)
            {
                var pinValue = Portable_GpioPinValue.High;
                _mPinBlue.Write(pinValue);
            }
        }

        private void OnClickRedOn(object sender, EventArgs e)
        {
            if (null != _mPinRed)
            {
                var pinValue = Portable_GpioPinValue.Low;
                _mPinRed.Write(pinValue);
            }
        }

        private void OnClickRedOff(object sender, EventArgs e)
        {
            if (null != _mPinRed)
            {
                var pinValue = Portable_GpioPinValue.High;
                _mPinRed.Write(pinValue);
            }
        }

        private void OnClickQuit(object sender, EventArgs e)
        {
            OnClickBlueOff(null, null);
            OnClickRedOff(null, null);
            DependencyService.Get<Portable_IQuit>().Quit();
        }
    }
}
