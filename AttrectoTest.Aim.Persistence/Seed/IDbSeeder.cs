using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttrectoTest.Aim.Persistence.Seed;

public interface IDbSeeder
{
    Task SeedAsync();
}
