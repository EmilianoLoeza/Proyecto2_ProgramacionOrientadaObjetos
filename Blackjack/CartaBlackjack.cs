using System;

namespace Uno
{
    public enum Palo
    {
        Corazones,
        Diamantes,
        Treboles,
        Picas
    }

    public enum Valor
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

    public class Carta
    {
        public Palo Palo { get; }
        public Valor Valor { get; }

        public Carta(Palo palo, Valor valor)
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
