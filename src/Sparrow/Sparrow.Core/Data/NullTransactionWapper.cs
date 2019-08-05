namespace Sparrow.Data
{
    public sealed class NullTransactionWapper : TransactionWapper
    {
        public NullTransactionWapper() :
            base(TransactionOptions.NoTransaction)
        {

        }
    }
}
