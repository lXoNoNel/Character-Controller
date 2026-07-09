public interface IActionSubState { }

public class xActionState : ChainedEnum
{
    private xActionState(string name, ChainedEnum? parent = null) : base(name, parent) { }

    public static readonly ActionGroup Actions = new();

    public class ActionGroup : ChainedEnum
    {
        public readonly ActionLeaf Idle = new("Idle", null);
        public readonly ActionLeaf Reloading = new("Reloading", null);
        public readonly ActionLeaf Shooting = new("Shooting", null);

        public ActionGroup() : base("Actions") { }

        public class ActionLeaf : xActionState, IActionSubState
        {
            public ActionLeaf(string name, ChainedEnum? parent) : base(name, parent) { }
        }
    }
}