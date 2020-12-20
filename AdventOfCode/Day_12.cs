namespace AdventOfCode
{
    using AdventOfCode.Day_12_Core;
    using AoCHelper;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Day_12 : BaseDay
    {
        private readonly string[] _input;
        private static List<char> _directionIds = new List<char>() { 'N', 'E', 'S', 'W' };

        public Day_12()
        {
            _input = File.ReadAllLines(InputFilePath);
        }

        public override string Solve_1()
        {
            SimpleShip ship = new SimpleShip();
            foreach (string command in _input)
            {
                ExecuteCommand(ship, command);
            }
            return ship.GetManhattanDistance().ToString();
        }

        public override string Solve_2()
        {
            WaypointShip ship = new WaypointShip();
            foreach (string command in _input)
            {
                ExecuteCommand(ship, command);
            }
            return ship.GetManhattanDistance().ToString();
        }

        static void ExecuteCommand(BaseShip ship, string command)
        {
            char commandName = command[0];
            if (commandName == 'F')
            {
                ship.MoveForward(ParseCommandValue(command));
            }
            else if (commandName == 'R')
            {
                ship.Rotate(ParseCommandValue(command), true);
            }
            else if (commandName == 'L')
            {
                ship.Rotate(ParseCommandValue(command), false);
            }
            else if (_directionIds.Contains(commandName))
            {
                Direction direction = ParseDirection(commandName);
                ship.Translate(direction, ParseCommandValue(command));
            }
            else
            {
                throw new ArgumentException("Command name not recognized.");
            }
        }

        static Direction ParseDirection(char directionID)
        {
            switch (directionID)
            {
                case 'N':
                    return Direction.North;
                case 'E':
                    return Direction.East;
                case 'S':
                    return Direction.South;
                case 'W':
                    return Direction.West;
                default:
                    throw new ArgumentException("Direction ID not recognized.");
            }
        }

        static int ParseCommandValue(string commandLine)
        {
            return int.Parse(commandLine[1..]);
        }
    }
}
