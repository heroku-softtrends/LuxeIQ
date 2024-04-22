namespace LuxeIQ.ViewModels
{
    public class ProductAttribute
    {
        public ProductAttribute()
        {

        }
        public string field_name { get; set; }
        public bool include_in_attribute { get; set; } = false;
        public string datatype { get; set; } = "text";
    }
}
