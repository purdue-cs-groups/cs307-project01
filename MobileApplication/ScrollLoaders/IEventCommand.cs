using System.Windows.Input;

namespace MetrocamPan.ScrollLoaders
{
    public interface IEventCommand : ICommand
    {
        string EventName { get; }
    }
}
