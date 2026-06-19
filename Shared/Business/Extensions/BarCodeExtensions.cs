namespace BRICOMA.ECOMMERCE.Business.Extensions
{
    public static class BarCodeExtensions
    {
        /// <summary>
        /// Premier code-barres EAN-13 valide (635... = préfixe Maroc), check digit = 9.
        /// </summary>
        public const long FirstBarCode = 6350000000109;

        /// <summary>
        /// Calcule le check digit EAN-13 à partir des 12 premiers chiffres.
        /// </summary>
        public static int Ean13CheckDigit(string twelveDigits)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int d = twelveDigits[i] - '0';
                sum += (i % 2 == 0) ? d : d * 3;
            }
            return (10 - (sum % 10)) % 10;
        }

        /// <summary>
        /// À partir d'une valeur 13 chiffres, recalcule le check digit sur les 12 premiers
        /// et renvoie le code-barres EAN-13 final valide.
        /// </summary>
        public static string BuildEan13(long value)
        {
            var bar = value.ToString();
            var first12 = bar.Substring(0, 12);
            var check = Ean13CheckDigit(first12);
            return first12 + check;
        }
    }
}
