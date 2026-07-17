using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public record LoginAccountRequest
    (
        string UserName,
        string Password
    );
}
