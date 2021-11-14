﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Dashboard
{
    public static class StartApplication
    {
        public static void RunApplication(string app)
        {
            Console.WriteLine("Hello World!");
            var apps = ReadExePaths.ReadAllPaths();
            var usePath = apps.Where(a => a.Key == app).Select(b => b.Value).FirstOrDefault();
            System.Diagnostics.Process.Start(usePath);
        }
    }
}