using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Collections;
using Newtonsoft.Json;

namespace JsonParser
{
    public class ProductInfo
    {
        public string SKU { get; set; }
        public string UrunStok { get; set; }
        public ArrayList UrunFiyat { get; set; }
    }
    public class FiyatDerleme
    {
        public static WebClient webClient;
        public static int outOfIndex;
        public static bool isOk = true;
        public static List<Uri> Targets = new List<Uri>();
        public static List<string> ToPaths = new List<string>();
        public static int cct = 0;
        public static void Main()
        {
            outOfIndex = 0;
            Targets.Add(new Uri("http://pfd.premierfarnell.com/farnell/resellers/eCat_Farnell_Resellers_Inventory.zip"));
            Targets.Add(new Uri("http://pfd.premierfarnell.com/farnell/resellers/eCat_Standard_EE_prices.zip"));
            ToPaths.Add(@"C:\Downloads\1.zip");
            ToPaths.Add(@"C:\Downloads\2.zip");
            Validation();
            //Dosyaların her ikiside yok ise aşağıda ki if calısacak
            if (isOk)
            {
                foreach (var link in Targets)
                {
                    DownloadFiles(link, ToPaths[outOfIndex]);
                    outOfIndex++;
                }
                DoWork();
            }
            //Dosyalardan herhangi birisi yok ise aşağıda ki hatayı alacaksın
            else
            {
                Console.WriteLine("Check your files! Looking exist!");
            }
            Console.ReadKey();
        }
        static void Validation()
        {
            //Dosya varmı diye kontrol
            foreach (var item in ToPaths)
            {
                if (File.Exists(item))
                {
                    Console.WriteLine("File Exist!");
                    isOk = false;



                }
            }
        }
        static void DownloadFiles(Uri targetLink, string toPath)
        {
            //Dosyaları indiriyor
            try
            {
                webClient = new WebClient();
                webClient.DownloadFile(targetLink, toPath);
                ExtractFiles(toPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        static void ExtractFiles(string pathToExtract)
        {
            //rardan çıkartıp rar dosyasını siliyor
            System.IO.Compression.ZipFile.ExtractToDirectory(pathToExtract, @"C:\Downloads\");
            File.Delete(pathToExtract);
        }
        static void DoWork()
        {
            //Dosya okuma ve json oluşturma işlemleri
            cct = 0;
            string[] adet = File.ReadAllLines(@"C:\Downloads\eCat_Farnell_Resellers_Inventory.txt");
            string[] fiyat = File.ReadAllLines(@"C:\Downloads\eCat_Standard_EE_prices.csv");

            List<string> liste = new List<string>();
            while (cct < adet.Length)
            {
                //ArrayList y = new ArrayList();
                ArrayList x = new ArrayList();
                //ArrayList butunfiyatlar = new ArrayList();
                /*int count = 0;
                for (int i = 1; i < 15; i++)
                {
                    /string deneme1;
                    string s=fiyat[cct].ToString().Split('|')[i].ToString();
                    if (s != "0")
                    {
                        x.Add(s);
                        count++;
                    }
                    if (s!="0"&&count == 2)
                    {
                        int[] fiyatlar = new int[20];
                        y.Add(new ArrayList(x));
                        count = 0;
                        x.Clear();
                       
                            //Console.WriteLine(fiyat[i]+"");
                        /*for(int k=0; k<20; k++)
                        {
                            deneme1 = fiyat[i].ToString().Split('|')[k];
                            string numberStr = deneme1;
                            int number;
                            

                            bool isParsable = Int32.TryParse(numberStr, out number);
                            fiyatlar[k] = number;
                        }*/
                //butunfiyatlar.Add(fiyatlar);
                //}

            }

            var productinfo = new ProductInfo
            {
                SKU = adet[cct].ToString().Split('|')[0],
                UrunStok = adet[cct].ToString().Split('|')[1],
                UrunFiyat = new ArrayList(),
            };
            string a = JsonSerializer.Serialize(productinfo);
            string b = a.Trim(new Char[] { '"' });
            liste.Add(b);
            //Console.WriteLine(a.Trim(new Char[] { '"'}));

            Console.WriteLine(liste);
            cct++;
        }
            try
            {
                //Okunan dosyalar ttaşınıyor
                File.Move(@"C:\Downloads\eCat_Farnell_Resellers_Inventory.txt", @"C:\Downloads\Archive\eCat_Farnell_Resellers_Inventory.txt");
                File.Move(@"C:\Downloads\eCat_Standard_EE_prices.csv", @"C:\Downloads\Archive\eCat_Standard_EE_prices.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Operation completed but some file copy errors occured!", ex.ToString());
            }
        }
    }
}