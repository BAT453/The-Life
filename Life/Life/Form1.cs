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
        private bool _stopFlag = false;
        private const int _pow = 30;
        private bool[,] _life = new bool[_pow, _pow];
        private bool[,] _tempLife = new bool[_pow, _pow];
        private Button[,] _sells = new Button[_pow, _pow];

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(_pow * 15 + 150, _pow * 15 + 100);

            Button GoButton = new Button();
            GoButton.Text = "Go";
            GoButton.Location = new Point(_pow * 10 + _pow * 5 + 40, 30);
            GoButton.Click += new EventHandler(this.GObuttonClick);
            this.Controls.Add(GoButton);

            Button ClearButton = new Button();
            ClearButton.Text = "Clear";
            ClearButton.Location = new Point(_pow * 10 + _pow * 5 + 40, 60);
            ClearButton.Click += new EventHandler(this.ClearbuttonClick);
            this.Controls.Add(ClearButton);

            for (int i = 0; i < _pow; i++)
            {
                for (int j = 0; j < _pow; j++)
                {
                    _sells[i, j] = new Button();
                    _sells[i, j].Size = new Size(15, 15);
                    _sells[i, j].Location = new Point(i * 10 + i * 5 + 20, j * 5 + j * 10 + 30);
                    _sells[i, j].Click += new EventHandler(this.SellClick);
                    _sells[i, j].Tag = new Coord(i, j);
                    _sells[i, j].FlatStyle = FlatStyle.Flat;
                    this.Controls.Add(_sells[i, j]);
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

            for (int i = 0; i < _pow; i++)
            {
                for (int j = 0; j < _pow; j++)
                {
                    int numberOfNeighbors = GetNumberOfNeighbors(i, j);

                    _tempLife[i, j] = CheckLifeOrDie(numberOfNeighbors, _life[i, j]);
                }
            }

            for (int i = 0; i < _pow; i++)
            {
                for (int j = 0; j < _pow; j++)
                {
                    _life[i, j] = _tempLife[i, j];

                    if (_life[j, i])
                    {
                        _sells[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        _sells[i, j].BackColor = Color.Black;
                    }
                }
            }
            this.ResumeLayout();
        }

        private int GetNumberOfNeighbors(int i, int j)
        {
            int neighbor = 0;

            if (_life[(i - 1) == (-1) ? (_pow - 1) : (i - 1), j])
            {
                neighbor++;
            }
            if (_life[(i - 1) == (-1) ? (_pow - 1) : (i - 1), (j - 1) == (-1) ? (_pow - 1) : (j - 1)])
            {
                neighbor++;
            }
            if (_life[(i - 1) == (-1) ? (_pow - 1) : (i - 1), (j + 1) == _pow ? (0) : (j + 1)])
            {
                neighbor++;
            }

            if (_life[(i + 1) == _pow ? (0) : (i + 1), j])
            {
                neighbor++;
            }
            if (_life[(i + 1) == _pow ? (0) : (i + 1), (j - 1) == (-1) ? (_pow - 1) : (j - 1)])
            {
                neighbor++;
            }
            if (_life[(i + 1) == _pow ? (0) : (i + 1), (j + 1) == _pow ? (0) : (j + 1)])
            {
                neighbor++;
            }
            if (_life[i, (j + 1) == _pow ? (0) : (j + 1)])
            {
                neighbor++;
            }
            if (_life[i, (j - 1) == (-1) ? (_pow - 1) : (j - 1)])
            {
                neighbor++;
            }

            return neighbor;
        }

        private void ClearbuttonClick(object sender, EventArgs e)
        {
            _stopFlag = true;
            NewLife();
        }

        private void GObuttonClick(object sender, EventArgs e)
        {
            GoLife();
        }

        private void GoLife()
        {
            for (int i = 0; i < _pow + _pow; i++)
            {
                NextGeneration();
                Refresh();
                if (_stopFlag)
                {
                    i = _pow + _pow;
                }
            }
            if (_stopFlag)
            {
                _stopFlag = false;
                NewLife();
            }
        }

        private void NewLife()
        {
            for (int i = 0; i < _pow; i++)
            {
                for (int j = 0; j < _pow; j++)
                {
                    _life[i, j] = false;
                    _tempLife[i, j] = false;
                    _sells[i, j].BackColor = Color.Black;
                }
            }
            this.Refresh();
        }

        private void SellClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Coord coord = (Coord)btn.Tag;
            btn.BackColor = Color.White;
            _life[coord.Y, coord.X] = true;
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
