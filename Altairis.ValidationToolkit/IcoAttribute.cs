﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class IcoAttribute : DataTypeAttribute {

        public IcoAttribute() : base("Ico") {
            this.ErrorMessage = "The field {0} must be valid IČO (Czech identification number of person).";
        }

        public override bool IsValid(object value) {
            // Empty values are valid - use RequiredAttribute instead
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s)) return true;

            // IČO is 8 digits, pad with zeroes when not
            if (!Regex.IsMatch(s, "^[0-9]{1,8}$")) return false;
            s = s.PadLeft(8, '0');

            // Calculate sum of digits
            var sum = 0;
            for (var i = 0; i < 7; i++) {
                sum += int.Parse(s[i].ToString()) * (8 - i);
            }

            // Verify checksum number
            var chs = 11 - (sum % 11);
            return chs.ToString().EndsWith(s.Substring(7), StringComparison.Ordinal);
        }
    }
}