namespace AdventOfCode
{
    // using AdventOfCode.Day_14_Core;
    using AoCHelper;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Day_14 : BaseDay
    {
        string[] _input;

        public Day_14()
        {
            ParseInput1();
        }

        public override string Solve_1()
        {
            string currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; // No mask
            Dictionary<long, long> memory = new Dictionary<long, long>();

            string pattern = @"^mem\[(?<memoryAddress>\d+)\] = (?<memoryValue>\d+)$";
            Regex regex = new Regex(pattern);

            // Var caching
            Match m;
            long memoryAddress;
            long memoryValue;
            char[] bits;
            for (int i = 0; i < _input.Length; i++)
            {
                if (_input[i].StartsWith("mask"))
                {
                    currentMask = _input[i].Right(36);
                }
                else
                {
                    m = regex.Match(_input[i]);
                    if (m.Success)
                    {
                        memoryAddress = long.Parse(m.Groups["memoryAddress"].Value);
                        memoryValue = long.Parse(m.Groups["memoryValue"].Value);

                        bits = Convert.ToString(memoryValue, 2).PadLeft(36, '0').ToCharArray();
                        for (int bit = 0; bit < 36; bit++)
                        {
                            if (currentMask[bit] != 'X')
                            {
                                bits[bit] = currentMask[bit];
                            }
                        }
                        // Mask add
                        memory[memoryAddress] = Convert.ToInt64(new string(bits), 2);
                    }
                    else
                    {
                        throw new Exception("Regex failed !");
                    }
                }
            }
            return memory.Values.Sum().ToString();
        }

        public override string Solve_2()
        {
            string currentMask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"; // No mask
            Dictionary<long, long> memory = new Dictionary<long, long>();

            string pattern = @"^mem\[(?<memoryAddress>\d+)\] = (?<memoryValue>\d+)$";
            Regex regex = new Regex(pattern);

            // Var caching
            Match m;
            long baseMemoryAddress;
            HashSet<StringBuilder> memoryAddresses;
            long memoryValue;
            char[] bits;
            for (int i = 0; i < _input.Length; i++)
            {
                if (_input[i].StartsWith("mask"))
                {
                    currentMask = _input[i].Right(36);
                }
                else
                {
                    m = regex.Match(_input[i]);
                    if (m.Success)
                    {
                        baseMemoryAddress = long.Parse(m.Groups["memoryAddress"].Value);
                        memoryValue = long.Parse(m.Groups["memoryValue"].Value);

                        bits = Convert.ToString(baseMemoryAddress, 2).PadLeft(36, '0').ToCharArray();
                        for (int bit = 0; bit < 36; bit++)
                        {
                            if (currentMask[bit] == '1')
                            {
                                bits[bit] = '1';
                            }
                            else if (currentMask[bit] == 'X')
                            {
                                bits[bit] = 'X';
                            }
                        }
                        memoryAddresses = DecodeAdresses(bits);
                        foreach (StringBuilder x in memoryAddresses)
                        {
                            memory[Convert.ToInt64(x.ToString(), 2)] = memoryValue;
                        }
                    }
                    else
                    {
                        throw new Exception("Regex failed !");
                    }
                }
            }
            return memory.Values.Sum().ToString();
        }

        private HashSet<StringBuilder> DecodeAdresses(char[] bits)
        {
            HashSet<StringBuilder> addresses = new HashSet<StringBuilder>();

            if (bits[0] == 'X')
            {
                addresses.Add(new StringBuilder("0"));
                addresses.Add(new StringBuilder("1"));
            }
            else
            {
                addresses.Add(new StringBuilder(bits[0].ToString()));
            }

            foreach (char bit in bits.Skip(1))
            {
                if (bit == 'X')
                {
                    HashSet<StringBuilder> copy = new HashSet<StringBuilder>();
                    foreach (StringBuilder x in addresses)
                    {
                        copy.Add(new StringBuilder(x.ToString()).Append('1'));
                        x.Append('0');
                    }
                    addresses.UnionWith(copy);
                }
                else
                {
                    foreach (StringBuilder x in addresses) { x.Append(bit); }
                }
            }
            return addresses;
        }

        private void ParseInput1()
        {
            _input = File.ReadAllLines(InputFilePath);
        }

    }
}
