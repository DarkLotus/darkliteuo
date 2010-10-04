using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DarkLiteUO
{
    public partial class Form2 : Form
    {
        private Form1 form1;


        public Form2(Form1 form1)
        {
            // TODO: Complete member initialization
            this.form1 = form1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Profiles profile = new Profiles();
            profile.Add(txtUser.Text, txtPass.Text, txtIP.Text, txtPort.Text, txtCharslot.Text);
            Profiles.Serialize("config.xml", profile);
            this.Close();
        }
    }


    
    }

