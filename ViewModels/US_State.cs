namespace LuxeIQ.ViewModels
{
    public class US_State
    {
        public string Name { get; set; }

        public string Abbreviations { get; set; }
        public US_State(string ab, string name)
        {
            Name = name;
            Abbreviations = ab;
        }

    }
}
