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

namespace WpfPelialue
{
    /// <summary>
    /// Interaction logic for PelialueControl.xaml
    /// </summary>
    public partial class PelialueControl : Grid
    {
        public PelialueControl()
        {
            InitializeComponent();
        }


 
        public static readonly DependencyProperty KokoProperty =
         DependencyProperty.Register(
           "Koko",
           typeof(int), // propertyn tietotyyppi
           typeof(PelialueControl), // luokka jossa property sijaitsee
           new FrameworkPropertyMetadata(0,  // propertyn oletusarvo
                new PropertyChangedCallback(OnValueChanged),  // kutsutaan propertyn arvon muuttumisen jälkeen
                new CoerceValueCallback(MuutaKokoa))); // kutsutaan ennen propertyn arvon muutosta

        // seuraavat tehtävä juuri näin. Ei mitään tarkistuksien lisäämistä
        public int Koko
        {
            get { return (int)GetValue(KokoProperty); }
            set { SetValue(KokoProperty, value); }
        }

        // tätä kutsutaan ennen laskurin muuttamista ja voidaan tässä vaiheessa
        // tehdä tarkistuksia ja muuttaa laskuriin asetettavaa arvoa
        private static object MuutaKokoa(DependencyObject element, object value)
        {
            int luku = (int)value;
            if (luku < 0) value = 0;

            return luku;
        }

        // Laskuria on muutettu. Päivitetään tieto myös textboxiin
        // parempi olisi bindata textbox tähän propertyyn:
        // {Binding ElementName=IkkunanNimi, Path=Laskuri}
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            PelialueControl pa = (PelialueControl)obj;
            pa.ColumnDefinitions.Clear();
            pa.RowDefinitions.Clear();


            Ellipse el = new Ellipse();
            el.Fill = Brushes.Black;
            pa.Children.Add(el);

            for (int i = 0; i < pa.Koko; i++)
            {
                // Create Columns
                ColumnDefinition gridCol1 = new ColumnDefinition();
                pa.ColumnDefinitions.Add(gridCol1);

                // Create Rows
                RowDefinition gridRow1 = new RowDefinition();
                pa.RowDefinitions.Add(gridRow1);
            }
            
            for (int i = 0; i < pa.Koko; i++)
            {
                for (int j = 0; j < pa.Koko; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.Aqua;
                    rect.Stroke = Brushes.Black;
                    rect.SetValue(Grid.RowProperty, i);
                    rect.SetValue(Grid.ColumnProperty, j);
                    pa.Children.Add(rect);
                }
            } 
            
        }

    }
}
