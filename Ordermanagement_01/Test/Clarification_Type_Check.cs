using DevExpress.XtraEditors;
using Ordermanagement_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ordermanagement_01.Test
{
    public partial class Clarification_Type_Check : XtraForm
    {

        DropDownistBindClass dropdown = new DropDownistBindClass();
        DataAccess da = new DataAccess();
        private readonly int Order_Id, Order_Task_Id, Order_Status_Id, User_Id;
        private SqlConnection Con;
        private Order_Passing_Params ObjRecivedparmas;
        private Employee_Order_Entry Mainfrom = null;
        public Clarification_Type_Check()
        {
           // Mainfrom = CallingFrom as Employee_Order_Entry;
           // InitializeComponent();
           //ObjRecivedparmas= orderparams;
           // Order_Id = orderparams.Order_Id;
           // Order_Task_Id = orderparams.Order_Task_Id;
           // User_Id = orderparams.User_Id;
            dropdown.Bind_Document_Check_Type(Clarification_Chk);

        }


        private void Clarification_Type_Check_Load(object sender, EventArgs e)
        {
         
        }
      
    }
}
