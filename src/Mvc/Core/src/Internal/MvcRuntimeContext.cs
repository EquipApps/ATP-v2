using EquipApps.Mvc;
using EquipApps.Mvc.Runtime;
using EquipApps.Testing;
using System;

namespace NLib.AtpNetCore.Testing.Mvc.Internal
{
    public class MvcRuntimeContext : RuntimeContext, IDisposable
    {
        private IRuntimeStateCollection stateCollection;
        private IRuntimeStateEnumerator stateEnumerator;



        private IRuntimeEnumerator _enumerator;

        private IActionInvokerFactory _invokerFactory;



        public MvcRuntimeContext(
            TestContext testContext,
            IRuntimeStateCollection stateCollection,
            IRuntimeEnumerator enumerator,
            IActionInvokerFactory invokerFactory)
        {
            TestContext = testContext ?? throw new ArgumentNullException(nameof(testContext));


            this.stateCollection = stateCollection ?? throw new ArgumentNullException(nameof(stateCollection));
            this.stateEnumerator = stateCollection.GetEnumerator();


            _enumerator = enumerator;

            _invokerFactory = invokerFactory;

        }

        //--
        public override TestContext TestContext { get; }

        //--

        /* Unmerged change from project 'AtpNetCore.Mvc.Core (netcoreapp3.1)'
        Before:
                public override IRuntimeStateEnumerator StateEnumerator => stateEnumerator;









                #region Base
        After:
                public override IRuntimeStateEnumerator StateEnumerator => stateEnumerator;









                #region Base
        */
        public override IRuntimeStateEnumerator StateEnumerator => stateEnumerator;









        #region Base



        //--
        public override IRuntimeEnumerator Enumerator => _enumerator;

        //--


        //--
        public override IActionInvokerFactory Factory => _invokerFactory;



        #endregion































        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //dispose managed state (managed objects).
                    StateEnumerator.Dispose();






                    _enumerator.Dispose();
                    _enumerator = null;

                    _invokerFactory.Dispose();
                    _invokerFactory = null;
                }

                //
                // set large fields to null.               
                stateCollection = null;
                stateEnumerator = null;

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }


        #endregion

    }
}
