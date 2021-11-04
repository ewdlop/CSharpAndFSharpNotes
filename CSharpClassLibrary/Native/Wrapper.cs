using System;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CSharpClassLibrary.Native
{
    class Wrapper
    {
        [DllImport("NativeClassLibrary.dll")]
        public static extern int Test(int value);

        //[DllImport(...)]
        private static extern unsafe int ExportedMethod(byte* pbData, int cbData);

        public unsafe int ManagedWrapper(Span<byte> data)
        {
            fixed (byte* pbData = &MemoryMarshal.GetReference(data))
            {
                int retVal = ExportedMethod(pbData, data.Length);

                /* error checking retVal goes here */

                return retVal;
            }
        }

        public unsafe int ManagedWrapperCheckNull(Span<byte> data)
        {
            fixed (byte* pbData = &MemoryMarshal.GetReference(data))
            {
                byte dummy = 0;
                int retVal = ExportedMethod((pbData != null) ? pbData : &dummy, data.Length);

                /* error checking retVal goes here */

                return retVal;
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate void OnCompletedCallback(IntPtr state, int result);

        //[DllImport(...)]
        private static extern unsafe int ExportedMethodAsync(byte* pbData, int cbData, IntPtr pState, IntPtr lpfnOnCompletedCallback);

        private static readonly IntPtr _callbackPtr = GetCompletionCallbackPointer();

        public unsafe Task<int> ManagedExportedMethodWrapperAsync(Memory<byte> data)
        {
            // setup
            var tcs = new TaskCompletionSource<int>();
            var state = new MyCompletedCallbackState
            {
                Tcs = tcs
            };
            var pState = (IntPtr)GCHandle.Alloc(state);

            var memoryHandle = data.Pin();
            state.MemoryHandle = memoryHandle;

            // make the call
            int result;
            try
            {
                result = ExportedMethodAsync((byte*)memoryHandle.Pointer, data.Length, pState, _callbackPtr);
            }
            catch
            {
                ((GCHandle)pState).Free(); // cleanup since callback won't be invoked
                memoryHandle.Dispose();
                throw;
            }

            //if (result != PENDING)
            //{
            //    // Operation completed synchronously; invoke callback manually
            //    // for result processing and cleanup.
            //    MyCompletedCallbackImplementation(pState, result);
            //}

            return tcs.Task;
        }

        private static IntPtr GetCompletionCallbackPointer()
        {
            OnCompletedCallback callback = MyCompletedCallbackImplementation;
            GCHandle.Alloc(callback); // keep alive for lifetime of application
            return Marshal.GetFunctionPointerForDelegate(callback);
        }

        private static void MyCompletedCallbackImplementation(IntPtr state, int result)
        {
            GCHandle handle = (GCHandle)state;
            var actualState = (MyCompletedCallbackState)(handle.Target);
            handle.Free();
            actualState.MemoryHandle.Dispose();

            /* error checking result goes here */

            //if (error)
            //{
            //    actualState.Tcs.SetException(...);
            //}
            //else
            //{
            //    actualState.Tcs.SetResult(result);


            //}

            actualState.Tcs.SetResult(result);
        }

        private class MyCompletedCallbackState
        {
            public TaskCompletionSource<int> Tcs;
            public MemoryHandle MemoryHandle;
        }
    }
}