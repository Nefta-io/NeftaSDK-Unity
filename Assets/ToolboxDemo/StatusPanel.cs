using UnityEngine;
using UnityEngine.UI;

namespace ToolboxDemo
{
    public class StatusPanel : MonoBehaviour
    {
        private readonly Color[] _backgroundColors = new Color[]
        {
            Color.white,
            new Color(0.390625f, 0.390625f, 0.390625f, 0.390625f),
            new Color(1, 0, 0, 0.390625f)
        };

        public enum Types
        {
            None,
            Loading,
            Error
        }
        
        [SerializeField] private Image _background;
        [SerializeField] private Text _text;
        [SerializeField] private Image _spinner;
        [SerializeField] private Button _button;

        private Types _currentShowMode;

        public void Init()
        {
            _button.onClick.AddListener(Hide);
        }
        
        public void Show(string status, Types type)
        {
            gameObject.SetActive(true);
            _text.text = status;

            _currentShowMode = type;
            _background.color = _backgroundColors[(int) _currentShowMode];
            _spinner.enabled = _currentShowMode == Types.Loading;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_currentShowMode == Types.Loading)
            {
                _spinner.transform.localRotation *= Quaternion.Euler(0, 0, -50 * Time.deltaTime);
            }
        }
    }
}

