namespace CineTerra.Models.Common;

/// <summary>
/// Root of every identifiable domain object.
/// </summary>
public abstract class BaseEntity
{
    public string Id { get; init; } = string.Empty;

    public override bool Equals(object? obj) =>
        obj is BaseEntity other && GetType() == other.GetType() && Id == other.Id;

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);
}
