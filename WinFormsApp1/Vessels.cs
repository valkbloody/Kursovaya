using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Кусовая;

namespace WinFormsApp1
{
    public partial class _Database : Form
    {
        //Судна
        string filename = "";
        int row_index_vess = 0;
        private void button12_Click(object sender, EventArgs e) // добавление судна
        {
            groupBox2.Visible = true;
            button12.Visible = false;
            textBox10.Text = "";
            textBox11.Text = "";
            textBox12.Text = "";
            textBox13.Text = "";
            comboBox4.SelectedIndex = 0;
            pictureBox1.Image = null;
            comboBox5.SelectedIndex = 0;
            row_index_vess = grid_vessels.Rows.Count;
            filename = "";
            del_vess.Visible=true;

        }
        private void Save_Vessels_Click(object sender, EventArgs e) // сохранить судно
        {
            try
            {
                Vessel vess = new Vessel(textBox10.Text, textBox11.Text, (_type_vessel)comboBox4.SelectedIndex, textBox12.Text, textBox13.Text, filename, comboBox5.SelectedItem.ToString());
                int count = 0;
                foreach (Vessel vessel in Data.base_vessels)
                    if (vessel._Number == vess._Number) count++;
                if ((count > 0 && row_index_vess == grid_vessels.Rows.Count) || (count > 1 && row_index_vess < grid_vessels.Rows.Count)) throw new ArgumentException("Номер судна должен быть уникальным");
                if (row_index_vess == grid_vessels.Rows.Count)
                {
                    Data.base_vessels.Add(vess);
                    grid_vessels.Rows.Add();
                }
                else
                {
                    Data.base_vessels.RemoveAt(row_index_vess);
                    Data.base_vessels.Add(vess);
                    if (Data.base_vessels.Count >= 2 && row_index_vess != grid_vessels.Rows.Count - 1)
                    {
                        Vessel vessel = Data.base_vessels[row_index_vess];
                        Data.base_vessels[row_index_vess] = Data.base_vessels[Data.base_vessels.Count - 1];
                        Data.base_vessels[Data.base_vessels.Count - 1] = vessel;
                    }
                }
                grid_vessels.Rows[row_index_vess].Cells[0].Value = vess._Number;
                grid_vessels.Rows[row_index_vess].Cells[1].Value = vess._FN_Capitan;
                grid_vessels.Rows[row_index_vess].Cells[2].Value = vess._Type;
                grid_vessels.Rows[row_index_vess].Cells[3].Value = vess._Lifting_capacity;
                grid_vessels.Rows[row_index_vess].Cells[4].Value = vess._Year_building;
                grid_vessels.Rows[row_index_vess].Cells[5].Value = vess._Get_Photo;
                grid_vessels.Rows[row_index_vess].Cells[6].Value = vess._Port_postscripts;
                groupBox2.Visible = false;
                button12.Visible = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void grid_vessels_CellClick(object sender, DataGridViewCellEventArgs e) // нажатие на ячейку таблицы судов
        {
            if (e.RowIndex >= 0)
            {
                row_index_vess = e.RowIndex;
                groupBox2.Visible = true;
                del_vess.Visible = true;
                textBox10.Text = grid_vessels.Rows[row_index_vess].Cells[0].Value.ToString();
                textBox11.Text = grid_vessels.Rows[row_index_vess].Cells[1].Value.ToString();
                textBox12.Text = grid_vessels.Rows[row_index_vess].Cells[3].Value.ToString();
                textBox13.Text = grid_vessels.Rows[row_index_vess].Cells[4].Value.ToString();
                comboBox4.SelectedItem = grid_vessels.Rows[row_index_vess].Cells[2].Value.ToString();
                foreach (Vessel vess in Data.base_vessels)
                    if (vess._Number == textBox10.Text)
                    {
                        filename = vess.Photo;
                        pictureBox1.Image = Data.base_vessels[row_index_vess]._Get_Photo;
                        break;
                    }
                comboBox5.SelectedItem = grid_vessels.Rows[row_index_vess].Cells[6].Value.ToString();
            }
        }
        private void button18_Click(object sender, EventArgs e) // выбор картинки
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialog1.FileName);
            }
        }
        private void del_vess_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            del_vess.Visible = false;
            if (row_index_vess != grid_vessels.Rows.Count)
            {
                Data.base_vessels.RemoveAt(row_index_vess);
                grid_vessels.Rows.RemoveAt(row_index_vess);
            }
            button12.Visible = true;
        }
        private void dataGridView5_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
