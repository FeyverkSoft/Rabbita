using System;

namespace Rabbita.Core.FluentExtensions
{
    public interface IExceptionHandlerRegistry
    {
        IExceptionHandlerRegistry Register<T>() where T : IExceptionHandler;
        Type? GetHandlerFor<TE>(TE exception) where TE : notnull, Exception;
    }
}