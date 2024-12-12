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
        int row_index_cons = 0;
        //Партии груза
        private void button7_Click(object sender, EventArgs e) // добавление партии товара
        {
            groupBox3.Visible = true;
            button7.Visible = false;
            row_index_cons = grid_cons.Rows.Count;
            textBox5.Text = "";
            textBox6.Text = "";
            cons_port_disp.Text = "";
            cons_port_arriv.Text = "";
            cons_arrivel.Text = "";
            cons_disp.Text = "";
            button7.Visible = true;
            cons_load_grid.Rows.Clear();
            cons_load_grid.Rows.Add();
        }
        private void textBox6_TextChanged(object sender, EventArgs e)  
            // при изменении номера декларации он дописывается в номер партии
        {
            textBox5.Enabled = true;
            label14.Text = $"/{textBox6.Text}";
        }
        private void tabPage3_Enter(object sender, EventArgs e) // загрузка клиентов
        {
            cons_arrivel.Items.Clear();
            cons_disp.Items.Clear();
            foreach (Client client in Data.base_clients)
            {
                cons_disp.Items.Add(client.ToString_Cons());
                cons_arrivel.Items.Add(client.ToString_Cons());
            }

        }
        private void button8_Click(object sender, EventArgs e) // сохранение партии груза
        {
            try
            {
                List<Load> a1 = new List<Load> { };
                for (int i = 0; i < cons_load_grid.Rows.Count; i++)
                {
                    if (cons_load_grid.Rows[i].Cells[0].Value == null) cons_load_grid.Rows[i].Cells[0].Value = "";
                    if (cons_load_grid.Rows[i].Cells[1].Value == null) cons_load_grid.Rows[i].Cells[1].Value = "";
                    if (cons_load_grid.Rows[i].Cells[2].Value == null) cons_load_grid.Rows[i].Cells[2].Value = "";
                    if (cons_load_grid.Rows[i].Cells[4].Value == null) cons_load_grid.Rows[i].Cells[4].Value = "";
                    if (cons_load_grid.Rows[i].Cells[3].Value == null) cons_load_grid.Rows[i].Cells[3].Value = __measurement.undefined;
                    Load b = new Load(cons_load_grid.Rows[i].Cells[0].Value.ToString(), cons_load_grid.Rows[i].Cells[1].Value.ToString(), cons_load_grid.Rows[i].Cells[2].Value.ToString(), cons_load_grid.Rows[i].Cells[3].Value.ToString(), cons_load_grid.Rows[i].Cells[4].Value.ToString());
                    a1.Add(b);
                }
                if (cons_disp.SelectedIndex == -1) cons_disp.SelectedIndex = 0;
                if (cons_arrivel.SelectedIndex == -1) cons_arrivel.SelectedIndex = 0;
                if (cons_port_disp.SelectedIndex == -1) cons_port_disp.SelectedIndex = 0;
                if (cons_port_arriv.SelectedIndex == -1) cons_port_arriv.SelectedIndex = 0;
                Consignment consignment = new Consignment(textBox5.Text + label14.Text, textBox6.Text, Data.base_clients[cons_disp.SelectedIndex], Data.base_clients[cons_arrivel.SelectedIndex], cons_port_disp.SelectedItem.ToString(), cons_port_arriv.SelectedItem.ToString(), dateTimePicker1.Value, dateTimePicker2.Value, a1); ;
                int count = 0;
                foreach (Consignment cons in Data.base_cons)
                    if (cons._Сons_number == consignment._Сons_number) count++;
                if ((count > 0 && row_index_cons == grid_cons.Rows.Count) || (count > 1 && row_index_cons < grid_cons.Rows.Count)) throw new ArgumentException("Номер партии должен быть уникальным");
                if (row_index_cons == grid_cons.Rows.Count)
                {
                    Data.base_cons.Add(consignment);
                    grid_cons.Rows.Add();
                }
                else
                {
                    Data.base_cons.RemoveAt(row_index_cons);
                    Data.base_cons.Add(consignment);
                    if (Data.base_cons.Count >= 2 && row_index_cons != grid_cons.Rows.Count - 1)
                    {
                        Consignment cons = Data.base_cons[row_index_cons];
                        Data.base_cons[row_index_cons] = Data.base_cons[Data.base_cons.Count - 1];
                        Data.base_cons[Data.base_cons.Count - 1] = cons;
                    }
                }
                grid_cons.Rows[row_index_cons].Cells[0].Value = textBox5.Text + label14.Text;
                grid_cons.Rows[row_index_cons].Cells[1].Value = textBox6.Text;
                grid_cons.Rows[row_index_cons].Cells[6].Value = cons_port_disp.SelectedItem.ToString();
                grid_cons.Rows[row_index_cons].Cells[7].Value = cons_port_arriv.SelectedItem.ToString();
                grid_cons.Rows[row_index_cons].Cells[4].Value = dateTimePicker1.Value.ToShortDateString();
                grid_cons.Rows[row_index_cons].Cells[5].Value = dateTimePicker2.Value.ToShortDateString();
                grid_cons.Rows[row_index_cons].Cells[2].Value = cons_disp.SelectedItem;
                grid_cons.Rows[row_index_cons].Cells[3].Value = cons_arrivel.SelectedItem;
                grid_cons.Rows[row_index_cons].Cells[8].Value = consignment.ToStringLoads();
                groupBox3.Visible = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Во время выполнения программы возникла ошибка:\n" + ex.Message);
            }
        }
        private void grid_cons_CellClick(object sender, DataGridViewCellEventArgs e) //нажатие на ячейку таблицы партии грузов
        {
            if (e.RowIndex >= 0)
            {
                row_index_cons = e.RowIndex;
                groupBox3.Visible = true;
                button1.Visible = true;
                textBox5.Text = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[0].Value.ToString().Split("/")[0]);
                textBox6.Text = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[1].Value);
                cons_port_disp.SelectedItem = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[6].Value);
                cons_port_arriv.SelectedItem = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[7].Value);
                cons_disp.SelectedItem = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[2].Value);
                cons_arrivel.SelectedItem = Convert.ToString(grid_cons.Rows[row_index_cons].Cells[3].Value);
                dateTimePicker1.Value = Convert.ToDateTime(grid_cons.Rows[row_index_cons].Cells[4].Value);
                dateTimePicker2.Value = Convert.ToDateTime(grid_cons.Rows[row_index_cons].Cells[5].Value);
                cons_load_grid.Rows.Clear();
                foreach (Consignment cons in Data.base_cons)
                {
                    if (cons._Сons_number == grid_cons.Rows[row_index_cons].Cells[0].Value.ToString())
                    {
                        for (int i = 0; i < cons._Content.Count; i++)
                        {
                            cons_load_grid.Rows.Add(cons._Content[i]._Number, cons._Content[i]._Name, cons._Content[i]._Declared_weight, cons._Content[i]._Measurement.ToString(), Data.base_cons[row_index_cons]._Content[i]._Insured_weight);
                        }
                        break;
                    }
                }
            }
        }
        private void button9_Click(object sender, EventArgs e) // кнопка добавления вида товара
        {
            cons_load_grid.Rows.Add();
        }
        private void button1_Click(object sender, EventArgs e) // удаление партии
        {
            groupBox3.Visible = false;
            button1.Visible = false;
            if (row_index_cons != grid_cons.Rows.Count)
            {
                Data.base_cons.RemoveAt(row_index_cons);
                grid_cons.Rows.RemoveAt(row_index_cons);
            }
        }
        private void dataGridView4_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

    }
}
