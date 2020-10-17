using System;
using System.Linq;

namespace BLL
{
    public class PackageDependency
    {
        public Package   Package      { get; }
        public Package[] Dependencies { get; }

        private PackageDependency(
            Package   package,
            Package[] dependencies
        ) {
            if (package      == null) throw new ArgumentNullException(nameof(package));
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            Package      = package;
            Dependencies = dependencies;
        }

        public override bool Equals(object obj)
        {
            var val = obj as PackageDependency;

            if (val == null)
                return false;

            return
                Package             == val.Package             &&
                Dependencies.Length == val.Dependencies.Length &&
                Dependencies.SequenceEqual(val.Dependencies);
        }

        public override int GetHashCode()
        {
            return new
            {
                Package,
                Dependencies
            }.GetHashCode();
        }

        public override string ToString()
        {
            return $"Package: {Package}, Dependencies ({Dependencies.Length}): [{string.Join(", ", (object[])Dependencies)}]";
        }
    }
}

