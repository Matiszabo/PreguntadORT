public class Categoria
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; }
    public string Foto { get; set; }


    public Categoria() 
    { 

    }

    public Categoria(int idCategoria, string nombre, string foto)
    {
        IdCategoria = idCategoria;
        Nombre = nombre;
        Foto = foto;
    }
}
