using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Game._Scripts.Lvl.UI
{
    public class LevelButtonsPanel : MonoBehaviour
    {
        [SerializeField] private Button _btnRestart;
        [SerializeField] private Button _btnNext;

        private LevelFlowController _flow;

        [Inject]
        public void Construct(LevelFlowController flow) => _flow = flow;

        private void Awake()
        {
            _btnRestart.onClick.AddListener(_flow.RestartCurrentLevel);
            _btnNext.onClick.AddListener(_flow.PlayNextLevel);
        }

        private void OnDestroy()
        {
            _btnRestart.onClick.RemoveAllListeners();
            _btnNext.onClick.RemoveAllListeners();
        }
    }
}