using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merp.Registry.CommandStack.Helpers
{
    public static class NationalIdentificationNumberHelper
    {
        private static readonly string Vocals = "AEIOU";
        private static readonly string Consonants = "BCDFGHJKLMNPQRSTVWXYZ";

        public static bool IsMatching(string nationalIdentificationNumber, string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                throw new ArgumentException("value must be specified", nameof(nationalIdentificationNumber));
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("value must be specified", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("value must be specified", nameof(lastName));
            }
            
            if(nationalIdentificationNumber.Length != 16)
            {
                return false;
            }

            var firstNameCode = CalculateFirstNameCode(firstName);
            var lastNameCode = CalculateLastNameCode(lastName);
            var validNationalIdentificationNumber = string.Format("{0}{1}{2}",lastNameCode, firstNameCode, nationalIdentificationNumber.Substring(6));

            return validNationalIdentificationNumber == nationalIdentificationNumber;
        }

        public static bool IsMatchingFirstName(string nationalIdentificationNumber, string firstName)
        {
            if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                throw new ArgumentException("value must be specified", nameof(nationalIdentificationNumber));
            }
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("value must be specified", nameof(firstName));
            }

            if (nationalIdentificationNumber.Length != 16)
            {
                return false;
            }

            var calculatedFirstNameCode = CalculateFirstNameCode(firstName);
            var inputFirstNameCode = nationalIdentificationNumber.Substring(3, 3);
            
            return calculatedFirstNameCode == inputFirstNameCode;
        }

        public static bool IsMatchingLastName(string nationalIdentificationNumber, string lastName)
        {
            if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                throw new ArgumentException("value must be specified", nameof(nationalIdentificationNumber));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("value must be specified", nameof(lastName));
            }

            if (nationalIdentificationNumber.Length != 16)
            {
                return false;
            }
            
            var calculatedLastNameCode = CalculateLastNameCode(lastName);
            var inputLastNameCode = nationalIdentificationNumber.Substring(0, 3);

            return calculatedLastNameCode == inputLastNameCode;
        }

        private static string CalculateFirstNameCode(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("value must be specified", nameof(firstName));
            }

            firstName = Normalize(firstName);
            string code = string.Empty;
            string cons = string.Empty;
            int i = 0;
            while ((cons.Length < 4) && (i < firstName.Length))
            {
                for (int j = 0; j < Consonants.Length; j++)
                {
                    if (firstName[i] == Consonants[j]) cons = cons + firstName[i];
                }
                i++;
            }

            code = (cons.Length > 3)
                ? cons[0].ToString() + cons[2].ToString() + cons[3].ToString()
                : code = cons;

            i = 0;
            while ((code.Length < 3) && (i < firstName.Length))
            {
                for (int j = 0; j < Vocals.Length; j++)
                {
                    if (firstName[i] == Vocals[j]) code += firstName[i];
                }
                i++;
            }
            
            return (code.Length < 3) ? code.PadRight(3, 'X') : code;
        }

        private static string CalculateLastNameCode(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("value must be specified", nameof(lastName));
            }

            lastName = Normalize(lastName);
            var code = string.Empty;
            int i = 0;

            while ((code.Length < 3) && (i < lastName.Length))
            {
                for (int j = 0; j < Consonants.Length; j++)
                {
                    if (lastName[i] == Consonants[j])
                    {
                        code += lastName[i];
                    }
                }
                i++;
            }
            i = 0;

            while (code.Length < 3 && i < lastName.Length)
            {
                for (int j = 0; j < Vocals.Length; j++)
                {
                    if (lastName[i] == Vocals[j]) code += lastName[i];
                }
                i++;
            }

            return (code.Length < 3) ? code.PadRight(3, 'X') : code;
        }

        private static string Normalize(string input)
        {
            input = input.Trim().ToUpper();
            string src = "ÀÈÉÌÒÙàèéìòù";
            string rep = "AEEIOUAEEIOU";
            for (int i = 0; i < src.Length; i++)
            {
                input = input.Replace(src[i], rep[i]);
            }
            return input;
        }
    }
}
