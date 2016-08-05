#region License and Terms
// MoreLINQ - Extensions to LINQ to Objects
// Copyright (c) 2008 Jonathan Skeet. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace MoreLinq.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class PartialSortTests
    {
        [Test]
        public void PartialSortWithNullSequence()
        {
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.PartialSort<object>(null, 0)).ParamName);
            Assert.AreEqual("source", Assert.Throws<ArgumentNullException>(() => MoreEnumerable.PartialSort(null, 0, Comparer<object>.Default)).ParamName);
        }

        [Test]
        public void PartialSort()
        {
            var top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(0)
                                .PartialSort(5);

            top.AssertSequenceEqual(Enumerable.Range(0, 5));
        }

        [Test]
        public void PartialSortWithOrder()
        {
            var top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(0)
                                .PartialSort(5, OrderByDirection.Ascending);

            top.AssertSequenceEqual(Enumerable.Range(0, 5));
            top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(0)
                                .PartialSort(5, OrderByDirection.Descending);
            top.AssertSequenceEqual(Enumerable.Range(6, 5).Reverse());
        }

        [Test]
        public void PartialSortWithDuplicates()
        {
            var top = Enumerable.Range(1, 10)
                                .Reverse()
                                .Concat(Enumerable.Repeat(3, 3))
                                .PartialSort(5);

            top.AssertSequenceEqual(1, 2, 3, 3, 3);
        }

        [Test]
        public void PartialSortWithComparer()
        {
            var alphabet = Enumerable.Range(0, 26)
                                     .Select((n, i) => ((char)((i % 2 == 0 ? 'A' : 'a') + n)).ToString())
                                     .ToArray();

            var top = alphabet.PartialSort(5, StringComparer.Ordinal);

            top.Select(s => s[0]).AssertSequenceEqual('A', 'C', 'E', 'G', 'I');
        }

        [Test]
        public void PartialSortIsLazy()
        {
            new BreakingSequence<object>().PartialSort(1);
        }
    }
}