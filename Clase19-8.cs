using System;
using System.Threading;

namespace Threading
{
    public class Clase19_8
    {
        public Clase19_8()
        {
        }

        public void Ejercicio1()
        {
            int top = 30;
            Thread thread = new Thread(() => PrintX(top));
            thread.IsBackground = true;
            thread.Start();
            Console.Write("Z");
            PrintZ(top);
        }

        private void PrintZ(int top)
        {
            for (int i = 0; i < top; i++)
            {
                Console.Write("Z");
            }

        }

        private void PrintY(int top)
        {
            for (int i = 0; i < top; i++)
            {
                Console.Write(i);
            }

        }

        private void PrintX(int top)
        {
            Thread thread = new Thread(() => PrintY(100));
            thread.IsBackground = false;
            thread.Start();
            for (int i = 0; i < top; i++)
            {
                Console.Write("X");
            }
        }
    }
}
