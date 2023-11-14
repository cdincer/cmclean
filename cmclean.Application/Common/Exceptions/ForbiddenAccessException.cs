using System.Runtime.Serialization;

namespace cmclean.Application.Common.Exceptions;

[Serializable]
public class ForbiddenAccessException:Exception
{
    public ForbiddenAccessException(string message) : base(message) { }
    
    protected ForbiddenAccessException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {

    }
    
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        HelpLink = HelpLink?.ToLower();
        Source = Source?.ToUpperInvariant();

        base.GetObjectData(info, context);
    }
}