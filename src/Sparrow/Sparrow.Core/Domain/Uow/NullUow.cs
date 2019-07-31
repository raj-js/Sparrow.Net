namespace Sparrow.Core.Domain.Uow
{
    public class NullUow : UowBase
    {
        public override void SaveChanges()
        {

        }

        protected override void BeginUow()
        {

        }

        protected override void CompleteUow()
        {

        }

        protected override void DisposeUow()
        {

        }

        public NullUow(
            IConnectionStringResolver connectionStringResolver
        ) : base(connectionStringResolver)
        {
        }
    }
}
