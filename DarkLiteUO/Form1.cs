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

       // public delegate void Client_onCharacterListReceive(ref UOLite2.LiteClient Client, System.Collections.ArrayList CharacterList);
        
        public Form1()
        {

           
            InitializeComponent();

            

            Profiles Profile = Profiles.Deserialize("config.xml");

            foreach (config config in Profile.Profileslist)
            {
                cmbProfileList.Items.Add(config);
            }
        


            
        }

       

        

        

        private void btnConnect_Click(object sender, EventArgs e)
        {
            config config = (config)cmbProfileList.SelectedItem;
            myTabPage page2 = new myTabPage(config);
            tabMain.TabPages.Add((TabPage)page2);

            


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
            Options form = new Options(this);
            form.Show();
        }

        private void dissconnect_Click(object sender, EventArgs e)
        {
            tabMain.TabPages.Remove(tabMain.SelectedTab);
            
        }

        

        
    }


}
