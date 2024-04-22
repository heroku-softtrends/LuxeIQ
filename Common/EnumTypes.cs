using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LuxeIQ.Common
{

    public enum UserType
    {
        None = 0,
        [Description("Manufacturer Admin")]
        M,
        [Description("Sales Rep")]
        SA
    }

    public enum DataType
    {
        TEXT,
        INTEGER,
        DECIMAL
    }

}
