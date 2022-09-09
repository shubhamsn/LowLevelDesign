using NotificationService.Implementation;
using System;
using System.Collections.Generic;

namespace NotificationService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TextNotification text = new TextNotification(new SmsNotification());
            text.SendNotification();

            try
            {
                Car car = new Car();
                car.model = "maruti";
                ExceptionTesting exe =  new ExceptionTesting();
                exe.AddCar(car);

                car.model = "honda";

                Car checkCar = exe.getCar();

                Console.WriteLine(checkCar.model);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }

    public class ExceptionTesting 
    {
        private readonly List<Car> cars = new List<Car>();
        public void AddCar(Car car)
        {
            cars.Add(car);
        }

        public Car getCar() => cars[0];
    }

    public class Car
    {
        public string model { get; set; }
    }

}
