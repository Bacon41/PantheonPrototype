using System;

namespace FuncWorks.XNA.XTiled {
    /// <summary>
    /// Represents a custom property value
    /// </summary>
    public struct Property {
        /// <summary>
        /// Raw String value of the propery
        /// </summary>
        public String Value;
        /// <summary>
        /// Value converted to a float, null if conversion failed
        /// </summary>
        public Single? AsSingle;
        /// <summary>
        /// Value converted to an int, null if conversion failed
        /// </summary>
        public Int32? AsInt32;
        /// <summary>
        /// Value converted to a boolean, null if conversion failed
        /// </summary>
        public Boolean? AsBoolean;

        /// <summary>
        /// Creates a property from a raw string value
        /// </summary>
        /// <param name="value">Value of the property</param>
        public static Property Create(String value) {
            Property p = new Property();
            p.Value = value;

            Boolean testBool;
            if (Boolean.TryParse(value, out testBool))
                p.AsBoolean = testBool;
            else
                p.AsBoolean = null;

            Single testSingle;
            if (Single.TryParse(value, out testSingle))
                p.AsSingle = testSingle;
            else
                p.AsSingle = null;

            Int32 testInt;
            if (Int32.TryParse(value, out testInt))
                p.AsInt32 = testInt;
            else
                p.AsInt32 = null;
            
            return p;
        }
    }
}
