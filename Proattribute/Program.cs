﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Proattribute
{
    class Program
    {
        public static void Main()
        {
            List<string> strList = new List<string>() { "A", "d", "b", "C" };

            var newList = strList.OrderBy(x => x, StringComparer.Ordinal);
        }
    }
}
