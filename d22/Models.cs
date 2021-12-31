using System.Numerics;

record Step(bool isOn, Cuboid cuboid);

record Voxel(int x, int y, int z);

record Cuboid(int x1, int x2, int y1, int y2, int z1, int z2)
{
    public ulong GetVolume() => (ulong)this.Width * (ulong)this.Height * (ulong)this.Depth;

    public bool Overlaps(Cuboid other)
        => !(this.x1 > other.x2) && !(this.x2 < other.x1) &&
            !(this.y1 > other.y2) && !(this.y2 < other.y1) &&
            !(this.z1 > other.z2) && !(this.z2 < other.z1);

    public Cuboid? Intersection(Cuboid other)
    {
        if (!this.Overlaps(other))
        {
            return null;
        }

        return new Cuboid(
            x1: Math.Max(this.x1, other.x1),
            x2: Math.Min(this.x2, other.x2),
            y1: Math.Max(this.y1, other.y1),
            y2: Math.Min(this.y2, other.y2),
            z1: Math.Max(this.z1, other.z1),
            z2: Math.Min(this.z2, other.z2));
    }

    public List<Cuboid> Except(Cuboid other)
    {
        if (other == null || other == this)
        {
            return new List<Cuboid>();
        }

        var above = new Cuboid(this.x1, this.x2, this.y1, other.y1 - 1, this.z1, this.z2);
        var below = new Cuboid(this.x1, this.x2, other.y2 + 1, this.y2, this.z1, this.z2);
        var left = new Cuboid(this.x1, other.x1 - 1, other.y1, other.y2, this.z1, this.z2);
        var right = new Cuboid(other.x2 + 1, this.x2, other.y1, other.y2, this.z1, this.z2);
        var front = new Cuboid(other.x1, other.x2, other.y1, other.y2, other.z2 + 1, this.z2);
        var rear = new Cuboid(other.x1, other.x2, other.y1, other.y2, this.z1, other.z1 - 1);

        return new[] { above, left, rear, front, right, below }.Where(item => !item.IsInvertedOrEmpty).ToList();
    }

    public long Width => 1 + this.x2 - this.x1;
    public long Height => 1 + this.y2 - this.y1;
    public long Depth => 1 + this.z2 - this.z1;

    public bool IsInvertedOrEmpty => this.Width <= 0 || this.Height <= 0 || this.Depth <= 0;

    public IEnumerable<Voxel> GetVoxels(bool applyPart1Bounds = false)
    {
        int lowerX = x1;
        int upperX = x2;
        int lowerY = y1;
        int upperY = y2;
        int lowerZ = z1;
        int upperZ = z2;

        if (applyPart1Bounds)
        {
            lowerX = Math.Max(-50, lowerX);
            upperX = Math.Min(50, upperX);
            lowerY = Math.Max(-50, lowerY);
            upperY = Math.Min(50, upperY);
            lowerZ = Math.Max(-50, lowerZ);
            upperZ = Math.Min(50, upperZ);
        }

        for (int x = lowerX; x <= upperX; x++)
        {
            for (int y = lowerY; y <= upperY; y++)
            {
                for (int z = lowerZ; z <= upperZ; z++)
                {
                    yield return new Voxel(x, y, z);
                }
            }
        }
    }
}