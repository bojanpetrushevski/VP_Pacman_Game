using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Pacman.Properties;

namespace Pacman
{
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT}
    public enum ANGLE { RIGHT_START_ANGLE = 45, DOWN_START_ANGLE = 135, LEFT_START_ANGLE = 225, END_ANGLE = 270, UP_START_ANGLE = 315}
    public class Pacman
    {
        
        public int PosX { set; get;}
        public int PosY { set; get; }
        public DIRECTION Direction { set; get; }
        public const int RADIUS = 20;
        public int Speed { set; get; }
        public bool IsMouthOpen { set; get; }
        public Brush Brush { set; get; }
        public ANGLE StartAngle { set; get; }
        public Image foodImage { set; get; }
        public Pacman()
        {
            PosX = 4;
            PosY = 6;
            Direction = DIRECTION.RIGHT;
            StartAngle = ANGLE.RIGHT_START_ANGLE;
            Speed = RADIUS;
            IsMouthOpen = true;
            Brush = new SolidBrush(Color.Yellow);
            foodImage = Resources.food;
        }
        public void ChangeDirection(DIRECTION direction)
        {
            // vasiot kod ovde
            Direction = direction;
            SetStartAngle();

        }
        public void SetStartAngle()
        {
            if (Direction == DIRECTION.UP)
                StartAngle = ANGLE.UP_START_ANGLE;
            if (Direction == DIRECTION.DOWN)
                StartAngle = ANGLE.DOWN_START_ANGLE;
            if (Direction == DIRECTION.LEFT)
                StartAngle = ANGLE.LEFT_START_ANGLE;
            if (Direction == DIRECTION.RIGHT)
                StartAngle = ANGLE.RIGHT_START_ANGLE;
        }
        public void Move(int Width, int Height, List<Obstacle> Obstacles)
        {
            // vasiot kod ovde
            if(Direction == DIRECTION.UP)
                PosX--;
            if(Direction == DIRECTION.DOWN)
                PosX++;  
            if (Direction == DIRECTION.LEFT)
                PosY--;
            if (Direction == DIRECTION.RIGHT)
                PosY++;    
            CheckOutOfBounds(Width, Height);
            if (!CheckCollision(Obstacles))
                FixPosition();
            IsMouthOpen = !IsMouthOpen;
        }
        public void FixPosition()
        {
            if (Direction == DIRECTION.UP)
                PosX++;
            if (Direction == DIRECTION.DOWN)
                PosX--;
            if (Direction == DIRECTION.LEFT)
                PosY++;
            if (Direction == DIRECTION.RIGHT)
                PosY--;
        }
        public void CheckOutOfBounds(int Width, int Height)
        {
            if (PosX < 0)
                PosX = Height - 1;
            else if (PosX > Height - 1)
                PosX = 0;
            else if (PosY < 0)
                PosY = Width - 1;
            else if (PosY > Width - 1)
                PosY = 0;
        }
        public bool CheckCollision(List<Obstacle> Obstacles)
        {
            foreach(Obstacle Obstacle in Obstacles)
            {
                if (PosY == Obstacle.Y && (PosX == Obstacle.X || PosX == Obstacle.X + 1 || PosX == Obstacle.X + 2))
                    return false;
            }
            return true;
        }
        public void Draw(Graphics g)
        {
            // vasiot kod ovde
            if (IsMouthOpen) 
                g.FillPie(Brush, new Rectangle(PosY * Pacman.RADIUS * 2 + (Pacman.RADIUS * 2 - foodImage.Height) / 2, PosX * Pacman.RADIUS * 2 + (Pacman.RADIUS * 2 - foodImage.Width) / 2, RADIUS * 2, RADIUS * 2), (int)StartAngle, (int)ANGLE.END_ANGLE);
            else
                g.FillEllipse(Brush, new Rectangle(PosY * Pacman.RADIUS * 2 + (Pacman.RADIUS * 2 - foodImage.Height) / 2, PosX * Pacman.RADIUS * 2 + (Pacman.RADIUS * 2 - foodImage.Width) / 2, RADIUS * 2, RADIUS * 2));
        }
    }
}
