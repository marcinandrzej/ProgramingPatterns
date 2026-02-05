using System.Threading.Tasks;

namespace Runtime.Command.Base
{
    public interface ICommand<ContextT>
    {
        public Task Execute(ContextT context);
    }
}