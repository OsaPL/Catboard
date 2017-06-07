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
            mainform.childopen = true;
        }
        private void SoundsForm_Load(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                Label labellast = null;
                Button buttonlast = null;
                foreach (string strings in mainform.soundsList)
                {
                    Label label;
                    if (this.Controls.Find("L" + i, true).Length <= 0)
                        label = new System.Windows.Forms.Label();
                    else
                        label = this.Controls.Find("L" + i, true).FirstOrDefault() as Label;
                    label.Name = "L" + i;

                    Button button;
                    if (this.Controls.Find("L" + i, true).Length <= 0)
                    {
                        button = new System.Windows.Forms.Button();
                        button.Click += new EventHandler(this.button_Click);
                    }
                    else
                        button = this.Controls.Find("A" + i, true).FirstOrDefault() as Button;
                    button.Name = "A" + i;

                    Button buttonRemove;
                    if (this.Controls.Find("L" + i, true).Length <= 0)
                    { 
                        buttonRemove = new System.Windows.Forms.Button();
                        buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
                    }
                    else
                        buttonRemove = this.Controls.Find("R" + i, true).FirstOrDefault() as Button;
                    buttonRemove.Name = "R" + i;

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


                    

                    label.Text = strings.Substring(strings.LastIndexOf("\\") + 1);
                    i++;
                }
                i += 2;
                Size = new Size(buttonlast.Right + 80, buttonlast.Height * i);
                Button Rbutton = this.Controls.Find("R0", true).FirstOrDefault() as Button;
                //pointless failsafe??
                if (i == 3)
                {
                    Rbutton.ForeColor = Color.Gray;
                }
                else
                {

                        Control[] labels = this.Controls.Find("L", true);
                        foreach (Control Controls in labels)
                        {
                            Label temp = Controls as Label;
                            if (Controls.Text == "")
                                return;
                        }
                    Rbutton.ForeColor = Color.Black;
                }
            }
            catch(Exception er)
            {
                mainform.soundsList.Clear();
                mainform.soundsList.Add("Sounds\\meow.mp3");
                mainform.soundsList.Add("Sounds\\meow1.mp3");
                mainform.soundsList.Add("Sounds\\woof.mp3");
                mainform.soundsList.Add("Sounds\\woof2.mp3");
                SoundsForm_Load(this, null);
            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1;
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Button button = sender as Button;
                int i = 0;
                Int32.TryParse(button.Name.Substring(1), out i);
                
                mainform.soundsList[i] = openFileDialog1.FileName;
                Label label = this.Controls.Find("L" + i, true).FirstOrDefault() as Label;
                label.Text = mainform.soundsList[i].Substring(mainform.soundsList[i].LastIndexOf("\\") + 1);
                SoundsForm_Load(this, null);
            }
        }
        private void buttonRemove_Click(object sender, EventArgs e)
        { 
            Label label;
            Button abutton;
            Button rbutton = sender as Button;
            if (rbutton.ForeColor == Color.Gray)
                return;
            int i;
            Int32.TryParse(rbutton.Name.Substring(1),out i);
            label = this.Controls.Find("L" + i, true).FirstOrDefault() as Label;
            abutton = this.Controls.Find("A" + i, true).FirstOrDefault() as Button;
            Controls.Remove(label);
            label.Dispose();
            Controls.Remove(abutton);
            abutton.Dispose();
            Controls.Remove(rbutton);
            rbutton.Dispose();
            mainform.soundsList.RemoveAt(i);
            SoundsForm_Load(this, null);
        }

        SoundsForm newsoundsform;
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            mainform.soundsList.Add("");
            SoundsForm_Load(this, null);
        }
        
        private void SoundsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainform.Focus(); //make usre it is focused after closing children
            //make sure there are no empty fields, if they are, delete
            int i = 0;
            List<Int32> idtoremove = new List<Int32>();
            foreach (string strings in mainform.soundsList)
            {
                if (strings == "")
                    idtoremove.Insert(0, i);
                i++;
            }
            foreach (Int32 id in idtoremove)
            {
                mainform.soundsList.RemoveAt(id);
            }
        }
    }
}
