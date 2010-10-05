using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UOLite2;
using System.Reflection;

namespace DarkLiteUO
{
    public partial class myTabPage : TabPage// : UserControl//
    {
        private Dictionary<String, GameVariable[]> variableCategs;
        private class GameVariable
        {
            public String name;
            public TreeNode node;
        }
        public UOLite2.Item Item;
        public UOLite2.LiteClient Client = new UOLite2.LiteClient("C:");
        private UOLite2.Serial _Player = new UOLite2.Serial(0);
        private UOLite2.Mobile _Mount = null;
        private bool bConnected = false;
        //Script myscript;
        private Thread ScriptThread;
        delegate void SetTextCallback(string text);
        
        private String _name = "";
        public myTabPage(config config)
        {
            _name = config.Username;
            //this.Name = config.Username;
            this.Text = config.Username;
            Client.onSkillUpdate += new LiteClient.onSkillUpdateEventHandler(Client_onSkillUpdate);
            Client.onRecievedServerList += new LiteClient.onRecievedServerListEventHandler(Client_onRecievedServerList);
            Client.onLoginConfirm += new LiteClient.onLoginConfirmEventHandler(Client_onLoginConfirm);
            Client.onLoginComplete += new LiteClient.onLoginCompleteEventHandler(Client_onLoginComplete);
            Client.onError += new LiteClient.onErrorEventHandler(Client_onError);
            Client.onLoginDenied += new LiteClient.onLoginDeniedEventHandler(Client_onLoginDenied);

            Client.onCliLocSpeech += new LiteClient.onCliLocSpeechEventHandler(Client_onCliLocSpeech);
            Client.onSpeech += new LiteClient.onSpeechEventHandler(Client_onSpeech);
            Client.onCharacterListReceive += new UOLite2.LiteClient.onCharacterListReceiveEventHandler(Client_onCharacterListReceive);
            Client.onPacketReceive += new UOLite2.LiteClient.onPacketReceiveEventHandler(Client_onPacketReceive);
            Client.onConnectionLoss += new LiteClient.onConnectionLossEventHandler(Client_onConnectionLoss);
            Client.onMovementBlocked += new LiteClient.onMovementBlockedEventHandler(Client_onMovementBlocked);
            Client.onNewGump += new LiteClient.onNewGumpEventHandler(Client_onNewGump);
            Client.onNewItem += new LiteClient.onNewItemEventHandler(Client_onNewItem);
            Client.onNewMobile += new LiteClient.onNewMobileEventHandler(Client_onNewMobile);
            Client.onPlayerMove += new LiteClient.onPlayerMoveEventHandler(Client_onPlayerMove);
            Client.onTargetRequest += new LiteClient.onTargetRequestEventHandler(Client_onTargetRequest);
            InitializeComponent();
            setuptreeview();
            String status = Client.GetServerList(config.Username, config.Password, config.IP, config.Port); ;
            if (status == "SUCCESS")
            {
                UpdateLog("Connected to server: " + Client.LoginServerAddress + ":" + Client.LoginPort.ToString());

            }
            else { UpdateLog(status); }
            
        }
        public override string ToString()
        {
            return _name;
        }
        private void btn_clearlog_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Client.Connected)
            {
                ScriptCompiler.Initialize(10, ref Client, this);
                NamespaceToAssembly.Initialize();
                if (ScriptCompiler.Compile(Assembly.GetExecutingAssembly().Location, txtScriptBox.Text))
                {
                    UpdateLog("Script Started");
                }
                else { UpdateLog("Script Compile Failed"); }
            }
            else
            {
                txtOutput.Text = txtOutput.Text + "\r\nNot Connected!";
            }
        }

        private void btn_stopscript_Click(object sender, EventArgs e)
        {
            ScriptCompiler.StopScript();
            if (ScriptThread.IsAlive) { ScriptThread.Abort(); }

        }
        public void UpdateLog(String text)
        {
            text = (text + Environment.NewLine);
            if (this.txtOutput.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(UpdateLog);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                txtOutput.AppendText(text);
            }
        }

        private void setuptreeview()
        {
            //Timo 'BtbN' Rothenpieler Code
            variableCategs = new Dictionary<String, GameVariable[]>
            {
                {
                    "Client",
                    new GameVariable[]
                    {
                        //new GameVariable { name = "CliCnt", node = null },
                        //new GameVariable { name = "CliLang", node = null },
                       // new GameVariable { name = "CliLogged", node = null },
                        //new GameVariable { name = "CliNr", node = null },
                        //new GameVariable { name = "CliLeft", node = null },
                       // new GameVariable { name = "CliTop", node = null },
                        //new GameVariable { name = "CliVer", node = null },
                       // new GameVariable { name = "CliXRes", node = null },
                        //new GameVariable { name = "CliYRes", node = null }
                    }
                },
                {
                    "Character",
                    new GameVariable[]
                    {
                        new GameVariable { name = "CharPosX", node = null },
                        new GameVariable { name = "CharPosY", node = null },
                        new GameVariable { name = "CharPosZ", node = null },
                        new GameVariable { name = "CursKind", node = null },
                        new GameVariable { name = "CharDir", node = null },
                        new GameVariable { name = "BackpackID", node = null },
                        new GameVariable { name = "CharID", node = null },
                        new GameVariable { name = "CharName", node = null },
                        new GameVariable { name = "CharStatus", node = null },
                        new GameVariable { name = "CharType", node = null },
                        new GameVariable { name = "Sex", node = null },
                    }
                },
                {
                    "Status",
                    new GameVariable[]
                    {
                        new GameVariable { name = "Str", node = null },
                        new GameVariable { name = "Dex", node = null },
                        new GameVariable { name = "Int", node = null },
                        new GameVariable { name = "MaxStats", node = null },
                        new GameVariable { name = "Hits", node = null },
                        new GameVariable { name = "MaxHits", node = null },
                        new GameVariable { name = "Stamina", node = null },
                        new GameVariable { name = "MaxStam", node = null },
                        new GameVariable { name = "Mana", node = null },
                        new GameVariable { name = "MaxMana", node = null },
                        new GameVariable { name = "MaxFol", node = null },
                        new GameVariable { name = "Followers", node = null },
                        new GameVariable { name = "MinDmg", node = null },
                        new GameVariable { name = "MaxDmg", node = null },
                        new GameVariable { name = "Weight", node = null },
                        new GameVariable { name = "MaxWeight", node = null },
                        new GameVariable { name = "Luck", node = null },
                        new GameVariable { name = "Gold", node = null },
                        new GameVariable { name = "AR", node = null },
                        new GameVariable { name = "PR", node = null },
                        new GameVariable { name = "FR", node = null },
                        new GameVariable { name = "CR", node = null },
                        new GameVariable { name = "ER", node = null },
                        new GameVariable { name = "TP", node = null },
                    }
                },
                {
                    "Last Action",
                    new GameVariable[]
                    {
                        new GameVariable { name = "LObjectID", node = null },
                        new GameVariable { name = "LObjectType", node = null },
                        new GameVariable { name = "LTargetID", node = null },
                        new GameVariable { name = "LTargetKind", node = null },
                        new GameVariable { name = "LTargetTile", node = null },
                        new GameVariable { name = "LTargetX", node = null },
                        new GameVariable { name = "LTargetY", node = null },
                        new GameVariable { name = "LTargetZ", node = null },
                        new GameVariable { name = "LLiftedID", node = null },
                        new GameVariable { name = "LLiftedKind", node = null },
                        new GameVariable { name = "LLiftedType", node = null },
                        new GameVariable { name = "LSkill", node = null },
                        new GameVariable { name = "LSpell", node = null },
                    }
                },
                {
                    "Container Info",
                    new GameVariable[]
                    {
                        new GameVariable { name = "ContID", node = null },
                        new GameVariable { name = "ContName", node = null },
                        new GameVariable { name = "ContKind", node = null },
                        new GameVariable { name = "ContType", node = null },
                        new GameVariable { name = "ContPosX", node = null },
                        new GameVariable { name = "ContPosY", node = null },
                        new GameVariable { name = "ContSizeX", node = null },
                        new GameVariable { name = "ContSizeY", node = null },
                        new GameVariable { name = "NextCPosX", node = null },
                        new GameVariable { name = "NextCPosY", node = null }
                    }
                },
                {
                    "Misc",
                    new GameVariable[]
                    {
                        new GameVariable { name = "TargCurs", node = null },
                        new GameVariable { name = "Shard", node = null },
                        new GameVariable { name = "LShard", node = null },
                        new GameVariable { name = "EnemyHits", node = null },
                        new GameVariable { name = "EnemyID", node = null },
                        new GameVariable { name = "LHandID", node = null },
                        new GameVariable { name = "RHandID", node = null },
                        new GameVariable { name = "SysMsg", node = null }
                    }
                }
            };
            setUpVariablesTree();
            // vartree.Nodes.Add("Player");
            //vartree.Nodes[0].Nodes.Add("Name:" + uonet.player.name);
        }

        private void updateVarsTimer_Tick(object sender, EventArgs e)
        {
            if (!bConnected) { return; }
            // if (vartree.Focused) return; // make it copy-able

            foreach (GameVariable[] vars in variableCategs.Values)
                foreach (GameVariable var in vars)
                {
                    String temp = "";
                    //var.node.
                    //.temp = var.name + " = " + var.var;
                    switch (var.name)
                    {
                        case "CharPosX":
                            temp = var.name + " = " + Client.Player.X;
                            break;
                        case "CharPosY":
                            temp = var.name + " = " + Client.Player.Y;
                            break;
                        case "CharPosZ":
                            temp = var.name + " = " + Client.Player.Z;
                            break;
                        case "BackpackID":
                            // temp = var.name + " = " + uonet.player.BackpackID;
                            break;
                        case "CharID":
                            temp = var.name + " = " + Client.Player.Serial;
                            break;
                        case "CharStatus":
                            //  temp = var.name + " = " + uonet.player.flags;
                            break;
                        case "CharType":
                            temp = var.name + " = " + Client.Player.Type;
                            break;
                        case "Sex":
                            //  temp = var.name + " = " + uonet.player.Sex;
                            break;
                        case "CharName":
                            temp = var.name + " = " + Client.Player.Name;
                            break;
                        case "CursKind":
                            // temp = var.name + " = " + uonet.player.Facet;
                            break;
                        case "Str":
                            temp = var.name + " = " + Client.Player.Strength;
                            break;
                        case "Dex":
                            temp = var.name + " = " + Client.Player.Dexterity;
                            break;
                        case "Int":
                            temp = var.name + " = " + Client.Player.Intelligence;
                            break;
                        case "MaxStats":
                            //temp = var.name + " = " + uonet ;
                            break;
                        case "Hits":
                            temp = var.name + " = " + Client.Player.Hits;
                            break;
                        case "MaxHits":
                            temp = var.name + " = " + Client.Player.HitsMax;
                            break;
                        case "Stamina":
                            temp = var.name + " = " + Client.Player.Stamina;
                            break;
                        case "MaxStam":
                            temp = var.name + " = " + Client.Player.StaminaMax;
                            break;
                        case "Mana":
                            temp = var.name + " = " + Client.Player.Mana;
                            break;
                        case "MaxMana":
                            //   temp = var.name + " = " + uonet.player.MaxMana;
                            break;
                        case "Weight":
                            // temp = var.name + " = " + uonet.player.Weight;
                            break;
                        case "MaxWeight":
                            //  temp = var.name + " = " + uonet.player.MaxWeight;
                            break;
                        case "Gold":
                            //  temp = var.name + " = " + uonet.player.Gold;
                            break;

                    }
                    if ((var.node.Text != temp) && (temp != "")) { var.node.Text = temp; }
                    /* GameDLL.SetTop(GH, 0);
                     GameDLL.PushStrVal(GH, "GetVar");
                     GameDLL.PushStrVal(GH, var.name);
                     if (GameDLL.Query(GH) != 0) continue;

                     String tmp = "";
                     switch (GameDLL.GetType(GH, 1))
                     {
                         case 1:
                             tmp = var.name + " = " + ((GameDLL.GetBoolean(GH, 1) != 0) ? "true" : "false");
                             break;
                         case 3:
                             tmp = var.name + " = " + GameDLL.GetInteger(GH, 1);
                             break;
                         case 4:
                             tmp = var.name + " = " + GameDLL.GetString(GH, 1);
                             break;
                         default:
                             tmp = var.name + " = nil";
                             break;
                     }
                     if (var.node.Text != tmp)
                         var.node.Text = tmp;*/
                }
        }

        private void setUpVariablesTree()
        {
            //Timo 'BtbN' Rothenpieler Code
            vartree.Nodes.Clear();

            foreach (String categ in variableCategs.Keys)
            {
                TreeNode node = vartree.Nodes.Add(categ);
                foreach (GameVariable var in variableCategs[categ])
                {
                    var.node = node.Nodes.Add(var.name);
                    var.node.Text = var.name + " = ?";
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            
        }

   





    }
}
