using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PreguntadORT.Models;


namespace PreguntadORT.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;


    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    public IActionResult Index()
    {
        return View("Index");
    }


    public IActionResult ConfigurarJuego()
    {
        Juego.InicializarJuego();
        Juego.ReiniciarVidas();


        List<Categoria> categorias = Juego.ObtenerCategorias();
        List<Dificultad> dificultades = Juego.ObtenerDificultades();


        ViewBag.Categorias = categorias;
        ViewBag.Dificultades = dificultades;


        return View("ConfigurarJuego");
    }


    public IActionResult Comenzar(string username, int dificultad, int categoria)
    {
        if (categoria == -1)
        {
            categoria = -1;
        }
        if (dificultad == -1)
        {
            dificultad = -1;
        }

        Juego.CargarPartida(username, dificultad, categoria);

        return RedirectToAction("Jugar", "Home");
    }


    public IActionResult Jugar()
    {
        ViewBag.Username = Juego.username;
        ViewBag.Puntaje = Juego.PuntajeActual;

        Pregunta PreguntaActual = Juego.ObtenerProximaPregunta();
        ViewBag.EsUltimaPregunta = (Juego.ObtenerProximaPregunta() == null);

        if (PreguntaActual == null)
        {
            DateTime fechaHora = DateTime.Now;
            Puntaje puntaje = new Puntaje(Juego.username, Juego.PuntajeActual, fechaHora);
            BD.InsertarPuntaje(puntaje);
            return View("Fin");
        }

        List<Respuesta> respuestas = Juego.ObtenerProximasRespuestas(PreguntaActual.IdPregunta);
        ViewBag.Foto = PreguntaActual.Foto;
        ViewBag.Dificultad = PreguntaActual.IdDificultad;
        ViewBag.Enunciado = PreguntaActual.Enunciado;
        ViewBag.Pregunta = PreguntaActual;
        ViewBag.Respuestas = respuestas;

        return View("Jugar");
    }

    public IActionResult HistorialPuntaje()
    {
        ViewBag.Score = BD.ObtenerHistorialPuntos();
        return View("HistorialPuntaje");
    }

    public IActionResult ListaPreguntas()
    {
        ViewBag.Lista = BD.ObtenerPreguntas(-1, -1);
        return View();
    }

    public IActionResult VerificarRespuesta(int idPregunta, int idRespuesta, int idDificultad)
    {
        Pregunta pregunta = Juego.ObtenerProximaPregunta();
        List<Respuesta> _ListaRespuestas = Juego.ObtenerProximasRespuestas(pregunta.IdPregunta);

        if (idRespuesta == 0)
        {
            ViewBag.Puntaje = Juego.PuntajeActual;
            ViewBag.Resultado = "La respuesta es Incorrecta";

            Juego.RestarVida();

            if (Juego.ObtenerVidas() <= 0)
            {
                return View("Derrota");
            }

            ViewBag.RespuestaCorrecta = Juego.ObtenerRespuestaCorrecta(idPregunta);
        }
        else if (Juego.VerificarRespuesta(idPregunta, idRespuesta, idDificultad))
        {
            ViewBag.Puntaje = Juego.PuntajeActual;
            ViewBag.Resultado = "La respuesta es Correcta!";
        }
        else
        {
            ViewBag.Puntaje = Juego.PuntajeActual;
            ViewBag.Resultado = "La respuesta es Incorrecta!";

            Juego.RestarVida();

            if (Juego.ObtenerVidas() <= 0)
            {
                return View("Derrota");
            }

            ViewBag.RespuestaCorrecta = Juego.ObtenerRespuestaCorrecta(idPregunta);
        }

        ViewBag.ContenidoRespuesta = _ListaRespuestas;
        ViewBag.ContenidoPregunta = pregunta;

        if (Juego.ObtenerProximaPregunta() == null)
        {
            return View("Fin");
        }

        return View("Respuesta");
    }


    public IActionResult RespuestaTiempoAgotado(int idPregunta)
    {
        ViewBag.Resultado = "La respuesta es Incorrecta!";
        ViewBag.Puntaje = Juego.PuntajeActual;
        Juego.RestarVida();

        if (Juego.ObtenerVidas() <= 0)
        {
            return View("Derrota");
        }

        ViewBag.RespuestaCorrecta = Juego.ObtenerRespuestaCorrecta(idPregunta);

        return View("Respuesta");
    }

    public IActionResult AgregarPreguntas(int IdPregunta, int IdDificultad, int IdCategoria, int IdRespuesta)
    {
        ViewBag.IdPregunta = IdPregunta;
        ViewBag.ListaCategoria = Juego.ObtenerCategorias();
        ViewBag.ListaDificultad = Juego.ObtenerDificultades();
        return View();
    }
    public IActionResult AgregarRespuestas(int IdPregunta)
    {
        ViewBag.IdPregunta = IdPregunta;
        return View();
    }

    [HttpPost]
    public IActionResult GuardarPregunta(int IdCategoria, int IdDificultad, string Enunciado, string Foto)
    {
        if (ModelState.IsValid)
        {
            if (string.IsNullOrEmpty(Foto))
            {
                ModelState.AddModelError("Foto", "Debe seleccionar una foto.");
                ViewBag.ListaCategoria = Juego.ObtenerCategorias();
                ViewBag.ListaDificultad = Juego.ObtenerDificultades();
                return View("AgregarPreguntas");
            }

            var nuevaPregunta = new Pregunta
            {
                IdCategoria = IdCategoria,
                IdDificultad = IdDificultad,
                Enunciado = Enunciado,
                Foto = Foto
            };

            BD.AgregarPregunta(nuevaPregunta);

            return RedirectToAction("ListaPreguntas");
        }

        ViewBag.ListaCategoria = Juego.ObtenerCategorias();
        ViewBag.ListaDificultad = Juego.ObtenerDificultades();
        return View("AgregarPreguntas");
    }

    [HttpPost]
    public IActionResult GuardarRespuesta(int IdPregunta, string Contenido1, int Opcion1, int Correcta1, string Contenido2, int Opcion2, int Correcta2, string Contenido3, int Opcion3, int Correcta3, string Contenido4, int Opcion4, int Correcta4)
    {
        if (ModelState.IsValid)
        {
            var respuesta1 = new Respuesta
            {
                IdPregunta = IdPregunta,
                Opcion = Opcion1,
                Contenido = Contenido1,
                Correcta = (Correcta1 == 1)
            };
            BD.AgregarRespuesta(respuesta1);

            var respuesta2 = new Respuesta
            {
                IdPregunta = IdPregunta,
                Opcion = Opcion2,
                Contenido = Contenido2,
                Correcta = (Correcta2 == 1)
            };
            BD.AgregarRespuesta(respuesta2);

            var respuesta3 = new Respuesta
            {
                IdPregunta = IdPregunta,
                Opcion = Opcion3,
                Contenido = Contenido3,
                Correcta = (Correcta3 == 1)
            };
            BD.AgregarRespuesta(respuesta3);

            var respuesta4 = new Respuesta
            {
                IdPregunta = IdPregunta,
                Opcion = Opcion4,
                Contenido = Contenido4,
                Correcta = (Correcta4 == 1)
            };
            BD.AgregarRespuesta(respuesta4);

            return RedirectToAction("ListaPreguntas");
        }

        return View("AgregarRespuestas", new { IdPregunta });
    }


    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Fin()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult EliminarPregunta(int IdPregunta)
{
    var respuestas = BD.ObtenerRespuestasPorPregunta(IdPregunta);

    // Eliminar las respuestas
    foreach (var respuesta in respuestas)
    {
        BD.EliminarRespuesta(respuesta.IdRespuesta);
    }

    BD.EliminarPregunta(IdPregunta);

    return RedirectToAction("ListaPreguntas");
}

}
