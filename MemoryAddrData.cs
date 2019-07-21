using System;
using System.Collections.Generic;
using System.Text;

namespace MachineLanguageProgramming
{
    
    public class MemoryAddrData
    {
        public string dataVariable { get; set; }
        public int addressVariable { get; set; }

        public MemoryAddrData(string data, int address)
        {
            dataVariable = data;
            addressVariable = address;
        }

        public override string ToString()
        {
            return $" Address {addressVariable} Data {dataVariable}";
        }

    }
    
}
