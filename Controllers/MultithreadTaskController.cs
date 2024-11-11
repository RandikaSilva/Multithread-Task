using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MultithreadTaskController : ControllerBase
    {
        private static int _counter = 0;
        private int incrementsPerThread = 1000000;

        [HttpGet("threadSafe")]
        public async Task<IActionResult> GetThreadSafe()
        {
            _counter = 0; 
            var tasks = new List<Task>();

            for (int i = 0; i < incrementsPerThread; i++)
            {
                tasks.Add(Task.Run(() => IncrementCounterThreadSafe()));
            }

            await Task.WhenAll(tasks);

            return Ok(new { message = "Thread safe count", count = _counter });
        }

        [HttpGet("threadUnsafe")]
        public async Task<IActionResult> GetThreadUnsafe()
        {
            _counter = 0; 
            var tasks = new List<Task>();

            for (int i = 0; i < incrementsPerThread; i++)
            {
                tasks.Add(Task.Run(() => IncrementCounterThreadUnsafe()));
            }

            await Task.WhenAll(tasks);

            return Ok(new { message = "thread unsafe count", count = _counter });
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
