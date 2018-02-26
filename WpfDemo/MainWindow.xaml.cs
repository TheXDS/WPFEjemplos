using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfDemo.Annotations;

namespace WpfDemo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
	    /// <summary>
	    /// Esta lista contiene elementos de ejemplo para un TreeView.
	    /// </summary>
        public ObservableCollection<ArbolEjemplo> EjemploDeArbol { get; set; } = new ObservableCollection<ArbolEjemplo>
        {
            new ArbolEjemplo
            {
                Id = 1,
                Nombre = "Elemento 1",
                Telefonos = new ObservableCollection<string>
                {
                    "2234-5678",
                    "2298-7654",
                    "2212-3456",
                }
            },
            new ArbolEjemplo
            {
                Id = 1,
                Nombre = "Elemento 2",
                Telefonos = new ObservableCollection<string>
                {
                    "9876-5432",
                    "3456-7890",
                    "8901-2345",
                }
            },
        };
		
	    /// <summary>
	    /// Esta lista contiene elementos de ejemplo para un ListView.
	    /// </summary>
        public ObservableCollection<ListaEjemplo> EjemploDeLista { get; set; } = new ObservableCollection<ListaEjemplo>
        {
            new ListaEjemplo{ Id=1,Nombre="Nombre 1"},
            new ListaEjemplo{ Id=2,Nombre="Nombre 2"},
            new ListaEjemplo{ Id=3,Nombre="Nombre 3"},
            new ListaEjemplo{ Id=4,Nombre="Nombre 4"},
        };

	    /// <summary>
	    /// Inicializa una nueva instancia de la clase <see cref="mainWindow"/>
	    /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

	    /// <summary>
	    /// Controla la acción del botón de nuevo elemento.
	    /// </summary>
        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
			// Nótese que, únicamente es necesario agregar al elemento a la
			// Lista, INotifyPropertyChanged se encarga de comunicar el cambio
			// al control que muestra la lista.
            EjemploDeLista.Add(new ListaEjemplo { Id = EjemploDeLista.Count + 1, Nombre = "Nuevo Item" });
        }

	    /// <summary>
	    /// Controla la acción del botón de editar.
	    /// </summary>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
			// Este if comprueba que el elemento seleccionado del ListView sea
			// del tipo ListaEjemplo, y aprovecha a declararlo...
			// ... Bonita sintáxis...
            if (Lst.SelectedItem is ListaEjemplo l)
            {
				// Editar directamente el elemento sin preocupaciones.
				// INotifyPropertyChanged se encarga de informar el cambio al
				// control correspondiente.
                l.Nombre = "Editado!";
            }
        }

	    /// <summary>
	    /// Controla la acción del botón de borrar.
	    /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
			// Este if comprueba que el elemento seleccionado del ListView sea
			// del tipo ListaEjemplo, y aprovecha a declararlo...
            if (Lst.SelectedItem is ListaEjemplo l)
            {
				// Borrar el elemento de la colección. INotifyPropertyChanged
				// se encarga de notificar al control del cambio. No es
				// necesario hacer nada más.
                EjemploDeLista.Remove(l);
            }
        }
    }

	
    /// <summary>
    /// Clase base que implementa INotifyProPertyChanged.
    /// </summary>
	/// <remarks>
	/// Es útil tener esta clase base, ya que permite compartir por ejemplo,
	/// un campo de Id para Entity Framework, además que permite implementar
	/// una única vez la interfaz INotifyPropertyChanged.
	/// </remarks>
    public class ModelBase<T> : INotifyPropertyChanged where T : struct
    {
		/*
		 * Una implementación correcta de INotifyPpropertyChanged tiene a sus
		 * propiedades soportadas por campos privados.
		 */
        private T _id;
        public event PropertyChangedEventHandler PropertyChanged;

		
	    /// <summary>
	    /// Obtiene o establece el Id de la entidad.
	    /// </summary>
        public T Id
        {
            get => _id;
            set
            {
				// Este if únicamente comprueba que el valor realmente haya
				// cambiado.
                if (value.Equals(_id)) return;
				
				// Establece el valor de la propiedad.
                _id = value;
				
				// Esta llamada es necesaria para informarle a WPF que esta
				// propiedad ha cambiado de valor.
                OnPropertyChanged();
            }
        }

        /*
         * ReSharper agrega ciertos atributos de forma automática cuando se
         * crea una clase que implementa INotifyPropertyChanged, por lo que
         * aparecerán algunos decoradores. Los mismos no son necesarios para el
         * correcto funcionamiento de INotifyPropertyChanged, a excepción de
         * CallerMemberName en la función a continuación.
         */

	    /// <summary>
	    /// No hay mucho que ver aquí. esta función notifica a WPF del cambio
		/// de una propiedad.
	    /// </summary>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

	
    /// <summary>
    /// Modelo de ejemplo para un TreeView.
    /// </summary>
    public class ArbolEjemplo : ModelBase<int>
    {
		/*
		 * Una implementación correcta de INotifyPpropertyChanged tiene a sus
		 * propiedades soportadas por campos privados.
		 */
        private ObservableCollection<string> _telefonos = new ObservableCollection<string>();
        private string _nombre;

        public string Nombre
        {
            get => _nombre;
            set
            {
				// Este if únicamente comprueba que el valor realmente haya
				// cambiado.
                if (value == _nombre) return;

				// Establece el valor de la propiedad.
                _nombre = value;

				// Esta llamada es necesaria para informarle a WPF que esta
				// propiedad ha cambiado de valor.
                OnPropertyChanged();
            }
        }

        public virtual ObservableCollection<string> Telefonos
        {
            get => _telefonos;
            set
            {
				// Este if únicamente comprueba que el valor realmente haya
				// cambiado.
                if (Equals(value, _telefonos)) return;

				// Establece el valor de la propiedad.
                _telefonos = value;

				// Esta llamada es necesaria para informarle a WPF que esta
				// propiedad ha cambiado de valor.
                OnPropertyChanged();
            }
        }

        public ArbolEjemplo()
        {
            _telefonos.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Telefonos));
        }
    }

    public class ListaEjemplo : ModelBase<int>
    {
		/*
		 * Una implementación correcta de INotifyPpropertyChanged tiene a sus
		 * propiedades soportadas por campos privados.
		 */
        private string _nombre;

        public string Nombre
        {
            get => _nombre;
            set
            {
                if (value == _nombre) return;
                _nombre = value;
                OnPropertyChanged();
            }
        }
    }
}
