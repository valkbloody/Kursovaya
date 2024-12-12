using System.Data;
using System.Drawing.Drawing2D;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Кусовая;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace WinFormsApp1
{
    public partial class _Database : Form
    {
        public _Database()
        {

            InitializeComponent();
            //Чтение даннах из файла и загрузки их в таблицы
            //Клиенты
            add_client.Left = grid_clients.Left + grid_clients.Width;
            add_client.Width = tabControl1.Width - add_client.Left;
            client_info.Width = tabControl1.Width - add_client.Left;
            File file = new File();
            file.Read();
            foreach (Client client in Data.base_clients)
            {
                grid_clients.Rows.Add(client._Name_client, client._INN_, client._Bank, client._Address);
            }
            foreach (VIP_CLIENT client in Data.VIP_base_clients)
            {
                grid_clients.Columns[4].Visible = true;
                grid_clients.Columns[5].Visible = true;
                grid_clients.Columns[6].Visible = true;
                grid_clients.Rows.Add(client._Name_client, client._INN_, client._Bank, client._Address, client.State, client.Price_subs, client.Amount_points);
            }
            Data.base_clients.AddRange(Data.VIP_base_clients);
            //Маршруты
            foreach (string path in Data.base_paths)
            {
                string[] ports = path.Split(" ");
                grid_paths.Rows.Add();
                grid_paths.Rows[grid_paths.Rows.Count - 1].Cells[0].Value = ports[0];
                grid_paths.Rows[grid_paths.Rows.Count - 1].Cells[1].Value = ports[1];
                for (int i = 2; i < ports.Count() - 2; i++)
                {
                    if (grid_paths.ColumnCount < Data.base_ports.Count())
                    {
                        var oldcol = grid_paths.Columns[1];
                        var newcol = new DataGridViewTextBoxColumn();
                        newcol.SortMode = DataGridViewColumnSortMode.NotSortable;
                        newcol.HeaderText = oldcol.HeaderText;
                        newcol.Name = oldcol.Name;
                        newcol.DataPropertyName = oldcol.DataPropertyName;
                        grid_paths.Columns.Insert(i, newcol);
                    }
                    grid_paths.Rows[grid_paths.Rows.Count - 1].Cells[i].Value = ports[i];
                }
                grid_paths.Rows[grid_paths.Rows.Count - 1].Cells[grid_paths.ColumnCount - 1].Value = ports[ports.Count() - 2];
            }
            //Партии товаров
            foreach (Consignment cons in Data.base_cons)
            {
                grid_cons.Rows.Add(cons._Сons_number, cons._Declaration_number, cons.Reciever.ToString_Cons(), cons.Sender.ToString_Cons(), cons._Dispatch_date.ToShortDateString(), cons._Arrivel_date.ToShortDateString(), cons._Place_dispatch, cons._Place_arrivel, cons.ToStringLoads());
            }
            //Судна
            foreach (Vessel vess in Data.base_vessels)
            {
                grid_vessels.Rows.Add(vess._Number, vess._FN_Capitan, vess._Type, vess._Lifting_capacity, vess._Year_building, vess._Get_Photo, vess._Port_postscripts);
            }
            //Рейсы
            foreach (Flight flight in Data.base_flights)
            {
                grid_flight.Rows.Add();
                grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[0].Value = flight.GetVessel.ToString();
                string[] ports = flight.ToPortsPath();
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
                grid_flight.Rows[grid_flight.Rows.Count - 1].Cells[grid_flight.Columns.Count - 1].Value = flight.ToStringPort(flight.Path.Split(" ")[flight.Path.Split(" ").Count() - 2]);
            }
            if (Data.base_flights.Count > 0) button5.Visible = true;
        }
        private void _Database_FormClosed(object sender, FormClosedEventArgs e) // сохранение в файл
        {
            File file = new File();
            file.Record();
        }
    }
}