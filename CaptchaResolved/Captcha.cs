using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace CaptchaResolved
{
    public class Captcha
    {
        /// <summary>
        /// Retona a resolução do captcha de acordo com o caminho.
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        public string solveCaptcha(string ImagePath)
        {
            string basepath = AppDomain.CurrentDomain.BaseDirectory + @"captcha\";
            var filepath = Path.Combine(basepath, ImagePath);
            
            Bitmap imagem = new Bitmap(filepath);
            imagem = imagem.Clone(new Rectangle(0, 0, imagem.Width, imagem.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Erosion erosion = new Erosion();
            Dilatation dilatation = new Dilatation();
            Invert inverter = new Invert();
            ColorFiltering cor = new ColorFiltering();
            cor.Blue = new AForge.IntRange(200, 255);
            cor.Red = new AForge.IntRange(200, 255);
            cor.Green = new AForge.IntRange(200, 255);
            Opening open = new Opening();
            BlobsFiltering bc = new BlobsFiltering();
            Closing close = new Closing();
            GaussianSharpen gs = new GaussianSharpen();
            ContrastCorrection cc = new ContrastCorrection();
            bc.MinHeight = 10;
            FiltersSequence seq = new FiltersSequence(inverter, gs, inverter, open, inverter, bc, inverter, open, cc, cor, bc, inverter);
            Bitmap newImage = new Bitmap(seq.Apply(imagem));
            
            
            using (var bitmap = new Bitmap(newImage))
            {
                bitmap.Save(basepath + @"captcha_copy.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            }

            Bitmap newImageLoaded = new Bitmap(filepath);

            string reconhecido = OCR(newImage);

            return reconhecido;
        }

        private string OCR(Bitmap b)
        {
            string res = "";

            using (var engine = new TesseractEngine(@"tessdata", "por", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "234567890abcdefghijklmnpqrstuvwxyz");
                engine.SetVariable("tessedit_unrej_any_wd", true);

                using (var page = engine.Process(b, PageSegMode.SingleWord))
                    res = page.GetText();
            }
            return res.Replace("\n", "");
        }

    }
}
