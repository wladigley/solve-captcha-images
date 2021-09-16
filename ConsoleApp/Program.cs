using CaptchaResolved;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cap = new Captcha();
            //cap.solveCaptcha(@"C:\Users\wladigley.s.de.souza\source\repos\ConsoleApp\ConsoleApp\bin\Debug\captcha\captcha.png");
            cap.solveCaptcha(@"C:\Users\wladigley.s.de.souza\Downloads\MicrosoftTeams-image (9).png");
        }
    }
}
