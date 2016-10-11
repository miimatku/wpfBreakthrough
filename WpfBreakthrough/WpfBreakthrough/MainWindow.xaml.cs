using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfNappula;
using WpfNappulaValkoinen;
using WpfTyhja;

namespace WpfBreakthrough
{
    /// <summary>
    /// Breakthrough ja Tammi pelien toteutus.
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<NappulaValkoinenControl> listaV = new List<NappulaValkoinenControl>();
        private List<NappulaMustaControl> listaM = new List<NappulaMustaControl>();
        private List<TyhjaControl> buttonlistaV = new List<TyhjaControl>();
        private int rivi;
        private int sarake;
        private int valkoinen;
        private int musta;
        private Vuoro pelivuoro;
        private Peli peli;
        private Brush nappula1 = Brushes.White;
        private Brush nappula2 = Brushes.Black;
        private Brush ruutu1 = Brushes.Beige;
        private Brush ruutu2 = Brushes.Bisque;
        private string pelaaja1Nimi = "Pertti Keinonen";
        private string pelaaja2Nimi = "Sakari Östermalm";
        private Boolean onSyotavaa = false;
        private List<string> listaKoordinaatit = new List<string>();


        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Vuorot
        /// </summary>
        private enum Vuoro
        {
            Valkoinen, Musta
        }


        /// <summary>
        /// Pelivaihtoehdot
        /// </summary>
        private enum Peli
        {
            Breakthrough, Tammi
        }


        /// <summary>
        /// Arvotaan aloittaja
        /// </summary>
        private void arvoAloittaja()
        {
            Random r = new Random();
            if (r.Next(0, 2) == 0) pelivuoro = Vuoro.Valkoinen;
            else pelivuoro = Vuoro.Musta;
        }


        /// <summary>
        /// Uuden pelin alustus
        /// </summary>
        private void ResetGame()
        {
            pelialue.IsEnabled = true;
            listaM.Clear();
            listaV.Clear();
            buttonlistaV.Clear();
            pelivuoro = Vuoro.Valkoinen;
            if (peli == Peli.Tammi) arvoAloittaja();
            onSyotavaa = false;
            listaKoordinaatit.Clear();

            # region Breakthrough kentan alustus
            if (peli == Peli.Breakthrough)
            {
                for (int i = 0; i < pelialue.Koko; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        TyhjaControl tyhja = new TyhjaControl();
                        tyhja.SetValue(Grid.RowProperty, i);
                        tyhja.SetValue(Grid.ColumnProperty, j);
                        tyhja.Row = i;
                        tyhja.Column = j;
                        tyhja.Vari = ruutu1;
                        pelialue.Children.Add(tyhja);
                        buttonlistaV.Add(tyhja);
                        tyhja.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        NappulaMustaControl nappi = new NappulaMustaControl();
                        nappi.SetValue(Grid.RowProperty, i);
                        nappi.SetValue(Grid.ColumnProperty, j);
                        nappi.Row = i;
                        nappi.Column = j;
                        nappi.Vari = nappula2;
                        pelialue.Children.Add(nappi);
                        listaM.Add(nappi);
                        nappi.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                    }
                }

                for (int i = pelialue.Koko - 2; i < pelialue.Koko; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        NappulaValkoinenControl nappi = new NappulaValkoinenControl();
                        nappi.SetValue(Grid.RowProperty, i);
                        nappi.SetValue(Grid.ColumnProperty, j);
                        nappi.Row = i;
                        nappi.Column = j;
                        nappi.Vari = nappula1;
                        pelialue.Children.Add(nappi);
                        listaV.Add(nappi);
                        nappi.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                    }
                }
            }
            # endregion

            # region Tammi alustus
            if (peli == Peli.Tammi)
            {
                for (int i = 0; i < pelialue.Koko; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        TyhjaControl tyhja = new TyhjaControl();
                        tyhja.SetValue(Grid.RowProperty, i);
                        tyhja.SetValue(Grid.ColumnProperty, j);
                        tyhja.Row = i;
                        tyhja.Column = j;
                        if ((i + j) % 2 == 0)
                        {
                            tyhja.Vari = ruutu1;
                        }
                        else
                        {
                            tyhja.Vari = ruutu2;
                        }
                        pelialue.Children.Add(tyhja);
                        buttonlistaV.Add(tyhja);
                        tyhja.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                    }
                }
                int a = (pelialue.Koko % 2) + 1;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        if ((j + a) % 2 == 0)
                        {
                            NappulaMustaControl nappi = new NappulaMustaControl();
                            nappi.Height = pelialue.ActualHeight / pelialue.ColumnDefinitions.Count * 0.6;
                            nappi.Width = pelialue.ActualWidth / pelialue.RowDefinitions.Count * 0.6;
                            nappi.SetValue(Grid.RowProperty, i);
                            nappi.SetValue(Grid.ColumnProperty, j);
                            nappi.Row = i;
                            nappi.Column = j;
                            nappi.Vari = nappula2;
                            pelialue.Children.Add(nappi);
                            listaM.Add(nappi);
                            nappi.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                        }
                    }
                    a++;
                }
                int k = 0;
                for (int i = pelialue.Koko - 3; i < pelialue.Koko; i++)
                {
                    for (int j = 0; j < pelialue.Koko; j++)
                    {
                        if ((j + k) % 2 == 0)
                        {
                            NappulaValkoinenControl nappi = new NappulaValkoinenControl();
                            nappi.Height = pelialue.ActualHeight / pelialue.ColumnDefinitions.Count * 0.6;
                            nappi.Width = pelialue.ActualWidth / pelialue.RowDefinitions.Count * 0.6;
                            nappi.SetValue(Grid.RowProperty, i);
                            nappi.SetValue(Grid.ColumnProperty, j);
                            nappi.Row = i;
                            nappi.Column = j;
                            nappi.Vari = nappula1;
                            pelialue.Children.Add(nappi);
                            listaV.Add(nappi);
                            nappi.Click += new System.Windows.RoutedEventHandler(this.Button_Click);
                        }
                    }
                    k++;
                }
            }

            # endregion
        }



        /// <summary>
        /// Kentän klikkaus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is TyhjaControl)
            {
                rivi = Grid.GetRow((TyhjaControl)e.Source);
                sarake = Grid.GetColumn((TyhjaControl)e.Source);
            }

            if (e.Source is NappulaMustaControl)
            {
                rivi = Grid.GetRow((NappulaMustaControl)e.Source);
                sarake = Grid.GetColumn((NappulaMustaControl)e.Source);
            }

            if (e.Source is NappulaValkoinenControl)
            {
                rivi = Grid.GetRow((NappulaValkoinenControl)e.Source);
                sarake = Grid.GetColumn((NappulaValkoinenControl)e.Source);
            }

            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].IsChecked == true)
                {
                    musta = i;
                }
            }

            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].IsChecked == true)
                {
                    valkoinen = i;
                }
            }

            # region Breakthrough liikkuminen
            if (peli == Peli.Breakthrough)
            {

                // Jos valkoisen pelivuoro
                if (pelivuoro == Vuoro.Valkoinen)
                {
                    if (valkoinen >= 0 && valkoinen < listaV.Count)
                    {
                        // Valkoinen ylös 
                        if (listaV[valkoinen].Column == sarake && listaV[valkoinen].Row == (rivi + 1) && (e.Source is TyhjaControl))
                        {
                            listaV[valkoinen].Row--;
                            // listaV[i].Column--;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi - 1, sarake);
                            pelivuoro = Vuoro.Musta;
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                        // VAlkoinen vasemmalle ja ylös 
                        if (listaV[valkoinen].Column == (sarake + 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is TyhjaControl))
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column--;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            pelivuoro = Vuoro.Musta;
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                        // Valkoinen oikealle ja ylös 
                        if (listaV[valkoinen].Column == (sarake - 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is TyhjaControl))
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column++;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            pelivuoro = Vuoro.Musta;
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                        // Valkoinen syö vasemmalle ja ylös
                        if (listaV[valkoinen].Column == (sarake + 1) && listaV[valkoinen].Row == (rivi + 1) && e.Source is NappulaMustaControl)
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column--;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            listaM[musta].Visibility = Visibility.Hidden;
                            pelivuoro = Vuoro.Musta;
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                        // Valkoinen syö oikealle ja ylös
                        if (listaV[valkoinen].Column == (sarake - 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is NappulaMustaControl))
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column++;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            listaM[musta].Visibility = Visibility.Hidden;
                            pelivuoro = Vuoro.Musta;
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                    }
                }
                // Musta pelivuoro
                else
                {
                    if (musta >= 0 && musta < listaM.Count)
                    {
                        // Musta alas
                        if (listaM[musta].Column == sarake && listaM[musta].Row == (rivi - 1) && (e.Source is TyhjaControl))
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            // listaM[musta].Column--;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                        // Musta alas ja oikealle 
                        if (listaM[musta].Column == (sarake - 1) && listaM[musta].Row == (rivi - 1) && (e.Source is TyhjaControl))
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column++;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                        // Musta alas ja vasemmalle 
                        if (listaM[musta].Column == (sarake + 1) && listaM[musta].Row == (rivi - 1) && (e.Source is TyhjaControl))
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column--;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                        // Musta syö alas ja oikealle 
                        if (listaM[musta].Column == (sarake - 1) && listaM[musta].Row == (rivi - 1) && (e.Source is NappulaValkoinenControl))
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column++;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            listaV[valkoinen].Visibility = Visibility.Hidden;
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                        // Musta syö alas ja vasemmalle 
                        if (listaM[musta].Column == (sarake + 1) && listaM[musta].Row == (rivi - 1) && (e.Source is NappulaValkoinenControl))
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column--;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            listaV[valkoinen].Visibility = Visibility.Hidden;
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                    }
                }
            }
            # endregion

            # region Tammi liikkuminen

            if (peli == Peli.Tammi)
            {
                if (pelivuoro == Vuoro.Valkoinen)
                {
                    if (valkoinen >= 0 && valkoinen < listaV.Count)
                    {
                        onkoSyotavaa();
                        if (valkoisenPakkoSyoda()) return;

                        // VAlkoinen vasemmalle ja ylös 
                        if (listaV[valkoinen].Column == (sarake + 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is TyhjaControl)
                            && onSyotavaa == false)
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column--;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            lisaaKoordinaatti("V", rivi, sarake, listaV[valkoinen].Row, listaV[valkoinen].Column);
                            pelivuoro = Vuoro.Musta;
                            if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            return;
                        }


                        // Valkoinen oikealle ja ylös 
                        if (listaV[valkoinen].Column == (sarake - 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is TyhjaControl)
                            && onSyotavaa == false)
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi, sarake);
                            listaV[valkoinen].Row--;
                            listaV[valkoinen].Column++;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            pelivuoro = Vuoro.Musta;
                            if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            return;
                        }

                        // Valkoinen syö vasemmalle ja ylös
                        if (listaV[valkoinen].Column == (sarake + 1) && listaV[valkoinen].Row == (rivi + 1) && e.Source is NappulaMustaControl
                            && voikoValkoinenSyodaVasen())
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi - 1, sarake - 1);
                            listaV[valkoinen].Row -= 2;
                            listaV[valkoinen].Column -= 2;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                            listaM[musta].Visibility = Visibility.Hidden;
                            listaM.RemoveAt(musta);
                            if (!onkoSyotavaa())
                            {
                                pelivuoro = Vuoro.Musta;
                                onSyotavaa = false;
                            }
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                        // Valkoinen syö oikealle ja ylös

                        if (listaV[valkoinen].Column == (sarake - 1) && listaV[valkoinen].Row == (rivi + 1) && (e.Source is NappulaMustaControl)
                            && voikoValkoinenSyodaOikea())
                        {
                            lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, rivi - 1, sarake + 1);
                            listaV[valkoinen].Row -= 2;
                            listaV[valkoinen].Column += 2;
                            listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                            listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                            listaM[musta].Visibility = Visibility.Hidden;
                            if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                            listaM.RemoveAt(musta);
                            if (!onkoSyotavaa())
                            {
                                pelivuoro = Vuoro.Musta;
                                onSyotavaa = false;
                            }
                            listaV[valkoinen].IsChecked = null;
                            valkoinen = -1;
                            musta = -1;
                            gameOver();
                            return;
                        }
                    }
                }


                if (pelivuoro == Vuoro.Musta)
                {
                    if (musta >= 0 && musta < listaM.Count)
                    {
                        onkoSyotavaa();
                        if (mustanPakkoSyoda()) return;

                        // Musta alas ja oikealle 
                        if (listaM[musta].Column == (sarake - 1) && listaM[musta].Row == (rivi - 1) && (e.Source is TyhjaControl)
                            && onSyotavaa == false)
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column++;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            pelivuoro = Vuoro.Valkoinen;
                            if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            return;
                        }
                        // Musta alas ja vasemmalle 
                        if (listaM[musta].Column == (sarake + 1) && listaM[musta].Row == (rivi - 1) && (e.Source is TyhjaControl)
                            && onSyotavaa == false)
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi, sarake);
                            listaM[musta].Row++;
                            listaM[musta].Column--;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                            pelivuoro = Vuoro.Valkoinen;
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            return;
                        }

                        // Musta syö alas ja oikealle 
                        if (listaM[musta].Column == (sarake - 1) && listaM[musta].Row == (rivi - 1) && (e.Source is NappulaValkoinenControl)
                            && voikoMustaSyodaOikea())
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi + 1, sarake + 1);
                            listaM[musta].Row += 2;
                            listaM[musta].Column += 2;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                            listaV[valkoinen].Visibility = Visibility.Hidden;
                            listaV.RemoveAt(valkoinen);
                            if (!onkoSyotavaa())
                            {
                                pelivuoro = Vuoro.Valkoinen;
                                onSyotavaa = false;
                            }
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                        // Musta syö alas ja vasemmalle 
                        if (listaM[musta].Column == (sarake + 1) && listaM[musta].Row == (rivi - 1) && (e.Source is NappulaValkoinenControl)
                            && voikoMustaSyodaVasen())
                        {
                            lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, rivi + 1, sarake - 1);
                            listaM[musta].Row += 2;
                            listaM[musta].Column -= 2;
                            listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                            listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                            if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                            listaV[valkoinen].Visibility = Visibility.Hidden;
                            listaV.RemoveAt(valkoinen);
                            if (!onkoSyotavaa())
                            {
                                pelivuoro = Vuoro.Valkoinen;
                                onSyotavaa = false;
                            }
                            listaM[musta].IsChecked = null;
                            musta = -1;
                            valkoinen = -1;
                            gameOver();
                            return;
                        }
                    }
                }
            }
            # endregion
        }



        /// <summary>
        /// Tarkistetaan onko millään nappulla mahdollisuus syödä
        /// </summary>
        private Boolean onkoSyotavaa()
        {
            Boolean on = false;
            if (pelivuoro == Vuoro.Valkoinen)
            {
                for (int j = 0; j < listaV.Count; j++)
                {
                    for (int i = 0; i < listaM.Count; i++)
                    {
                        if (listaM[i].Row + 1 == listaV[j].Row && listaM[i].Column + 1 == listaV[j].Column && voikoValkoinenSyodaVasen())
                        {
                            onSyotavaa = true;
                            on = true;
                        }
                        if (listaM[i].Row + 1 == listaV[j].Row && listaM[i].Column - 1 == listaV[j].Column && voikoValkoinenSyodaOikea())
                        {
                            onSyotavaa = true;
                            on = true;
                        }
                    }
                }
            }
            if (pelivuoro == Vuoro.Musta)
            {
                for (int j = 0; j < listaM.Count; j++)
                {
                    for (int i = 0; i < listaV.Count; i++)
                    {
                        if (listaV[i].Row - 1 == listaM[j].Row && listaV[i].Column + 1 == listaM[j].Column && voikoMustaSyodaVasen())
                        {
                            onSyotavaa = true;
                            on = true;
                        }
                        if (listaV[i].Row - 1 == listaM[j].Row && listaV[i].Column - 1 == listaM[j].Column && voikoMustaSyodaOikea())
                        {
                            onSyotavaa = true;
                            on = true;
                        }
                    }
                }
            }
            return on;
        }


        /// <summary>
        /// Tarkistaa onko valkoisen pakko syödä
        /// </summary>
        /// <returns>tehtiinkö pakollinen syönti</returns>
        private Boolean valkoisenPakkoSyoda()
        {
            int syotava1 = -1;
            int syotava2 = -1;
            int voiSyoda = 0;
            // 0 = syödään vasemmalle, 1 = syödään oikealle
            int suunta = 0;
            Boolean syoty = false;

            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].Row + 1 == listaV[valkoinen].Row && listaM[i].Column + 1 == listaV[valkoinen].Column && voikoValkoinenSyodaVasen())
                {
                    voiSyoda++;
                    syotava1 = i;
                }
                if (listaM[i].Row + 1 == listaV[valkoinen].Row && listaM[i].Column - 1 == listaV[valkoinen].Column && voikoValkoinenSyodaOikea())
                {
                    voiSyoda++;
                    syotava2 = i;
                    suunta = 1;
                }
            }

            if (voiSyoda == 1)
            {
                if (suunta == 0)
                {
                    lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, listaV[valkoinen].Row - 2, listaV[valkoinen].Column - 2);
                    listaV[valkoinen].Row -= 2;
                    listaV[valkoinen].Column -= 2;
                    listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                    listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                    if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                    listaM[syotava1].Visibility = Visibility.Hidden;
                    listaM.RemoveAt(syotava1);
                    if (!onkoSyotavaa())
                    {
                        pelivuoro = Vuoro.Musta;
                        onSyotavaa = false;
                    }
                    listaV[valkoinen].IsChecked = null;
                    valkoinen = -1;
                    musta = -1;
                    syoty = true;
                }
                if (suunta == 1)
                {
                    lisaaKoordinaatti("V", listaV[valkoinen].Row, listaV[valkoinen].Column, listaV[valkoinen].Row - 2, listaV[valkoinen].Column + 2);
                    listaV[valkoinen].Row -= 2;
                    listaV[valkoinen].Column += 2;
                    listaV[valkoinen].SetValue(Grid.RowProperty, listaV[valkoinen].Row);
                    listaV[valkoinen].SetValue(Grid.ColumnProperty, listaV[valkoinen].Column);
                    if (listaV[valkoinen].Row == 0 && listaV[valkoinen].Tammi == false) nappulaTammeksi("V");
                    listaM[syotava2].Visibility = Visibility.Hidden;
                    listaM.RemoveAt(syotava2);
                    if (!onkoSyotavaa())
                    {
                        pelivuoro = Vuoro.Musta;
                        onSyotavaa = false;
                    }
                    listaV[valkoinen].IsChecked = null;
                    valkoinen = -1;
                    musta = -1;
                    syoty = true;
                }
                gameOver();
            }
            return syoty;
        }


        /// <summary>
        /// Tarkistaa onko mustan pakko syödä
        /// </summary>
        /// <returns>tehtiinkö pakollinen syönti</returns>
        private Boolean mustanPakkoSyoda()
        {
            int syotava1 = -1;
            int syotava2 = -1;
            int voiSyoda = 0;
            // 0 = syödään vasemmalle, 1 = syödään oikealle
            int suunta = 0;
            Boolean syoty = false;

            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].Row - 1 == listaM[musta].Row && listaV[i].Column + 1 == listaM[musta].Column && voikoMustaSyodaVasen())
                {
                    voiSyoda++;
                    syotava1 = i;
                }
                if (listaV[i].Row - 1 == listaM[musta].Row && listaV[i].Column - 1 == listaM[musta].Column && voikoMustaSyodaOikea())
                {
                    voiSyoda++;
                    syotava2 = i;
                    suunta = 1;
                }
            }
            if (voiSyoda == 1)
            {
                if (suunta == 0)
                {
                    lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, listaM[musta].Row - 2, listaM[musta].Column - 2);
                    listaM[musta].Row += 2;
                    listaM[musta].Column -= 2;
                    listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                    listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                    if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                    listaV[syotava1].Visibility = Visibility.Hidden;
                    listaV.RemoveAt(syotava1);
                    if (!onkoSyotavaa())
                    {
                        pelivuoro = Vuoro.Valkoinen;
                        onSyotavaa = false;
                    }
                    listaM[musta].IsChecked = null;
                    valkoinen = -1;
                    musta = -1;
                    syoty = true;
                }
                if (suunta == 1)
                {
                    lisaaKoordinaatti("M", listaM[musta].Row, listaM[musta].Column, listaM[musta].Row - 2, listaM[musta].Column + 2);
                    listaM[musta].Row += 2;
                    listaM[musta].Column += 2;
                    listaM[musta].SetValue(Grid.RowProperty, listaM[musta].Row);
                    listaM[musta].SetValue(Grid.ColumnProperty, listaM[musta].Column);
                    if (listaM[musta].Row == pelialue.Koko - 1 && listaM[musta].Tammi == false) nappulaTammeksi("M");
                    listaV[syotava2].Visibility = Visibility.Hidden;
                    listaV.RemoveAt(syotava2);
                    if (!onkoSyotavaa())
                    {
                        pelivuoro = Vuoro.Valkoinen;
                        onSyotavaa = false;
                    }
                    listaM[musta].IsChecked = null;
                    valkoinen = -1;
                    musta = -1;
                    syoty = true;
                }
                gameOver();
            }
            return syoty;
        }


        # region Tarkistetaan onko syötävän nappulan taakse mahdollista liikkua
        /// <summary>
        /// Tarkastetaan onko syötävän nappulan takana tyhjä ruutu
        /// </summary>
        /// <returns>onko tyhjä ruutu</returns>
        private bool voikoValkoinenSyodaVasen()
        {
            Boolean ok = false;
            for (int i = 0; i < buttonlistaV.Count; i++)
            {
                if (buttonlistaV[i].Row == listaV[valkoinen].Row - 2 && buttonlistaV[i].Column == listaV[valkoinen].Column - 2) ok = true;
            }
            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].Row == listaV[valkoinen].Row - 2 && listaM[i].Column == listaV[valkoinen].Column - 2) ok = false;
            }
            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].Row == listaV[valkoinen].Row - 2 && listaV[i].Column == listaV[valkoinen].Column - 2) ok = false;
            }
            return ok;
        }


        /// <summary>
        /// Tarkistetaan onko syötävän nappulan takana tyhjä ruutu
        /// </summary>
        /// <returns>onko tyhjä ruutu</returns>
        private bool voikoValkoinenSyodaOikea()
        {
            Boolean ok = false;
            for (int i = 0; i < buttonlistaV.Count; i++)
            {
                if (buttonlistaV[i].Row == listaV[valkoinen].Row - 2 && buttonlistaV[i].Column == listaV[valkoinen].Column + 2) ok = true;
            }
            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].Row == listaV[valkoinen].Row - 2 && listaM[i].Column == listaV[valkoinen].Column + 2) ok = false;
            }
            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].Row == listaV[valkoinen].Row - 2 && listaV[i].Column == listaV[valkoinen].Column + 2) ok = false;
            }
            return ok;
        }


        /// <summary>
        /// Tarkastetaan onko syötävän nappulan takana tyhjä ruutu
        /// </summary>
        /// <returns>onko tyhjä ruutu</returns>
        private bool voikoMustaSyodaVasen()
        {
            Boolean ok = false;
            for (int i = 0; i < buttonlistaV.Count; i++)
            {
                if (buttonlistaV[i].Row == listaM[musta].Row + 2 && buttonlistaV[i].Column == listaM[musta].Column - 2) ok = true;
            }
            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].Row == listaM[musta].Row + 2 && listaM[i].Column == listaM[musta].Column - 2) ok = false;
            }
            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].Row == listaM[musta].Row + 2 && listaV[i].Column == listaM[musta].Column - 2) ok = false;
            }
            return ok;
        }


        /// <summary>
        /// Tarkistetaan onko syötävän nappulan takana tyhjä ruutu
        /// </summary>
        /// <returns>onko tyhjä ruutu</returns>
        private bool voikoMustaSyodaOikea()
        {
            Boolean ok = false;
            for (int i = 0; i < buttonlistaV.Count; i++)
            {
                if (buttonlistaV[i].Row == listaM[musta].Row + 2 && buttonlistaV[i].Column == listaM[musta].Column + 2) ok = true;
            }
            for (int i = 0; i < listaM.Count; i++)
            {
                if (listaM[i].Row == listaM[musta].Row + 2 && listaM[i].Column == listaM[musta].Column + 2) ok = false;
            }
            for (int i = 0; i < listaV.Count; i++)
            {
                if (listaV[i].Row == listaM[musta].Row + 2 && listaV[i].Column == listaM[musta].Column + 2) ok = false;
            }
            return ok;
        }
        # endregion


        /// <summary>
        /// Lisätään siirron koordinaatit listaan
        /// </summary>
        /// <param name="pelaaja"></param>
        /// <param name="lahtorivi"></param>
        /// <param name="lahtosarakae"></param>
        /// <param name="tulorivi"></param>
        /// <param name="tulosarake"></param>
        private void lisaaKoordinaatti(string pelaaja, int lahtorivi, int lahtosarake, int tulorivi, int tulosarake)
        {
            string nimi = "";
            if (pelaaja.Equals("V")) nimi = pelaaja1Nimi;
            if (pelaaja.Equals("M")) nimi = pelaaja2Nimi;
            string apu = "";
            string apu2 = "";
            int[] t = new int[pelialue.Koko];
            int i = 1;
            for (int j = pelialue.Koko - 1; j >= 0; j--)
            {
                t[j] = i;
                i++;
            }
            switch (lahtosarake)
            {
                case 0:
                    apu = "a";
                    break;

                case 1:
                    apu = "b";
                    break;

                case 2:
                    apu = "c";
                    break;

                case 3:
                    apu = "d";
                    break;

                case 4:
                    apu = "e";
                    break;

                case 5:
                    apu = "f";
                    break;

                case 6:
                    apu = "g";
                    break;

                case 7:
                    apu = "h";
                    break;

                case 8:
                    apu = "i";
                    break;

                case 9:
                    apu = "j";
                    break;

                case 10:
                    apu = "k";
                    break;

                case 11:
                    apu = "l";
                    break;

                case 12:
                    apu = "m";
                    break;

                case 13:
                    apu = "n";
                    break;

                case 14:
                    apu = "o";
                    break;

                case 15:
                    apu = "p";
                    break;

                default:
                    break;
            }
            apu = apu + ((t[lahtorivi])).ToString();
            apu = apu + " - ";
            switch (tulosarake)
            {
                case 0:
                    apu2 = "a";
                    break;

                case 1:
                    apu2 = "b";
                    break;

                case 2:
                    apu2 = "c";
                    break;

                case 3:
                    apu2 = "d";
                    break;

                case 4:
                    apu2 = "e";
                    break;

                case 5:
                    apu2 = "f";
                    break;

                case 6:
                    apu2 = "g";
                    break;

                case 7:
                    apu2 = "h";
                    break;

                case 8:
                    apu2 = "i";
                    break;

                case 9:
                    apu2 = "j";
                    break;

                case 10:
                    apu2 = "k";
                    break;

                case 11:
                    apu2 = "l";
                    break;

                case 12:
                    apu2 = "m";
                    break;

                case 13:
                    apu2 = "n";
                    break;

                case 14:
                    apu2 = "o";
                    break;

                case 15:
                    apu2 = "p";
                    break;

                default:
                    break;
            }
            apu = apu + apu2;
            apu = apu + ((t[tulorivi])).ToString();
            apu = nimi + " " + apu;
            listaKoordinaatit.Add(apu);
        }


        /// <summary>
        /// Jos nappulasta tulee Tammi
        /// </summary>
        private void nappulaTammeksi(string vari)
        {
            if (pelivuoro == Vuoro.Valkoinen)
            {
                if (vari.Equals("V"))
                {
                    listaV[valkoinen].Tammi = true;
                    var anim = new DoubleAnimation
                    {
                        From = listaV[valkoinen].Height,
                        To = listaV[valkoinen].Height * 1.3
                    };

                    listaV[valkoinen].BeginAnimation(WidthProperty, anim);

                    var anim2 = new DoubleAnimation
                    {
                        From = listaV[valkoinen].Height,
                        To = listaV[valkoinen].Height * 1.3
                    };
                    listaV[valkoinen].BeginAnimation(HeightProperty, anim2);
                }
            }
            if (pelivuoro == Vuoro.Musta)
            {
                if (vari.Equals("M")) listaM[musta].Tammi = true;
                {
                    listaM[musta].Tammi = true;
                    var anim = new DoubleAnimation
                    {
                        From = listaM[musta].Height,
                        To = listaM[musta].Height * 1.3
                    };

                    listaM[musta].BeginAnimation(WidthProperty, anim);

                    var anim2 = new DoubleAnimation
                    {
                        From = listaM[musta].Height,
                        To = listaM[musta].Height * 1.3
                    };
                    listaM[musta].BeginAnimation(HeightProperty, anim2);
                }
            }
        }



        /// <summary>
        /// Tarkastetaan loppuiko peli
        /// </summary>
        private void gameOver()
        {
            Boolean loppuiko = false;
            string voittaja = "";

            if (peli == Peli.Breakthrough)
            {
                for (int i = 0; i < listaV.Count; i++)
                {
                    if (listaV[i].Row == 0)
                    {
                        loppuiko = true;
                        voittaja = pelaaja1Nimi;
                    }
                }
                for (int i = 0; i < listaM.Count; i++)
                {
                    if (listaM[i].Row == pelialue.Koko - 1)
                        loppuiko = true;
                    voittaja = pelaaja2Nimi;
                }
            }

            if (peli == Peli.Tammi)
            {
                if (listaV.Count == 0)
                {
                    loppuiko = true;
                    voittaja = pelaaja2Nimi;
                }

                if (listaM.Count == 0)
                {
                    loppuiko = true;
                    voittaja = pelaaja1Nimi;
                }
            }

            if (loppuiko == true)
            {
                GameOver game = new GameOver(voittaja);
                game.ShowDialog();
                pelialue.IsEnabled = false;
            }
        }


        /// <summary>
        /// Uuden pelin klikkaus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void menuReset_Click(object sender, RoutedEventArgs e)
        {
            asetukset.IsEnabled = true;
            pelialue.IsEnabled = false;
        }


        /// <summary>
        /// About
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void menuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }


        /// <summary>
        /// Säännöt
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void menuSaannot_Click(object sender, RoutedEventArgs e)
        {
            Saannot saannot = new Saannot();
            saannot.Show();
        }


        /// <summary>
        /// Lopetus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void menuQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        /// <summary>
        /// Avustus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        protected void menuAvustus_Click(object sender, RoutedEventArgs e)
        {
            Avustus avustus = new Avustus();
            avustus.Show();
        }


        /// <summary>
        /// Aloita peli klikkaus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void buttonAloita_Click(object sender, RoutedEventArgs e)
        {
            asetukset.IsEnabled = false;
            if (!textBoxP1.Text.ToString().Equals("")) pelaaja1Nimi = textBoxP1.Text.ToString();
            if (!textBoxP2.Text.ToString().Equals("")) pelaaja2Nimi = textBoxP2.Text.ToString();
            textBoxP1.Text = pelaaja1Nimi.ToString();
            textBoxP2.Text = pelaaja2Nimi.ToString();
            if (radioBreak.IsChecked == true)
            {
                peli = Peli.Breakthrough;
            }
            else
            {
                peli = Peli.Tammi;
            }
            ResetGame();
        }


        # region Varin valinta color dialogista

        /// <summary>
        /// Pelaajan 1 värin valinta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void p1vari_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new System.Windows.Forms.ColorDialog();
            if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfColor = System.Windows.Media.Color.FromArgb(Dialog.Color.A, Dialog.Color.R, Dialog.Color.G, Dialog.Color.B);
                var brush = new SolidColorBrush(wpfColor);
                nappula1 = brush;
            }
            for (int i = 0; i < listaV.Count; i++)
            {
                listaV[i].Vari = nappula1;
            }
        }


        /// <summary>
        /// Pelaajan 2 ärin valinta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void p2vari_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new System.Windows.Forms.ColorDialog();
            if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfColor = System.Windows.Media.Color.FromArgb(Dialog.Color.A, Dialog.Color.R, Dialog.Color.G, Dialog.Color.B);
                var brush = new SolidColorBrush(wpfColor);
                nappula2 = brush;
            }
            for (int i = 0; i < listaM.Count; i++)
            {
                listaM[i].Vari = nappula2;
            }
        }


        /// <summary>
        /// Ruudukon varin valinta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void r1vari_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new System.Windows.Forms.ColorDialog();
            if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfColor = System.Windows.Media.Color.FromArgb(Dialog.Color.A, Dialog.Color.R, Dialog.Color.G, Dialog.Color.B);
                var brush = new SolidColorBrush(wpfColor);
                ruutu1 = brush;
            }
            int k = 0;
            for (int i = 0; i < pelialue.Koko; i++)
            {
                for (int j = 0; j < pelialue.Koko; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        buttonlistaV[k].Vari = ruutu1;
                    }
                    else
                    {
                        buttonlistaV[k].Vari = ruutu2;
                    }
                    k++;
                }
            }
        }


        /// <summary>
        /// Ruudukon toisen värin valinta
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void r2vari_Click(object sender, RoutedEventArgs e)
        {
            var Dialog = new System.Windows.Forms.ColorDialog();
            if (Dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var wpfColor = System.Windows.Media.Color.FromArgb(Dialog.Color.A, Dialog.Color.R, Dialog.Color.G, Dialog.Color.B);
                var brush = new SolidColorBrush(wpfColor);
                ruutu2 = brush;
            }
            int k = 0;
            for (int i = 0; i < pelialue.Koko; i++)
            {
                for (int j = 0; j < pelialue.Koko; j++)
                {
                    if ((i + j) % 2 == 0)
                    {
                        buttonlistaV[k].Vari = ruutu1;
                    }
                    else
                    {
                        buttonlistaV[k].Vari = ruutu2;
                    }
                    k++;
                }
            }
        }
        # endregion



        /// <summary>
        /// Save napin klikkaus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "CanastaPeli";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Tekstitiedosto (.txt)|*.txt";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == null || result == false)
                return;

            StreamWriter file = new System.IO.StreamWriter(dialog.OpenFile());
            foreach (string s in listaKoordinaatit)
                file.WriteLine(s);
            file.Close();
        }



        /// <summary>
        /// Print napin klikkaus
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">RoutedEventArgs</param>
        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() != true) return;

            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new Size(dialog.PrintableAreaWidth - 40, dialog.PrintableAreaHeight - 40);

            FixedPage page1 = new FixedPage();
            page1.RenderTransform = new TranslateTransform(20, 20);
            page1.Width = document.DocumentPaginator.PageSize.Width;
            page1.Height = document.DocumentPaginator.PageSize.Height;

            DockPanel panel = new DockPanel();
            panel.Width = page1.Width;
            panel.Height = page1.Height;
            page1.Children.Add(panel);
            panel.LastChildFill = true;

            TextBlock page1Text = new TextBlock();
            page1Text.TextWrapping = TextWrapping.Wrap;
            foreach (string s in listaKoordinaatit)
                page1Text.Text += s + Environment.NewLine;
            page1Text.FontSize = 12;
            panel.Children.Add(page1Text);
            DockPanel.SetDock(page1Text, Dock.Top);

            PageContent page1Content = new PageContent();
            ((IAddChild)page1Content).AddChild(page1);
            document.Pages.Add(page1Content);

            dialog.PrintDocument(document.DocumentPaginator, "Fixed document");
        }
    }
}
