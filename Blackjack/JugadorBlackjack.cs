using System;
using System.Collections.Generic;

namespace Uno
{
    public class JugadorBlackjack
    {
        public string Nombre { get; }
        public IComportamientoBlackjack Comportamiento { get; }
        public List<CartaBlackjack> Mano { get; } = new List<CartaBlackjack>();
        public int Victorias { get; set; }

        public JugadorBlackjack(string nombre, IComportamientoBlackjack comportamiento)
        {
            Nombre = nombre;
            Comportamiento = comportamiento;
        }

        public void LimpiarMano() => Mano.Clear();

        public void AgregarCarta(CartaBlackjack carta)
        {
            if (carta != null) Mano.Add(carta);
        }

        public int CalcularPuntos()
        {
            int suma = 0;
            int ases = 0;

            foreach (var c in Mano)
            {
                suma += (int)c.Valor;
                if (c.Valor == ValorBlackjack.As) ases++;
            }

            while (suma > 21 && ases > 0)
            {
                suma -= 10;
                ases--;
            }

            return suma;
        }

        public void MostrarMano(bool ocultarPrimera = false)
        {
            Console.Write($"Mano de {Nombre}: ");
            for (int i = 0; i < Mano.Count; i++)
            {
                if (ocultarPrimera && i == 0)
                    Console.Write("[Carta oculta]");
                else
                    Console.Write(Mano[i]);

                if (i < Mano.Count - 1) Console.Write(", ");
            }

            Console.WriteLine($" (Total: {(ocultarPrimera ? "?" : CalcularPuntos().ToString())})");
        }
    }
}
