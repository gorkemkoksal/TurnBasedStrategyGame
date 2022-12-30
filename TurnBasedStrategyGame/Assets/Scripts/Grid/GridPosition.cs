using System;
public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;
    public GridPosition(int x, int z) { this.x = x; this.z = z; }
    public override bool Equals(object obj) => obj is GridPosition position && x == position.x && z == position.z;
    public bool Equals(GridPosition other) => this == other;
    public override int GetHashCode() => HashCode.Combine(x, z);
    public override string ToString() => $"x: {x} z: {z}";
    public static bool operator ==(GridPosition a, GridPosition b) => a.x == b.x && a.z == b.z;
    public static bool operator !=(GridPosition a, GridPosition b) => !(a == b);
    public static GridPosition operator +(GridPosition a, GridPosition b) => new GridPosition(a.x + b.x, a.z + b.z);
    public static GridPosition operator -(GridPosition a, GridPosition b) => new GridPosition(a.x - b.x, a.z - b.z);

}
