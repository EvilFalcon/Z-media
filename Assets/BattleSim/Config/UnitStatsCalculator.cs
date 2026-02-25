using UnityEngine;

namespace BattleSim.Config
{
    public sealed class UnitStatsCalculator : IUnitStatsCalculator
    {
        private readonly UnitBaseStatsSO _baseStats;
        private readonly FormModifiersSO _formModifiers;
        private readonly SizeModifiersSO _sizeModifiers;
        private readonly ColorModifiersSO _colorModifiers;

        public UnitStatsCalculator(
            UnitBaseStatsSO baseStats,
            FormModifiersSO formModifiers,
            SizeModifiersSO sizeModifiers,
            ColorModifiersSO colorModifiers)
        {
            _baseStats = baseStats;
            _formModifiers = formModifiers;
            _sizeModifiers = sizeModifiers;
            _colorModifiers = colorModifiers;
        }

        public void Compute
        (
            UnitFormType form,
            UnitSizeType size,
            UnitColorType color,
            out int hp,
            out int atk,
            out int speed,
            out float atkSpd
        )
        {
            hp = _baseStats.BaseHp
                 + _formModifiers.GetHpBonus(form)
                 + _sizeModifiers.GetHpBonus(size);
            atk = _baseStats.BaseAtk + _formModifiers.GetAtkBonus(form);
            speed = _baseStats.BaseSpeed;
            atkSpd = _baseStats.BaseAtkSpd;

            _colorModifiers.GetModifiers(color, out int chp, out int catk, out int cspd, out float catkSpd);
            hp += chp;
            atk += catk;
            speed += cspd;
            atkSpd += catkSpd;

            hp = Mathf.Max(1, hp);
            atk = Mathf.Max(0, atk);
            speed = Mathf.Max(0, speed);
            atkSpd = Mathf.Max(0.1f, atkSpd);
        }
    }

    public interface IUnitStatsCalculator
    {
        public void Compute
        (
            UnitFormType form,
            UnitSizeType size,
            UnitColorType color,
            out int hp,
            out int atk,
            out int speed,
            out float atkSpd
        );
    }
}