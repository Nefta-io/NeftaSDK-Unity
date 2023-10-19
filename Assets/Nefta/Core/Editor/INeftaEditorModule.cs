using System;
using System.Collections.Generic;

namespace Nefta.Core.Editor
{
    public interface INeftaEditorModule
    {
        void AddPages(Dictionary<string, Action> pages);
    }
}