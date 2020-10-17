using System;
using System.Linq;

namespace BLL
{
    public class ParseResult
    {
        public ParseSyntaxError    SyntaxError          { get; private set; }
        public Package[]           PackagesToInstall    { get; private set; }
        public PackageDependency[] PackagesDependencies { get; private set; }

        private ParseResult() { }

        public static ParseResult FromSyntaxError(ParseSyntaxError error)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));

            return new ParseResult()
            {
                SyntaxError          = error,
                PackagesToInstall    = null,
                PackagesDependencies = null
            };
        }

        public static ParseResult FromPackages(
            Package[]           toInstall,
            PackageDependency[] dependencies
        ) {
            if (toInstall    == null) throw new ArgumentNullException(nameof(toInstall));
            if (dependencies == null) throw new ArgumentNullException(nameof(dependencies));

            return new ParseResult()
            {
                SyntaxError          = null,
                PackagesToInstall    = toInstall,
                PackagesDependencies = dependencies
            };
        }

        public override bool Equals(object obj)
        {
            var val = obj as ParseResult;

            if (val == null)
                return false;

            return
                SyntaxError                 == val.SyntaxError            &&
                // first check just the lengths
                PackagesToInstall.Length    == val.PackagesToInstall.Length    &&
                PackagesDependencies.Length == val.PackagesDependencies.Length &&
                // if lengths are equal then we have to check whole sequences
                PackagesToInstall.SequenceEqual(val.PackagesToInstall)        &&
                PackagesDependencies.SequenceEqual(val.PackagesDependencies);
        }

        public override int GetHashCode()
        {
            return new
            {
                SyntaxError,
                PackagesToInstall,
                PackagesDependencies
            }.GetHashCode();
        }

        public override string ToString()
        {
            if (SyntaxError != null)
                return SyntaxError.ToString();
            else
                return $"PackagesToInstall ({PackagesToInstall.Length}): [{string.Join(", ", (object[])PackagesToInstall)}], " +
                       $"PackagesDependencies ({PackagesDependencies.Length}): [{string.Join(", ", (object[])PackagesDependencies)}]";
        }
    }
}

