using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Idera.SQLsecure.UI.Console.ActiveDirectory
{
    ///<summary> packs an array of strings (type = string[]) to unmanaged memory
    /// also works for 64-bit environments
    ///</summary>
    public sealed class packLPArray
    {
        private IntPtr taskAlloc;
        private readonly int _length;
        private IntPtr[] _strings;

        public packLPArray(string[] theArray)
        {
            int sizeIntPtr = IntPtr.Size;

            int neededSize = 0;

            if (theArray != null)
            {
                this._length = theArray.Length;
                this._strings = new IntPtr[this._length];
                neededSize = this._length * sizeIntPtr;
                this.taskAlloc = Marshal.AllocHGlobal(neededSize);
                for (int cx = this._length - 1; cx >= 0; cx--)
                {
                    this._strings[cx] = Marshal.StringToHGlobalUni(theArray[cx]);
                    Marshal.WriteIntPtr(this.taskAlloc, cx * sizeIntPtr, this._strings[cx]);
                }
            }
        }

        /// <summary>
        /// retrieves array length
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        public IntPtr arrayPtr
        {
            get { return this.taskAlloc; }
        }

        ~packLPArray() // clean up the rub
        {
            if (taskAlloc != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.taskAlloc);
                int cx = this._length;
                while (cx-- != 0)
                    Marshal.FreeHGlobal(this._strings[cx]);
            }
        }
    }
}
