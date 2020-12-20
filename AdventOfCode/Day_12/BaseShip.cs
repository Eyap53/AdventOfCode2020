namespace AdventOfCode.Day_12_Core
{
    using System;

    public abstract class BaseShip
    {
        protected IntVector2 _position = new IntVector2(0, 0);

        public abstract void Translate(Direction direction, int value);
        public abstract void Rotate(int value, bool isClockwiseRotation);
        public abstract void MoveForward(int value);

        public int GetManhattanDistance()
        {
            return _position.manhattanDistance;
        }

        public string ParsePosition()
        {
            return "(" + _position.x.ToString() + ", " + _position.y.ToString() + ")";
        }
    }
}
