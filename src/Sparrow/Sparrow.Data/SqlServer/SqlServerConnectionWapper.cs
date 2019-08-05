namespace Sparrow.Data.SqlServer
{
    public class SqlServerConnectionWapper : ConnectionWapperBase, IConnectionWapper
    {
        public override ITransactionWapper BeginTransaction(TransactionOptions options)
        {
            var transactionWapper = new TransactionWapper(options);

            if (options.IsTransactional)
                transactionWapper.DbTransaction = DbConnection.BeginTransaction(options.IsolationLevel);
            else
            {
                
            }

            return null;
        }
    }
}
