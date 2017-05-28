using Microsoft.Practices.Prism.PubSubEvents;

namespace DeepInsights.Shell.Infrastructure.Events
{
    public sealed class InstrumentChangedEvent : PubSubEvent<string>
    {
    }
}
