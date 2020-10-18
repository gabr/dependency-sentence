using System;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Represents a single package in its specific version.
    /// </summary>
    public class Package
    {
        public string Name    { get; }
        public string Version { get; }

        public Package(
            string name,
            string version
        ) {
            if (string.IsNullOrWhiteSpace(name))    throw new ArgumentException($"Given {nameof(name)} is null or empty: '{name ?? "null"}'", nameof(name));
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentException($"Given {nameof(version)} is null or empty: '{version ?? "null"}'", nameof(version));

            Name    = name.Trim();
            Version = version.Trim();
        }

        public static bool Equals(Package left, Package right)
        {
            if ((object)left == (object)right)
                return true;

            if ((object)left == null || (object)right == null)
                return false;

            return
                left.Name    == right.Name &&
                left.Version == right.Version;
        }

        public static bool operator == (Package left, Package right) => Package.Equals(left, right);
        public static bool operator != (Package left, Package right) => !Package.Equals(left, right);

        public override bool Equals(object obj) => Package.Equals(this, obj as Package);

        public override int GetHashCode()
        {
            return new
            {
                Name,
                Version
            }.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}

