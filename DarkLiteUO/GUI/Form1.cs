using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UOLite2;
using System.Reflection;
namespace DarkLiteUO
{
    public partial class Form1 : Form
    {
        Options Optionform;
       // public delegate void Client_onCharacterListReceive(ref UOLite2.LiteClient Client, System.Collections.ArrayList CharacterList);
        
        public Form1()
        {
            
            
            InitializeComponent();
            try
            {
                Profiles Profile = Profiles.Deserialize("config.xml");
                foreach (config config in Profile.Profileslist)
                {
                    cmbProfileList.Items.Add(config);
                }
            }
            catch { }

        


            
        }

        void Optionform_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Profiles Profile = Profiles.Deserialize("config.xml");
                foreach (config config in Profile.Profileslist)
                {
                    cmbProfileList.Items.Add(config);
                }
            }
            catch { }
            Optionform.FormClosing -= Optionform_FormClosing;
        }
       

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                config config = (config)cmbProfileList.SelectedItem;
                myTabPage page2 = new myTabPage(config);
               
                tabMain.TabPages.Add((TabPage)page2);
            }
            catch { }


        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScriptCompiler.ForceStopScript();
            this.Close();
        }


        private void btn_clearlog_Click(object sender, EventArgs e)
        {
            //txtOutput.Clear();
        }


         

        private void button3_Click(object sender, EventArgs e)
        {
           // Script myscript = new Script();
            //myscript.Start(ref Client, this);
          // ScriptThread = new Thread(myscript.Main);
           // ScriptThread.Start();
           
            //ScriptThread.IsBackground = true;
        }

        private void updateVarsTimer_Tick()
        {

        }

        private void addAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Optionform = new Options(this);
            Optionform.Show();
            Optionform.FormClosing += new FormClosingEventHandler(Optionform_FormClosing);
        }

        private void dissconnect_Click(object sender, EventArgs e)
        {
            tabMain.TabPages.Remove(tabMain.SelectedTab);
            
        }

        

        
    }


}
