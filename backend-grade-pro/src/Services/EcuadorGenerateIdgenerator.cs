namespace backend_grade_pro.src.Services
{
    public class EcuadorGenerateIdgenerator
    {
        public static string Generate()
        {
            Random random = new Random();
            int provinceCode = random.Next(1, 25); // Código de provincia entre 01 y 24
            int thirdDigit = random.Next(0, 6); // Tercer dígito entre 0 y 5

            string id = provinceCode.ToString("D2") + thirdDigit.ToString();

            for (int i = 3; i < 9; i++)
            {
                id += random.Next(0, 10).ToString();
            }

            int[] coefficients = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int total = 0;

            for (int i = 0; i < coefficients.Length; i++)
            {
                int value = int.Parse(id[i].ToString()) * coefficients[i];
                total += value >= 10 ? value - 9 : value;
            }

            int checkDigit = total % 10 == 0 ? 0 : 10 - (total % 10);
            id += checkDigit.ToString();

            return id;
        }
    }
}
