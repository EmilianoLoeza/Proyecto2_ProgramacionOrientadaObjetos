using System;
using System.Collections.Generic;

namespace Uno
{
    public class ComportamientoAleatorio : IComportamientoUno
    {
        private static readonly Random rng = new Random();

        public int ElegirIndiceCarta(JuegoUno juego, JugadorUno jugador, List<CartaUno> jugables)
        {
            if (jugables.Count == 0) return -1;
            return rng.Next(jugables.Count);
        }

        public ColorUno ElegirColor(JuegoUno juego, JugadorUno jugador)
        {
            int[] conteo = new int[4];
            foreach (var c in jugador.Mano)
            {
                switch (c.Color)
                {
                    case ColorUno.Azul: conteo[0]++; break;
                    case ColorUno.Rojo: conteo[1]++; break;
                    case ColorUno.Verde: conteo[2]++; break;
                    case ColorUno.Amarillo: conteo[3]++; break;
                }
            }
            int max = Array.IndexOf(conteo, Math.Max(Math.Max(conteo[0], conteo[1]), Math.Max(conteo[2], conteo[3])));
            return (ColorUno)max;
        }
    }
}
