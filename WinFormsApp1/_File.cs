using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Кусовая;
using static System.Net.Mime.MediaTypeNames;
namespace WinFormsApp1
{
    public class File
    {
        string path = "note2.txt";
        public void Record() // запись в файл
        {
            FileStream fstream = null;
            try
            {

                using StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8);
                writer.WriteLine("/Clients/");
                for (int i = 0; i < Data.base_clients.Count(); i++) // запись клиентов
                {
                    writer.WriteLine(Data.base_clients[i].ToString('#'));
                }
                writer.WriteLine("/Roots/");
                for (int i = 0; i < Data.base_paths.Count(); i++) // запись маршрутов
                {
                    writer.WriteLine(Data.base_paths[i].ToString());
                }
                writer.WriteLine("/Consigments/");
                for (int i = 0; i < Data.base_cons.Count(); i++)
                {
                    writer.WriteLine(Data.base_cons[i].ToString('#')); //запись товаров
                }
                writer.WriteLine("/Vessels/");
                for (int i = 0; i < Data.base_vessels.Count(); i++)
                { 
                    writer.WriteLine(Data.base_vessels[i].ToString('#')); // запись суден
                }
                writer.WriteLine("/Flights/");
                for (int i = 0; i < Data.base_flights.Count(); i++) // запись рейсов
                {
                    writer.WriteLine(Data.base_flights[i].ToString('#'));
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                fstream?.Close();
            }
        }
        public void Read() // чтение из файла
        {
            StreamReader? fstream = null;
            try
            {

                fstream = new StreamReader(path, Encoding.UTF8);
                fstream.BaseStream.Position = 0;
                int i = 0;
                int key = 0;
                int number = 0;
                while (true)
                {
                    var str = fstream.ReadLine();
                    if (str == null || str == "" || str == " ") //достигнут конец файла
                        break;
                    if (str == "/Clients/")
                    {
                        key = 1;
                        continue;
                    }
                    if (str == "/Roots/")
                    {
                        key = 2;
                        continue;
                    }
                    if (str == "/Consigments/")
                    {
                        key = 3;
                        continue;
                    }
                    if (str == "/Vessels/")
                    {
                        key = 4;
                        continue;
                    }
                    if (str == "/Flights/")
                    {
                        key = 5;
                        continue;
                    }
                    if (key == 1) // чтение клиентов
                    {
                        string[] s = str.Split("#");
                        if (s.Count() > 4)
                        {
                            VIP_CLIENT vip_client = new VIP_CLIENT(s[0], s[1], s[2], s[3], (_state_subs)Enum.Parse(typeof(_state_subs), s[4]), s[5], s[6]);
                            Data.VIP_base_clients.Add(vip_client);
                        }
                        else
                        {
                            Client client = new Client(s[0], s[1], s[2], s[3]);
                            Data.base_clients.Add(client);
                        }
                    }
                    if (key == 2) // чтение маршрутов
                    {
                        Data.base_paths.Add(str);
                    }
                    if (key == 3) // чтение партий товаров
                    {
                        if (str == "/Loads/")
                        {
                            key = 3;
                            continue;
                        }
                        List<Load> load = new List<Load> { };
                        while (str != "/Consigment/")
                        {
                            string[] s = str.Split("#");
                            Load l = new Load(s[0], s[1], s[2], s[3], s[4]);
                            load.Add(l);
                            str = fstream.ReadLine();
                        }
                        if (str == "/Consigment/")
                        {
                            str = fstream.ReadLine();
                            string[] s = str.Split("#");
                            Consignment con = new Consignment(s[0], s[1], s[2], s[3], s[6], s[7], Convert.ToDateTime(s[4]), Convert.ToDateTime(s[5]), load);
                            Data.base_cons.Add(con);
                        }
                    }
                    if (key == 4) // чтение судов
                    {
                        string[] s = str.Split("#");
                        Vessel vess = new Vessel(s[0], s[1], (_type_vessel)Enum.Parse(typeof(_type_vessel), s[2]), s[3], s[4], s[5], s[6]);
                        Data.base_vessels.Add(vess);
                    }
                    if (key == 5) // чтение рейсов
                    {
                        if (str == "/Flight/") str = fstream.ReadLine();
                        string[] s = str.Split("#");
                        Vessel vess = new Vessel(s[0], s[1], (_type_vessel)Enum.Parse(typeof(_type_vessel), s[2]), s[3], s[4], s[5], s[6]);
                        str = fstream.ReadLine();
                        Flight flight = new Flight(vess, str);
                        number = 2;
                        str = fstream.ReadLine();
                        List<Consignment> cons = new List<Consignment> { };
                        while (str != "/Flight/" || str != null || str != "" || str != " ")
                        {
                            if (str == "/Flight/" || str == null || str == "" || str == " ") break;
                            str = fstream.ReadLine();
                            List<Load> load = new List<Load> { };
                            while (str != "/Consigment/")
                            {
                                string[] loa = str.Split("#");
                                Load l = new Load(loa[0], loa[1],loa[2], loa[3], loa[4]);
                                load.Add(l);
                                str = fstream.ReadLine();
                            }
                            if (str == "/Consigment/")
                            {
                                str = fstream.ReadLine();
                                string[] s1 = str.Split("#");
                                Consignment con = new Consignment(s1[0], s1[1], s1[2], s1[3], s1[6], s1[7], Convert.ToDateTime(s1[4]), Convert.ToDateTime(s1[5]), load);
                                cons.Add(con);
                                str = fstream.ReadLine();
                            }
                        }
                        flight.Add_Cons(cons);
                        Data.base_flights.Add(flight);
                    }
                }

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                fstream?.Close();
            }
        }
    }
}
