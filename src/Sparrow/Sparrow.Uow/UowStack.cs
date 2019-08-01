using System;
using System.Collections.Generic;

namespace Sparrow.Uow
{
    public class UowStack<TUow>: IDisposable where TUow : IUow
    {
        protected Stack<TUow> Stacks { get; set; }

        public void Dispose()
        {
            
        }
    }
}
