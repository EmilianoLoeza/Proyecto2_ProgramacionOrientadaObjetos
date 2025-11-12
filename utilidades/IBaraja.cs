namespace Uno
{
    // Interfaz genérica para cualquier baraja de cartas.
    public interface IBaraja<T>
    {
        void Generar();
        void Mezclar();
        T Robar();
        int CartasDisponibles();
    }
}
