using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace juegosnake.eg
{
    public class Comida
    {
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }

        public Comida(ConsoleColor color, Ventana ventanaC)
        {
            Color = color;
            VentanaC = ventanaC;
        }

        public void Dibujar()
        {
            Console.ForegroundColor = Color;
            Console.SetCursorPosition(Posicion.X, Posicion.Y);
            Console.Write((char)219);
        }

        public bool GenerarComida(Snake snake)
        {
            int longitudSnake = snake.Cuerpo.Count + 1;
            if (VentanaC.Area - longitudSnake <= 0) return false;

            Random rnd = new Random();
            int x = rnd.Next(VentanaC.LimiteSuperior.X + 1, VentanaC.LimiteInferior.X);
            int y = rnd.Next(VentanaC.LimiteSuperior.Y + 1, VentanaC.LimiteInferior.Y);

            Posicion = new Point(x, y);

            bool enSnake = false;
            if (Posicion.Equals(snake.Cabeza)) enSnake = true;
            foreach (Point parte in snake.Cuerpo)
            {
                if (Posicion.Equals(parte)) enSnake = true;
            }

            if (enSnake)
            {
                return GenerarComida(snake);
            }

            Dibujar();
            return true;
        }
    }
}
