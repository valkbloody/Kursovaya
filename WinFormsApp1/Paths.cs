using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public partial class _Database : Form
    {
        int row_index_paths = 0;
        void Add_Col_Path() // добавление столбца промежуточного пункта
        {
            string[] ports = new string[] { "-", "Мурманск", "Архангельск", "Мезень", "Певек", "Владивосток", "Магадан" };
            var oldcol = grid_add_path.Columns[1];
            var newcol = new DataGridViewComboBoxColumn();
            newcol.HeaderText = oldcol.HeaderText;
            newcol.Name = oldcol.Name;
            newcol.DataSource = ports;
            newcol.DataPropertyName = oldcol.DataPropertyName;
            grid_add_path.Columns.Insert(grid_add_path.ColumnCount - 2, newcol);
            if (grid_add_path.Columns.Count == Data.base_ports.Count) add_port.Visible = false;
        }
        private void button5_Click(object sender, EventArgs e) // добавление маршрута
        {
            del_path.Visible = true;
            row_index_paths = grid_paths.Rows.Count;
            panel3.Visible = true;
            while (grid_add_path.Columns.Count < grid_paths.Columns.Count) Add_Col_Path();
            grid_add_path.Rows.Clear();
            grid_add_path.Rows.Add();
            add_path.Visible = false;
        }
        private void button17_Click(object sender, EventArgs e) // сохранение маршрута
        {
            try
            {
                int empty_ports = -1;
                List<string> keys = new List<string> { };
                foreach (DataGridViewCell cell in grid_add_path.Rows[0].Cells)
                {
                    if (cell.Value == null || cell.Value.ToString() == "-" || cell.Value == "-")
                    {
                        if (cell.ColumnIndex == 0) throw new ArgumentException("Пункт отправки не введен");
                        if (cell.ColumnIndex == grid_add_path.ColumnCount - 1) throw new ArgumentException("Пункт доставки не введен");
                        empty_ports++;
                        cell.Value = "-";
                    }
                    keys.Add(cell.Value.ToString());

                }
                IEnumerable<string> keys1 = keys.Distinct();
                if (empty_ports == -1) empty_ports = 0;
                if (keys1.Count() != grid_add_path.Rows[0].Cells.Count - empty_ports) throw new ArgumentException($"Маршрут содержит одинаковые порты");
                panel3.Visible = false;
                string path = "";
                foreach (DataGridViewCell cell in grid_add_path.Rows[0].Cells)
                {
                    if (cell.Value != null) path += cell.Value.ToString() + " ";
                }
                if (row_index_paths == grid_paths.Rows.Count)
                {
                    Data.base_paths.Add(path);
                    grid_paths.Rows.Add();
                }
                else
                {
                    Data.base_paths.RemoveAt(row_index_paths);
                    Data.base_paths.Add(path);
                    if (Data.base_paths.Count >= 2 && row_index_paths != grid_paths.Rows.Count - 1)
                    {
                        string cons = Data.base_paths[row_index_paths];
                        Data.base_paths[row_index_paths] = Data.base_paths[Data.base_paths.Count - 1];
                        Data.base_paths[Data.base_paths.Count - 1] = cons;
                    }
                }
                while (grid_add_path.Columns.Count > grid_paths.Columns.Count)
                {
                    var oldcol = grid_paths.Columns[1];
                    var newcol = new DataGridViewTextBoxColumn();
                    newcol.HeaderText = oldcol.HeaderText;
                    newcol.Name = oldcol.Name;
                    newcol.SortMode = DataGridViewColumnSortMode.NotSortable;
                    newcol.DefaultCellStyle.DataSourceNullValue = '-';
                    newcol.DataPropertyName = oldcol.DataPropertyName;
                    grid_paths.Columns.Insert(grid_paths.ColumnCount - 2, newcol);
                }
                for (int i = 0; i < grid_paths.Columns.Count; i++)
                {
                    grid_paths.Rows[row_index_paths].Cells[i].Value = grid_add_path.Rows[0].Cells[i].Value;
                }
                add_path.Visible = true;
            }
            catch (ArgumentException error)
            {
                MessageBox.Show("Во время выполнения программы возникла ошибка:\n" + error.Message);

            }
        }
        private void grid_paths_CellClick(object sender, DataGridViewCellEventArgs e) // нажатие на ячейку таблицы маршрутов
        {
            if (e.RowIndex >= 0)
            {
                del_path.Visible = true;
                row_index_paths = e.RowIndex;
                grid_add_path.Rows.Clear();
                grid_add_path.Rows.Add();
                while (grid_add_path.Columns.Count < grid_paths.Columns.Count) Add_Col_Path();
                for (int i = 0; i < grid_add_path.Columns.Count; i++)
                {
                    grid_add_path.Rows[0].Cells[i].Value = grid_paths.Rows[row_index_paths].Cells[i].Value;
                }
                panel3.Visible = true;
            }
        }
        private void del_path_Click(object sender, EventArgs e) // удаление маршрута
        {
            panel3.Visible = false;
            del_path.Visible = false;
            if (row_index_paths != grid_paths.Rows.Count)
            {
                Data.base_paths.RemoveAt(row_index_paths);
                grid_paths.Rows.RemoveAt(row_index_paths);
            }

        }
        private void button11_Click(object sender, EventArgs e) // кнопка добавить маршрут
        {
            Add_Col_Path();
        }
        private void grid_add_path_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
