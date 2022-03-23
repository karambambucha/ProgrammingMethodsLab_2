using System;

namespace MenuStripCreator
{
    public class MethodArgs : EventArgs
    {
        public string Value { get; private set; }
        public MethodArgs(string value)
        {
            this.Value = value;
        }
    }
}
