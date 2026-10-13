using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace juegosnake.eg
{
        public class Ventana
        {
            public string Titulo { get; set; }
            public int Ancho { get; set; }
            public int Altura { get; set; }
            public ConsoleColor ColorFondo { get; set; }
            public ConsoleColor ColorLetra { get; set; }
            public Point LimiteSuperior { get; set; }
            public Point LimiteInferior { get; set; }
            public int Area { get; set; }
            public Snake SnakeC { get; set; }

            public Ventana(string titulo, int ancho, int altura, ConsoleColor colorFondo, ConsoleColor colorLetra, Point limiteSuperior, Point limiteInferior)
            {
                Titulo = titulo;
                Ancho = ancho;
                Altura = altura;
                ColorFondo = colorFondo;
                ColorLetra = colorLetra;
                LimiteSuperior = limiteSuperior;
                LimiteInferior = limiteInferior;
                Area = (LimiteInferior.X - LimiteSuperior.X - 1) * (LimiteInferior.Y - LimiteSuperior.Y - 1);

                Init();
            }

            private void Init()
            {
                Console.SetWindowSize(Ancho, Altura);
                Console.Title = Titulo;
                Console.CursorVisible = false;
                Console.BackgroundColor = ColorFondo;
                Console.Clear();
            }

            public void DibujarMarco()
            {
                Console.ForegroundColor = ColorLetra;

                for (int i = LimiteSuperior.X; i < LimiteInferior.X; i++)
                {
                    Console.SetCursorPosition(i, LimiteSuperior.Y);
                    Console.Write((char)205);
                    Console.SetCursorPosition(i, LimiteInferior.Y);
                    Console.Write((char)205);
                }

                for (int i = LimiteSuperior.Y; i <= LimiteInferior.Y; i++)
                {
                    Console.SetCursorPosition(LimiteSuperior.X, i);
                    Console.Write((char)186);
                    Console.SetCursorPosition(LimiteInferior.X, i);
                    Console.Write((char)186);
                }


                Console.SetCursorPosition(LimiteSuperior.X, LimiteSuperior.Y);
                Console.Write((char)201);
                Console.SetCursorPosition(LimiteSuperior.X, LimiteInferior.Y);
                Console.Write((char)200);
                Console.SetCursorPosition(LimiteInferior.X, LimiteSuperior.Y);
                Console.Write((char)187);
                Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
                Console.Write((char)188);
            }

            public void Menu(ref bool ejecucion, ref bool jugar, Snake snake, Comida comida)
            {
                Console.Clear();
                DibujarMarco();

                int centroX = (LimiteSuperior.X + LimiteInferior.X) / 2;
                int centroY = (LimiteSuperior.Y + LimiteInferior.Y) / 2;

                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(centroX - 12, centroY - 4);
                Console.Write("S N A K E   E N   C #");

                Console.SetCursorPosition(centroX - 8, centroY - 2);
                Console.Write("Enter - Jugar");

                Console.SetCursorPosition(centroX - 8, centroY - 1);
                Console.Write("Escape - Salir");

                if (SnakeC == null)
                {
                    SnakeC = new Snake(new Point(centroX, LimiteInferior.Y - 3), ConsoleColor.Magenta, ConsoleColor.White, this);
                }
                SnakeC.MoverMenu();
            }

            public void Teclado(ref bool ejecucion, ref bool jugar, Snake snake)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    if (tecla.Key == ConsoleKey.Enter)
                    {
                        jugar = true;
                        Console.Clear();
                        DibujarMarco();
                        snake.Init();
                    }
                    else if (tecla.Key == ConsoleKey.Escape)
                    {
                        ejecucion = false;
                    }
                }
            }

            public void GameOver(string texto, ref bool jugar, ref int puntaje)
            {
                Console.Clear();
                DibujarMarco();
                int centroX = (LimiteSuperior.X + LimiteInferior.X) / 2;
                int centroY = (LimiteSuperior.Y + LimiteInferior.Y) / 2;

                Console.SetCursorPosition(centroX - 10, centroY - 2);
                Console.Write(texto);
                Thread.Sleep(3000);
                jugar = false;
                puntaje = 0;
                Console.Clear();
            }
        }
}
