using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OsosOracle.Framework.Aspects.TransactionAspects
{
    /// <summary>
    /// Method başlangıcında Transaction başlatır
    /// Daha önce başlatılmış ise var olan ile birleştirir
    /// TransactionScopeOption belitrilerek var olan ile ayrılabilir
    /// </summary>
    [Serializable]
    public sealed class TransactionScopeAspect : OnMethodBoundaryAspect
    {
        private readonly TransactionScopeOption _option;

        public TransactionScopeAspect()
        {
            AspectPriority = 2;
        }

        public TransactionScopeAspect(TransactionScopeOption option)
        {
            _option = option;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = new TransactionScope(_option);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            ((TransactionScope)args.MethodExecutionTag).Dispose();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            ((TransactionScope)args.MethodExecutionTag).Complete();
        }
    }
}
