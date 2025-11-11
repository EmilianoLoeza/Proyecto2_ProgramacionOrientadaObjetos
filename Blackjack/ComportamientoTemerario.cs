namespace Uno
{
    public class ComportamientoTemerario : IComportamientoBlackjack
    {
        public bool DebePedirCarta(JuegoBlackjack juego, JugadorBlackjack jugador)
        {
            int puntos = jugador.CalcularPuntos();
            return puntos < 21;
        }
    }
}
