public class HistorialPuntos
{
    public int IdHistorial { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaHora { get; set; }
    public int PuntosObtenidos { get; set; }

    public HistorialPuntos() { }


    public HistorialPuntos(int idHistorial, int idUsuario, DateTime fechaHora, int puntosObtenidos)
    {
        IdHistorial = idHistorial;
        IdUsuario = idUsuario;
        FechaHora = fechaHora;
        PuntosObtenidos = puntosObtenidos;
    }
}
