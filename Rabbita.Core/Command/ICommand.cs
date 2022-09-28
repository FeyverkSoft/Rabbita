﻿namespace Rabbita.Core.Command;

using Message;

/// <summary>
/// Интерфейс маркер, указывающий то что объект является командой которая будет отправлена в шину
/// </summary>
public interface ICommand : IMessage { }