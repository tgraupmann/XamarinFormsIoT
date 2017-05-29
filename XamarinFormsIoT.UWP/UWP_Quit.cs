using Windows.ApplicationModel.Core;
using Xamarin.Forms;
using XamarinFormsIoT;

[assembly: Dependency(typeof(UWP_QUit))]
namespace XamarinFormsIoT
{
    public class UWP_QUit : Portable_IQuit
    {
        public void Quit()
        {
            CoreApplication.Exit();
        }        
    }
}
