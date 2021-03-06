﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SQLFitness
{
    public class Selection : Chromosome
    {
        private static string[] operators = { "<", ">", "=", "<>", ">=", "<=" };
    //private enum Operator { equal, notEqual, greaterThan, lessThan, greaterThanEqual, lessThanEqual }
    public string Operator { get; }
        public string Condition { get; }

        private readonly Func<string, List<object>> _validDataGetter;
        private readonly List<string> _validData;

        public Selection (List<string> validData, Func<string, List<object>>validDataGetter)
        {
            //Has an operator
            //Has a condition (might be a numerical value or something) - sourced from the database
            //Has a value (i.e. attribute name)
            _validData = validData;
            _validDataGetter = validDataGetter;
            this.Operator = operators.GetRandomValue();
            _field = validData.GetRandomValue();

            //TODO Generics warning - this is a point at which the object values are converted into stringy representations
            //Could need to be different in future, unsure of how this will play out
            this.Condition = validDataGetter(this.Field).GetRandomValue().ToString();
        }

        //This assumes that the same data that was valid at object creation time is still valid when a new object is instantiated.
        //This is done to reduce coupling though could be implemented via a getter as the second paramter is if this ever were to change.
        public override Chromosome Mutate() => new Selection(_validData, _validDataGetter);

        public override string ToString() => $"\"{this.Field}\" {this.Operator} \"{this.Condition}\"";
    }
}
