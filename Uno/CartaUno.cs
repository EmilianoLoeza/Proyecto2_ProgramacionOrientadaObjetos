using System;
using System.Collections.Generic;

namespace Uno
{
    public enum TipoCarta
    {
        Numero,
        Bloqueo,
        Reversa,
        Mas2,
        Comodin,
        Mas4
    }

    public enum Color
    {
        Azul,
        Rojo,
        Verde,
        Amarillo,
        Comodines
    }

    public class Carta
    {
        public TipoCarta Tipo { get; private set; }
        public Color Color { get; private set; }
        public int? Numero { get; private set; }

        public Carta(TipoCarta tipo, Color color, int? numero = null)
        {
            Tipo = tipo;
            Color = color;
            Numero = numero;
        }

        public override string ToString()
        {
            if (Tipo == TipoCarta.Numero)
                return $"{Numero} {Color}";

            if (Color == Color.Comodines)
                return $"{Tipo}";

            return $"{Tipo} {Color}";
        }

        
        public bool PuedeJugarseSobre(Carta superior, Color colorActual)
        {
            if (Tipo == TipoCarta.Comodin || Tipo == TipoCarta.Mas4)
                return true;

            if (Color == colorActual)
                return true;

            if (superior != null)
            {
                if (Tipo != TipoCarta.Numero && superior.Tipo == Tipo)
                    return true;

                if (Tipo == TipoCarta.Numero &&
                    superior.Tipo == TipoCarta.Numero &&
                    Numero == superior.Numero)
                    return true;
            }

            return false;
        }
    }

    public class Baraja : IBaraja<Carta>
    {
        private static readonly Color[] Colores = new[]
        {
            Color.Azul, Color.Rojo, Color.Verde, Color.Amarillo
        };

        public List<Carta> Cartas { get; set; }
        private readonly Random rng;

        public Baraja(int? seed = null)
        {
            Cartas = new List<Carta>();
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
            Generar();
        }

        public void Generar()
        {
            Cartas.Clear();

            foreach (var color in Colores)
            {
                Cartas.Add(new Carta(TipoCarta.Numero, color, 0));
                for (int num = 1; num <= 9; num++)
                {
                    Cartas.Add(new Carta(TipoCarta.Numero, color, num));
                    Cartas.Add(new Carta(TipoCarta.Numero, color, num));
                }
            }

            foreach (var color in Colores)
            {
                for (int i = 0; i < 2; i++)
                {
                    Cartas.Add(new Carta(TipoCarta.Bloqueo, color));
                    Cartas.Add(new Carta(TipoCarta.Reversa, color));
                    Cartas.Add(new Carta(TipoCarta.Mas2, color));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Cartas.Add(new Carta(TipoCarta.Comodin, Color.Comodines));
                Cartas.Add(new Carta(TipoCarta.Mas4, Color.Comodines));
            }

            if (Cartas.Count != 108)
                throw new ArgumentException($"La baraja UNO debe tener 108 cartas, pero tiene {Cartas.Count}.");
        }

        public void Mezclar()
        {
            int n = Cartas.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                var tmp = Cartas[i];
                Cartas[i] = Cartas[j];
                Cartas[j] = tmp;
            }
        }

        public Carta Robar()
        {
            if (Cartas.Count == 0)
                return null;

            var carta = Cartas[0];
            Cartas.RemoveAt(0);
            return carta;
        }

        public int CartasDisponibles() => Cartas.Count;
    }
}
