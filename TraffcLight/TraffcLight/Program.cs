using System;
using System.Collections.Generic;
using System.Threading;

namespace TraffcLight
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TrafficLightManager.GetInstance().StartSystem();
        }
    }

    public enum Color
    {
        RED,
        YELLOW,
        GREEN
    }


    public class Light
    {
        Color Color;
        int Time;
        Boolean IsOn;

        public Light(Color color, int time)
        {
            Color = color;
            Time = time;
            IsOn = false;
        }

        public void TurnOn()
        {
            IsOn = true;
            Console.WriteLine($"{Color} light is On");
        }

        public void TurnOff()
        {
            IsOn = false;
            Console.WriteLine($"{Color} light is Off");
        }

        public int GetTime() => Time;
    }

    public interface ILightState
    {
        public void TurnOnLight();

        public void TurnOffLight();
    }

    public class RedLightState : ILightState
    {
        Light RedLight;
        Timer Timer;

        public RedLightState(Timer timer)
        {
            RedLight = new Light(Color.RED, 5000);
            Timer = timer;
        }

        public void TurnOnLight()
        {
            RedLight.TurnOn();
            //Timer.ResetTimer(RedLight.GetTime());
            TrafficLightManager.GetInstance().SetNextLight();
        }

        public void TurnOffLight()
        {
            RedLight.TurnOff();
            TrafficLightManager.GetInstance().GetCurrentGreenLight().SetCurrentState(new GreenLightState(Timer));
        }
    }

    public class YellowLightState : ILightState
    {
        Light YellowLight;
        Timer Timer;

        public YellowLightState(Timer timer)
        {
            YellowLight = new Light(Color.YELLOW, 3000);
            Timer = timer;
        }

        public void TurnOnLight()
        {
            YellowLight.TurnOn();
            Timer.ResetTimer(YellowLight.GetTime());
        }

        public void TurnOffLight()
        {
            YellowLight.TurnOff();
            TrafficLightManager.GetInstance().GetCurrentGreenLight().SetCurrentState(new RedLightState(Timer));
        }
    }

    public class GreenLightState : ILightState
    {
        Light GreenLight;
        Timer Timer;

        public GreenLightState(Timer timer)
        {
            GreenLight = new Light(Color.GREEN, 5000);
            Timer = timer;
        }

        public void TurnOnLight()
        {
            GreenLight.TurnOn();
            Timer.ResetTimer(GreenLight.GetTime());
        }

        public void TurnOffLight()
        {
            GreenLight.TurnOff();
            TrafficLightManager.GetInstance().GetCurrentGreenLight().SetCurrentState(new YellowLightState(Timer));
        }
    }

    public class Timer
    {
        int Time = 0;
        public void ResetTimer(int time)
        {
            Time = time;
            RunTimer();
        }

        private void RunTimer()
        {
            Thread.Sleep(Time);
            TrafficLightManager.GetInstance().GetCurrentGreenLight().GetCurrentState().TurnOffLight();
        }
    }

    public class TrafficLight
    {
        ILightState CurrentState = null;

        public string TrafficLightName;

        public TrafficLight(string name)
        {
            TrafficLightName = name;
            CurrentState = new RedLightState(new Timer());
        }

        public void StartTrafficLight()
        {
            CurrentState.TurnOnLight();
        }

        public void SetCurrentState(ILightState newState)
        {
            Console.WriteLine(TrafficLightName);

            CurrentState = newState;
            CurrentState.TurnOnLight();
        }

        public ILightState GetCurrentState() => CurrentState;
    }

    public class TrafficLightManager
    {
        private readonly Queue<TrafficLight> TrafficLights;

        private static TrafficLightManager Instance;

        private TrafficLight CurrentGreenLight;

        private TrafficLightManager()
        {
            TrafficLights = new Queue<TrafficLight>();
            TrafficLights.Enqueue(new TrafficLight("N"));
            TrafficLights.Enqueue(new TrafficLight("E"));
            TrafficLights.Enqueue(new TrafficLight("S"));
            TrafficLights.Enqueue(new TrafficLight("W"));
        }

        public static TrafficLightManager GetInstance()
        {
            if (Instance == null)
                Instance = new TrafficLightManager();

            return Instance;
        }

        public void StartSystem()
        {
            SetNextLight();
        }

        public void SetNextLight()
        {
            if (CurrentGreenLight != null)
                TrafficLights.Enqueue(CurrentGreenLight);

            CurrentGreenLight = TrafficLights.Dequeue();
            CurrentGreenLight.SetCurrentState(new GreenLightState(new Timer()));
        }

        public TrafficLight GetCurrentGreenLight() => CurrentGreenLight;
    }
}

