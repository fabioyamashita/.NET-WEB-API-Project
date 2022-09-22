using System;

namespace SPX_WEBAPI.Domain.Dto
{
    public record SpxDto (DateTime? Date, decimal Close, decimal Open, decimal High, decimal Low);
}
