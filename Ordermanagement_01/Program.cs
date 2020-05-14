using System;
using System.Windows.Forms;

namespace Ordermanagement_01
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new New_Dashboard.Orders.OrderEntry());
            //Application.Run(new New_Dashboard.Settings.Process_Settings());
            //Application.Run(new New_Dashboard.Settings.EmailSetting());
            // Application.Run(new Vendors.Keywords(1));
            // Application.Run(new New_Dashboard.NewLogin());
            //Application.Run(new DailyStatusReport_Preview(1,"2",""));
            //  Application.Run(new New_Dashboard.New_Dashboard(0058, 2));
            Application.Run(new New_Dashboard.Settings.Clarification_Setting(1,"1")
                );
           // Application.Run(new Check_List_New(1, 1, 1, "2", "1", "2", 209816, 0, 0, "565172", "", "", 2, 1, "565172", "2", 1));
            //Application.Run(new Order_Document_List(1, 202696, 4,1));
           // Application.Run(new Employee_Error_Entry(1, "1", "3", 1, 1, 1, "", "",0, 1));
           
        }
    }
}
