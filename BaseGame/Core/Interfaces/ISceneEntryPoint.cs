using System.Collections;
using System.Threading.Tasks;
using BaseGame.Core.DI;
using UnityEngine;

namespace BaseGame.Core.Interfaces
{
    public interface ISceneEntryPoint
    {
        public Task Init(DIContainer rootContainer);
        public void Run();
    }
}