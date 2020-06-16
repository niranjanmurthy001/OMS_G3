using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Ordermanagement_01.Opp.Opp_Master
{
    public partial class Sub_Product_Type_Entry : DevExpress.XtraEditors.XtraForm
    {
        string Operation_Type;
        public Sub_Product_Type_Entry(string _OperationType)
        {
            InitializeComponent();
            Operation_Type = _OperationType;
        }

        private void Sub_Product_Type_Entry_Load(object sender, EventArgs e)
        {
            if(Operation_Type == "Sub Product Type")
            {
                navigationFrame1.SelectedPage = navigationPage1;
            }
           else if(Operation_Type == "Sub Product Type Abbreviation")
            {
                navigationFrame1.SelectedPage = navigationPage2;
            }

        }
    }
}