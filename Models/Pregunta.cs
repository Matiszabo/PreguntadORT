public class Pregunta 
{
    public int IdPregunta { set;  get;}
    public int IdCategoria { set;  get;}
    public int IdDificultad { set;  get;}
    public string Enunciado { set;  get;}
    public string Foto { set;  get;}
    
    public Pregunta() {
        
    }

    public Pregunta(int idPregunta, int idCategoria, int idDificultad, string enunciado, string foto) {
        IdPregunta = idPregunta;
        IdCategoria = idCategoria;
        IdDificultad = idDificultad;
        Enunciado = enunciado;
        Foto = foto;
    }
}
