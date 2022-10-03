using System;
using System.Collections.Generic;
using System.Text;
using Game.Blocks.Gas;
using Game.core;

namespace Game.Blocks.Fluid
{
    public abstract class TestVelocityEnum : TestFixtureBase
    {
        protected IEnumerable<(Vector2Int vector, CellVelocity enumeration)> GetAllPairs()
        {
            yield return (Vector2Int.L, CellVelocity.DIR_L);
            yield return (Vector2Int.U, CellVelocity.DIR_U);
            yield return (Vector2Int.D, CellVelocity.DIR_D);
            yield return (Vector2Int.R, CellVelocity.DIR_R);
            yield return (Vector2Int.RU, CellVelocity.DIR_R_U);
            yield return (Vector2Int.LU, CellVelocity.DIR_L_U);
            yield return (Vector2Int.RD, CellVelocity.DIR_R_D);
            yield return (Vector2Int.LD, CellVelocity.DIR_L_D);
            yield return (Vector2Int.Z, CellVelocity.ZERO);
        }
        
    }

    public class TestEnumFromVec : TestVelocityEnum
    {
        public override void RunAllTests()
        {
            TestVectorize();
        }
        private void TestVectorize()
        {
            int cnt = 0;
            foreach (var pairs in GetAllPairs())
            {
                CellVelocity found = pairs.vector.FromVec();
                CellVelocity expect = pairs.enumeration;
                RecordResult($"{nameof(TestVectorize)}_{cnt++}", expect, found);
            }
        }
    }

    public class TestEnumerationToVec : TestVelocityEnum
    {
        public override void RunAllTests()
        {
            TestEnumerateVector();
        }
        
        private void TestEnumerateVector()
        {
            int cnt = 0;
            foreach (var pair in GetAllPairs())
            {
                Vector2Int found = pair.enumeration.ToVec();
                Vector2Int expect = pair.vector;
                RecordResult($"{nameof(TestEnumerateVector)}_{cnt++}", expect, found);
            }
        }

    }

    public class EnumVelocityTests
    {
        private int numberOfTests;
        private int numberOfPasses;
        private List<string> failedTests = new List<string>();
        public static void TestAll()
        {
            var tests = new EnumVelocityTests();
            
            tests.TestHasX();
            tests.TestHasY();
            tests.TestNotHasX();
            tests.TestNotHasY();
            tests.TestMaskX();
            tests.TestMaskY();
            tests.TestVectorize();
            tests.TestEnumerateVector();
            StringBuilder results = new StringBuilder();
            results.AppendLine($"# Tests Run:\t{tests.numberOfTests}");
            results.AppendLine($"# Tests Passed:\t{tests.numberOfPasses}");
            results.AppendLine($"# Tests Failed:\t{tests.numberOfTests-tests.numberOfPasses}");
            results.AppendLine("---------------------------------------");
            results.AppendLine("-Failed Tests--------------------------");
            results.AppendLine("---------------------------------------");
            foreach (var test in tests.failedTests)
            {
                results.AppendLine($"FAILED:\t {test}");
            }
            Debug.Log(results.ToString());
        }
        void RecordResult<T>(string testName, T expected, T actual)
        {
            numberOfTests++;
            var result = expected.Equals(actual);
            if (result)
            {
                numberOfPasses++;
            }
            else
            {
                failedTests.Add(testName);
            }
            Debug.Assert(result, $"{testName} Failed:\nexpected: {expected} but found {actual}");
        }

        private IEnumerable<(Vector2Int vector, CellVelocity enumeration)> GetAllPairs()
        {
            yield return (Vector2Int.L, CellVelocity.DIR_L);
            yield return (Vector2Int.U, CellVelocity.DIR_U);
            yield return (Vector2Int.D, CellVelocity.DIR_D);
            yield return (Vector2Int.R, CellVelocity.DIR_R);
            yield return (Vector2Int.RU, CellVelocity.DIR_R_U);
            yield return (Vector2Int.LU, CellVelocity.DIR_L_U);
            yield return (Vector2Int.RD, CellVelocity.DIR_R_D);
            yield return (Vector2Int.LD, CellVelocity.DIR_L_D);
            yield return (Vector2Int.Z, CellVelocity.ZERO);
        }

        private void TestEnumerateVector()
        {
            int cnt = 0;
            foreach (var pair in GetAllPairs())
            {
                Vector2Int found = pair.enumeration.ToVec();
                Vector2Int expect = pair.vector;
                RecordResult($"{nameof(TestEnumerateVector)}_{cnt++}", expect, found);
            }
        }

        private void TestVectorize()
        {
            int cnt = 0;
            foreach (var pairs in GetAllPairs())
            {
                CellVelocity found = pairs.vector.FromVec();
                CellVelocity expect = pairs.enumeration;
                RecordResult($"{nameof(TestVectorize)}_{cnt++}", expect, found);
            }
        }

        public void TestHasX() => RecordResult(nameof(TestHasX), true, CellVelocity.DIR_L_U.HasX());

        public void TestNotHasX() => RecordResult(nameof(TestNotHasX), false, CellVelocity.DIR_U.HasX());
        
        public void TestHasY() => RecordResult(nameof(TestHasY), true, CellVelocity.DIR_U.HasY());

        public void TestNotHasY() => RecordResult(nameof(TestNotHasY), false, CellVelocity.DIR_L.HasY());

        public void TestMaskX()
        {
            CellVelocity originalX = CellVelocity.DIR_L_U;
            CellVelocity actualMasked =  originalX.GetX();
            CellVelocity expect = CellVelocity.DIR_L;
            RecordResult(nameof(TestMaskX), expect, actualMasked);
        }

        public void TestMaskY()
        {
            CellVelocity originalX = CellVelocity.DIR_L_U;
            CellVelocity actualMasked =  originalX.GetY();
            CellVelocity expect = CellVelocity.DIR_U;
            RecordResult(nameof(TestMaskY), expect, actualMasked);

        }
    }
}