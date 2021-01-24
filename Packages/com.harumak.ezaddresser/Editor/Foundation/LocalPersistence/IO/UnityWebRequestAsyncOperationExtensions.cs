using UnityEngine.Networking;

namespace EZAddresser.Editor.Foundation.LocalPersistence.IO
{
    internal static class UnityWebRequestAsyncOperationExtensions
    {
        public static UnityWebRequestAsyncOperationAwaiter ConfigureAwait(
            this UnityWebRequestAsyncOperation asyncOperation)
        {
            return new UnityWebRequestAsyncOperationAwaiter(asyncOperation);
        }
    }
}