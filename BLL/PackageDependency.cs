using System;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Represents package dependencies.
    /// Single package in specific version may require other packages in
    /// specific versions and this class allows to represent such relation.
    /// </summary>
    public class PackageDependency
    {
        public Package   Package      { get; }
        public Package[] Dependencies { get; }

        public PackageDependency(
            Package   package,
            Package[] dependencies
        ) {
            if (package      == null) throw new ArgumentNullException(nameof(package));
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            Package      = package;
            Dependencies = dependencies;
        }

        public static bool Equals(PackageDependency left, PackageDependency right)
        {
            if ((object)left == (object)right)
                return true;

            if ((object)left == null || (object)right == null)
                return false;

            return
                left.Package             == right.Package             &&
                left.Dependencies.Length == right.Dependencies.Length &&
                left.Dependencies.SequenceEqual(right.Dependencies);
        }

        public static bool operator == (PackageDependency left, PackageDependency right) => PackageDependency.Equals(left, right);
        public static bool operator != (PackageDependency left, PackageDependency right) => !PackageDependency.Equals(left, right);

        public override bool Equals(object obj) => PackageDependency.Equals(this, obj as PackageDependency);

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

