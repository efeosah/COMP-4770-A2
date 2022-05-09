using System.ComponentModel;

// TO DON'T for A2: Don't change this namespace. It matters.
// ReSharper disable once CheckNamespace
namespace GameBrains.EventSystem // leave me alone.
{
    // TODO for A2: Add your own event types. Make use of these for mating and reproduction
    public static partial class Events
    {
        // Got eaten
        [Description("YouJustGotSwallowed")] public static readonly EventType YouJustGotSwallowed = (EventType)Count++;
        
        // Got attracted for eating
        [Description("TagYouAreIt")] public static readonly EventType TagYouAreIt = (EventType)Count++;
        
        // Got attracted for mating (add attractor)
        [Description("LetsMakeABaby")] public static readonly EventType LetsMakeABaby = (EventType)Count++;
        
        // Got rejected (drop attractor)
        [Description("YouAreNotMyType")] public static readonly EventType YouAreNotMyType = (EventType)Count++;

        // TODO for A2: Add and use your own event types.

        //New Event types
        [Description("JustWokeUp")] public static readonly EventType JustWokeUp = (EventType)Count++;



    }
}