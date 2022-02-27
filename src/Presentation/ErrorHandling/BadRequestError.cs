using System;
using System.Collections.Generic;
using System.Linq;

namespace Cps360.SyncWithCps.Presentation.ErrorHandling
{
    // 400 series erros
    public class BadRequestError
    {
        public string Key { get; set; }
        public IEnumerable<string> Values { get; set; }

        public BadRequestError(string key, string errorMessage)
        {
            Key = Char.ToLowerInvariant(key[0]) + key.Substring(1);

            Values = errorMessage?
                .Split(",")
                .Select(o => Char.ToLowerInvariant(o[0]) + o.Substring(1)) ??
                new string[0];

            if (Values.Any(o => o.Contains("is required")))
            {
                Values = Values.Select(o => {
                    if (o.Contains("is required"))
                        o = "isRequired";

                    return o;
                });
            }
        }
    }
}