namespace ITOps.ViewModelComposition.Mvc
{
    using System;

    public sealed class UICompositionSupportAttribute : Attribute
    {
        public UICompositionSupportAttribute(string baseNamespace)
        {
            this.BaseNamespace = baseNamespace;
        }

        public string BaseNamespace { get; private set; }
    }
}