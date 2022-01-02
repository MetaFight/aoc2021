using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common
{
    public class AutoStopwatch : IDisposable
    {
        private readonly string name;
        private readonly Stopwatch stopwatch;
        private bool isDisposed;

        public AutoStopwatch([CallerMemberName] string name = "")
        {
            this.name = name;
            this.stopwatch = new Stopwatch();

            Utils.print("starting", name);
            this.stopwatch.Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                this.stopwatch.Stop();
                Utils.print(this.stopwatch.Elapsed, this.name);
                
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
