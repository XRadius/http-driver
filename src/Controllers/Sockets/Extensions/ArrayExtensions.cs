namespace HttpDriver.Controllers.Sockets.Extensions
{
    public static class ArrayExtensions
    {
        #region Statics

        public static T[] Copy<T>(this T[] source, int offset, int count)
        {
            var result = new T[count];
            Array.Copy(source, offset, result, 0, count);
            return result;
        }

        #endregion
    }
}