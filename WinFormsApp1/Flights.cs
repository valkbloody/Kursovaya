using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Кусовая;

namespace WinFormsApp1
{
    public partial class _Database : Form
    {
        List<Consignment> Availble_cons = new List<Consignment> { };
        List<Consignment> Curr_availble_cons = new List<Consignment> { };
        private void button10_Click(object sender, EventArgs e) // добавление рейса
        {
            grid_flight.Rows.Add();
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            comboBox3.Items.Clear();
            Availble_cons.Clear();
            listBox1.Items.Clear();
            label13.Text = "0";
            label34.Text = "0";
            button15.Visible = false;
            button16.Visible = false;
            comboBox1.Text = "";
            comboBox2.Text = "";
            panel1.Visible = true;
            button3.Visible = true;
            button13.Visible = false;

        }

        private void dataGridView6_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
        private void comboBox1_Click(object sender, EventArgs e) // заполение со списка судов
        {
            comboBox1.DataSource = null;
            comboBox1.DataSource = Data.base_vessels;
        }
        string WhereIsVessel(Vessel vess) // текущее местоположение судна
        {
            string place = vess._Port_postscripts;
            foreach (Flight flight in Data.base_flights)
            {
                if (flight.GetVessel._Number == vess._Number) place = flight.Path.Split(" ")[flight.Path.Split(" ").Count() - 2];
            }
            return place;
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e) // выбор судна
        {
            try
            {
                checkedListBox1.Items.Clear();
                Vessel myVessel = Data.base_vessels[comboBox1.SelectedIndex];
                label34.Text = myVessel._Lifting_capacity;
                List<string> availble_paths = new List<string> { };
                foreach (string path in Data.base_paths)
                {
                    if (WhereIsVessel(myVessel) == path.Split(" ")[0])
                    {
                        availble_paths.Add(path);
                    }
                }
                if (availble_paths.Count == 0)
                {
                    panel1.Visible = false;
                    grid_flight.Rows.RemoveAt(grid_flight.Rows.Count - 1);
                    
                    throw new Exception("Нет достпных маршрутов");
                }
                comboBox2.DataSource = availble_paths;
                comboBox2.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e) // выбор пути
        {
            Reset(1);
        }
        public int IsConsDelivered(Consignment cons) // проверка доставлен ли груз
        {
            foreach (Flight flight in Data.base_flights)
            {
                if (flight.Cons.Contains(cons)) return 2;
            }
            return 0;
        }
        void Reset(int need_check) // сброс рейса
        {
            try
            {
                checkedListBox1.Items.Clear();
                checkedListBox2.Items.Clear();
                comboBox3.Items.Clear();
                Availble_cons.Clear();
                listBox1.Items.Clear();
                label13.Text = "0";
                button13.Visible = true;
                button15.Visible = false;
                button16.Visible = false;
                string[] ports = comboBox2.SelectedItem.ToString().Split(" ");
                for (int i = 0; i < ports.Count() - 1; i++) comboBox3.Items.Add(ports[i]);
                foreach (Consignment cons in Data.base_cons)
                {
                    int key = 0;
                    for (int i = 0; i < ports.Count() - 2; i++)
                    {
                        if (cons._Place_dispatch == ports[i])
                        {
                            for (int j = i + 1; j < ports.Count() - 1; j++)
                            {
                                if (cons._Place_arrivel == ports[j])
                                {
                                    if (need_check == 1)
                                    {
                                        key = IsConsDelivered(cons);
                                    }
                                    if (key == 0)
                                    {
                                        Availble_cons.Add(cons);
                                        key = 1;
                                        listBox1.Items.Add(cons.ToStringInRaces());
                                        break;
                                    }
                                }
                            }
                            if (key == 1) break;
                        }
                    }
                }
                if (Availble_cons.Count == 0)
                {
                    panel1.Visible = false;
                    grid_flight.Rows.RemoveAt(grid_flight.Rows.Count - 1);
                    throw new Exception("Нет подходящего груза");
                }
                Curr_availble_cons.Clear();
                if (need_check == 1)
                {
                    foreach (Vessel vess in Data.base_vessels)
                    {
                        if (vess.ToString().Split(" ")[0] == comboBox1.SelectedItem.ToString().Split(" ")[0])
                        {
                            Flight flight = new Flight(vess, comboBox2.SelectedItem.ToString());
                            Data.base_flights.Add(flight);
                            break;
                        }
                    }
                }
                comboBox3.SelectedIndex = 0;
                comboBox3.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void curr_weight(int index, int combobox, int key) // текущий вес партии
        {
            try
            {
                int weight = Convert.ToInt32(label13.Text);
                if (combobox == 1)
                {
                    foreach (Consignment consigmnent in Availble_cons)
                        if (checkedListBox1.Items[index].ToString().Split(" ")[2] == consigmnent._Сons_number)
                        {
                            if (key == 1) weight += consigmnent.GetWieght();
                        }
                }
                else
                {
                    foreach (Consignment consigmnent in Curr_availble_cons)
                        if (checkedListBox2.Items[index].ToString().Split(" ")[2] == consigmnent._Сons_number)
                        {
                            if (key == 1) weight -= consigmnent.GetWieght();
                        }
                }

                if (weight > Convert.ToInt32(Data.base_flights[Data.base_flights.Count - 1].GetVessel._Lifting_capacity))
                {
                    checkedListBox1.Items.Clear();
                    if (comboBox3.SelectedIndex == 0)
                    {
                        weight = 0;
                        label13.Text = Convert.ToString(weight);
                    }
                    foreach (Consignment cons in Availble_cons)
                    {
                        if (cons._Place_dispatch == comboBox3.SelectedItem.ToString()) checkedListBox1.Items.Add(cons.ToStringInRaces());
                    }
                    throw new Exception("Вес грузов больше грузоподъмности корабля");
                }
                if (weight < 0) weight = 0;
                label13.Text = Convert.ToString(weight);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Во время выполнения программы возникла ошибка:\n" + ex.Message);
            }
        }
        private void button13_Click(object sender, EventArgs e) // подтверждение загрузки и выгрузки грузов в порту
        {
            List<Consignment> consigments = new List<Consignment> { };
            int pos = 0;
            while (pos < checkedListBox2.Items.Count)
            {
                if (checkedListBox2.GetItemChecked(pos))
                {
                    curr_weight(pos, 2, 1);
                    foreach (Consignment consigmnent in Availble_cons)
                        if (checkedListBox2.Items[pos].ToString().Split(" ")[2] == consigmnent._Сons_number.ToString())
                        {
                            Curr_availble_cons.Remove(consigmnent);
                            break;
                        }
                    checkedListBox2.Items.RemoveAt(pos);
                    pos--;
                }
                pos++;
            }
            pos = 0;
            CheckedListBox check_box = checkedListBox1;
            while (pos < checkedListBox1.Items.Count)
            {
                if (checkedListBox1.GetItemChecked(pos))
                {
                    curr_weight(pos, 1, 1);
                }
                pos++;
            }
            pos = 0;
            while (pos < checkedListBox1.Items.Count)
            {
                if (checkedListBox1.GetItemChecked(pos))
                {
                    foreach (Consignment consigmnent in Availble_cons)
                        if (checkedListBox1.Items[pos].ToString().Split(" ")[2] == consigmnent._Сons_number.ToString())
                        {
                            Curr_availble_cons.Add(consigmnent);
                            consigments.Add(consigmnent);
                            break;
                        }
                    checkedListBox1.Items.RemoveAt(pos);
                    pos--;
                }
                pos++;
            }
            Data.base_flights[Data.base_flights.Count - 1].Add_Cons(consigments);
            comboBox3.Enabled = true;
            if (comboBox3.SelectedIndex == comboBox3.Items.Count - 1)
            {
                button15.Visible = true;
                button13.Visible = false;

            }
            do
            {
                if (comboBox3.SelectedIndex < comboBox3.Items.Count - 1)
                    comboBox3.SelectedIndex += 1;
            }
            while (checkedListBox1.Items.Count == 0 && checkedListBox2.Items.Count == 0 && comboBox3.SelectedIndex < comboBox3.Items.Count - 1);
            comboBox3.Enabled = false;



        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e) // смена порта
        {
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            if (comboBox3.SelectedIndex == 0)
            {
                foreach (Consignment cons in Availble_cons)
                {
                    if (cons._Place_dispatch == comboBox3.SelectedItem.ToString())
                        checkedListBox1.Items.Add(cons.ToStringInRaces());
                }
            }
            else
            {
                if (comboBox3.SelectedItem.ToString() == "-") comboBox3.SelectedIndex++;
                else
                {
                    button16.Visible = true;
                    foreach (Consignment cons in Availble_cons)
                    {
                        if (cons._Place_dispatch == comboBox3.SelectedItem.ToString())
                            checkedListBox1.Items.Add(cons.ToStringInRaces());
                    }
                    foreach (Consignment cons in Curr_availble_cons)
                    {
                        if (cons._Place_arrivel == comboBox3.SelectedItem.ToString())
                        {
                            checkedListBox2.Items.Add(cons.ToStringInRaces());
                            if (checkedListBox2.Items.Count > 0)
                            {
                                checkedListBox2.SetItemChecked(checkedListBox2.Items.Count - 1, true);
                                checkedListBox2.SelectedIndex = checkedListBox2.Items.Count - 1;

                            }
                        }
                    }
                }
            }
        }
        private void button15_Click(object sender, EventArgs e) // кнопка подтвердить рейс
        {
            try
            {
                if (Data.base_flights[Data.base_flights.Count - 1].Cons.Count == 0) throw new Exception("В рейсе не участвует ни один груз");
                panel1.Visible = false;
                grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[0].Value = Data.base_flights[Data.base_flights.Count - 1].GetVessel.ToString();
                string[] ports = Data.base_flights[Data.base_flights.Count - 1].ToPortsPath();
                grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[1].Value = ports[1];
                for (int i = 2; i < ports.Count() - 2; i++)
                {
                    if (grid_flight.ColumnCount < ports.Count() - 1)
                    {
                        var newcol = new DataGridViewTextBoxColumn();
                        newcol.HeaderText = "Промежуточный пункт";
                        newcol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        grid_flight.Columns.Insert(grid_flight.ColumnCount - 1, newcol);
                    }
                    grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[i].Value = ports[i];

                }
                grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[grid_flight.Columns.Count - 1].Value = Data.base_flights[Data.base_flights.Count - 1].ToStringPort(Data.base_flights[Data.base_flights.Count - 1].Path.Split(" ")[Data.base_flights[Data.base_flights.Count - 1].Path.Split(" ").Count() - 2]);
                Reset(0);
                button5.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e) // кнопка отмена
        {
            button3.Visible = false;
            panel1.Visible = false;
            if (grid_flight.Rows.Count > 0)
            grid_flight.Rows.RemoveAt(grid_flight.Rows.Count - 1);
            if (Data.base_flights.Count > 0)
            Data.base_flights.RemoveAt(Data.base_flights.Count - 1);
        }

        private void button5_Click_1(object sender, EventArgs e) // кнопка удалить рейсы
        {
            Data.base_flights.Clear();
            grid_flight.Rows.Clear();
            button5.Visible = false;
        }
        private void button16_Click(object sender, EventArgs e) // кнопка сброса рейса
        {
            Reset(0);
        }

    }
}
