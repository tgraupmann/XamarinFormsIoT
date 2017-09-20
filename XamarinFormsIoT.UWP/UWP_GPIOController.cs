using Windows.Devices.Gpio;
using Xamarin.Forms;
using XamarinFormsIoT;

[assembly: Dependency(typeof(UWP_GPIOController))]
namespace XamarinFormsIoT
{
    public class UWP_GPIOController : Portable_IGPIOController
    {
        public GpioController GpioController { get; set; }
        public Portable_IGPIOController GetDefault()
        {
            GpioController = GpioController.GetDefault();
            return this;
        }
        public Portable_IGpioPin OpenPin(int pin)
        {
            if (GpioController != null)
            {
                UWP_GpioPin result = new UWP_GpioPin();
                result.GpioPin = GpioController.OpenPin(pin);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
