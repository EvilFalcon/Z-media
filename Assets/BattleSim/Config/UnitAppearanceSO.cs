using UnityEngine;

namespace BattleSim.Config
{
    [CreateAssetMenu(fileName = "UnitAppearance", menuName = "BattleSim/Unit Appearance")]
    public sealed class UnitAppearanceSO : ScriptableObject
    {
        [Header("Colors")] [SerializeField] private Color _blue = Color.blue;
        [SerializeField] private Color _green = Color.green;
        [SerializeField] private Color _red = Color.red;

        [Header("Scale")] [SerializeField] private float smallScale = 0.5f;
        [SerializeField] private float _bigScale = 1f;

        public Color GetColor(UnitColorType color)
        {
            return color switch
            {
                UnitColorType.Blue => _blue,
                UnitColorType.Green => _green,
                UnitColorType.Red => _red,
                _ => Color.white
            };
        }

        public float GetScale(UnitSizeType size)
        {
            return size == UnitSizeType.Small ? smallScale : _bigScale;
        }
    }
}