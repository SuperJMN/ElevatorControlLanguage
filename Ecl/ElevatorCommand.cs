namespace Ecl
{
    public struct ElevatorCommand
    {
        public Distance Distance { get; set; }
        public bool IsRelative { get; set; }
        public bool IsWait { get; set; }
    }
}