using System;
using System.Linq;

namespace BLL
{
    /// <summary>
    /// Represents result of parsing packages description.
    /// Such description consists of packages which have to be installed and
    /// list of this packages dependencies.
    ///
    /// This class does not guaranties the description to be valid.  It allows
    /// to store all possible descriptions: valid and invalid ones.
    /// </summary>
    public class ParseResult
    {
        public Package[]           PackagesToInstall    { get; private set; }
        public PackageDependency[] PackagesDependencies { get; private set; }

        public ParseResult(
            Package[]           packagesToInstall,
            PackageDependency[] packagesDependencies
        ) {
            if (packagesToInstall    == null) throw new ArgumentNullException(nameof(packagesToInstall));
            if (packagesDependencies == null) throw new ArgumentNullException(nameof(packagesDependencies));

            PackagesToInstall    = packagesToInstall;
            PackagesDependencies = packagesDependencies;
        }

        public static bool Equals(ParseResult left, ParseResult right)
        {
            if ((object)left == (object)right)
                return true;

            if ((object)left == null || (object)right == null)
                return false;

            return
                // first check just the lengths
                left.PackagesToInstall.Length    == right.PackagesToInstall.Length    &&
                left.PackagesDependencies.Length == right.PackagesDependencies.Length &&
                // if lengths are equal then we have to check whole sequences
                left.PackagesToInstall.SequenceEqual(right.PackagesToInstall)         &&
                left.PackagesDependencies.SequenceEqual(right.PackagesDependencies);
        }

        public static bool operator == (ParseResult left, ParseResult right) => ParseResult.Equals(left, right);
        public static bool operator != (ParseResult left, ParseResult right) => !ParseResult.Equals(left, right);

        public override bool Equals(object obj) => ParseResult.Equals(this, obj as ParseResult);

        public override int GetHashCode()
        {
            return new
            {
                PackagesToInstall,
                PackagesDependencies
            }.GetHashCode();
        }

        public override string ToString()
        {
            return $"PackagesToInstall ({PackagesToInstall.Length}): [{string.Join(", ", (object[])PackagesToInstall)}], " +
                   $"PackagesDependencies ({PackagesDependencies.Length}): [{string.Join(", ", (object[])PackagesDependencies)}]";
        }
    }
}

