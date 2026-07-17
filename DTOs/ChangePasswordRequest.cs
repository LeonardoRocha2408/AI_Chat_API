using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public record ChangePasswordRequest(
        string UserName,
        string Password,
        string NewPassword
        );
}
