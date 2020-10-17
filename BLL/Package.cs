using System;
using System.Linq;

namespace BLL
{
    public class Package
    {
        public string Name    { get; }
        public string Version { get; }

        private Package(
            string name,
            string version
        ) {
            if (string.IsNullOrWhiteSpace(name))    throw new ArgumentException($"Given {nameof(name)} is null or empty: '{name ?? "null"}'", nameof(name));
            if (string.IsNullOrWhiteSpace(version)) throw new ArgumentException($"Given {nameof(version)} is null or empty: '{version ?? "null"}'", nameof(version));

            Name    = name.Trim();
            Version = version.Trim();
        }

        public override bool Equals(object obj)
        {
            var val = obj as Package;

            if (val == null)
                return false;

            return
                Name    == val.Name &&
                Version == val.Version;
        }

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

