using System.Data.SqlClient;
using Dapper;


public static class BD
{
    private static string _ConnectionString = @"Server=localhost; DataBase=PreguntadOrt; Trusted_Connection=True;";


    private static List<Categoria> _ListadoCategorias = new List<Categoria>();
    private static List<Respuesta> _ListaRespuestas = new List<Respuesta>();
    private static List<Puntaje> _ListaPuntajes = new List<Puntaje>();


    public static List<Categoria> ObtenerCategorias()
    {
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            string SQL = "SELECT * From Categorias";
            _ListadoCategorias = db.Query<Categoria>(SQL).ToList();
        }
        return _ListadoCategorias;
    }


    private static List<Dificultad> _ListadoDificultades = new List<Dificultad>();


    public static List<Dificultad> ObtenerDificultades()
    {
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            string SQL = "SELECT * FROM Dificultades";
            _ListadoDificultades = db.Query<Dificultad>(SQL).ToList();
        }
        return _ListadoDificultades;
    }

    public static List<Pregunta> ObtenerPreguntas(int IdDificultad, int IdCategoria)
    {
        string SQL = "SELECT * FROM Preguntas WHERE (IdCategoria = @IdCategoria OR @IdCategoria = -1) AND (IdDificultad = @IdDificultad OR @IdDificultad = -1)";

        List<Pregunta> _ListadoPreguntas = new List<Pregunta> { };
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            _ListadoPreguntas = db.Query<Pregunta>(SQL, new { IdCategoria = IdCategoria, IdDificultad = IdDificultad }).ToList();
        }
        return _ListadoPreguntas;
    }


    public static List<Respuesta> ObtenerRespuestas(List<Pregunta> preguntas)
    {

        List<Respuesta> _ListadoRespuestas = new List<Respuesta>();

        foreach (Pregunta recorrerPreguntas in preguntas)
        {
            string SQL = "SELECT * FROM Respuestas WHERE IdPregunta = @pIdPregunta";
            using (SqlConnection db = new SqlConnection(_ConnectionString))
            {
                _ListadoRespuestas.AddRange(db.Query<Respuesta>(SQL, new { pIdPregunta = recorrerPreguntas.IdPregunta }));
            }
        }
        return _ListadoRespuestas;
    }

    public static void InsertarPuntaje(Puntaje puntaje)
    {
        string SQL = "INSERT INTO Puntajes(Nombre,Puntos,FechaHora) VALUES (@pUsername, @pPuntaje, @pFechaHora)";
            using(SqlConnection db = new SqlConnection(_ConnectionString))
            {
                db.Execute(SQL, new{pUsername = puntaje.Nombre, pPuntaje = puntaje.Puntos, pFechaHora = puntaje.FechaHora});
            }
    }

    public static List<Puntaje> ObtenerHistorialPuntos()
{
    using (SqlConnection db = new SqlConnection(_ConnectionString))
    {
        string SQL = "SELECT * FROM Puntajes ORDER BY Puntos DESC";
        return db.Query<Puntaje>(SQL).ToList();
    }
}


    public static void AgregarPregunta(Pregunta preg)
    {
        string SQL = "INSERT INTO Preguntas(IdCategoria, IdDificultad, Enunciado, Foto) VALUES (@pCategoria, @pDificultad, @pEnunciado, @pfoto); ";
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            db.Execute(SQL, new { pCategoria = preg.IdCategoria, pDificultad = preg.IdDificultad, pEnunciado = preg.Enunciado, pfoto = preg.Foto });
        }
    }

    public static void AgregarRespuesta(Respuesta resp)
    {
        string SQL = "INSERT INTO Respuestas(IdPregunta, Opcion, Contenido, Correcta) VALUES (@pIdPregunta, @pOpcion, @pContenido, @pCorrecta)";
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            db.Execute(SQL, new { pIdPregunta = resp.IdPregunta, pOpcion = resp.Opcion, pContenido = resp.Contenido, pCorrecta = resp.Correcta });
        }
    }

    public static List<Respuesta> ObtenerRespuestasPorPregunta(int IdPregunta)
    {
        string SQL = "SELECT * FROM Respuestas WHERE IdPregunta = @pIdPregunta;";
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            return db.Query<Respuesta>(SQL, new { pIdPregunta = IdPregunta }).ToList();
        }
    }

    public static void EliminarRespuesta(int IdPregunta)
    {
        string SQL = "DELETE FROM Respuestas WHERE IdPregunta = @pIdPregunta;";
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            db.Execute(SQL, new { pIdPregunta = IdPregunta });
        }
    }

    public static void EliminarPregunta(int IdPregunta)
    {
        EliminarRespuesta(IdPregunta);

        string SQL = "DELETE FROM Preguntas WHERE IdPregunta = @pIdPregunta;";
        using (SqlConnection db = new SqlConnection(_ConnectionString))
        {
            db.Execute(SQL, new { pIdPregunta = IdPregunta });
        }
    }

    public static void RegistrarHistorialPuntos(int idUsuario, int puntosObtenidos)
{
    using (SqlConnection db = new SqlConnection(_ConnectionString))
    {
        string SQL = "INSERT INTO HistorialPuntos (IdUsuario, FechaHora, PuntosObtenidos) " +
                     "VALUES (@IdUsuario, @FechaHora, @PuntosObtenidos)";
        db.Execute(SQL, new { IdUsuario = idUsuario, FechaHora = DateTime.Now, PuntosObtenidos = puntosObtenidos });
    }
}



}
