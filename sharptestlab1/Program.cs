using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace sharptestlab1
{
    [Serializable]
    class Program
    {
        private String region;
        private String tkind;
        private long quant;

        static List<Program> list;
        static bool newList;
        private Program() { }

        private void data(String r, String t, long q)
        {
            this.region = r;
            this.tkind = t;
            this.quant = q;
        }

        private Program(String r,String t,long q) {
            this.region = r;
            this.tkind = t;
            this.quant = q;
        }
        public string show()
        {
            return this.region+" "+this.tkind+" "+this.quant;
        }

        public static void Serialize()
        {
            FileStream fstr = new FileStream("d:\\files\\ris\\file.txt", FileMode.Create, FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            if (list.Count > 0)
            {
                int i = 0;
                while (i<list.Count)
                {
                    bf.Serialize(fstr, list.ElementAt(i));
                    i++;
                }
            }
            fstr.Close();
        }
        public static List<Program> Deserealize()
        {
            FileStream  fstr = new FileStream("d:\\files\\ris\\file.txt", FileMode.OpenOrCreate, FileAccess.Read);
            list.Clear();
            StreamReader reader = new StreamReader(fstr); // создаем «потоковый читатель» и связываем его с файловым потоком 
            reader.ReadToEnd();
            if (fstr.Position == 0)
            {
                Console.Write("файл пуст\n");
            }
            else
            {
                long length = fstr.Position;
                fstr.Seek(0, SeekOrigin.Begin);
                BinaryFormatter bf = new BinaryFormatter();
                while (fstr.Position < length)
                {
                    Program pr = (Program)bf.Deserialize(fstr);
                    list.Add(pr);
                }
            }
            reader.Close();
            fstr.Close();
            newList = false;
            return list;
        }
        static void Main(string[] args)
        {
            String choice = " ";
            newList = true;
            //открытие файла с данными на запись
            list = new List<Program>();
            while (choice != "7")
            {
                if (newList == true)
                {
                    list = Deserealize();
                }
                FileStream fstr = new FileStream("d:\\files\\ris\\file.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
               
                Console.WriteLine("Vyberite punkt:\n 1 - Vvesti novuju zapis\n 2 - Posmotret' posadki derevjev v rajonax\n 3 - Posmotret konkretnij rajon\n"
                    + " 4 - Udalit zapis\n 5 - Redaktirovat' zapis\n 6 - Sortirovat' po rajonu\n 7 - Exit");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        {
                            Console.WriteLine("Vvedite rajon, porodu dereva i kolichestvo:");
                            String region = Console.ReadLine();
                            String tkind = Console.ReadLine();
                            long quant = System.Int64.Parse(Console.ReadLine());
                            Program obj = new Program(region, tkind, quant);
                            fstr.Seek(0, SeekOrigin.End);

                            BinaryFormatter bf = new BinaryFormatter();
                            bf.Serialize(fstr, obj);
                            list.Add(obj);
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("Список:");
                            int i = 0;
                            foreach (var ob in list)
                            {
                                i++;
                                Console.WriteLine(i+") "+ob.show());
                            }
                            Console.ReadLine();
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Введите название раойна для поиска");
                            string r = Console.ReadLine();
                            Console.WriteLine();
                            for (int i = 0; i < list.Count; i++)
                            {
                                if(list.ElementAt(i).region.Equals(r))
                                Console.WriteLine(list.ElementAt(i).show());
                            }
                            Console.ReadLine();
                            break;
                        }
                    case "4":
                        {
                            Console.WriteLine("Введите номер записи на удаление:");
                            int num = Convert.ToInt32(Console.ReadLine());
                            if (num>0&&num<list.Count)
                            list.RemoveAt(num-1);
                            fstr.Close();
                            Serialize();
                            newList = true;
                            break;
                        }
                    case "5":
                        {

                            break;
                        }
                }
                Console.Clear();
                fstr.Close();
            }
        }
    }
}
