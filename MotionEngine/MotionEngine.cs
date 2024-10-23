using Microsoft.Extensions.Hosting;

namespace MotionEngine
{
    public class MotionEngine : BackgroundService
    { 
        TestDataSource testDataSource;
        private CancellationTokenSource _cts;
        private readonly TestState _testState;

        public MotionEngine(TestState testState)
        {
            testDataSource = new TestDataSource();
            _cts = new CancellationTokenSource();
            _testState = testState ?? throw new ArgumentNullException(nameof(testState));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Combine the stopping token with the internal cancellation token source
            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _cts.Token))
            {
                await PollAsync(linkedCts.Token);
            }
        }

        private async Task PollAsync(CancellationToken token)
        {

            while (!token.IsCancellationRequested)
            {
                var result = testDataSource.GetCurrent();
                _testState.SetSceneObjects(result.Objects);

                testDataSource.MoveNext();

                await Task.Delay(1000 * 1, token);


            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            // Clean-up logic, if any
            return Task.CompletedTask;
        }
    }
}
