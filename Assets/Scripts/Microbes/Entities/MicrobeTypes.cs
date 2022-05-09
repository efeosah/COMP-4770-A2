namespace Microbes.Entities
{
    // TODO for A2 (optional): Add more types. Set Microbe to use them.
    [System.Flags]
    public enum MicrobeTypes
    {
        None = 0,
        Blue = 1,
        Red = 2,
        Yellow = 4,
        Green = 8,
        //New type hmmm...
        White = 7,
        Black = 6,
        All = ~None
    }
}