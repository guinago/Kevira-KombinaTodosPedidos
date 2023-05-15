using System;

namespace KeViraKombinaTodos.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ComponentAttribute : Attribute
    {
        #region Public Properties

        public string Name { get; set; }

        #endregion
    }
}
