using Newtonsoft.Json;
using PerezEvaluacionProgreso.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

public class BuscarPeliculaJPViewModel : INotifyPropertyChanged
{
    private string _query;
    private PeliculaJP _peliculaEncontrada;
    private string _mensajeError;

    public event PropertyChangedEventHandler PropertyChanged;

    public string Query
    {
        get => _query;
        set
        {
            if (_query != value)
            {
                _query = value;
                OnPropertyChanged();
            }
        }
    }
    public PeliculaJP PeliculaEncontrada
    {
        get => _peliculaEncontrada;
        set
        {
            if (_peliculaEncontrada != value)
            {
                _peliculaEncontrada = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HayPelicula));
            }
        }
    }
    public string MensajeError
    {
        get => _mensajeError;
        set
        {
            if (_mensajeError != value)
            {
                _mensajeError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HayError));
            }
        }
    }
    private bool _hayError;
    public bool HayError
    {
        get => _hayError;
        set
        {
            _hayError = value;
            OnPropertyChanged();
        }
    }

    private bool _hayPelicula;
    public bool HayPelicula
    {
        get => _hayPelicula;
        set
        {
            _hayPelicula = value;
            OnPropertyChanged();
        }
    }
    public Command BuscarPeliculaCommand { get; }
    public ICommand BuscarCommand { get; }
    public BuscarPeliculaJPViewModel()
    {
        BuscarPeliculaCommand = new Command(async () => await BuscarPeliculaAsync());
    }

    public async Task BuscarPeliculaAsync()
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            MensajeError = "Por favor, ingresa el nombre de una película.";
            HayError = true;
            HayPelicula = false;
            return;
        }

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://www.freetestapi.com/api/v1/movies?title=The%20Shawshank%20Redemption");
            Console.WriteLine(response);
            var peliculas = JsonConvert.DeserializeObject<List<PeliculaJP>>(response);

            if (peliculas == null || peliculas.Count == 0)
            {
                MensajeError = "No se han encontrado películas en la base de datos.";
                HayError = true;
                HayPelicula = false;
                return;
            }

            var peliculaEncontrada = peliculas.FirstOrDefault(p =>
                p.Nombre.Equals(Query, StringComparison.OrdinalIgnoreCase));

            if (peliculaEncontrada != null)
            {
                PeliculaEncontrada = peliculaEncontrada;
                HayError = false;
                HayPelicula = true;
            }
            else
            {
                MensajeError = "No se encontró ninguna película con ese nombre.";
                HayError = true;
                HayPelicula = false;
            }
        }
        catch (Exception ex)
        {
            MensajeError = $"Error al buscar la película: {ex.Message}";
            HayError = true;
            HayPelicula = false;
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}