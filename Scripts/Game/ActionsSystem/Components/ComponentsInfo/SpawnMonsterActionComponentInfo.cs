namespace GD.Game.ActionsSystem
{
    using Combat;

    public class SpawnMonsterActionComponentInfo : ComponentInfoBase
    {
        public MonsterInfo MonsterInfo;

        public override ComponentBase GetInstance()
        {
            return new SpawnMonsterActionComponent(this);
        }
    }
}
