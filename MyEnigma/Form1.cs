using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyEnigma
{
    public partial class Form1 : Form
    {
        public string text;
        public string patch;
        public Form1()
        {
            InitializeComponent();
        }
        private static string Encrypt(string message, int key, string pass)
        {
            string result = "";
            for (int i = 0; i < pass.Length; i++)
                result += (char)(pass[i] ^ key);


            for (int i = pass.Length; i < message.Length; i++)
                result += (char)(message[i] ^ key);
            return result;
        }

        private static string Decrypt(string message, int key, string pass )
        {
            string encryptText = "";
                for (int i = 0; i < message.Length; i++)
                encryptText += (char)(message[i] ^ key);

            if (encryptText.Substring(0, pass.Length) == pass)
            {
                encryptText.Remove(0, pass.Length);
                return encryptText;
            }
            else return "Incorrect password";

            //return encryptText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                patch = fileDialog.FileName;
                PatchBox.Text = patch;

                text = File.ReadAllText(fileDialog.FileName, Encoding.Default);
            }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            PatchBox.Clear();
            progressBar1.Value = 100;
            progressBar1.Style = ProgressBarStyle.Marquee;
            var task = new Task(() =>
            {
                if (radioButton1.Checked)
                {
                    using (StreamWriter sw = new StreamWriter(patch, false, Encoding.Default))
                    {
                        sw.WriteLine(Encrypt(text, 5, PassTextBox.Text));
                        PassTextBox.Clear();

                    }
                }
                else if (radioButton2.Checked && Decrypt(text, 5, PassTextBox.Text) != "Incorrect password")
                {
                    using (StreamWriter sw = new StreamWriter(patch, false, Encoding.Default))
                    {
                        sw.WriteLine(Decrypt(text, 5, PassTextBox.Text));
                        PassTextBox.Clear();
                    }
                }
                
            }
            );
            task.Start();
            task.ContinueWith((x) => {
                progressBar1.Style = ProgressBarStyle.Blocks;
                MessageBox.Show("Complete"); });
           if (task.IsFaulted)
            {
                MessageBox.Show("Incorrect Password");
            }
        }
    }
}
