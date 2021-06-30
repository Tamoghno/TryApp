using System;
using System.Threading;

namespace TryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            double webLength, bulbSectionThickness;
            string input;
            string input1;

            //Time will be used for delay

            int millisecs = 2000;
            //User Inputs

            Console.Write("Enter the length of the web of bulb section (in mm): ");
            webLength = Convert.ToInt32(Console.ReadLine());
            Console.Write("Enter the thickness of bulb section (in mm): ");
            bulbSectionThickness = Convert.ToInt32(Console.ReadLine());

            //Output of Conversion from bulb to angled section
            double[] values = Converter(webLength, bulbSectionThickness);
            Console.WriteLine($"The values are \n Length of web (AS): {Math.Round(values[0],2)} mm," +
                $"\n Thickness of web: {Math.Round(values[3],2)} mm, \n Length of flange: {Math.Round(values[1],2)} mm, \n Thickness of flange: {Math.Round(values[2], 2)} mm, \n Cross-Sectional Area: {Math.Round(values[4],2)} mm^2");

            //Continuation prompt
            Console.WriteLine("Do you want to calculate area moment of inertia [y/n]?");
            input = Console.ReadLine();
            if (input == "y" || input == "yes")
            {  //Display moment and centroid

                double[] moments = momentCalculator(values[1], values[2], values[0], values[3]);
                Console.WriteLine($"Centroid of angled section: {moments[0]} mm, \nArea moment of inertia about centroid: {moments[1]}cm^4");
                Console.Write("To exit program, press y: ");
                input1 = Console.ReadLine();
                if(input1 == "y")
                {
                    Console.WriteLine("Exiting Program....");
                    Thread.Sleep(millisecs);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("You didn't type 'y'!\nExiting anyways....");
                    Thread.Sleep(millisecs);
                    Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Exiting Program....");
                Thread.Sleep(millisecs);
                Environment.Exit(0);
            }
            
            Console.ReadLine();
        }
       
        public static double[] Converter(double hw, double tw)  //Function to convert bulb sections params to angled ones
        {
            double angleLength, angleBreadth, webThickness, flangeThickness, alpha,csa;
            double[] values = new double[5];

            if(hw <= 120)
            {
                alpha = 1.1 + Math.Pow((120 - hw), 2) / 3000;//alpha values are as per IRS rules
            }
            else
            {
                alpha = 1;
            }
            //All formulae derived from Rulebook
            angleLength = hw + 2 - hw / 9.2; //hw
            angleBreadth = alpha * (tw - 2 + (hw / 6.7));//bf
            flangeThickness = (hw / 9.2) - 2;//tf
            webThickness = tw;//tw
            csa = (angleBreadth * flangeThickness) + (angleLength * webThickness);

            values[0] = angleLength;
            values[1] = angleBreadth;
            values[2] = flangeThickness;
            values[3] = webThickness;
            values[4] = csa;

            return values;
        }
        public static double[] momentCalculator(double bf, double tf, double hw, double tw)//Calculates 2nd moment about centroid
        {
            double A1, A2,Y1,Y2, Y_bar, Ixx1, Ixx2, Ixx;
            double[] moments = new double[2];

            A1 = bf * tf;
            A2 = hw * tw;
            Y1 = hw + (tf / 2);
            Y2 = hw / 2;

            Y_bar = (((A1 * Y1) + (A2 * Y2)) /(A1+A2));


            Ixx1 = ((tw * Math.Pow(hw,3))/12) + ((Y_bar - (hw / 2)) * (Y_bar - (hw / 2)) * (hw * tw));
            Ixx2 =  ((bf * Math.Pow(tf, 3)) /12) + ((hw + (tf / 2) - Y_bar) * (hw + (tf / 2) - Y_bar) * (bf * tf));

            Ixx = Ixx1 + Ixx2;

            moments[0] = Math.Round(Y_bar,2);
            moments[1] = Math.Round(Ixx/10000,2);

            return moments;
        }
    }
}

