namespace LuxeIQ.ViewModels
{
    public class TableColumns
    {
        private bool? _isRequired;
        private bool? _isPrimaryKey;

        public string name { get; set; }
        public string fieldType { get; set; }
        public long maxLength { get; set; }
        public long precision { get; set; }
        public long scale { get; set; }
        public bool? isRequired
        {
            get
            {
                return _isRequired.HasValue ? _isRequired : false;
            }
            set
            {
                _isRequired = value;
            }
        }
        public bool? isPrimaryKey
        {
            get
            {
                return _isPrimaryKey.HasValue ? _isPrimaryKey : false;
            }
            set
            {
                _isPrimaryKey = value;
            }
        }
        public string defaultValue { get; set; }
    }
}

