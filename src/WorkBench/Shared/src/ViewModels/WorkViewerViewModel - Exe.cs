using System;
using System.Threading;
using System.Threading.Tasks;

namespace EquipApps.WorkBench.ViewModels
{
    public partial class WorkViewerViewModel
    {
        private volatile CancellationTokenSource cts;

        private async Task TestStartAsync()
        {
            cts = new CancellationTokenSource();

            try
            {
                IsTesting = true;

                var testCurrent = test;
                if (testCurrent == null)
                {
                    throw new NullReferenceException(nameof(testCurrent));
                }

                await testCurrent.ProcessAsync(cts.Token);

            }
            catch (Exception ex)
            {
                await Interactions.HandleCreateException(ex);
            }
            finally
            {
                IsTesting = false;
            }
        }

        private void TestCancel()
        {
            if ((cts != null) && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }
    }
}
