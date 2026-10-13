using System;
using System.Drawing;
using System.Threading;
using juegosnake.eg; 

class Program
{
    static bool ejecutar = true;
    static bool jugar = false;

    static void Main(string[] args)
    {
        Ventana ventana = new Ventana("Snake", 65, 20, ConsoleColor.Black, ConsoleColor.White, new Point(5, 3), new Point(59, 18));
        Snake snake = new Snake(new Point(8, 5), ConsoleColor.Red, ConsoleColor.White, ventana);
        Comida comida = new Comida(ConsoleColor.Green, ventana);

        GameLoop(ventana, snake, comida);
    }

    static void GameLoop(Ventana ventana, Snake snake, Comida comida)
    {
        while (ejecutar)
        {
            while (jugar && snake.Vivo)
            {
                ventana.Teclado(ref ejecutar, ref jugar, snake);
                snake.Mover(ref jugar, ref ejecutar);
                snake.Informacion(0, 34);
                Thread.Sleep(100);
            }

            if (!ejecutar) break;

            ventana.Menu(ref ejecutar, ref jugar, snake, comida);
            Thread.Sleep(100);
        }
    }
}