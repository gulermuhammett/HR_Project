namespace HR_Project.UI.Models
{
    public class Password
    {
        private static readonly Random random = new Random();

        public static string GeneratePassword()
        {
            const string upperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            string password = "";

            // Büyük harf
            password += GetRandomCharacter(upperCaseLetters);

            // Küçük harf
            password += GetRandomCharacter(lowerCaseLetters);

            // Sayı
            password += GetRandomCharacter(numbers);

            // Özel karakter
            password += GetRandomCharacter(specialChars);

            // Diğer karakterler
            for (int i = 0; i < 2; i++)
            {
                string allChars = upperCaseLetters + lowerCaseLetters + numbers + specialChars;
                password += GetRandomCharacter(allChars);
            }

            // Karakterleri karıştır
            password = new string(password.ToCharArray().OrderBy(x => random.Next()).ToArray());

            return password;
        }

        private static char GetRandomCharacter(string characterSet)
        {
            int randomIndex = random.Next(characterSet.Length);
            return characterSet[randomIndex];
        }
    }
}
