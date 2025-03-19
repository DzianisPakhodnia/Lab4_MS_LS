using System;
using System.Drawing;
using System.Windows.Forms;

namespace Lab4_MS_LS
{
    // Интерфейс для отображения данных
    public interface IResultDisplay
    {
        void DisplayResults(CMS_LS LSA);
        void ClearView();
    }

    // Отдельный класс для настройки DataGridView
    public class DataGridViewConfigurator
    {
        public static void Setup(DataGridView grid, int cols, int rows, string firstCellValue)
        {
            grid.ColumnCount = cols;
            grid.RowCount = rows;
            grid.ColumnHeadersVisible = false;
            grid.RowHeadersVisible = false;
            grid.Rows[0].Cells[0].Value = firstCellValue;
            for (int i = 0; i < cols; i++) grid.Columns[i].Width = 50;
        }
    }

    // Отдельный класс для работы с таблицей операций
    public class OperationsTableFiller
    {
        public static void Fill(DataGridView grid, CMS_LS LSA)
        {
            for (int i = 0; i < LSA.countOperations; i++)
            {
                grid.Rows[i + 1].Cells[0].Value = i + 1;
            }
            for (int i = 0; i < LSA.countTypes; i++)
            {
                foreach (int op in LSA.arrayTypes[i])
                {
                    grid.Rows[op + 1].Cells[i + 1].Style.BackColor = Color.Blue;
                }
            }
        }
    }

    // Отдельный класс для работы с матрицей смежности
    public class AdjacencyMatrixFiller
    {
        public static void Fill(DataGridView grid, CMS_LS LSA)
        {
            for (int i = 0; i < LSA.countOperations; i++)
            {
                foreach (int k in LSA.arrayH[i])
                {
                    grid.Rows[i + 1].Cells[k + 1].Style.BackColor = Color.Blue;
                }
            }
        }
    }

    // Класс отображения результатов, реализующий интерфейс
    public class ResultViewer : IResultDisplay
    {
        private readonly RichTextBox _richTextBox;
        private readonly DataGridView _dataGridView1;
        private readonly DataGridView _dataGridView2;

        public ResultViewer(RichTextBox richTextBox, DataGridView dataGridView1, DataGridView dataGridView2)
        {
            _richTextBox = richTextBox;
            _dataGridView1 = dataGridView1;
            _dataGridView2 = dataGridView2;
        }

        public void ClearView()
        {
            _dataGridView1.ColumnCount = 1;
            _dataGridView1.RowCount = 1;
            _dataGridView2.ColumnCount = 1;
            _dataGridView2.RowCount = 1;
        }

        public void DisplayResults(CMS_LS LSA)
        {
            _richTextBox.Text = "Всего шагов: " + (LSA.steps.Count - 1);
            for (int i = 0; i < LSA.steps.Count; i++)
            {
                _richTextBox.Text += "\nНа " + i + " шаге запланированы операции: " + string.Join(", ", LSA.steps[i]);
            }

            DataGridViewConfigurator.Setup(_dataGridView1, LSA.countTypes + 1, LSA.countOperations + 1, "Operation \\ type");
            OperationsTableFiller.Fill(_dataGridView1, LSA);

            DataGridViewConfigurator.Setup(_dataGridView2, LSA.countOperations + 1, LSA.countOperations + 1, "Operation \\ operation");
            AdjacencyMatrixFiller.Fill(_dataGridView2, LSA);
        }
    }
}