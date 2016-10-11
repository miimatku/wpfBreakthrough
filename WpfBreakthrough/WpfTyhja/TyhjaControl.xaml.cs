using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfTyhja
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TyhjaControl : Button
    {
        private int column;
        private int row;


        public TyhjaControl()
        {
            InitializeComponent();
        }


        public int Column
        {
            get { return column; }
            set { column = value; }
        }

        public int Row
        {
            get { return row; }
            set { row = value; }
        }


        public static readonly DependencyProperty VariProperty =
        DependencyProperty.Register(
          "Vari",
          typeof(Brush), // propertyn tietotyyppi
          typeof(TyhjaControl), // luokka jossa property sijaitsee
          new FrameworkPropertyMetadata(null,  // propertyn oletusarvo
               new PropertyChangedCallback(OnValueChanged),  // kutsutaan propertyn arvon muuttumisen jälkeen
               new CoerceValueCallback(MuutaVaria))); // kutsutaan ennen propertyn arvon muutosta

        // seuraavat tehtävä juuri näin. Ei mitään tarkistuksien lisäämistä
        public Brush Vari
        {
            get { return (Brush)GetValue(VariProperty); }
            set { SetValue(VariProperty, value); }
        }

        // tätä kutsutaan ennen laskurin muuttamista ja voidaan tässä vaiheessa
        // tehdä tarkistuksia ja muuttaa laskuriin asetettavaa arvoa
        private static object MuutaVaria(DependencyObject element, object value)
        {
            Brush vari = (Brush)value;
            return vari;
        }

        // Laskuria on muutettu. Päivitetään tieto myös textboxiin
        // parempi olisi bindata textbox tähän propertyyn:
        // {Binding ElementName=IkkunanNimi, Path=Laskuri}
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

        }
    }
}
