﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tagplaner
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung. Krass!
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Init());
        }
    }
}
