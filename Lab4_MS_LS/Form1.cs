using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab4_MS_LS
{
    public partial class Form1 : Form
    {
        CMS_LS LSA = new CMS_LS();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LSA.Clear_Object();
            ClearView();
            LSA.File_Load("start_param.txt");
            LSA.Planning();
            ViewResults();
        }

        private void ViewResults()
        {
            // шаги
            richTextBox1.Text = "Всего шагов: " + (LSA.steps.Count - 1);

            for (int i = 0; i < LSA.steps.Count; i++)
            {
                richTextBox1.Text += "\nНа " + (i) + " шаге запланированы следующие операции: ";

                for (int k = 0; k < LSA.steps[i].Count; k++)
                {
                    richTextBox1.Text += " " + (LSA.steps[i][k] + 1);
                }
                richTextBox1.Text += "\nСостояние списка: ( ";
                for (int t = 0; t < LSA.List_operations_on_steps[i][0].Count; t++)
                {
                    if (t == LSA.List_operations_on_steps[i][0].Count - 1)
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][0][t] + 1) + " ";
                    else
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][0][t] + 1) + ", ";
                }

                richTextBox1.Text += "| ";
                for (int t = 0; t < LSA.List_operations_on_steps[i][1].Count; t++)
                {
                    if (t == LSA.List_operations_on_steps[i][1].Count - 1)
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][1][t] + 1) + " ";
                    else
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][1][t] + 1) + ", ";
                }

                richTextBox1.Text += "| ";
                for (int t = 0; t < LSA.List_operations_on_steps[i][2].Count; t++)
                {
                    if (t == LSA.List_operations_on_steps[i][2].Count - 1)
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][2][t] + 1) + " ";
                    else
                        richTextBox1.Text += (LSA.List_operations_on_steps[i][2][t] + 1) + ", ";
                }

                richTextBox1.Text += ")\n";

            }



            // типы операций

            dataGridView1.ColumnCount = LSA.countTypes + 1;
            dataGridView1.RowCount = LSA.countOperations + 1;
            dataGridView1.Rows[0].Cells[0].Value = "Operation \\  type";
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;

            for (int i = -1; i < LSA.countTypes; i++)
            {
                dataGridView1.Rows[0].Cells[i + 1].Style.BackColor = Color.FromArgb(110, 110, 110);
                dataGridView1.Rows[0].Cells[i + 1].Style.ForeColor = Color.FromArgb(255, 255, 255);
                if (i > -1)
                {
                    dataGridView1.Rows[0].Cells[i + 1].Value = i + 1;
                    dataGridView1.Columns[i + 1].Width = 50;
                }
            }

            for (int i = 0; i < LSA.countOperations; i++)
            {
                dataGridView1.Rows[i + 1].Cells[0].Style.BackColor = Color.FromArgb(110, 110, 110);
                dataGridView1.Rows[i + 1].Cells[0].Style.ForeColor = Color.FromArgb(255, 255, 255);
                dataGridView1.Rows[i + 1].Cells[0].Value = i + 1;

            }

            for (int i = 0; i < LSA.countTypes; i++)
            {
                for (int k = 0; k < LSA.arrayTypes[i].Length; k++)
                {
                    dataGridView1.Rows[LSA.arrayTypes[i][k]].Cells[i + 1].Style.BackColor = Color.FromArgb(82, 97, 160);
                }
            }

            // матрица смежности

            dataGridView2.ColumnCount = LSA.countOperations + 1;
            dataGridView2.RowCount = LSA.countOperations + 1;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

            for (int i = 0; i < LSA.countOperations + 1; i++)
            {
                // по вертикали
                dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(110, 110, 110);
                dataGridView2.Rows[i].Cells[0].Style.ForeColor = Color.FromArgb(255, 255, 255);
                dataGridView2.Rows[i].Cells[0].Value = i;
                // по горизонали
                dataGridView2.Rows[0].Cells[i].Style.ForeColor = Color.FromArgb(255, 255, 255);
                dataGridView2.Rows[0].Cells[i].Style.BackColor = Color.FromArgb(110, 110, 110);
                dataGridView2.Rows[0].Cells[i].Value = i;

                dataGridView2.Columns[i].Width = 50;
            }

            dataGridView2.Rows[0].Cells[0].Value = "Operation \\  operation";

            for (int i = 0; i < LSA.countOperations; i++)
            {
                for (int k = 0; k < LSA.arrayH[i].Length; k++)
                {
                    if (LSA.arrayH[i][k] == 1)
                    {
                        dataGridView2.Rows[i + 1].Cells[k + 1].Style.BackColor = Color.FromArgb(82, 97, 160);
                    }
                }
            }
        }

        private void ClearView()
        {
            dataGridView1.ColumnCount = 1;
            dataGridView1.RowCount = 1;
            dataGridView2.ColumnCount = 1;
            dataGridView2.RowCount = 1;
        }
    }
}
