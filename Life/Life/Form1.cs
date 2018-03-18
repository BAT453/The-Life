using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Life
{
    public partial class Form1 : Form
    {
        bool stopFlag = false;
        const int pow = 30;
        bool[,] life = new bool[pow, pow];
        bool[,] temp_life = new bool[pow, pow];
        Button[,] sells = new Button[pow, pow];

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(pow * 15 + 150, pow * 15 + 100);

            Button GoButton = new Button();
            GoButton.Text = "Go";
            GoButton.Location = new Point(pow * 10 + pow * 5 + 40, 30);
            GoButton.Click += new EventHandler(this.GObuttonClick);
            this.Controls.Add(GoButton);

            Button ClearButton = new Button();
            ClearButton.Text = "Clear";
            ClearButton.Location = new Point(pow * 10 + pow * 5 + 40, 60);
            ClearButton.Click += new EventHandler(this.ClearbuttonClick);
            this.Controls.Add(ClearButton);

            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    sells[i, j] = new Button();
                    sells[i, j].Size = new Size(15, 15);
                    sells[i, j].Location = new Point(i * 10 + i * 5 + 20, j * 5 + j * 10 + 30);
                    sells[i, j].Click += new EventHandler(this.SellClick);
                    sells[i, j].Tag = new Coord(i, j);
                    sells[i, j].FlatStyle = FlatStyle.Flat;
                    this.Controls.Add(sells[i, j]);
                }
            }
            NewLife();
        }

        private bool CheckLifeOrDie(int numberOfNeighbors, bool currentSell)
        {
            if (numberOfNeighbors == 3 && !currentSell)
            {
                return true;
            }
            if ((numberOfNeighbors == 3 || numberOfNeighbors == 2) && currentSell)
            {
                return true;
            }

            return false;
        }

        private void NextGeneration()
        {
            SuspendLayout();

            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    int numberOfNeighbors = GetNumberOfNeighbors(i, j);

                    temp_life[i, j] = CheckLifeOrDie(numberOfNeighbors, life[i, j]);
                }
            }

            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    life[i, j] = temp_life[i, j];

                    if (life[j, i])
                    {
                        sells[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        sells[i, j].BackColor = Color.Black;
                    }
                }
            }
            this.ResumeLayout();
        }

        private int GetNumberOfNeighbors(int i, int j)
        {
            int neighbor = 0;

            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), j])
            {
                neighbor++;
            }
            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), (j - 1) == (-1) ? (pow - 1) : (j - 1)])
            {
                neighbor++;
            }
            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), (j + 1) == pow ? (0) : (j + 1)])
            {
                neighbor++;
            }

            if (life[(i + 1) == pow ? (0) : (i + 1), j])
            {
                neighbor++;
            }
            if (life[(i + 1) == pow ? (0) : (i + 1), (j - 1) == (-1) ? (pow - 1) : (j - 1)])
            {
                neighbor++;
            }
            if (life[(i + 1) == pow ? (0) : (i + 1), (j + 1) == pow ? (0) : (j + 1)])
            {
                neighbor++;
            }
            if (life[i, (j + 1) == pow ? (0) : (j + 1)])
            {
                neighbor++;
            }
            if (life[i, (j - 1) == (-1) ? (pow - 1) : (j - 1)])
            {
                neighbor++;
            }

            return neighbor;
        }

        private void ClearbuttonClick(object sender, EventArgs e)
        {
            stopFlag = true;
            NewLife();
        }


        private void GObuttonClick(object sender, EventArgs e)
        {
            GoLife();
        }

        private void GoLife()
        {
            for (int i = 0; i < pow + pow; i++)
            {
                NextGeneration();
                Refresh();
                if (stopFlag)
                {
                    i = pow + pow;
                }
            }
            if (stopFlag)
            {
                stopFlag = false;
                NewLife();
            }
        }

        private void NewLife()
        {
            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    life[i, j] = false;
                    temp_life[i, j] = false;
                    sells[i, j].BackColor = Color.Black;
                }
            }
            this.Refresh();
        }

        private void SellClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Coord coord = (Coord)btn.Tag;
            btn.BackColor = Color.White;
            life[coord.Y, coord.X] = true;
        }
    }

    public class Coord
    {
        public int X { set; get; }
        public int Y { set; get; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

}
