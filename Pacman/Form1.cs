using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using Pacman.Properties;

namespace Pacman
{
    public partial class Form1 : Form
    {
        Timer timer;
        Pacman Pacman;
        static readonly int TIMER_INTERVAL = 250;
        static readonly int WORLD_WIDTH = 15;
        static readonly int WORLD_HEIGHT = 10;
        static readonly int MAX_POINTS = 125;
        Pen RedPen;
        Image foodImage;
        bool[][] foodWorld;
        public int Points;
        public List<Obstacle> Obstacles;
        public static int NUMBER_OF_OBSTACLES = 8;
        public Form1()
        {
            InitializeComponent();
            foodImage = Resources.food;
            DoubleBuffered = true;
            RedPen = new Pen(Color.Red, 2);
            newGame();
        }
        public void newGame()
        {
            Pacman = new Pacman();
            Points = 0;
            pbPercent.Value = 0;
            this.Width = Pacman.RADIUS * 2 * (WORLD_WIDTH + 1);
            this.Height = Pacman.RADIUS * 2 * (WORLD_HEIGHT + 1) + 50;
            // овде кодот за иницијализација на матрицата foodWorld
            foodWorld = new bool[WORLD_HEIGHT][];
            for(int i = 0; i < WORLD_HEIGHT; i++)
            {
                foodWorld[i] = new bool[WORLD_WIDTH];
                for (int j = 0; j < WORLD_WIDTH; j++)
                    foodWorld[i][j] = true;
            }
            foodWorld[Pacman.PosX][Pacman.PosY] = false;
            Obstacles = new List<Obstacle>();
            GenerateObstacles();
            // овде кодот за иницијализација и стартување на тајмерот
            timer = new Timer();
            timer.Interval = TIMER_INTERVAL;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }
        public void GenerateObstacles()
        {
            for(int i = 0; i < NUMBER_OF_OBSTACLES; i++)
            {
                Obstacle o = new Obstacle();
                while (!CheckValidObstaclePosition(o))
                    o.GeneratePosition();
                Obstacles.Add(o);
            }
        }
        public bool CheckValidObstaclePosition(Obstacle obstacle)
        {
            foreach(Obstacle o in Obstacles)
            {
               if (Math.Abs(obstacle.X - o.X) <= 3 && Math.Abs(obstacle.Y - o.Y) <= 1)
                    return false;
            }
            return true;
        }
        public bool checkCollision()
        {
            foreach (Obstacle Obstacle in Obstacles)
            {
                if (Math.Abs(Pacman.PosX - Obstacle.X) <= 2 && Obstacle.Y - Pacman.PosY == 0)
                    return false;
            }
            return true;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            // овде вашиот код
            Pacman.Move(WORLD_WIDTH, WORLD_HEIGHT, Obstacles);
            for (int i = 0; i < WORLD_HEIGHT; i++)
                for (int j = 0; j < WORLD_WIDTH; j++)
                    if (Pacman.PosX == i && Pacman.PosY == j && foodWorld[i][j])
                    {
                        foodWorld[i][j] = false;
                        Points++;
                    }                  
            Invalidate();
            if (IsGameOver())
                EndGame();
        }
        public bool IsGameOver()
        {
            return Points == MAX_POINTS;
        }
        public void EndGame()
        {
            timer.Stop();
            MessageBox.Show("You finished the game.");
            if (MessageBox.Show("Do you want to start a new game?", "New game", MessageBoxButtons.YesNo) == DialogResult.Yes)
                newGame();
            else
                this.Close();
        }
        public void DrawObstacles(Graphics g)
        {
            foreach (Obstacle o in Obstacles)
                o.Draw(g);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            for (int i = 0; i < foodWorld.Length; i++)
            {
                for (int j = 0; j < foodWorld[i].Length; j++)
                {
                    if (foodWorld[i][j])
                        g.DrawImageUnscaled(foodImage, j * Pacman.RADIUS * 2 + ((Pacman.RADIUS * 2 - foodImage.Height) / 2) + 7, i * Pacman.RADIUS * 2 + ((Pacman.RADIUS * 2 - foodImage.Width) / 2) + 10);
                }
            }
            lbPoints.Text = Points.ToString();
            pbPercent.Value = (int)((decimal)Points * 100 / MAX_POINTS);
            Pacman.Draw(g);
            DrawObstacles(g);
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                Pacman.ChangeDirection(DIRECTION.UP);
            if (e.KeyCode == Keys.Down)
                Pacman.ChangeDirection(DIRECTION.DOWN);
            if (e.KeyCode == Keys.Left)
                Pacman.ChangeDirection(DIRECTION.LEFT);
            if (e.KeyCode == Keys.Right)
                Pacman.ChangeDirection(DIRECTION.RIGHT);
            Invalidate();
        }

       
    }
}
