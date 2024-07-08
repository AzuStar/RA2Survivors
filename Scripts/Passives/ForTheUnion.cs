namespace RA2Survivors
{
    public partial class ForTheUnion : Passive
    {
        public UpgradeButtonSettings[] upgrades;
        public double attackSpeedBoost = 0.05;
        public int stackLimit = 5;
        public double damageBoost = 0.00;

        public double stackGainTime = 1;
        public double stackLossTime = 2;

        public bool invertedLogic = false;

        private int _stackCount;
        private double _stackGain;
        private double _timeout;

        private EState _state = EState.Standing;

        public ForTheUnion()
        {
            upgrades =
            [
                new UpgradeButtonSettings
                {
                    resourcePath = "Upgrades/UpgradeAttackSpeed.tscn",
                    callback = () =>
                    {
                        attackSpeedBoost += 0.01;
                    }
                },
                new UpgradeButtonSettings
                {
                    resourcePath = "Upgrades/UpgradeDamage.tscn",
                    callback = () =>
                    {
                        damageBoost += 1;
                        attackSpeedBoost -= 0.02;
                    }
                },
                new UpgradeButtonSettings
                {
                    resourcePath = "Upgrades/UpgradeStackLimit.tscn",
                    callback = () =>
                    {
                        stackLimit += 1;
                    }
                },
            ];
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (invertedLogic)
            {
                if (owner.movementVelocity.Length() > 0)
                    SwitchState(EState.Standing);
                else
                    SwitchState(EState.Moving);
            }
            else
            {
                if (owner.movementVelocity.Length() > 0)
                    SwitchState(EState.Moving);
                else
                    SwitchState(EState.Standing);
            }

            ProcessStates(delta);
        }

        public void SwitchState(EState newState)
        {
            if (_state != newState)
            {
                _state = newState;
                _timeout = 0;
            }
        }

        public void ProcessStates(double delta)
        {
            switch (_state)
            {
                // while standing still, every 1 second gain a stack
                case EState.Standing:
                    _timeout += delta;
                    if (_timeout >= stackGainTime)
                    {
                        _timeout = 0;
                        if (_stackCount < stackLimit)
                            UpdateStack(_stackCount + 1);
                    }
                    break;
                // after starting moving, lose all stacks after 2 seconds
                case EState.Moving:
                    _timeout += delta;
                    if (_timeout >= stackLossTime)
                    {
                        _timeout = 0;
                        UpdateStack(0);
                    }
                    break;
            }
        }

        public void UpdateStack(int newStack)
        {
            owner.stats.attackSpeed -= attackSpeedBoost * _stackCount;
            owner.stats.damage -= damageBoost * _stackCount;
            _stackCount = newStack;
            owner.stats.attackSpeed += attackSpeedBoost * _stackCount;
            owner.stats.damage += damageBoost * _stackCount;
        }

        public enum EState
        {
            Standing,
            Moving
        }
    }
}
