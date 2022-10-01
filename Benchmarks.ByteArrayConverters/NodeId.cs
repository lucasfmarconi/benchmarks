// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
namespace Benchmarks.ByteArrayConverters;

public class NodeId
{
    public NodeId(string nodeId)
    { 
        Id = nodeId;
    }

    public virtual string Id { get; }

    public override string ToString() => Id;

    public static implicit operator NodeId(string id) => new(id);
}
