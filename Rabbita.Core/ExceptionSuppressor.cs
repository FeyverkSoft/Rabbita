namespace Rabbita.Core;

using Microsoft.Extensions.Logging;

public static class ExceptionSuppressor
{
    public static async Task<List<T>?> ExceptionSuppress<T>(Func<Task<List<T>>> func, ILogger logger)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            //если core система упала, то игнорим
            logger.Log(LogLevel.Error, e, e.Message);
            return null;
        }
    }

    public static async Task<T?> ExceptionSuppress<T>(Func<Task<T>> func, ILogger logger)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            //если core система упала, то игнорим
            logger.Log(LogLevel.Error, e, e.Message);
            return default;
        }
    }
    
    public static async Task ExceptionSuppress(Func<Task> func, ILogger logger)
    {
        try
        {
            await func();
        }
        catch (Exception e)
        {
            //если core система упала, то игнорим
            logger.Log(LogLevel.Error, e, e.Message);
        }
    }
}