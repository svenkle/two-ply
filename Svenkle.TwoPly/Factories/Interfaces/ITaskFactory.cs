using System.Collections.Generic;
using Microsoft.Build.Framework;

namespace Svenkle.TwoPly.Factories.Interfaces
{
    public interface ITaskFactory
    {
        bool CanCreate(IReadOnlyList<string> tokens);
        ITask Create(IReadOnlyList<string> tokens);
    }
}
