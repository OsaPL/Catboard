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
        Form1 mainform;
        public SoundsForm(Form1 form)
        {
            InitializeComponent();
            mainform = form;
        }
        private void SoundsForm_Load(object sender, EventArgs e)
        {
            int i = 0;
            Label labellast = null;
            Button buttonlast = null;
            foreach (string strings in mainform.soundsList)
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                label.Name = "L" + i;
                System.Windows.Forms.Button button = new System.Windows.Forms.Button();
                button.Name = "" + i;
                System.Windows.Forms.Button buttonRemove = new System.Windows.Forms.Button();
                button.Name = "" + i;
                this.Controls.Add(label);
                this.Controls.Add(button);
                this.Controls.Add(buttonRemove);
                if (i == 0)
                {
                    label.Location = new Point(0, 0);
                    label.Size = new Size(150, 20);
                    button.Location = new Point(label.Right, label.Location.Y);
                    button.Size = new Size(60, label.Height);
                    buttonRemove.Location = new Point(button.Right, label.Location.Y);
                    buttonRemove.Size = new Size(60, label.Height);
                }
                else
                {
                    label.Location = new Point(labellast.Location.X, labellast.Bottom);
                    label.Size = labellast.Size;
                    button.Location = new Point(label.Right, label.Location.Y);
                    button.Size = new Size(60, label.Height);
                    buttonRemove.Location = new Point(button.Right, label.Location.Y);
                    buttonRemove.Size = new Size(60, label.Height);
                }
                labellast = label;
                buttonlast = buttonRemove;
                button.Text = "Browse";
                buttonRemove.Text = "Remove";


                button.Click += new EventHandler(this.button_Click);
                button.Click += new EventHandler(this.buttonRemove_Click);

                label.Text = strings.Substring(strings.LastIndexOf("/") + 1);
                i++;
            }
            i+=2;
            Size = new Size(buttonlast.Width * i, buttonlast.Height * i);
            
        }
        private void button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1;
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                int i = 0;
                Int32.TryParse((sender as Button).Name, out i);

                mainform.soundsList[i] = openFileDialog1.FileName;
                
            }
        }
        private void buttonRemove_Click(object sender, EventArgs e)
        {
                
        }
    }
}
