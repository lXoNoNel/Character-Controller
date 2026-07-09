

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

    public bool IsChildOf(ChainedEnum potentialParent)
    {
        if (Parent == null) return false;
        if (Parent == potentialParent) return true;
        return Parent.IsChildOf(potentialParent); // Recursively looks all the way up the tree!
    }
}

