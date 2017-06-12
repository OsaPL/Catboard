using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace catboard
{
    public partial class fixedform : Form
    {
        Form1 mainform;
        public fixedform(Form1 form)
        {
            InitializeComponent();
            mainform = form;
            mainform.childopen = true;
        }
        private void fixedform_Load(object sender, EventArgs e)
        {
            toolTips.SetToolTip(this.buttonA, "A");
        }
    }
}
