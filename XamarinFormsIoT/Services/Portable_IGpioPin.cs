namespace XamarinFormsIoT
{
    public interface Portable_IGpioPin
    {
        void SetDriveMode(Portable_GpioPinDriveMode output);
        void Write(Portable_GpioPinValue pinValue);
        Portable_GpioPinValue Read();
    }
}
