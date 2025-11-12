using System;
using System.Collections.Generic;

namespace Uno
{
    public class BarajaBlackjack : IBaraja<CartaBlackjack>
    {
        public List<CartaBlackjack> Cartas { get; set; } = new List<CartaBlackjack>();
        private readonly Random rng;

        public BarajaBlackjack(int? semilla = null)
        {
            rng = semilla.HasValue ? new Random(semilla.Value) : new Random();
            Generar();
        }

        public void Generar()
        {
            Cartas.Clear();

            foreach (PaloBlackjack palo in Enum.GetValues(typeof(PaloBlackjack)))
            {
                foreach (ValorBlackjack valor in Enum.GetValues(typeof(ValorBlackjack)))
                {
                    Cartas.Add(new CartaBlackjack(palo, valor));
                }
            }

            if (Cartas.Count != 52)
                throw new Exception($"La baraja de Blackjack debe tener 52 cartas, pero se generaron {Cartas.Count}.");
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

        public CartaBlackjack Robar()
        {
            if (Cartas.Count == 0) return null;
            var carta = Cartas[0];
            Cartas.RemoveAt(0);
            return carta;
        }

        public int CartasDisponibles() => Cartas.Count;
    }
}
