namespace uFrogio.iOS.Services
{
    //using System.Reactive.Subjects;
    //using YourCoreProject.Services;
    using UIKit;
    using System.Reactive.Subjects;

    public interface IKeyboardInteractionService
    {
        Subject<float> KeyboardHeightChanged { get; }
    }


    /// <inheritdoc cref="IKeyboardInteractionService" />
    public class KeyboardInteractionService : IKeyboardInteractionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardInteractionService" /> class.
        /// </summary>
        public KeyboardInteractionService()
        {
            UIKeyboard.Notifications.ObserveWillShow((_, uiKeyboardEventArgs) =>
            {
                var newKeyboardHeight = (float)uiKeyboardEventArgs.FrameEnd.Height;
                this.KeyboardHeightChanged.OnNext(newKeyboardHeight);
            });

            UIKeyboard.Notifications.ObserveWillHide((_, uiKeyboardEventArgs) =>
            {
                this.KeyboardHeightChanged.OnNext(0);
            });
        }

        /// <inheritdoc cref="IKeyboardInteractionService.KeyboardHeightChanged" />
        public Subject<float> KeyboardHeightChanged { get; } = new Subject<float>();
    }
}