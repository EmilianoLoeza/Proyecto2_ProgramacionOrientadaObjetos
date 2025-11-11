using System.Collections.Generic;

namespace Uno
{
    public class ComportamientoCalculado : IComportamientoUno
    {
        public int ElegirIndiceCarta(JuegoUno juego, JugadorUno jugador, List<CartaUno> jugables)
        {
            if (jugables.Count == 0) return -1;

            var siguiente = juego.ObtenerSiguienteJugador();
            if (siguiente.CartasEnMano == 1)
            {
                int idx = Buscar(jugables, TipoCartaUno.Mas4);
                if (idx != -1) return idx;
                idx = Buscar(jugables, TipoCartaUno.Mas2);
                if (idx != -1) return idx;
                idx = Buscar(jugables, TipoCartaUno.Bloqueo);
                if (idx != -1) return idx;
            }

            int normal = Buscar(jugables, TipoCartaUno.Numero);
            return normal != -1 ? normal : 0;
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
            int max = 0;
            for (int i = 1; i < conteo.Length; i++)
                if (conteo[i] > conteo[max]) max = i;

            
        }

        private int Buscar(List<CartaUno> lista, TipoCartaUno tipo)
        {
            for (int i = 0; i < lista.Count; i++)
                if (lista[i].Tipo == tipo) return i;
            return -1;
        }
    }
}