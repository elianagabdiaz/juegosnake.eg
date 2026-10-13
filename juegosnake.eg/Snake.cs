using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace juegosnake.eg
{
    public enum Direccion { Derecha, Izquierda, Arriba, Abajo }

    public class Snake
    {
        public bool Vivo { get; set; }
        public ConsoleColor ColorCabeza { get; set; }
        public ConsoleColor ColorCuerpo { get; set; }
        public Ventana VentanaC { get; set; }
        public Comida ComidaC { get; set; }
        public List<Point> Cuerpo { get; set; }
        public Point Cabeza { get; set; }
        public Point PosicionInicial { get; set; }
        public Direccion Dir { get; set; }
        public bool Comiendo { get; set; }
        public int Puntaje { get; set; }
        public int PuntajeMaximo { get; set; }

        public Snake(Point posicionInicial, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventanaC)
        {
            PosicionInicial = posicionInicial;
            ColorCabeza = colorCabeza;
            ColorCuerpo = colorCuerpo;
            VentanaC = ventanaC;
            Cuerpo = new List<Point>();
            Puntaje = 0;
            PuntajeMaximo = 0;
        }

        public void Init()
        {
            Console.Clear();
            VentanaC.DibujarMarco();
            Cabeza = PosicionInicial;
            Cuerpo.Clear();
            IniciarCuerpo(2);
            Vivo = true;
            Dir = Direccion.Derecha;
            Comiendo = false;
            if (ComidaC != null)
            {
                ComidaC.GenerarComida(this);
            }
        }

        public void IniciarCuerpo(int cantidad)
        {
            int x = Cabeza.X - 1;
            for (int i = 0; i < cantidad; i++)
            {
                Console.SetCursorPosition(x, Cabeza.Y);
                Console.Write((char)178);
                Cuerpo.Add(new Point(x, Cabeza.Y));
                x--;
            }
        }

        public void Mover(ref bool jugar, ref bool ejecucion)
        {
            Teclado();
            Point posicionCabezaAnterior = Cabeza;
            MoverCabeza();
            ColisionesMarco();
            ColisionesComida(ref jugar);

            if (ColisionesCuerpo())
            {
                int puntajeAux = Puntaje;
                VentanaC.GameOver("G A M E   O V E R", ref jugar, ref puntajeAux);
                Puntaje = puntajeAux;
                return;
            }

            MoverCuerpo(posicionCabezaAnterior);
        }

        public void MoverCabeza()
        {
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write(" ");

            switch (Dir)
            {
                case Direccion.Derecha: Cabeza = new Point(Cabeza.X + 1, Cabeza.Y); break;
                case Direccion.Izquierda: Cabeza = new Point(Cabeza.X - 1, Cabeza.Y); break;
                case Direccion.Arriba: Cabeza = new Point(Cabeza.X, Cabeza.Y - 1); break;
                case Direccion.Abajo: Cabeza = new Point(Cabeza.X, Cabeza.Y + 1); break;
            }

            //ERROR 

            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.Write((char)219);
        }

        public void MoverCuerpo(Point posicionAnterior)
        {
            Console.ForegroundColor = ColorCuerpo;
            Console.SetCursorPosition(posicionAnterior.X, posicionAnterior.Y);
            Console.Write((char)178);

            Cuerpo.Insert(0, posicionAnterior);

            if (Comiendo)
            {
                Comiendo = false;
                return;
            }

            Point ultimaPosicion = Cuerpo[Cuerpo.Count - 1];
            Console.SetCursorPosition(ultimaPosicion.X, ultimaPosicion.Y);
            Console.Write(" ");
            Cuerpo.RemoveAt(Cuerpo.Count - 1);
        }

        public void MoverMenu()
        {
            Point posicionAnterior = Cabeza;
            MoverCabeza();
            Cuerpo.Insert(0, posicionAnterior);
            Point ultima = Cuerpo[Cuerpo.Count - 1];
            Console.SetCursorPosition(ultima.X, ultima.Y);
            Console.Write(" ");
            Cuerpo.RemoveAt(Cuerpo.Count - 1);
        }

        public void Teclado()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.RightArrow && Dir != Direccion.Izquierda) Dir = Direccion.Derecha;
                if (tecla.Key == ConsoleKey.LeftArrow && Dir != Direccion.Derecha) Dir = Direccion.Izquierda;
                if (tecla.Key == ConsoleKey.UpArrow && Dir != Direccion.Abajo) Dir = Direccion.Arriba;
                if (tecla.Key == ConsoleKey.DownArrow && Dir != Direccion.Arriba) Dir = Direccion.Abajo;
            }
        }

        public void ColisionesMarco()
        {
            if (Cabeza.X <= VentanaC.LimiteSuperior.X)
            {
                Cabeza = new Point(VentanaC.LimiteInferior.X - 1, Cabeza.Y);
            }
            else if (Cabeza.X >= VentanaC.LimiteInferior.X)
            {
                Cabeza = new Point(VentanaC.LimiteSuperior.X + 1, Cabeza.Y);
            }
            if (Cabeza.Y <= VentanaC.LimiteSuperior.Y)
            {
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteInferior.Y - 1);
            }
            else if (Cabeza.Y >= VentanaC.LimiteInferior.Y)
            {
                Cabeza = new Point(Cabeza.X, VentanaC.LimiteSuperior.Y + 1);
            }
        }

        public bool ColisionesCuerpo()
        {
            foreach (Point parte in Cuerpo)
            {
                if (Cabeza.Equals(parte)) return true;
            }
            return false;
        }

        public void ColisionesComida(ref bool jugar)
        {
            if (Cabeza.Equals(ComidaC.Posicion))
            {
                Comiendo = true;
                Puntaje++;
                if (Puntaje > PuntajeMaximo) PuntajeMaximo = Puntaje;

                if (!ComidaC.GenerarComida(this))
                {
                    int puntajeAux = Puntaje;
                    VentanaC.GameOver("JUEGO COMPLETADO", ref jugar, ref puntajeAux);
                    Puntaje = puntajeAux;
                }
            }
        }

        public void Informacion(int dist1, int dist2)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + dist1, VentanaC.LimiteSuperior.Y - 1);
            Console.Write($"Puntaje: {Puntaje}  ");

            Console.SetCursorPosition(VentanaC.LimiteSuperior.X + dist2, VentanaC.LimiteSuperior.Y - 1);
            Console.Write($"Maximo: {PuntajeMaximo}  ");
        }
    }
}
