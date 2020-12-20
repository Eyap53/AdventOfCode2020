namespace AdventOfCode.Day_12_Core
{
    using System;

    public class SimpleShip : BaseShip
    {
        protected int _orientation = 0;

        public override void Translate(Direction direction, int value)
        {
            switch (direction)
            {
                case Direction.North:
                    _position.y += value;
                    break;
                case Direction.South:
                    _position.y -= value;
                    break;
                case Direction.East:
                    _position.x += value;
                    break;
                case Direction.West:
                    _position.x -= value;
                    break;
                default:
                    throw new NotImplementedException("Direction not implemented.");
            }
        }

        public override void Rotate(int value, bool isClockwiseRotation)
        {
            _orientation += value * (isClockwiseRotation ? 1 : -1);
            if (_orientation < 0)
            {
                _orientation += 360;
            }
            if (_orientation >= 360)
            {
                _orientation -= 360;
            }
        }

        public override void MoveForward(int value)
        {
            switch (_orientation)
            {
                case 0:
                    _position.x += value;
                    break;
                case 90:
                    _position.y -= value;
                    break;
                case 180:
                    _position.x -= value;
                    break;
                case 270:
                    _position.y += value;
                    break;
                default:
                    throw new ArgumentException("Orientation not recognized : " + _orientation);
            }
        }
    }
}
