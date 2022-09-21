using System;
using System.IO;

namespace ClassLibraryForArray
{
    class IntArray
    {
        public delegate void Handler(string masseger);
        public event Handler Notify;

        private int[] a;    //закрытый одномерный массив
        private int length;      //закрытая длина

        public IntArray(int length)  //конструктор 1 для создания массива заданной длины length, стр. 114-117, 158
        {
            if (length >= 0)
            {
                a = new int[length];
                length = a.Length;
                Notify?.Invoke("Выполняется конструктор IntArray(int length)");
            }
            else
            {
                throw new Exception("Длина не должна быть отрицательной!");
            }
        }

        public IntArray(params int[] arr)  //конструктор 2 с переменным числом параметров, стр. 154, 163
        {
            length = arr.Length;
            for (int i = 0; i < length; i++)
            {
                a[i] = arr[i];
            }
            Notify?.Invoke("Выполняется конструктор IntArray(params int[] arr)");
        }



        public int Length { get; }   //свойство Длина массива, стр. 120-123
        public int this[int i]  // Индексатор, стр. 157-159
        {
            get
            {
                if (this != null)
                {
                    Notify?.Invoke("Выполняется индексатор get");
                    return a[i];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                a[i] = value;
                Notify?.Invoke("Выполняется индексатор set");
            }
        }



        public static IntArray RandomIntArray(int length, int a, int b) // создание массива длины length и заполнение его случайными целыми числами в диапазоне от a до b, стр. 148-150 
        {
            Random rnd = new Random();
            IntArray intArray = new IntArray();

            for (int i = 0; i < length; i++)
            {
                intArray.a[i] = rnd.Next(a, b);
            }

            intArray.Notify?.Invoke("Выполняется метод RandomIntArray");
            return intArray;
        }

        public static IntArray ArrayFromTextFile(string fileName)  //ввод массива из текстового файла с именем filename, стр. 258-259
        {
            IntArray intArray = new IntArray();
            intArray.a = new int[intArray.length];
            try
            {
                StreamReader file = new StreamReader(fileName);
                string line;
                string[] buf;
                char[] separators = new char[] { ' ', ',' };

                line = file.ReadToEnd();
                buf = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < buf.Length; ++i)
                {
                    intArray[i] = Convert.ToInt32(buf[i]);
                }
                file.Close();
                intArray.Notify?.Invoke("Выполняется метод ArrayFromTextFile");
                return intArray;
            }
            catch (FileNotFoundException e)
            {
                intArray.Notify?.Invoke($"Ошибка: {e.Message}. Проверьте правильность имени файла!");
                return null;
            }
            catch (Exception e)
            {
                intArray.Notify?.Invoke("Ошибка: " + e.Message);
                return null;
            }
        }

        public static void ArrayToTextFile(IntArray arr, string fileName)  //вывод массива arr в текстовый файл с именем filename, стр. 256-257
        {
            try
            {
                StreamWriter file = new StreamWriter(fileName);

                file.Write(arr[0]);
                if (arr.length > 1)
                {
                    for (int i = 1; i < arr.Length; ++i)
                    {
                        file.Write(", " + arr[i]);
                    }
                }
                file.Close();
                arr.Notify?.Invoke("Выполняется метод ArrayToTextFile");
            }
            catch (Exception e)
            {
                arr.Notify?.Invoke("Ошибка: " + e.Message);
            }
        }

        public static int SumArray(IntArray arr)   //вычисление суммы элементов массива arr, стр. 128-129
        {
            int sum = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                sum += arr[i];
            }
            arr.Notify?.Invoke("Выполняется метод SumArray");
            return sum;
        }

        public static IntArray operator ++(IntArray arr)    // ++: инкремент: увеличение на 1 всех элементов массива, 163-164
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i]++;
            }
            arr.Notify?.Invoke("Выполняется перегрузку оператора operator ++(IntArray arr)");
            return arr;
        }

        public static IntArray operator +(IntArray x, int y)  // +: сложение массива x со скаляром y, стр. 165-167
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] += y;
            }
            x.Notify?.Invoke("Выполняется перегрузку оператора operator +(IntArray x, int y)");
            return x;
        }

        public static IntArray operator +(int x, IntArray y)  // +: сложение скаляра x с массивом y, стр. 165-167
        {
            for (int i = 0; i < y.Length; i++)
            {
                y[i] += x;
            }
            y.Notify?.Invoke("Выполняется перегрузку оператора operator +(int x, IntArray y)");
            return y;
        }

        public static IntArray operator +(IntArray x, IntArray y)  // +: сложение двух массивов x и y, стр. 165-167
        {
            int len = Math.Max(x.length, y.length);
            IntArray intArray = new IntArray(len);

            for (int i = 0; i < len; i++)
            {
                if (i > x.length)
                {
                    intArray[i] = y[i];
                }
                else if (i > x.length)
                {
                    intArray[i] = x[i];
                }
                else
                {
                    intArray[i] = x[i] + y[i];
                }
            }
            intArray.Notify?.Invoke("Выполняется перегрузку оператора operator +(IntArray x, IntArray y)");
            return intArray;
        }

        public static IntArray operator --(IntArray arr)    // --: декремент: уменьшение на 1 всех элементов массива
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i]--;
            }
            arr.Notify?.Invoke("Выполняется перегрузку оператора operator --(IntArray arr)");
            return arr;
        }

        public static IntArray operator -(IntArray x, int y)  // -: вычитание из массива x скаляра y (x - y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] -= y;
            }
            x.Notify?.Invoke("Выполняется перегрузку оператора operator -(IntArray x, int y)");
            return x;
        }

        public static IntArray operator -(int x, IntArray y)  // -: вычитание из скаляра x массива y (x - y)
        {
            for (int i = 0; i < y.Length; i++)
            {
                y[i] -= x;
            }
            y.Notify?.Invoke("Выполняется перегрузку оператора operator -(int x, IntArray y)");
            return y;
        }

        public static IntArray operator -(IntArray x, IntArray y)  // -: вычитание из массива x массива y (x - y)
        {
            int len = Math.Max(x.length, y.length);
            IntArray intArray = new IntArray(len);

            for (int i = 0; i < len; i++)
            {
                if (i > x.length)
                {
                    intArray[i] = y[i];
                }
                else if (i > x.length)
                {
                    intArray[i] = x[i];
                }
                else
                {
                    intArray[i] = x[i] + y[i];
                }
            }
            intArray.Notify?.Invoke("Выполняется перегрузку оператора operator -(IntArray x, IntArray y)");
            return intArray;
        }

    }
}
