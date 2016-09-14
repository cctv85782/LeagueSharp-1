using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rethought_Irelia.IreliaV1.DamageCalculator
{
    internal interface IDamageCalculatorModule : IDamageCalculator
    {
        string Name { get; set; }

        int EstimatedAmountInOneCombo { get; }
    }
}
