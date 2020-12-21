namespace AdventOfCode.Day_20_Core
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class Tile
    {
        public int id { get; private set; }
        private char[,] _rawImage;
        public int imageWidth { get; private set; }

        public Tile(List<string> rawTileData)
        {
            string pattern = @"^Tile (?<id>\d+):$";
            Regex regex = new Regex(pattern);
            Match m = regex.Match(rawTileData[0]);

            if (m.Success)
            {
                this.id = int.Parse(m.Groups["id"].Value);
            }
            else
            {
                throw new ArgumentException("Not matched. Line : '" + rawTileData[0] + "'");
            }

            imageWidth = rawTileData.Count - 1;
            _rawImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    _rawImage[i, j] = rawTileData[i + 1][j];
                }
            }
        }

        public string[] GetAllPossibleBorders()
        {
            string[] possibleBorders = new string[8];
            for (int i = 0; i < imageWidth; i++)
            {
                possibleBorders[0] += _rawImage[0, i];
                possibleBorders[1] += _rawImage[0, imageWidth - 1 - i];
                possibleBorders[2] += _rawImage[i, imageWidth - 1];
                possibleBorders[3] += _rawImage[imageWidth - 1 - i, imageWidth - 1];
                possibleBorders[4] += _rawImage[imageWidth - 1, imageWidth - 1 - i];
                possibleBorders[5] += _rawImage[imageWidth - 1, i];
                possibleBorders[6] += _rawImage[imageWidth - 1 - i, 0];
                possibleBorders[7] += _rawImage[i, 0];
            }

            return possibleBorders;
        }

        public string[] GetCurrentBorders()
        {
            string[] currentBorders = new string[4];
            for (int i = 0; i < imageWidth; i++)
            {
                currentBorders[0] += _rawImage[0, i];
                currentBorders[1] += _rawImage[i, imageWidth - 1];
                currentBorders[2] += _rawImage[imageWidth - 1, imageWidth - 1 - i];
                currentBorders[3] += _rawImage[imageWidth - 1 - i, 0];
            }

            return currentBorders;
        }

        public void FlipVertically()
        {
            char[,] flippedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    flippedImage[i, j] = _rawImage[imageWidth - 1 - i, j];
                }
            }
            _rawImage = flippedImage;
        }

        public void FlipHorizontally()
        {
            char[,] flippedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    flippedImage[i, j] = _rawImage[i, imageWidth - 1 - j];
                }
            }
            _rawImage = flippedImage;
        }
        public void FlipBoth()
        {
            Rotate180();
        }

        public void RotateRight()
        {
            char[,] rotatedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    rotatedImage[i, j] = _rawImage[imageWidth - 1 - j, i];
                }
            }
            _rawImage = rotatedImage;
        }

        public void RotateLeft()
        {
            char[,] rotatedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    rotatedImage[i, j] = _rawImage[j, imageWidth - 1 - i];
                }
            }
            _rawImage = rotatedImage;
        }

        public void Rotate180()
        {
            char[,] rotatedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    rotatedImage[i, j] = _rawImage[imageWidth - 1 - i, imageWidth - 1 - j];
                }
            }
            _rawImage = rotatedImage;
        }

        public override string ToString()
        {
            string result = "Tile " + id.ToString() + "\n";
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    result += _rawImage[i, j];

                }
                result += "\n";
            }
            return result;
        }

        public char GetPixel(int i, int j)
        {
            return _rawImage[i, j];
        }
    }
}
