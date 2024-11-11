using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MultithreadTaskController : ControllerBase
    {
        private static int _counter = 0;
        private int incrementsPerThread = 10000000;

        [HttpGet("threadSafe")]
        public async Task<IActionResult> GetThreadSafe()
        {
            _counter = 0; 
            var tasks = new List<Task>();

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < incrementsPerThread; i++)
            {
                tasks.Add(Task.Run(() => IncrementCounterThreadSafe()));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            var execTime = stopwatch.ElapsedMilliseconds;

            return Ok(new { message = "Thread safe count", count = _counter, execTime = execTime.ToString() });
        }

        [HttpGet("threadUnsafe")]
        public async Task<IActionResult> GetThreadUnsafe()
        {
            _counter = 0; 
            var tasks = new List<Task>();

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < incrementsPerThread; i++)
            {
                tasks.Add(Task.Run(() => IncrementCounterThreadUnsafe()));
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            var execTime = stopwatch.ElapsedMilliseconds;

            return Ok(new { message = "thread unsafe count", count = _counter , execTime = execTime.ToString() });
        }

        private void IncrementCounterThreadSafe()
        {
            lock (this) 
            {
                _counter++;
            }
        }

        private void IncrementCounterThreadUnsafe()
        {
            _counter++;
        }
    }
}
