using System;
using System.Collections.Generic;

namespace Uno
{
    public enum TipoCartaUno
    {
        Numero,
        Bloqueo,
        Reversa,
        Mas2,
        Comodin,
        Mas4
    }

    public enum ColorUno
    {
        Azul,
        Rojo,
        Verde,
        Amarillo,
        Comodines
    }

    public class CartaUno
    {
        public TipoCartaUno Tipo { get; private set; }
        public ColorUno Color { get; private set; }
        public int? Numero { get; private set; }

        public CartaUno(TipoCartaUno tipo, ColorUno color, int? numero = null)
        {
            Tipo = tipo;
            Color = color;
            Numero = numero;
        }

        public override string ToString()
        {
            if (Tipo == TipoCartaUno.Numero)
                return $"{Numero} {Color}";

            if (Color == ColorUno.Comodines)
                return $"{Tipo}";

            return $"{Tipo} {Color}";
        }

        
        public bool PuedeJugarseSobre(CartaUno superior, ColorUno colorActual)
        {
            if (Tipo == TipoCartaUno.Comodin || Tipo == TipoCartaUno.Mas4)
                return true;

            if (Color == colorActual)
                return true;

            if (superior != null)
            {
                if (Tipo != TipoCartaUno.Numero && superior.Tipo == Tipo)
                    return true;

                if (Tipo == TipoCartaUno.Numero &&
                    superior.Tipo == TipoCartaUno.Numero &&
                    Numero == superior.Numero)
                    return true;
            }

            return false;
        }
    }

    public class BarajaUno : IBaraja<CartaUno>
    {
        private static readonly ColorUno[] Colores = new[]
        {
            ColorUno.Azul, ColorUno.Rojo, ColorUno.Verde, ColorUno.Amarillo
        };

        public List<CartaUno> Cartas { get; set; }
        private readonly Random rng;

        public BarajaUno(int? seed = null)
        {
            Cartas = new List<CartaUno>();
            rng = seed.HasValue ? new Random(seed.Value) : new Random();
            Generar();
        }

        public void Generar()
        {
            Cartas.Clear();

            foreach (var color in Colores)
            {
                Cartas.Add(new CartaUno(TipoCartaUno.Numero, color, 0));
                for (int num = 1; num <= 9; num++)
                {
                    Cartas.Add(new CartaUno(TipoCartaUno.Numero, color, num));
                    Cartas.Add(new CartaUno(TipoCartaUno.Numero, color, num));
                }
            }

            foreach (var color in Colores)
            {
                for (int i = 0; i < 2; i++)
                {
                    Cartas.Add(new CartaUno(TipoCartaUno.Bloqueo, color));
                    Cartas.Add(new CartaUno(TipoCartaUno.Reversa, color));
                    Cartas.Add(new CartaUno(TipoCartaUno.Mas2, color));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                Cartas.Add(new CartaUno(TipoCartaUno.Comodin, ColorUno.Comodines));
                Cartas.Add(new CartaUno(TipoCartaUno.Mas4, ColorUno.Comodines));
            }

            if (Cartas.Count != 108)
                throw new InvalidOperationException($"La baraja UNO debe tener 108 cartas, pero tiene {Cartas.Count}.");
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

        public CartaUno Robar()
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
