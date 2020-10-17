using System;
using System.Linq;
using System.Collections.Generic;

namespace BLL
{
    public class Validator
    {
        public Validator() { }

        // TODO(Arek): Instead of simple bool result implement ValidationResult
        // class through which will be possible to tell which dependencies are
        // incorrect

        public bool ValidateDependencies(
            Package[]           packagesToInstall,
            PackageDependency[] packagesDependencies
        ) {
            if (packagesToInstall    == null) throw new ArgumentNullException(nameof(packagesToInstall));
            if (packagesDependencies == null) throw new ArgumentNullException(nameof(packagesDependencies));

            // determine packages set to install
            var allPackagesToInstall = DetermineAllPackagesToInstall(
                packagesToInstall,
                packagesDependencies);

            // now we have unique list of packages with their versions,
            // so if we group by name we will reveal if more then one
            // version of given package is required to install
            var groupedByName = allPackagesToInstall
                .GroupBy(p => p.Name);

            // all packages can have only single version requested to install
            // if not the configuration is invalid
            return groupedByName.All(g => g.Count() == 1);
        }

        private HashSet<Package> DetermineAllPackagesToInstall(
            Package[]           packagesToInstall,
            PackageDependency[] packagesDependencies
        ) {
            // result as set to not worry about duplicates
            var packagesSet = new HashSet<Package>();

            // convert dependencies array into dictionary to have fast access
            // to given package dependencies list
            var dependenciesByPackage = new Dictionary<Package, List<Package>>();
            foreach (var dependency in packagesDependencies)
            {
                if (dependenciesByPackage.ContainsKey(dependency.Package) == false)
                    dependenciesByPackage.Add(dependency.Package, new List<Package>());

                dependenciesByPackage[dependency.Package].AddRange(dependency.Dependencies);
            }

            // Adds given package dependencies to the result packagesSet
            // recursively so all dependencies dependencies are included
            // as well.
            void addPackageDependencies(Package p)
            {
                if (dependenciesByPackage.ContainsKey(p) == false)
                    return;

                foreach (var dependency in dependenciesByPackage[p])
                {
                    // ignore already added packages to
                    // avoid infinite recursion loops
                    if (packagesSet.Contains(dependency) == false)
                    {
                        packagesSet.Add(dependency);
                        addPackageDependencies(dependency);
                    }
                }
            }

            // finally go through all packages required to install and add
            // them with their dependencies
            foreach (var package in packagesToInstall)
            {
                packagesSet.Add(package);
                addPackageDependencies(package);
            }

            return packagesSet;
        }
    }
}

