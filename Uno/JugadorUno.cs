using System;
using System.Collections.Generic;

namespace Uno
{
    public abstract class JugadorUno
    {
        public string Nombre { get; }
        protected readonly IComportamientoUno comportamiento;
        protected readonly List<CartaUno> mano = new List<CartaUno>();

        protected JugadorUno(string nombre, IComportamientoUno comportamiento)
        {
            Nombre = nombre;
            this.comportamiento = comportamiento;
        }

        public IReadOnlyList<CartaUno> Mano => mano.AsReadOnly();
        public int CartasEnMano => mano.Count;

        public void AgregarCarta(CartaUno carta)
        {
            if (carta != null) mano.Add(carta);
        }

        public void QuitarCarta(CartaUno carta)
        {
            mano.Remove(carta);
        }

        public bool ContieneCarta(CartaUno carta) => mano.Contains(carta);

        public void MostrarMano()
        {
            Console.Write($"Mano de {Nombre}: ");
            for (int i = 0; i < mano.Count; i++)
            {
                Console.Write(mano[i]);
                if (i < mano.Count - 1) Console.Write(", ");
            }
            Console.WriteLine();
        }

        public void TomarTurno(JuegoUno juego)
        {
            var jugables = new List<CartaUno>();
            foreach (var c in mano)
                if (c.PuedeJugarseSobre(juego.CartaSuperior, juego.ColorActual))
                    jugables.Add(c);

            int indice = comportamiento.ElegirIndiceCarta(juego, this, jugables);

            if (indice >= 0 && indice < jugables.Count)
            {
                var carta = jugables[indice];
                ColorUno? colorElegido = null;

                if (carta.Tipo == TipoCartaUno.Comodin || carta.Tipo == TipoCartaUno.Mas4)
                    colorElegido = comportamiento.ElegirColor(juego, this);

                Console.WriteLine($"{Nombre} juega {carta}");
                juego.JugarCarta(this, carta, colorElegido);
            }
            else
            {
                var robada = juego.RobarCarta(this);
                if (robada == null)
                {
                    Console.WriteLine($"{Nombre} intenta robar pero no hay cartas.");
                    return;
                }

                Console.WriteLine($"{Nombre} roba {robada}");

                if (robada.PuedeJugarseSobre(juego.CartaSuperior, juego.ColorActual))
                {
                    Console.WriteLine($"{Nombre} juega la carta robada: {robada}");
                    juego.JugarCarta(this, robada, null);
                }
                else
                {
                    Console.WriteLine($"{Nombre} no puede jugar la carta robada.");
                }
            }

            if (CartasEnMano == 1)
                Console.WriteLine($"{Nombre}: UNO!");
        }
    }

    public class JugadorAleatorio : JugadorUno
    {
        public JugadorAleatorio(string nombre) : base(nombre, new ComportamientoAleatorio()) { }
    }

    public class JugadorCalculador : JugadorUno
    {
        public JugadorCalculador(string nombre) : base(nombre, new ComportamientoCalculador()) { }
    }
}