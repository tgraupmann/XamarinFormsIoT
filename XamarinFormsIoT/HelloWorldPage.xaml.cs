using Plugin.MediaManager;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinFormsIoT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelloWorldPage : ContentPage
    {
        /// <summary>
        /// The LED pin numbers
        /// </summary>
        private const int LED_PIN_BLUE = 2;
        private const int LED_PIN_RED = 10;
        private const int LED_PIN_YELLOW = 19;
        private const int LED_PIN_IR = 18;

        /// <summary>
        /// GPIO controller interface for the portable library
        /// </summary>
        private Portable_IGPIOController _mGpioController = null;

        /// <summary>
        /// GPIO Pin interface for the portable library
        /// </summary>
        private Portable_IGpioPin _mPinBlue = null;
        private Portable_IGpioPin _mPinRed = null;
        private Portable_IGpioPin _mPinYellow = null;
        private Portable_IGpioPin _mPinIR = null;

        public HelloWorldPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // handle stopping a sound from playing
            CrossMediaManager.Current.MediaFinished += Current_MediaFinished;

            // get the gpio controller interface
            _mGpioController = DependencyService.Get<Portable_IGPIOController>().GetDefault();

            // validity check
            if (null != _mGpioController)
            {
                // setup blue led
                _mPinBlue = _mGpioController.OpenPin(LED_PIN_BLUE);
                if (null != _mPinBlue)
                {
                    _mPinBlue.SetDriveMode(Portable_GpioPinDriveMode.Output);
                }

                // setup red led
                _mPinRed = _mGpioController.OpenPin(LED_PIN_RED);
                if (null != _mPinRed)
                {
                    _mPinRed.SetDriveMode(Portable_GpioPinDriveMode.Output);
                }

                // setup yellow led
                _mPinYellow = _mGpioController.OpenPin(LED_PIN_YELLOW);
                if (null != _mPinYellow)
                {
                    _mPinYellow.SetDriveMode(Portable_GpioPinDriveMode.Output);
                }

                // setup ir receiver
                _mPinIR = _mGpioController.OpenPin(LED_PIN_IR);
                if (null != _mPinIR)
                {
                    _mPinIR.SetDriveMode(Portable_GpioPinDriveMode.Input);

                    // subscribe to changes
                    _mPinIR.AddListenerValueChanged((sender,edge) =>
                    {
                        // update text on the main thread
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await CrossMediaManager.Current.Play("ms-appx:///Assets/Pew.wav");

                            _mTextIR.Text = string.Format("IR: {0}", edge);

                            await Task.Delay(1000);

                            _mTextIR.Text = "IR:";
                        });

                    });
                }
            }
        }

        /// <summary>
        /// Read IR values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickButtonIR(object sender, EventArgs e)
        {
            if (null != _mPinIR)
            {
                Portable_GpioPinValue val = _mPinIR.Read();
                _mTextIR.Text = string.Format("IR: {0}", val);
            }
        }

        /// <summary>
        /// Stop playing when media finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Current_MediaFinished(object sender, Plugin.MediaManager.Abstractions.EventArguments.MediaFinishedEventArgs e)
        {
            await CrossMediaManager.Current.Stop();
        }

        /// <summary>
        /// Click event to play a sound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClickPlaySound(object sender, EventArgs e)
        {
            await CrossMediaManager.Current.Play("ms-appx:///Assets/Test.wav");
        }

        /// <summary>
        /// Turn on the blue led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBlueOn(object sender, EventArgs e)
        {
            if (null != _mPinBlue)
            {
                var pinValue = Portable_GpioPinValue.Low;
                _mPinBlue.Write(pinValue);
            }
        }

        /// <summary>
        /// Turn off the blue led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickBlueOff(object sender, EventArgs e)
        {
            if (null != _mPinBlue)
            {
                var pinValue = Portable_GpioPinValue.High;
                _mPinBlue.Write(pinValue);
            }
        }

        /// <summary>
        /// Turn on the red led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickRedOn(object sender, EventArgs e)
        {
            if (null != _mPinRed)
            {
                var pinValue = Portable_GpioPinValue.Low;
                _mPinRed.Write(pinValue);
            }
        }

        /// <summary>
        /// Turn off the red led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickRedOff(object sender, EventArgs e)
        {
            if (null != _mPinRed)
            {
                var pinValue = Portable_GpioPinValue.High;
                _mPinRed.Write(pinValue);
            }
        }

        /// <summary>
        /// Turn on the yellow led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickYellowOn(object sender, EventArgs e)
        {
            if (null != _mPinYellow)
            {
                var pinValue = Portable_GpioPinValue.Low;
                _mPinYellow.Write(pinValue);
            }
        }

        /// <summary>
        /// Turn off the yellow led
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickYellowOff(object sender, EventArgs e)
        {
            if (null != _mPinYellow)
            {
                var pinValue = Portable_GpioPinValue.High;
                _mPinYellow.Write(pinValue);
            }
        }

        /// <summary>
        /// Use a portable interface to quit the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickQuit(object sender, EventArgs e)
        {
            DependencyService.Get<Portable_IQuit>().Quit();
        }
    }
}
