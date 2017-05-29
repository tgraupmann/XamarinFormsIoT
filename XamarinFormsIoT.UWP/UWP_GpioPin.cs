using Windows.Devices.Gpio;
using Xamarin.Forms;
using XamarinFormsIoT;

[assembly: Dependency(typeof(UWP_GPIOController))]
namespace XamarinFormsIoT
{
    public class UWP_GpioPin : Portable_IGpioPin
    {
        public GpioPin GpioPin { get; set; }

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
    }
}
