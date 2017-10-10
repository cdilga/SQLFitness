using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLFitness
{
    public class Fitness
    {
        public double Value {get;}
        public Fitness(double val)
        {
            this.Value = val;
        }

        //Used https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/overloadable-operators
        public static double operator +(Fitness f1, Fitness f2)
        {
            return f1.Value + f2.Value;
        }

        public static double operator +(Fitness f1, double d1)
        {
            return f1.Value + d1;
        }

        public static double operator +(double d1, Fitness f1)
        {
            return f1.Value + d1;
        }

        public static double operator -(Fitness f1, Fitness f2)
        {
            return f1.Value - f2.Value;
        }

        public static double operator -(Fitness f1, double d1)
        {
            return f1.Value - d1;
        }

        public static double operator -(double d1, Fitness f1)
        {
            return f1.Value - d1;
        }

        public static double operator *(Fitness f1, Fitness f2)
        {
            return f1.Value * f2.Value;
        }

        public static double operator /(Fitness f1, Fitness f2)
        {
            return f1.Value / f2.Value;
        }

        public static double operator ^(Fitness f1, Fitness f2)
        {
            return f1.Value / f2.Value;
        }
    }
}
