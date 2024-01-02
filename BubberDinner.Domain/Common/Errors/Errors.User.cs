using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuberDinner.Domain.Common.Errors
{
    public static class Errors
    {
        public static class User
        {
            public static Error DulicateEmail => Error.Conflict(code: "User.DuplicateEmail", DescriptionAttribute: "Email is already in use.");
        }
    }
}
