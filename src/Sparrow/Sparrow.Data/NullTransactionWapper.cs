namespace Sparrow.Data
{
    public sealed class NullTransactionWapper : TransactionWapperBase
    {
        public NullTransactionWapper() :
            base(TransactionOptions.NoTransaction)
        {

        }
    }
}
