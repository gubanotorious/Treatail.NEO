﻿using System.Web;
using System.Web.Mvc;

namespace Treatail.NEO.Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
