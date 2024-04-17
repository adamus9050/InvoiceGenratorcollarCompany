using NUnit;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Transactions;

namespace InvoiceGenerator.IntegrationTest
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope _transactionScope;
        public ActionTargets Targets => ActionTargets.Test;

        public void AfterTest(ITest test)
        {
            _transactionScope.Dispose(); // jest to rollback po tescie, eby nie zapisywać nowych rekordów do bazy przy każdym tesowanu.
        }

        public void BeforeTest(ITest test)
        {
            _transactionScope = new TransactionScope();
        }
    }
}
