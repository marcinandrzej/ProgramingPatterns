using Cysharp.Threading.Tasks;
using System.Threading;

namespace Runtime.Command.Base
{
    public interface ICommand<ContextT>
    {
        public UniTask Execute(ContextT context, CancellationToken cancel);
    }
}