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
        int row_index_clients;
        private void button1_Click_1(object sender, EventArgs e) // добавление клиента
        {
            add_client.Left = grid_clients.Left + grid_clients.Width;
            client_info.Left = grid_clients.Left + grid_clients.Width;
            panel2.Left = add_client.Left - panel2.Width;
            client_info.Text = "Добавление клиента";
            textBox1.Text = "";
            textBox2.Text = "0000000000";
            textBox3.Text = "";
            textBox4.Text = "";
            client_info.Visible = true;
            add_client.Visible = false;
            for (int i = 0; i < grid_clients.RowCount; i++)
            {
                grid_clients.Rows[i].Selected = false;
            }
            row_index_clients = grid_clients.RowCount;
            vip_client.Checked = false;
            grid_clients.Enabled = false;
            del_client.Visible = false;
        }
        private void grid_clients_CellClick(object sender, DataGridViewCellEventArgs e) // нажатие на ячейку таблицы клиентов
        {
            if (e.RowIndex >= 0)
            {
                row_index_clients = e.RowIndex;
                client_info.Text = "Информация о клиенте";
                client_info.Visible = true;
                textBox1.Text = grid_clients.Rows[row_index_clients].Cells[0].Value.ToString();
                textBox2.Text = grid_clients.Rows[row_index_clients].Cells[1].Value.ToString();
                textBox3.Text = grid_clients.Rows[row_index_clients].Cells[2].Value.ToString();
                textBox4.Text = grid_clients.Rows[row_index_clients].Cells[3].Value.ToString();
                vip_client.Checked = false;
                if (grid_clients.Rows[row_index_clients].Cells[4].Value != null)
                {
                    vip_client.Checked = true;
                    comboBox6.SelectedIndex = Convert.ToInt32(grid_clients.Rows[row_index_clients].Cells[4].Value);
                    textBox7.Text = grid_clients.Rows[row_index_clients].Cells[5].Value.ToString();
                    textBox8.Text = grid_clients.Rows[row_index_clients].Cells[6].Value.ToString();
                }

                add_client.Visible = false;
                del_client.Visible = true;
            }
        }
        private void button2_Click_1(object sender, EventArgs e) // сохранение клиента
        {
            try
            {
                Client client = new Client(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);
                if (vip_client.Checked)
                {
                    grid_clients.Columns[4].Visible = true;
                    grid_clients.Columns[5].Visible = true;
                    grid_clients.Columns[6].Visible = true;
                    VIP_CLIENT vip_client = new VIP_CLIENT(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, (_state_subs)comboBox6.SelectedIndex, textBox7.Text, textBox8.Text);
                    if (row_index_clients == grid_clients.Rows.Count)
                    {
                        Data.base_clients.Add(vip_client);
                    }
                    else
                    {
                        Data.base_clients.RemoveAt(row_index_clients);
                        Data.base_clients.Add(vip_client);
                        if (Data.base_clients.Count >= 2 && row_index_clients != grid_clients.Rows.Count - 1)
                        {
                            Client clien = Data.base_clients[row_index_clients];
                            Data.base_clients[row_index_clients] = Data.base_clients[Data.base_clients.Count - 1];
                            Data.base_clients[Data.base_clients.Count - 1] = clien;
                        }
                    }
                    if (row_index_clients == grid_clients.Rows.Count)
                    {
                        grid_clients.Rows.Add();
                        grid_clients.ReadOnly = false;
                    }
                    grid_clients.Rows[row_index_clients].Cells[4].Value = (_state_subs)vip_client.State;
                    grid_clients.Rows[row_index_clients].Cells[5].Value = vip_client.Price_subs;
                    grid_clients.Rows[row_index_clients].Cells[6].Value = vip_client.Amount_points;
                }
                else
                {
                    if (row_index_clients == grid_clients.Rows.Count) Data.base_clients.Add(client);
                    else
                    {
                        Data.base_clients.RemoveAt(row_index_clients);
                        Data.base_clients.Add(client);
                        if (Data.base_clients.Count >= 2 && row_index_clients != grid_clients.Rows.Count - 1)
                        {
                            Client clien = Data.base_clients[row_index_clients];
                            Data.base_clients[row_index_clients] = Data.base_clients[Data.base_clients.Count - 1];
                            Data.base_clients[Data.base_clients.Count - 1] = clien;
                        }
                    }
                    if (row_index_clients == grid_clients.Rows.Count)
                    {
                        grid_clients.Rows.Add();
                        grid_clients.ReadOnly = false;
                    }
                    if (grid_clients.Rows[row_index_clients].Cells[4].Value != null)
                    {
                        grid_clients.Rows[row_index_clients].Cells[4].Value = null;
                        grid_clients.Rows[row_index_clients].Cells[5].Value = null;
                        grid_clients.Rows[row_index_clients].Cells[6].Value = null;
                    }
                }
                grid_clients.Rows[row_index_clients].Cells[0].Value = client._Name_client;
                grid_clients.Rows[row_index_clients].Cells[1].Value = client._INN_;
                grid_clients.Rows[row_index_clients].Cells[2].Value = client._Bank;
                grid_clients.Rows[row_index_clients].Cells[3].Value = client._Address;
                add_client.Visible = true;
                grid_clients.Enabled = true;
                client_info.Visible = false;
                panel2.Visible = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Во время выполнения программы возникла ошибка:\n" + ex.Message);
            }
        }
        private void button14_Click(object sender, EventArgs e) // кнопка расчёта стоимости подписки
        {
            try
            {
                VIP_CLIENT client = new VIP_CLIENT(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, (_state_subs)comboBox6.SelectedIndex, textBox7.Text, textBox8.Text);
                textBox9.Text = Convert.ToString(client.Payment_On_Next_Month()) + " р.";
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) // вип клиент
        {
            if (vip_client.Checked == true)
            {
                panel2.Visible = true;
                comboBox6.SelectedIndex = 0;
            }
            else
            {
                panel2.Visible = false;
                textBox7.Text = "0";
                textBox8.Text = "0";
                comboBox6.SelectedIndex = 0;
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.Text = $"ИНН клиента \n({textBox2.Text.Length} из 10 символов)";
        }
        private void button4_Click(object sender, EventArgs e) // кнопка закрытия
        {
            grid_clients.Enabled = true;
            client_info.Visible = false;
            panel2.Visible = false;
            add_client.Visible = true;
        }
        private void button6_Click(object sender, EventArgs e) // кнопка удаления клиента
        {
            grid_clients.Enabled = true;
            client_info.Visible = false;
            panel2.Visible = false;
            add_client.Visible = true;
            if (row_index_clients != grid_clients.Rows.Count)
            {
                Data.base_clients.RemoveAt(row_index_clients);
                grid_clients.Rows.RemoveAt(row_index_clients);
            }
        }
    }
}
