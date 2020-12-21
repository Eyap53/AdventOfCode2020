namespace AdventOfCode.Day_20_Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Image
    {
        public int id { get; private set; }
        public int imageWidth { get; private set; }
        private int _tileWidth;
        private int _tilesByRow;
        private int _tileCount;
        private char[,] _pixelsGrid;

        private Tile[,] _positionnedTiles;

        public Image(List<Tile> tiles)
        {
            _tileCount = tiles.Count;
            _tileWidth = tiles[0].imageWidth;
            _tilesByRow = (int)(Math.Sqrt(_tileCount));

            #region tiles aggregation (reconstruction by borders)

            _positionnedTiles = new Tile[_tilesByRow, _tilesByRow];
            List<Tile> notPlacedTiles = new List<Tile>(tiles);
            Queue<Tile> placedWithNeigborsTiles = new Queue<Tile>();

            _positionnedTiles[0, 0] = notPlacedTiles[0];
            placedWithNeigborsTiles.Enqueue(notPlacedTiles[0]);
            notPlacedTiles.RemoveAt(0);

            while (notPlacedTiles.Any())
            {
                Tile checkedTile = placedWithNeigborsTiles.Dequeue();

                IntVector2 checkedPosition = CoordinatesOf(_positionnedTiles, checkedTile); // Bad for perf

                string[] checkedBorders = checkedTile.GetCurrentBorders(); // already found border could be removed to improve perf

                // Var caching
                Tile notPlacedTile;
                for (int tileIndex = notPlacedTiles.Count - 1; tileIndex >= 0; tileIndex--)
                {
                    notPlacedTile = notPlacedTiles[tileIndex];
                    string[] notPlacedBorders = notPlacedTile.GetAllPossibleBorders();
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (checkedBorders[i] == notPlacedBorders[j])
                            {
                                if (i == 0 && j == 0 || i == 2 && j == 4)
                                {
                                    notPlacedTile.FlipVertically();
                                }
                                else if (i == 0 && j == 1 || i == 1 && j == 3 || i == 2 && j == 5 || i == 3 && j == 7)
                                {
                                    notPlacedTile.Rotate180();
                                }
                                else if (i == 0 && j == 2 || i == 2 && j == 6)
                                {
                                    notPlacedTile.RotateLeft();
                                    notPlacedTile.FlipVertically();
                                }
                                else if (i == 0 && j == 3 || i == 1 && j == 5 || i == 2 && j == 7 || i == 3 && j == 1)
                                {
                                    notPlacedTile.RotateRight();
                                }
                                else if (i == 0 && j == 4 || i == 2 && j == 0)
                                {
                                    notPlacedTile.FlipHorizontally();
                                }
                                else if (i == 0 && j == 5 || i == 1 && j == 7 || i == 2 && j == 1 || i == 3 && j == 3)
                                {
                                    // Nothing to do, already well placed !
                                }
                                else if (i == 0 && j == 6 || i == 2 && j == 2)
                                {
                                    notPlacedTile.RotateRight();
                                    notPlacedTile.FlipVertically();
                                }
                                else if (i == 0 && j == 7 || i == 1 && j == 1 || i == 2 && j == 3 || i == 3 && j == 5)
                                {
                                    notPlacedTile.RotateLeft();
                                }
                                else if (i == 1 && j == 0 || i == 3 && j == 4)
                                {
                                    notPlacedTile.RotateRight();
                                    notPlacedTile.FlipHorizontally();
                                }
                                else if (i == 1 && j == 2 || i == 3 && j == 6)
                                {
                                    notPlacedTile.FlipHorizontally();
                                }
                                else if (i == 1 && j == 4 || i == 3 && j == 0)
                                {
                                    notPlacedTile.RotateLeft();
                                    notPlacedTile.FlipHorizontally();
                                }
                                else if (i == 1 && j == 6 || i == 3 && j == 2)
                                {
                                    notPlacedTile.FlipVertically();
                                }
                                else
                                {
                                    throw new NotImplementedException();
                                }

                                switch (i)
                                {
                                    case 0:
                                        if (checkedPosition.x == 0)
                                        {
                                            // Need to move all previous tiles
                                            for (int x = _tilesByRow - 1; x >= 1; x--)
                                            {
                                                for (int y = 0; y < _tilesByRow; y++)
                                                {
                                                    _positionnedTiles[x, y] = _positionnedTiles[x - 1, y];
                                                }
                                            }
                                            for (int y = 0; y < _tilesByRow; y++)
                                            {
                                                _positionnedTiles[0, y] = null;
                                            }
                                            checkedPosition.x = 1;
                                        }
                                        _positionnedTiles[checkedPosition.x - 1, checkedPosition.y] = notPlacedTile;
                                        break;
                                    case 1:
                                        _positionnedTiles[checkedPosition.x, checkedPosition.y + 1] = notPlacedTile;
                                        break;
                                    case 2:
                                        _positionnedTiles[checkedPosition.x + 1, checkedPosition.y] = notPlacedTile;
                                        break;
                                    case 3:
                                        if (checkedPosition.y == 0)
                                        {
                                            // Need to move all previous tiles
                                            for (int x = 0; x < _tilesByRow; x++)
                                            {
                                                for (int y = _tilesByRow - 1; y >= 1; y--)
                                                {
                                                    _positionnedTiles[x, y] = _positionnedTiles[x, y - 1];
                                                }
                                            }
                                            for (int x = 0; x < _tilesByRow; x++)
                                            {
                                                _positionnedTiles[x, 0] = null;
                                            }
                                            checkedPosition.y = 1;
                                        }
                                        _positionnedTiles[checkedPosition.x, checkedPosition.y - 1] = notPlacedTile;
                                        break;
                                    default:
                                        throw new NotImplementedException();
                                }

                                placedWithNeigborsTiles.Enqueue(notPlacedTile);
                                notPlacedTiles.RemoveAt(tileIndex);

                            }
                        }
                    }
                }

                ToString();
            }

            #endregion

            #region True image reconstruction

            imageWidth = (_tileWidth - 2) * _tilesByRow;
            _pixelsGrid = new char[imageWidth, imageWidth];
            int tileIndex_i = 0;
            int tileIndex_j;
            for (int i = 0; i < imageWidth; i++)
            {
                if (i != 0 && i % (_tileWidth - 2) == 0)
                {
                    tileIndex_i++;
                }

                tileIndex_j = 0;
                for (int j = 0; j < imageWidth; j++)
                {
                    if (j != 0 && j % (_tileWidth - 2) == 0)
                    {
                        tileIndex_j++;
                    }
                    Tile currentTile = _positionnedTiles[tileIndex_i, tileIndex_j];
                    _pixelsGrid[i, j] = currentTile.GetPixel(i + 1 - tileIndex_i * (_tileWidth - 2), j + 1 - tileIndex_j * (_tileWidth - 2));
                }
            }
            #endregion


        }

        public static IntVector2 CoordinatesOf<Tile>(Tile[,] matrix, Tile tileToSearch)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y] != null && matrix[x, y].Equals(tileToSearch))
                    {
                        return new IntVector2(x, y);
                    }
                }
            }

            throw new Exception("Not found in array.");
        }

        public string TilesDisplay()
        {
            string result = "Tile Display :\n";
            for (int i = 0; i < _tileWidth * _tilesByRow; i++)
            {
                if (i % _tileWidth == 0)
                {
                    result += "\n";
                }
                for (int j = 0; j < _tileWidth * _tilesByRow; j++)
                {
                    if (j % _tileWidth == 0)
                    {
                        result += " ";
                    }
                    if (_positionnedTiles[i / _tileWidth, j / _tileWidth] != null)
                    {
                        result += _positionnedTiles[i / _tileWidth, j / _tileWidth].GetPixel(i % _tileWidth, j % _tileWidth);
                    }
                    else
                    {
                        result += " ";
                    }
                }
                result += "\n";
            }
            return result;
        }

        public override string ToString()
        {
            string result = "Image :\n";
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    result += _pixelsGrid[i, j];
                }
                result += "\n";
            }
            return result;
        }

        public Int64 MultiplyIdCorners()
        {
            long result = 1;
            result *= _positionnedTiles[0, 0].id;
            result *= _positionnedTiles[_tilesByRow - 1, 0].id;
            result *= _positionnedTiles[_tilesByRow - 1, _tilesByRow - 1].id;
            result *= _positionnedTiles[0, _tilesByRow - 1].id;
            return result;
        }

        public int DetermineWaterRoughness()
        {
            int testWithoutFlipCount = 0;
            int monsterFoundCount = 0;
            int roughness;
            while (testWithoutFlipCount < 4)
            {
                if (testWithoutFlipCount != 0)
                {
                    RotateRight();
                }
                roughness = ComputeWaterRoughness(ref monsterFoundCount);
                if (monsterFoundCount > 0)
                {
                    return roughness;
                }
                testWithoutFlipCount++;
            }
            FlipHorizontally();
            testWithoutFlipCount = 0;
            while (testWithoutFlipCount < 4)
            {
                if (testWithoutFlipCount != 0)
                {
                    RotateRight();
                }
                roughness = ComputeWaterRoughness(ref monsterFoundCount);
                if (monsterFoundCount > 0)
                {
                    return roughness;
                }
                testWithoutFlipCount++;
            }
            throw new Exception("Monster not found");
        }
        private int ComputeWaterRoughness(ref int monsterFoundCount)
        {
            monsterFoundCount = 0;
            int roughnessCount = 0;
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    if (_pixelsGrid[i, j] == '#')
                    {
                        roughnessCount++;

                        if (i <= imageWidth - 3 && j >= 18 && j <= imageWidth - 2)
                        {
                            // Check if it is a monster. HARDCODED for now
                            if (_pixelsGrid[i + 1, j - 18] == '#' &&
                                _pixelsGrid[i + 1, j - 13] == '#' &&
                                _pixelsGrid[i + 1, j - 12] == '#' &&
                                _pixelsGrid[i + 1, j - 7] == '#' &&
                                _pixelsGrid[i + 1, j - 6] == '#' &&
                                _pixelsGrid[i + 1, j - 1] == '#' &&
                                _pixelsGrid[i + 1, j] == '#' &&
                                _pixelsGrid[i + 1, j + 1] == '#' &&
                                _pixelsGrid[i + 2, j - 17] == '#' &&
                                _pixelsGrid[i + 2, j - 14] == '#' &&
                                _pixelsGrid[i + 2, j - 11] == '#' &&
                                _pixelsGrid[i + 2, j - 8] == '#' &&
                                _pixelsGrid[i + 2, j - 5] == '#' &&
                                _pixelsGrid[i + 2, j - 2] == '#'
                                )
                            {
                                monsterFoundCount++;
                            }
                        }
                    }
                }
            }

            return roughnessCount - monsterFoundCount * 15;
        }

        public void FlipVertically()
        {
            char[,] flippedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    flippedImage[i, j] = _pixelsGrid[imageWidth - 1 - i, j];
                }
            }
            _pixelsGrid = flippedImage;
        }

        public void FlipHorizontally()
        {
            char[,] flippedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    flippedImage[i, j] = _pixelsGrid[i, imageWidth - 1 - j];
                }
            }
            _pixelsGrid = flippedImage;
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
                    rotatedImage[i, j] = _pixelsGrid[imageWidth - 1 - j, i];
                }
            }
            _pixelsGrid = rotatedImage;
        }

        public void RotateLeft()
        {
            char[,] rotatedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    rotatedImage[i, j] = _pixelsGrid[j, imageWidth - 1 - i];
                }
            }
            _pixelsGrid = rotatedImage;
        }

        public void Rotate180()
        {
            char[,] rotatedImage = new char[imageWidth, imageWidth];
            for (int i = 0; i < imageWidth; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    rotatedImage[i, j] = _pixelsGrid[imageWidth - 1 - i, imageWidth - 1 - j];
                }
            }
            _pixelsGrid = rotatedImage;
        }
    }
}
