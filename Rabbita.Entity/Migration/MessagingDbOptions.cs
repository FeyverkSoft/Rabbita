namespace Rabbita.Entity.Migration;

public sealed class MessagingDbOptions
{
    public String ConnectionString { get; set; }

    public Int32 DbCommandTimeout { get; set; }
}