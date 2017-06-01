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
    public partial class SoundsForm : Form
    {
        public SoundsForm(Form1 form)
        {
            InitializeComponent();
            int i = 0;
            Label labellast = null;
            TextBox textboxlast = null;
            foreach (string strings in form.soundsList)
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Name = "L"+i;
                System.Windows.Forms.TextBox textbox = new System.Windows.Forms.TextBox();
                textbox.Name = "T"+i;
                this.Controls.Add(label);
                this.Controls.Add(textbox);
                if (i==0)
                {
                    label.Location = new Point(0, 0);
                    label.Size = new Size(50, 20);
                    textbox.Location = new Point(label.Right, label.Location.Y);
                    textbox.Size = new Size(120, label.Height);
                }
                else
                {
                    label.Location = new Point(labellast.Location.X, labellast.Bottom);
                    label.Size = labellast.Size;
                    textbox.Location = new Point(label.Right, label.Location.Y);
                    textbox.Size = new Size(120, label.Height);
                }
                labellast = label;
                textboxlast = textbox;
                textbox.Text = strings;
                label.Text = label.Name;
                i++;
            }
            Size = new Size(textboxlast.Location.X+textboxlast.Width + 1, textboxlast.Location.Y +textboxlast.Height + 1);
            textboxlast.Text = "kupa";
        }
    }
}
