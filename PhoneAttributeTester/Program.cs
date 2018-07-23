using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhoneAttributeTester
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> someNumbers = new List<string>
            {
                "0468160456",
                "+33468160456",
                  "+3346816045644"
            };
            foreach (var item in someNumbers)
            {
                Console.WriteLine(item + " is " + IsValid(item));
            }
            Console.ReadLine();
        }

        public static bool IsValid(object value)
        {
            if (value == null) return true;

            var unboxedValue = (string)value;//throws InvalidCastException

            //if it contains letters, return false
            if (unboxedValue.Any(c => char.IsLetter(c)))
                return false;

            //remove all non-digits like () - : whitespace etc... except '+' sign
            unboxedValue = new String(unboxedValue.Where(c => char.IsDigit(c) || c == '+').ToArray());

            if (unboxedValue.Length == 0)
                return false;

            if (unboxedValue[0] == '+')
            {
                //added for clarity: in case of leading +, just proceed with regex value test
            }
            else
            {
                // no country code entered, so must be local, we assume Belgium, and we assume with leading 0
                //zone number (03, 011, 015, etc...) as well as all mobile phone operator numbers (0468, etc...) start with a 0
                if (unboxedValue[0] != '0')
                    return false;
                // Feedback gehad, dat buitenlandse nummes ook moeten kunnen
                // Todo; Kijken of hier nog zaken moeten extra gevalideerd worden
                return true;
                //unboxedValue = unboxedValue.Remove(0, 1).Insert(0, "+32");
            }

            // //start regex testing: what you get at this stage is a guaranteed leading '+', as well as only digits
            //todo: complete list
            List<string> possibleRegexValues = new List<string>
            {
                @"^(\+)\b32(\d{7}|\d{8}|\d{9})$",//Belgium matches +32 + 1, 2 or 3 charachters + 6 charachters e.g.: +323123456 
                @"^(\+)\b31[1-9][0-9][1-9][0-9]{6}$",//e.g.: +31229567890 Nederland fixed number is https://www.murani.nl/blog/2015-09-28/nederlandse-reguliere-expressies/
                @"^(((\+)316){1}[1-9]{1}[0-9]{7})$",//e.g.: +31654322345 Dutch gsm --> note: always starts with 6, and following number is not 0
                @"^((?:\+33)[1-7](?:\d{2,8}))$",//e.g.: +33612345678  French number fixed and mobile --> fixed starts with +33+1to5, mobile +33+6or7
                @"^((?:\+49)(?:\d{2,12}))$",//Germany +49 + 2 to 12 numbers --> validation can be fine-tuned, but for Germany; it's complex. see https://en.wikipedia.org/wiki/Telephone_numbers_in_Germany
                @"^(\+352)\d{5,6}$", //Luxemburg fixed e.g.: +352791211 --> +352 + 5 or 6 characters
                @"^(\+352)6\d1(\d{6})$", //Luxemburg mobile e.g.: +352661123456 --> +352+6+x+1+xxxxxx

            };

            Regex r;
            Match m;

            foreach (var regexString in possibleRegexValues)
            {
                r = new Regex(regexString, RegexOptions.None);
                m = r.Match(unboxedValue);
                if (m.Success)
                    return true;
            }

            return false;
        }
    }

    

      
     
    
}
