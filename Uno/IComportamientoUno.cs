using System.Collections.Generic;

namespace Uno
{
    public interface IComportamientoUno
    {
        int ElegirIndiceCarta(JuegoUno juego, JugadorUno jugador, List<CartaUno> jugables);
        ColorUno ElegirColor(JuegoUno juego, JugadorUno jugador);
    }
}