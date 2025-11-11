using System;

namespace Uno
{
    public enum PaloBlackjack
    {
        Corazones,
        Diamantes,
        Treboles,
        Picas
    }

    public enum ValorBlackjack
    {
        Dos = 2,
        Tres = 3,
        Cuatro = 4,
        Cinco = 5,
        Seis = 6,
        Siete = 7,
        Ocho = 8,
        Nueve = 9,
        Diez = 10,
        J = 10,
        Q = 10,
        K = 10,
        As = 11
    }

    public class CartaBlackjack
    {
        public PaloBlackjack Palo { get; }
        public ValorBlackjack Valor { get; }

        public CartaBlackjack(PaloBlackjack palo, ValorBlackjack valor)
        {
            Palo = palo;
            Valor = valor;
        }

        public override string ToString()
        {
            return $"{Valor} de {Palo}";
        }
    }
}
