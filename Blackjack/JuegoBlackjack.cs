using System;
using System.Collections.Generic;

namespace Uno
{
    public class JuegoBlackjack : IJuego
    {
        private readonly List<JugadorBlackjack> jugadores;
        private readonly JugadorBlackjack dealer;
        private readonly BarajaBlackjack baraja;
        private readonly int rondas;

        public JuegoBlackjack(List<JugadorBlackjack> jugadores, JugadorBlackjack dealer, int rondas, BarajaBlackjack baraja = null)
        {
            this.jugadores = jugadores ?? throw new ArgumentNullException(nameof(jugadores));
            this.dealer = dealer ?? throw new ArgumentNullException(nameof(dealer));
            this.rondas = rondas;
            this.baraja = baraja ?? new BarajaBlackjack();

            if (baraja == null)
                this.baraja.Mezclar();
        }

        public void Ejecutar()
        {
            Console.WriteLine("=== Comienzo del juego de 21 Blackjack ===");
            Console.WriteLine($"Rondas a jugar: {rondas}");
            Console.WriteLine("******************************************");

            foreach (var j in jugadores)
                j.Victorias = 0;

            for (int ronda = 1; ronda <= rondas; ronda++)
            {
                Console.WriteLine($"\n===== Ronda {ronda} =====");
                PrepararRonda();

                foreach (var jugador in jugadores)
                {
                    Console.WriteLine($"\nTurno de {jugador.Nombre}");
                    jugador.MostrarMano();
                    while (jugador.Comportamiento.DebePedirCarta(this, jugador) && jugador.CalcularPuntos() <= 21)
                    {
                        Console.WriteLine($"{jugador.Nombre} pide carta.");
                        jugador.AgregarCarta(baraja.Robar());
                        jugador.MostrarMano();

                        if (jugador.CalcularPuntos() > 21)
                        {
                            Console.WriteLine($"{jugador.Nombre} se pasa de 21.");
                            break;
                        }
                    }

                    if (!jugador.Comportamiento.DebePedirCarta(this, jugador) && jugador.CalcularPuntos() <= 21)
                    {
                        Console.WriteLine($"{jugador.Nombre} se planta con {jugador.CalcularPuntos()} puntos.");
                    }
                }

                Console.WriteLine("\nTurno del Dealer:");
                dealer.MostrarMano();
                while (dealer.CalcularPuntos() < 17)
                {
                    Console.WriteLine("Dealer pide carta.");
                    dealer.AgregarCarta(baraja.Robar());
                    dealer.MostrarMano();
                }

                int puntosDealer = dealer.CalcularPuntos();
                if (puntosDealer > 21)
                    Console.WriteLine("Dealer se pasa de 21.");

                foreach (var jugador in jugadores)
                {
                    int pj = jugador.CalcularPuntos();

                    if (pj > 21)
                    {
                        Console.WriteLine($"{jugador.Nombre} pierde (más de 21).");
                    }
                    else if (puntosDealer > 21)
                    {
                        Console.WriteLine($"{jugador.Nombre} gana (dealer se pasó).");
                        jugador.Victorias++;
                    }
                    else if (pj > puntosDealer)
                    {
                        Console.WriteLine($"{jugador.Nombre} gana (más puntos que el dealer).");
                        jugador.Victorias++;
                    }
                    else
                    {
                        Console.WriteLine($"{jugador.Nombre} pierde o empata (no supera al dealer).");
                    }
                }

                Console.WriteLine("===== Fin de la ronda =====");
            }

            Console.WriteLine("\n=== Resultados finales ===");
            int max = 0;
            foreach (var j in jugadores)
            {
                Console.WriteLine($"{j.Nombre}: {j.Victorias} victoria(s).");
                if (j.Victorias > max) max = j.Victorias;
            }

            var ganadores = jugadores.FindAll(j => j.Victorias == max);
            if (max == 0)
            {
                Console.WriteLine("Nadie ganó rondas.");
            }
            else
            {
                Console.Write("Ganador(es): ");
                for (int i = 0; i < ganadores.Count; i++)
                {
                    Console.Write(ganadores[i].Nombre);
                    if (i < ganadores.Count - 1) Console.Write(", ");
                }
                Console.WriteLine();
            }
        }

        private void PrepararRonda()
        {
            if (baraja.CartasDisponibles() < (jugadores.Count + 1) * 5)
            {
                baraja.Generar();
                baraja.Mezclar();
                Console.WriteLine("Se regenera y barajea la baraja para la nueva ronda.");
            }

            foreach (var j in jugadores)
                j.LimpiarMano();
            dealer.LimpiarMano();

            foreach (var j in jugadores)
            {
                j.AgregarCarta(baraja.Robar());
                j.AgregarCarta(baraja.Robar());
            }

            dealer.AgregarCarta(baraja.Robar());
            dealer.AgregarCarta(baraja.Robar());

            Console.WriteLine("\nCartas iniciales:");
            foreach (var j in jugadores)
                j.MostrarMano();

            Console.WriteLine("Mano inicial del Dealer:");
            dealer.MostrarMano();
        }
    }
}
