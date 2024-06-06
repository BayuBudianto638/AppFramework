using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInfrastructures.Helpers
{
    public class PageInfo
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip
        {
            get
            {
                return PageSize * (Page - 1);
            }
        }

        public PageInfo()
        {
            // Default Constructer/Parameterless Constructor
        }

        public PageInfo(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
