using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkipList2020;

namespace SkipListUnitTests
{
    [TestClass]
    public class SkipListUnitTest
    {
        [TestMethod]
        public void CountIncreaseAfterAdding()
        {
            int n = 10;
            var lib = new SkipList<int, int>();

            for(int i=0; i<n; i++)
            {
                lib.Add(i, i);
            }
            Assert.AreEqual(n, lib.Count);
        }
        [TestMethod]
        public void ItemsExistsAfterAdding()
        {
            var lib = new SkipList<int, int>();
            var nums = new List<int>(new[] { 44, 22, 1 , 56, 3, 90, 31, 15, 26 });
            int n = nums.Count;
            for (int i = 0; i < n; i++)
            {
                lib.Add(nums[i], i);
            }
            nums.Sort();
            int j = 0;
            foreach(var pair in lib)
            {
                Assert.AreEqual(nums[j], pair.Key);
                j++;
            }
            Assert.AreEqual(n, lib.Count);
        }
        [TestMethod]
        public void RandomItemsExistsAfterAdding()
        {
            var lib = new SkipList<int, int>();
            var nums = new HashSet<int>();
            var rd = new Random();
            int n = 100;
            while (nums.Count < n)
            {
                nums.Add(rd.Next(1, n * 3));
            }
            foreach(var item in nums)
            {
                lib.Add(item,1);
            }

            var a = nums.ToList();
            a.Sort();
            int j = 0;
            foreach (var pair in lib)
            {
                Assert.AreEqual(a[j], pair.Key);
                j++;
            }
            Assert.AreEqual(n, lib.Count);
        }
        [TestMethod]
        public void ContainsItems()
        {
            var lib = new SkipList<int, int>();
            var numsForAdds = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            var numsForCheckContainsTrue = new List<int>() { 90, 31, 15, 26 };
            var numsForCheckContainsFalse = new List<int>() { 23, 13, 55, 100 };
            for (int i = 0; i < numsForAdds.Count; i++)
            {
                lib.Add(numsForAdds[i], i);
            }
            foreach (int i in numsForCheckContainsFalse)
            {
                Assert.IsFalse(lib.Contains(i));
            }
            foreach (int i in numsForCheckContainsTrue)
            {
                Assert.IsTrue(lib.Contains(i));
            }
        }
        [TestMethod]
        public void RemovesItems()
        {
            var lib = new SkipList<int, int>();
            var numsForAdds = new List<int>(new[] { 44, 22, 1, 56, 3, 90, 31, 15, 26 });
            var numsForRemoves = new List<int>() { 90, 31, 15, 26, 1 };
            for (int i = 0; i < numsForAdds.Count; i++)
            {
                lib.Add(numsForAdds[i], i);
            }
            foreach (int i in numsForRemoves)
            {
                lib.Remove(i);
            }
            Assert.AreEqual(numsForAdds.Count - numsForRemoves.Count, lib.Count);
            foreach (int i in numsForRemoves)
            {
                Assert.IsFalse(lib.Contains(i));
            }
        }
    }
}
