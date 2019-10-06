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
using System.Threading;
using System.Reflection;

namespace GameOFLife
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Button[][] Buttons;
        GOL g = new GOL();
        public MainWindow()
        {
            InitializeComponent();
            g.create(80, 50,false);
            this.show(g.GameField);

        }
        private void CreateDynamicWPFGrid(int width, int height)

        {

            // Create the Grid
            Buttons = new Button[width][];
            int buttonwidth = 10;
            int buttonheight = 10;
            Grid DynamicGrid = new Grid();

            DynamicGrid.Width = buttonwidth*width+(70*3);

            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;

            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;

            DynamicGrid.ShowGridLines = false;

            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);



            for (int i = 0; i < width; i++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = new GridLength(buttonwidth);
                DynamicGrid.ColumnDefinitions.Add(c);
            }
            

            for (int i = 0; i < height; i++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = new GridLength(buttonheight);
                
                DynamicGrid.RowDefinitions.Add(r);
            }
            Thickness t = new Thickness(0.1);
            for (int x = 0; x < width; x++)
            {
                Buttons[x] = new Button[height];
                for (int y = 0; y < height ; y++)
                {
                    
                    Button MyControl1 = new Button();
                    MyControl1.DataContext = new int[]{ x,y};
                    MyControl1.Background = Brushes.Black;
                    MyControl1.BorderThickness = t;
                    MyControl1.Height = buttonheight;
                    MyControl1.Width = buttonwidth;
                    MyControl1.Click += (s, e) => { Buttonclick(MyControl1); };
                    Grid.SetColumn(MyControl1, x);
                    Grid.SetRow(MyControl1, y);
                    DynamicGrid.Children.Add(MyControl1);
                    
                    Buttons[x][y] = MyControl1;
                    
                }
            }


            // Display grid into a Window



            SetButtons(DynamicGrid, width, height);


            this.Content = DynamicGrid;

        }
        public void SetButtons(Grid DynamicGrid, int width, int height ) {
            RowDefinition rr = new RowDefinition();
            rr.Height = new GridLength(100);
            DynamicGrid.RowDefinitions.Add(rr);

            ColumnDefinition cc = new ColumnDefinition();
            cc.Width = new GridLength(70);
            DynamicGrid.ColumnDefinitions.Add(cc);
            cc = new ColumnDefinition();
            cc.Width = new GridLength(70);
            DynamicGrid.ColumnDefinitions.Add(cc);
            cc = new ColumnDefinition();
            cc.Width = new GridLength(70);
            DynamicGrid.ColumnDefinitions.Add(cc);


            Button MyControl = new Button();
            MyControl.Click += (s, e) => { Button_Click(s, e); };
            MyControl.Content = "Run";
            MyControl.Background = Brushes.Red;
            MyControl.Height = 20;
            MyControl.Width = 30;
            MyControl.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(MyControl, width);
            Grid.SetRow(MyControl, height);
            DynamicGrid.Children.Add(MyControl);

            MyControl = new Button();
            MyControl.Click += (s, e) => { Button_Click_next(s, e); };
            MyControl.Content = "next";
            MyControl.Background = Brushes.Red;
            MyControl.Height = 20;
            MyControl.Width = 40;
            MyControl.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(MyControl, width);
            Grid.SetRow(MyControl, height);
            DynamicGrid.Children.Add(MyControl);



            MyControl = new Button();
            MyControl.Click += (s, e) => { Button_Click_stop(s, e); };
            MyControl.Content = "Pause";
            MyControl.Background = Brushes.Red;
            MyControl.Height = 20;
            MyControl.Width = 40;
            MyControl.HorizontalAlignment = HorizontalAlignment.Left;

            Grid.SetColumn(MyControl, width+1);
            Grid.SetRow(MyControl, height);
            DynamicGrid.Children.Add(MyControl);

            

            MyControl = new Button();
            MyControl.Click += (s, e) => { Button_Click_previous(s, e); };
            MyControl.Content = "prev";
            MyControl.Background = Brushes.Red;
            MyControl.Height = 20;
            MyControl.Width = 30;
            MyControl.HorizontalAlignment = HorizontalAlignment.Right;
            Grid.SetColumn(MyControl, width+1);
            Grid.SetRow(MyControl, height);
            DynamicGrid.Children.Add(MyControl);

            MyControl = new Button();
            MyControl.Click += (s, e) => { Button_Click_rewind(s, e); };
            MyControl.Content = "Rewind";
            MyControl.Background = Brushes.Red;
            MyControl.Height = 20;
            MyControl.Width = 50;
            MyControl.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetColumn(MyControl, width + 2);
            Grid.SetRow(MyControl, height);
            DynamicGrid.Children.Add(MyControl);

        }


        public void setButtonColor(int x, int y, Brush b)
        {
            Button button = Buttons[x][y];
            setButtonColor(button, b);
        }
        public delegate void UpdateColorCallback(Brush b);
        public void setButtonColor(Button button, Brush b)
        {
                if (button.Background != b)
                {

                    button.Background = b;
                }
            
        }
        public void show(bool[][] field, List<int[]> l)
        {
            
            //Updates all fields  with the coords in l
            SolidColorBrush b;
            foreach (int[] elem in l)
            {
                if (field[elem[0]][elem[1]])
                {
                    b = Brushes.White;
                }
                else
                {
                    b=Brushes.Black;
                }
                setButtonColor(elem[0], elem[1], b);
            }

        }
        public void show(bool[][] field)
        {
        
            if (Buttons==null ||field.Length != Buttons.Length || field[0].Length != Buttons[0].Length) {
                CreateDynamicWPFGrid(field.Length, field[0].Length);
            }
            int x = 0;
            int y = 0;
            foreach (bool[] row in field) {
                y = 0;
                foreach (bool f in row)
                {
                    if (f)
                    {
                        setButtonColor(x, y, Brushes.White);
                    }
                    else
                    {
                        setButtonColor(x, y, Brushes.Black);
                    }
                    y++;
                }
                x++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            g.stop();
            Task.Factory.StartNew(() => g.run(this));

        }
        private void Button_Click_rewind(object sender, RoutedEventArgs e)
        {
            g.stop();
            Task.Factory.StartNew(() => g.rewind(this));

        }
        private void Button_Click_stop(object sender, RoutedEventArgs e)
        {
            g.stop();
        }
        private void Button_Click_next(object sender, RoutedEventArgs e)
        {
            g.stop();
            Task.Factory.StartNew(() => g.runOnce(this));
            
            
        }
        private void Button_Click_previous(object sender, RoutedEventArgs e)
        {
            g.stop();
            g.rewindOnce(this);


        }
        public void Buttonclick(Button MyControl1)
        {
            if (!g.IsRunning) {
                if (MyControl1.Background == Brushes.White)
                {
                    MyControl1.Background = Brushes.Black;
                    g.changeGamefield(((int[])MyControl1.DataContext)[0], ((int[])MyControl1.DataContext)[1], false);
                } else
                {
                    MyControl1.Background = Brushes.White;
                    g.changeGamefield(((int[])MyControl1.DataContext)[0], ((int[])MyControl1.DataContext)[1], true);
                }
            }
        }


    }
}
