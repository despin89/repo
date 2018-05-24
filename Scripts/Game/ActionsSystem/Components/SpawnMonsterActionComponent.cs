namespace GD.Game.ActionsSystem
{
    using Combat;

    public class SpawnMonsterActionComponent : ComponentBase
    {
        public MonsterInfo MonsterInfo { get; }

        public SpawnMonsterActionComponent(SpawnMonsterActionComponentInfo info)
        {
            this.MonsterInfo = info.MonsterInfo;
        }
    }
}
