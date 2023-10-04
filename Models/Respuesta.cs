public class Respuesta
{
    public int IdRespuesta { get; set; }
    public int IdPregunta { get; set; }
    public int Opcion { get; set; }
    public string Contenido { get; set; }
    public bool Correcta { get; set; }
    public string Foto { get; set; }


    public Respuesta() 
    { 

    }

    public Respuesta(int idRespuesta, int idPregunta, int opcion, string contenido, bool correcta, string foto)
    {
        IdRespuesta = idRespuesta;
        IdPregunta = idPregunta;
        Opcion = opcion;
        Contenido = contenido;
        Correcta = correcta;
        Foto = foto;
    }
}
