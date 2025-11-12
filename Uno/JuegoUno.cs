using System;
using System.Collections.Generic;
using System.Linq;

namespace Uno
{
    public class JuegoUno : IJuego
    {
        private readonly List<JugadorUno> _jugadores;
        private BarajaUno _mazo;
        private readonly List<CartaUno> _descarte = new List<CartaUno>();

        private int _indiceActual;
        private int _direccion = 1;
        private ColorUno _colorActual;
        private int _saltosExtra;
        private static readonly Random _rng = new Random();

        // Constructor normal
        public JuegoUno(List<JugadorUno> jugadores)
            : this(jugadores, null)
        {
        }

        public JuegoUno(List<JugadorUno> jugadores, BarajaUno mazoPersonalizado)
        {
            if (jugadores == null || jugadores.Count < 2)
                throw new ArgumentException("Se necesitan al menos 2 jugadores.");

            _jugadores = jugadores;
            _mazo = mazoPersonalizado ?? new BarajaUno();

            if (mazoPersonalizado == null)
                _mazo.Mezclar();
        }

        public CartaUno CartaSuperior => _descarte.Count > 0 ? _descarte[^1] : null;
        public ColorUno ColorActual => _colorActual;

        public JugadorUno ObtenerSiguienteJugador()
        {
            int idx = CalcularSiguienteIndice(1);
            return _jugadores[idx];
        }

        public void Ejecutar()
        {
            Jugar();
        }

        public void Jugar()
        {
            Console.WriteLine("Comienzo de juego de Uno");

            // Repartir 4 cartas a cada jugador
            for (int i = 0; i < 4; i++)
            {
                foreach (var j in _jugadores)
                    j.AgregarCarta(RobarDelMazo());
            }

            // Carta inicial
            var inicial = RobarDelMazo();
            if (inicial == null)
            {
                Console.WriteLine("No se pudo obtener carta inicial.");
                return;
            }

            _descarte.Add(inicial);
            _colorActual = inicial.Color == ColorUno.Comodines
                ? ColorAleatorio()
                : inicial.Color;

            Console.WriteLine($"Carta inicial: {inicial}");
            Console.WriteLine($"Color actual: {_colorActual}");
            Console.WriteLine("***********");

            bool hayGanador = false;
            int limiteSeguridad = 3000;

            while (!hayGanador && limiteSeguridad-- > 0)
            {
                var actual = _jugadores[_indiceActual];

                Console.WriteLine($"Turno de jugador {actual.Nombre}");
                actual.MostrarMano();

                _saltosExtra = 0;
                actual.TomarTurno(this);

                if (actual.CartasEnMano == 0)
                {
                    Console.WriteLine($">>> {actual.Nombre} gana la partida.");
                    hayGanador = true;
                    break;
                }

                Console.WriteLine($"Fin de turno de {actual.Nombre}");
                Console.WriteLine("***********");

                _indiceActual = CalcularSiguienteIndice(1 + _saltosExtra);
            }

            if (!hayGanador)
                Console.WriteLine("Se alcanzó el límite de turnos. Revisar reglas o lógica.");
        }

        public CartaUno RobarCarta(JugadorUno jugador)
        {
            var c = RobarDelMazo();
            if (c != null)
                jugador.AgregarCarta(c);
            return c;
        }

        public void JugarCarta(JugadorUno jugador, CartaUno carta, ColorUno? colorElegido)
        {
            if (carta == null) return;
            if (!jugador.ContieneCarta(carta)) return;

            var superior = CartaSuperior;
            if (!carta.PuedeJugarseSobre(superior, _colorActual))
                return;

            jugador.QuitarCarta(carta);
            _descarte.Add(carta);

            if (carta.Tipo == TipoCartaUno.Comodin || carta.Tipo == TipoCartaUno.Mas4)
            {
                _colorActual = (colorElegido == null || colorElegido == ColorUno.Comodines)
                    ? ColorAleatorio()
                    : colorElegido.Value;
                Console.WriteLine($"Nuevo color elegido: {_colorActual}");
            }
            else
            {
                _colorActual = carta.Color;
            }

            AplicarEfecto(carta);
        }

        private void AplicarEfecto(CartaUno carta)
        {
            switch (carta.Tipo)
            {
                case TipoCartaUno.Bloqueo:
                    _saltosExtra += 1;
                    Console.WriteLine("Se bloquea al siguiente jugador.");
                    break;

                case TipoCartaUno.Reversa:
                    _direccion *= -1;
                    Console.WriteLine("Se invierte el sentido del juego.");
                    break;

                case TipoCartaUno.Mas2:
                    {
                        var victima = _jugadores[CalcularSiguienteIndice(1)];
                        Console.WriteLine($"{victima.Nombre} roba 2 cartas.");
                        RobarCarta(victima);
                        RobarCarta(victima);
                        _saltosExtra += 1;
                        break;
                    }

                case TipoCartaUno.Mas4:
                    {
                        var victima = _jugadores[CalcularSiguienteIndice(1)];
                        Console.WriteLine($"{victima.Nombre} roba 4 cartas.");
                        for (int i = 0; i < 4; i++)
                            RobarCarta(victima);
                        _saltosExtra += 1;
                        break;
                    }
            }
        }

        private int CalcularSiguienteIndice(int pasos)
        {
            int n = _jugadores.Count;
            int idx = _indiceActual;

            for (int i = 0; i < pasos; i++)
            {
                idx += _direccion;
                if (idx < 0) idx = n - 1;
                if (idx >= n) idx = 0;
            }

            return idx;
        }

        private CartaUno RobarDelMazo()
        {
            if (_mazo.CartasDisponibles() == 0)
                ReciclarDescarteEnMazo();

            if (_mazo.CartasDisponibles() == 0)
                return null;

            return _mazo.Robar();
        }

        private void ReciclarDescarteEnMazo()
        {
            if (_descarte.Count <= 1)
                return;

            var tope = _descarte[^1];
            var recicladas = _descarte.GetRange(0, _descarte.Count - 1);

            _descarte.Clear();
            _descarte.Add(tope);

            _mazo.Cartas = new List<CartaUno>(recicladas);
            _mazo.Mezclar();

            Console.WriteLine("El mazo se quedó sin cartas, se baraja la pila de descarte.");
        }

        private static ColorUno ColorAleatorio()
        {
            var colores = new[] { ColorUno.Azul, ColorUno.Rojo, ColorUno.Verde, ColorUno.Amarillo };
            return colores[_rng.Next(colores.Length)];
        }
    }
}
