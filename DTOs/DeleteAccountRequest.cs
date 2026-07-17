using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public record DeleteAccountRequest(
        string UserName,
        string Password
        );
}
