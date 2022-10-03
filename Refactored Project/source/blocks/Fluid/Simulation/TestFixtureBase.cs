using System.Collections.Generic;
using System.Text;
using Game.core;

namespace Game.Blocks.Fluid
{
    public abstract class TestFixtureBase
    {
        private int numberOfTests;
        private int numberOfPasses;
        private StringBuilder _stringBuilder = new StringBuilder();
        private List<string> failedTests = new List<string>();
        
        protected void RecordResult<T>(string testName, T expected, T actual)
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

        public void Init()
        {
            _stringBuilder.Clear();
            numberOfTests = 0;
            numberOfPasses = 0;
        }

        public void RunTests()
        {
            Init();
            
            Finished();
        }

        public abstract void RunAllTests();
        
        public void Finished()
        {
            Debug.Log(GetReadableResults(GetType().Name));
        }

        public string GetReadableResults(string group)
        {
            StringBuilder results = new StringBuilder();
            results.AppendLine("=======================================");
            results.AppendLine($"{group}");
            results.AppendLine("---------------------------------------");
            
            results.AppendLine($"\n# Tests Run:\t{numberOfTests}");
            results.AppendLine($"\n# Tests Passed:\t{numberOfPasses}");
            results.AppendLine($"\n# Tests Failed:\t{numberOfTests-numberOfPasses}");
            results.AppendLine("---------------------------------------");
            results.AppendLine("-Failed Tests--------------------------");
            results.AppendLine("---------------------------------------");
            foreach (var test in failedTests)
            {
                results.AppendLine($"FAILED:\t {test}");
            }

            return results.ToString();
        }
    }
}