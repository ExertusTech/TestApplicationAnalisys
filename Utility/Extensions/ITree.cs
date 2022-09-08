namespace Common.Utility.Extensions
{
    public interface ITree
    {
        int Id { get; }

        int ParentId { get; }
        int Order { get; }
        int Level { get; set; }

        bool IsLeaf { get; set; }
    }
}
