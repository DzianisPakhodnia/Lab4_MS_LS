using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Lab4_MS_LS
{
    public partial class Form1 : Form
    {
        CMS_LS LSA = new CMS_LS();
        private ResultViewer resultViewer;
        public Form1()
        {
            InitializeComponent();
            resultViewer = new ResultViewer(richTextBox1, dataGridView1, dataGridView2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "data2.json");
            string json = File.ReadAllText(fullPath);
            Model data = JsonConvert.DeserializeObject<Model>(json);

            LSA.Clear_Object();
            resultViewer.ClearView();
            LSA.File_Load(data);
            LSA.Planning();
            resultViewer.DisplayResults(LSA);
        }

        
    }
}
