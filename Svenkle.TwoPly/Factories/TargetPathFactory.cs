using System;
using System.Linq;
using Svenkle.TwoPly.Extensions;
using Svenkle.TwoPly.Factories.Interfaces;
using Svenkle.TwoPly.Models;
using Svenkle.TwoPly.Models.Interfaces;

namespace Svenkle.TwoPly.Factories
{
    public class TargetPathFactory : ITargetPathFactory
    {
        public ITargetPath Create(string syntax)
        {
            var path = syntax.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
            var location = path.LastOrDefault()?.Trim();

            if (path.Length != 2 || string.IsNullOrEmpty(location))
                throw new ArgumentException("Path is missing or invalid");

            return new TargetPath(location);
        }
    }
}