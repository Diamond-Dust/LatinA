using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace LatinA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        int counter = 0, outtake = -1, whole = 0, good = 0;
        char mode=' ';
        Dictionary<string, string> Anatomy = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            string key="", val="", line;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(@".\data.txt");
                while ((line = file.ReadLine()) != null)
                {
                    if (counter % 2 == 0)
                        key = line;
                    else
                    {
                        val = line;
                        Anatomy.Add(key, val);
                    }
                    counter++;
                }
                counter = counter / 2;
                file.Close();
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show("I had a problem while loading a data file. Are you sure there is properly formatted data.txt file in the LatinA.exe directory?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                if(result==MessageBoxResult.OK)
                    System.Windows.Application.Current.Shutdown();
            }
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Content.ToString() == "Results")
            {
                double prct = Math.Round(((double)good / (double)whole) * 100, 2);
                if (Double.IsNaN(prct))
                    MessageBox.Show("Your percentage of correct answers is:\n" + "Nothing.", "Percentage");
                else
                    MessageBox.Show("Your percentage of correct answers is:\n" + prct + "%", "Percentage");
            }
            else
            {
                mode = 'S';
                (sender as Button).Content = "Results";
                RslE.Content = "Results+End";
                Check.Visibility = Visibility.Visible;
                Ans.Visibility = Visibility.Visible;
                Str.Visibility = Visibility.Visible;
                if (outtake + 1 < Anatomy.Count())
                    outtake++;
                else
                {
                    double prct = Math.Round(((double)good / (double)whole) * 100, 2);
                    if (Double.IsNaN(prct))
                        MessageBox.Show("You have gone through the entire data file.\nYour percentage of correct answers is:\n" + "Nothing.", "Percentage");
                    else
                        MessageBox.Show("You have gone through the entire data file.\nYour percentage of correct answers is:\n" + prct + "%", "Percentage");
                    Latin.Text = "Ready?";
                    Pol.Text = "Ready?";
                    Check.Visibility = Visibility.Hidden;
                    Ans.Visibility = Visibility.Collapsed;
                    Str.Visibility = Visibility.Hidden;
                    whole = 0;
                    good = 0;
                    outtake = -1;
                    Rslt.Content = "Sequence Mode";
                    RslE.Content = "Random Mode";
                }
                if (outtake != -1)
                    Latin.Text = Anatomy.Keys.ElementAt(outtake);
                Pol.Text = "";
                Keyboard.Focus(Pol);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if((sender as Button).Content.ToString() == "Results+End")
            {
                double prct = Math.Round(((double)good / (double)whole) * 100, 2);
                if (Double.IsNaN(prct))
                    MessageBox.Show("Your percentage of correct answers is:\n" + "Nothing.", "Percentage");
                else
                    MessageBox.Show("Your percentage of correct answers is:\n" + prct + "%", "Percentage");
                Latin.Text = "Ready?";
                Pol.Text = "Ready?";
                Check.Visibility = Visibility.Hidden;
                Ans.Visibility = Visibility.Collapsed;
                Str.Visibility = Visibility.Hidden;
                whole = 0;
                good = 0;
                outtake = -1;
                Rslt.Content = "Sequence Mode";
                RslE.Content = "Random Mode";
            }
            else
            {
                mode = 'R';
                (sender as Button).Content = "Results+End";
                Rslt.Content = "Results";
                Check.Visibility = Visibility.Visible;
                Ans.Visibility = Visibility.Visible;
                Str.Visibility = Visibility.Visible;
                Random rnd = new Random();
                outtake = rnd.Next(0, counter);
                Latin.Text = Anatomy.Keys.ElementAt(outtake);
                Pol.Text = "";
                Keyboard.Focus(Pol);
            }
        }

        private void Rvrs_Click(object sender, RoutedEventArgs e)
        {
            Anatomy = Anatomy
                .ToLookup(kp => kp.Value)
                .ToDictionary(g => g.Key, g => g.First().Key);
            string tmp = TopLab.Content.ToString();
            TopLab.Content = BotLab.Content;
            BotLab.Content = tmp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pol.Text = "";
            if(mode=='R')
            {
                Random rnd = new Random();
                outtake = rnd.Next(0, counter);
            }
            else if(mode=='S')
            {
                if (outtake + 1 < Anatomy.Count())
                    outtake++;
                else
                {
                    double prct = Math.Round(((double)good / (double)whole) * 100, 2);
                    if (Double.IsNaN(prct))
                        MessageBox.Show("You have gone through the entire data file.\nYour percentage of correct answers is:\n" + "Nothing.", "Percentage");
                    else
                        MessageBox.Show("You have gone through the entire data file.\nYour percentage of correct answers is:\n" + prct + "%", "Percentage");
                    Str.Content = "Start";
                    Latin.Text = "Ready?";
                    Pol.Text = "Ready?";
                    Check.Visibility = Visibility.Hidden;
                    Ans.Visibility = Visibility.Collapsed;
                    Str.Visibility = Visibility.Hidden;
                    whole = 0;
                    good = 0;
                    outtake = -1;
                    Rslt.Content = "Sequence Mode";
                    RslE.Content = "Random Mode";
                }
            }
            if(outtake!=-1)
                Latin.Text = Anatomy.Keys.ElementAt(outtake);
            Pol.Background = Brushes.White;
            Str.IsDefault = false;
            Check.IsDefault = true;
            Check.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Anatomy.Values.ElementAt(outtake), "Answer");
            whole++;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Pol.Text.ToLower().Trim() == Anatomy.Values.ElementAt(outtake).ToLower().Trim())
            {
                whole++;
                good++;
                Pol.Background = Brushes.Chartreuse;
                Check.IsDefault = false;
                Str.IsDefault = true;
                Check.IsEnabled = false;
            }
            else
            {
                whole++;
                Pol.Background = Brushes.IndianRed;
            }
        }
    }
}
