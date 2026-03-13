using NUnit.Framework;

// Enable parallel test execution at all levels (fixtures and their children)
// This allows cross-browser tests (fixtures) AND scenarios to run in parallel
[assembly: Parallelizable(ParallelScope.All)]

// Set maximum degree of parallelism (number of concurrent test workers)
// For cross-browser testing: 3 browsers × number of parallel tests per browser
// Adjust based on your machine's capabilities (CPU cores and memory)
[assembly: LevelOfParallelism(6)]
