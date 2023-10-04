public static class Juego
{
    private static string _username = "";
    private static int _puntajeActual;
    private static int _cantidadPreguntasCorrectas = 0;
    private static List<Pregunta> _preguntas = new List<Pregunta>();
    private static List<Respuesta> _respuestas = new List<Respuesta>();
    private static List<Puntaje> _puntajes = new List<Puntaje>();
    private static int _vidas = 3;

    public static int ObtenerVidas()
    {
        return _vidas;
    }

    public static void RestarVida()
    {
        _vidas--;
    }

    public static void ReiniciarVidas()
    {
        _vidas = 3; 
    }

    public static List<Pregunta> preguntas
    {
        get { return _preguntas; }

    }

    public static string username
    {
        get { return _username; }
        set { _username = value; }
    }

    public static int PuntajeActual
    {
        get { return _puntajeActual; }
        set { _puntajeActual = value; }
    }

    public static int CantidadPreguntasCorrectas
    {
        get { return _cantidadPreguntasCorrectas; }
        set { _cantidadPreguntasCorrectas = value; }
    }

    public static List<Puntaje> Puntajes
    {
        get { return _puntajes; }
        set { _puntajes = value; }
    }

    public static void InicializarJuego()
    {
        _username = "";
        _puntajeActual = 0;
        _cantidadPreguntasCorrectas = 0;
        _preguntas.Clear();
        _respuestas.Clear();
    }

    public static List<Categoria> ObtenerCategorias()
    {
        return BD.ObtenerCategorias();
    }

    public static List<Dificultad> ObtenerDificultades()
    {
        return BD.ObtenerDificultades();
    }

    public static void CargarPartida(string username, int dificultad, int categoria)
    {

        _preguntas = BD.ObtenerPreguntas(dificultad, categoria);
        Console.WriteLine(_preguntas.Count);
        _respuestas = BD.ObtenerRespuestas(_preguntas);
        _username = username;
    }


    public static Pregunta ObtenerProximaPregunta()
    {
        if (_preguntas.Count() != 0)
    {
        Random random = new Random();
        int preguntaAleatoria = random.Next(0, _preguntas.Count);
        Pregunta preguntaSeleccionada = _preguntas[preguntaAleatoria];

        if (_preguntas.Count > 7)
        {
            _preguntas.RemoveAt(preguntaAleatoria);
        }

        return preguntaSeleccionada;
    }
    else
    {
        return null;
    }
    }



    public static List<Respuesta> ObtenerProximasRespuestas(int idPregunta)
    {
        List<Respuesta> listaObtenerProximasRespuestas = new List<Respuesta>();

        foreach (Respuesta recorrerRespuestas in _respuestas)
        {

            if (recorrerRespuestas.IdPregunta == idPregunta)
            {
                listaObtenerProximasRespuestas.Add(recorrerRespuestas);
            }

        }

        return listaObtenerProximasRespuestas;
    }

    public static string ObtenerRespuestaCorrecta(int idPregunta)
{
    string respuestaCorrecta = string.Empty;

    foreach (Respuesta recorrerRespuestas in Juego.ObtenerProximasRespuestas(idPregunta))
            {
                if (recorrerRespuestas.Correcta)
                {
                    respuestaCorrecta = recorrerRespuestas.Contenido;
                    break;
                }
            }
    return respuestaCorrecta;
}

    public static bool VerificarRespuesta(int idPregunta, int idRespuesta, int idDificultad)
    {
        for (int i = 0; i < _preguntas.Count(); i++)
        {
            if (_preguntas[i].IdPregunta == idPregunta)
            {
                _preguntas.RemoveAt(i);
            }
        }

        foreach (Respuesta recorrerRespuestas in _respuestas)
        {
            if (recorrerRespuestas.IdRespuesta == idRespuesta)
            {
                if (recorrerRespuestas.Correcta == true)
                {
                    switch (idDificultad)
                    {
                        case 1:
                            _puntajeActual += 100;
                            break;
                        case 2:
                            _puntajeActual += 200;
                            break;
                        case 3:
                            _puntajeActual += 300;
                            break;
                    }
                    _cantidadPreguntasCorrectas++;
                    return true;
                }
            }
        }

        return false;
    }
    
}
