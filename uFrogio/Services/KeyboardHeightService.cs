using uFrogio.iOS.Services;
using System.Reactive.Subjects;
using Xamarin.Forms;

[assembly: Dependency(typeof(DependencyServiceDemos.iOS.KeyboardHeightService))]
namespace DependencyServiceDemos.iOS
{
    public class KeyboardHeightService : IKeyboardInteractionService
    {
        public Subject<float> KeyboardHeightChanged => new uFrogio.iOS.Services.KeyboardInteractionService().KeyboardHeightChanged;
    }
}