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
            //    Application.Run(new New_Dashboard.NewLogin());
            //Application.Run(new DailyStatusReport_Preview(1,"2",""));
            //  Application.Run(new New_Dashboard.New_Dashboard(0058, 2));
            // Application.Run(new New_Dashboard.Settings.Clarification_Setting(1,"1"));
            // Application.Run(new Check_List_New(1, 1, 1, "2", "1", "2", 209816, 0, 0, "565172", "", "", 2, 1, "565172", "2", 1));
            //Application.Run(new Order_Document_List(1, 202696, 4,1));
            //Application.Run(new Employee_Error_Entry(1, "1", "3", 1, 1, 1, "", "",0, 1));
            //Application.Run(new New_Dashboard.Settings.Process_Settings());
            //Application.Run(new Opp.Opp_Master.Project_Type_Order_Task());
            // Application.Run(new Opp.Opp_Master.Project_Type_OrderStatus_Settings(1,1));
            // Application.Run(new Opp.Opp_Master.Product_Type_Settings());
            //Application.Run(new Masters.Error_Field());
            //Application.Run(new Opp.Opp_Master.Error_Setting(1, 1));
            // Application.Run(new Opp.Opp_Master.TEst1());
            //Application.Run(new Opp.Opp_Master.Error_Settings());
            //Application.Run(new Opp.Opp_Master.ErrorTabSetting());
            // Application.Run(new Opp.Opp_Efficiency.Category_Salary_Bracket_ProjectWise());
            //Application.Run(new Opp.Opp_Master.ImportErrorInfo("Error Field",));
            //Application.Run(new Opp.Opp_Efficiency.Efficiency_View(5));
            //  Application.Run(new Opp.Opp_Master.Sub_Product_Type_View(3));
            // Application.Run(new Opp.Opp_Master.Order_SourceType_View(3));
            //Application.Run(new Opp.Opp_Master.Error_Settings());
             Application.Run(new Opp.Opp_Efficiency.Efficiency_Order_Source_Type(1));
          // Application.Run(new Opp.Opp_Efficiency.Efficiency_Source_Type(1));
        }

    }

}
