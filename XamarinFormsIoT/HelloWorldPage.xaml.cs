using Plugin.MediaManager;
using System;
using System.Text;
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

        /// <summary>
        /// Timer for IR stream
        /// </summary>
        private DateTime _mTimerIR = DateTime.MinValue;

        /// <summary>
        /// Used to build the IR string
        /// </summary>
        private StringBuilder _mStrIR = new StringBuilder();

        /// <summary>
        /// Wait for quit
        /// </summary>
        private bool _mWaitForExit = true;

        public HelloWorldPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Read IR values
        /// </summary>
        async void ReadIR()
        {
            while (_mWaitForExit)
            {
                if (null != _mPinIR)
                {
                    switch (_mPinIR.Read())
                    {
                        case Portable_GpioPinValue.High:
                            _mStrIR.Append("0");
                            break;
                        case Portable_GpioPinValue.Low:
                            _mStrIR.Append("1");
                            break;
                    }
                    if (_mStrIR.Length > 35) // keep at a certain length
                    {
                        _mStrIR.Remove(0, 1);
                    }
                    if (_mTimerIR < DateTime.Now)
                    {
                        _mTimerIR = DateTime.Now + TimeSpan.FromMilliseconds(100);
                        if (_mStrIR.Length > 1) //display text
                        {
                            string text = _mStrIR.ToString();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                _mTextIR.Text = text;
                            });
                        }
                    }
                }
                await Task.Delay(1);
            }
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

                    Task.Run(() =>
                    {
                        ReadIR();
                    });

                    /*

                    // subscribe to changes
                    _mPinIR.AddListenerValueChanged((sender,edge) =>
                    {
                        if (_mTimerIR < DateTime.Now)
                        {
                            if (_mStrIR.Length > 0)
                            {
                                _mStrIR.Remove(0, _mStrIR.Length);
                            }
                        }
                        switch (edge)
                        {
                            case Portable_GpioPinEdge.FallingEdge:
                                _mStrIR.Append(0);
                                break;
                            case Portable_GpioPinEdge.RisingEdge:
                                _mStrIR.Append(1);
                                break;
                        }
                        
                        // capture for short interval
                        _mTimerIR = DateTime.Now + TimeSpan.FromMilliseconds(100);

                        // update text
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _mTextIR.Text = _mStrIR.ToString();
                        });

                        if (null != _mPinBlue &&
                            null != _mPinRed)
                        {
                            switch (edge)
                            {
                                case Portable_GpioPinEdge.FallingEdge:
                                    _mPinBlue.Write(Portable_GpioPinValue.Low);
                                    _mPinRed.Write(Portable_GpioPinValue.High);
                                    _mPinYellow.Write(Portable_GpioPinValue.Low);
                                    break;
                                case Portable_GpioPinEdge.RisingEdge:
                                    _mPinRed.Write(Portable_GpioPinValue.Low);
                                    _mPinBlue.Write(Portable_GpioPinValue.High);
                                    _mPinYellow.Write(Portable_GpioPinValue.High);
                                    break;
                            }
                        }

                        // turn off LEDs
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Task.Delay(1000);
                            if (null != _mPinBlue &&
                                null != _mPinRed)
                            {
                                _mPinBlue.Write(Portable_GpioPinValue.High);
                                _mPinRed.Write(Portable_GpioPinValue.High);
                                _mPinYellow.Write(Portable_GpioPinValue.High);
                            }

                        });

                    });

                    */
                }
            }

            /*
            Device.BeginInvokeOnMainThread(async () =>
            {
                // wait for 5 minutes and then auto close the app
                await Task.Delay(600000);
                DependencyService.Get<Portable_IQuit>().Quit();
            });
            */
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
            _mWaitForExit = false;
            DependencyService.Get<Portable_IQuit>().Quit();
        }
    }
}
