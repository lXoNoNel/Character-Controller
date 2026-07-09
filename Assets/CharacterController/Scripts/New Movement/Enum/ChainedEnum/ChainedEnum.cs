

public abstract class ChainedEnum
{
    public string Name { get; }
    public ChainedEnum? Parent { get; }

    protected ChainedEnum(string name, ChainedEnum? parent = null)
    {
        Name = name;
        Parent = parent;
    }

    public string FullPath => Parent == null ? Name : $"{Parent.FullPath}/{Name}";
    
    public override string ToString() => Name;
}