using System;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace WhosHostingMe
{
    public partial class whoisHostingme : Form
    {

        // Default VARS
        static string twitchName = "saladin1980";
        static string id = null;

        public whoisHostingme()
        {
            

            InitializeComponent();
            
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            label3.Text = "v." + version;
            getID(twitchName);
            getHosts(id);
        }

        public static void getID(string tn = "saladin1980")
        {

            string URL = "https://api.twitch.tv/kraken/users/"+ tn;
            using (var webClient = new System.Net.WebClient())
            {
                try
                {
                    var json2 = webClient.DownloadString(URL);
                }
                catch (Exception e)
                {
                    MessageBox.Show("error: please advise creater that an error happened on line:38");
                    MessageBox.Show("Error is: "+e);
                    MessageBox.Show("Closing the application due to the error stated. Please advise the coder for a resolution.");
                    Application.Exit();
                }

                var json = webClient.DownloadString(URL);
                JsonTextReader reader = new JsonTextReader(new StringReader(json));
                bool I = false;
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        string v = Convert.ToString(reader.Value);
                        if ("_id" == v)
                        {
                            I = true;
                        }
                        else if(I)
                        {
                            // MessageBox.Show(v);
                            I = false;
                            id = v;
                        }
                    }
                }
            }
        }
        private void getHosts(string id)
        {
            string URL = "http://tmi.twitch.tv/hosts?include_logins=1&target="+ id;
            List<string> _items = new List<string>();
            using (var webClient = new System.Net.WebClient())
            {
                string json = webClient.DownloadString(URL);
                JsonTextReader reader = new JsonTextReader(new StringReader(json));
                bool I = false;
                _items = new List<string>();
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
                        string v = Convert.ToString(reader.Value);
                        if ("host_login" == v)
                        {
                            I = true;
                            // MessageBox.Show("ID FOUND");
                        }
                        else if (I)
                        {
                            _items.Add(v);
                            I = false;
                        }
                    }
                }
                listBox1.DataSource = _items;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "Twitch Name") twitchName = "saladin1980";
            else twitchName = textBox1.Text;
            listBox1.DataSource = null;
            getID(twitchName);
            getHosts(id);
            textBox2.Text = listBox1.Items.Count.ToString();
        }



        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = null;
            s = twitchName +" is hosted by: ";
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if(i == 0) s = s + listBox1.Items[i].ToString();
                if(i>0) s = s + ", " + listBox1.Items[i].ToString();
                //MessageBox.Show(listBox1.Items[i].ToString());
            }
            MessageBox.Show("Copied to your clipboard: " + Environment.NewLine + s);
            Clipboard.SetText(s);
        }
        private void listBox1_MouseUp(object sender, MouseEventArgs e)

        {

            if (e.Button == MouseButtons.Right)

                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);

        }
    }
}
