using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace window
{
    public partial class Form1 : Form
    {

        //коллекция посещённых адрессов
        ArrayList Adresses = new ArrayList();
        //индекс текущего адресса из коллекции Adresses
        int currIndex = -1;
        //текущий адресс
        string currListViewAdress = "";

        public Form1()
        {

            this.AcceptButton = this.button3;
            InitializeComponent();
            
            //заполнение TreeView узлами локальных дисков и заполнение дочерних узлов этих дисков
            string[] str = Environment.GetLogicalDrives();
            int n = 1;
            foreach (string s in str)
            {
                try
                {
                    TreeNode tn = new TreeNode();
                    tn.Name = s;
                    tn.Text = "Локальный диск " + s;
                    treeView1.Nodes.Add(tn.Name, tn.Text, 2);
                    FileInfo f = new FileInfo(s);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(s);
                    foreach (string s2 in str2)
                    {
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        ((TreeNode)treeView1.Nodes[n - 1]).Nodes.Add(s2, t, 0);
                    }
                }
                catch { }
                n++;
            }
            foreach (TreeNode tn in treeView1.Nodes)
            {
                for (int i = 65; i < 91; i++)
                {
                    char sym = Convert.ToChar(i);
                    if (tn.Name == sym + ":\\")
                        tn.SelectedImageIndex = 2;
                }
            }

        }
       
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string strtmp = "";
            if (Adresses.Count != 0)
            {
                strtmp = ((string)Adresses[Adresses.Count - 1]);
                Adresses.Clear();
                Adresses.Add(strtmp);
                currIndex = 0;
            }
            Adresses.Add(e.Node.Name);
            currIndex++;
            //проверка возможности перехода назад/вперёд
            if (currIndex + 1 == Adresses.Count)
                button2.Enabled = false;
            else
                button2.Enabled = true;
            if (currIndex - 1 == -1)
                button1.Enabled = false;
            else
                button1.Enabled = true;
            listView1.Items.Clear();
            currListViewAdress = e.Node.Name;
            textBox1.Text = currListViewAdress;
            //заполнение ListView
            try
            {
                
                    FileInfo f = new FileInfo(e.Node.Name);
                    string t = "";
                    string[] str2 = Directory.GetDirectories(e.Node.Name);
                    ListViewItem lw = new ListViewItem();
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, "", f.LastWriteTime.ToString() }, 0);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                    str2 = Directory.GetFiles(e.Node.Name);
                    foreach (string s2 in str2)
                    {
                        f = new FileInfo(s2);
                        t = s2.Substring(s2.LastIndexOf('\\') + 1);
                        lw = new ListViewItem(new string[] { t, f.Length.ToString(), f.LastWriteTime.ToString() }, 1);
                        lw.Name = s2;
                        listView1.Items.Add(lw);
                    }
                
                
            }
            catch { }

            int i = 0;
            //заполнение дочерних узлов дочерними узлами развёртываемого узала
            try
            {
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    string[] str2 = Directory.GetDirectories(tn.Name);
                    foreach (string str in str2)
                    {
                        TreeNode temp = new TreeNode();
                        temp.Name = str;
                        temp.Text = str.Substring(str.LastIndexOf('\\') + 1);
                        e.Node.Nodes[i].Nodes.Add(temp);
                    }
                    i++;
                }
            }
            catch { }

        }
      

        private void button1_Click(object sender, EventArgs e)
        {
            //обработка "Назад"
            if (currIndex - 1 != -1)
            {
                currIndex--;
                currListViewAdress = ((string)Adresses[currIndex]);
                if (currIndex + 1 == Adresses.Count)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;
                if (currIndex - 1 == -1)
                    button2.Enabled = false;
                else
                    button1.Enabled = true;
                textBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(currListViewAdress);
                string t = "";
                string[] str2 = Directory.GetDirectories(currListViewAdress);
                string[] str3 = Directory.GetFiles(currListViewAdress);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();

                foreach (string s2 in str2)
                {
                    f = new FileInfo(s2);
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, "", f.LastWriteTime.ToString() }, 0);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
                foreach (string s2 in str3)
                {
                    f = new FileInfo(s2);
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, f.Length.ToString(), f.LastWriteTime.ToString() }, 1);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //обработка "Вперёд"
            if (currIndex + 1 != Adresses.Count)
            {
                currIndex++;
                currListViewAdress = ((string)Adresses[currIndex]);
                if (currIndex + 1 == Adresses.Count)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;
                if (currIndex - 1 == -1)
                    button1.Enabled = false;
                else
                    button1.Enabled = true;
                textBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(currListViewAdress);
                string t = "";
                string[] str2 = Directory.GetDirectories(currListViewAdress);
                string[] str3 = Directory.GetFiles(currListViewAdress);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();

                foreach (string s2 in str2)
                {
                    f = new FileInfo(s2);
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, "", f.LastWriteTime.ToString() }, 0);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
                foreach (string s2 in str3)
                {
                    f = new FileInfo(s2);
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, f.Length.ToString(), f.LastWriteTime.ToString() }, 1);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }


            }



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //переход в указанную директорию по нажатию на кнопку

            try
            {
              
                string[] str2 = Directory.GetDirectories(textBox1.Text);
                string[] str3 = Directory.GetFiles(textBox1.Text);
                currIndex++;
                currListViewAdress = textBox1.Text;
                Adresses.Add(textBox1.Text);
                if (currIndex + 1 == Adresses.Count)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;
                if (currIndex - 1 == -1)
                    button1.Enabled = false;
                else
                    button1.Enabled = true;
                FileInfo f = new FileInfo(textBox1.Text);
                string t = "";
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();

                foreach (string s2 in str2)
                {
                    f = new FileInfo(s2);

                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, "", f.LastWriteTime.ToString() }, 0);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
                foreach (string s2 in str3)
                {
                    f = new FileInfo(s2);

                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, f.Length.ToString(), f.LastWriteTime.ToString() }, 1);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
            }



            catch
            {
                textBox1.Text = currListViewAdress;
            }
           
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems[0].Text.IndexOf('.') == -1)
            {
                //обработка нажатия на папку
                Adresses.Add(listView1.SelectedItems[0].Name);
                currIndex++;
                currListViewAdress = ((string)Adresses[currIndex]);
                
                if (currIndex + 1 == Adresses.Count)
                    button2.Enabled = false;
                else
                    button2.Enabled = true;
                if (currIndex - 1 == -1)
                    button1.Enabled = false;
                else
                    button1.Enabled = true;
                currListViewAdress = listView1.SelectedItems[0].Name;
                textBox1.Text = currListViewAdress;
                FileInfo f = new FileInfo(listView1.SelectedItems[0].Name);
                string t = "";
                string[] str2 = Directory.GetDirectories(listView1.SelectedItems[0].Name);
                string[] str3 = Directory.GetFiles(listView1.SelectedItems[0].Name);
                listView1.Items.Clear();
                ListViewItem lw = new ListViewItem();

                foreach (string s2 in str2)
                {
                    f = new FileInfo(s2);
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, "", f.LastWriteTime.ToString() }, 0);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
                foreach (string s2 in str3)
                {
                    f = new FileInfo(s2);
                    
                    t = s2.Substring(s2.LastIndexOf('\\') + 1);
                    lw = new ListViewItem(new string[] { t, f.Length.ToString(), f.LastWriteTime.ToString() }, 1);
                    lw.Name = s2;
                    listView1.Items.Add(lw);
                }
            }
            else
            {
                //обработка нажатия на файл(его запуска)
                System.Diagnostics.Process MyProc = new System.Diagnostics.Process();
                MyProc.StartInfo.FileName = listView1.SelectedItems[0].Name;
                MyProc.Start();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
