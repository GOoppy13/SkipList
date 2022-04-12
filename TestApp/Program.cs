using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkipList2020;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var lib = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            int n = nums.Count;
            for (int i = 0; i < n; i++)
            {
                lib.Add(nums[i], i);
            }
            nums.Sort();
            int j = 0;
            foreach (var pair in lib)
            {
                bool f = nums[j] == pair.Key;
                j++;
            }
        }
    }
}
