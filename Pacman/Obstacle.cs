using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Pacman.Properties;

namespace Pacman
{
    public class Obstacle
    {
        public int X { set; get; }
        public int Y { set; get; }
        public Pen RedPen { set; get; }
        public Random PositionGenerator { set; get; }
        public Image foodImage { set; get; }
        public Obstacle()
        {
            RedPen = new Pen(Color.Red, 2);
            foodImage = Resources.food;
            PositionGenerator = new Random();
            GeneratePosition();
        }
        public void GeneratePosition()
        {
            X = PositionGenerator.Next(8);
            Y = PositionGenerator.Next(15);
        }
        public void Draw(Graphics g)
        {
            g.DrawRectangle(RedPen, Y * Pacman.RADIUS * 2 + ((Pacman.RADIUS * 2 - foodImage.Height) / 2) + 5, X * Pacman.RADIUS * 2 + ((Pacman.RADIUS * 2 - foodImage.Width) / 2) + 2 , 30, 115);
        }
    }
}
