public class Puntaje
{
    public string Nombre { get; set; }
    public int Puntos { get; set; }
    public DateTime FechaHora { get; set; }

    public Puntaje()
    {

    }

    public Puntaje(string nombre, int puntos, DateTime fechaHora)
    {
        Nombre = nombre;
        Puntos = puntos;
        FechaHora = fechaHora;
    }
}