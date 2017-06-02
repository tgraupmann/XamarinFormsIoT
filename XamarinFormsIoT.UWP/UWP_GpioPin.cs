using System.Collections.Generic;
using Windows.Devices.Gpio;
using Xamarin.Forms;
using XamarinFormsIoT;

[assembly: Dependency(typeof(UWP_GPIOController))]
namespace XamarinFormsIoT
{
    public class UWP_GpioPin : Portable_IGpioPin
    {
        public GpioPin GpioPin { get; set; }

        private List<GpioPinValueChangedEvent> ValueChanged = new List<GpioPinValueChangedEvent>();

        public void AddListenerValueChanged(GpioPinValueChangedEvent e)
        {
            if (null == GpioPin)
            {
                return;
            }
            GpioPin.ValueChanged += GpioPin_ValueChanged;
            ValueChanged.Add(e);
        }

        private void GpioPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            for (int i = 0; i < ValueChanged.Count; ++i)
            {
                switch (args.Edge)
                {
                    case GpioPinEdge.FallingEdge:
                        ValueChanged[i].Invoke(this, Portable_GpioPinEdge.FallingEdge);
                        break;
                    case GpioPinEdge.RisingEdge:
                        ValueChanged[i].Invoke(this, Portable_GpioPinEdge.RisingEdge);
                        break;
                }
            }
        }

        public void SetDriveMode(Portable_GpioPinDriveMode output)
        {
            switch (output)
            {
                case Portable_GpioPinDriveMode.Input:
                    GpioPin.SetDriveMode(GpioPinDriveMode.Input);
                    break;
                case Portable_GpioPinDriveMode.Output:
                    GpioPin.SetDriveMode(GpioPinDriveMode.Output);
                    break;
            }
        }

        public void Write(Portable_GpioPinValue pinValue)
        {
            switch (pinValue)
            {
                case Portable_GpioPinValue.Low:
                    GpioPin.Write(GpioPinValue.Low);
                    break;
                case Portable_GpioPinValue.High:
                    GpioPin.Write(GpioPinValue.High);
                    break;
            }
        }

        public Portable_GpioPinValue Read()
        {
            var input = GpioPin.Read();
            switch (input)
            {
                case GpioPinValue.Low:
                    return Portable_GpioPinValue.Low;
                case GpioPinValue.High:
                    return Portable_GpioPinValue.High;
            }
            return Portable_GpioPinValue.High;
        }
    }
}
