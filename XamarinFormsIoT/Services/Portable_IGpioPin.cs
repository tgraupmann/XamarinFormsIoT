namespace XamarinFormsIoT
{
    public delegate void GpioPinValueChangedEvent(Portable_IGpioPin sender, Portable_GpioPinEdge edge);

    public interface Portable_IGpioPin
    {
        void AddListenerValueChanged(GpioPinValueChangedEvent e);

        void SetDriveMode(Portable_GpioPinDriveMode output);
        void Write(Portable_GpioPinValue pinValue);
        Portable_GpioPinValue Read();
    }
}
