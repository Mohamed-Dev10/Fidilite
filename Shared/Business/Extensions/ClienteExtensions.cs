namespace BRICOMA.ECOMMERCE.Business.Extensions
{
    public static class ClienteExtensions
    {
        /// <summary>
        /// Incrémente un code client existant (MagasinCode + Alpha + 4 chiffres, ex: MAA0001).
        /// </summary>
        public static string GetClienteCode(string code)
        {
            var codeMagasin = code.Substring(0, 2);
            var codeAlphabic = code.Substring(2, 1);
            var codeNumber = int.Parse(code.Substring(code.Length - 4));

            string codeNumberToString;

            if (codeNumber == 9999) // fin de la plage 4 chiffres
            {
                codeNumberToString = "0001";

                codeAlphabic = GetNextAlphabic(codeAlphabic[0]).ToString().ToUpper();

                if (codeAlphabic == "C" || codeAlphabic == "S")
                    codeAlphabic = GetNextAlphabic(codeAlphabic[0]).ToString().ToUpper();
            }
            else
            {
                codeNumber += 1;
                codeNumberToString = codeNumber.ToString().PadLeft(4, '0');
            }

            return codeMagasin + codeAlphabic + codeNumberToString;
        }

        private static char GetNextAlphabic(char c) => (char)((int)c + 1);
    }
}
