using System;
using System.Threading.Tasks;
using System.Windows.Input;
using PerezEvaluacionProgreso.Models;
using PerezEvaluacionProgreso.Services;

namespace PerezEvaluacionProgreso.ViewModels
{
    public class BuscarPeliculaJPViewModel : BaseJPViewModel
    {
        private readonly PeliculaJPService _peliculaService;
        private string _query;
        private PeliculaJP? _peliculaEncontrada;
        private string _mensajeError;

        public BuscarPeliculaJPViewModel()
        {
            _peliculaService = new PeliculaJPService();
            BuscarCommand = new Command(async () => await BuscarPeliculaAsync());
        }

        public string Query
        {
            get => _query;
            set => SetProperty(ref _query, value);
        }

        public PeliculaJP? PeliculaEncontrada
        {
            get => _peliculaEncontrada;
            set => SetProperty(ref _peliculaEncontrada, value);
        }

        public string MensajeError
        {
            get => _mensajeError;
            set => SetProperty(ref _mensajeError, value);
        }

        public ICommand BuscarCommand { get; }

        private async Task BuscarPeliculaAsync()
        {
            MensajeError = string.Empty;
            PeliculaEncontrada = null;

            if (string.IsNullOrWhiteSpace(Query))
            {
                MensajeError = "Por favor, ingresa un nombre para buscar.";
                return;
            }

            var pelicula = await _peliculaService.BuscarPeliculaAsync(Query);
            if (pelicula != null)
            {
                PeliculaEncontrada = pelicula;
            }
            else
            {
                MensajeError = "No se encontró ninguna película con ese nombre.";
            }
        }
    }
}
