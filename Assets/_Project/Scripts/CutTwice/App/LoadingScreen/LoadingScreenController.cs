using UnityEngine;

namespace CutTwice.App.LoadingScreen
{
    public class LoadingScreenController
    {
        private readonly LoadingScreenView _view;

        public LoadingScreenController(LoadingScreenView view)
        {
            _view = view;
            _view.gameObject.SetActive(false);
        }

        public void Show()
        {
            _view.gameObject.SetActive(true);
            SetProgress(0f);
        }

        public void Hide()
        {
            _view.gameObject.SetActive(false);
        }

        public void SetProgress(float progress01)
        {
            _view.ProgressBar.fillAmount = progress01;
            _view.ProgressText.text = $"{Mathf.RoundToInt(progress01 * 100)}%";
        }
    }
}
