using System;
using System.Threading;

namespace Threading
{
    public class Clase26_8
    {
        // Ej 3
        private bool finish = false;

        // Ej 4
        private AutoResetEvent _autoResetEvent1 = new AutoResetEvent(false);
        private AutoResetEvent _autoResetEvent2 = new AutoResetEvent(false);

        // Ej 5
        private AutoResetEvent _autoResetEvent3 = new AutoResetEvent(true);
        private AutoResetEvent _autoResetEvent4 = new AutoResetEvent(false);

        // Ej 6 y 7
        private bool yaimprimi = false;
        private readonly object locker = new Object();

        public Clase26_8()
        {
        }

        public void Ejercicio7()
        {
            for (int i = 0; i < 10; i++)
            {
                var j = i;
                Thread th = new Thread(() => FuncThread2($"FuncThread: { j }"));
                th.Start();
            }
        }

        public void FuncThread2(String name)
        {
            // Ej. Solo 1 puede imprimir que llego primero

            // Evita que esta parte del codigo ejecute en paralelo
            // Deberiamos utilizar la minima cantidad de codigo
            // NO deberiamos utilizar codigo externo (ej. librerias, etc)

            // Con lo monitor puedo hacer lo mismo que con lock. Solo que con
            // Monitor puedo hacer un Monitor.Wait(locker), para esperar por un
            // impulso, y Monitor.Impulse(locker) para que otro hilo lo utilice
            // y desbloquee ese punto.
            Monitor.Enter(locker);

            try
            {
                if (!yaimprimi)
                {
                    Console.WriteLine($"Soy el Thread {name} y llegue primero");
                    yaimprimi = true;
                }
            }catch (Exception e)
            {
                Console.WriteLine($"Capture una exception {e.Message}");
            }
            finally
            {
                Monitor.Exit(locker);
            }


            Console.WriteLine($"Soy el Thread {name} y finalice");

        }

        public void Ejercicio6()
        {
            for (int i = 0; i < 10; i++)
            {
                var j = i;
                Thread th = new Thread(() => FuncThread($"FuncThread: { j }"));
                th.Start();
            }
        }

        public void FuncThread(String name)
        {
            // Ej. Solo 1 puede imprimir que llego primero

            // Evita que esta parte del codigo ejecute en paralelo
            // Deberiamos utilizar la minima cantidad de codigo
            // NO deberiamos utilizar codigo externo (ej. librerias, etc)
            lock (locker) 
            {
                if (!yaimprimi)
                {
                    Console.WriteLine($"Soy el Thread {name} y llegue primero");
                    yaimprimi = true;
                }
            }

            Console.WriteLine($"Soy el Thread {name} y finalice");

        }

        public void Ejercicio5()
        {
            // Ej. Imprimr X e Y de forma alternada

            var thread1 = new Thread(() => FuncThreadX3("FunThreadX"));
            var thread2 = new Thread(() => FuncThreadZ3("FunThreadZ"));

            thread1.Start();
            thread2.Start();
        }

        public void FuncThreadX3(String name)
        {
            for (var i = 0; i < 10; i++)
            {
                _autoResetEvent3.WaitOne();

                Console.Write("X ");

                _autoResetEvent4.Set();
            }
        }

        public void FuncThreadZ3(String name)
        {
            for (var i = 0; i < 10; i++)
            {
                _autoResetEvent4.WaitOne();

                Console.Write("Z ");

                _autoResetEvent3.Set();
            }
        }

        public void Ejercicio4()
        {
            // Ej. Imprimr X e Y de forma alternada
            // Este lo hice yo

            var thread1 = new Thread(() => FuncThreadX2("FunThreadX"));
            var thread2 = new Thread(() => FuncThreadZ2("FunThreadZ"));

            thread1.Start();
            thread2.Start();
        }

        public void FuncThreadX2(String name)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.Write("X ");

                _autoResetEvent1.Set();

                _autoResetEvent2.WaitOne();
            }

            
        }

        public void FuncThreadZ2(String name)
        {
            for (var i = 0; i < 10; i++)
            {
                _autoResetEvent1.WaitOne();

                Console.Write("Z ");

                _autoResetEvent2.Set();
            }
        }

        public void Ejercicio3()
        {
            // Ejercicio de la clase pasada...

            var thread1 = new Thread(() =>  FuncThreadX("FunThreadX") );
            var thread2 = new Thread(() =>  FuncThreadZ("FunThreadZ") );

            thread1.Start();
            thread2.Start();
        }

        public void FuncThreadX(String name)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.Write("X ");
            }

            Console.WriteLine();

            finish = true;
        }

        public void FuncThreadZ(String name)
        {
            while (!finish) // Gastamos recursos de procesador al pedo
            {
                Thread.Sleep(100);
            }

            for (var i = 0; i < 10; i++)
            {
                Console.Write("Z ");
            }
        }

        public void Ejercicio2()
        {
            // Ej. Bloquear - Desbloquear con manualResetEvent

            ManualResetEvent manualResetEvent = new ManualResetEvent(false); // false: sin destrancar
            new Thread(() =>
            {
                Console.WriteLine("Thread 1 has started... ");
                Console.WriteLine("Waiting for signal from another thread");
                manualResetEvent.WaitOne(); //deja bloqueado el thread, se puede poner un timer...
                Console.WriteLine("Got signal");
            }).Start();

            manualResetEvent.Set(); // se libera el WaitOne, y se el otro thread termina.
        }

        public void Ejercicio1()
        {
            // Muestra de lo que pasa cuando largamos hilos de un for y le
            // pasamos la variable de control del for (imprime en otro
            // orden y repetido)
            // Definiendo y pasando una variable local dentro del for y 
            // haciendo una copia de la variable de control, ya soluciona el
            // problema.

            Console.WriteLine();
            for (int i = 0; i < 10; i++)
            {
                var interm = i;
                Thread th = new Thread(() => PrintVar(interm));
                th.Start();
            }
        }

        private void PrintVar(int i)
        {
            Console.Write(i + " ");
        }
    }

    
}
