using System.Collections;
using BaseGame.Core.DI;

namespace BaseGame.Core.Interfaces
{
    public interface ISceneEntryPoint
    {
        public IEnumerator Init(DIContainer rootContainer);
        public void Run();
    }
}