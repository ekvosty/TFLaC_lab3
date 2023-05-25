using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @interface
{
    internal enum AnalyzerState
    {
        NEW_OPERATOR,
        CREATE_INT,
        UPDATE_INT,
        SET_VARIABLE,
        NEED_DIGIT,
        NEED_OPERAND,
        NEED_DIGIT_FOR_DIVIDE
    }
}
