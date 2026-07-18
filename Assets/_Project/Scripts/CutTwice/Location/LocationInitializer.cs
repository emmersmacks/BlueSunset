using System.Threading;
using CutTwice.App.LoadingScreen;
using CutTwice.Core.GameStates;
using CutTwice.Core.Lifecycle;
using CutTwice.Location.States;
using CutTwice.Menu.States;
using Cysharp.Threading.Tasks;

namespace CutTwice.Location
{
    public class LocationInitializer : IInitializable
    {
        private readonly MenuStateMachine _menuStateMachine;
        private readonly LoadingScreenController _loadingScreenController;

        public LocationInitializer(MenuStateMachine menuStateMachine, LoadingScreenController loadingScreenController)
        {
            _menuStateMachine = menuStateMachine;
            _loadingScreenController = loadingScreenController;
        }
        
        public async UniTask InitAsync(CancellationToken ct)
        {
            await _menuStateMachine.SetStateAsync<MainLocationState>(ct);
            _loadingScreenController.Hide();
        }
    }
}