namespace HttpDriver.Controllers.Sockets
{
    public class TightLoopTimer : EventWaitHandle
    {
        private readonly Timer _timer;

        #region Constructors

        private TightLoopTimer() : base(false, EventResetMode.AutoReset)
        {
            _timer = new Timer(_ => Set());
        }

        public static TightLoopTimer Create(TimeSpan interval)
        {
            var tl = new TightLoopTimer();
            tl.SetInterval(interval);
            return tl;
        }

        #endregion

        #region Methods

        private void SetInterval(TimeSpan duration)
        {
            _timer.Change(0, (int)duration.TotalMilliseconds);
        }

        #endregion

        #region Overrides of WaitHandle

        protected override void Dispose(bool explicitDisposing)
        {
            if (explicitDisposing)
            {
                _timer.Dispose();
            }

            base.Dispose(explicitDisposing);
        }

        #endregion
    }
}