namespace Uno
{
    public class ComportamientoCauteloso : IComportamientoBlackjack
    {
        private readonly int puntoCorte;

        public ComportamientoCauteloso(int puntoCorte)
        {
            this.puntoCorte = puntoCorte;
        }

        public bool DebePedirCarta(JuegoBlackjack juego, JugadorBlackjack jugador)
        {
            int puntos = jugador.CalcularPuntos();
            return puntos < puntoCorte && puntos <= 21;
        }
    }
}
