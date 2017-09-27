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
        private String marka;
        private String model;
        private long year;
        private bool err = false;

        static List<Program> list;
        static bool newList;
        private Program() { }

        private void data(String r, String t, long q)
        {
            this.marka = r;
            this.model = t;
            this.year = q;
        }

        private Program(String r,String t,long q) {
            this.marka = r;
            this.model = t;
            this.year = q;
        }
        public string show()
        {
            return this.marka+" "+this.model+" "+this.year;
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

        Program vvod()
        {
            Program obj = new Program();
            try
            {
                String marka = Console.ReadLine();
                if (marka.Length == 0)
                {
                    throw new Exception("Ничего не было введено!");
                }
                String model = Console.ReadLine();
                if (model.Length == 0)
                {
                    throw new Exception("Ничего не было введено!");
                }
                long year = System.Int64.Parse(Console.ReadLine());
                obj.data(marka,model,year);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                obj.err = true;
                Console.ReadLine();
            }
            return obj;
        }

        static void Main(string[] args)
        {
            try
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

                    Console.WriteLine("Выберите пункт:\n 1 - Ввести новую запись\n 2 - Посмотреть список автомобилей\n 3 - Посмотреть автомобили конкретной марки\n"
                        + " 4 - Удалить запись\n 5 - Редактировать запись\n 6 - Сортировать по модели\n 7 - Выход");
                    choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            {
                                Console.WriteLine("Введите марку автомобиля, модель и год выпуска:");

                                Program obj = new Program();

                                obj = obj.vvod();
                                if (obj.err == false)
                                {
                                    fstr.Seek(0, SeekOrigin.End);
                                    BinaryFormatter bf = new BinaryFormatter();
                                    bf.Serialize(fstr, obj);
                                    list.Add(obj);
                                }
                                break;
                            }
                        case "2":
                            {
                                Console.WriteLine("Список:");
                                int i = 0;
                                if (list.Count == 0)
                                {
                                    Console.WriteLine("Список пуст");
                                }
                                else
                                    foreach (var ob in list)
                                    {
                                        i++;
                                        Console.WriteLine(i + ") " + ob.show());
                                    }
                                Console.ReadLine();
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("Введите марку автомобиля для поиска");
                                string r = Console.ReadLine();
                                if (r.Length != 0)
                                {
                                    Console.WriteLine();
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        if (list.ElementAt(i).marka.Equals(r))
                                            Console.WriteLine(list.ElementAt(i).show());
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка ввода!");
                                }
                                Console.ReadLine();
                                break;
                            }
                        case "4":
                            {
                                Console.WriteLine("Введите номер записи для удаления");
                                try
                                {
                                    int num = Convert.ToInt32(Console.ReadLine());

                                    if (num <= 0)
                                    {
                                        throw new Exception("Введенное число - отрицательно");
                                    }
                                    if (num > list.Count)
                                    {
                                        throw new Exception("Введенное число больше размера списка");
                                    }
                                    num--;
                                    if (num >= 0 && num < list.Count)
                                    {
                                        list.RemoveAt(num);
                                        fstr.Close();
                                        Serialize();
                                        newList = true;
                                    }
                                    Console.WriteLine("Удалено!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                Console.ReadLine();
                                break;
                            }
                        case "5":
                            {
                                Console.WriteLine("Введите номер записи для редактирования:");
                                try
                                {
                                    int num = Convert.ToInt32(Console.ReadLine());

                                    if (num <= 0)
                                    {
                                        throw new Exception("Введенное число - отрицательно");
                                    }
                                    if (num > list.Count)
                                    {
                                        throw new Exception("Введенное число больше размера списка");
                                    }
                                    num--;
                                    if (num > 0 && num < list.Count)
                                    {
                                        Console.WriteLine("Редактируемая запись: " + list.ElementAt(num).show());
                                        Console.WriteLine("Введите марку автомобиля, модель и год выпуска:");
                                        Program obj = new Program();
                                        obj = obj.vvod();
                                        if (obj.err == false)
                                        {
                                            list.ElementAt(num).data(obj.marka, obj.model, obj.year);
                                            fstr.Close();
                                            Serialize();
                                            newList = true;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                Console.ReadLine();
                                break;
                            }
                        case "6":
                            {
                                list.Sort(delegate (Program ob1, Program ob2)
                                { return ob1.marka.CompareTo(ob2.marka); });
                                Console.WriteLine("Отсортировано!");
                                Console.ReadLine();
                                break;
                            }
                    }
                    Console.Clear();
                    fstr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message); 
            }
        }
    }
}
