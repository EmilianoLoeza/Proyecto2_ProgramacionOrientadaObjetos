using System;
using System.Collections.Generic;
using System.Linq;

namespace Uno
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Simulador de Juegos de Cartas ===");
                Console.WriteLine("1) Evaluar barajas");
                Console.WriteLine("2) Simular juego de UNO");
                Console.WriteLine("3) Simular juego de 21 Blackjack");
                Console.WriteLine("4) Simular UNO con mazo forzado (pruebas)");
                Console.WriteLine("5) Simular Blackjack con mazo forzado (pruebas)");
                Console.WriteLine("0) Salir");
                Console.Write("Opción: ");

                var opcion = Console.ReadLine();
                IJuego juego = null;

                switch (opcion)
                {
                    case "1":
                        Console.Clear();
                        EvaluarBarajas();
                        Esperar();
                        break;

                    case "2":
                        Console.Clear();
                        juego = ConfigurarJuegoUno();
                        EjecutarJuego(juego);
                        break;

                    case "3":
                        Console.Clear();
                        juego = ConfigurarBlackjack();
                        EjecutarJuego(juego);
                        break;

                    case "4":
                        Console.Clear();
                        juego = ConfigurarUnoForzado();
                        EjecutarJuego(juego);
                        break;

                    case "5":
                        Console.Clear();
                        juego = ConfigurarBlackjackForzado();
                        EjecutarJuego(juego);
                        break;

                    case "0":
                        Console.WriteLine("Saliendo...");
                        return;

                    default:
                        Console.WriteLine("Opción no válida.");
                        Esperar();
                        break;
                }
            }
        }

        private static void EjecutarJuego(IJuego juego)
        {
            if (juego == null)
                return;

            Console.WriteLine("=== COMIENZA LA SIMULACIÓN ===\n");
            juego.Ejecutar();
            Console.WriteLine("\n=== FIN DE LA SIMULACIÓN ===");
            Esperar();
        }

        private static void Esperar()
        {
            Console.WriteLine("\nPresiona ENTER para volver al menú...");
            Console.ReadLine();
        }

        private static void EvaluarBarajas()
        {
            Console.WriteLine("Evaluación de barajas:");
            Console.WriteLine("1) Baraja UNO");
            Console.WriteLine("2) Baraja Blackjack");
            Console.Write("Selecciona: ");

            var op = Console.ReadLine();

            if (op == "1")
            {
                var baraja = new BarajaUno();
                baraja.Mezclar();

                Console.WriteLine("\nPrimeras 10 cartas de la baraja UNO:");
                ImprimirLista(baraja.Cartas.Take(10));

                var primeras10 = baraja.Cartas.Take(10).ToList();
                baraja.Cartas.RemoveRange(0, primeras10.Count);

                Console.WriteLine("\nSe vuelven a introducir las 10 cartas y se barajea.");
                baraja.Cartas.AddRange(primeras10);
                baraja.Mezclar();

                Console.WriteLine("\nNuevas 10 cartas de la baraja UNO:");
                ImprimirLista(baraja.Cartas.Take(10));
            }
            else if (op == "2")
            {
                var baraja = new BarajaBlackjack();
                baraja.Mezclar();

                Console.WriteLine("\nPrimeras 10 cartas de la baraja Blackjack:");
                ImprimirLista(baraja.Cartas.Take(10));

                var primeras10 = baraja.Cartas.Take(10).ToList();
                baraja.Cartas.RemoveRange(0, primeras10.Count);

                Console.WriteLine("\nSe vuelven a introducir las 10 cartas y se barajea.");
                baraja.Cartas.AddRange(primeras10);
                baraja.Mezclar();

                Console.WriteLine("\nNuevas 10 cartas de la baraja Blackjack:");
                ImprimirLista(baraja.Cartas.Take(10));
            }
            else
            {
                Console.WriteLine("Opción no válida.");
            }
        }

        private static void ImprimirLista<T>(IEnumerable<T> lista)
        {
            int i = 1;
            foreach (var c in lista)
            {
                Console.WriteLine($"{i}. {c}");
                i++;
            }

            if (i == 1)
                Console.WriteLine("(sin cartas)");
        }


        private static IJuego ConfigurarJuegoUno()
        {
            var jugadores = new List<JugadorUno>
            {
                new JugadorAleatorio("Conejo Malo"),
                new JugadorCalculador("Chaquira"),
                new JugadorAleatorio("Doja K")
            };

            return new JuegoUno(jugadores);
        }

        private static IJuego ConfigurarUnoForzado()
        {
            var jugadores = new List<JugadorUno>
            {
                new JugadorAleatorio("Jugador +4"),
                new JugadorCalculador("Calculador"),
                new JugadorAleatorio("Random")
            };

            var mazo = new BarajaUno();
            mazo.Cartas.Clear();

            // Primeras 4 cartas: +4 para el primer jugador
            for (int i = 0; i < 4; i++)
                mazo.Cartas.Add(new CartaUno(TipoCartaUno.Mas4, ColorUno.Comodines));

            // Resto normal
            var resto = new BarajaUno();
            mazo.Cartas.AddRange(resto.Cartas);

            Console.WriteLine("[Mazo UNO forzado] Primer jugador comenzará con puros +4.");
            return new JuegoUno(jugadores, mazo);
        }


        private static IJuego ConfigurarBlackjack()
        {
            var jugadores = new List<JugadorBlackjack>
            {
                new JugadorBlackjack("Cauteloso 17", new ComportamientoCauteloso(17)),
                new JugadorBlackjack("Cauteloso 15", new ComportamientoCauteloso(15)),
                new JugadorBlackjack("Temerario", new ComportamientoTemerario())
            };

            var dealer = new JugadorBlackjack("Dealer", new ComportamientoCauteloso(17));

            return new JuegoBlackjack(jugadores, dealer, rondas: 3);
        }

        private static IJuego ConfigurarBlackjackForzado()
        {
            var jugador = new JugadorBlackjack("Cauteloso 17", new ComportamientoCauteloso(17));
            var dealer = new JugadorBlackjack("Dealer", new ComportamientoCauteloso(17));

            var mazo = new BarajaBlackjack();
            mazo.Cartas.Clear();

            // Mano controlada:
            // Jugador: 10 + 6 = 16 -> debe pedir
            // Dealer: 4 + 5 = 9
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Corazones, ValorBlackjack.Diez));   // jugador
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Picas, ValorBlackjack.Seis));      // jugador
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Treboles, ValorBlackjack.Cuatro)); // dealer
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Diamantes, ValorBlackjack.Cinco)); // dealer

            // Cartas siguientes para que se vea su comportamiento
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Corazones, ValorBlackjack.Cinco));
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Picas, ValorBlackjack.Cinco));
            mazo.Cartas.Add(new CartaBlackjack(PaloBlackjack.Treboles, ValorBlackjack.Cuatro));

            // Resto normal
            var resto = new BarajaBlackjack();
            mazo.Cartas.AddRange(resto.Cartas);

            Console.WriteLine("[Mazo Blackjack forzado] Configurado para probar jugador cauteloso.");
            return new JuegoBlackjack(new List<JugadorBlackjack> { jugador }, dealer, rondas: 1, baraja: mazo);
        }
    }
}
