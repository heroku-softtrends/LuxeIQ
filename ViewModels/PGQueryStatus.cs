namespace LuxeIQ.ViewModels
{
    public struct PGQueryStatus
    {
        public int total_rows { set; get; }
        public int inserted_rows { set; get; }
        public int updated_rows { set; get; }
    }
}
