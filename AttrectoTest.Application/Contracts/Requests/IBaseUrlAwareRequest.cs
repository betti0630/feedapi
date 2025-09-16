using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Application.Contracts.Requests;

public interface IBaseUrlAwareRequest
{
    string? BaseUrl { get; set; }
}
