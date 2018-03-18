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
    public class Coord
    {
        public int x { set; get; }
        public int y { set; get; }

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    public partial class Form1 : Form
    {
        bool stopflag = false;
        const int pow = 30;
        bool[,] life = new bool[pow, pow];
        bool[,] temp_life = new bool[pow, pow];
        Button[,] soil = new Button[pow, pow];


        public Form1()
        {
            InitializeComponent();

            this.Size = new System.Drawing.Size(pow * 15 + 150, pow*15 + 100);

            Button GObutton = new Button();
            GObutton.Text = "Go";
            GObutton.Location = new Point(pow * 10 + pow * 5 + 40, 30);
            GObutton.Click += new EventHandler(this.GObuttonClick);
            this.Controls.Add(GObutton);

            Button Clearbutton = new Button();
            Clearbutton.Text = "Clear";
            Clearbutton.Location = new Point(pow * 10 + pow * 5 + 40, 60);
            Clearbutton.Click += new EventHandler(this.ClearbuttonClick);
            this.Controls.Add(Clearbutton);

            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    soil[i, j] = new Button();
                    soil[i, j].Size = new System.Drawing.Size(15, 15);
                    soil[i, j].Location = new Point(i * 10 + i * 5 + 20, j * 5 + j * 10 + 30);
                    soil[i, j].Click += new EventHandler(this.SoilClick);
                    soil[i, j].Tag = new Coord(i, j);
                    soil[i, j].FlatStyle = FlatStyle.Flat;
                    this.Controls.Add(soil[i, j]);
                }
            }
            NewLife();
        }

        private void NextGeneration()
        {
            this.SuspendLayout();

            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    int nc = NeiborthCount(i, j);
                    if ((nc == 3) && (!life[i, j])) { temp_life[i, j] = true; }
                    if (((nc == 3) || (nc == 2)) && life[i, j]) { temp_life[i, j] = true; }
                    if (((nc < 2) || (nc > 3)) && life[i, j]) { temp_life[i, j] = false; }
                }
            }
            for (int i = 0; i < pow; i++)
            {
                for (int j = 0; j < pow; j++)
                {
                    life[i, j] = temp_life[i, j];
                    if (life[j, i])
                    {
                        soil[i, j].BackColor = Color.White;
                    }
                    else
                    {
                        soil[i, j].BackColor = Color.Black;
                    }
                }
            }
            this.ResumeLayout();
        }

        private int NeiborthCount(int i, int j)
        {
            int neib = 0;

            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), j]) { neib++; }
            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), (j - 1) == (-1) ? (pow - 1) : (j - 1)]) { neib++; }
            if (life[(i - 1) == (-1) ? (pow - 1) : (i - 1), (j + 1) == pow ? (0) : (j + 1)]) { neib++; }

            if (life[(i + 1) == pow ? (0) : (i + 1), j]) { neib++; }
            if (life[(i + 1) == pow ? (0) : (i + 1), (j - 1) == (-1) ? (pow - 1) : (j - 1)]) { neib++; }
            if (life[(i + 1) == pow ? (0) : (i + 1), (j + 1) == pow ? (0) : (j + 1)]) { neib++; }

            if (life[i, (j + 1) == pow ? (0) : (j + 1)]) { neib++; }
            if (life[i, (j - 1) == (-1) ? (pow - 1) : (j - 1)]) { neib++; }
           
            return neib;
        }

        private void ClearbuttonClick(object sender, EventArgs e)
        {
            stopflag = true;
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
                this.Refresh();
                if (stopflag)
                {
                    i = pow + pow;
                }
            }
            if (stopflag)
            {
                stopflag = false;
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
                    soil[i, j].BackColor = Color.Black;
                }
            }
            this.Refresh();
        }

        private void SoilClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Coord coord = (Coord)btn.Tag;
            btn.BackColor = Color.White;
            life[coord.y, coord.x] = true;
        }
    }
}
