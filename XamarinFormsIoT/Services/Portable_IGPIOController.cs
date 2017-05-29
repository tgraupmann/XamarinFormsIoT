namespace XamarinFormsIoT
{
    public interface Portable_IGPIOController
    {
        Portable_IGPIOController GetDefault();
        Portable_IGpioPin OpenPin(int pin);
    }
}
