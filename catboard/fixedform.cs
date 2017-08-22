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
        ToolTip tooltip;
        public fixedform(Form1 form)
        {
            InitializeComponent();
            mainform = form;
            mainform.childopen = true;
            tooltip = new ToolTip();

            AddEvents();
        }

        private void AddEvents()
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Click += Ctrl_Click;
            }
        }

        private void Ctrl_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1;
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                Button button = sender as Button;
                int temp = 0;
                Int32.TryParse(button.Name.Substring(1), out temp);

                for(int i =0;i<mainform.soundsList.Count;i++)
                {
                    if (mainform.soundsList[i].Substring(0, mainform.soundsList[i].IndexOf(" ")) == (sender as Control).Text)
                    {
                        mainform.soundsList[i] = (sender as Control).Text + " " + openFileDialog1.FileName;
                        return;
                    }
                }

                mainform.soundsList.Add((sender as Control).Text + " " + openFileDialog1.FileName);
            }
        }

        private void fixedform_Load(object sender, EventArgs e)
        {
            string sound;
            foreach (Control ctrl in this.Controls)
            {
                sound = GetSound(ctrl.Text);
                if (sound != String.Empty)
                {
                    tooltip.SetToolTip(ctrl, ctrl.Text + "[" + sound + "]");
                }
                else
                {
                    tooltip.SetToolTip(ctrl, ctrl.Text);
                }
            }
        }

        public string GetSound(string key)
        {
            if(mainform.soundsList.Count > 0)
            {
                foreach (string str in mainform.soundsList)
                {
                    if(key.IndexOf(" ") >-1)
                    {
                        if (str == key.Substring(0, key.IndexOf(" ")))
                        {
                            return key.Substring(0, key.IndexOf(" "));
                        }
                    }
                }
            }

            return String.Empty;
        }
    }
}
