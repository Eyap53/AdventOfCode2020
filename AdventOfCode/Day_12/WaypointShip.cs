namespace AdventOfCode.Day_12_Core
{
    using System;

    public class WaypointShip : BaseShip
    {
        protected IntVector2 _relativeWaypointPosition = new IntVector2(10, 1);

        public override void Translate(Direction direction, int value)
        {
            switch (direction)
            {
                case Direction.North:
                    _relativeWaypointPosition.y += value;
                    break;
                case Direction.South:
                    _relativeWaypointPosition.y -= value;
                    break;
                case Direction.East:
                    _relativeWaypointPosition.x += value;
                    break;
                case Direction.West:
                    _relativeWaypointPosition.x -= value;
                    break;
                default:
                    throw new NotImplementedException("Direction not implemented.");
            }
        }

        public override void Rotate(int value, bool isClockwiseRotation)
        {
            int orientation = value * (isClockwiseRotation ? 1 : -1);
            if (orientation < 0)
            {
                orientation += 360;
            }
            if (orientation >= 360)
            {
                orientation -= 360;
            }

            switch (orientation)
            {
                case 0:
                    break;
                case 90:
                    _relativeWaypointPosition = new IntVector2(_relativeWaypointPosition.y, -_relativeWaypointPosition.x);
                    break;
                case 180:
                    _relativeWaypointPosition = new IntVector2(-_relativeWaypointPosition.x, -_relativeWaypointPosition.y);
                    break;
                case 270:
                    _relativeWaypointPosition = new IntVector2(-_relativeWaypointPosition.y, _relativeWaypointPosition.x);
                    break;
                default:
                    throw new ArgumentException("Orientation not recognized : " + orientation);
            }
        }

        public override void MoveForward(int value)
        {
            _position += value * _relativeWaypointPosition;
        }
    }
}
