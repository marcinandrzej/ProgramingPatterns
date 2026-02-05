using Runtime.Agents;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Command.AgentCommands
{
    public class SeekCommand : AgentCommandBase
    {
        public async override Task Execute(Agent context)
        {
            Debug.Log("START");
            await Awaitable.WaitForSecondsAsync(2);
            Debug.Log("END");
        }
    }
}