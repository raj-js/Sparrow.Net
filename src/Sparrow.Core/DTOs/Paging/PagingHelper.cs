using System.Collections.Generic;

namespace Sparrow.Core.DTOs.Paging
{
    public class PagingHelper
    {
        public static Paged<TDTO> From<TDTO>(List<TDTO> List, long Total)
        {
            return new Paged<TDTO>() { List = List, Total = Total };
        }

        public static Paged<TDTO> From<TDTO>((List<TDTO> List, long Total) data)
        {
            return new Paged<TDTO>() { List = data.List, Total = data.Total };
        }
    }
}
